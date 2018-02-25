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
    public class BannersApiController : ApiController
    {
        private readonly IBannerAccessRepository<Banner, int> _repository;

        public BannersApiController(IBannerAccessRepository<Banner, int> r)
        {
            _repository = r;
        }

        [Route("api/BannersApi/")]
        // GET: api/BannersApi
        public IEnumerable<Banner> GetBanners()
        {
            return _repository.Get();
        }

        [Route("api/BannersApi/{id}")]
        // GET: api/BannersApi/5
        [ResponseType(typeof(Banner))]
        public IHttpActionResult GetBanner(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/BannersApi/{id}")]
        // PUT: api/BannersApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBanner(int id, Banner entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/BannersApi/")]
        // POST: api/BannersApi
        [ResponseType(typeof(Banner))]
        public IHttpActionResult PostBanner(Banner entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/BannersApi/{id}")]
        // DELETE: api/BannersApi/5
        [ResponseType(typeof(Banner))]
        public IHttpActionResult DeleteBanner(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        //custom

        [Route("api/BannersApi/GetAllBannersSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllBannersSelectList()
        {
            return _repository.GetAllBannersSelectList();
        }

        [Route("api/BannersApi/GetHomePageBanner/")]
        // GetAllBrandsSelectList: api/BannersApi/GetHomePageBanner
        public BannerMobile GetHomePageBanner()
        {
            return _repository.GetHomePageBanner();
        }

    }
}