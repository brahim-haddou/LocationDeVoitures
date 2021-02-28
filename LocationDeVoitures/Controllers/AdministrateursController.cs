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
    [Authorize(Roles = MesConstants.RoleAdministrateur)]
    public class AdministrateursController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = MesConstants.RoleAdministrateur)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAdministrateur)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Administrateur administrateur)
        {

            administrateur.UserID = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Administrateurs.Add(administrateur);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(administrateur);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrateur administrateur = db.Administrateurs.Find(id);
            if (administrateur == null)
            {
                return HttpNotFound();
            }
            return View(administrateur);
        }


        public ActionResult Edit()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Administrateur administrateur = db.Administrateurs.Where(a => a.UserID == user_id).FirstOrDefault();

            if (administrateur == null)
            {
                return HttpNotFound();
            }
            return View(administrateur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(administrateur);
        }
    }
}