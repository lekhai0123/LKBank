using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.HttpContext.Session["Email"] == null)
        {
            filterContext.Result = new RedirectResult("~/USER/Login");
        }
        base.OnActionExecuting(filterContext);
    }
}
