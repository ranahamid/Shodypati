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
    public class CategoriesController : BaseController
    {
        private readonly FilesHelper _filesHelper;
        String tempPath = "~/categories/";
        String serverMapPath = "~/Content/images/categories/";
        private string UrlBase = "/Content/images/categories/";
        String DeleteURL = "/categories/DeleteFile/?file=";
        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(serverMapPath));
        String DeleteType = "GET";


        public CategoriesController()
        {
            int randN = GetRandomNumber();

            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot+randN+"/", UrlBase + randN + "/", tempPath + randN + "/", serverMapPath + randN + "/");
            //api url                  
            url = baseUrl + "api/CategoriesApi";
        }


        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<Category>>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Category>(responseData);
            return View(entity);
        }


        // GET: Categories/Create
        public ActionResult Create()
        {
            var entity = new Category
            {
                Categories = Categories,
                AllCategories = AllCategories,
                ChildCategories = ChildCategories
            };

            return View(entity);
        }

        // POST: Categories/Create
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category entity)
        {
            if (ModelState.IsValid)
            {          
                //end parent name
                var responseMessage = await client.PostAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            entity.Categories = Categories;
            entity.AllCategories = AllCategories;
            entity.ChildCategories = ChildCategories;       
            return View(entity);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();

            var currentContext = HttpContext;
           
            _filesHelper.UploadAndShowResults(currentContext, resultList);
            //save to db
         
            //end
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

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Category>(responseData);

            //if worked , removed these
            //set selected and remove this id based item
            entity.Categories = Categories;
            entity.ChildCategories = ChildCategories;
            entity.AllCategories = AllCategories;
            if (AllCategories!=null && entity.Parent1Id != null  && entity.Parent1Id!=0)
                entity.AllCategories = SetSelectedItem(AllCategories, entity.Parent1Id);
            //end -set selected and remove this id based item

            return View(entity);
        }



        // POST: Categories/Edit/5
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category entity)
        {
            if (ModelState.IsValid)
            {                
                var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            entity.Categories = Categories;
            entity.ChildCategories = ChildCategories;

            //set selected and remove this id based item
            entity.AllCategories = AllCategories;
            if (AllCategories == null || entity.Parent1Id == null || entity.Parent1Id == 0) return View(entity);
            entity.AllCategories = SetSelectedItem(AllCategories, entity.Parent1Id);
            //end -set selected and remove this id based item
            return View(entity);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Category>(responseData);
            return View(entity);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var responseMessage = await client.DeleteAsync(url + "/" + id);
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
