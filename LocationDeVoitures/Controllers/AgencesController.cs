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
    public class AgencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Agences.ToList());
        }

        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAgence)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agence agence)
        {

            agence.UserID = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Agences.Add(agence);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(agence);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agence agence = db.Agences.Find(id);
            if (agence == null)
            {
                return HttpNotFound();
            }
            return View(agence);
        }


        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Edit()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Agence agence = db.Agences.Where(a => a.UserID == user_id).FirstOrDefault();
            if (agence == null)
            {
                return HttpNotFound();
            }
            return View(agence);
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAgence)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Agence agence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(agence);
        }


        // GET: Offres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agence agence = db.Agences.Find(id);
            if (agence == null)
            {
                return HttpNotFound();
            }
            return View(agence);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agence agence = db.Agences.Find(id);
            db.Agences.Remove(agence);
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