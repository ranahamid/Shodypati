using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class BazarListDataAccessRepository : BaseController, IBazarListAccessRepository<BazarList, int>
    {
        public IEnumerable<BazarList> Get()
        {
            var entities = Db.BazarListTbls.Select(x => new BazarList
            {
                Id = x.Id,
                Name = x.Name,
                MainImagePath = HttpUtility.UrlPathEncode(baseUrl + x.MainImagePath),
                RawDBImagePath = x.MainImagePath,
                Address = x.Address,
                Description = x.Description,
                Number = x.Number,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc
            }).ToList();

            return entities;
        }

        public BazarList Get(int id)
        {
            var entity = Db.BazarListTbls.Where(x => x.Id == id).Select(x => new BazarList
            {
                Id = x.Id,
                Name = x.Name,

                MainImagePath = HttpUtility.UrlPathEncode(baseUrl + x.MainImagePath),
                RawDBImagePath = x.MainImagePath,
                Address = x.Address,
                Description = x.Description,
                Number = x.Number,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc
            }).SingleOrDefault();

            return entity;
        }


        public void Post(BazarList entity)
        {
            var imgAddress = string.Empty;

            if (entity.MainImagePath != null) imgAddress = entity.MainImagePath.TrimStart('/');

            Db.BazarListTbls.InsertOnSubmit(new BazarListTbl
            {
                Name = entity.Name,
                MainImagePath = imgAddress,
                Address = entity.Address,
                Description = entity.Description,
                Number = entity.Number,
                CreatedOnUtc = entity.CreatedOnUtc,
                UpdatedOnUtc = entity.UpdatedOnUtc
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

        public void Put(int id, BazarList entity)
        {
            var isEntity = from x in Db.BazarListTbls
                where x.Id == entity.Id
                select x;

            var imgAddress = string.Empty;
            if (entity.RawDBImagePath != null) imgAddress = entity.RawDBImagePath.TrimStart('/');


            var entitySingle = isEntity.Single();
            entitySingle.Name = entity.Name;
            entitySingle.MainImagePath = imgAddress;
            entitySingle.Address = entity.Address;
            entitySingle.Description = entity.Description;
            entitySingle.Number = entity.Number;
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
            var query = from x in Db.BazarListTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.BazarListTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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