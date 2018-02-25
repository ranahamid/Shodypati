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
    public class CampaignProductsApiController : ApiController
    {

        private ICampaignProductAccessRepository<CampaignProducts, int> _repository;

        public CampaignProductsApiController(ICampaignProductAccessRepository<CampaignProducts, int> r)
        {
            _repository = r;
        }

        [Route("api/CampaignProductsApi/")]
        // GET: api/CampaignProductsApi
        public IEnumerable<CampaignProducts> GetCampaignProducts()
        {
            return _repository.Get();
        }

        [Route("api/CampaignProductsApi/{id}")]
        // GET: api/CampaignProductsApi/5
        [ResponseType(typeof(CampaignProducts))]
        public IHttpActionResult GetCampaignProducts(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/CampaignProductsApi/")]
        // PUT: api/CampaignProductsApi
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCampaignProducts(CampaignProducts entity)
        {
            _repository.Put(entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        //[Route("api/CampaignProductsApi/")]
        //// POST: api/CampaignProductsApi
        //[ResponseType(typeof(CampaignProducts))]
        //public IHttpActionResult PostCampaignProducts(CampaignProducts entity)
        //{
        //    _repository.Post(entity);
        //    return Ok(entity);
        //}


        //[Route("api/CampaignProductsApi/{id}")]
        //// DELETE: api/CampaignProductsApi/5
        //[ResponseType(typeof(CampaignProducts))]
        //public IHttpActionResult DeleteCampaignProducts(int id)
        //{
        //    _repository.Delete(id);
        //    return StatusCode(HttpStatusCode.NoContent);
        //}
        ////custom api 
        [Route("api/CampaignProductsApi/GetAllCampaignProductsStringList/")]
        // GetAllCampaignProductsStringList: api/CampaignProductsApi/GetAllCampaignProductsStringList
        public List<string> GetAllCampaignProductsStringList()
        {
            return _repository.GetAllCampaignProductsList();
        }


    }
}