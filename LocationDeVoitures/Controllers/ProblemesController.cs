using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    public class ProblemesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Problemes
        public ActionResult Index()
        {
            return View(db.Problemes.ToList());
        }


        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Probleme probleme, string id)
        {
            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;

            probleme.UserDefendeurID = user_id;
            probleme.UserPlaignantID = id;
            if (ModelState.IsValid)
            {
                db.Problemes.Add(probleme);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult UserDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if(db.Agences.Any(a => a.UserID == id))
            {
                return RedirectToAction("Details", "Agences", new { id = db.Agences.Where(a => a.UserID == id).FirstOrDefault().AgenceID });
            }
            else if (db.Locataires.Any(a => a.UserID == id))
            {
                return RedirectToAction("Details", "Locataires", new { id = db.Locataires.Where(a => a.UserID == id).FirstOrDefault().LocataireID });
            }
            return View();
        }
    }
}