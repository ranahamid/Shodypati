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
    public class CatalogController : BaseController
    {
        public CatalogController()
        {
           
        }


        public ActionResult MenuBanner()
        {
           // Banner entity = new Banner();

            return PartialView("_MenuBanner");
        }


        public ActionResult HomepageBanner()
        {
            url = baseUrl + "api/BannersApi/GetHomePageBanner";

            var responseMessage = new WebClient().DownloadString(url);

            var entity = JsonConvert.DeserializeObject<BannerMobile>(responseMessage);

            return PartialView("_HomepageBanner", entity);
        }

       

    }
}