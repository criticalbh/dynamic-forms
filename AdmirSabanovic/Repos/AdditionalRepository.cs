using AdmirSabanovic.Areas.User.Models;
using AdmirSabanovic.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Repos
{
    public class AdditionalRepository : GenericRepository<DBContext, Additional>
    {
        public void storeDynamicFields(User user, Hashtable dynamicFields)
        {
            User user_noSAVE = Context.Users.AsNoTracking().Where(s => s.ID == user.ID).First();

            Context.Entry(user_noSAVE).State = EntityState.Unchanged;

            Context.Users.Attach(user_noSAVE);
            foreach (String item in dynamicFields.Keys)
            {
                Additional atd = new Additional();
                atd.UserID = user_noSAVE;
                atd.Key = item;
                atd.Value = dynamicFields[item].ToString();
                Add(atd);
            }
            Save();
        }

        public List<Additional> getAllDynamicFIeldsByUserID(int userID)
        {
            return FindBy(s => s.UserID.ID == userID).ToList();
        }


        public List<String> getAllKeys(int formID)
        {
            UserRepository usr = new UserRepository();
            User user = usr.GetAnyByFormId(formID);
            return FindBy(u => u.UserID.ID == user.ID).GroupBy(a => a.Key).Select(a => a.Key).ToList();
        }

        public void UpdateByKeyAndUserId(int id, string key, string value)
        {
            Additional atd = FindBy(a => a.UserID.ID == id && a.Key == key).First();
            atd.Value = value;
            Edit(atd);
            Save();
        }
        //The object cannot be deleted because it was not found in the ObjectStateManager
        //I have no more time to debug this. I have to finish senior design.. I hope this is enoguh :(
        public void DeleteByUserId(int id)
        {
            List<Additional> atd = FindBy(u => u.UserID.ID == id).ToList();
            foreach (Additional item in atd)
            {
                Delete(item);
                Context.Entry(item).State = EntityState.Deleted;
                Context.SaveChanges();  
            }
        }
    }
}