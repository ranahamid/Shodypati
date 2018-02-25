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
    public class OrderPaymentStatusDataAccessRepository : BaseController, IOrderPaymentStatusAccessRepository<OrderPaymentStatus, int>
    {
        public IEnumerable<OrderPaymentStatus> Get()
        {
            List<OrderPaymentStatus> entities = new List<OrderPaymentStatus>();

            entities = Db.OrderPaymentStatusTbls.Select(x => new OrderPaymentStatus()
            {
                Id = x.Id,
                Name = x.Name,
                DefaultStatus = x.DefaultStatus,
            }).ToList();

            return entities;
        }

        public OrderPaymentStatus Get(int id)
        {
            OrderPaymentStatus entity = Db.OrderPaymentStatusTbls.Where(x => x.Id == id).Select(x => new OrderPaymentStatus()
            {
                Id = x.Id,
                Name = x.Name,
                DefaultStatus = x.DefaultStatus,
            }).SingleOrDefault();

            if (entity == null)
            {
                return null;
            }

            return entity;
        }

        public void Post(OrderPaymentStatus entity)
        {
            Db.OrderPaymentStatusTbls.InsertOnSubmit(new OrderPaymentStatusTbl
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

        public void Put(int id, OrderPaymentStatus entity)
        {
            var isEntity = from x in Db.OrderPaymentStatusTbls
                           where x.Id == entity.Id
                           select x;

            if (isEntity == null)
            {
                return;
            }

            if (isEntity != null)
            {
                OrderPaymentStatusTbl entitySingle = isEntity.Single();

                entitySingle.Name = entity.Name;
                entitySingle.DefaultStatus = entity.DefaultStatus;
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
            var query = from x in Db.OrderPaymentStatusTbls
                        where x.Id == id
                        select x;

            if (query != null && query.Count() == 1)
            {
                OrderPaymentStatusTbl entity = query.SingleOrDefault();
                Db.OrderPaymentStatusTbls.DeleteOnSubmit(entity);
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

        public List<System.Web.Mvc.SelectListItem> GetAllOrderPaymentStatusSelectList()
        {
            List<SelectListItem> entities = new List<SelectListItem>();

            entities = Db.OrderPaymentStatusTbls.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }

    }
}