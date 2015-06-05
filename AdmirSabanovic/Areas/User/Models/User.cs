using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdmirSabanovic.Areas.Admin.Models;

namespace AdmirSabanovic.Areas.User.Models
{
    public class User
    {
        public int ID { get; set; }
        public String Email { get; set; }
        public String Token { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String ActivationCode { get; set; }
        public bool isActivated { get; set; }
        public virtual Forms Form_ID { get; set; }
        public DateTime Registered { get; set; }
        public ICollection<Additional> Additionals { get; set; }
    }
}