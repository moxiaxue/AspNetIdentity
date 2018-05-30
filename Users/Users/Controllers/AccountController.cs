using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Users.Infrastructure;
using Users.Models;

namespace Users.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Access Denied"});
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details,String returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.Name, details.Password);
                if (user == null)
                    ModelState.AddModelError("", "Invalid name or password!");
                else
                {
                    ClaimsIdentity claimsIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    claimsIdentity.AddClaims(LocationClaimsProvider.GetClaims(claimsIdentity));
                    claimsIdentity.AddClaims(ClaimsRoles.CreateRolesFromClaims(claimsIdentity));
                    //remove the Cookie Authenticated and apply the new one
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent=false},claimsIdentity);
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(details);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult MicrosoftLogin(String returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("MicrosoftCallback", new { returnUrl = returnUrl })
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Microsoft");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> MicrosoftCallback(String returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindAsync(loginInfo.Login);
            if(user==null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                    City = Cities.LONDON,
                    Country = Countries.UK
                };
                IdentityResult result = await UserManager.CreateAsync(user);
                if(!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if(!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }
            ClaimsIdentity claimsIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            claimsIdentity.AddClaims(loginInfo.ExternalIdentity.Claims);

            AuthManager.SignIn(new AuthenticationProperties{IsPersistent = false}, claimsIdentity);
            return Redirect(returnUrl ?? "/");
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}