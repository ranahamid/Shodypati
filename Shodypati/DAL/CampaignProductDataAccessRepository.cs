using Microsoft.Practices.Unity;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class CampaignProductDataAccessRepository : BaseController, ICampaignProductAccessRepository<CampaignProducts, int>
    {
        public CampaignProductDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }


        public IEnumerable<CampaignProducts> Get()
        {
            var entities = Db.CampaignProductsTbls.Select(x => new CampaignProducts()
            {
                Id = x.Id,
                Products = Db.ProductTbls.Where(y => y.Id == x.ProductId).Select(z => new Product()
                {
                    Id                                  =z.Id,
                    ProductName_English                 =z.ProductName_English                  ,
                    ProductName_Bangla                  =z.ProductName_Bangla                   ,
                    Description_English                 =z.Description_English                  ,
                    Description_Bangla                  =z.Description_Bangla                   ,
                    CategoryId                          =z.CategoryId                           ,               
                    QuantityInStock                     =z.QuantityInStock                      ,                
                    MainImagePath =  HttpUtility.UrlPathEncode(baseUrl + z.MainImagePath) ,
                    UnitPrice                           =z.UnitPrice                            ,
                    OfferPrice                          =z.OfferPrice                           ,
                    DiscountPercentage                  =z.DiscountPercentage                   ,
                    DiscountAmount                      =z.DiscountAmount                       ,
                    DiscountStartDateUtc                =z.DiscountStartDateUtc                 ,
                    DiscountEndDateUtc                  =z.DiscountEndDateUtc                   ,
                    DiscountRequiresCouponCode          =z.DiscountRequiresCouponCode           ,
                    CouponCode                          =z.CouponCode                           ,
                    DisplayOrder                        =z.DisplayOrder                         ,
                    IsHot                               =z.IsHot                                ,
                    BrandId                             =z.BrandId                              ,
                    MerchantId                          =z.MerchantId                           ,
                    AllowCustomerReviews                =z.AllowCustomerReviews                 ,
                    IsShipEnabled                       =z.IsShipEnabled                        ,
                    IsFreeShipping                      =z.IsFreeShipping                       ,
                    ShipSeparately                      =z.ShipSeparately                       ,
                    AdditionalShippingCharge            =z.AdditionalShippingCharge             ,
                    DisplayStockAvailability            =z.DisplayStockAvailability             ,
                    DisplayStockQuantity                =z.DisplayStockQuantity                 ,
                    OrderMinimumQuantity                =z.OrderMinimumQuantity                 ,
                    OrderMaximumQuantity                =z.OrderMaximumQuantity                 ,
                    NotReturnable                       =z.NotReturnable                        ,
                    DisableBuyButton                    =z.DisableBuyButton                     ,
                    AvailableForPreOrder                =z.AvailableForPreOrder                 ,
                    PreOrderAvailabilityStartDateTimeUtc=z.PreOrderAvailabilityStartDateTimeUtc ,
                    ProductCost                         =z.ProductCost                          ,
                    MarkAsNew                           =z.MarkAsNew                            ,
                    MarkAsNewStartDateTimeUtc           =z.MarkAsNewStartDateTimeUtc            ,
                    MarkAsNewEndDateTimeUtc             =z.MarkAsNewEndDateTimeUtc              ,
                    Size                                =z.Size                                 ,
                    Color                               =z.Color                                ,
                    Weight                              =z.Weight                               ,
                    Length                              =z.Length                               ,
                    Width                               =z.Width                                ,
                    Height                              =z.Height                               ,
                    AvailableStartDateTimeUtc           =z.AvailableStartDateTimeUtc            ,
                    AvailableEndDateTimeUtc             =z.AvailableEndDateTimeUtc              ,
                    RelatedProductId                    =z.RelatedProductId                     ,
                    CreatedOnUtc                        =z.CreatedOnUtc                         ,
                    UpdatedOnUtc                        =z.UpdatedOnUtc                         ,
                    Published                           =z.Published                            ,
                

                }).FirstOrDefault(),

            }).ToList();

            return entities;
        }

        public CampaignProducts Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetAllCampaignProductSelectList()
        {
            throw new NotImplementedException();
        }

        //public void Post(CampaignProducts entity)
        //{
        //    throw new NotImplementedException();
        //}

        public void Put(CampaignProducts entity)
        {
            //delete all table 
            var query = from x in Db.CampaignProductsTbls
                        select x;

            foreach (var detail in query)
            {
                Db.CampaignProductsTbls.DeleteOnSubmit(detail);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
            //insert into table 

            List<string> selectedProduct= entity.AllProductsSelectListStr;

            foreach(var item in selectedProduct)
            {
                Db.CampaignProductsTbls.InsertOnSubmit(new CampaignProductsTbl
                {
                    ProductId = Int32.Parse(item)
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
        }


        public List<string> GetAllCampaignProductsList()
        {
            List<string> entities = new List<string>();

            var items =  Db.CampaignProductsTbls.ToList();
            foreach(var item in items)
            {
                entities.Add(item.ProductId.ToString());
            }
            return entities;
        }

        //public void Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
