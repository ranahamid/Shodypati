using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shodypati.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Shodypati.DAL;
using System.Configuration;
using Shodypati.Filters;

namespace Shodypati.Controllers
{
    [ExceptionHandler]
    [Authorize(Roles = "Admin")]
    public class CampaignProductsController : BaseController
    { 

        public CampaignProductsController()
        {
            //api url                  
            url = baseUrl + "api/CampaignProductsApi";
        }

        // GET: CampaignProducts
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<CampaignProducts>>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }



        // GET: CampaignProducts/Edit/5
        public async Task<ActionResult> Edit()
        {
            CampaignProducts entity = new CampaignProducts();
            entity.AllProductsSelectList = await GetAllProductsSelectedList();
            entity.AllProductsSelectListStr = await GetAllCampaignProductsStringList();
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( CampaignProducts entity)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            entity.AllProductsSelectList = await GetAllProductsSelectedList();
            entity.AllProductsSelectListStr = await GetAllCampaignProductsStringList();
            return View(entity);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
