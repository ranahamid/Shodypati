using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class ProductDataAccessRepository : BaseController, IProductAccessRepository<Product, int>
    {
        public ProductDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }

        public IEnumerable<Product> Get()
        {
            var entities = Db.ProductTbls.Select(x => new Product
            {
                Id = x.Id,

                ProductName_English = x.ProductName_English,
                ProductName_Bangla = x.ProductName_Bangla,
                Description_English = x.Description_English,
                Description_Bangla = x.Description_Bangla,
                CategoryId = x.CategoryId,
                QuantityInStock = x.QuantityInStock,
                MainImagePath = HttpUtility.UrlPathEncode(baseUrl + x.MainImagePath),
                RawDBImagePath = x.MainImagePath,
                UnitPrice = x.UnitPrice,
                OfferPrice = x.OfferPrice,
                DiscountPercentage = x.DiscountPercentage,
                DiscountAmount = x.DiscountAmount,
                DiscountStartDateUtc = x.DiscountStartDateUtc,
                DiscountEndDateUtc = x.DiscountEndDateUtc,
                DiscountRequiresCouponCode = x.DiscountRequiresCouponCode,
                CouponCode = x.CouponCode,
                DisplayOrder = x.DisplayOrder,
                IsHot = x.IsHot,
                BrandId = x.BrandId,
                MerchantId = x.MerchantId,
                AllowCustomerReviews = x.AllowCustomerReviews,
                IsShipEnabled = x.IsShipEnabled,
                IsFreeShipping = x.IsFreeShipping,
                ShipSeparately = x.ShipSeparately,
                AdditionalShippingCharge = x.AdditionalShippingCharge,
                DisplayStockAvailability = x.DisplayStockAvailability,
                DisplayStockQuantity = x.DisplayStockQuantity,
                OrderMinimumQuantity = x.OrderMinimumQuantity,
                OrderMaximumQuantity = x.OrderMaximumQuantity,
                NotReturnable = x.NotReturnable,
                DisableBuyButton = x.DisableBuyButton,
                AvailableForPreOrder = x.AvailableForPreOrder,
                PreOrderAvailabilityStartDateTimeUtc = x.PreOrderAvailabilityStartDateTimeUtc,
                ProductCost = x.ProductCost,
                MarkAsNew = x.MarkAsNew,
                MarkAsNewStartDateTimeUtc = x.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = x.MarkAsNewEndDateTimeUtc,
                Size = x.Size,
                Color = x.Color,
                Weight = x.Weight,
                Length = x.Length,
                Width = x.Width,
                Height = x.Height,
                AvailableStartDateTimeUtc = x.AvailableStartDateTimeUtc,
                AvailableEndDateTimeUtc = x.AvailableEndDateTimeUtc,
                RelatedProductId = x.RelatedProductId,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).OrderByDescending(x => x.Id).ToList();

            return entities;
        }

        public Product Get(int id)
        {
            var entity = Db.ProductTbls.Where(x => x.Id == id).Select(x => new Product
            {
                Id = x.Id,
                ProductName_English = x.ProductName_English,
                ProductName_Bangla = x.ProductName_Bangla,
                Description_English = x.Description_English,
                Description_Bangla = x.Description_Bangla,
                CategoryId = x.CategoryId,
                QuantityInStock = x.QuantityInStock,
                MainImagePath = HttpUtility.UrlPathEncode(baseUrl + x.MainImagePath),
                RawDBImagePath = x.MainImagePath,
                ProductImages = Db.ProductImageTbls
                    .Where(z => z.ProductId.ToString().ToLower() == id.ToString().ToLower())
                    .Select(z => new ProductImage
                    {
                        Id = z.Id,
                        ImagePath = HttpUtility.UrlPathEncode(baseUrl + z.ImagePath),
                        Description = z.Description,
                        DisplayOrder = z.DisplayOrder
                    }).ToList(),


                UnitPrice = x.UnitPrice,
                OfferPrice = x.OfferPrice,
                DiscountPercentage = x.DiscountPercentage,
                DiscountAmount = x.DiscountAmount,
                DiscountStartDateUtc = x.DiscountStartDateUtc,
                DiscountEndDateUtc = x.DiscountEndDateUtc,
                DiscountRequiresCouponCode = x.DiscountRequiresCouponCode,
                CouponCode = x.CouponCode,
                DisplayOrder = x.DisplayOrder,
                IsHot = x.IsHot,
                BrandId = x.BrandId,
                MerchantId = x.MerchantId,
                AllowCustomerReviews = x.AllowCustomerReviews,
                IsShipEnabled = x.IsShipEnabled,
                IsFreeShipping = x.IsFreeShipping,
                ShipSeparately = x.ShipSeparately,
                AdditionalShippingCharge = x.AdditionalShippingCharge,
                DisplayStockAvailability = x.DisplayStockAvailability,
                DisplayStockQuantity = x.DisplayStockQuantity,
                OrderMinimumQuantity = x.OrderMinimumQuantity,
                OrderMaximumQuantity = x.OrderMaximumQuantity,
                NotReturnable = x.NotReturnable,
                DisableBuyButton = x.DisableBuyButton,
                AvailableForPreOrder = x.AvailableForPreOrder,
                PreOrderAvailabilityStartDateTimeUtc = x.PreOrderAvailabilityStartDateTimeUtc,
                ProductCost = x.ProductCost,
                MarkAsNew = x.MarkAsNew,
                MarkAsNewStartDateTimeUtc = x.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = x.MarkAsNewEndDateTimeUtc,
                Size = x.Size,
                Color = x.Color,
                Weight = x.Weight,
                Length = x.Length,
                Width = x.Width,
                Height = x.Height,
                AvailableStartDateTimeUtc = x.AvailableStartDateTimeUtc,
                AvailableEndDateTimeUtc = x.AvailableEndDateTimeUtc,
                RelatedProductId = x.RelatedProductId,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).FirstOrDefault();

            return entity;
        }

        public void Post(Product entity)
        {
            var imgAddress = string.Empty;
            if (entity.MainImagePath != null) imgAddress = entity.MainImagePath.TrimStart('/');


            Db.ProductTbls.InsertOnSubmit(new ProductTbl
            {
                //  Id = entity.Id,            
                ProductName_English = entity.ProductName_English,
                ProductName_Bangla = entity.ProductName_Bangla,
                Description_English = entity.Description_English,
                Description_Bangla = entity.Description_Bangla,
                CategoryId = entity.CategoryId,
                QuantityInStock = entity.QuantityInStock,
                MainImagePath = imgAddress,
                UnitPrice = entity.UnitPrice,
                OfferPrice = entity.OfferPrice,
                DiscountPercentage = entity.DiscountPercentage,
                DiscountAmount = entity.DiscountAmount,
                DiscountStartDateUtc = entity.DiscountStartDateUtc,
                DiscountEndDateUtc = entity.DiscountEndDateUtc,
                DiscountRequiresCouponCode = entity.DiscountRequiresCouponCode,
                CouponCode = entity.CouponCode,
                DisplayOrder = entity.DisplayOrder,
                IsHot = entity.IsHot,
                BrandId = entity.BrandId,
                MerchantId = entity.MerchantId,
                AllowCustomerReviews = entity.AllowCustomerReviews,
                IsShipEnabled = entity.IsShipEnabled,
                IsFreeShipping = entity.IsFreeShipping,
                ShipSeparately = entity.ShipSeparately,
                AdditionalShippingCharge = entity.AdditionalShippingCharge,
                DisplayStockAvailability = entity.DisplayStockAvailability,
                DisplayStockQuantity = entity.DisplayStockQuantity,
                OrderMinimumQuantity = entity.OrderMinimumQuantity,
                OrderMaximumQuantity = entity.OrderMaximumQuantity,
                NotReturnable = entity.NotReturnable,
                DisableBuyButton = entity.DisableBuyButton,
                AvailableForPreOrder = entity.AvailableForPreOrder,
                PreOrderAvailabilityStartDateTimeUtc = entity.PreOrderAvailabilityStartDateTimeUtc,
                ProductCost = entity.ProductCost,
                MarkAsNew = entity.MarkAsNew,
                MarkAsNewStartDateTimeUtc = entity.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = entity.MarkAsNewEndDateTimeUtc,
                Size = entity.Size,
                Color = entity.Color,
                Weight = entity.Weight,
                Length = entity.Length,
                Width = entity.Width,
                Height = entity.Height,
                AvailableStartDateTimeUtc = entity.AvailableStartDateTimeUtc,
                AvailableEndDateTimeUtc = entity.AvailableEndDateTimeUtc,
                RelatedProductId = entity.RelatedProductId,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now,
                Published = entity.Published
            });

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        public void Put(int id, Product entity)
        {
            var isEntity = from x in Db.ProductTbls
                where x.Id == entity.Id
                select x;

            var imgAddress = string.Empty;
            if (entity.RawDBImagePath != null) imgAddress = entity.RawDBImagePath.TrimStart('/');


            var entitySingle = isEntity.FirstOrDefault();

            if (entitySingle != null)
            {
                entitySingle.ProductName_English = entity.ProductName_English;
                entitySingle.ProductName_Bangla = entity.ProductName_Bangla;
                entitySingle.Description_English = entity.Description_English;
                entitySingle.Description_Bangla = entity.Description_Bangla;
                entitySingle.CategoryId = entity.CategoryId;
                entitySingle.QuantityInStock = entity.QuantityInStock;
                entitySingle.MainImagePath = imgAddress;
                entitySingle.UnitPrice = entity.UnitPrice;
                entitySingle.OfferPrice = entity.OfferPrice;
                entitySingle.DiscountPercentage = entity.DiscountPercentage;
                entitySingle.DiscountAmount = entity.DiscountAmount;
                entitySingle.DiscountStartDateUtc = entity.DiscountStartDateUtc;
                entitySingle.DiscountEndDateUtc = entity.DiscountEndDateUtc;
                entitySingle.DiscountRequiresCouponCode = entity.DiscountRequiresCouponCode;
                entitySingle.CouponCode = entity.CouponCode;
                entitySingle.DisplayOrder = entity.DisplayOrder;
                entitySingle.IsHot = entity.IsHot;
                entitySingle.BrandId = entity.BrandId;
                entitySingle.MerchantId = entity.MerchantId;
                entitySingle.AllowCustomerReviews = entity.AllowCustomerReviews;
                entitySingle.IsShipEnabled = entity.IsShipEnabled;
                entitySingle.IsFreeShipping = entity.IsFreeShipping;
                entitySingle.ShipSeparately = entity.ShipSeparately;
                entitySingle.AdditionalShippingCharge = entity.AdditionalShippingCharge;
                entitySingle.DisplayStockAvailability = entity.DisplayStockAvailability;
                entitySingle.DisplayStockQuantity = entity.DisplayStockQuantity;
                entitySingle.OrderMinimumQuantity = entity.OrderMinimumQuantity;
                entitySingle.OrderMaximumQuantity = entity.OrderMaximumQuantity;
                entitySingle.NotReturnable = entity.NotReturnable;
                entitySingle.DisableBuyButton = entity.DisableBuyButton;
                entitySingle.AvailableForPreOrder = entity.AvailableForPreOrder;
                entitySingle.PreOrderAvailabilityStartDateTimeUtc = entity.PreOrderAvailabilityStartDateTimeUtc;
                entitySingle.ProductCost = entity.ProductCost;
                entitySingle.MarkAsNew = entity.MarkAsNew;
                entitySingle.MarkAsNewStartDateTimeUtc = entity.MarkAsNewStartDateTimeUtc;
                entitySingle.MarkAsNewEndDateTimeUtc = entity.MarkAsNewEndDateTimeUtc;
                entitySingle.Size = entity.Size;
                entitySingle.Color = entity.Color;
                entitySingle.Weight = entity.Weight;
                entitySingle.Length = entity.Length;
                entitySingle.Width = entity.Width;
                entitySingle.Height = entity.Height;
                entitySingle.AvailableStartDateTimeUtc = entity.AvailableStartDateTimeUtc;
                entitySingle.AvailableEndDateTimeUtc = entity.AvailableEndDateTimeUtc;
                entitySingle.RelatedProductId = entity.RelatedProductId;
                entitySingle.UpdatedOnUtc = DateTime.Now;
                entitySingle.Published = entity.Published;
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

        public void Delete(int id)
        {
            var query = from x in Db.ProductTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.FirstOrDefault();
                Db.ProductTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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


        public List<SelectListItem> GetAllProductsSelectList()
        {
            var entities = Db.ProductTbls.Where(z => z.Published == true).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ProductName_English
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }

        public List<CategoryMobile> GetProductsByCategoriesList()
        {
            var key = "GetProductsByCategoriesList";
            //List<CategoryMobile> items=new List<CategoryMobile>();
            var items = CacheStorage.Get<List<CategoryMobile>>(key);

            if (items != null)
            {
                return items;
            }

            items = Db.CategoryTbls.Select(x => new CategoryMobile
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                ImagePath = HttpUtility.UrlPathEncode(baseUrl + x.ImagePath),

                products = Db.ProductTbls.Where(y => y.CategoryId == x.Id && y.Published == true).Select(y =>
                    new ProductMobile
                    {
                        Id = y.Id,
                        ProductName_English = y.ProductName_English,
                        ProductName_Bangla = y.ProductName_Bangla,
                        Description_English = y.Description_English,
                        Description_Bangla = y.Description_Bangla,
                        CategoryId = y.CategoryId,
                        QuantityInStock = y.QuantityInStock,
                        MainImagePath = HttpUtility.UrlPathEncode(baseUrl + y.MainImagePath),

                        UnitPrice = y.UnitPrice,
                        OfferPrice = y.OfferPrice,
                        //DiscountPercentage                  =y.DiscountPercentage                   ,
                        //DiscountAmount                      =y.DiscountAmount                       ,
                        //DiscountStartDateUtc                =y.DiscountStartDateUtc                 ,
                        //DiscountEndDateUtc                  =y.DiscountEndDateUtc                   ,
                        //DiscountRequiresCouponCode          =y.DiscountRequiresCouponCode           ,
                        //CouponCode                          =y.CouponCode                           ,
                        DisplayOrder = y.DisplayOrder,
                        //IsHot                               =y.IsHot                                ,
                        BrandId = y.BrandId,
                        MerchantId = y.MerchantId,
                        //AllowCustomerReviews                =y.AllowCustomerReviews                 ,
                        //IsShipEnabled                       =y.IsShipEnabled                        ,
                        //IsFreeShipping                      =y.IsFreeShipping                       ,
                        //ShipSeparately                      =y.ShipSeparately                       ,
                        //AdditionalShippingCharge            =y.AdditionalShippingCharge             ,
                        //DisplayStockAvailability            =y.DisplayStockAvailability             ,
                        //DisplayStockQuantity                =y.DisplayStockQuantity                 ,
                        OrderMinimumQuantity = y.OrderMinimumQuantity,
                        OrderMaximumQuantity = y.OrderMaximumQuantity,
                        //NotReturnable                       =y.NotReturnable                        ,
                        //DisableBuyButton                    =y.DisableBuyButton                     ,
                        //AvailableForPreOrder                =y.AvailableForPreOrder                 ,
                        //PreOrderAvailabilityStartDateTimeUtc=y.PreOrderAvailabilityStartDateTimeUtc ,
                        //ProductCost                         =y.ProductCost                          ,
                        //MarkAsNew                           =y.MarkAsNew                            ,
                        //MarkAsNewStartDateTimeUtc           =y.MarkAsNewStartDateTimeUtc            ,
                        //MarkAsNewEndDateTimeUtc             =y.MarkAsNewEndDateTimeUtc              ,
                        Size = y.Size,
                        Color = y.Color,
                        Weight = y.Weight,
                        Length = y.Length,
                        Width = y.Width,
                        Height = y.Height
                        //  AvailableStartDateTimeUtc           =y.AvailableStartDateTimeUtc            ,
                        //  AvailableEndDateTimeUtc             =y.AvailableEndDateTimeUtc              ,
                        // RelatedProductId                    =y.RelatedProductId                      ,
                        // CreatedOnUtc                        =y.CreatedOnUtc                          ,
                        //  UpdatedOnUtc                        =y.UpdatedOnUtc                         ,
                        //  Published                           =y.Published                            ,
                        //  Active                              =y.Active
                    }).Take(4).OrderBy(y => y.DisplayOrder).ToList()
            }).ToList();

            var itemsWithoutProduct = new List<CategoryMobile>();
            foreach (var item in items)
                if (item.products.Any())
                    itemsWithoutProduct.Add(item);
            CacheStorage.Add(key, itemsWithoutProduct);
            return itemsWithoutProduct;
        }

        public List<CategoryMobile> GetProductsByCategoriesListWeb()
        {
            var key = "GetProductsByCategoriesListWeb";
            //List<CategoryMobile> items=new List<CategoryMobile>();
            var items = CacheStorage.Get<List<CategoryMobile>>(key);

            if (items != null)
            {
                return items;
            }

            items = Db.CategoryTbls.Select(x => new CategoryMobile
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                ImagePath = HttpUtility.UrlPathEncode(baseUrl + x.ImagePath),

                products = Db.ProductTbls.Where(y => y.CategoryId == x.Id && y.Published == true).Select(y =>
                    new ProductMobile
                    {
                        Id = y.Id,
                        ProductName_English = y.ProductName_English,
                        ProductName_Bangla = y.ProductName_Bangla,
                        Description_English = y.Description_English,
                        Description_Bangla = y.Description_Bangla,
                        CategoryId = y.CategoryId,
                        QuantityInStock = y.QuantityInStock,
                        MainImagePath = HttpUtility.UrlPathEncode(baseUrl + y.MainImagePath),

                        UnitPrice = y.UnitPrice,
                        OfferPrice = y.OfferPrice,
                        //DiscountPercentage                  =y.DiscountPercentage                   ,
                        //DiscountAmount                      =y.DiscountAmount                       ,
                        //DiscountStartDateUtc                =y.DiscountStartDateUtc                 ,
                        //DiscountEndDateUtc                  =y.DiscountEndDateUtc                   ,
                        //DiscountRequiresCouponCode          =y.DiscountRequiresCouponCode           ,
                        //CouponCode                          =y.CouponCode                           ,
                        DisplayOrder = y.DisplayOrder,
                        //IsHot                               =y.IsHot                                ,
                        BrandId = y.BrandId,
                        MerchantId = y.MerchantId,
                        //AllowCustomerReviews                =y.AllowCustomerReviews                 ,
                        //IsShipEnabled                       =y.IsShipEnabled                        ,
                        //IsFreeShipping                      =y.IsFreeShipping                       ,
                        //ShipSeparately                      =y.ShipSeparately                       ,
                        //AdditionalShippingCharge            =y.AdditionalShippingCharge             ,
                        //DisplayStockAvailability            =y.DisplayStockAvailability             ,
                        //DisplayStockQuantity                =y.DisplayStockQuantity                 ,
                        OrderMinimumQuantity = y.OrderMinimumQuantity,
                        OrderMaximumQuantity = y.OrderMaximumQuantity,
                        //NotReturnable                       =y.NotReturnable                        ,
                        //DisableBuyButton                    =y.DisableBuyButton                     ,
                        //AvailableForPreOrder                =y.AvailableForPreOrder                 ,
                        //PreOrderAvailabilityStartDateTimeUtc=y.PreOrderAvailabilityStartDateTimeUtc ,
                        //ProductCost                         =y.ProductCost                          ,
                        //MarkAsNew                           =y.MarkAsNew                            ,
                        //MarkAsNewStartDateTimeUtc           =y.MarkAsNewStartDateTimeUtc            ,
                        //MarkAsNewEndDateTimeUtc             =y.MarkAsNewEndDateTimeUtc              ,
                        Size = y.Size,
                        Color = y.Color,
                        Weight = y.Weight,
                        Length = y.Length,
                        Width = y.Width,
                        Height = y.Height
                        //  AvailableStartDateTimeUtc           =y.AvailableStartDateTimeUtc            ,
                        //  AvailableEndDateTimeUtc             =y.AvailableEndDateTimeUtc              ,
                        // RelatedProductId                    =y.RelatedProductId                      ,
                        // CreatedOnUtc                        =y.CreatedOnUtc                          ,
                        //  UpdatedOnUtc                        =y.UpdatedOnUtc                         ,
                        //  Published                           =y.Published                            ,
                        //  Active                              =y.Active
                    }).OrderBy(y => y.DisplayOrder).ToList()
            }).ToList();

            var itemsWithoutProduct = new List<CategoryMobile>();
            foreach (var item in items)
                if (item.products.Any())
                    itemsWithoutProduct.Add(item);
            CacheStorage.Add(key, itemsWithoutProduct);
            return itemsWithoutProduct;
        }

        public List<ProductMobile> GetProductsBy(int categoryId, int merchantId)
        {
            var key = "GetProductsBy";

            key = key + "CategoryId-" + categoryId;
            key = key + "MerchantId-" + merchantId;


            var items = CacheStorage.Get<List<ProductMobile>>(key);
            var itemsReturn = new List<ProductMobile>();

            if (items == null)
            {
                var key2 = "AllProducts";
                var itemsAll = CacheStorage.Get<List<ProductMobile>>(key2);

                var items1 = new List<ProductMobile>();
                var items2 = new List<ProductMobile>();

                if (itemsAll == null)
                    items = Db.ProductTbls.Where(z => z.Published == true).Select(y => new ProductMobile
                    {
                        Id = y.Id,
                        ProductName_English = y.ProductName_English,
                        ProductName_Bangla = y.ProductName_Bangla,
                        Description_English = y.Description_English,
                        Description_Bangla = y.Description_Bangla,
                        CategoryId = y.CategoryId,
                        QuantityInStock = y.QuantityInStock,
                        MainImagePath = HttpUtility.UrlPathEncode(baseUrl + y.MainImagePath),

                        UnitPrice = y.UnitPrice,
                        OfferPrice = y.OfferPrice,
                        //DiscountPercentage                  =y.DiscountPercentage                   ,
                        //DiscountAmount                      =y.DiscountAmount                       ,
                        //DiscountStartDateUtc                =y.DiscountStartDateUtc                 ,
                        //DiscountEndDateUtc                  =y.DiscountEndDateUtc                   ,
                        //DiscountRequiresCouponCode          =y.DiscountRequiresCouponCode           ,
                        //CouponCode                          =y.CouponCode                           ,
                        //DisplayOrder                        =y.DisplayOrder                         ,
                        //IsHot                               =y.IsHot                                ,
                        BrandId = y.BrandId,
                        MerchantId = y.MerchantId,
                        //AllowCustomerReviews                =y.AllowCustomerReviews                 ,
                        //IsShipEnabled                       =y.IsShipEnabled                        ,
                        //IsFreeShipping                      =y.IsFreeShipping                       ,
                        //ShipSeparately                      =y.ShipSeparately                       ,
                        //AdditionalShippingCharge            =y.AdditionalShippingCharge             ,
                        //DisplayStockAvailability            =y.DisplayStockAvailability             ,
                        //DisplayStockQuantity                =y.DisplayStockQuantity                 ,
                        OrderMinimumQuantity = y.OrderMinimumQuantity,
                        OrderMaximumQuantity = y.OrderMaximumQuantity,
                        //NotReturnable                       =y.NotReturnable                        ,
                        //DisableBuyButton                    =y.DisableBuyButton                     ,
                        //AvailableForPreOrder                =y.AvailableForPreOrder                 ,
                        //PreOrderAvailabilityStartDateTimeUtc=y.PreOrderAvailabilityStartDateTimeUtc ,
                        //ProductCost                         =y.ProductCost                          ,
                        //MarkAsNew                           =y.MarkAsNew                            ,
                        //MarkAsNewStartDateTimeUtc           =y.MarkAsNewStartDateTimeUtc            ,
                        //MarkAsNewEndDateTimeUtc             =y.MarkAsNewEndDateTimeUtc              ,
                        Size = y.Size,
                        Color = y.Color,
                        Weight = y.Weight,
                        Length = y.Length,
                        Width = y.Width,
                        Height = y.Height
                        //  AvailableStartDateTimeUtc           =y.AvailableStartDateTimeUtc            ,
                        //  AvailableEndDateTimeUtc             =y.AvailableEndDateTimeUtc              ,
                        // RelatedProductId                    =y.RelatedProductId                     ,
                        // CreatedOnUtc                        =y.CreatedOnUtc                         ,
                        //  UpdatedOnUtc                        =y.UpdatedOnUtc                         ,
                        //  Published                           =y.Published                            ,
                        //  Active                              =y.Active
                    }).ToList();
                else
                    items = CacheStorage.Get<List<ProductMobile>>(key2);

                if (categoryId != 0) items1 = items.Where(y => y.CategoryId == categoryId).ToList();
                if (merchantId != 0) items2 = items.Where(y => y.MerchantId == merchantId).ToList();

                if (items1.Any()) itemsReturn = items1;

                if (items2.Any())
                    foreach (var it in items2)
                        if (!itemsReturn.Contains(it))
                            itemsReturn.Add(it);

                CacheStorage.Add(key, itemsReturn);
                return itemsReturn;
            }

            return items;
        }
    }
}