public class bkp
{
}

/*using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Repos
{
    public class FormCreatsdorRepoImpl// : GenericRepository<DBContext, Forms> too much job for refactoring
    {
        public FormCreatorRepoImpl() {
            ctx = new DBContext();
        }
        
        public void createForm(Forms form, string adminIdentifier)
        {
            Forms newForm = form;
            Areas.Admin.Models.Admin admin = ctx.Admins.Find(adminIdentifier);
            newForm.Created_at = DateTime.Now;
            newForm.Updated_at = DateTime.Now;
            newForm.Description = form.Description;
            newForm.ShortDescription = form.ShortDescription;
            newForm.Admin = admin;
            ctx.Forms.Add(newForm);
            ctx.SaveChanges();
        }

        public List<Forms> getAllForms() 
        {
            List<Forms> allForms = ctx.Forms.Include("Admin").ToList();
            return allForms;
        }

        public void updateForm(int pk, string name, string value)
        {
            Forms form = ctx.Forms.Find(pk);
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
            ctx.SaveChanges();
        }

        public void deleteForm(int pk) 
        {
            Forms form = ctx.Forms.Find(pk);
            firstDeleteDynamicForm(pk);
            ctx.Forms.Remove(form);
            ctx.SaveChanges();
        }

        /*
         * Not good solution for cascade deleting, but I had no time to search for better solution
         *
        private void firstDeleteDynamicForm(int formId) 
        {
            var query = from dynamic in ctx.Dynamics where dynamic.form.ID == formId select dynamic;
            if(query.Count()!=0)
            {
                Dynamic dyn = ctx.Dynamics.Find(query.First().ID);
                ctx.Dynamics.Remove(dyn);
            }
        }

        public String getFormsLongDescription(int pk) 
        {
            Forms form = ctx.Forms.Find(pk);
            return form.Description;
        }

        private DBContext ctx;
    }
}*/