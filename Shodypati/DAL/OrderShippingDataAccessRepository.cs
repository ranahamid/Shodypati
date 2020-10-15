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
    public class OrderShippingDataAccessRepository : BaseController, IOrderShippingAccessRepository<OrderShipping, int>
    {
        public IEnumerable<OrderShipping> Get()
        {
            var entities = Db.OrderShippingTbls.Select(x => new OrderShipping
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }).ToList();

            return entities;
        }

        public OrderShipping Get(int id)
        {
            var entity = Db.OrderShippingTbls.Where(x => x.Id == id).Select(x => new OrderShipping
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }).SingleOrDefault();

            return entity;
        }

        public void Post(OrderShipping entity)
        {
            Db.OrderShippingTbls.InsertOnSubmit(new OrderShippingTbl
            {
                //   Id              = entity.Id,           
                Name = entity.Name,
                Note = entity.Note
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

        public void Put(int id, OrderShipping entity)
        {
            var isEntity = from x in Db.OrderShippingTbls
                where x.Id == entity.Id
                select x;

            var entitySingle = isEntity.Single();

            entitySingle.Name = entity.Name;
            entitySingle.Note = entity.Note;


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
            var query = from x in Db.OrderShippingTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.OrderShippingTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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
        public List<SelectListItem> GetAllOrderShippingSelectList()
        {
            var entities = Db.OrderShippingTbls.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}