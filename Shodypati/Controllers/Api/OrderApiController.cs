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
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Shodypati.Controllers.Api
{
    public class OrderApiController : ApiController
    {
        private IOrderAccessRepository<Orders, int> _repository;


        public ApplicationUserManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }
        public OrderApiController(IOrderAccessRepository<Orders, int> r)
        {
            _repository = r;
        }


        [Route("api/OrderApi/")]
        // GET: api/OrderApi
        public IEnumerable<OrderMobile> GetOrders()
        {
            return _repository.Get();
        }

        [Route("api/OrderApi/{id}")]
        // GET: api/OrderApi/5
        [ResponseType(typeof(OrderMobile))]
        public IHttpActionResult GetOrder(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/OrderApi/{id}")]
        // PUT: api/OrderApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Orders entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/OrderApi/")]
        // POST: api/OrderApi
        [ResponseType(typeof(OrderMobile))]
        public async Task<IHttpActionResult> PostOrder(/*Orders entity*/)
        {
            string responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<Orders>(responseData);

            //_repository.Post(entity);
            //return Ok(entity);

            var item = _repository.Post(entity, UserManager);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/OrderApi/{id}")]
        // DELETE: api/OrderApi/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult DeleteOrder(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
