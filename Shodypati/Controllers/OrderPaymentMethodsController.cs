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
    [Authorize(Roles = "Admin")]
    [ExceptionHandler]
    public class OrderPaymentMethodsController : BaseController
    {
        private readonly FilesHelper _filesHelper;
        private readonly string DeleteURL = "/OrderPaymentMethods/DeleteAdditionalFile/?file=";
        private readonly string serverMapPath = "~/Content/images/OrderPaymentMethods/";
        private readonly string tempPath = "~/OrderPaymentMethods/";
        private readonly string UrlBase = "/Content/images/OrderPaymentMethods/"; //with out '/'
        private readonly string DeleteType = "GET";

        public OrderPaymentMethodsController()
        {
            var randN = GetRandomNumber();
            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + randN + "/", UrlBase + randN + "/",
                tempPath + randN + "/", serverMapPath + randN + "/");

            //api url                  
            url = baseUrl + "api/OrderPaymentMethodApi";
        }

        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(serverMapPath));


        // GET: OrderPaymentMethods
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<List<OrderPaymentMethod>>(responseData);
            return View(entity);
        }

        // GET: OrderPaymentMethods/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<OrderPaymentMethod>(responseData);
            return View(entity);
        }

        // GET: OrderPaymentMethods/Create
        public ActionResult Create()
        {
            var entity = new OrderPaymentMethod();
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderPaymentMethod entity)
        {
            if (!ModelState.IsValid) return View(entity);
            var responseMessage = await client.PostAsJsonAsync(url, entity);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");

            return View(entity);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();

            var currentContext = HttpContext;
            _filesHelper.UploadAndShowResults(currentContext, resultList);
            var files = new JsonFiles(resultList);

            var isEmpty = !resultList.Any();
            if (isEmpty) return Json("Error ");

            return Json(files);
        }

        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            _filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // GET: OrderPaymentMethods/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<OrderPaymentMethod>(responseData);
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderPaymentMethod entity)
        {
            if (!ModelState.IsValid) return View(entity);
            var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
            if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index");
            return View(entity);
        }


        // GET: OrderPaymentMethods/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var entity = JsonConvert.DeserializeObject<OrderPaymentMethod>(responseData);

            return View(entity);
        }

        // POST: OrderPaymentMethods/Delete/5
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