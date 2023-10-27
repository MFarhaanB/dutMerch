using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Timeline
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public String Action { get; set; }
        public String Description { get; set; }
        public DateTime DateTime { get; set; }

        public Guid AutoEnquiryId { get; set; }
        [ForeignKey("AutoEnquiryId")]
        public AutoEnquiry AutoEnquiry { get; set; }
    }
}