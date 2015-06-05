using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Context;

namespace AdmirSabanovic.Repos
{
    public class DynamicFormCreatorImpl : GenericRepository<DBContext, Dynamic>
    {
        public void storeDynamicForm(int form_id, String form_generated)
        {
            try
            {
                Forms form = Context.Forms.Find(form_id);
                var query = FindBy(d => d.form.ID == form.ID);
                int counter = query.Count();
                Dynamic dynamicForm = new Dynamic();
                if(counter == 0)
                {
                    dynamicForm.Form_generated = form_generated;
                    dynamicForm.form = form;
                    Add(dynamicForm);
                }
                else
                {
                    foreach (var item in query)
                    {
                        dynamicForm.Form_generated = item.Form_generated;
                        dynamicForm.ID = item.ID;
                    }
                    Dynamic dynamic = FindBy(d => d.ID == dynamicForm.ID).First();
                    dynamic.Form_generated = form_generated;
                    Edit(dynamic);
                }
                form.Updated_at = DateTime.Now;
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Dynamic> getAllDynamicForms()
        {
            return GetAllInList();
        }

        public String getDynamicFormByFormId(int id)
        {
            Forms form = Context.Forms.Find(id);
            var result = FindBy(d=> d.form.ID == form.ID).First();
            return result.Form_generated.ToString();
        }

        public String getDynamicForm(int id) 
        {
            String generatedForm;
            try
            {
                Dynamic dyn = FindBy(d => d.ID == id).First();
                generatedForm = dyn.Form_generated;
            }
            catch (Exception)
            {
                generatedForm = "";
            }
            return generatedForm;
        }
    }
}