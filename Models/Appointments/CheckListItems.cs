using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace BookStore.Models.Appointments
{
    public class CheckListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(Order = 2)]
        public String Group { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public  Decimal  Charge { get; set; }

        
    }

    public class AppointmentCheckList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CheckListItemKey { get; set; }
        public Guid AutoEnquiryKey { get; set; }

        [NotMapped]
        public CheckListItem CheckListItem { get; set; }

        [NotMapped]
        public AutoEnquiry AutoEnquiry { get; set; }
    }


    public class RepairPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AutoEnquiryKey { get; set; }
        public int ProductKey { get; set; }

        [NotMapped]
        public AutoEnquiry AutoEnquiry { get; set; }

        [NotMapped]
        public Products Product { get; set; }
    }


    public class VehicleInspection
    {
        //https://chat.openai.com/share/cbb9f5f8-3158-46ad-97f3-20a37b10f97f
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AutoEnquiryKey { get; set; }
        [NotMapped]
        public AutoEnquiry AutoEnquiry { get; set; }
        public String AssignedToUserId { get; set; }
        [NotMapped]
        public ApplicationUser AssignedToUser => String.IsNullOrEmpty(this.AssignedToUserId) ? null : new ApplicationDbContext().Users.Find(this.AssignedToUserId);

        [Display(Name = "VIN")]
        public String VehicleIdentificationNumber { get; set; }

        [Display(Name = "Plate Number")]
        public String PlateNumber { get; set; }
        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }

        [Display(Name = "Visual Inspection Details")]
        public String VisualInspectionDetails { get; set; }

        [Display(Name = "OBD diagnostic codes and descriptions (if applicable)")]
        public String OBDdiagnosticCodeAndDescriptions { get; set; } //if applicable 

        [Display(Name = "Emissions test results (if applicable)")]
        public String Emissions { get; set; } //if applicable 

        [Display(Name = "Test Drive Notes")]
        public String TestDriveNotes { get; set; }

        [Display(Name = "Remarks and Recommendations")]
        public String RemarksAndRecommendations { get; set; }

        [Display(Name = "Signature and Authorization")]
        public String SignatureAndAuthorization { get; set; }

        [Display(Name = "Additional Informationn")]
        public String AdditionalInformation { get; set; }
    }
}