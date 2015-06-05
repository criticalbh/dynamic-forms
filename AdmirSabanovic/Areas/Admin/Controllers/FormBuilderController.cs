using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Repos;
using AdmirSabanovic.Repos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdmirSabanovic.Areas.Admin.Controllers
{
    [Authenticator()]
    public class FormBuilderController : Controller
    {
        public FormBuilderController() 
        {
            form = new FormCreatorRepoImpl();
            dynamic = new DynamicFormCreatorImpl();
        }

        public ActionResult DynamicCreator()
        {
            ViewData["allForms"] = form.getAllForms();
            return View("dynamic");
        }

        public ActionResult Forms() 
        {
            ViewData["allForms"] = form.getAllForms();
            return View("forminit");
        }

        [HttpPost]
        public ActionResult Forms(Forms model, string adminIdentifier)
        {
            /*
             * Even though I could extract adminId from logged user, I prefered to do like this
             */
            FormCreatorRepoImpl form = new FormCreatorRepoImpl();
            form.createForm(model, adminIdentifier);
            return Forms();
        }

        [HttpPut]
        [ValidateInput(false)]
        public void UpdateForms(int pk, string name, string value) {
            form.updateForm(pk, name, value);
        }

        [HttpDelete]
        public void DeleteForm(int pk) {
            form.deleteForm(pk);
        }

        public String GetFormsLongDescription(int pk) 
        {
            return form.getFormsLongDescription(pk);
        }

        /*
         * Dynamic fields area 
         */
        public String GetDynamicForm(int id) 
        {
            return dynamic.getDynamicFormByFormId(id);
        }

        [HttpPost]
        public void StoreDynamicForm(int form_id, String form_generated) 
        {
            dynamic.storeDynamicForm(form_id, form_generated);
        }

        FormCreatorRepoImpl form;
        DynamicFormCreatorImpl dynamic;
    }
}
