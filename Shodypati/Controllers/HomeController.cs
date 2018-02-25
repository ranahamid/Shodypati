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


namespace Shodypati.Controllers
{
    [ExceptionHandler]
    public class HomeController : BaseController
    {
        
        public async Task<ActionResult> Index()
        {
            Log.Info("App started...");
            //api url                  
            url = baseUrl + "api/ProductsApi";
            url = url + "/GetProductsByCategoriesListWeb";

            //GetProductsByCategories
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<CategoryMobile>>(responseData);
                return View(entity);
            }
            throw new Exception("Exception");          
        }
    }
}
