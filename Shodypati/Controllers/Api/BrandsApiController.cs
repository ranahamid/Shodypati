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
    public class BrandsApiController : ApiController
    {
        private IBrandAccessRepository<Brand, int> _repository;


        public BrandsApiController(IBrandAccessRepository<Brand, int> r)
        {
            _repository = r;
        }


        [Route("api/BrandsApi/")]
        // GET: api/BrandsApi
        public IEnumerable<Brand> GetBrands()
        {
            return _repository.Get();
        }

        [Route("api/BrandsApi/{id}")]
        // GET: api/BrandsApi/5
        [ResponseType(typeof(Brand))]
        public IHttpActionResult Get(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [Route("api/BrandsApi/{id}")]
        // PUT: api/BrandsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBrand(int id, Brand entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/BrandsApi/")]
        // POST: api/BrandsApi
        [ResponseType(typeof(Brand))]
        public IHttpActionResult PostBrand(Brand entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/BrandsApi/{id}")]
        // DELETE: api/BrandsApi/5
        [ResponseType(typeof(Brand))]
        public IHttpActionResult DeleteBrand(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);    
        }

        ////custom api 
        [Route("api/BrandsApi/GetAllBrandsSelectList/")]
        // GetAllBrandsSelectList: api/BrandsApi/GetAllBrandsSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllCategoriesSelectList()
        {
            return _repository.GetAllBrandsSelectList();
        }

    }
}