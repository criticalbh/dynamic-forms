using AdmirSabanovic.Areas.Admin.Models;
using AdmirSabanovic.Areas.User.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdmirSabanovic
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("IBUConString")
        {

        }

        public DbSet<Forms> Forms { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Dynamic> Dynamics { get; set; }
        public DbSet<User> Users { get; set; }
    }
}