using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            //Dictionary<String, object> data = new Dictionary<string, object>();
            //data.Add("Placeholder", "Placeholder");
            return View(GetData("Index"));
        }


        [Authorize(Roles = "Administror")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }
        private Dictionary<String,object> GetData(String actionName)
        {
            Dictionary<String, object> dict = new Dictionary<string, object>();
            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
            dict.Add("In Users Role", HttpContext.User.IsInRole("Administror"));
            return dict;
        }
    }
}