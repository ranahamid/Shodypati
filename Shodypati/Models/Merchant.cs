using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class Merchant
    {
        public int Id { get; set; }

        public string URL { get; set; }

        [Required]
        [Display(Name = "Merchant Name")]
        public string Name_English { get; set; }

        [Display(Name = "ব্যবসায়ীর নাম")]
        public string Name_Bangla { get; set; }

        public string Logo { get; set; }

        [Display(Name = "Type Of Goods")]
        public string TypeOfGoods { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Discount Available")]
        public bool? DiscountAvailable { get; set; }

        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Published { get; set; }

    }

    public class MerchantMobile
    {
        public int Id { get; set; }

        public string URL { get; set; }
        [Required]
        public string Name_English { get; set; }

        public string Name_Bangla { get; set; }

        public string Logo { get; set; }

        //public string TypeOfGoods { get; set; }

        // public string Notes { get; set; }

        // public bool? DiscountAvailable { get; set; }

        //public int DisplayOrder { get; set; }

        //public DateTime? CreatedOnUtc { get; set; }

        //public DateTime? UpdatedOnUtc { get; set; }

        //public bool? Published { get; set; }

        //public bool? Active { get; set; }

        public List<ProductMobile> products { get; set; }
    }


}