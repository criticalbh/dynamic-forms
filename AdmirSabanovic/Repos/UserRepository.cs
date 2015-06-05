using AdmirSabanovic.Areas.Admin.Models;
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
            Edit(userToVerify);
            Save();
        }
    }
}