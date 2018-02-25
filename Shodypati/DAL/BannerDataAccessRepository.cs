using Microsoft.Practices.Unity;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class BannerDataAccessRepository : BaseController, IBannerAccessRepository<Banner, int>
    {     
        public IEnumerable<Banner> Get()
        {
            var entities = Db.BannerTbls.Select(x => new Banner()
            {
                Id              = x.Id,
                GuidId          = x.GuidId,
                Name            = x.Name,
                BannerImages = Db.BannerImageTbls.Where(y => y.BannerGuidId == x.GuidId).Select(y => new BannerImage()
                {
                    Id              = y.Id,
                    URL             = HttpUtility.UrlPathEncode(y.URL),
                    MerchantId      = y.MerchantId,
                    CategoryId      = y.CategoryId,
                    ImagePath       = HttpUtility.UrlPathEncode(baseUrl + y.ImagePath),                  
                    Description     = y.Description,
                    DisplayOrder    = y.DisplayOrder
                }).ToList(),

                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc    = x.UpdatedOnUtc,
                Published       = x.Published,
                
            }).ToList();
            return entities;
        }
        

        public BannerMobile GetHomePageBanner()
        {
            var entity = Db.BannerTbls.FirstOrDefault(x => x.IsHomePageBanner == true);
            BannerMobile banner =new BannerMobile();
            if (entity != null)
            {

                banner.Id = entity.Id;
                banner.Name = entity.Name;
                banner.BannerImages = Db.BannerImageTbls.Where(y => y.BannerGuidId == entity.GuidId).Select(y =>
                    new BannerImageMobile()
                    {
                        URL = HttpUtility.UrlPathEncode(y.URL),
                        MerchantId = y.MerchantId,
                        CategoryId = y.CategoryId,
                        //Content/images/categories/placeholder.png
                        ImagePath = HttpUtility.UrlPathEncode(baseUrl + y.ImagePath),
                        Description = y.Description,
                        DisplayOrder = y.DisplayOrder
                    }).ToList();
            }
            return banner;
        }


        public IEnumerable<Banner> GetAllBanner()
        {
            var entities = Db.BannerTbls.Select(x => new Banner()
            {
                Id = x.Id,
                Name = x.Name,
                BannerImages = Db.BannerImageTbls.Where(y => y.BannerGuidId == x.GuidId).Select(y => new BannerImage()
                {
                    Id = y.Id,
                    URL = HttpUtility.UrlPathEncode(y.URL),
                    MerchantId = y.MerchantId,
                    CategoryId = y.CategoryId,
                    ImagePath = HttpUtility.UrlPathEncode(baseUrl + y.ImagePath),
                  
                    Description = y.Description,
                    DisplayOrder = y.DisplayOrder
                }).ToList(),

                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published,

            }).ToList();
            return entities;
        }



        public Banner Get(int id)
        {
            var entity = Db.BannerTbls.Where(x => x.Id == id).Select(x => new Banner()
            {
                Id              = x.Id,
                GuidId          = x.GuidId,
                Name            = x.Name,              
                CreatedOnUtc    = x.CreatedOnUtc,
                UpdatedOnUtc    = x.UpdatedOnUtc,
                Published       = x.Published,
             
            }).SingleOrDefault();

            if (entity == null)
            {
                return null;
            }

            //images
            entity.BannerImages = Db.BannerImageTbls.Where(x => x.BannerGuidId == entity.GuidId).Select(x => new BannerImage()
            {
                Id              = x.Id,
                URL             = HttpUtility.UrlPathEncode(x.URL),
                MerchantId      = x.MerchantId,
                CategoryId      = x.CategoryId,
                ImagePath       = HttpUtility.UrlPathEncode(baseUrl + x.ImagePath),               
                Description     = x.Description,
                DisplayOrder    = x.DisplayOrder
            }).ToList();
                       
            return entity;
        }
        
        public void Post(Banner entity)
        {
            Db.BannerTbls.InsertOnSubmit(new BannerTbl
            {
             //   Id              = entity.Id,       
                GuidId            = entity.GuidId,
                Name            = entity.Name,              
                CreatedOnUtc    = DateTime.Now,
                UpdatedOnUtc    = DateTime.Now,
                Published       = entity.Published,              
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

        public void Put(int id, Banner entity)
        {
            var isEntity = from x in Db.BannerTbls
                           where x.Id == entity.Id
                           select x;

           
                var entitySingle = isEntity.Single();

                entitySingle.Name = entity.Name;
                entitySingle.UpdatedOnUtc = DateTime.Now;
                entitySingle.Published = entity.Published;
           
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
            var entity = (from x in Db.BannerTbls
                        where x.Id == id
                        select x).SingleOrDefault();

            if (entity != null)
            {
                if (entity.IsHomePageBanner == true)
                {
                    ViewBag.Title = "Can't delete activated home page banner.";
                    return;
                }
                    

                Db.BannerTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());

                var bannerImages = (from x in Db.BannerImageTbls
                    where x.BannerGuidId == entity.GuidId
                    select x).ToList();

                foreach (var item in bannerImages)
                {
                    Db.BannerImageTbls.DeleteOnSubmit(item ?? throw new InvalidOperationException());
                }

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

        //custom
        public List<SelectListItem> GetAllBannersSelectList()
        {
            var entities = Db.BannerTbls.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text  = x.Name,
                Selected = (x.IsHomePageBanner!=null && x.IsHomePageBanner == true)
            }).ToList();

            return entities;
        }
    }
}
