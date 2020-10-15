using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shodypati.Models
{
    public class CampaignProducts
    {
        public int Id { get; set; }

        [Required] public int ProductId { get; set; }

        public int DisplayOrder { get; set; }

        public Product Products { get; set; }

        public List<SelectListItem> AllProductsSelectList { get; set; }

        public List<string> AllProductsSelectListStr { get; set; }
    }
}