using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdmirSabanovic.Areas.Admin.Models
{
    public class Admin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display (Name="Email: ")]
        [Key]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength=6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public ICollection<Forms> Forms { get; set; }
    }
}