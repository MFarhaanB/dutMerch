using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models.Product
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ManufactureKey { get; set; }
        [NotMapped]
        public Manufacture Manufacture { get; set; }
        public string VehicleModelKey { get; set; }
        [NotMapped]
        public VehicleModel VehicleModel { get; set; }
        public string VehicleTypelKey { get; set; }
        [NotMapped]
        public VehicleType VehicleType { get; set; }
    }
}