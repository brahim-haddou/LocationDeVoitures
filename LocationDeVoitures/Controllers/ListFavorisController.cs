using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
    [Authorize]
    public class ListFavorisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {

            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (User.IsInRole(MesConstants.RoleAdministrateur))
            {
                var ids = db.ListFavoris.Where(l => l.UserID == user_id).Select(l => l.UserFavorisID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    AgenceList = db.Agences.Where(l => ids.Contains(l.UserID)).ToList(),
                    LocataireList = db.Locataires.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);

            }
            else if (User.IsInRole(MesConstants.RoleAgence))
            {
                var ids = db.ListFavoris.Where(l => l.UserID == user_id).Select(l => l.UserFavorisID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    LocataireList = db.Locataires.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);
            }
            else if (User.IsInRole(MesConstants.RoleLocataire))
            {
                var ids = db.ListFavoris.Where(l => l.UserID == user_id).Select(l => l.UserFavorisID);
                MasterModelProfile list = new MasterModelProfile()
                {
                    AgenceList = db.Agences.Where(l => ids.Contains(l.UserID)).ToList()
                };
                return View(list);
            }
            return View();
        }

        public ActionResult Add(string UserFavorisID)
        {
            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            ListFavoris favoris = new ListFavoris()
            {
                UserID = user_id,
                UserFavorisID = UserFavorisID
            };
            db.ListFavoris.Add(favoris);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Remove(string id)
        {
            var user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            ListFavoris favoris = db.ListFavoris.Where(l => l.UserID == user_id && l.UserFavorisID == id).FirstOrDefault();
            db.ListFavoris.Remove(favoris);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}