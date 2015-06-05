using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdmirSabanovic.Areas.Admin.Models
{
    public class Forms
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(160, MinimumLength = 2)]
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public Admin Admin { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

        public ICollection<AdmirSabanovic.Areas.User.Models.User> users { get; set; }
    }
}