using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PSIMS.Models;
using PSIMS.Models.Account;
using IdentitySample.Models;
using IdentitySample.Controllers;



namespace PSIMS.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        public ActionResult Index()
        {
            // Populate Dropdown Lists
            var context = new ApplicationDbContext();

            var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;

            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            var accessTypes = context.AccessTypes.ToList().Select(a => new SelectListItem {Value=a.Code,Text=a.Name }).ToList();
            ViewBag.AccessTypes = accessTypes;

            var menulist = context.MenuModels.OrderBy(m => m.MenuId).ToList();//.Select(rr => new SelectListItem { Value = rr.MainMenuName.ToString(), Text = rr.MainMenuName }).ToList();
            ViewBag.menulist = menulist;

            ViewBag.Message = "";

            return View();
        }


        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var context = new ApplicationDbContext();
                var identityRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name=collection["RoleName"]
                };
                context.Roles.Add(identityRole);

                context.SaveChanges();

                context.RoleAccessTypes.Add(new Models.Account.RoleAccessType()
                {
                    AccessTypeCode = collection["AccessType"],
                    RoleID = identityRole.Id
                });

                context.SaveChanges();

                ViewBag.Message = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(string RoleName)
        {
            var context = new ApplicationDbContext();
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var roleAccsTyp = context.RoleAccessTypes.FirstOrDefault(x => x.RoleID == thisRole.Id);
            if(roleAccsTyp!=null)
                context.RoleAccessTypes.Remove(roleAccsTyp);
            if (thisRole != null)
                context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var context = new ApplicationDbContext();

            RoleAccessType selectedRolAccTyp = context.RoleAccessTypes.FirstOrDefault(x => x.Role.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

            var accTypList = context.AccessTypes;
            ViewBag.AccessTypes = new SelectList(accTypList, "Code", "Name", selectedRolAccTyp);

            RoleEditViewModel vm = context.Roles.Select(r => new RoleEditViewModel
            {
                AccessTypeCode=selectedRolAccTyp.AccessTypeCode,
                Id=r.Id,
                Name=r.Name
            }).FirstOrDefault(r=>r.Name.Equals(roleName,StringComparison.CurrentCultureIgnoreCase));
            return View(vm);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleEditViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var context = new ApplicationDbContext();
                    Microsoft.AspNet.Identity.EntityFramework.IdentityRole role = new IdentityRole()
                    {
                        Id = vm.Id,
                        Name = vm.Name
                    };
                    context.Roles.Attach(role);
                    context.Entry(role).State = System.Data.Entity.EntityState.Modified;

                    //check if role has access type
                    RoleAccessType existingObj = context.RoleAccessTypes.FirstOrDefault(a => a.RoleID == vm.Id);
                    if (existingObj == null)
                    {
                        //add role access type
                        RoleAccessType rolAccTyp = new RoleAccessType
                        {
                            AccessTypeCode = vm.AccessTypeCode,
                            RoleID = role.Id
                        };
                        context.RoleAccessTypes.Add(rolAccTyp);
                    }
                    else
                    {
                        ////edit existing one.
                        existingObj.AccessTypeCode = vm.AccessTypeCode;
                        context.Entry(existingObj).State = System.Data.Entity.EntityState.Modified;
                    }

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(vm);
            }
            catch
            {
                return View();
            }
        }

        //  Adding Roles to a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var context = new ApplicationDbContext();

            if (context == null)
            {
                throw new ArgumentNullException("context", "Context must not be null.");
            }

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.AddToRole(user.Id, RoleName);


            ViewBag.Message = "Role created successfully !";

            // Repopulate Dropdown Lists
            var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            var accessTypes = context.AccessTypes.ToList().Select(a => new SelectListItem { Value = a.Code, Text = a.Name }).ToList();
            ViewBag.AccessTypes = accessTypes;

            return View("Index");
        }


        //Getting a List of Roles for a User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var context = new ApplicationDbContext();
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);


                // Repopulate Dropdown Lists
                var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = rolelist;
                var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
                new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                ViewBag.Users = userlist;

                var accessTypes = context.AccessTypes.ToList().Select(a => new SelectListItem { Value = a.Code, Text = a.Name }).ToList();
                ViewBag.AccessTypes = accessTypes;

                ViewBag.Message = "Roles retrieved successfully !";
            }
            return View("Index");
        }


        //Deleting a User from A Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.Message = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.Message = "This user doesn't belong to selected role.";
            }

            // Repopulate Dropdown Lists
            var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            var accessTypes = context.AccessTypes.ToList().Select(a => new SelectListItem { Value = a.Code, Text = a.Name }).ToList();
            ViewBag.AccessTypes = accessTypes;

            return View("Index");
        }
    }
}