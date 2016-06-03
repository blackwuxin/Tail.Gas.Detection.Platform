using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Tail.Gas.Detection.Platform.Controllers
{
    public class BaseController : Controller
    {
        public const byte RESULT_SUCCESS = 0;
        public const byte RESULT_ERROR = 1;


        public static ServiceResponse CreateSuccessResponse()
        {
            return CreateSuccessResponse(default(byte), default(object));
        }

        public static ServiceResponse CreateSuccessResponse(object data)
        {
            return CreateSuccessResponse(default(byte), data);
        }

        public static ServiceResponse CreateSuccessResponse(byte resultCode, object data)
        {
            return new ServiceResponse() { Result = RESULT_SUCCESS, ResultCode = resultCode, Body = data };
        }

        public static ServiceResponse CreateErrorResponse()
        {
            return CreateErrorResponse(default(byte), default(string));
        }

        public static ServiceResponse CreateErrorResponse(string errorMsg)
        {
            return CreateErrorResponse(default(byte), errorMsg);
        }
        public static ServiceResponse CreateErrorResponse(byte resultCode, string errorMsg)
        {
            return new ServiceResponse() { Result = RESULT_ERROR, ResultCode = resultCode, ResultMessage = errorMsg };
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
    
            ViewBag.ApplicationPath = ApplicationPath;
            ViewBag.Menus = GetMenusString();
        }
        private string GetMenusString()
        {
            StringBuilder sbMenus = new StringBuilder();
            string activeClass = " class='active'";
            string url = Request.RawUrl.ToLower();

            if (!url.Contains("home")&& !url.Equals("/myapp"))
            {
                sbMenus.Append(string.Format("<li role='presentation' {0}><a href='{1}/carstatusinfo/Index' target=''>状态正常</a></li>",
                   url.Contains("/carstatusinfo/index")||url.Equals("/myapp/carstatusinfo") ? activeClass : "", ApplicationPath));
                sbMenus.Append(string.Format("<li role='presentation' {0}><a href='{1}/carstatusinfo/error' target=''>状态异常</a></li>",
                   url.Contains("/carstatusinfo/error") ? activeClass : "", ApplicationPath));
            }
            if ("管理员".Equals(System.Web.HttpContext.Current.Session["usertype"]))
            {
                sbMenus.Append(string.Format("<li role='presentation' {0}><a href='{1}/importcarinfo/' target=''>导入车辆信息</a></li>",
                  url.Contains("/importcarinfo") ? activeClass : "", ApplicationPath));
            }
           
            return sbMenus.ToString();
        }
        /// <summary>
        /// 返回不以斜杠结尾的应用程序路径
        /// </summary>
        protected string ApplicationPath
        {
            get
            {
                string applicationPath = Request.ApplicationPath;
                return applicationPath.TrimEnd('/');
            }
        }
    }
    public class ServiceResponse
    {
        [JsonProperty(PropertyName = "result")]
        public byte Result { get; set; }

        [JsonProperty(PropertyName = "resultCode")]
        public byte ResultCode { get; set; }

        [JsonProperty(PropertyName = "resultMessage")]
        public string ResultMessage { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Body { get; set; }
    }

    public class ServiceDataResponse : ServiceResponse
    {
        public long iTotalRecords { get; set; }
        public long iTotalDisplayRecords { get; set; }
        public List<object> aaData { get; set; }

    }
}
