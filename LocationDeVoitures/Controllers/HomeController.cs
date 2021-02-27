using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Voitures = db.Voitures.Count();
            ViewBag.Agences = db.Agences.Count();
            ViewBag.Locataires = db.Locataires.Count();
            ViewBag.Offres = db.Offres.Count();
            var of = db.Offres.ToList();
            var idsOf = of.Select(o => o.VoitureID);
            var VoiOf = db.Voitures.Where(v => idsOf.Contains(v.VoitureID)).ToList();
            MasterModelVoiture list = new MasterModelVoiture()
            {
                OffreeList = of,
                VoitureList = VoiOf
            };
            return View(list);
        }
    }
}