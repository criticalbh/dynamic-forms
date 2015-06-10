using AdmirSabanovic.Repos;
using AdmirSabanovic.Repos.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AdmirSabanovic.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View("login");
        }

        [HttpPost]
        public ActionResult Index(AdmirSabanovic.Areas.Admin.Models.Admin admin) {
            
            if(ModelState.IsValid)
            {
                if(isValid(admin.Email, admin.Password)){
                    FormsAuthentication.SetAuthCookie(admin.Email, false);
                    return RedirectToAction("index", "dashboard");
                }
            }

            return RedirectToAction("index", "dashboard");
            
        }

        public ActionResult logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "login");
        }

        //registration
        /*
         * crypto new simple crypto
         * var encrypass = crypto.computer password
         * 
         * logout
         * formauthentication.signout
         */
        public void createAcc(String email, String pw)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var encr = crypto.Compute(pw);
            DBContext db = new DBContext();
            AdmirSabanovic.Areas.Admin.Models.Admin newAdmin = new AdmirSabanovic.Areas.Admin.Models.Admin();
            newAdmin.Email = email;
            newAdmin.Password = encr.ToString();
            newAdmin.Name = "testni";
            newAdmin.LastName = "lik";
            db.Admins.Add(newAdmin);
            db.SaveChanges();
        }
        private bool isValid(string email, string password) {
            DBContext db = new DBContext();
            var crypto = new SimpleCrypto.PBKDF2();
            String encr = crypto.Compute(password).ToString(); ;
            var admin = db.Admins.FirstOrDefault(u => u.Email.CompareTo(email) == 1); 
            if(admin != null){
                return true;
            }
            return false;
        }

    }
}
