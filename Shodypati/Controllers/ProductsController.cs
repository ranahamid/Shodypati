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
    public class ProductsController : BaseController
    {
        private readonly FilesHelper _filesHelper;
        readonly string tempPath = "~/products/";
        readonly string serverMapPath = "~/Content/images/products/";
        private readonly string UrlBase = "/Content/images/products/"; //with out '/'
        readonly string DeleteURL = "/products/DeleteAdditionalFile/?file=";
        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(serverMapPath));
        string DeleteType = "GET";


        public ProductsController()
        {
            int randN = GetRandomNumber();
            _filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + randN + "/", UrlBase + randN + "/", tempPath + randN + "/", serverMapPath + randN + "/");

            //api url                  
            url = baseUrl + "api/ProductsApi";
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var responseMessage = await client.GetAsync(url);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<List<Product>>(responseData);
            return View(entity);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var responseMessage = await client.GetAsync(url + "/" + id);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception("Exception");
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var entity = JsonConvert.DeserializeObject<Product>(responseData);
            if (entity != null)
            {
                //category  
                var allCategoriesSelectList = await GetAllCategoriesSelectedList();
                //end category
                //parent name                
                if (entity.CategoryId != 0 && allCategoriesSelectList != null)
                    entity.Parent1Name_English = ParentNameFromAllCategories(allCategoriesSelectList, entity.CategoryId);
                //end parent name

                //MerchantName
                if (entity.MerchantId != null && entity.MerchantId != 0)
                    entity.MerchantName = GetMarchantName(entity.MerchantId);
                //end MerchantName
                //BrandName
                if (entity.BrandId != null &&  entity.BrandId != 0)
                    entity.BrandName = GetBrandName(entity.BrandId);
                //end BrandName
                return View(entity);
            }
            throw new Exception("Exception");          
        }

   

        // GET: Products/Create
        public async Task<ActionResult> Create()
        {
            var entity = new Product();         
            //entity.Id = Guid.NewGuid();
            ViewBag.Id = entity.Id;
            //category
            entity.AllCategoriesSelectList = await GetAllCategoriesSelectedList();
            //end category
            //AllMerchantNameList
            entity.AllMerchantNameList = await GetAllMerchantNameList();
            //end AllMerchantNameList
            //AllBrandNameList
            entity.AllBrandNameList = await GetAllBrandNameList();
            //end AllBrandNameList
            return View(entity);
        }

        // POST: Products/Create
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product entity)
        {
            if (ModelState.IsValid)
            {
                var responseMessage = await client.PostAsJsonAsync(url, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            //category          
            entity.AllCategoriesSelectList = await GetAllCategoriesSelectedList(); ;
            //end category
            //AllMerchantNameList
            entity.AllMerchantNameList = await GetAllMerchantNameList();
            //end AllMerchantNameList
            //AllBrandNameList
            entity.AllBrandNameList = await GetAllBrandNameList();
            //end AllBrandNameList
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

            return Json(files);
        }

        [HttpPost]
        public JsonResult UploadAdditional()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            var currentContext = HttpContext;

            _filesHelper.UploadAndShowResults(currentContext, resultList);
            //save to db
            int productId = Int32.Parse(Request.Form["Id"]);

          //  var OutputresultList = new List<ViewDataUploadFilesResult>();

            if ( productId != 0)
            {
                foreach (var item in resultList)
                {
                   
                    string fielUrl = item.url;                 
                    Db.ProductImageTbls.InsertOnSubmit(new ProductImageTbl
                    {
                      
                        ProductId = productId,
                        ImagePath = fielUrl.TrimStart('/'),
                        Description = null,
                        DisplayOrder = 1,
                    });
                }

                try
                {
                    Db.SubmitChanges();
                }
                catch (Exception)
                {
                    throw new Exception("Exception");
                }
            }
            
            //end
            var files = new JsonFiles(resultList);

            var isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
            }
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

            //
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<Product>(responseData);
                if(entity!=null)
                {
                    //category  
                    var allCategoriesSelectList = await GetAllCategoriesSelectedList();
                    entity.AllCategoriesSelectList = allCategoriesSelectList;
                    //end category
                    if (allCategoriesSelectList!=null && entity.CategoryId!=null)
                        entity.AllCategoriesSelectList = SetSelectedItem(allCategoriesSelectList, entity.CategoryId);
                    //end category
                    //AllMerchantNameList

                    var merchantsList = await GetAllMerchantNameList();
                    entity.AllMerchantNameList = merchantsList;
                    if (merchantsList!=null && entity.MerchantId!=null)
                        entity.AllMerchantNameList = SetSelectedItem(merchantsList, entity.MerchantId);
                    //end AllMerchantNameList
                    //AllBrandNameList
                    var brandsList = await GetAllBrandNameList();
                    entity.AllBrandNameList = brandsList;
                    if (brandsList != null && entity.BrandId!=null)
                        entity.AllBrandNameList = SetSelectedItem(brandsList, entity.BrandId);
                    //end AllBrandNameList
                    return View(entity);
                }                           
            }           
            throw new Exception("Exception");
        }

        // POST: Products/Edit/5
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product entity)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync(url + "/" + id, entity);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            //category  
            var allCategoriesSelectList = await GetAllCategoriesSelectedList();
            entity.AllCategoriesSelectList = allCategoriesSelectList;
            //end category
            if (allCategoriesSelectList != null && entity.CategoryId != null)
                entity.AllCategoriesSelectList = SetSelectedItem(allCategoriesSelectList, entity.CategoryId);
            //end category
            //AllMerchantNameList

            var merchantsList = await GetAllMerchantNameList();
            entity.AllMerchantNameList = merchantsList;
            if (merchantsList != null && entity.MerchantId != null)
                entity.AllMerchantNameList = SetSelectedItem(merchantsList, entity.MerchantId);
            //end AllMerchantNameList
            //AllBrandNameList
            var brandsList = await GetAllBrandNameList();
            entity.AllBrandNameList = brandsList;
            if (brandsList != null && entity.BrandId != null)
                entity.AllBrandNameList = SetSelectedItem(brandsList, entity.BrandId);
            //end AllBrandNameList
            return View(entity);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var entity = JsonConvert.DeserializeObject<Product>(responseData);
                if(entity!=null)
                {
                    //category  
                    var allCategoriesSelectList = await GetAllCategoriesSelectedList();
                    //end category
                    //parent name                
                    if (entity.CategoryId != null && allCategoriesSelectList != null)
                        entity.Parent1Name_English = ParentNameFromAllCategories(allCategoriesSelectList, entity.CategoryId);
                    //end parent name

                    //MerchantName
                    if (entity.MerchantId != null)
                        entity.MerchantName = GetMarchantName(entity.MerchantId);
                    //end MerchantName
                    //BrandName
                    if (entity.BrandId != null)
                        entity.BrandName = GetBrandName(entity.BrandId);
                    //end BrandName
                    return View(entity);
                }              
            }        
           throw new Exception("Exception");
        }

        // POST: Products/Delete/5
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
