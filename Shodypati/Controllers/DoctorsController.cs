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
    [Authorize(Roles = "Admin")]
    public class DoctorsController : BaseController
    {
        private readonly FilesHelper _filesHelper;
        readonly string _tempPath = "~/Doctor/";
        private readonly string _serverMapPath = "~/Content/images/Doctor/";
        private readonly string _urlBase = "/Content/images/Doctor/";
        private readonly string DeleteURL = "/Doctor/DeleteFile/?file=";

        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(_serverMapPath));
        private const string DeleteType = "GET";


        public DoctorsController()
        {
            var randN = GetRandomNumber();

            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + randN + "/", _urlBase + randN + "/", _tempPath + randN + "/", _serverMapPath + randN + "/");

            //api url                  
            url = baseUrl + "api/DoctorsApi";
        }

        
        // GET: Doctors
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<Doctor>>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");
        }
     
        // GET: Doctors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Doctor>(responseData);
            return View(entity);
        }
        [AllowAnonymous]
        // GET: Doctors/Create
        public async Task<ActionResult> Create()
        {
            var entity = new Doctor();
            return await CreateSub(entity);
        }

        public async Task<ActionResult> CreateSub(Doctor entity)
        {
            var allWorkingTypes = await GetAllDoctorWorkingTypes();
            entity.AllWorkingSelectListItems = allWorkingTypes;
            entity.CanVisitDays = GetAllWeekDaysName();
            return View("Create", entity);
        }

        public async Task<ActionResult> EditSub(Doctor entity)
        {
            var allWorkingTypes = await GetAllDoctorWorkingTypes();
            List<SelectListItem> workingItems = new List<SelectListItem>();
            foreach (var item in allWorkingTypes)
            {
                workingItems.Add(new SelectListItem()
                {
                    Value = item.Value,
                    Text = item.Text,
                    Selected = item.Value == entity.SelectedDoctorWorkingTypeId
                });

            }
            entity.CanVisitDays = GetAllWeekDaysName();
            entity.AllWorkingSelectListItems = workingItems;
            return View("Edit", entity);
        }

        [AllowAnonymous]
        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Doctor entity)
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
            return await CreateSub(entity);
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

     
        // GET: Doctors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Doctor>(responseData);

            return await EditSub(entity);
        }


    

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Doctor entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return await EditSub(entity);          
        }
     
        // GET: Doctors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<Doctor>(responseData);
            return View(entity);
        }
  
        // POST: Doctors/Delete/5
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