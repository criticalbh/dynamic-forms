using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Areas.User.Models
{
    public class Additional
    {
        public int ID { get; set; }
        public User UserID { get; set; }
        public String Key { get; set; }
        public String Value { get; set; }
    }
}