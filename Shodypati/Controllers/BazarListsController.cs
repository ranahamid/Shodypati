using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shodypati.Filters;
using Shodypati.Helpers;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [ExceptionHandler]
    public class BazarListsController : BaseController
    {
        private const string DeleteType = "GET";
        private readonly FilesHelper _filesHelper;
        private readonly string _serverMapPath = "~/Content/images/BazarList/";
        private readonly string _tempPath = "~/BazarList/";
        private readonly string _urlBase = "/Content/images/BazarList/";
        private readonly string DeleteURL = "/BazarList/DeleteFile/?file=";


        public BazarListsController()
        {
            var randN = GetRandomNumber();

            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + randN + "/", _urlBase + randN + "/",
                _tempPath + randN + "/", _serverMapPath + randN + "/");

            //api url                  
            url = baseUrl + "api/BazarListApi";
        }

        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(_serverMapPath));

        [Authorize(Roles = "Admin")]
        // GET: BazarLists
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<BazarList>>(responseData);
                return View(entity);
            }

            throw new Exception("Exception");
        }

        [Authorize(Roles = "Admin")]
        // GET: BazarLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<BazarList>(responseData);
            return View(entity);
        }

        [AllowAnonymous]
        // GET: BazarLists/Create
        public ActionResult Create()
        {
            var entity = new BazarList();
            return View();
        }

        [AllowAnonymous]
        // POST: BazarLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BazarList entity)
        {
            if (ModelState.IsValid)
            {
                //end parent name
                var responseMessage = await client.PostAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Success");
            }

            return View(entity);
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            var currentContext = HttpContext;
            _filesHelper.UploadAndShowResults(currentContext, resultList);
            var files = new JsonFiles(resultList);

            var isEmpty = !resultList.Any();
            return isEmpty ? Json("Error ") : Json(files);
        }

        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            _filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        // GET: BazarLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<BazarList>(responseData);
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        // POST: BazarLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BazarList entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            }

            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        // GET: BazarLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<BazarList>(responseData);
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        // POST: BazarLists/Delete/5
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