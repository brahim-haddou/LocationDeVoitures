using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    public class VoituresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Voitures
        public ActionResult Index()
        {
            return View(db.Voitures.ToList());
        }

        [HttpPost]
        public ActionResult Index(string search, string opt)
        {
            if (Search.Marque.ToString() == opt)
            {
                return View(db.Voitures.Where(v => v.Marque.Equals(search)).ToList());
            }
            else if (Search.Module.ToString() == opt)
            {
                int annee = int.Parse(search);
                return View(db.Voitures.Where(v => v.Module == annee).ToList());
            }
            else if (Search.Couleur.ToString() == opt)
            {
                return View(db.Voitures.Where(v => v.Couleur.Equals(search)).ToList());
            }
            else if (Search.Km.ToString() == opt)
            {
                float km = float.Parse(search);
                return View(db.Voitures.Where(v => v.Km == km).ToList());
            }
            return View(db.Voitures.ToList());
        }

        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Create(Voiture voiture, HttpPostedFileBase imageP, IEnumerable<HttpPostedFileBase> imageS)
        {

            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Agence myAgence = db.Agences.Where(v => v.UserID == user_id).FirstOrDefault();
            if (myAgence == null)
            {
                return RedirectToAction("Agence", "Profile");
            }
            voiture.AgenceID = myAgence.AgenceID;

            byte[] bytesP;
            using (BinaryReader br = new BinaryReader(imageP.InputStream))
            {
                bytesP = br.ReadBytes(imageP.ContentLength);
                voiture.Image = bytesP;
                db.Voitures.Add(voiture);
                db.SaveChanges();
            }
            foreach (var i in imageS)
            {
                byte[] bytesS;
                using (BinaryReader br = new BinaryReader(i.InputStream))
                {
                    bytesS = br.ReadBytes(i.ContentLength);
                    ImagesVoiture imagesVoiture = new ImagesVoiture()
                    {
                        Image = bytesS,
                        VoitureID = voiture.VoitureID,
                    };
                    db.ImagesVoitures.Add(imagesVoiture);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture vol = db.Voitures.Find(id);
            if (vol == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(MesConstants.RoleAgence))
            {
                string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
                Agence myAgence = db.Agences.Where(v => v.UserID == user_id).FirstOrDefault();
                if (vol.AgenceID == myAgence.AgenceID)
                {
                    ViewBag.BOOL = true;
                }
                else
                {
                    ViewBag.BOOL = false;
                }
            }
            else
            {
                ViewBag.BOOL = false;
            }
            MasterModelVoiture details = new MasterModelVoiture()
            {
                VoitureList = new List<Voiture>() { vol },
                OffreeList = db.Offres.Where(o => o.VoitureID == vol.VoitureID).ToList(),
                VoitureListImages = db.ImagesVoitures.Where(i => i.VoitureID == vol.VoitureID).ToList()

            };
            return View(details);
        }


        [Authorize(Roles = MesConstants.RoleAgence)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture voiture = db.Voitures.Find(id);
            if (voiture == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgenceID = new SelectList(db.Agences, "AgenceID", "nom", voiture.AgenceID);
            return View(voiture);
        }

        [HttpPost]
        [Authorize(Roles = MesConstants.RoleAgence)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voiture voiture, HttpPostedFileBase imageP, IEnumerable<HttpPostedFileBase> imageS)
        {


            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            Agence myAgence = db.Agences.Where(v => v.UserID == user_id).FirstOrDefault();
            if (myAgence == null)
            {
                return RedirectToAction("Agence", "Profile");
            }
            voiture.AgenceID = myAgence.AgenceID;

            if (ModelState.IsValid)
            {
                if (imageP != null)
                {
                    byte[] bytesP;
                    using (BinaryReader br = new BinaryReader(imageP.InputStream))
                    {
                        bytesP = br.ReadBytes(imageP.ContentLength);
                        voiture.Image = null;
                        voiture.Image = bytesP;
                    }
                }
                if (imageS.ToArray()[0] != null)
                {
                    List<ImagesVoiture> vs = db.ImagesVoitures.Where(i => i.VoitureID == voiture.VoitureID).ToList();
                    foreach(var e in vs)
                    {
                        db.ImagesVoitures.Remove(e);
                        db.SaveChanges();
                    }
                    foreach (var i in imageS)
                    {
                        byte[] bytesS;
                        using (BinaryReader br = new BinaryReader(i.InputStream))
                        {
                            bytesS = br.ReadBytes(i.ContentLength);
                            ImagesVoiture imagesVoiture = new ImagesVoiture()
                            {
                                Image = bytesS,
                                VoitureID = voiture.VoitureID,
                            };
                            db.ImagesVoitures.Add(imagesVoiture);
                        }
                        db.SaveChanges();
                    }
                }

                db.Entry(voiture).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (OptimisticConcurrencyException)
                {
                    db.Refresh(RefreshMode.ClientWins, db.Voitures);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.AgenceID = new SelectList(db.Agences, "AgenceID", "nom", voiture.AgenceID);
            return View(voiture);
        }

        [Authorize(Roles = MesConstants.RoleAgence+","+MesConstants.RoleAdministrateur)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voiture vol = db.Voitures.Find(id);
            if (vol == null)
            {
                return HttpNotFound();
            }
            db.Voitures.Remove(vol);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}