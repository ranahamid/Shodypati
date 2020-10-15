using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class CategoriesApiController : ApiController
    {
        private readonly ICategoryAccessRepository<Category, int> _repository;

        public CategoriesApiController(ICategoryAccessRepository<Category, int> r)
        {
            _repository = r;
        }

        [System.Web.Http.Route("api/CategoriesApi/")]
        // GET: api/CategoriesApi
        public IEnumerable<Category> Get()
        {
            return _repository.Get();
        }


        [System.Web.Http.Route("api/CategoriesApi/{id}")]
        // GET: api/CategoriesApi/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult Get(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [System.Web.Http.Route("api/CategoriesApi/{id}")]
        // PUT: api/CategoriesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Category category)
        {
            _repository.Put(id, category);
            return StatusCode(HttpStatusCode.NoContent);
        }


        [System.Web.Http.Route("api/CategoriesApi/")]
        // POST: api/CategoriesApi
        [ResponseType(typeof(Category))]
        public IHttpActionResult Post(Category category)
        {
            _repository.Post(category);
            return Ok(category);
        }


        [System.Web.Http.Route("api/CategoriesApi/{id}")]
        // DELETE: api/CategoriesApi/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult Delete(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [System.Web.Http.Route("api/CategoriesApi/GetAllCategoriesSelectList/")]
        // GetAllCategoriesSelectList: api/CategoriesApi/GetAllCategoriesSelectList
        public List<SelectListItem> GetAllCategoriesSelectList()
        {
            return _repository.GetAllCategoriesSelectList();
        }
    }
}