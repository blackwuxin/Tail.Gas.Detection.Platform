using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tail.Gas.Detection.Platform.Authorize;

namespace Tail.Gas.Detection.Platform.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["username"] != null)
            {
                return RedirectToAction("index","carstatusinfo");
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult Forbidden()
        {
            return View();
        }
        public string login(string username, string password,string usertype)
        {
       
            System.Web.HttpContext.Current.Session["username"] = username;
            System.Web.HttpContext.Current.Session["usertype"] = usertype;
            JObject joResult = new JObject();
            joResult["result"] = 0;
            return joResult.ToString();
        }
    }
}
