using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models.Appointments
{
    public class InspectionViewModel
    {
        public AutoEnquiry AutoEnquiry { get; set; }
        public VehicleInspection VehicleInspection { get; set; }    
        public CheckListItem CheckListItem { get; set; }
        public ApplicationUser Inspector { get; set; }
        public List<AppointmentCheckList> appointmentCheckLists { get; set; }
        public List<RepairPart> RepairParts { get; set; }
        public InspectionPayment inspectionPayment { get; set; }
    }

    public class InspectionPayment
    {
        [Display(Name ="Inspection Charges")]
        public decimal InspectionCharge { get; set; }
        public decimal AmountPaid { get; set; }

        [Display(Name = "Repair Charges")]
        public decimal RepairCharge { get; set; }

        [Display(Name = "Labour Cost")]
        public decimal labourCharge { get; set; }

        [Display(Name = "Additional Cost")]
        public decimal Additional { get; set; }

        [Display(Name = "Repairs")]
        public bool RepairsPayment { get; set; }

        [Display(Name = "Inspection")]
        public bool InspectionPay { get; set; }

        [Display(Name = "Payment Method")]
        public string Paymentmethod { get; set; }

        public string InspDsp => this.InspectionCharge.ToString("C") ?? null;
        public string reprpDsp => this.RepairCharge.ToString("C") ?? null;
        public string lbcDsp => this.labourCharge.ToString("C") ?? null;
        public string addDsp => this.Additional.ToString("C") ?? null;
        public string TotalCost => this.InspectionCharge.ToString("C") ?? null;
        public string TotalAmount => this.AmountPaid.ToString("C") ?? null;
    }
}