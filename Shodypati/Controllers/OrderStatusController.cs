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
    [Authorize(Roles = "Admin")]
    [ExceptionHandler]
    public class OrderStatusController : BaseController
    {
        public OrderStatusController()
        {
            //api url                  
            url = baseUrl + "api/OrderStatusApi";
        }

        // GET: OrderStatuss
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<List<OrderStatus>>(responseData);
            return View(entity);
        }

        // GET: OrderStatuss/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<OrderStatus>(responseData);
            return View(entity);
        }

        // GET: OrderStatuss/Create
        public ActionResult Create()
        {
            var entity = new OrderStatus();
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderStatus entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PostAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            }

            return View(entity);
        }

        // GET: OrderStatuss/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<OrderStatus>(responseData);
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderStatus entity)
        {
            if (!ModelState.IsValid) return View(entity);
            var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            return View(entity);
        }


        // GET: OrderStatuss/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<OrderStatus>(responseData);
            return View(entity);
        }

        // POST: OrderStatuss/Delete/5
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