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
            /*
            if(ModelState.IsValid)
            {
                if(isValid(admin.Email, admin.Password)){
                    FormsAuthentication.SetAuthCookie(admin.Email, false);
                    return RedirectToAction("Admin/Dashboard");
                }
            }

            return RedirectToAction("Admin/Dashboard");*/

            FormsAuthentication.SetAuthCookie(admin.Email, false);
           /* var data = admin;
            return RedirectToAction("index", "dashboard", new { data = data });*/

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

        private bool isValid(string email, string password) {
            var crypto = new SimpleCrypto.PBKDF2();
            using (var db = new DBContext())
            {
                var user = db.Admins.FirstOrDefault(
                    u => u.Email == email
                    );
                if(user != null){
                    if(user.Password == crypto.Compute(password)){
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
