using IdentitySample.Models;
using PSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSIMS.Controllers
{
    public class RolePermissionController : Controller
    {
        private Object role = String.Empty;
        // GET: RolePermission
        public ActionResult Index(string RoleName)
        {
            var context = new ApplicationDbContext();
            var model = new List<MenuViewModal>();

            // Repopulate Dropdown Lists
            var rolelist = from r in context.Roles
                           orderby r.Name
                           select r.Name;

            ViewBag.Roles = new SelectList(rolelist);


            //-----------------------START INSERT INCASE THE PERMISSION NOT EXISTS IN A ROLE------------------------//
            //----------------------ADD THIS WHEN THE ROLE CREATED TO INITIALIZE PERMISSION-------------------------//
            //----------------------UNDER REVIEW BUT WORKING--------------------------------------------------------//

            if (RoleName != null)
            {
                var RoleId = from r in context.Roles
                             where r.Name == RoleName
                             select r.Id;

                role = RoleId;

                var tempMenuModel = (from p in context.MenuModels.AsEnumerable()
                                     select (new MenuUserRoleModel
                                     {
                                         MenuId = p.MenuId,
                                         Enabled = p.Enabled
                                     })
                                             ).OrderBy(m => m.MenuId).ToList();

                foreach (MenuUserRoleModel menu in tempMenuModel)
                {
                    menu.RoleId = RoleId.FirstOrDefault();
                    if (!context.MenuUserRoleModels.Any(x => (x.MenuId == menu.MenuId) && (x.RoleId == menu.RoleId)))
                    {
                        context.MenuUserRoleModels.Add(menu);
                    }
                }
                context.SaveChanges();
            }
            //-----------------------END INSERT INCASE THE PERMISSION NOT EXISTS IN A ROLE------------------------//

            if (!String.IsNullOrEmpty(RoleName))
            {

                model = (from p in context.MenuModels.AsEnumerable()
                         join q in context.MenuUserRoleModels on p.MenuId equals q.MenuId
                         join r in context.Roles on q.RoleId equals r.Id
                         where r.Name == RoleName
                         select (new MenuViewModal
                         {
                             MenuId = p.MenuId,
                             MainMenuName = p.MainMenuName,
                             SubMenuName = p.SubMenuName,
                             Url = p.Url,
                             FaIconName = p.FaIconName,
                             Enabled = q.Enabled,
                             RoleId = r.Id
                         })
                                        ).OrderBy(m => m.MenuId).ToList();

            }
            ////               from p in ps.DefaultIfEmpty();         
            ////var menulist = context.MenuModels.OrderBy(m => m.MenuId).ToList();//.Select(rr => new SelectListItem { Value = rr.MainMenuName.ToString(), Text = rr.MainMenuName }).ToList();
            //ViewBag.menulist = menulist;

            return View("Index", model);
        }

        public ActionResult GetRolePermission(String RoleName)
        {
            if (RoleName == null)
            {
                throw new ArgumentNullException("context", "Context must not be null.");
            }

            var context = new ApplicationDbContext();

            var model = (from p in context.MenuModels.AsEnumerable()
                         join q in context.MenuUserRoleModels on p.MenuId equals q.MenuId
                         join r in context.Roles on q.RoleId equals r.Id
                         where r.Name == RoleName
                         select (new MenuModels
                         {
                             MenuId = p.MenuId,
                             MainMenuName = p.MainMenuName,
                             SubMenuName = p.SubMenuName,
                             Url = p.Url,
                             FaIconName = p.FaIconName,
                             Enabled = q.Enabled
                         })
                ).OrderBy(m => m.MenuId).ToList();
            //}


            return PartialView("_PermissionPartialView", model);
        }

        public JsonResult InsertPermission(List<MenuUserRoleModel> menuUserRoleModel)
        {
            var context = new ApplicationDbContext();

            if (context == null)
            {
                throw new ArgumentNullException("context", "Context must not be null.");
            }

            foreach (MenuUserRoleModel menu in menuUserRoleModel)
            {
                if (!context.MenuUserRoleModels.Any(x => (x.MenuId == menu.MenuId) && (x.RoleId == menu.RoleId)))
                {
                    context.MenuUserRoleModels.Add(menu);
                }
                else
                {
                    context.Entry(menu).State = System.Data.Entity.EntityState.Modified;
                }
            }
            context.SaveChanges();

            return Json(1);
        }
    }
}