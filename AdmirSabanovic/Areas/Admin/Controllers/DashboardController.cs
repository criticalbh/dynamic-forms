using AdmirSabanovic.Repos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdmirSabanovic.Areas.Admin.Controllers
{
    [Authenticator()]
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
