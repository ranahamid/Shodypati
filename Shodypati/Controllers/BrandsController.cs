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
    public class BrandsController : BaseController
    {
        public BrandsController()
        {
            //api url                  
            url = baseUrl + "api/BrandsApi";
        }

        // GET: Brands
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var entity = JsonConvert.DeserializeObject<List<Brand>>(responseData);

                return View(entity);
            }

            throw new Exception("Exception");
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var entity = JsonConvert.DeserializeObject<Brand>(responseData);

                return View(entity);
            }

            throw new Exception("Exception");
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            var entity = new Brand();

            return View(entity);
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Brand entity)
        {
            if (!ModelState.IsValid) return View(entity);
            var responseMessage = await client.PostAsJsonAsync(url, entity);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            return View(entity);
        }

        // GET: Brands/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Brand>(responseData);
            return View(entity);
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Brand entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            }

            throw new Exception("Exception");
        }

        // GET: Brands/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Brand>(responseData);
            return View(entity);
        }

        // POST: Brands/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var responseMessage = await client.DeleteAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            throw new Exception("Exception");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) Db.Dispose();
            base.Dispose(disposing);
        }
    }
}