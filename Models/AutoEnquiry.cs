using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class AutoEnquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Display(Name = "Full Name")]
        public String FullName { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Display(Name = "Mobile Number")]
        public String MobileNumber { get; set; }

        [Display(Name = "Vehicle Make")]
        public String VehicleMake { get; set; }

        [Display(Name = "Vehicle Model")]
        public String VehicleModel { get; set; }

        [Display(Name = "Year ")]
        public String Year { get; set; }

        [Display(Name = "Engine Capacity")]
        public String EngineCapacity { get; set; }

        [Display(Name = "Interested in:")]
        public String ServiceTypeKey { get; set; }

        [NotMapped]
        public ServiceType ServiceType { get; set; }

        [Display(Name = "Message")]
        public String Message { get; set; }

        public DateTime DateTimeCreated { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [Display(Name = "Shcedule Date")]
        public Nullable<DateTime> AppointmentDate { get; set; }

        [Display(Name = "Shcedule Date")]
        [NotMapped]
        public DateTime DateComponent { get; set; }

        [Display(Name = "Time Of Day")]
        [NotMapped]
        public String TimeComponent { get; set; }

        public String ReferenceNumber { get; set; }
        public String UserId { get; set; }
        [NotMapped]
        public ApplicationUser User { get; set; }
        public String AssignedToUserId { get; set; }
        [NotMapped]
        public ApplicationUser AssignedToUser { get; set; } 
        public Boolean IsInspection { get; set; }
        public decimal AmountPaid { get; set; }

        public String AdminBy { get; set; }
        public String CollectedBy { get; set; }
    }

    public class ServiceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Desccription")]
        public string Desccription { get; set; }
    }
}