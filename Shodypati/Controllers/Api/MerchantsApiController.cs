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
    public class MerchantsApiController : ApiController
    {
        private IMerchantAccessRepository<Merchant, int> _repository;

        public MerchantsApiController(IMerchantAccessRepository<Merchant, int> r)
        {
            _repository = r;
        }

        [Route("api/MerchantsApi/")]
        // GET: api/MerchantsApi
        public IEnumerable<Merchant> GetMerchants()
        {
            return _repository.Get();
        }

        [Route("api/MerchantsApi/{id}")]
        // GET: api/MerchantsApi/5
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult GetMerchant(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/MerchantsApi/{id}")]
        // PUT: api/MerchantsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMerchant(int id, Merchant entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/MerchantsApi/")]
        // POST: api/MerchantsApi
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult PostMerchant(Merchant entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/MerchantsApi/{id}")]
        // DELETE: api/MerchantsApi/5
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult DeleteMerchant(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        ////custom api 
        [Route("api/MerchantsApi/GetAllMerchantsSelectList/")]
        // GetAllCategoriesSelectList: api/CategoriesApi/GetAllCategoriesSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllCategoriesSelectList()
        {
            return _repository.GetAllMerchantsSelectList();
        }
    }
}