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
    public class OrderShippingDataAccessRepository : BaseController, IOrderShippingAccessRepository<OrderShipping, int>
    {
        public IEnumerable<OrderShipping> Get()
        {
            var entities = Db.OrderShippingTbls.Select(x => new OrderShipping()
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note,
            }).ToList();

            return entities;
        }

        public OrderShipping Get(int id)
        {
            OrderShipping entity = Db.OrderShippingTbls.Where(x => x.Id == id).Select(x => new OrderShipping()
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note,
            }).SingleOrDefault();

            return entity;
        }

        public void Post(OrderShipping entity)
        {
            Db.OrderShippingTbls.InsertOnSubmit(new OrderShippingTbl
            {
                //   Id              = entity.Id,           
                Name = entity.Name,
                Note = entity.Note,

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
                OrderShippingTbl entity = query.SingleOrDefault();
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
        public List<System.Web.Mvc.SelectListItem> GetAllOrderShippingSelectList()
        {
            var entities = Db.OrderShippingTbls.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}