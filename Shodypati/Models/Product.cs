using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shodypati.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName_English { get; set; }

        [Display(Name = "প্রোডাক্টের নাম")] public string ProductName_Bangla { get; set; }

        [AllowHtml]
        [Display(Name = "Description")]
        public string Description_English { get; set; }

        [AllowHtml] [Display(Name = "বর্ণনা")] public string Description_Bangla { get; set; }

        [Display(Name = "Category")] public int? CategoryId { get; set; }

        [Display(Name = "Quantity In Stock")] public int? QuantityInStock { get; set; }

        [Display(Name = "Main Product Image")] public string MainImagePath { get; set; }

        public string RawDBImagePath { get; set; }

        //  public int AdditionalImageId { get; set; }

        [Display(Name = "Unit Price")] public int? UnitPrice { get; set; }

        [Display(Name = "Offer Price")] public int? OfferPrice { get; set; }

        [Display(Name = "Discount Percentage")]
        public decimal? DiscountPercentage { get; set; }

        [Display(Name = "Discount Amount")] public decimal? DiscountAmount { get; set; }

        [Display(Name = "Discount Start Date")]
        public DateTime? DiscountStartDateUtc { get; set; }

        [Display(Name = "Discount End Date")] public DateTime? DiscountEndDateUtc { get; set; }

        [Display(Name = "Discount Requires Coupon Code")]
        public bool? DiscountRequiresCouponCode { get; set; }

        [Display(Name = "Coupon Code")] public string CouponCode { get; set; }

        [Display(Name = "Display Order")] public int? DisplayOrder { get; set; }

        [Display(Name = "Is Hot")] public bool? IsHot { get; set; }

        //  public int OrderLimit { get; set; }
        [Display(Name = "Brand")] public int? BrandId { get; set; }

        [Display(Name = "Merchant")] public int? MerchantId { get; set; }

        [Display(Name = "Allow Customer Reviews")]
        public bool? AllowCustomerReviews { get; set; }

        [Display(Name = "Is ShipEnabled")] public bool? IsShipEnabled { get; set; }

        [Display(Name = "Is FreeShipping")] public bool? IsFreeShipping { get; set; }

        [Display(Name = "Ship Separately")] public bool? ShipSeparately { get; set; }

        [Display(Name = "Additional Shipping Charge")]
        public int? AdditionalShippingCharge { get; set; }

        [Display(Name = "Display Stock Availability")]
        public bool? DisplayStockAvailability { get; set; }

        [Display(Name = "Display Stock Quantity")]
        public bool? DisplayStockQuantity { get; set; }

        [Display(Name = "Order Minimum Quantity")]
        public int? OrderMinimumQuantity { get; set; }

        [Display(Name = "Order Maximum Quantity")]
        public int? OrderMaximumQuantity { get; set; }

        [Display(Name = "Not Returnable")] public bool? NotReturnable { get; set; }

        [Display(Name = "Disable Buy Button")] public bool? DisableBuyButton { get; set; }

        [Display(Name = "Available For Pre Order")]
        public bool? AvailableForPreOrder { get; set; }

        [Display(Name = "Pre Order Availability Start Date")]
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        [Display(Name = "Product Buy Cost")] public int? ProductCost { get; set; }

        [Display(Name = "Mark As New")] public bool? MarkAsNew { get; set; }

        [Display(Name = "Mark As New Start Date")]
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }

        [Display(Name = "Mark As New End Date")]
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        [Display(Name = "Available Start Date")]
        public DateTime? AvailableStartDateTimeUtc { get; set; }

        [Display(Name = "Available End Date")] public DateTime? AvailableEndDateTimeUtc { get; set; }

        [Display(Name = "Related Product")] public int? RelatedProductId { get; set; }

        public DateTime? CreatedOnUtc { get; set; }


        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Published { get; set; }


        //custom
        public List<ProductRelated> ProductRelateds { get; set; }


        public List<SelectListItem> AllCategoriesSelectList { get; set; }

        public string Parent1Name_English { get; set; }
        public string Parent1Name_Bangla { get; set; }

        public string MerchantName { get; set; }
        public string BrandName { get; set; }

        public List<SelectListItem> AllMerchantNameList { get; set; }

        public List<SelectListItem> AllBrandNameList { get; set; }

        //custom
        public List<ProductImage> ProductImages { get; set; }
    }


    public class ProductMobile
    {
        public int Id { get; set; }

        [Required] public string ProductName_English { get; set; }

        public string ProductName_Bangla { get; set; }

        public string Description_English { get; set; }

        public string Description_Bangla { get; set; }

        public int? CategoryId { get; set; }


        public int? QuantityInStock { get; set; }

        public string MainImagePath { get; set; }

        public string RawDBImagePath { get; set; }
        //  public int AdditionalImageId { get; set; }


        public int? UnitPrice { get; set; }

        public int? OfferPrice { get; set; }

        //public decimal? DiscountPercentage { get; set; }

        //public decimal? DiscountAmount { get; set; }

        //public DateTime? DiscountStartDateUtc { get; set; }

        //public DateTime? DiscountEndDateUtc { get; set; }

        //public bool? DiscountRequiresCouponCode { get; set; }

        //public string CouponCode { get; set; }

        public int? DisplayOrder { get; set; }

        //public bool? IsHot { get; set; }


        public int? BrandId { get; set; }

        public int? MerchantId { get; set; }

        //public bool? AllowCustomerReviews { get; set; }

        //public bool? IsShipEnabled { get; set; }

        //public bool? IsFreeShipping { get; set; }

        //public bool? ShipSeparately { get; set; }

        //public decimal? AdditionalShippingCharge { get; set; }

        //public bool? DisplayStockAvailability { get; set; }

        //public bool? DisplayStockQuantity { get; set; }

        public int? OrderMinimumQuantity { get; set; }

        public int? OrderMaximumQuantity { get; set; }

        //public bool? NotReturnable { get; set; }

        //public bool? DisableBuyButton { get; set; }

        //public bool? AvailableForPreOrder { get; set; }

        //public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        //public int ProductCost { get; set; }

        //public bool? MarkAsNew { get; set; }

        //public DateTime? MarkAsNewStartDateTimeUtc { get; set; }

        //public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        //public DateTime? AvailableStartDateTimeUtc { get; set; }

        //public DateTime? AvailableEndDateTimeUtc { get; set; }

        //public int RelatedProductId { get; set; }

        //public DateTime? CreatedOnUtc { get; set; }

        //public DateTime? UpdatedOnUtc { get; set; }

        //public bool? Published { get; set; }

        //public bool? Active { get; set; }

        //custom
        //public List<ProductRelated> ProductRelateds { get; set; }


        //public List<System.Web.Mvc.SelectListItem> AllCategoriesSelectList { get; set; }

        //public string Parent1Name_English { get; set; }
        //public string Parent1Name_Bangla { get; set; }

        //public string MerchantName { get; set; }
        //public string BrandName { get; set; }

        //public List<System.Web.Mvc.SelectListItem> AllMerchantNameList { get; set; }

        //public List<System.Web.Mvc.SelectListItem> AllBrandNameList { get; set; }
        //custom
        public List<ProductImage> ProductImages { get; set; }
    }


    public class ProductImage
    {
        public int Id { get; set; }

        [Required] public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }
    }


    public class ProductRelated
    {
        public int Id { get; set; }

        [Required] public int? ProductId { get; set; }

        public int? RelatedProductId { get; set; }
    }


    public class ProductReview
    {
        public int Id { get; set; }

        [Required] public int? ProdcutId { get; set; }

        public int? CustomerId { get; set; }

        public bool? IsApproved { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public string ReplyText { get; set; }

        public int Rating { get; set; }

        public DateTime? CreatedOnUtc { get; set; }
    }
}