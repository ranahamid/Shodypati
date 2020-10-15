using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class BrandDataAccessRepository : BaseController, IBrandAccessRepository<Brand, int>
    {
        public IEnumerable<Brand> Get()
        {
            var entities = new List<Brand>();

            entities = Db.BrandTbls.Select(x => new Brand
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Logo = x.Logo,
                DisplayOrder = x.DisplayOrder,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).ToList();

            return entities;
        }

        public Brand Get(int id)
        {
            var entity = Db.BrandTbls.Where(x => x.Id == id).Select(x => new Brand
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Logo = x.Logo,
                DisplayOrder = x.DisplayOrder,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).SingleOrDefault();

            if (entity == null) return null;

            return entity;
        }

        public void Post(Brand entity)
        {
            Db.BrandTbls.InsertOnSubmit(new BrandTbl
            {
                //  Id              = Guid.NewGuid(),           
                Name_English = entity.Name_English,
                Name_Bangla = entity.Name_Bangla,
                Logo = entity.Logo,
                DisplayOrder = entity.DisplayOrder,
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

        public void Put(int id, Brand entity)
        {
            var isEntity = from x in Db.BrandTbls
                where x.Id == entity.Id
                select x;

            if (isEntity == null) return;

            if (isEntity != null)
            {
                var entitySingle = isEntity.Single();

                entitySingle.Name_English = entity.Name_English;
                entitySingle.Name_Bangla = entity.Name_Bangla;
                // entitySingle.Logo         = entity.Logo;
                entitySingle.DisplayOrder = entity.DisplayOrder;

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
            var query = from x in Db.BrandTbls
                where x.Id == id
                select x;

            if (query != null && query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.BrandTbls.DeleteOnSubmit(entity);
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

        public List<SelectListItem> GetAllBrandsSelectList()
        {
            var entities = new List<SelectListItem>();

            entities = Db.BrandTbls.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name_English
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}