using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class SmsNotification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int DeliveryId { get; set; }
        public String Driver { get; set; }
        public String Request { get; set; }
        public String Response { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}