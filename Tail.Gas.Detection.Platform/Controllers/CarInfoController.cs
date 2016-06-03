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
    public class CarInfoController : BaseController
    {
        //
        // GET: /CarInfo/
        [AuthorizeFilterAttribute]
        public ActionResult Index()
        {
            return View("detail");
        }
          [AuthorizeFilterAttribute]
        public ActionResult Detail()
        {
            return View();
        }
        public string Get(string carno)
        {
            try
            {
                CarInfo carinfo =  CarInfoDao.GetCardInfoByCarNo(carno);

                JObject joRow = new JObject();
                joRow["NO"] = carinfo.NO;
                joRow["Color"] = carinfo.Color;
                joRow["Category"] = carinfo.Category;
                joRow["Belong"] = carinfo.Belong;
                joRow["OriginalEmissionValues"] = carinfo.OriginalEmissionValues;
                joRow["ProductModel"] = carinfo.ProductModel;
                joRow["ModifiedCompany"] = carinfo.ModifiedCompany;
                joRow["UserInfo"] = carinfo.UserInfo;
                joRow["IndividualCompany"] = carinfo.IndividualCompany;
                joRow["Telphone"] = carinfo.Telphone;
                joRow["ModifiedTime"] = carinfo.ModifiedTime;
                return joRow.ToString();
                 
            }
            catch (Exception ex)
            {
                return "";
            }
            
        }

    }
}
