using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Products
    {
        //Keys

        [Key]
        public int ProductId { get; set; } //Primary Key..
       
        public string ProductName { get; set; }
        public string ProductAuthor { get; set; }
        public int ProductStock { get; set; }
        public string ProductDescription { get; set; }
        public byte[] ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public bool isActive { get; set; }
        public int? ProductCatergoryId { get; set; }
        [ForeignKey("ProductCatergoryId")]
        public ProductCatergory ProductCatergory  { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }

        public string vManufactureKey { get; set; }
        public string vModelKey { get; set; }
        public string vTypeKey { get; set; }


    }
}