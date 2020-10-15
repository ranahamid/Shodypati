using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class AccountDataAccessRepository : BaseController, IAccountAccessRepository<RegisterViewModel, int>
    {
        // POST api/Account/Register       
        public UserStatusInfo Register(RegisterViewModel model, ApplicationUserManager UserManager)
        {
            var emailModel = model.Email;

            if (!ModelState.IsValid)
                return new UserStatusInfo
                {
                    message = "Form is not valid",
                    success = false
                };

            var user = GetApplicationUser(model);

            var result = CreateCustomerUser(user, model.Password, UserManager);

            if (!result.Succeeded)
            {
                var msg = string.Empty;
                foreach (var item in result.Errors) msg = msg + item;
                return new UserStatusInfo
                {
                    message = msg,
                    success = false
                };
            }

            var regUser = UserManager.FindByName(model.PhoneNumber);
            return new UserStatusInfo
            {
                message = "User successfully created.",
                success = true,
                UserId = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = emailModel,
                UserName = user.UserName
            };
        }

        // POST api/Account/Register       
        public UserStatusInfo RegisterPatient(RegisterPatient model, ApplicationUserManager UserManager)
        {
            var emailModel = model.Email;

            if (!ModelState.IsValid)
                return new UserStatusInfo
                {
                    message = "Form is not valid",
                    success = false
                };

            var user = GetApplicationUserPatient(model);

            var result = CreatePatientUser(user, model.Password, UserManager);

            if (!result.Succeeded)
            {
                var msg = string.Empty;
                foreach (var item in result.Errors) msg = msg + item;
                return new UserStatusInfo
                {
                    message = msg,
                    success = false
                };
            }

            var RegUser = UserManager.FindByName(model.PhoneNumber);
            return new UserStatusInfo
            {
                message = "User successfully created.",
                success = true,
                UserId = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = emailModel,
                UserName = user.UserName
            };
        }

        public async Task<UserStatusInfo> Login(LoginViewModel model, ApplicationSignInManager SignInManager,
            ApplicationUserManager UserManager)
        {
            if (!ModelState.IsValid) return null;

            var userName = await GetUserNameAsync(model, UserManager);

            if (userName == null) return null;
            var result2 = SignInManager.PasswordSignIn(userName, model.Password, model.RememberMe, false);

            switch (result2)
            {
                case SignInStatus.Success:

                    var user = UserManager.FindByName(userName);

                    string emailModel = null;
                    if (!user.IsFakeEmail) emailModel = user.Email;
                    //check if patient role
                    // var user = await UserManager.FindByNameAsync(userName);
                    var RoleNames = UserManager.GetRoles(user.Id);
                    var patientRole = false;
                    if (RoleNames != null && RoleNames.Count > 0)
                        foreach (var item in RoleNames)
                            if (item == "Patient")
                            {
                                patientRole = true;
                                break;
                            }

                    //end
                    return new UserStatusInfo
                    {
                        UserId = user
                            .Id, //SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId(),                        
                        PhoneNumber =
                            user.PhoneNumber, //SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserName(),                      
                        message = "User successfully logged in.",
                        success = true,
                        Name = user.Name,
                        Email = emailModel,
                        UserName = user.UserName,
                        PatientRole = patientRole
                    };
                case SignInStatus.LockedOut:
                    return null;
                case SignInStatus.RequiresVerification:
                    return null;
                case SignInStatus.Failure:
                default:
                    return null;
            }
        }
    }
}