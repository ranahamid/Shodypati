using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : BaseController
    {
        public ShodypatiDataContext db;

        public LogsController()
        {
            //api url                  
            url = baseUrl + "api/LogApi";
            db = new ShodypatiDataContext();
        }


        // GET: Logs
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<List<Log>>(responseData);
            return View(entity);
        }

        // GET: Logs/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Log>(responseData);
            return View(entity);
        }

        // GET: Logs/Create
        public ActionResult Create()
        {
            return null;
        }

        // POST: Logs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Log log)
        {
            if (ModelState.IsValid) throw new Exception("Exception");
            throw new Exception("Exception");
        }

        // GET: Logs/Edit/5
        public ActionResult Edit(int id)
        {
            throw new Exception("Exception");
        }

        // POST: Logs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Log log)
        {
            if (ModelState.IsValid) throw new Exception("Exception");
            throw new Exception("Exception");
        }

        // GET: Logs/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Log>(responseData);
            return View(entity);
        }


        // POST: Logs/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var responseMessage = await client.DeleteAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            throw new Exception("Exception");
        }

        public ActionResult DeleteAllLog()
        {
            var q = from x in db.LogTbls
                select x;

            foreach (var item in q) db.LogTbls.DeleteOnSubmit(item);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) Db.Dispose();
            base.Dispose(disposing);
        }
    }
}