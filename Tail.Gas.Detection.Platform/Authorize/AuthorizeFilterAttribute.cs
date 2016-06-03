using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Tail.Gas.Detection.Platform.Authorize
{
    /// <summary>
    ///　权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["username"] == null)
            {
                string applicationPath = filterContext.RequestContext.HttpContext.Request.ApplicationPath;
                applicationPath = applicationPath.TrimEnd('/');
                filterContext.Result = new RedirectResult(applicationPath + "/home");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}