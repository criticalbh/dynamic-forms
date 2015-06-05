using AdmirSabanovic.Areas.User.Models;
using AdmirSabanovic.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View(data);
        }

        public String getDynamicForm(int id) 
        {
            return dynamicRepo.getDynamicFormByFormId(id);
        }

        [HttpPost]
        public void storeForm(FormCollection data){
            String[] allKeys = data.AllKeys;
            int formID = Convert.ToInt32(data.GetValues(allKeys[0]).First());
            Hashtable dynamicFields = new Hashtable();
            Hashtable staticFields = new Hashtable();

            seperateDynamicFromStatic(data, allKeys, staticFields, dynamicFields);

            AdmirSabanovic.Areas.User.Models.User user =
                userRepo.storeUserFromStaticFields(staticFields, formID);

            additionalRepo.storeDynamicFields(user, dynamicFields);
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

        public void verify(int id, string code)
        {
            userRepo.verifyUser(id, code);
        }


        /*
         * old way of using repos.. if I find time I will refactor 
         */
        FormCreatorRepoImpl formRepo;
        DynamicFormCreatorImpl dynamicRepo;

        /*
         * Generic Repositories
         */
        UserRepository userRepo;
        AdditionalRepository additionalRepo;
    }
}
