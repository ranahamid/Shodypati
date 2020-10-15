using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [ExceptionHandler]
    public class CatalogController : BaseController
    {
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