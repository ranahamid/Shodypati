using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class OrderStatusApiController : ApiController
    {
        private readonly IOrderStatusAccessRepository<OrderStatus, int> _repository;

        public OrderStatusApiController(IOrderStatusAccessRepository<OrderStatus, int> r)
        {
            _repository = r;
        }


        [System.Web.Http.Route("api/OrderStatusApi/")]
        public IEnumerable<OrderStatus> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderStatus entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/OrderStatusApi/")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult PostOrderPaymentMethod(OrderStatus entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/OrderStatusApi/{id}")]
        [ResponseType(typeof(OrderStatus))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [System.Web.Http.Route("api/OrderStatusApi/GetAllOrderStatusSelectList/")]
        // GetAllBrandsSelectList: api/OrderStatusApi/GetAllOrderStatusSelectList
        public List<SelectListItem> GetAllOrderStatusSelectList()
        {
            return _repository.GetAllOrderStatusSelectList();
        }
    }
}