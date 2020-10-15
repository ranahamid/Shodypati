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
    public class OrderPaymentMethodDataAccessRepository : BaseController,
        IOrderPaymentMethodAccessRepository<OrderPaymentMethod, int>
    {
        public IEnumerable<OrderPaymentMethod> Get()
        {
            var entities = new List<OrderPaymentMethod>();

            entities = Db.OrderPaymentMethodTbls.Select(x => new OrderPaymentMethod
            {
                Id = x.Id,
                Name = x.Name,
                Instructions = x.Instructions,

                InstructionsImageUrl = !string.IsNullOrEmpty(x.InstructionsImageUrl)
                    ? HttpUtility.UrlPathEncode(baseUrl + x.InstructionsImageUrl)
                    : string.Empty,
                RawDbImagePath = x.InstructionsImageUrl,
                Published = x.Published
            }).ToList();

            return entities;
        }

        public OrderPaymentMethod Get(int id)
        {
            var entity = Db.OrderPaymentMethodTbls.Where(x => x.Id == id).Select(x => new OrderPaymentMethod
            {
                Id = x.Id,
                Name = x.Name,
                Instructions = x.Instructions,
                InstructionsImageUrl = !string.IsNullOrEmpty(x.InstructionsImageUrl)
                    ? HttpUtility.UrlPathEncode(baseUrl + x.InstructionsImageUrl)
                    : string.Empty,
                RawDbImagePath = x.InstructionsImageUrl,
                Published = x.Published
            }).SingleOrDefault();

            return entity;
        }

        public void Post(OrderPaymentMethod entity)
        {
            var imgAddress = string.Empty;
            if (entity.InstructionsImageUrl != null) imgAddress = entity.InstructionsImageUrl.TrimStart('/');

            Db.OrderPaymentMethodTbls.InsertOnSubmit(new OrderPaymentMethodTbl
            {
                //   Id              = entity.Id,           
                Name = entity.Name,
                Instructions = entity.Instructions,
                InstructionsImageUrl = imgAddress,
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

        public void Put(int id, OrderPaymentMethod entity)
        {
            var imgAddress = string.Empty;
            if (entity.RawDbImagePath != null) imgAddress = entity.RawDbImagePath.TrimStart('/');

            var isEntity = from x in Db.OrderPaymentMethodTbls
                where x.Id == entity.Id
                select x;

            var entitySingle = isEntity.Single();

            entitySingle.Name = entity.Name;
            entitySingle.Instructions = entity.Instructions;
            entitySingle.InstructionsImageUrl = imgAddress;
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
            var query = from x in Db.OrderPaymentMethodTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.OrderPaymentMethodTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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
        public List<SelectListItem> GetAllOrderPaymentMethodSelectList()
        {
            var entities = Db.OrderPaymentMethodTbls.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
                // Selected = (item.Value.ToLower() == entity..ToString().ToLower()) ? true : false
            }).ToList();

            return entities;
        }
    }
}