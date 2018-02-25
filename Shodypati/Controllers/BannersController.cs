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

using Shodypati.Helpers;
using System.IO;
using System.Web.Hosting;


namespace Shodypati.Controllers
{
    [ExceptionHandler]
    [Authorize(Roles = "Admin")]
    public class BannersController : BaseController
    {
        private readonly FilesHelper _filesHelper;
        String tempPath = "~/banners/";
        String serverMapPath = "~/Content/images/banners/";
        private string UrlBase = "/Content/images/banners/"; //with out '/'
        String DeleteURL = "/banners/DeleteAdditionalFile/?file=";
        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(serverMapPath));
        String DeleteType = "GET";

        public BannersController()
        {
            int randN = GetRandomNumber();

            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + randN + "/", UrlBase + randN + "/", tempPath + randN + "/", serverMapPath + randN + "/");

            //api url                  
            url = baseUrl + "api/BannersApi";
        }
        // GET: Banners
        public async Task<ActionResult> Index()
        {

            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<Banner>>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }



        // GET: Banners/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<Banner>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }




        // GET: Banners/SetBanner
        public async Task<ActionResult> SetBanner()
        {
            BannerSelectList banner = new BannerSelectList
            {
                AllBanerItems = await GetAllBannerList()
            };       
            return View(banner);
        }


        public BannerTbl SetisHomePage(IQueryable<BannerTbl> entity, bool setValue)
        {
            var entitySingle = entity.SingleOrDefault();
            if (entitySingle == null) return null;
            entitySingle.IsHomePageBanner = setValue;
            entitySingle.UpdatedOnUtc = DateTime.Now;
            return entitySingle;
        }

        // POST: Products/SetBanner
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetBanner(BannerSelectList entity)
        {
            if (ModelState.IsValid)
            {
                //set previous to 0
                var preEntity = from x in Db.BannerTbls
                                where x.IsHomePageBanner == true
                                select x;
                SetisHomePage(preEntity, false);


                //current
                var isEntity = from x in Db.BannerTbls
                               where x.Id.ToString() == entity.SelectedBanner
                               select x;
                SetisHomePage(isEntity, true);

                try
                {
                    Db.SubmitChanges();
                }
                catch (Exception)
                {
                    throw new Exception("Exception");
                }
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        // GET: Banners/Create
        public ActionResult Create()
        {
            Banner entity = new Banner {GuidId = Guid.NewGuid()};
            return View(entity);
        }

        // POST: Banners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Banner entity)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(entity);
        }


        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();

            var currentContext = HttpContext;

            _filesHelper.UploadAndShowResults(currentContext, resultList);
            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
            }
        }
        [HttpPost]
        public JsonResult UploadAdditional()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            var currentContext = HttpContext;
           

            _filesHelper.UploadAndShowResults(currentContext, resultList);
            //save to db
            try
            {
                var bannerId = Guid.Parse(Request.Form["GuidId"]);
                foreach (var item in resultList)
                {
                    string fileUrl = item.url;
                    Db.BannerImageTbls.InsertOnSubmit(new BannerImageTbl
                    {
                        // Id = GuidId,
                        BannerGuidId = bannerId,
                        ImagePath = fileUrl.TrimStart('/'),
                        Description = null,
                        DisplayOrder = 1,
                    });
                }
                Db.SubmitChanges();
            }
           
            catch (Exception)
            {
                throw new Exception("Exception");
            }

            //end
            var files = new JsonFiles(resultList);

            var isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }

            return Json(files);
        }

        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            _filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteAdditionalFile(string file)
        {
            _filesHelper.DeleteFile(file);
            //delete from db
            var bannerId = Guid.Parse(Request.Form["GuidId"]);
            var img = "Content/images/banner/"+file;
            var query = from x in Db.BannerImageTbls
                        where x.BannerGuidId == bannerId && 
                              x.ImagePath== img                              
                        select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.BannerImageTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
            //
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        // GET: Banners/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<Banner>(responseData);

                return View(entity);
            }
            throw new Exception("Exception");
        }

        // POST: Banners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Banner entity)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            throw new Exception("Exception");
        }

        // GET: Banners/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<Banner>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage responseMessage = await client.DeleteAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            throw new Exception("Exception");
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
