using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class OrderShippingApiController : ApiController
    {
        private readonly IOrderShippingAccessRepository<OrderShipping, int> _repository;

        public OrderShippingApiController(IOrderShippingAccessRepository<OrderShipping, int> r)
        {
            _repository = r;
        }


        [System.Web.Http.Route("api/OrderShippingApi/")]
        public IEnumerable<OrderShipping> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderShipping entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/OrderShippingApi/")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult PostOrderPaymentMethod(OrderShipping entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        ////custom api 
        [System.Web.Http.Route("api/OrderShippingApi/GetAllOrderShippingSelectList/")]
        // GetAllBrandsSelectList: api/OrderShippingApi/GetAllOrderShippingSelectList
        public List<SelectListItem> GetAllOrderShippingSelectList()
        {
            return _repository.GetAllOrderShippingSelectList();
        }
    }
}