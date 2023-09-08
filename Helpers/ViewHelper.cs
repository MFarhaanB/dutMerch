using BookStore.Models;
using BookStore.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static iTextSharp.text.pdf.AcroFields;

namespace BookStore.Helpers
{
    public class ViewHelper
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public static string getVehicleModel(Guid vehicleId) => db.VehicleModels.FirstOrDefault(a => a.Id == vehicleId).Name;
        public static string getVManufacture(Guid vehicleId) => db.Manufactures.FirstOrDefault(a => a.Id == vehicleId).Name;


        public static string FormatAppointmentDate(Nullable<DateTime> AppointmentDate)
            => AppointmentDate == null ? "Pending Confirmation"
            : String.Format("{0} at {1}", Convert.ToDateTime(AppointmentDate).ToLongDateString(), Convert.ToDateTime(AppointmentDate).ToLongTimeString());

        public static Dictionary<string, List<string>> VehicleCheckListItemsDb()
        {
            using (var _context = new ApplicationDbContext())
            {
                List<CheckListItem> checkListItems = _context.CheckListItems.ToList();

                // Group the checkListItems by the 'Group' property
                var groupedItems = checkListItems.GroupBy(item => item.Group)
                                                .ToDictionary(group => group.Key, group => group.Select(item => $"{item.Name}|{item.Description}|{item.Id}").ToList());

                return groupedItems;
            }
        }

        public static string AppointmentItemChecked(Guid Id, List<AppointmentCheckList> appointmentCheckLists)
        {
            return (appointmentCheckLists.FirstOrDefault(a => a.CheckListItemKey == Id) == null) ? "" : " checked=\"checked\"";
        }

        public static SelectList GetPaymentMethos()
        {
            return new SelectList(new List<SelectListItem>
                    {
                        new SelectListItem { Selected = false, Value = "c_yoco_payment_c", Text = "Debit/Credit Card"},
                        //new SelectListItem { Selected = false, Value = "c_payfast_c", Text = "PayFast"},
                    }, "Value", "Text");
        }
    }
}