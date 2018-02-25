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
    public class ProductsApiController : ApiController
    {
        private IProductAccessRepository<Product, int> _repository;
   

        public ProductsApiController(IProductAccessRepository<Product, int> r)
        {
            _repository = r;
        }

        [Route("api/ProductsApi")]
        // GET: api/ProductsApi
        public IEnumerable<Product> GetProducts()
        {
            return _repository.Get();
        }

        [Route("api/ProductsApi/{id}")]
        // GET: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [Route("api/ProductsApi/{id}")]
        // PUT: api/ProductsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            _repository.Put(id, product);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/ProductsApi/")]
        // POST: api/ProductsApi
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            _repository.Post(product);
            return Ok(product);
        }

        [Route("api/ProductsApi/{id}")]
        // DELETE: api/ProductsApi/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        ////custom api 
        [Route("api/ProductsApi/GetAllProductsSelectList/")]
        // GetParentCategory: api/ProductsApi/GetAllCategoriesSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllProductsSelectList()
        {
            return _repository.GetAllProductsSelectList();
        }

        ////custom api 
        [Route("api/ProductsApi/GetProductsByCategories/")]
        // GetParentCategory: api/ProductsApi/GetAllCategoriesSelectList
        public List<CategoryMobile> GetProductsByCategoriesList()
        {
            return _repository.GetProductsByCategoriesList();
        }

        [Route("api/ProductsApi/GetProductsByCategoriesListWeb/")]
        public List<CategoryMobile> GetProductsByCategoriesListWeb()
        {
            return _repository.GetProductsByCategoriesListWeb();
        }

        ////custom api 
        [Route("api/ProductsApi/GetProductsBy/{CategoryId}/{MerchantId}")]
        // GetProductsBy: api/ProductsApi/GetProductsBy
        public List<ProductMobile> GetProductsBy(int categoryid, int merchantid )
        {
            return _repository.GetProductsBy(categoryid, merchantid);
        }

    

    }
}