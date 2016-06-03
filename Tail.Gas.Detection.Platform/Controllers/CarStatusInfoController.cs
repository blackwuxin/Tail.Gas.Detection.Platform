using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tail.Gas.Detection.Platform.Authorize;
using Tail.Gas.Detection.Platform.Dao;
using Tail.Gas.Detection.Platform.Models;

namespace Tail.Gas.Detection.Platform.Controllers
{
    public class CarStatusInfoController : BaseController
    {
        //
        // GET: /CarStatusInfo/
        [AuthorizeFilterAttribute]
        public ActionResult Index()
        {
            return View("NormalList");
            
        }
        [AuthorizeFilterAttribute]
        public ActionResult error()
        {
            return View("ErrorList");
        }
        public string query(string PageUrl, string CarNo,string Category, string Belong, int istart, int ilen)
        {

            

            JObject joResult = new JObject();
            JArray messageList = new JArray();
            if (!string.IsNullOrEmpty(PageUrl) && PageUrl.ToLower().Contains("carstatusinfo/index") || PageUrl.Equals("/myapp/carstatusinfo"))
            {
                CarStatusInfoDao.GetNormalListByPage(CarNo,Category,
                    Belong, ref messageList);
            }
            else if (!string.IsNullOrEmpty(PageUrl) && PageUrl.ToLower().Contains("carstatusinfo/error"))
            {
                CarStatusInfoDao.GetErrorListByPage(CarNo,Category,
                    Belong, ref messageList);
            }
            

            JArray ja = new JArray();
            foreach (var token in messageList.Skip(istart).Take(ilen))
            {
                ja.Add(token);
            }

            joResult["messages"] = ja;
            joResult["total-records"] = messageList.Count;
            return joResult.ToString();
        }


    }
}
