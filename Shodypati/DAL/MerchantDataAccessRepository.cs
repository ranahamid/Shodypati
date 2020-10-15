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
    public class MerchantDataAccessRepository : BaseController, IMerchantAccessRepository<Merchant, int>
    {
        public MerchantDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }

        public IEnumerable<Merchant> Get()
        {
            var entities = Db.MerchantTbls.Select(x => new Merchant
            {
                Id = x.Id,
                URL = x.URL,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Logo = x.Logo,
                TypeOfGoods = x.TypeOfGoods,
                Notes = x.Notes,
                DiscountAvailable = x.DiscountAvailable,
                DisplayOrder = x.DisplayOrder,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).ToList();

            return entities;
        }

        public Merchant Get(int id)
        {
            var entity = Db.MerchantTbls.Where(x => x.Id == id).Select(x => new Merchant
            {
                Id = x.Id,
                URL = x.URL,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Logo = x.Logo,
                TypeOfGoods = x.TypeOfGoods,
                Notes = x.Notes,
                DiscountAvailable = x.DiscountAvailable,
                DisplayOrder = x.DisplayOrder,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published
            }).FirstOrDefault();

            return entity;
        }

        public void Post(Merchant entity)
        {
            Db.MerchantTbls.InsertOnSubmit(new MerchantTbl
            {
                // Id                  = Guid.NewGuid(),
                URL = entity.URL,
                Name_English = entity.Name_English,
                Name_Bangla = entity.Name_Bangla,
                Logo = entity.Logo,
                TypeOfGoods = entity.TypeOfGoods,
                Notes = entity.Notes,
                DiscountAvailable = entity.DiscountAvailable,
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

        public void Put(int id, Merchant entity)
        {
            var isEntity = from x in Db.MerchantTbls
                where x.Id == entity.Id
                select x;


            var entitySingle = isEntity.FirstOrDefault();

            if (entitySingle != null)
            {
                entitySingle.URL = entity.URL;
                entitySingle.Name_English = entity.Name_English;
                entitySingle.Name_Bangla = entity.Name_Bangla;
                // entitySingle.Logo               = entity.Logo;
                entitySingle.TypeOfGoods = entity.TypeOfGoods;
                entitySingle.Notes = entity.Notes;
                entitySingle.DiscountAvailable = entity.DiscountAvailable;
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
            var query = from x in Db.MerchantTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.FirstOrDefault();
                Db.MerchantTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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

        public List<SelectListItem> GetAllMerchantsSelectList()
        {
            var entities = Db.MerchantTbls.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name_English
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}