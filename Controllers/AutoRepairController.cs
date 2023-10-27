using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ViewModels;
using BookStore.Helpers;
using BookStore.Models.Appointments;
using System.Collections.ObjectModel;
using System.Web.Helpers;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using static BookStore.Controllers.SalesController;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace BookStore.Controllers
{
    [Authorize]
    public class AutoRepairController : Controller
    {
        private ApplicationDbContext _context;
        public AutoRepairController()
        {
            _context = new ApplicationDbContext();
        }

        public AutoRepairController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Inqueries()
        {
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.ToList(), "Id", "Name");
            ViewBag.message = null;
            return View(new InquiriesVM
            {
                ApplicationUser = new BaseHelper().CurrentUser(User) ?? new ApplicationUser(),
                ServiceType = _context.ServiceTypes.ToList(),
                AutoEnquiry = new AutoEnquiry()
            });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inqueries(InquiriesVM model)
        {
            AutoEnquiry autoEnquiry = model.AutoEnquiry;
            autoEnquiry.DateTimeCreated = DateTime.Now;
            autoEnquiry.StatusId = _context.Statuses.FirstOrDefault(a => a.Key == "c_enquiry_pending").Id;
          
            autoEnquiry.UserId = new BaseHelper().CurrentUser(User).Id;
            autoEnquiry.ReferenceNumber = GenerateInqueryRef(String.Format("INQ{0}", new Random().Next(100000, 1000000).ToString()));
            AutoRepairHelper.SaveEnquiry(_context, autoEnquiry);
            TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Customer submited an enquery for his/her vehicle", $"Customer Message: {autoEnquiry.Message}, the enquery is awaiting review by technician.");
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.ToList(), "Id", "Name");
            ViewBag.message = "Your quiery was successfully submited, we'll be in touch as soon as possible!";
            return RedirectToAction("Index");   
        }

        public String GenerateInqueryRef(String random) 
            => _ = (_context.AutoEnquies.FirstOrDefault(a => a.ReferenceNumber == random) == null) 
            ? random : GenerateInqueryRef(String.Format("INQ{0}", new Random().Next(100000, 1000000).ToString()));


        [Authorize]
        public ActionResult Appointment(Guid Id)
        {
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.ToList(), "Id", "Name");
            return View(_context.AutoEnquies.Find(Id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Appointment(ScheduledAppointment collection)
        {
            try
            {
                collection.AppointmentDate = collection.DateComponent.Date + Convert.ToDateTime(collection.TimeComponent).TimeOfDay;
                collection.Conducted = false;
                collection.Approved = (_context.ScheduledAppointments.FirstOrDefault(a => a.AppointmentDate == collection.AppointmentDate) == null ? true : false);
                collection.UserId = new BaseHelper().CurrentUser(User).Id;
                _context.ScheduledAppointments.Add(collection);
                _context.SaveChanges();
                return RedirectToAction("MySchedued", "AutoRepair");
            }
            catch { throw; }
        }


        [Authorize]
        public ActionResult MySchedued()
        {
            var userid = new BaseHelper().CurrentUser(User).Id;
            return View(_context.ScheduledAppointments.Where(c => c.UserId == userid).ToList());
        }


        [Authorize]
        public ActionResult VehicleCheckIns()
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            return View(_context.AutoEnquies.Include(a => a.Status)
                .Where(a => (a.AssignedToUserId == applicationUser.Id)).ToList());
        }


        public ActionResult Index()
        {
            Guid id = Guid.Parse("cffcd6e4-4d3d-417a-9896-557e2c274cc3");
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            return View(_context.AutoEnquies.Where(a => a.Email == applicationUser.Email || a.UserId == applicationUser.Id).ToList());
        }

        [Authorize]
        public ActionResult CustomerInqueries()
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            return View(_context.AutoEnquies.Include(a=>a.Status).Where(a => a.Status.Key== "c_enquiry_pending" && (a.AssignedToUserId == null || a.AssignedToUserId == applicationUser.Id)).ToList());
        }

        public ActionResult CustomerInqueryDetails(Guid Id)
        {
            
            var vm = new InquiriesVM
            {
                ApplicationUser = new BaseHelper().CurrentUser(User) ?? new ApplicationUser(),
                ServiceType = _context.ServiceTypes.ToList(),
                AutoEnquiry = _context.AutoEnquies.Find(Id)
            };
            Id = Guid.Parse(vm.AutoEnquiry.ServiceTypeKey);
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.Where(a => a.Id == Id).ToList(), "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public ActionResult CustomerInqueryDetails(InquiriesVM m, Guid Id)
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
            autoEnquiry.StatusId = _context.Statuses.FirstOrDefault(a => a.Key == "a_awaiting_appointment").Id;
            autoEnquiry.AppointmentDate = m.AutoEnquiry.DateComponent.Date + Convert.ToDateTime(m.AutoEnquiry.TimeComponent).TimeOfDay;
            autoEnquiry.AssignedToUserId = applicationUser.Id;
            _context.Entry(autoEnquiry).State = EntityState.Modified;
            _context.SaveChanges();

            TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Appointment Approved", $"Appointment has been approved for {ViewHelper.FormatAppointmentDate(autoEnquiry.AppointmentDate)}, customer required to avail him/herself at least 10 minutes prior.");

            // send out notification to user
            String body = $"Your appointment has been approved for {ViewHelper.FormatAppointmentDate(autoEnquiry.AppointmentDate)}, please avail yourself at least 10 minutes prior. This <strong>{autoEnquiry.ReferenceNumber}</strong> is your appointment ref, you are required to produce it on your arrival for verification purposes.";
            new Email().SendEmail("Hendrix Auto: Appointment Confirmation", autoEnquiry.FullName, body, autoEnquiry.Email, "e_appointment_confirm");

            return RedirectToAction("CustomerInqueries");
        }



        public ActionResult VehicleInspection(Guid Id)
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            InspectionViewModel inspectionViewModel = new InspectionViewModel();
            inspectionViewModel.AutoEnquiry =  _context.AutoEnquies.Find(Id);
            inspectionViewModel.Inspector = applicationUser;
            inspectionViewModel.VehicleInspection = new VehicleInspection { InspectionDate = DateTime.Now };
            Id = Guid.Parse(inspectionViewModel.AutoEnquiry.ServiceTypeKey);
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.Where(a => a.Id == Id).ToList(), "Id", "Name");
            return View(inspectionViewModel);
        }


        [HttpPost]
        [Authorize]
        public ActionResult VehicleInspection(InspectionViewModel collection, Guid Id, String[] SelectedCheckListItem)
        {
            AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            VehicleInspection vehicleInspection = collection.VehicleInspection;
            vehicleInspection.AutoEnquiryKey = autoEnquiry.Id;
            vehicleInspection.AssignedToUserId = applicationUser.Id;
            _context.VehicleInspections.Add(vehicleInspection);

            autoEnquiry.StatusId = _context.Statuses.FirstOrDefault(a => a.Key == "a_await_repair_acceptance").Id;
            _context.Entry(autoEnquiry).State = EntityState.Modified;
            decimal cost = 0;
            String breakdown = String.Empty;
            foreach (var item in SelectedCheckListItem)
            {
                AppointmentCheckList appointmentCheckList = new AppointmentCheckList
                {
                    CheckListItemKey = Guid.Parse(item),
                    AutoEnquiryKey = Id
                };
                _context.AppointmentCheckLists.Add(appointmentCheckList);
                CheckListItem checkListItem = _context.CheckListItems.Find(appointmentCheckList.CheckListItemKey);
                if(breakdown == String.Empty) 
                    breakdown += $"- {checkListItem.Charge.ToString("C")} : {checkListItem.Name}";
                else
                    breakdown += $"<br/>- {checkListItem.Charge.ToString("C")} : {checkListItem.Name}";
                cost += checkListItem.Charge;

            }
            _context.SaveChanges();

            

            //send email to customer with cost or anyother things
            String body = $"This email confirms your vehicle has been reviewed by the Mechanic or Technician. For ref no <strong>{autoEnquiry.ReferenceNumber}</strong> you are required to indicate if you'd like to continue with the repairs, please login to our website to confirm. <br/><br/><strong>BREAKDOWN</strong><br/>{breakdown}<br/><br/> The total of your checklist is {cost.ToString("C")}";
            new Email().SendEmail("Hendrix Auto: Repair Confirmation", autoEnquiry.FullName, body, autoEnquiry.Email, "e_repair_confirmation");

            breakdown = breakdown.Replace("<br/>", "\n");
            var timeline = $"For ref no {autoEnquiry.ReferenceNumber} customer is required to indicate if he/she'd like to continue with the repairs.\n\nBREAKDOWN\n\n{breakdown}\n The total of your checklist is {cost.ToString("C")}";
            TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Mechanic or Technician Inspection Completed", timeline);

            return RedirectToAction("VehicleCheckIns");
        }


        public ActionResult InspectionDetails(Guid Id)
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            InspectionViewModel inspectionViewModel = new InspectionViewModel();
            inspectionViewModel.AutoEnquiry = _context.AutoEnquies.Find(Id);
            inspectionViewModel.Inspector = applicationUser;
            inspectionViewModel.VehicleInspection = _context.VehicleInspections.FirstOrDefault(a => a.AutoEnquiryKey.Equals(Id));
            List<AppointmentCheckList> appointmentCheckLists = _context.AppointmentCheckLists.Where(a => a.AutoEnquiryKey.Equals(Id)).ToList();
            foreach(var  appointmentCheckList in appointmentCheckLists)
                appointmentCheckLists.FirstOrDefault(a => a.Id == appointmentCheckList.Id).CheckListItem = _context.CheckListItems.Find(appointmentCheckList.CheckListItemKey);
            inspectionViewModel.appointmentCheckLists = appointmentCheckLists;

            inspectionViewModel.inspectionPayment = new InspectionPayment
            {
                Additional = 0,
                InspectionCharge = 2500,
                labourCharge = 2500,
                RepairCharge = appointmentCheckLists.Sum(a => a.CheckListItem.Charge)
            };

            Id = Guid.Parse(inspectionViewModel.AutoEnquiry.ServiceTypeKey);
            ViewBag.ServiceTypes = new SelectList(_context.ServiceTypes.Where(a => a.Id == Id).ToList(), "Id", "Name");
            return View(inspectionViewModel);
        }

        [HttpPost]
        public ActionResult Payment(DebitOrCreditCard m, String p, Guid Id, String sid, String ssid, Decimal total, Boolean PaymentForAll) 
        {
            try
            {
                List<AppointmentCheckList> appointmentCheckLists = _context.AppointmentCheckLists.Where(a => a.AutoEnquiryKey.Equals(Id)).ToList();
                source source = m.source;
                source.sid = sid;
                _context.sources.Add(source);
                _context.SaveChanges();

                DebitOrCreditCard debitOrCreditCard = m;
                debitOrCreditCard.source = null;
                debitOrCreditCard.sourceKey = source.Id;
                debitOrCreditCard.AutoEnquiryKey = Id;
                debitOrCreditCard.ssid = ssid;
                _context.DebitOrCreditCards.Add(debitOrCreditCard);
                _context.SaveChanges();

                AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
                autoEnquiry.AmountPaid = total;
                autoEnquiry.IsInspection = PaymentForAll;
                autoEnquiry.StatusId = (PaymentForAll) ? _context.Statuses.FirstOrDefault(a => a.Key == "a_await_repair_by_m").Id
                                        : _context.Statuses.FirstOrDefault(a => a.Key == "c_no_repair_checkout").Id;
                _context.Entry(autoEnquiry).State = EntityState.Modified;
                _context.SaveChanges();

                if (PaymentForAll) TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Cusomer Payment With Repairs", "Customer has chosen to proceed with repairs, the vehicle is now sent to mechanic.");
                else TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Cusomer Payment With No Repairs", "Customer has chosen not to proceed with repairs, the vehicle will be avaiting collection at wthe warehouse.");
                
                

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }


        public InspectionViewModel InspectionViewModel(Guid Id, InspectionViewModel inspectionViewModel)
        {
            ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
            inspectionViewModel.AutoEnquiry = _context.AutoEnquies.Find(Id);
            inspectionViewModel.Inspector = applicationUser;
            inspectionViewModel.VehicleInspection = _context.VehicleInspections.FirstOrDefault(a => a.AutoEnquiryKey.Equals(Id));
            List<AppointmentCheckList> appointmentCheckLists = _context.AppointmentCheckLists.Where(a => a.AutoEnquiryKey.Equals(Id)).ToList();
            foreach (var appointmentCheckList in appointmentCheckLists)
                appointmentCheckLists.FirstOrDefault(a => a.Id == 
                appointmentCheckList.Id).CheckListItem = _context.CheckListItems.Find(appointmentCheckList.CheckListItemKey);
            inspectionViewModel.appointmentCheckLists = appointmentCheckLists;
            Id = Guid.Parse(inspectionViewModel.AutoEnquiry.ServiceTypeKey);
            inspectionViewModel.AutoEnquiry.ServiceType = _context.ServiceTypes.Find(Id);
            inspectionViewModel.AutoEnquiry.Status = _context.Statuses.Find(inspectionViewModel.AutoEnquiry.StatusId);
            inspectionViewModel.AutoEnquiry.AssignedToUser = _context.Users.Find(inspectionViewModel.AutoEnquiry.AssignedToUserId);
            inspectionViewModel.AutoEnquiry.User = _context.Users.Find(inspectionViewModel.AutoEnquiry.UserId);
            inspectionViewModel.RepairParts = _context.RepairParts.Where(a => a.AutoEnquiryKey == inspectionViewModel.AutoEnquiry.Id).ToList();
            for (int i = 0; i < inspectionViewModel.RepairParts.Count(); i++)
            {
                RepairPart part = inspectionViewModel.RepairParts[i];
                inspectionViewModel.RepairParts[i].Product = _context.Products.Find(part.ProductKey);
            }

            inspectionViewModel.inspectionPayment = new InspectionPayment
            {
                Additional = 0,
                InspectionCharge = 2500,
                labourCharge = 2500,
                RepairCharge = appointmentCheckLists.Sum(a => a.CheckListItem.Charge),
                AmountPaid = inspectionViewModel.AutoEnquiry.AmountPaid
            };
            return inspectionViewModel;
        }


        public ActionResult AutoRepairVehicle(Guid Id)
        {
            InspectionViewModel inspectionViewModel = InspectionViewModel(Id, new InspectionViewModel());
            return View(inspectionViewModel);
        }

        public ActionResult Parts(Guid Id, string sortOrder, string currentFilter, string searchString, string manufacture, string vModel, string bType, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.manufacture = new SelectList(_context.Manufactures, "Id", "Name");
            ViewBag.vModel = new SelectList(_context.VehicleModels, "Id", "Name");
            ViewBag.bType = new SelectList(_context.VehicleTypes, "Id", "Name");
            ViewBag.AutoEnquiryId = Id;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in _context.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(manufacture) || !String.IsNullOrEmpty(vModel) || !String.IsNullOrEmpty(bType))
            {
                if (!String.IsNullOrEmpty(manufacture))
                {
                    items = items.Where(s => s.vManufactureKey.Contains(manufacture));
                }
                if (!String.IsNullOrEmpty(vModel))
                {
                    items = items.Where(s => s.vModelKey.Contains(vModel));
                }
                if (!String.IsNullOrEmpty(bType))
                {
                    items = items.Where(s => s.vTypeKey.Contains(bType));
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddPartToRepair(Guid Id, Int64 PartId)
        {
            try
            {
                _context.RepairParts.Add(new RepairPart { AutoEnquiryKey = Id, ProductKey = (int)PartId });
                _context.SaveChanges();

                InspectionViewModel inspectionViewModel = InspectionViewModel(Id, new InspectionViewModel());
                return RedirectToAction("AutoRepairVehicle", new { Id = Id });
            }
            catch (Exception)
            {
                InspectionViewModel inspectionViewModel = InspectionViewModel(Id, new InspectionViewModel());
                return RedirectToAction("AutoRepairVehicle", new { Id = Id });
            }
           
        }
        public ActionResult DeletePart(Guid Id)
        {
            try
            {
                RepairPart _part = _context.RepairParts.Find(Id);
                _context.Entry(_part).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return Json(true);
        }

        [HttpPost]
        public ActionResult AutoRepairSubmit(Guid Id)
        {
            try
            {
                AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
                autoEnquiry.StatusId = _context.Statuses.FirstOrDefault(a => a.Key == "c_awaiting_checkout").Id;
                _context.Entry(autoEnquiry).State = EntityState.Modified;
                _context.SaveChanges();

                //send email to customer with cost or anyother things
                String body = $"This email confirms your vehicle has been repaired by the Mechanic or Technician. For ref no <strong>{autoEnquiry.ReferenceNumber}</strong>. Please collect your vehicle at our facility as soon as possible.";
                new Email().SendEmail("Hendrix Auto: Repair Complete", autoEnquiry.FullName, body, autoEnquiry.Email, "e_auto_repair_complete");

                body = $"The vehicle has been repaired by the Mechanic or Technician. For ref no {autoEnquiry.ReferenceNumber}. The customer is required to collect his/her vehicle at our facility as soon as possible.";
                TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Vehicle Repairs Complete", body);

                return RedirectToAction("VehicleCheckIns");
            }
            catch (Exception)
            {
                return RedirectToAction("VehicleCheckIns");
            }
            
        }

        public ActionResult AutoRepairCheckOut(Guid Id)
        {
            InspectionViewModel inspectionViewModel = InspectionViewModel(Id, new InspectionViewModel());
            return View(inspectionViewModel);
        }

        [HttpPost]
        public ActionResult CheckOutVehicle(InspectionViewModel model, Guid Id)
        {
            AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
            autoEnquiry.CollectedBy = model.AutoEnquiry.CollectedBy;
            autoEnquiry.AdminBy = model.AutoEnquiry.AdminBy;
            autoEnquiry.StatusId = _context.Statuses.FirstOrDefault(a => a.Key == "c_auto_checkedout_or_colleced").Id;
            _context.Entry(autoEnquiry).State = EntityState.Modified;
            _context.SaveChanges();

            String body = $"This email confirms your vehicle has been repaired by the Mechanic or Technician. For ref no <strong>{autoEnquiry.ReferenceNumber}</strong>. Please collect your vehicle at our facility as soon as possible.";
            new Email().SendEmail("Hendrix Auto: Auto Mobile Checked Out/Collected", autoEnquiry.FullName, body, autoEnquiry.Email, "e_auto_repair_complete");

            body = $"The vehicle has been been collected by customer - {model.AutoEnquiry.CollectedBy}, at our facility with assistance from - {model.AutoEnquiry.AdminBy}. The collection process was validated with an OTP to confirm the ownership of the vehicle.";
            TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "Auto Mobile Checked Out/Collected", body);

            return RedirectToAction("VehicleCheckIns");
        }


        public async Task<ActionResult> SentCollectionOTP(Guid Id)
        {
            var random = new Random().Next(100000, 1000000).ToString();
            AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
            autoEnquiry.OneTimePin = random;
            _context.Entry(autoEnquiry).State= EntityState.Modified;
            _context.SaveChanges();

            String body = $"For ref no <strong>{autoEnquiry.ReferenceNumber}</strong>. Here is one time pin: {random}";
            new Email().SendEmail("Hendrix Auto: One Time Pin", body, autoEnquiry.Email);
            body = body.Replace("<strong>", "").Replace("</strong>", "");
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == autoEnquiry.Email);

            SmsNotification smsNotification = new SmsNotification();
            SMSViewModel sms = new SMSViewModel();
            sms.messages = new Message[]
            {
                new Message
            {
                to =String.Format("+27{0}", autoEnquiry.MobileNumber.Substring(Math.Max(0, autoEnquiry.MobileNumber.Length - 9))),
                source = "php",
                body = $"Hendrix Auto. One time pin {random}",
                custom_string = $"Hendrix Auto. One time pin {random}"
            }
            };

            body = $"An OTP has been requested and it has been sent out to customers mobile number on {DateTime.Now}";
            TimelineHelper.CreateTimelineEvent(autoEnquiry.Id, "One Time Pin Requested", body);

            string username = ConfigurationManager.AppSettings["sms_username"];
            string api_key = ConfigurationManager.AppSettings["sms_api_key"];

            // Concatenate username and password with a colon
            string credentials = String.Format("{0}:{1}", username, api_key);

            // Convert the concatenated string to a byte array
            byte[] credentialsBytes = Encoding.UTF8.GetBytes(credentials);

            // Encode the byte array as a Base64 string
            string base64Credentials = Convert.ToBase64String(credentialsBytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rest.clicksend.com/v3/sms/send");
            request.Headers.Add("Authorization", $"Basic {base64Credentials}");
            var content = new StringContent(JsonSerializer.Serialize(sms), null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
            smsNotification.Request = JsonSerializer.Serialize(sms);
            smsNotification.Response = await response.Content.ReadAsStringAsync();
            smsNotification.CreatedDateTime = DateTime.Now;
            
            _context.SmsNotifications.Add(smsNotification);
            await _context.SaveChangesAsync();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateOTP(Guid Id, String OneTimePin)
        {
            AutoEnquiry autoEnquiry = _context.AutoEnquies.Find(Id);
            if(autoEnquiry.OneTimePin == OneTimePin)
            {
                autoEnquiry.OneTimePin = null;
                _context.Entry(autoEnquiry).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}