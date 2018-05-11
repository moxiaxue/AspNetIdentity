
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Users.Infrastructure;
using Users.Models;

namespace Users.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult>Create(CreateModel model)
        {
            if(ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(String id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    return View("Error", result.Errors);
            }
            else
                return View("Error", new string[] { "User Not Found" });
        }


        public async Task<ActionResult> Edit(String id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null) return View(user);
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(String id,String email, String password)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if(user!=null)
            {
                user.Email = email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                    AddErrorsFromResult(validEmail);

                IdentityResult validPassword = null;
                if(password!=String.Empty)
                {
                    validPassword = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPassword.Succeeded)
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    else
                        AddErrorsFromResult(validPassword);
                }

                if((validEmail.Succeeded&&validPassword==null)||password!=String.Empty&&validPassword.Succeeded)
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded) return RedirectToAction("Index");
                    else
                        AddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(String error in result.Errors)
            {
                ModelState.AddModelError("", error);
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