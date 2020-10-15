using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shodypati.Models
{
    public class Banner
    {
        public int Id { get; set; }

        public Guid GuidId { get; set; }

        [Required] public string Name { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Published { get; set; }

        public List<BannerImage> BannerImages { get; set; }
    }

    public class BannerMobile
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public List<BannerImageMobile> BannerImages { get; set; }
    }


    public class BannerSelectList
    {
        public List<SelectListItem> AllBanerItems { get; set; }

        [Display(Name = "Banner Name")] public string SelectedBanner { get; set; }
    }

    public class BannerImage
    {
        public int Id { get; set; }

        [Required] public Guid BannerGuidId { get; set; }

        public string URL { get; set; }

        public int? MerchantId { get; set; }

        public int? CategoryId { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }
    }


    public class BannerImageMobile
    {
        public string URL { get; set; }

        public int? MerchantId { get; set; }

        public int? CategoryId { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }
    }
}