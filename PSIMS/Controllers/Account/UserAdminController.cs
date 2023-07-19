using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class UsersAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {

            return View(await UserManager.Users.Include(i=>i.Location).ToListAsync());

        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
           // ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            ViewBag.LocationID = new SelectList(db.Locations.OrderByDescending(m => m.LocationName), "ID", "LocationName");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = userViewModel.Username, 
                    Email = userViewModel.Email, 
                    FullName = userViewModel.FullName, 
                    Address= userViewModel.Address,
                    ContactNo = userViewModel.ContactNo,
                    EmailConfirmed = true, //HARDCODE
                    LocationID = userViewModel.LocationID,
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    string callbackUrl=SendEmailConfirmationToken(user.Id,"Confirm your account"); 
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.LocationID = new SelectList(db.Locations.OrderByDescending(m => m.LocationName), "ID", "LocationName");
                   // ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        private string SendEmailConfirmationToken(string userID,string subject)
        {
            string code = UserManager.GenerateEmailConfirmationToken(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
             UserManager.SendEmail(userID, subject, "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            return callbackUrl;
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Deactivate(string id)
        {
            //var user = await UserManager.FindByIdAsync(id);
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.Active = false;
            //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            
            return View("Index", await UserManager.Users.Include(i => i.Location).ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Activate(string id)
        {
            //var user = await UserManager.FindByIdAsync(id);
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.Active = true;
            //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return View("Index", await UserManager.Users.Include(i => i.Location).ToListAsync());
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.LocationID = new SelectList(db.Locations.OrderByDescending(m => m.LocationName), "ID", "LocationName");

            return View(new EditUserViewModel()
            {
            

                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                ContactNo = user.ContactNo,
                LocationID=user.LocationID
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Username;
                user.Email = editUser.Email;
                user.FullName = editUser.FullName;
                user.Address = editUser.Address;
                user.ContactNo = editUser.ContactNo;
                user.LocationID = editUser.LocationID;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
