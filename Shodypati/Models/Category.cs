using Shodypati.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shodypati.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name_English { get; set; }

        [Display(Name = "ক্যাটাগরি নাম")]
        public string Name_Bangla { get; set; }

        public string RawDBImagePath { get; set; }

        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "বর্ণনা")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Category Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Parent Category")]
        public int? Parent1Id { get; set; }

        public string Parent1Name_English { get; set; }

        public string Parent1Name_Bangla { get; set; }

        [Display(Name = "Show On HomePage")]
        public bool? ShowOnHomePage { get; set; }

        [Display(Name = "Include In TopMenu")]
        public bool? IncludeInTopMenu { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        [Display(Name = "Published")]
        public bool? Published { get; set; }

        //custom
        [Display(Name = "Parent Category")]
        public IEnumerable<SelectListItem> Parent1IdList { get; set; }

        public int? SelectedParent1Id { get; set; }

        public List<SelectListItem> Categories      { get; set; }
        public List<SelectListItem> AllCategories   { get; set; }
        public List<SelectListItem> ChildCategories { get; set; }

        public List<Product> products { get; set; }
    }

    public class CategoryMobile
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name_English { get; set; }

        public string Name_Bangla { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }

        public string ImagePath { get; set; }


        //[Display(Name = "Parent Category")]
        //public int Parent1Id { get; set; }

        //public string Parent1Name_English { get; set; }

        //public string Parent1Name_Bangla { get; set; }

        //public bool? ShowOnHomePage { get; set; }

        //public bool? IncludeInTopMenu { get; set; }

        //public DateTime? CreatedOnUtc { get; set; }

        //public DateTime? UpdatedOnUtc { get; set; }

        //public bool? Published { get; set; }

        //public bool? Active { get; set; }

        //[Display(Name = "Parent Category")]
        //public IEnumerable<SelectListItem> Parent1IdList { get; set; }

        //public int SelectedParent1Id { get; set; }
        //public List<SelectListItem> Categories { get; set; }
        //public List<SelectListItem> AllCategories { get; set; }
        //public List<SelectListItem> ChildCategories { get; set; }

        public List<ProductMobile> products { get; set; }
    }
}