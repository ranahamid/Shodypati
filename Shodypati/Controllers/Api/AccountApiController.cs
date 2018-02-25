using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Shodypati.Models;
using Shodypati.DAL;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Newtonsoft.Json;

namespace Shodypati.Controllers.Api
{
    public class AccountApiController : ApiController
    {
        private IAccountAccessRepository<RegisterViewModel, int> _repository;

        public AccountApiController(IAccountAccessRepository<RegisterViewModel, int> r)
        {
            _repository = r;
        }

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        
        [Route("api/Account/RegisterPatient/")]
        // Post : api/Account/RegisterPatient/
        public async Task<UserStatusInfo> RegisterPatient( /*RegisterBindingMobile model*/)
        {
            //RegisterBindingMobile model
            string responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<RegisterPatient>(responseData);

            return _repository.RegisterPatient(entity, UserManager);
        }

        [Route("api/Account/Register/")]
        // Post : api/Account/Register/
        public async Task<UserStatusInfo> Register( /*RegisterBindingMobile model*/)
        {

            //RegisterBindingMobile model
            string responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<RegisterViewModel>(responseData);

            return _repository.Register(entity, UserManager);
        }
        
     
        [Route("api/Account/Login/")]
        // Post : api/Account/Login
        public async Task<UserStatusInfo> Login(/*LoginViewModel model*/)
        {            
            string responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<LoginViewModel>(responseData);

            return await _repository.Login(entity, SignInManager, UserManager);
        }
        

    }
}
