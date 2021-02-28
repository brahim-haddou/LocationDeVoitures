using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    [Authorize]
    public class ListNoireController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {

            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (User.IsInRole(MesConstants.RoleAdministrateur))
            {
                var ids = db.ListNoire.Where(l => l.UserID == user_id).Select(l => l.UserNoireID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    AgenceList = db.Agences.Where(l => ids.Contains(l.UserID)).ToList(),
                    LocataireList = db.Locataires.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);

            }
            else if (User.IsInRole(MesConstants.RoleAgence))
            {
                var ids = db.ListNoire.Where(l => l.UserID == user_id).Select(l => l.UserNoireID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    LocataireList = db.Locataires.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);
            }
            else if (User.IsInRole(MesConstants.RoleLocataire))
            {
                var ids = db.ListNoire.Where(l => l.UserID == user_id).Select(l => l.UserNoireID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    AgenceList = db.Agences.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);
            }
            return View();
        }

        // GET: ListFavoris
        public ActionResult Add(string UserNoireID)
        {
            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            ListNoire Noire = new ListNoire()
            {
                UserID = user_id,
                UserNoireID = UserNoireID
            };
            db.ListNoire.Add(Noire);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Remove(string id)
        {
            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            ListNoire Noire = db.ListNoire.Where(l => l.UserID == user_id && l.UserNoireID == id).FirstOrDefault();
            db.ListNoire.Remove(Noire);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}