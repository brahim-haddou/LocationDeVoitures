using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocationDeVoitures.Models;

namespace LocationDeVoitures.Controllers
{
    [Authorize()]
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Locations
        public ActionResult Index()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;

            if (User.IsInRole(MesConstants.RoleLocataire))
            {
                return View(db.Locations.Where(l => l.LocataireLocation.UserID == user_id));
            }
            else if (User.IsInRole(MesConstants.RoleAgence))
            {
                return View(db.Locations.Where(l => l.VoitureLocation.Agence.UserID == user_id));
            }
            else if (User.IsInRole(MesConstants.RoleAdministrateur))
            {
                var locations = db.Locations.Include(l => l.LocataireLocation).Include(l => l.VoitureLocation);
                return View(locations.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.idVoiture = location.VoitureLocation.VoitureID;
            return View(location);
        }

        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Id_Voiture = id;
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = MesConstants.RoleLocataire)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationID,VoitureID,LocataireID,Data,Duree,ChoiseDePayment,Status")] Location location, int Id_Voiture)
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Locataire myLocataire = db.Locataires.Where(v => v.UserID == user_id).FirstOrDefault();

            location.LocataireID = myLocataire.LocataireID;
            location.VoitureID = Id_Voiture;
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.LocataireID = new SelectList(db.Locataires, "LocataireID", "Nom", location.LocataireID);
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", location.VoitureID);
            return View(location);
        }

        // GET: Locations/Edit/5
        [Authorize(Roles = MesConstants.RoleLocataire+","+ MesConstants.RoleAgence)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocataireID = new SelectList(db.Locataires, "LocataireID", "Nom", location.LocataireID);
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", location.VoitureID);
            ViewBag.Voiture = location.VoitureID;
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MesConstants.RoleLocataire + "," + MesConstants.RoleAgence)]
        public ActionResult Edit([Bind(Include = "LocationID,VoitureID,LocataireID,Data,Duree,ChoiseDePayment,Status")] Location location)
        {
            if (User.IsInRole(MesConstants.RoleLocataire))
            {
                location.Status = false;
            }
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LocataireID = new SelectList(db.Locataires, "LocataireID", "Nom", location.LocataireID);
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", location.VoitureID);
            return View(location);
        }

        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MesConstants.RoleLocataire)]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
