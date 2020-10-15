using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class BrandsApiController : ApiController
    {
        private readonly IBrandAccessRepository<Brand, int> _repository;


        public BrandsApiController(IBrandAccessRepository<Brand, int> r)
        {
            _repository = r;
        }


        [System.Web.Http.Route("api/BrandsApi/")]
        // GET: api/BrandsApi
        public IEnumerable<Brand> GetBrands()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/BrandsApi/{id}")]
        // GET: api/BrandsApi/5
        [ResponseType(typeof(Brand))]
        public IHttpActionResult Get(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [System.Web.Http.Route("api/BrandsApi/{id}")]
        // PUT: api/BrandsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBrand(int id, Brand entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/BrandsApi/")]
        // POST: api/BrandsApi
        [ResponseType(typeof(Brand))]
        public IHttpActionResult PostBrand(Brand entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/BrandsApi/{id}")]
        // DELETE: api/BrandsApi/5
        [ResponseType(typeof(Brand))]
        public IHttpActionResult DeleteBrand(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [System.Web.Http.Route("api/BrandsApi/GetAllBrandsSelectList/")]
        // GetAllBrandsSelectList: api/BrandsApi/GetAllBrandsSelectList
        public List<SelectListItem> GetAllCategoriesSelectList()
        {
            return _repository.GetAllBrandsSelectList();
        }
    }
}