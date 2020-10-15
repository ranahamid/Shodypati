using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class BannersApiController : ApiController
    {
        private readonly IBannerAccessRepository<Banner, int> _repository;

        public BannersApiController(IBannerAccessRepository<Banner, int> r)
        {
            _repository = r;
        }

        [System.Web.Http.Route("api/BannersApi/")]
        // GET: api/BannersApi
        public IEnumerable<Banner> GetBanners()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/BannersApi/{id}")]
        // GET: api/BannersApi/5
        [ResponseType(typeof(Banner))]
        public IHttpActionResult GetBanner(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/BannersApi/{id}")]
        // PUT: api/BannersApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBanner(int id, Banner entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/BannersApi/")]
        // POST: api/BannersApi
        [ResponseType(typeof(Banner))]
        public IHttpActionResult PostBanner(Banner entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/BannersApi/{id}")]
        // DELETE: api/BannersApi/5
        [ResponseType(typeof(Banner))]
        public IHttpActionResult DeleteBanner(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        //custom

        [System.Web.Http.Route("api/BannersApi/GetAllBannersSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList
        public List<SelectListItem> GetAllBannersSelectList()
        {
            return _repository.GetAllBannersSelectList();
        }

        [System.Web.Http.Route("api/BannersApi/GetHomePageBanner/")]
        // GetAllBrandsSelectList: api/BannersApi/GetHomePageBanner
        public BannerMobile GetHomePageBanner()
        {
            return _repository.GetHomePageBanner();
        }
    }
}