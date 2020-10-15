using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        #region ErrorException

        public ActionResult ErrorException(string backUrl)
        {
            ViewBag.backUrl = backUrl;
            return View();
        }

        #endregion


        #region GetUserInfoById

        public ActionResult GetUserInfoById(Guid? Id)
        {
            var User = UserManager.FindById(Id.ToString());
            return PartialView(User);
        }

        #endregion

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var userName = await GetUserNameAsync(model, UserManager);
            if (userName == null)
                //error
                return View(model);

            //if user in patient role, then redirect to medicine page by default
            var user = await UserManager.FindByNameAsync(userName);
            var RoleNames = await UserManager.GetRolesAsync(user.Id);
            foreach (var item in RoleNames)
                if (item == "Patient")
                {
                    returnUrl = "/Medicines/Create";
                    break;
                }

            //end


            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl});
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync()) return View("Error");
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " +
                                 await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result =
                await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, false, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterPatient()
        {
            var entity = new RegisterPatient();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPatient(RegisterPatient model)
        {
            if (ModelState.IsValid)
            {
                var user = GetApplicationUserPatient(model);

                var result = CreatePatientUser(user, model.Password, UserManager);

                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code},
                        Request.Url.Scheme);

                    //send email
                    var body = "Dear " + model.Email + "," +
                               "\n\nWelcome to Shodypati!" +
                               "\n\nA request has been received to open your Shodypati account." +
                               "\n\nPlease confirm your account by clicking this link: <a href=\"" + callbackUrl +
                               "\">Click here</a>." +
                               "\n\nIf you did not initiate this request, please contact us immediately at support@shodypati.com." +
                               "\n\nThank you," +
                               "\nThe Shodypati Team.";

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);


                    //sms
                    // Generate the token and send it
                    var codeSMS = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);
                    if (UserManager.SmsService != null)
                    {
                        var message = new IdentityMessage
                        {
                            Destination = model.PhoneNumber,
                            Body = "Your security code is: " + codeSMS
                        };
                        await UserManager.SmsService.SendAsync(message);
                    }

                    //ViewBag.Link = callbackUrl;
                    //return View("DisplayEmail");
                    return RedirectToAction("VerifyPhoneNumber", "Manage", new {model.PhoneNumber});
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = GetApplicationUser(model);

                var result = CreateCustomerUser(user, model.Password, UserManager);

                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code},
                        Request.Url.Scheme);

                    //send email
                    var body = "Dear " + model.Email + "," +
                               "\n\nWelcome to Shodypati!" +
                               "\n\nA request has been received to open your Shodypati account." +
                               "\n\nPlease confirm your account by clicking this link: <a href=\"" + callbackUrl +
                               "\">Click here</a>." +
                               "\n\nIf you did not initiate this request, please contact us immediately at support@shodypati.com." +
                               "\n\nThank you," +
                               "\nThe Shodypati Team.";

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);


                    //sms
                    // Generate the token and send it
                    var codeSMS = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);
                    if (UserManager.SmsService != null)
                    {
                        var message = new IdentityMessage
                        {
                            Destination = model.PhoneNumber,
                            Body = "Your security code is: " + codeSMS
                        };
                        await UserManager.SmsService.SendAsync(message);
                    }

                    //ViewBag.Link = callbackUrl;
                    //return View("DisplayEmail");
                    return RedirectToAction("VerifyPhoneNumber", "Manage", new {model.PhoneNumber});
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return View("Error");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // NOT Implemented
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code},
                    Request.Url.Scheme);

                var body = "Dear " + model.Email + "," +
                           "A request has been received to change the password for your Virtual Classroom account." +
                           "\n\nPlease reset your password by clicking here: <a href=\"" + callbackUrl +
                           "\">Click here</a>." +
                           "\n\nIf you did not initiate this request, please contact us immediately at support@shodypati.com." +
                           "\n\nThank you," +
                           "\nThe Virtual Classroom Team.";

                await UserManager.SendEmailAsync(user.Id, "Reset Password", body);
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation", "Account");
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null) return View("Error");
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose})
                .ToList();
            return View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid) return View();

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) return View("Error");
            return RedirectToAction("VerifyCode", new {Provider = model.SelectedProvider, model.ReturnUrl});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login");

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl});
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null) return View("ExternalLoginFailure");
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null) properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}