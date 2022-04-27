using Events.web.Controllers;
using Events.web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Events.web.Areas.Admin.Controllers
{
    //[MyCustomAuthorize(Roles = "Administrator, QAManager")] //Set role for admin only
    public class UsersController : BaseController //Announce class 'AdminController' from 'BaseController'
    {
        public ActionResult Index() //Publicly get action of 'Users' to set in other files
        {
            //List<ApplicationUser> QACoor = new List<ApplicationUser>();
            //var role = Db.Roles
            //    .Where(r => r.Name == "QACoordinator, QAManager")
            //    .FirstOrDefault();
            //if (role != null)
            //{
            //    var users = Db.Users
            //        .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
            //        .ToList();
            //    return View(users);
            //}
            //return View(QACoor);

            //var user = Db.Users.Where(u => u.Role == "Staff, QACoordinator").ToList();

            //return View(user);
            var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var currentU = Db.Users.Find(currentUser.Id);
            var user = Db.Users.AsQueryable().ToList();
            if (currentU.Role == "Administrator" || currentU.Role == "QAManager")
            {
                user = user
                     .Where(u => u.Role == "QAManager" || u.Role == "QACoordinator").ToList();
                return View(user);
            }
            else if (currentU.Role == "QACoordinator")
            {
                user = user
                .Where(u => u.DepartmentId == currentU.DepartmentId).ToList();
                return View(user);
            }

            return View();
        }

        [HttpGet] //Send data using a query string
        public ActionResult Edit(string id) //Publicly get action of 'Edit' to set in other files
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                var PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return this.Redirect(PreviousUrl);
            }
            return this.View(accountToEdit);
        }

        [HttpPost] //Request method via Http page
        public ActionResult Edit(string id, ApplicationUser user) //Get 'Edit' action to set in other files
        {
            var accountToEdit = this.Db.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (accountToEdit == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (user != null && this.ModelState.IsValid)
            {
                accountToEdit.FullName = user.FullName;
                accountToEdit.StaffId = user.StaffId;

                this.Db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return this.View(user);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            var user = Db.Users.Find(id);
            if (user == null)
            {
                return Redirect(url);
            }
            Db.Users.Remove(user);
            Db.SaveChanges();
            return Redirect(url);
        }

        public ActionResult Role() //Publicly get 'Role' action to detail the role list
        {
            var Roles = Db.Roles.ToList();
            return View(Roles);
        }

        public ActionResult RoleCreate() //Publicly get 'RoleCreate' action to create new roles
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost] //Request method via Http page
        public ActionResult RoleCreate(IdentityRole Role) //Get 'RoleCreate' action to save into database
        {
            Db.Roles.Add(Role);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}