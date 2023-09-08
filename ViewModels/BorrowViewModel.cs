using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.ViewModels
{
    [NotMapped]
    public class BorrowViewModel
    {
        public Library Library { get; set; }
        public BorrowedBooks BorrowedBooks { get; set; }

        public BookQrCodes BookQrCodes { get; set; }
    }

    public class InquiriesVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<ServiceType> ServiceType { get; set; }
        public AutoEnquiry AutoEnquiry { get; set; }
    }
}