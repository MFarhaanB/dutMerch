using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Asn1.Cms;
using BookStore.Helpers;

namespace BookStore.Models.Appointments
{
    public class ScheduledAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public String UserId { get; set; }
        [NotMapped]
        public ApplicationUser User { get; set; }
        public String ServicedByUserId { get; set; }
        [NotMapped]
        public ApplicationUser ServicedByUser
        {
            get
            {
                return (String.IsNullOrEmpty(this.ServicedByUserId) ? new ApplicationUser() : new BaseHelper().GetUserById(this.ServicedByUserId));
            }
        }

        [Display(Name ="Service Type")]
        public Guid ServiceTypeKey { get; set; }
        [NotMapped]
        public ServiceType ServiceType { get; set; }
        [Display(Name = "Shcedule Date")]
        public DateTime AppointmentDate { get; set; }
        [Display(Name = "Additional Information")]
        public string AdditionalInformation  { get; set; }
        public bool Approved { get; set; }
        public bool Conducted { get; set; }
        public string Outcome { get; set; }

        [Display(Name = "Shcedule Date")]
        [NotMapped]
        public DateTime DateComponent { get; set; }

        [Display(Name = "Time Of Day")]
        [NotMapped]
        public String TimeComponent { get; set; }
    }

    public class source
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public String brand { get; set; }
        public String expiryMonth { get; set; }
        public String expiryYear { get; set; }
        public String fingerprint { get; set; }
        public String sid { get; set; }
        public String maskedCard { get; set; }
        public String _object { get; set; }
    }

    public class DebitOrCreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid sourceKey { get; set; }
        public Guid AutoEnquiryKey { get; set; }
        public String ssid { get; set; }
        public String metadata { get; set; }
        public String sourceType { get; set; }
        public String status { get; set; }
        [NotMapped]
        public source source { get; set; }

        [NotMapped]
        public AutoEnquiry AutoEnquiry { get; set; }
    }

}