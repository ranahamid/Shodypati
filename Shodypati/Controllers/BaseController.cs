using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Shodypati.DAL;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [ExceptionHandler]
    public class BaseController : Controller
    {
        //public ICacheStorage _cacheStorage = new NullObjectCache(); 


        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string baseUrl = ConfigurationManager.AppSettings["webapibaseurl"];


        /// <summary>
        ///     caching implementation
        /// </summary>
        public ICacheStorage CacheStorage = new HttpContextCacheAdapter();

        public HttpClient client;
        public ShodypatiDataContext Db = new ShodypatiDataContext();
        public string url;

        public BaseController()
        {
            client = new HttpClient {BaseAddress = new Uri(baseUrl)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromMinutes(30);
            //api url
        }

        #region user

        public ApplicationUser GetUserInfo(ApplicationUserManager UserManager, Guid? id)
        {
            var user = UserManager.FindById(id.ToString());
            if (user.IsFakeEmail)
                user.Email = null;
            return user;
        }

        #endregion


        #region Email

        public bool SendEmailBase(string receiver, string subject, string body)
        {
            var destination = receiver;
            var msg = new MailMessage();
            msg.To.Add(destination);
            msg.From = new MailAddress(
                ConfigurationManager.AppSettings["FromAddress"], ConfigurationManager.AppSettings["TeamName"]);
            msg.Subject = subject;

            msg.Body = body;

            var smtpclient = new SmtpClient
            {
                UseDefaultCredentials = true,
                Host = ConfigurationManager.AppSettings["BaseMailHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["Port"]),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential
                (
                    ConfigurationManager.AppSettings["mailAccount"],
                    ConfigurationManager.AppSettings["mailPassword"]
                ),
                Timeout = 20000
            };

            try
            {
                smtpclient.Send(msg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Basic

        private string SwitchEngBan(string number)
        {
            var en = "1234567890.,";
            var bn = "১২৩৪৫৬৭৮৯০.,";
            return number.Select(o => en.Contains(o)
                    ? bn.Substring(en.IndexOf(o), 1)
                    : en.Substring(bn.IndexOf(o), 1))
                .Aggregate((a, b) => a + b);
        }

        #endregion

        #region Banner

        public async Task<List<SelectListItem>> GetAllBannerList()
        {
            //custom property for parent     
            url = baseUrl + "api/BannersApi/GetAllBannersSelectList";
            var responseMessageParentCat = await client.GetAsync(url);
            var entities = new List<SelectListItem>();
            if (responseMessageParentCat.IsSuccessStatusCode)
            {
                var responseDataParentCat = responseMessageParentCat.Content.ReadAsStringAsync().Result;

                entities = JsonConvert.DeserializeObject<List<SelectListItem>>(responseDataParentCat);
            }

            //end custom property for parent 
            return entities;
        }

        #endregion

        #region all

        public List<SelectListItem> SetSelectedItem(List<SelectListItem> items, int? id)
        {
            var tempItems = new List<SelectListItem>();

            foreach (var item in items)
                tempItems.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = string.Equals(item.Value, id.ToString(), StringComparison.CurrentCultureIgnoreCase)
                        ? true
                        : false
                });
            return tempItems;
        }

        #endregion

        #region dcoctorappointment

        public async Task<List<SelectListItem>> GetAllDoctorWorkingTypes()
        {
            url = baseUrl + "api/DoctorWorkingAreaApi/GetAllDoctorWorkingAreasSelectList";
            var responseMessage = await client.GetAsync(url);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);
                return entity;
            }

            return null;
        }

        public List<SelectListItem> GetAllWeekDaysName()
        {
            var days = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "রবিবার"},
                new SelectListItem {Value = "1", Text = "সোমবার"},
                new SelectListItem {Value = "2", Text = "মঙ্গলবার"},
                new SelectListItem {Value = "3", Text = "বুধবার"},
                new SelectListItem {Value = "4", Text = "বৃহস্পতিবার"},
                new SelectListItem {Value = "5", Text = "শুক্রবার"},
                new SelectListItem {Value = "6", Text = "শনিবার"}
            };
            return days;
        }


        public List<string> GetListFromCommaSeparatedIntList(string list)
        {
            var daysName = new List<string>();
            try
            {
                if (list != string.Empty)
                {
                    var daysIds = list.Split(',').Select(int.Parse).ToList();

                    foreach (var item in daysIds) daysName.Add(item.ToString());
                }
            }
            catch (Exception)
            {
                return daysName;
            }

            return daysName;
        }


        public string GetHiddenDaysFromActiveDays(string dayLists)
        {
            var daysName = new StringBuilder();
            try
            {
                if (dayLists != string.Empty)
                {
                    var daysIds = dayLists.Split(',').Select(int.Parse).ToList();
                    var numberList = Enumerable.Range(0, 7).ToList();
                    var resultList = numberList.Except(daysIds);
                    foreach (var item in resultList)
                        if (daysName.ToString() != string.Empty)
                            daysName.Append("," + item);
                        else
                            daysName.Append(item);
                }
            }
            catch (Exception)
            {
                return daysName.ToString();
            }

            return daysName.ToString();
        }


        public string GetDaysNameFromNumbers(string dayLists)
        {
            var daysName = new StringBuilder();
            try
            {
                if (dayLists != string.Empty)
                {
                    var daysIds = dayLists.Split(',').Select(int.Parse).ToList();

                    foreach (var item in daysIds)
                    {
                        var dayInStr = GetDayByInt(item);

                        if (daysName.ToString() != string.Empty)
                            daysName.Append("," + dayInStr);
                        else
                            daysName.Append(dayInStr);
                    }
                }
            }
            catch (Exception)
            {
                return daysName.ToString();
            }

            return daysName.ToString();
        }

        public string GetDayByInt(int dayNumber)
        {
            switch (dayNumber)
            {
                case 0:
                    return "রবিবার";

                case 1:
                    return "সোমবার";

                case 2:
                    return "মঙ্গলবার";

                case 3:
                    return "বুধবার";


                case 4:
                    return "বৃহস্পতিবার";

                case 5:
                    return "শুক্রবার";

                case 6:
                    return "শনিবার";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///     Dates  represented as Unix timestamp
        ///     with slight modification: it defined as the number
        ///     of seconds that have elapsed since 00:00:00, Thursday, 1 January 1970.
        ///     To convert it to .NET DateTime use following extension
        /// </summary>
        /// <param name="time">DateTime</param>
        /// <returns>
        ///     Return as DateTime of uint time
        /// </returns>
        public DateTime ToDateTime(uint time)
        {
            return new DateTime(1970, 1, 1).AddSeconds(time);
        }

        /// <summary>
        ///     Dates  represented as Unix timestamp
        ///     with slight modification: it defined as the number
        ///     of seconds that have elapsed since 00:00:00 Thursday, 1 January 1970.
        ///     To convert .NET DateTime to Unix time use following extension
        /// </summary>
        /// <param name="time">DateTime</param>
        /// <returns>
        ///     Return as uint time of DateTime
        /// </returns>
        public uint ToUnixTime(DateTime time)
        {
            return (uint) time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        #endregion

        #region usermanager, rolemanager

        public BaseController(ApplicationUserManager userManager, ApplicationRoleManager roleManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; set; }
        public ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            private set => _roleManager = value;
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        #endregion

        #region Account

        public IdentityResult CreatePatientUser(ApplicationUser user, string password,
            ApplicationUserManager UserManager)
        {
            var result = UserManager.Create(user, password);
            if (!result.Succeeded) return result;

            //customer role
            var role = "Patient";
            var resultRole = UserManager.AddToRole(user.Id, role);
            return result;
        }

        public IdentityResult CreateCustomerUser(ApplicationUser user, string password,
            ApplicationUserManager UserManager)
        {
            var result = UserManager.Create(user, password);
            if (!result.Succeeded) return result;
            //customer role
            var role = "Customer";
            var resultRole = UserManager.AddToRole(user.Id, role);
            return result;
        }

        public async Task<string> GetUserNameAsync(LoginViewModel model, ApplicationUserManager UserManager)
        {
            if (model.UserName.IndexOf('@') > -1)
            {
                //Validate email format
                var emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                var re = new Regex(emailRegex);
                if (!re.IsMatch(model.UserName)) ModelState.AddModelError("UserName", "Email is not valid");
            }
            else
            {
                //validate mobile format
                var emailRegex = @"^[+0-9]*$";
                var re = new Regex(emailRegex);
                if (!re.IsMatch(model.UserName)) ModelState.AddModelError("UserName", "Username is not valid");
            }

            var userName = model.UserName;
            if (userName.IndexOf('@') > -1)
            {
                var user = await UserManager.FindByEmailAsync(model.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return null;
                }

                userName = user.UserName;
                // userName = user.PhoneNumber;
            }

            return userName;
        }


        public ApplicationUser GetApplicationUserPatient(RegisterPatient model)
        {
            var isFakeMail = false;
            if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = GetGeneratedEmail();
                isFakeMail = true;
            }

            var user = new ApplicationUser
            {
                UserName = model.PhoneNumber,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Name = model.Name,
                DoctorName = model.DoctorName,
                HospitalName = model.HospitalName,
                Description = model.Description,
                IsFakeEmail = isFakeMail
            };
            return user;
        }


        public ApplicationUser GetApplicationUser(RegisterViewModel model)
        {
            var isFakeMail = false;
            if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = GetGeneratedEmail();
                isFakeMail = true;
            }

            var user = new ApplicationUser
            {
                UserName = model.PhoneNumber,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Name = model.Name,
                IsFakeEmail = isFakeMail
            };
            return user;
        }


        public string GetGeneratedEmail()
        {
            var email = string.Empty;

            email = "temp" + GetRandomNumber() + DateTime.Now.Ticks + "@gmail.com";

            return email;
        }


        // Generate random number for email address
        public static int GetRandomNumber()
        {
            return new Random().Next(100000, 100000000);
        }

        #endregion

        #region Category

        public string GetParentNameFromAllCategories(int? Id)
        {
            if (Id != null)
                foreach (var item in AllCategories)
                    if (item.Value.ToLower() == Id.ToString().ToLower())
                        return item.Text;
            return string.Empty;
        }

        public List<SelectListItem> Categories
        {
            get
            {
                var listItems = Db.CategoryTbls.Where(x => x.Parent1Id == null || x.Parent1Id == 0).Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name_English,
                        Value = x.Id.ToString()
                    }).ToList();

                return listItems;
            }
            set { }
        }

        public List<SelectListItem> ChildCategories
        {
            get
            {
                var listItems = Db.CategoryTbls.Where(x => x.Parent1Id == null && x.Parent1Id != 0).Select(x =>
                    new SelectListItem
                    {
                        Text = Db.CategoryTbls.FirstOrDefault(a => a.Id == x.Parent1Id).Name_English + " > " +
                               x.Name_English,
                        Value = x.Id.ToString()
                    }).ToList();
                return listItems;
            }
            set { }
        }

        public List<SelectListItem> AllCategories
        {
            get
            {
                var sli = new List<SelectListItem>();
                foreach (var item in Categories) sli.Add(item);
                foreach (var item in ChildCategories)
                {
                    var i = item.Text?.IndexOf(">") ?? 0;

                    if (item.Text != null)
                    {
                        var text = item.Text.Substring(0, i - 1);
                        foreach (var listItem in ChildCategories)
                        {
                            var k = listItem.Text.LastIndexOf(">", StringComparison.Ordinal);
                            var text2 = listItem.Text.Substring(k + 1);
                            if (text2.Contains(text)) item.Text = listItem.Text + " > " + item.Text.Substring(i + 2);
                        }
                    }

                    sli.Add(item);
                }

                return sli;
            }
        }

        public bool IsParentOfAnyCategory(int id)
        {
            var query = from x in Db.CategoryTbls
                where x.Parent1Id == id
                select x;
            if (query.Any()) return true;
            return false;
        }

        #endregion

        #region product

        public async Task<List<SelectListItem>> GetAllProductsSelectedList()
        {
            //custom property for parent     
            url = baseUrl + "api/ProductsApi";
            url = url + "/GetAllProductsSelectList";
            var responseMessageParentCat = await client.GetAsync(url);

            var entities = new List<SelectListItem>();

            if (responseMessageParentCat.IsSuccessStatusCode)
            {
                var responseDataParentCat = responseMessageParentCat.Content.ReadAsStringAsync().Result;

                entities = JsonConvert.DeserializeObject<List<SelectListItem>>(responseDataParentCat);
            }
            //end custom property for parent 

            return entities;
        }


        public async Task<List<string>> GetAllCampaignProductsStringList()
        {
            //custom property for parent     
            url = baseUrl + "api/CampaignProductsApi";
            url = url + "/GetAllCampaignProductsStringList";
            var responseMessageParentCat = await client.GetAsync(url);

            var entities = new List<string>();

            if (responseMessageParentCat.IsSuccessStatusCode)
            {
                var responseDataParentCat = responseMessageParentCat.Content.ReadAsStringAsync().Result;

                entities = JsonConvert.DeserializeObject<List<string>>(responseDataParentCat);
            }
            //end custom property for parent 

            return entities;
        }


        public async Task<List<SelectListItem>> GetAllCategoriesSelectedList()
        {
            var customurl = baseUrl + "api/CategoriesApi/GetAllCategoriesSelectList";
            var responseMessageCategory = await client.GetAsync(customurl);
            var responseDataCategory = responseMessageCategory.Content.ReadAsStringAsync().Result;

            List<SelectListItem> listItems;
            listItems = JsonConvert.DeserializeObject<List<SelectListItem>>(responseDataCategory);
            return listItems;
        }

        public string ParentNameFromAllCategories(List<SelectListItem> categoriesitems, int? id)
        {
            var paentText = string.Empty;
            foreach (var item in categoriesitems)
                if (item.Value.ToLower() == id.ToString().ToLower())
                {
                    paentText = item.Text;
                    break;
                }

            return paentText;
        }


        public string GetMarchantName(int? Id)
        {
            var query = from x in Db.MerchantTbls
                where x.Id == Id
                select x;
            var entity = query.Single();
            return entity.Name_English;
        }

        public string GetBrandName(int? Id)
        {
            var query = from x in Db.BrandTbls
                where x.Id == Id
                select x;
            var entity = query.Single();
            return entity.Name_English;
        }


        public async Task<List<SelectListItem>> GetAllMerchantNameList()
        {
            //custom property for parent     
            url = baseUrl + "api/MerchantsApi";
            url = url + "/GetAllMerchantsSelectList";
            var responseMessageParentCat = await client.GetAsync(url);
            var entities = new List<SelectListItem>();
            if (responseMessageParentCat.IsSuccessStatusCode)
            {
                var responseDataParentCat = responseMessageParentCat.Content.ReadAsStringAsync().Result;

                entities = JsonConvert.DeserializeObject<List<SelectListItem>>(responseDataParentCat);
            }

            //end custom property for parent 
            return entities;
        }

        public async Task<List<SelectListItem>> GetAllBrandNameList()
        {
            //custom property for parent     
            url = baseUrl + "api/BrandsApi";
            url = url + "/GetAllBrandsSelectList";
            var responseMessageParentCat = await client.GetAsync(url);

            var entities = new List<SelectListItem>();

            if (responseMessageParentCat.IsSuccessStatusCode)
            {
                var responseDataParentCat = responseMessageParentCat.Content.ReadAsStringAsync().Result;

                entities = JsonConvert.DeserializeObject<List<SelectListItem>>(responseDataParentCat);
            }
            //end custom property for parent 

            return entities;
        }

        #endregion
    }
}