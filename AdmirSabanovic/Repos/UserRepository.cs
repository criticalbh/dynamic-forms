using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Areas.User.Models;
using AdmirSabanovic.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Repos
{
    public class UserRepository: GenericRepository<DBContext, User>
    {
        public User GetSingle(int fooId)
        {
            var query = GetAll().FirstOrDefault(x => x.ID == fooId);
            return query;
        }

        public User GetAnyByFormId(int formID)
        {
            var q = GetAll().Where(s => s.Form_ID.ID == formID).First();
            return q;
        }

        public int CountAllByFormId(int formID) 
        {
            var q = GetAll().Where(s => s.Form_ID.ID == formID).ToList();
            return q.Count();
        }

        public int[] CountingStatistics(int formID) {
            int[] stats = new int[2];

            int all = GetAll().Where(u => u.Form_ID.ID == formID).ToList().Count();
            int activated = GetAll().Where(u => u.Form_ID.ID == formID && u.isActivated == true).ToList().Count();
            stats[0] = all;
            stats[1] = activated;
            return stats;
        }

        public List<Hashtable> GetStatisticsByCreationByFormId(int formID)
        {
            List<Hashtable> returnTableInList = new List<Hashtable>();
            List<DateTime?> dateTimes = dateTimeDistincted(formID);
            foreach (var item in dateTimes)
            {
                var q = (from t in GetAll()
                         where EntityFunctions.TruncateTime(t.Registered) == item
                                              select t).ToList();
                Hashtable table = new Hashtable();
                table.Add("day", item.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                table.Add("value", q.Count());
                returnTableInList.Add(table);
            }
            return returnTableInList;
        }

        private List<DateTime?> dateTimeDistincted(int formID)
        {
            var distinctYearMonthsDays = (from t in GetAll()
                                          where t.Form_ID.ID == formID
                                          select EntityFunctions.TruncateTime(t.Registered)).Distinct().ToList();
            return distinctYearMonthsDays;
        }

        public IQueryable<User> getAllAsIQueryable()
        {
            IQueryable<User> allUsers = GetAll().AsQueryable();
            return allUsers;
        }

        public IQueryable<User> getAllAsIQueryableByForm(int formID)
        {
            IQueryable<User> allUsers = FindBy(s => s.Form_ID.ID == formID).AsQueryable();
            return allUsers;
        }

        public IQueryable<User> sortAllUsersAsIQueryable(IQueryable<User> filteredUsers, int iSortCol,
            String sSortDir)
        {
            switch (iSortCol)
            {
                case 1:
                    if (sSortDir == "asc") { filteredUsers = filteredUsers.OrderBy(p => p.Name); }
                    if (sSortDir == "desc") { filteredUsers = filteredUsers.OrderByDescending(p => p.Name); }
                    break;
                case 2:
                    if (sSortDir == "asc") { filteredUsers = filteredUsers.OrderBy(p => p.Surname); }
                    if (sSortDir == "desc") { filteredUsers = filteredUsers.OrderByDescending(p => p.Surname); }
                    break;
                case 3:
                    if (sSortDir == "asc") { filteredUsers = filteredUsers.OrderBy(p => p.Email); }
                    if (sSortDir == "desc") { filteredUsers = filteredUsers.OrderByDescending(p => p.Email); }
                    break;
                case 4:
                    if (sSortDir == "asc") { filteredUsers = filteredUsers.OrderBy(p => p.isActivated); }
                    if (sSortDir == "desc") { filteredUsers = filteredUsers.OrderByDescending(p => p.isActivated); }
                    break;
                default:
                    if (sSortDir == "asc") { filteredUsers = filteredUsers.OrderBy(p => p.Registered); }
                    if (sSortDir == "desc") { filteredUsers = filteredUsers.OrderByDescending(p => p.Registered); }
                    break;
            }
            return filteredUsers;
        }

        public User storeUserFromStaticFields(Hashtable staticFields, int formID)
        {
            Forms form = Context.Forms.AsNoTracking().Where(s => s.ID == formID).First();
            Context.Entry(form).State = EntityState.Unchanged;
            Context.Forms.Attach(form);

            AdmirSabanovic.Areas.User.Models.User user = new AdmirSabanovic.Areas.User.Models.User();
            user.Name = staticFields["name"].ToString();
            user.Surname = staticFields["surname"].ToString();
            user.Email = staticFields["email"].ToString();
            user.Token = generateRandomToken(8);
            user.ActivationCode = generateRandomToken(16);
            user.isActivated = false;
            user.Registered = DateTime.Now;
            user.Form_ID = form;
            Add(user);
            Save();

            MailerRepo mr = new MailerRepo();
            mr.To = user.Email;
            mr.Subject = "Email Verification";
            mr.Body = "Dear " + user.Name + " Please verify your email: " +
            "http://localhost:15999/verify?id=" + user.ID + "&code=" + user.ActivationCode;
            mr.Send();
            Context.Entry(form).State = EntityState.Detached;
            return user;
        }

        public void UpdateUserByIdAndKey(int id, string key, string value)
        {
            User user = FindBy(u => u.ID == id).First();
            switch(key)
            {
                case "Name":
                    user.Name = value;
                    break;
                case "Surname":
                    user.Surname = value;
                    break;
                case "Email":
                    user.Email = value;
                    break;
                case "Token":
                    user.Token = value;
                    break;
            }
            Edit(user);
            Save();
        }

        public void DeleteById(int id)
        {
            User usr = FindBy(u => u.ID == id).First();
            Delete(usr);
            Save();
        }

        private String generateRandomToken(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        internal void verifyUser(int id, string code)
        {
            User userToVerify = FindBy(s=> s.ID == id && s.ActivationCode.Equals(code) == true).First();
            userToVerify.isActivated = true;
            userToVerify.ActivationCode = null;
            Edit(userToVerify);
            Save();
        }
    }
}