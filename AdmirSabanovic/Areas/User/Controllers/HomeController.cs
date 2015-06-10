using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Areas.User.Models;
using AdmirSabanovic.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AdmirSabanovic.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private const int NUMBER_OF_STATIC_FIELDS = 3;
        public HomeController()
        {
            formRepo = new FormCreatorRepoImpl();
            dynamicRepo = new DynamicFormCreatorImpl();
            userRepo = new UserRepository();
            additionalRepo = new AdditionalRepository();
        }

        public ActionResult Index()
        {
            FormPreviewVM data = new FormPreviewVM();
            data.forms = formRepo.getAllForms();
            return View("Index", data);
        }

        public String getDynamicForm(int id) 
        {
            return dynamicRepo.getDynamicFormByFormId(id);
        }

        [HttpPost]
        public ActionResult storeForm(FormCollection data){
            String[] allKeys = data.AllKeys;
            int formID = Convert.ToInt32(data.GetValues(allKeys[0]).First());
            Hashtable dynamicFields = new Hashtable();
            Hashtable staticFields = new Hashtable();

            seperateDynamicFromStatic(data, allKeys, staticFields, dynamicFields);

            AdmirSabanovic.Areas.User.Models.User user =
                userRepo.storeUserFromStaticFields(staticFields, formID);

            additionalRepo.storeDynamicFields(user, dynamicFields);
            ViewData["message"] = "Thanks for filling the form. Please verify your email.";
            return Index();
        }

        private void seperateDynamicFromStatic(FormCollection data ,String [] allKeys, 
            Hashtable staticFields, Hashtable dynamicFields)
        {
            if ((allKeys.Length > NUMBER_OF_STATIC_FIELDS) && (allKeys != null))
            {
                for (int i = 1; i < allKeys.Length; i++)
                {
                    if (i > NUMBER_OF_STATIC_FIELDS)
                        dynamicFields.Add(allKeys[i].ToString(), data.GetValues(allKeys[i]).First());
                    else 
                        staticFields.Add(allKeys[i].ToString(), data.GetValues(allKeys[i]).First());
                }
            }
        }

        public ActionResult verify(int id, string code)
        {
            userRepo.verifyUser(id, code);
            ViewData["message"] = "Thanks for veryfing email.";
            return Index();
        }

        public String getFormDetails(int id)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Forms returnForm = formRepo.FindByWithInclude(f => f.ID == id, "Admin").First();
            Hashtable table = new Hashtable();
            table.Add("admin", returnForm.Admin.Name);
            table.Add("details", returnForm.Description);
            table.Add("created_at", returnForm.Created_at.ToLongDateString());
            return ser.Serialize(table);
        }

        FormCreatorRepoImpl formRepo;
        DynamicFormCreatorImpl dynamicRepo;
        UserRepository userRepo;
        AdditionalRepository additionalRepo;
    }
}
