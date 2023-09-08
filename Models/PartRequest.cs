using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class PartRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Display(Name = "Part Name")]
        public String Name { get; set; }
        public String Email { get; set; }
        public String Make { get; set; }
        public String Model { get; set; }
        public Int32 Year { get; set; }

        [Display(Name= "Part Description")]
        public String Decription { get; set; }
        public DateTime Date_Requested { get; set; }
        public bool Admindecision { get; set; }
    }
}