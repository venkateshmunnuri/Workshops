using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkShopsBucket.Attributes
{
    public class WorkshopAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("https://localhost:44306/");
            }
            else
            {
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Url.AbsoluteUri);
            }
        }
    }
}