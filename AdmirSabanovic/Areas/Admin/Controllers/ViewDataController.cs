using AdmirSabanovic.Repos;
using AdmirSabanovic.Repos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdmirSabanovic.Areas.Admin.Models;
using System.Web.Script.Serialization;
using System.Collections;
using AdmirSabanovic.Areas.User.Models;
namespace AdmirSabanovic.Areas.Admin.Controllers
{
    [Authenticator]
    public class ViewDataController : Controller
    {
        public ViewDataController()
        {
            form = new FormCreatorRepoImpl();
            db = new DBContext();
            vtd = new ViewDataTable();
            additionalRepo = new AdditionalRepository();
        }

        //
        // GET: /Admin/ViewData/

        public ActionResult Index()
        {
            dataToPass();
            return View("ViewData");
        }

        private void dataToPass()
        {
            ViewData["allForms"] = form.getAllForms();   
        }

        public void returnTableData()
        {           
            int iDisplayLength = Convert.ToInt32(Request.QueryString["length"]);
            int iDisplayStart = Convert.ToInt32(Request.QueryString["start"]);

            int iSortCol = Convert.ToInt32(HttpContext.Request["order[0][column]"]);


            String sSortDir = HttpContext.Request["order[0][dir]"];

            String search = Request.QueryString["search[value]"];
            Int16 formID = Convert.ToInt16(Request.QueryString["formName"]);

            ViewDataTable vtd = new ViewDataTable();
            JavaScriptSerializer ser = new JavaScriptSerializer();

            Hashtable tt = vtd.returnTableData(search, iDisplayLength, iDisplayStart, iSortCol, sSortDir, formID);

            Response.ContentType = ("text/html");
            Response.BufferOutput = true;
            Response.Write(ser.Serialize(tt));
            Response.End();   
        }

        public string GetDynamicKeysByFormId(int formid)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(additionalRepo.getAllKeys(formid));
        }
       
        DBContext db;
        FormCreatorRepoImpl form;
        ViewDataTable vtd;
        AdditionalRepository additionalRepo;
    }
}
