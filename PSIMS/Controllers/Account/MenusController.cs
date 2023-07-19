using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models;

namespace PSIMS.Controllers
{
    public class MenusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menus
        public ActionResult Index()
        {
            return View(db.MenuModels.OrderByDescending(m=>m.MenuId).ToList());
        }

        // GET: Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModels menuModels = db.MenuModels.Find(id);
            if (menuModels == null)
            {
                return HttpNotFound();
            }
            return View(menuModels);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuId,MainMenuName,SubMenuName,Url,FaIconName")] MenuModels menuModels)
        {
            if (ModelState.IsValid)
            {
                db.MenuModels.Add(menuModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuModels);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModels menuModels = db.MenuModels.Find(id);
            if (menuModels == null)
            {
                return HttpNotFound();
            }
            return View(menuModels);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuId,MainMenuName,SubMenuName,Url,FaIconName")] MenuModels menuModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuModels);
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModels menuModels = db.MenuModels.Find(id);
            if (menuModels == null)
            {
                return HttpNotFound();
            }
            return View(menuModels);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuModels menuModels = db.MenuModels.Find(id);
            db.MenuModels.Remove(menuModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
