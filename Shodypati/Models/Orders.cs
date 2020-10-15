using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public Guid? GuidId { get; set; }

        [Required] public Guid? CustomerId { get; set; }

        [Display(Name = "Customer Name")] public string CustomerName { get; set; }

        [Display(Name = "Customer Phone")] public string CustomerPhone { get; set; }

        //1
        [Display(Name = "Order Status")] public int? OrderStatusId { get; set; }

        [Display(Name = "Order Status")] public string OrderStatus { get; set; }

        //2
        [Display(Name = "Shipping Method")] public int? ShippingMethodId { get; set; }

        [Display(Name = "Shipping Method")] public string ShippingMethod { get; set; }

        [Display(Name = "Total Product Price")]
        public int? TotalProductPrice { get; set; }

        //public int? TotalPriceBeforeShipping { get; set; }
        [Display(Name = "Order Shipping Price")]
        public int? ShippingPrice { get; set; }

        [Display(Name = "Total Price After Shipping")]
        public int? TotalPriceAfterShipping { get; set; }

        [Display(Name = "Order Discount Price")]
        public int? OrderDiscountPrice { get; set; }

        [Display(Name = "Total Price After Shipping And Discount")]
        public int? TotalPriceAfterShippingAndDiscount { get; set; }

        //3
        [Display(Name = "Payment Method")] public int? PaymentMethodId { get; set; }

        [Display(Name = "Payment Method")] public string PaymentMethod { get; set; }

        public DateTime? OrderDate { get; set; }

        [Display(Name = "Shipping Date")] public DateTime? ShippingDate { get; set; }

        [Display(Name = "Billing Date")] public DateTime? BillingDate { get; set; }

        //Address
        [Display(Name = "Pick Up In Store")] public bool? PickUpInStore { get; set; }

        //4
        [Display(Name = "Payment Status")] public string PaymentStatus { get; set; }

        [Display(Name = "Payment Status")] public int? PaymentStatusId { get; set; }

        public bool? Active { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        //custom
        public List<OrdersProduct> OrdersProducts { get; set; }

        //Address
        public OrderAddress BillingAddress { get; set; }

        public OrderAddress ShippingAddress { get; set; }
    }

    public class OrderMobile
    {
        public int Id { get; set; }


        [Required] public Guid? CustomerId { get; set; }

        [Display(Name = "Customer Name")] public string CustomerName { get; set; }

        [Display(Name = "Customer Phone")] public string CustomerPhone { get; set; }

        //1
        [Display(Name = "Order Status")] public string OrderStatus { get; set; }

        //2
        [Display(Name = "Shipping Method")] public string ShippingMethod { get; set; }

        [Display(Name = "Total Product Price")]
        public int? TotalProductPrice { get; set; }

        [Display(Name = "Order Shipping Price")]
        public int? ShippingPrice { get; set; }

        [Display(Name = "Total Price After Shipping")]
        public int? TotalPriceAfterShipping { get; set; }

        [Display(Name = "Order Discount Price")]
        public int? OrderDiscountPrice { get; set; }

        [Display(Name = "Order Total Price")] public int? TotalPriceAfterShippingAndDiscount { get; set; }

        //3
        //  public int? PaymentMethodId { get; set; }
        [Display(Name = "Payment Method")] public string PaymentMethod { get; set; }

        public DateTime? OrderDate { get; set; }

        //  public DateTime? ShippingDate { get; set; }

        //  public DateTime? BillingDate { get; set; }

        //Address

        [Display(Name = "Pick Up In Store")] public bool? PickUpInStore { get; set; }


        //4
        [Display(Name = "Payment Status")] public string PaymentStatus { get; set; }
        //   public int? PaymentStatusId { get; set; }

        public bool? Active { get; set; }

        //   public DateTime? CreatedOnUtc { get; set; }

        //  public DateTime? UpdatedOnUtc { get; set; }

        //custom
        public List<OrdersProduct> OrdersProducts { get; set; }

        //Address
        public OrderAddressMobile BillingAddress { get; set; }

        public OrderAddressMobile ShippingAddress { get; set; }
    }


    public class OrdersProduct
    {
        public int Id { get; set; }

        [Required] public Guid? OrderGuidId { get; set; }

        [Required] public int ProductId { get; set; }

        [Required] public int? Quantity { get; set; }

        public int? ShippingCharge { get; set; }

        [Display(Name = "Main Product Image")] public string MainImagePath { get; set; }

        [Display(Name = "Product Name")] public string ProductName_English { get; set; }

        [Display(Name = "প্রোডাক্টের নাম")] public string ProductName_Bangla { get; set; }

        [Display(Name = "Unit Price")] public int? UnitPrice { get; set; }

        [Display(Name = "Offer Price")] public int? OfferPrice { get; set; }

        [Display(Name = "Total Price")] public int? TotalPrice { get; set; }

        //extra

        public int? Discount { get; set; }

        [Display(Name = "Total Price After Discount")]
        public int? TotalPriceAfterDiscount { get; set; }

        [Display(Name = "Brand")] public int? BrandId { get; set; }

        [Display(Name = "Merchant")] public int? MerchantId { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public string Note { get; set; }
    }

    public class OrdersProductMobile
    {
        public int Id { get; set; }

        [Required] public int ProductId { get; set; }

        [Required] public int? Quantity { get; set; }


        public string MainImagePath { get; set; }

        public string ProductName_English { get; set; }

        public string ProductName_Bangla { get; set; }

        public int? UnitPrice { get; set; }

        public int? OfferPrice { get; set; }
        public int? TotalPrice { get; set; }

        //extra

        public int? Discount { get; set; }
        public int? TotalPriceAfterDiscount { get; set; }

        public int? BrandId { get; set; }
        public int? MerchantId { get; set; }

        //public string Size { get; set; }
        //public string Color { get; set; }
        //public string Weight { get; set; }
        //public string Length { get; set; }
        //public string Width { get; set; }
        //public string Height { get; set; }

        //public string Note { get; set; }
    }

    public class OrderAddress
    {
        public int Id { get; set; }

        [Required] public Guid? OrderGuidId { get; set; }

        public bool? IsShipping { get; set; }

        public bool? IsBilling { get; set; }

        [Display(Name = "Address Line 1")] public string Address1 { get; set; }

        [Display(Name = "Address Line 2")] public string Address2 { get; set; }

        public string Division { get; set; }

        public string District { get; set; }

        public string Thana { get; set; }

        public string PostOffice { get; set; }

        public string PostCode { get; set; }

        public string MobileNumber { get; set; }
    }

    public class OrderAddressMobile
    {
        [Display(Name = "Address Line 1")] public string Address1 { get; set; }

        [Display(Name = "Address Line 2")] public string Address2 { get; set; }

        public string Division { get; set; }

        public string District { get; set; }

        public string Thana { get; set; }

        public string PostOffice { get; set; }

        public string PostCode { get; set; }

        public string MobileNumber { get; set; }
    }

    public class OrderPaymentMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public string InstructionsImageUrl { get; set; }

        public string RawDbImagePath { get; set; }

        public bool? Published { get; set; }
    }

    public class OrderPaymentStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ScaffoldColumn(false)] public bool? DefaultStatus { get; set; }
    }

    public class OrderShipping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }

    public class OrderStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ScaffoldColumn(false)] public bool? DefaultStatus { get; set; }
    }
}