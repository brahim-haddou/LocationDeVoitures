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
    public class OffresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offres
        public ActionResult Index()
        {
            var offres = db.Offres.Include(o => o.VoitureOffre);
            return View(offres.ToList());
        }

        // GET: Offres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offre offre = db.Offres.Find(id);
            if (offre == null)
            {
                return HttpNotFound();
            }
            return View(offre);
        }

        // GET: Offres/Create
        public ActionResult Create(int id)
        {
            ViewBag.Id_Voiture = id;
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule");
            return View();
        }

        // POST: Offres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OffreID,Pourcentage,Data,Duree,VoitureID")] Offre offre, int Id_Voiture)
        {
            offre.VoitureID = Id_Voiture;
            if (ModelState.IsValid)
            {
                db.Offres.Add(offre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", offre.VoitureID);
            return View(offre);
        }

        // GET: Offres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offre offre = db.Offres.Find(id);
            if (offre == null)
            {
                return HttpNotFound();
            }
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", offre.VoitureID);
            return View(offre);
        }

        // POST: Offres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OffreID,Pourcentage,Data,Duree,VoitureID")] Offre offre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VoitureID = new SelectList(db.Voitures, "VoitureID", "Matricule", offre.VoitureID);
            return View(offre);
        }

        // GET: Offres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offre offre = db.Offres.Find(id);
            if (offre == null)
            {
                return HttpNotFound();
            }
            return View(offre);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offre offre = db.Offres.Find(id);
            db.Offres.Remove(offre);
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
