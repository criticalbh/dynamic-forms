using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdmirSabanovic.Repos.Admin
{

    public class Authenticator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ID = filterContext.HttpContext.User.Identity.Name;
            var data = AdminData.getData(ID);
            filterContext.Controller.ViewBag.User = data;
            base.OnActionExecuting(filterContext);
        }
    }
}