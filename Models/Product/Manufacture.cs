using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BookStore.Models.Product
{
    public class Manufacture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string  Name { get; set; }
        public string  UrlLogo { get; set; }
    }
}