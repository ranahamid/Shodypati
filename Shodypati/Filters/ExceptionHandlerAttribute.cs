using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shodypati.Models;
using System.Web.Mvc;
using Shodypati.DAL;
using System.Web.Routing;

namespace Shodypati.Filters
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public ShodypatiDataContext db = new ShodypatiDataContext();

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {

                //ip
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string ip;

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        ip = addresses[0];
                    }
                }

                ip = context.Request.ServerVariables["REMOTE_ADDR"];
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
                //end ip

                //broser
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                //string brwDescription =   "Type =                         " + browser.Type + "\n"
                //                        + "Name =                         " + browser.Browser + "\n"
                //                        + "Version =                      " + browser.Version + "\n"
                //                        + "Major Version =                " + browser.MajorVersion + "\n"
                //                        + "Minor Version =                " + browser.MinorVersion + "\n"
                //                        + "Platform =                     " + browser.Platform + "\n"
                //                        + "Is Beta =                      " + browser.Beta + "\n"
                //                        + "Is Crawler =                   " + browser.Crawler + "\n"
                //                        + "Is AOL =                       " + browser.AOL + "\n"
                //                        + "Is Win16 =                     " + browser.Win16 + "\n"
                //                        + "Is Win32 =                     " + browser.Win32 + "\n"
                //                        + "Supports Frames =              " + browser.Frames + "\n"
                //                        + "Supports Tables =              " + browser.Tables + "\n"
                //                        + "Supports Cookies =             " + browser.Cookies + "\n"
                //                        + "Supports VBScript =            " + browser.VBScript + "\n"
                //                        + "Supports JavaScript =          " + browser.EcmaScriptVersion.ToString() + "\n"
                //                        + "Supports Java Applets =        " + browser.JavaApplets + "\n"
                //                        + "Supports ActiveX Controls =    " + browser.ActiveXControls + "\n";

                string brwDescription = browser.Browser +" " +browser.Version +", " + browser.EcmaScriptVersion.ToString();
                //end browser
                //os
                string OSName = string.Empty;
                OSName = HttpContext.Current.Request.Browser.Platform;
                //end-os
                //backurl
                string back = string.Empty;
                back = HttpContext.Current.Request.Url.AbsoluteUri;
                //end
                db.LogTbls.InsertOnSubmit(new LogTbl
                {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    IpAddress = ip,
                    Browser = brwDescription,
                    OS= OSName,
                    UserId = new Guid(),
                    ActionName = filterContext.RouteData.Values["action"].ToString(),
                    CreatedOnUtc = DateTime.Now
                });
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception)
                {

                }

                filterContext.ExceptionHandled = true;
                //redirect
                var context2 = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                var urlHelper = new UrlHelper(context2);
                var url = urlHelper.Action("ErrorException", "Account", new { backUrl = @back });
                System.Web.HttpContext.Current.Response.Redirect(url);
            }
        }
    }
}