using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class OrderPaymentStatusApiController : ApiController
    {
        private readonly IOrderPaymentStatusAccessRepository<OrderPaymentStatus, int> _repository;

        public OrderPaymentStatusApiController(IOrderPaymentStatusAccessRepository<OrderPaymentStatus, int> r)
        {
            _repository = r;
        }


        [System.Web.Http.Route("api/OrderPaymentStatusApi/")]
        public IEnumerable<OrderPaymentStatus> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/OrderPaymentStatusApi/{id}")]
        [ResponseType(typeof(OrderPaymentStatus))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/OrderPaymentStatusApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderPaymentStatus entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/OrderPaymentStatusApi/")]
        [ResponseType(typeof(OrderPaymentStatus))]
        public IHttpActionResult PostOrderPaymentMethod(OrderPaymentStatus entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/OrderPaymentStatusApi/{id}")]
        [ResponseType(typeof(OrderPaymentStatus))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        ////custom api 
        [System.Web.Http.Route("api/OrderPaymentStatusApi/GetAllOrderPaymentStatusSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentStatusApi/GetAllOrderPaymentStatusSelectList
        public List<SelectListItem> GetAllOrderPaymentStatusSelectList()
        {
            return _repository.GetAllOrderPaymentStatusSelectList();
        }
    }
}