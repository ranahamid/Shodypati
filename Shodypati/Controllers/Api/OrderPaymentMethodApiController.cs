using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class OrderPaymentMethodApiController : ApiController
    {
        private readonly IOrderPaymentMethodAccessRepository<OrderPaymentMethod, int> _repository;

        public OrderPaymentMethodApiController(IOrderPaymentMethodAccessRepository<OrderPaymentMethod, int> r)
        {
            _repository = r;
        }


        [System.Web.Http.Route("api/OrderPaymentMethodApi/")]
        public IEnumerable<OrderPaymentMethod> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderPaymentMethod entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/OrderPaymentMethodApi/")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult PostOrderPaymentMethod(OrderPaymentMethod entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [System.Web.Http.Route("api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList
        public List<SelectListItem> GetAllOrderPaymentMethodSelectList()
        {
            return _repository.GetAllOrderPaymentMethodSelectList();
        }
    }
}