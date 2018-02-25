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
    public class OrderStatusDataAccessRepository : BaseController, IOrderStatusAccessRepository<OrderStatus, int>
    {
        public IEnumerable<OrderStatus> Get()
        {
            var entities = Db.OrderStatusTbls.Select(x => new OrderStatus()
            {
                Id = x.Id,
                Name = x.Name,
                DefaultStatus = x.DefaultStatus,
            }).ToList();

            return entities;
        }

        public OrderStatus Get(int id)
        {
            var entity = Db.OrderStatusTbls.Where(x => x.Id == id).Select(x => new OrderStatus()
            {
                Id = x.Id,
                Name = x.Name,
                DefaultStatus = x.DefaultStatus,
            }).SingleOrDefault();

            return entity;
        }

        public void Post(OrderStatus entity)
        {
            Db.OrderStatusTbls.InsertOnSubmit(new OrderStatusTbl
            {
                //   Id              = entity.Id,           
                Name = entity.Name,
                DefaultStatus = entity.DefaultStatus,

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

        public void Put(int id, OrderStatus entity)
        {
            var isEntity = from x in Db.OrderStatusTbls
                           where x.Id == entity.Id
                           select x;



            var entitySingle = isEntity.Single();

            entitySingle.Name = entity.Name;
            entitySingle.DefaultStatus = entity.DefaultStatus;


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
            var query = from x in Db.OrderStatusTbls
                        where x.Id == id
                        select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.OrderStatusTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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
        public List<System.Web.Mvc.SelectListItem> GetAllOrderStatusSelectList()
        {
            var entities = Db.OrderStatusTbls.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}