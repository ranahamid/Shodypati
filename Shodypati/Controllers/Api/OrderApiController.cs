using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class OrderApiController : ApiController
    {
        private readonly IOrderAccessRepository<Orders, int> _repository;

        public OrderApiController(IOrderAccessRepository<Orders, int> r)
        {
            _repository = r;
        }


        public ApplicationUserManager UserManager =>
            HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ApplicationSignInManager SignInManager =>
            HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();


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
            if (item == null) return NotFound();
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
        public async Task<IHttpActionResult> PostOrder( /*Orders entity*/)
        {
            var responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<Orders>(responseData);

            //_repository.Post(entity);
            //return Ok(entity);

            var item = _repository.Post(entity, UserManager);
            if (item == null) return NotFound();
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