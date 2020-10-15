using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class MerchantsApiController : ApiController
    {
        private readonly IMerchantAccessRepository<Merchant, int> _repository;

        public MerchantsApiController(IMerchantAccessRepository<Merchant, int> r)
        {
            _repository = r;
        }

        [System.Web.Http.Route("api/MerchantsApi/")]
        // GET: api/MerchantsApi
        public IEnumerable<Merchant> GetMerchants()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/MerchantsApi/{id}")]
        // GET: api/MerchantsApi/5
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult GetMerchant(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/MerchantsApi/{id}")]
        // PUT: api/MerchantsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMerchant(int id, Merchant entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/MerchantsApi/")]
        // POST: api/MerchantsApi
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult PostMerchant(Merchant entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/MerchantsApi/{id}")]
        // DELETE: api/MerchantsApi/5
        [ResponseType(typeof(Merchant))]
        public IHttpActionResult DeleteMerchant(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        ////custom api 
        [System.Web.Http.Route("api/MerchantsApi/GetAllMerchantsSelectList/")]
        // GetAllCategoriesSelectList: api/CategoriesApi/GetAllCategoriesSelectList
        public List<SelectListItem> GetAllCategoriesSelectList()
        {
            return _repository.GetAllMerchantsSelectList();
        }
    }
}