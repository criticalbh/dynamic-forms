using AdmirSabanovic.Repos;
using AdmirSabanovic.Repos.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AdmirSabanovic.Areas.Admin.Controllers
{
    [Authenticator()]
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/
        
        public ActionResult Index()
        {
            FormCreatorRepoImpl form = new FormCreatorRepoImpl();
            ViewData["allForms"] = form.getAllForms();
            return View("Index");
        }

        public JsonResult getStatisticsRegisteredGraph(int id)
        {
             UserRepository userRepo = new UserRepository();
             List<Hashtable> statistics = userRepo.GetStatisticsByCreationByFormId(id);
             return Json(statistics, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getStatisticForEachForm() 
        {
            FormCreatorRepoImpl fcri = new FormCreatorRepoImpl();
            UserRepository userRepo = new UserRepository();
            List<Hashtable> returnTableInList = new List<Hashtable>();
            foreach (var item in fcri.getAllForms())
            {
                Hashtable table = new Hashtable();
                table.Add("form", item.Name);
                table.Add("value", userRepo.CountAllByFormId(item.ID));
                returnTableInList.Add(table);
            }
            return Json(returnTableInList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult countingStatistics(int formID)
        {
            UserRepository userRepo = new UserRepository();
            return Json(userRepo.CountingStatistics(formID), JsonRequestBehavior.AllowGet);
        }
    }
}
