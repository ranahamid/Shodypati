using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class ProductsApiController : ApiController
    {
        private readonly IProductAccessRepository<Product, int> _repository;


        public ProductsApiController(IProductAccessRepository<Product, int> r)
        {
            _repository = r;
        }

        [System.Web.Http.Route("api/ProductsApi")]
        // GET: api/ProductsApi
        public IEnumerable<Product> GetProducts()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/ProductsApi/{id}")]
        // GET: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [System.Web.Http.Route("api/ProductsApi/{id}")]
        // PUT: api/ProductsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            _repository.Put(id, product);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/ProductsApi/")]
        // POST: api/ProductsApi
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            _repository.Post(product);
            return Ok(product);
        }

        [System.Web.Http.Route("api/ProductsApi/{id}")]
        // DELETE: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [System.Web.Http.Route("api/ProductsApi/GetAllProductsSelectList/")]
        // GetParentCategory: api/ProductsApi/GetAllCategoriesSelectList
        public List<SelectListItem> GetAllProductsSelectList()
        {
            return _repository.GetAllProductsSelectList();
        }

        ////custom api 
        [System.Web.Http.Route("api/ProductsApi/GetProductsByCategories/")]
        // GetParentCategory: api/ProductsApi/GetAllCategoriesSelectList
        public List<CategoryMobile> GetProductsByCategoriesList()
        {
            return _repository.GetProductsByCategoriesList();
        }

        [System.Web.Http.Route("api/ProductsApi/GetProductsByCategoriesListWeb/")]
        public List<CategoryMobile> GetProductsByCategoriesListWeb()
        {
            return _repository.GetProductsByCategoriesListWeb();
        }

        ////custom api 
        [System.Web.Http.Route("api/ProductsApi/GetProductsBy/{CategoryId}/{MerchantId}")]
        // GetProductsBy: api/ProductsApi/GetProductsBy
        public List<ProductMobile> GetProductsBy(int categoryid, int merchantid)
        {
            return _repository.GetProductsBy(categoryid, merchantid);
        }
    }
}