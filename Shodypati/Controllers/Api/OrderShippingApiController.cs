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
    public class OrderShippingApiController : ApiController
    {
        private IOrderShippingAccessRepository<OrderShipping, int> _repository;

        public OrderShippingApiController(IOrderShippingAccessRepository<OrderShipping, int> r)
        {
            _repository = r;
        }


        [Route("api/OrderShippingApi/")]
        public IEnumerable<OrderShipping> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderShipping entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/OrderShippingApi/")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult PostOrderPaymentMethod(OrderShipping entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/OrderShippingApi/{id}")]
        [ResponseType(typeof(OrderShipping))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        ////custom api 
        [Route("api/OrderShippingApi/GetAllOrderShippingSelectList/")]
        // GetAllBrandsSelectList: api/OrderShippingApi/GetAllOrderShippingSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllOrderShippingSelectList()
        {
            return _repository.GetAllOrderShippingSelectList();
        }
    }
}
