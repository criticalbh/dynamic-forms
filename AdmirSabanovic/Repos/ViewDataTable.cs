using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Areas.User.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace AdmirSabanovic.Repos
{
    public class ViewDataTable
    {
        public ViewDataTable()
        {
            db = new DBContext();
            userRepo = new UserRepository();
            additionalRepo = new AdditionalRepository();
        }

        public Hashtable returnTableData(String search, int iDisplayLength, int iDisplayStart, int iSortCol,
            String sSortDir, int formID)
        {
            IQueryable<User> allUsers = userRepo.getAllAsIQueryableByForm(formID);
            IQueryable<User> filteredUsers = allUsers;

            if (search != null)
            {
                filteredUsers = from s in filteredUsers
                                where s.Name.Contains(search) ||
                                    s.Email.Contains(search) ||
                                    s.Surname.Contains(search)
                                select s;
            }

            if (iSortCol != 0)
            {
                filteredUsers = userRepo.sortAllUsersAsIQueryable(filteredUsers, iSortCol, sSortDir)
                    .Skip(iDisplayStart).Take(iDisplayLength);
            }
            else 
            {
                //for some reason they want to have orderby before skip
                filteredUsers = filteredUsers.OrderBy(f => f.ID).Skip(iDisplayStart).Take(iDisplayLength);
            }

            int count = 1;
            List<List<string>> lightUserList = new List<List<string>>();
            foreach (User currUser in filteredUsers)
            {
                List<string> newUser = new List<string>();
                newUser.Add(count++.ToString());
                newUser.Add(currUser.Name);
                newUser.Add(currUser.Surname);
                newUser.Add(currUser.Email);
                newUser.Add(currUser.isActivated == false ? "no" : "yes");
                newUser.Add(currUser.Token);
                newUser.Add(currUser.Registered.ToString());
                foreach (Additional item in additionalRepo.getAllDynamicFIeldsByUserID(currUser.ID))
                {
                    newUser.Add(item.Value);
                }
                lightUserList.Add(newUser);
            }

            Hashtable returnTable = new Hashtable();
            returnTable.Add("iTotalRecords", allUsers.Count());
            returnTable.Add("iTotalDisplayRecords", allUsers.Count());
            returnTable.Add("aaData", lightUserList);

            return returnTable;
        }
        DBContext db;
        UserRepository userRepo;
        AdditionalRepository additionalRepo;
    }
}