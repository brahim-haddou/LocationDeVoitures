using LocationDeVoitures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocationDeVoitures.Controllers
{
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

        public ActionResult Add(string UserID, string UserFavorisID)
        {
            ListFavoris favoris = new ListFavoris()
            {
                UserID = UserID,
                UserFavorisID = UserFavorisID
            };
            db.ListFavoris.Add(favoris);
            return View();
        }
        public ActionResult Remove(string UserID, string UserFavorisID)
        {
            ListFavoris favoris = db.ListFavoris.Where(l => l.UserID == UserID && l.UserFavorisID == UserFavorisID).FirstOrDefault();
            db.ListFavoris.Remove(favoris);
            db.SaveChanges();
            return View();
        }
    }
}