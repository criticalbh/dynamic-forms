using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdmirSabanovic.Areas.Admin.Models;
namespace AdmirSabanovic.Repos.Admin
{
    public static class AdminData
    {
        public static object getData(String id) 
        {
            DBContext ctx = new DBContext();
            var query = from admin in ctx.Admins where admin.Email == id select admin;
            Areas.Admin.Models.Admin user = new Areas.Admin.Models.Admin();
            foreach (var item in query)
            {
                user.Email = item.Email;
                user.Name = item.Name;
                user.ID = item.ID;
                user.LastName = item.LastName;
            }
            return user;
        }
    }
}