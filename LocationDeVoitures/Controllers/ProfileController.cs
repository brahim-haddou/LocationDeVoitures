using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    public class ProfileController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = MesConstants.RoleAdministrateur)]
        public ActionResult Administrateur()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAdministrateur)]
        [ValidateAntiForgeryToken]
        public ActionResult Administrateur(Administrateur administrateurs)
        {

            administrateurs.UserID = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Administrateurs.Add(administrateurs);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(administrateurs);
        }


        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Agence()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAgence)]
        [ValidateAntiForgeryToken]
        public ActionResult Agence(Agence agence)
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

        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult Locataire()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleLocataire)]
        [ValidateAntiForgeryToken]
        public ActionResult Locataire(Locataire locataire)
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

    }
}