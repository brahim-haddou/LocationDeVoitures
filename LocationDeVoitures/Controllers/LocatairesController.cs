using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    public class LocatairesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var locataires = db.Locataires.ToList();
            return View(locataires);
        }

        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAdministrateur)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Locataire locataire)
        {

            locataire.UserID = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Locataires.Add(locataire);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(locataire);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locataire locataire = db.Locataires.Find(id);
            if (locataire == null)
            {
                return HttpNotFound();
            }
            return View(locataire);
        }


        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult Edit()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Locataire locataire = db.Locataires.Where(a => a.UserID == user_id).FirstOrDefault();
            if (locataire == null)
            {
                return HttpNotFound();
            }
            return View(locataire);
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleLocataire)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Locataire locataire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locataire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(locataire);
        }



        // GET: Offres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locataire locataire = db.Locataires.Find(id);
            if (locataire == null)
            {
                return HttpNotFound();
            }
            return View(locataire);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Locataire locataire = db.Locataires.Find(id);
            db.Locataires.Remove(locataire);
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