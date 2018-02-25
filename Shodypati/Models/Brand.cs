using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Brand Name")]
        public string Name_English { get; set; }

        [Display(Name = "ব্রান্ড নাম")]
        public string Name_Bangla { get; set; }

        public string Logo { get; set; }

        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Published { get; set; }


    }

}