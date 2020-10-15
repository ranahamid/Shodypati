using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class AccountApiController : ApiController
    {
        private readonly IAccountAccessRepository<RegisterViewModel, int> _repository;

        public AccountApiController(IAccountAccessRepository<RegisterViewModel, int> r)
        {
            _repository = r;
        }

        public ApplicationUserManager UserManager =>
            HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ApplicationSignInManager SignInManager =>
            HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();


        [Route("api/Account/RegisterPatient/")]
        // Post : api/Account/RegisterPatient/
        public async Task<UserStatusInfo> RegisterPatient( /*RegisterBindingMobile model*/)
        {
            //RegisterBindingMobile model
            var responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<RegisterPatient>(responseData);

            return _repository.RegisterPatient(entity, UserManager);
        }

        [Route("api/Account/Register/")]
        // Post : api/Account/Register/
        public async Task<UserStatusInfo> Register( /*RegisterBindingMobile model*/)
        {
            //RegisterBindingMobile model
            var responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<RegisterViewModel>(responseData);

            return _repository.Register(entity, UserManager);
        }


        [Route("api/Account/Login/")]
        // Post : api/Account/Login
        public async Task<UserStatusInfo> Login( /*LoginViewModel model*/)
        {
            var responseData = await Request.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<LoginViewModel>(responseData);

            return await _repository.Login(entity, SignInManager, UserManager);
        }
    }
}