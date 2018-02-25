using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class CampaignProducts
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        
        public int DisplayOrder { get; set; }

        public Product Products { get; set; }

        public List<System.Web.Mvc.SelectListItem> AllProductsSelectList { get; set; }

        public List<string> AllProductsSelectListStr { get; set; }
    }
}