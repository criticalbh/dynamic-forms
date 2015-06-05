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
    }
}