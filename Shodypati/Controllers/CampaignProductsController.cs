using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shodypati.Filters;
using Shodypati.Models;

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
            var responseMessage = await client.GetAsync(url);
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
            var entity = new CampaignProducts();
            entity.AllProductsSelectList = await GetAllProductsSelectedList();
            entity.AllProductsSelectListStr = await GetAllCampaignProductsStringList();
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CampaignProducts entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PutAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            }

            entity.AllProductsSelectList = await GetAllProductsSelectedList();
            entity.AllProductsSelectListStr = await GetAllCampaignProductsStringList();
            return View(entity);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) Db.Dispose();
            base.Dispose(disposing);
        }
    }
}