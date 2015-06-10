using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Repos
{
    public class FormCreatorRepoImpl : GenericRepository<DBContext, Forms>
    {
  
        public void createForm(Forms form, string adminIdentifier)
        {
            Forms newForm = form;
            Areas.Admin.Models.Admin admin = Context.Admins.Find(adminIdentifier);
            newForm.Created_at = DateTime.Now;
            newForm.Updated_at = DateTime.Now;
            newForm.Description = form.Description;
            newForm.ShortDescription = form.ShortDescription;
            newForm.Admin = admin;
            Add(newForm);
            Save();
        }

        public List<Forms> getAllForms() 
        {
            List<Forms> allForms = getAllWithInclude("Admin").ToList();
            return allForms;
        }

        public void updateForm(int pk, string name, string value)
        {

            Forms form = FindBy(s=> s.ID == pk).First();
            switch (name)
            {
                case "Name":
                    form.Name = value;
                    break;
                case "ShortDescription":
                    form.ShortDescription = value;
                    break;
                case "Description":
                    form.Description = value;
                    break;
                default:
                    break;
            }
            form.Updated_at = DateTime.Now;
            Save();
        }

        public void deleteForm(int pk) 
        {
            Forms form = FindBy(s => s.ID == pk).First();
            firstDeleteDynamicForm(pk);
            Delete(form);
            Save();
        }

        /*
         * Not good solution for cascade deleting, but I had no time to search for better solution
         */
        private void firstDeleteDynamicForm(int formId) 
        {
            var query = from dynamic in Context.Dynamics where dynamic.form.ID == formId select dynamic;
            if(query.Count()!=0)
            {
                Dynamic dyn = Context.Dynamics.Find(query.First().ID);
                Context.Dynamics.Remove(dyn);
            }
        }

        public String getFormsLongDescription(int pk) 
        {
            Forms form = FindBy(s => s.ID == pk).First();
            return form.Description;
        }
    }
}