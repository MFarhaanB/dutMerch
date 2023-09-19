using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BookStore.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using BookStore.Models.Product;
using BookStore.Models.Appointments;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }

        public string IdNumber { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Id", Id));
            userIdentity.AddClaim(new Claim("Name", Name));
            userIdentity.AddClaim(new Claim("Address", Address));
            userIdentity.AddClaim(new Claim("City", City));
            userIdentity.AddClaim(new Claim("State", State));
            userIdentity.AddClaim(new Claim("PostalCode", PostalCode));
            userIdentity.AddClaim(new Claim("Country", Country));
            userIdentity.AddClaim(new Claim("Phone", PhoneNo));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {

        }
        public static ApplicationDbContext Create() => new ApplicationDbContext();

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductCatergory> ProductCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryCatergory> LibraryCatergories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<BankingDetails> BankingDetails { get; set; }
        public DbSet<BankInfo> BankInfos { get; set; }
        public DbSet<OnlineSalesOrCODInfo> OnlineSalesInfos { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get;set; }
        public DbSet<RNRData> RNRDatas { get; set; }
        public DbSet<ReasonDrop> ReasonDrops { get; set; }
        public DbSet<ReturnAndRefund> ReturnAndRefunds { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<WishLists> WishLists { get; set; }
        public DbSet<WishListsProducts> WishListsProducts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Analytic> Analytics { get; set; }
        public DbSet<eBook> eBooks { get; set; }
        public DbSet<UsedCouponList> UsedCouponLists { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<RoleViewModel> RoleViewModels { get; set; }
        public DbSet<RequestBook> RequestBooks { get; set; }
        public DbSet<ChatBox> ChatBoxes { get; set; }
        public DbSet<BorrowedBooks> BorrowedBooks { get; set; }
        public DbSet<BookQrCodes> BookQrCodes { get; set; }
        public DbSet<AutoEnquiry> AutoEnquies { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ScheduledAppointment> ScheduledAppointments { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }
        public DbSet<AppointmentCheckList> AppointmentCheckLists { get; set; }
        public DbSet<VehicleInspection> VehicleInspections { get; set; }
        public DbSet<DebitOrCreditCard> DebitOrCreditCards { get; set; }
        public DbSet<source> sources { get; set; }
        public DbSet<RepairPart> RepairParts { get; set; }
        public DbSet<PartRequest> PartRequests { get; set; }
        public DbSet<SmsNotification> SmsNotifications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}