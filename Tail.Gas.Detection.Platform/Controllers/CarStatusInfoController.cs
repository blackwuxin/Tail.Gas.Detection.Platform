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
            long totalCount = 0;
            if (!string.IsNullOrEmpty(PageUrl) && PageUrl.ToLower().Contains("carstatusinfo/index") || PageUrl.Equals("/myapp/carstatusinfo"))
            {
                CarStatusInfoDao.GetNormalListByPage(CarNo,Category,
                    Belong,ilen, istart / ilen + 1, out totalCount, ref messageList);
            }
            else if (!string.IsNullOrEmpty(PageUrl) && PageUrl.ToLower().Contains("carstatusinfo/error"))
            {
                CarStatusInfoDao.GetErrorListByPage(CarNo, Category,
                    Belong, ilen, istart / ilen + 1, out totalCount, ref messageList);
            }
            joResult["messages"] = messageList;
            joResult["total-records"] = totalCount;
            return joResult.ToString();
        }


    }
}
