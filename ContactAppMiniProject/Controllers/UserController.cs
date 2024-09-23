using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ContactApp_MiniProject.ViewModels;
using ContactAppMiniProject.Data;
using ContactAppMiniProject.Models;
using NHibernate.Linq;
using BC = BCrypt.Net;

namespace ContactAppMiniProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using (var s = Helper.CreateSession())
            {
                var users = s.Query<User>().ToList();
                return View(users);
            }

        }
        [AllowAnonymous]
        public ActionResult Redirect()
        {
            if (Session["User"] == null)
                return RedirectToAction("logout", "User");
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index");
                return RedirectToAction("Index", "Contact");
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View();
            using (var s = Helper.CreateSession())
            {
                var user = s.Query<User>().Fetch(u => u.Role).FetchMany(u => u.Contacts).FirstOrDefault(u => u.UserName == loginVM.UserName);

                if (user != null && user.IsActive)
                {
                    if (BC.BCrypt.EnhancedVerify(loginVM.Password, user.Password))
                    {
                        Session["User"] = user;
                        FormsAuthentication.SetAuthCookie(loginVM.UserName, true);
                        if (user.IsAdmin)
                            return RedirectToAction("Index", "User");
                        return RedirectToAction("Index", "Contact");
                    }
                }
                ModelState.AddModelError("", "Username/ Password don't match");
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View();
            user.Password = BC.BCrypt.EnhancedHashPassword(user.Password, 4);
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    user.Role = new Role();
                    if (user.IsAdmin)
                        user.Role.RoleName = "Admin";
                    else
                        user.Role.RoleName = "Staff";

                    user.Contacts = new List<Contact>();
                    user.IsActive = true;
                    user.Role.User = user;
                    s.Save(user);
                    txn.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var existingUser = s.Query<User>().FirstOrDefault(u => u.Id == id);
                    if (existingUser.Role.RoleName == "Admin")
                    {
                        var admins = s.Query<User>().Where(u => u.Role.RoleName == "Admin").ToList();
                        if (admins.Count() == 1)
                            return Json(new { active = false, message = "1 admin member should be present, Cannot delete user!" });
                    }
                    else if (existingUser.Role.RoleName == "Staff")
                    {
                        var staff = s.Query<User>().Where(u => u.Role.RoleName == "Staff").ToList();
                        if (staff.Count() == 1)
                            return Json(new { active = false, message = "1 staff member should be present, Cannot delete user!" });
                    }
                    else if(existingUser.UserName == User.Identity.Name)
                    {
                        return Json(new { active = false, message = "Cannot delete self!" });
                    }
                    

                    existingUser.IsActive = !existingUser.IsActive;
                    s.Update(existingUser);
                    txn.Commit();
                    return Json(new { active = true, message="User Deleted Successfully!" });


               }
            }
        }
        public ActionResult Edit(Guid id)
        {
            using (var s = Helper.CreateSession())
            {
                var user = s.Query<User>().FirstOrDefault(u => u.Id == id);
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
                return View();
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var existingUser = s.Query<User>().FirstOrDefault(u => u.Id == user.Id);

                    existingUser.UserName = user.UserName;

                    s.Update(existingUser);
                    txn.Commit();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}