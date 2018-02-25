using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Shodypati.Models;
using Shodypati.DAL;

namespace Shodypati.Controllers.Api
{
    public class OrderStatusApiController : ApiController
    {
        private IOrderStatusAccessRepository<OrderStatus, int> _repository;

        public OrderStatusApiController(IOrderStatusAccessRepository<OrderStatus, int> r)
        {
            _repository = r;
        }


        [Route("api/OrderStatusApi/")]
        public IEnumerable<OrderStatus> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderStatus entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/OrderStatusApi/")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult PostOrderPaymentMethod(OrderStatus entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [Route("api/OrderStatusApi/GetAllOrderStatusSelectList/")]
        // GetAllBrandsSelectList: api/OrderStatusApi/GetAllOrderStatusSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllOrderStatusSelectList()
        {
            return _repository.GetAllOrderStatusSelectList();
        }
    }
}
