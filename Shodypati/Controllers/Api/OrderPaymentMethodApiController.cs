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
    public class OrderPaymentMethodApiController : ApiController
    {
        private IOrderPaymentMethodAccessRepository<OrderPaymentMethod, int> _repository;

        public OrderPaymentMethodApiController(IOrderPaymentMethodAccessRepository<OrderPaymentMethod, int> r)
        {
            _repository = r;
        }


        [Route("api/OrderPaymentMethodApi/")]
        public IEnumerable<OrderPaymentMethod> GetOrderPaymentMethods()
        {
            return _repository.Get();
        }

        [Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult GetOrderPaymentMethod(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderPaymentMethod(int id, OrderPaymentMethod entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/OrderPaymentMethodApi/")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult PostOrderPaymentMethod(OrderPaymentMethod entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/OrderPaymentMethodApi/{id}")]
        [ResponseType(typeof(OrderPaymentMethod))]
        public IHttpActionResult DeleteOrderPaymentMethod(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [Route("api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllOrderPaymentMethodSelectList()
        {
            return _repository.GetAllOrderPaymentMethodSelectList();
        }
    }
}
