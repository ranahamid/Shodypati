using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shodypati.DAL
{

    [ExceptionHandlerAttribute]
    public class OrderDataAccessRepository : BaseController, IOrderAccessRepository<Orders, int>
    {
        public OrderDataAccessRepository()
        {
        }


        public IEnumerable<OrderMobile> Get()
        {
            var entities = Db.OrdersTbls.Select(x => new OrderMobile()
            {
                Id = x.Id,

                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName,
                CustomerPhone = x.CustomerPhone,
                OrderStatus = x.OrderStatus,
                TotalProductPrice = x.TotalProductPrice,
                ShippingPrice = x.ShippingPrice,
                TotalPriceAfterShipping = x.TotalPriceAfterShipping,
                OrderDiscountPrice = x.OrderDiscountPrice,
                TotalPriceAfterShippingAndDiscount = x.TotalPriceAfterShippingAndDiscount,

                PaymentMethod = x.PaymentMethod,
                OrderDate = x.OrderDate,
                PickUpInStore = x.PickUpInStore,

                PaymentStatus = x.PaymentStatus,

                ShippingMethod = x.ShippingMethod, // AUTO

                Active = x.Active,

                OrdersProducts = Db.OrdersProductTbls.Where(y => y.OrderGuidId == x.GuidId).Select(y => new OrdersProduct()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    ShippingCharge = y.ShippingCharge,
                    Quantity = y.Quantity,
                    MainImagePath = HttpUtility.UrlPathEncode(baseUrl + y.MainImagePath),
                    ProductName_English = y.ProductName_English,
                    ProductName_Bangla = y.ProductName_Bangla,
                    UnitPrice = y.UnitPrice,
                    OfferPrice = y.OfferPrice,
                    TotalPrice = y.TotalPrice,
                    Discount = y.Discount,
                    TotalPriceAfterDiscount = y.TotalPriceAfterDiscount,

                    BrandId = y.BrandId,
                    MerchantId = y.MerchantId

                }).OrderByDescending(y => y.Id).ToList(),


            }).OrderByDescending(y => y.Id).ToList();

            return entities;
        }

        public OrderMobile Get(int id)
        {
            OrderMobile entity = Db.OrdersTbls.Where(x => x.Id == id).Select(x => new OrderMobile()
            {
                Id = x.Id,

                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName,
                CustomerPhone = x.CustomerPhone,
                //1
                OrderStatus = x.OrderStatus,
                // OrderStatusId                                           = x.OrderStatusId                                     ,

                TotalProductPrice = x.TotalProductPrice,
                //  TotalPriceBeforeShipping                                = x.TotalPriceBeforeShipping                          ,
                ShippingPrice = x.ShippingPrice,
                TotalPriceAfterShipping = x.TotalPriceAfterShipping,
                OrderDiscountPrice = x.OrderDiscountPrice,
                TotalPriceAfterShippingAndDiscount = x.TotalPriceAfterShippingAndDiscount,

                //2
                PaymentMethod = x.PaymentMethod,
                //  PaymentMethodId                                           = x.PaymentMethodId                                 ,

                OrderDate = x.OrderDate,
                //  ShippingDate                                            = x.ShippingDate                                      ,
                //  BillingDate                                             = x.BillingDate                                       ,

                PickUpInStore = x.PickUpInStore,
                //3
                //  PaymentStatusId                                         = x.PaymentStatusId                                   ,             
                PaymentStatus = x.PaymentStatus,
                //4
                // ShippingMethodId                                         = x.ShippingMethodId                                  , //AUTO
                ShippingMethod = x.ShippingMethod, // AUTO

                Active = x.Active,

                OrdersProducts = Db.OrdersProductTbls.Where(y => y.OrderGuidId == x.GuidId).Select(y => new OrdersProduct()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Quantity = y.Quantity,
                    MainImagePath = HttpUtility.UrlPathEncode(baseUrl + y.MainImagePath),
                    ProductName_English = y.ProductName_English,
                    ProductName_Bangla = y.ProductName_Bangla,
                    UnitPrice = y.UnitPrice,
                    OfferPrice = y.OfferPrice,
                    TotalPrice = y.TotalPrice,
                    Discount = y.Discount,
                    ShippingCharge = y.ShippingCharge,

                    TotalPriceAfterDiscount = y.TotalPriceAfterDiscount,

                    //extra
                    BrandId = y.BrandId,
                    MerchantId = y.MerchantId,
                    Size = y.Size,
                    Color = y.Color,
                    Weight = y.Weight,
                    Length = y.Length,
                    Width = y.Width,
                }).ToList(),

                BillingAddress = Db.OrdersAddressTbls.Where(y => y.OrderGuidId == x.GuidId && y.IsBilling == true).Select(y => new OrderAddressMobile()
                {
                    Address1 = y.Address1,
                    Address2 = y.Address2,
                    Division = y.Division,
                    District = y.District,
                    Thana = y.Thana,
                    PostOffice = y.PostOffice,
                    PostCode = y.PostCode,
                    MobileNumber = y.MobileNumber,
                }).SingleOrDefault(),

                ShippingAddress = Db.OrdersAddressTbls.Where(y => y.OrderGuidId == x.GuidId && y.IsShipping == true).Select(y => new OrderAddressMobile()
                {
                    Address1 = y.Address1,
                    Address2 = y.Address2,
                    Division = y.Division,
                    District = y.District,
                    Thana = y.Thana,
                    PostOffice = y.PostOffice,
                    PostCode = y.PostCode,
                    MobileNumber = y.MobileNumber,
                }).SingleOrDefault(),

                //   CreatedOnUtc = x.CreatedOnUtc,
                //   UpdatedOnUtc = x.UpdatedOnUtc

            }).SingleOrDefault();
            return entity;
        }



        public OrderMobile Post(Orders entity, ApplicationUserManager UserManager)
        {
            Guid orderGuidId = Guid.NewGuid();
            //Add all prdoucts
            int? totalProductPrice = 0;
            string productInfo = string.Empty;

            if (entity.OrdersProducts != null && entity.OrdersProducts.Count > 0)
            {
                foreach (var product in entity.OrdersProducts)
                {
                    //product details
                    var productQuery = (from x in Db.ProductTbls
                                        where x.Id == product.ProductId
                                        select x).SingleOrDefault();

                    if (product.Quantity == null || product.Quantity == 0)
                    {
                        product.Quantity = 1;
                    }

                    int? productPrice;
                    if (productQuery.OfferPrice == 0 || productQuery.OfferPrice == null)
                    {
                        if (productQuery.UnitPrice == null)
                        {
                            productQuery.UnitPrice = 0;
                        }

                        productPrice = product.Quantity * productQuery.UnitPrice;
                    }
                    else
                    {
                        productPrice = product.Quantity * productQuery.OfferPrice;
                    }




                    if (product.Discount == null || product.Discount < 0)
                    {
                        product.Discount = 0;
                    }



                    int shippingCharge = productQuery.AdditionalShippingCharge ?? 0;
                    int totalPriceAll = productPrice + shippingCharge - product.Discount ?? 0;
                    totalProductPrice += totalPriceAll;

                    //Email template
                    productInfo = productInfo +
                                  "ProductName:    " + productQuery.ProductName_English + "\n" +
                                  "ProductId:       " + product.ProductId + "\n" +
                                  "Quantity:        " + product.Quantity + "\n" +
                                  "UnitPrice:       " + productQuery.UnitPrice + "\n" +
                                  "productPrice:    " + productPrice + "\n" +
                                  "ShippingCharge:  " + shippingCharge + "\n" +
                                  "TotalPrice :     " + totalPriceAll + "\n" + "\n";


                    Db.OrdersProductTbls.InsertOnSubmit(new OrdersProductTbl
                    {
                        OrderGuidId = orderGuidId,            //AUTO
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        ProductName_English = productQuery.ProductName_English,
                        ProductName_Bangla = productQuery.ProductName_Bangla,
                        UnitPrice = productQuery.UnitPrice,
                        OfferPrice = productQuery.OfferPrice,
                        TotalPrice = productPrice,             //AUTO
                        Discount = product.Discount,

                        ShippingCharge = shippingCharge,

                        TotalPriceAfterDiscount = totalPriceAll, // //AUTO

                        MainImagePath = productQuery.MainImagePath,
                        //extra para            
                        BrandId = productQuery.BrandId,
                        MerchantId = productQuery.MerchantId,
                        Size = productQuery.Size,
                        Color = productQuery.Color,
                        Weight = productQuery.Weight,
                        Length = productQuery.Length,
                        Width = productQuery.Width,
                        Height = productQuery.Height,
                    });
                }
            }

            //Order Status 
            entity.OrderStatusId = 1; //need to implement set default from db
            var query = (from x in Db.OrderStatusTbls
                         where x.Id == entity.OrderStatusId
                         select x).SingleOrDefault();

            string orderStatusStr = string.Empty;

            if (query != null)
            {
                orderStatusStr = query.Name;
            }

            //Payment Type 
            if (entity.PaymentMethodId == null)
            {
                entity.PaymentMethodId = 1;
            }
            var query2 = (from x in Db.OrderPaymentMethodTbls
                          where x.Id == entity.PaymentMethodId
                          select x).SingleOrDefault();

            string paymentMethodStr = string.Empty;

            if (query2 != null)
            {
                paymentMethodStr = query2.Name;
            }

            //payment status
            entity.PaymentStatusId = 1; //need to implement set default from db

            var query3 = (from x in Db.OrderPaymentStatusTbls
                          where x.Id == entity.PaymentStatusId
                          select x).SingleOrDefault();

            string paymentStatuStr = string.Empty;

            if (query3 != null)
            {
                paymentStatuStr = query3.Name;
            }

            //Payment Type 
            if (entity.ShippingMethodId == null)
            {
                entity.ShippingMethodId = 1;
            }
            var query4 = (from x in Db.OrderShippingTbls
                          where x.Id == entity.ShippingMethodId
                          select x).SingleOrDefault();

            string shippingMethodStr = string.Empty;

            if (query4 != null)
            {
                shippingMethodStr = query4.Name;
            }

            //User template
            var customerDetails = GetUserInfo(UserManager, entity.CustomerId);
            var customer =
                          "Name:        " + customerDetails.Name + "\n" +
                          "Address:     " + customerDetails.Address + "\n" +
                          "Email:       " + customerDetails.Email.Trim() + "\n" +
                          "PhoneNumber: " + customerDetails.PhoneNumber + "\n" + "\n";

            var totalPriceOrder = entity.ShippingPrice + totalProductPrice - entity.OrderDiscountPrice;
            Db.OrdersTbls.InsertOnSubmit(new OrdersTbl
            {
                GuidId = orderGuidId, //AUTO           
                CustomerId = entity.CustomerId,
                CustomerName = customerDetails.Name,
                CustomerPhone = customerDetails.PhoneNumber,
                //1
                OrderStatus = orderStatusStr, //AUTO
                OrderStatusId = entity.OrderStatusId, //AUTO

                TotalProductPrice = totalProductPrice, //AUTO
                                                       // TotalPriceBeforeShipping                                = TotalProductPrice                                        , //AUTO
                ShippingPrice = entity.ShippingPrice,
                TotalPriceAfterShipping = entity.ShippingPrice + totalProductPrice, //AUTO
                OrderDiscountPrice = entity.OrderDiscountPrice,
                TotalPriceAfterShippingAndDiscount = totalPriceOrder, //AUTO
                //2
                PaymentMethod = paymentMethodStr,  //AUTO
                PaymentMethodId = entity.PaymentMethodId,

                OrderDate = DateTime.Now, //AUTO
                ShippingDate = entity.ShippingDate, //Do Not Need To Post
                BillingDate = entity.BillingDate, //Do Not Need To Post
                PickUpInStore = entity.PickUpInStore,
                //3
                PaymentStatusId = entity.PaymentStatusId, //AUTO
                PaymentStatus = paymentStatuStr, // AUTO

                //4
                ShippingMethodId = entity.ShippingMethodId, //AUTO
                ShippingMethod = shippingMethodStr, // AUTO

                Active = true, //AUTO
                CreatedOnUtc = DateTime.Now, //AUTO
                UpdatedOnUtc = DateTime.Now, //AUTO
            });



            //Add billing address
            if (entity.BillingAddress != null)
            {
                Db.OrdersAddressTbls.InsertOnSubmit(new OrdersAddressTbl
                {
                    OrderGuidId = orderGuidId,
                    Address1 = entity.BillingAddress.Address1,
                    Address2 = entity.BillingAddress.Address2,
                    Division = entity.BillingAddress.Division,
                    District = entity.BillingAddress.District,
                    Thana = entity.BillingAddress.Thana,
                    PostOffice = entity.BillingAddress.PostOffice,
                    PostCode = entity.BillingAddress.PostCode,
                    MobileNumber = entity.BillingAddress.MobileNumber,
                    IsBilling = true,
                    IsShipping = false,
                });
            }

            //Add billing address
            if (entity.ShippingAddress != null)
            {
                Db.OrdersAddressTbls.InsertOnSubmit(new OrdersAddressTbl
                {
                    OrderGuidId = orderGuidId,
                    Address1 = entity.ShippingAddress.Address1,
                    Address2 = entity.ShippingAddress.Address2,
                    Division = entity.ShippingAddress.Division,
                    District = entity.ShippingAddress.District,
                    Thana = entity.ShippingAddress.Thana,
                    PostOffice = entity.ShippingAddress.PostOffice,
                    PostCode = entity.ShippingAddress.PostCode,
                    MobileNumber = entity.ShippingAddress.MobileNumber,
                    IsShipping = true,
                    IsBilling = false,
                });
            }
            try
            {
                Db.SubmitChanges();


            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }

            int orderId = GetOrderIdFromGuidId(orderGuidId);

            //web
            var webOrderDetailsUrl = baseUrl + "orders/Details/" + orderId;

            //Order template
            var orderDetails =
                "Order ID                                           :" + orderId + "\n" +
                "Total Product Price (without order shipping price) :" + totalProductPrice + "\n" +
                "Order ShippingPrice                                :" + entity.ShippingPrice + "\n" +
                "total Order Price (with order shipping price )     :" + totalPriceOrder + "\n" +
                "CreatedOnUtc                                       :" + DateTime.Now.ToString("dddd, dd MMMM yyyy h:mm tt") + "\n" + "\n";

            //EMAI Integraiton to Admin   
            string receiver = ConfigurationManager.AppSettings["AdminAddress"];
            string body = "Dear Admin," +
                          "\nA New Order has been placed!" +
                          "\nCustomer Details are\n" +
                          "__________________________________\n" +
                          customer +
                          "\nThe product order details are: \n" +
                          "__________________________________\n" +
                          productInfo +
                          "\n\nThe order details are: \n" +
                          "__________________________________\n" +
                          orderDetails +
                          "\n\nFor more info, click here " +
                          webOrderDetailsUrl +
                          "\n\n\nThank you," +
                          "\nThe Shodypati Team.";

            string subject = "A New Order has been placed.";
            SendEmailBase(receiver, subject, body);
            //Get Order
            var order = Get(orderId);
            return order;
        }

        public int GetOrderIdFromGuidId(Guid id)
        {
            //db.OrdersTbls.Select(x => new Orders()
            var query = (from x in Db.OrdersTbls
                         where x.GuidId == id
                         select x.Id).SingleOrDefault();
            return query;
        }



        public void Put(int id, Orders entity)
        {
            var isEntity = from x in Db.OrdersTbls
                           where x.Id == entity.Id
                           select x;


            OrdersTbl entitySingle = isEntity.Single();


            entitySingle.CustomerId = entity.CustomerId;
            entitySingle.OrderStatus = entity.OrderStatus;
            //  entitySingle.TotalPriceBeforeShipping                                = entity.TotalPriceBeforeShipping                          ;
            entitySingle.ShippingPrice = entity.ShippingPrice;
            entitySingle.TotalPriceAfterShipping = entity.TotalPriceAfterShipping;
            entitySingle.OrderDiscountPrice = entity.OrderDiscountPrice;
            entitySingle.TotalPriceAfterShippingAndDiscount = entity.TotalPriceAfterShippingAndDiscount;


            entitySingle.OrderDate = entity.OrderDate;
            entitySingle.ShippingDate = entity.ShippingDate;
            entitySingle.BillingDate = entity.BillingDate;

            entitySingle.PickUpInStore = entity.PickUpInStore;
            entitySingle.OrderStatusId = entity.OrderStatusId;
            entitySingle.PaymentStatusId = entity.PaymentStatusId;
            entitySingle.Active = entity.Active;
            entitySingle.UpdatedOnUtc = DateTime.Now;




            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }


        public void Delete(int id)
        {
            var query = from x in Db.OrdersTbls
                        where x.Id == id
                        select x;

            if (query.Count() == 1)
            {
                OrdersTbl entity = query.SingleOrDefault();
                Db.OrdersTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());

                //delete all products
                Db.OrdersProductTbls.DeleteAllOnSubmit(from x in Db.OrdersProductTbls
                                                       where x.OrderGuidId == entity.GuidId
                                                       select x);
                // delete shipping and billing address
                Db.OrdersAddressTbls.DeleteAllOnSubmit(from x in Db.OrdersAddressTbls
                                                       where x.OrderGuidId == entity.GuidId
                                                       select x);

            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }


    }
}