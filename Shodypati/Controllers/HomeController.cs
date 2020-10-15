using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shodypati.Filters;
using Shodypati.Models;

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
            var responseMessage = await client.GetAsync(url);
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