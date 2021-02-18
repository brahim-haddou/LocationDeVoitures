using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LocationDeVoitures.Models;
using System.Collections.Generic;

namespace LocationDeVoitures.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult MyChartAdmin()
        {


            string[] XValues = new string[] { "Agences", "Locataires", "Voitures", "Locations" };
            int[] YValues = new int[]{ db.Agences.Count(), db.Locataires.Count(), db.Voitures.Count(), db.Locations.Count()};

            new System.Web.Helpers.Chart(width: 800, height: 400)
                .AddTitle("Nombre des etudiants par filiere")
                .AddSeries(
                chartType: "Column",
                xValue: XValues,
                yValues: YValues
                ).Write("png");
            return null;
        }
        
        public ActionResult MyChartAgence()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var ag_id = db.Agences.Where(x => x.UserID == user_id).FirstOrDefault().AgenceID;
            var My = db.Voitures.Where(V => V.AgenceID == ag_id);
            string[] XValues = new string[] { "Voitures", "Voutures  used", "Location" };
            int[] YValues = new int[] {
                My.Count(),
                db.Locations.Where(l => !My.Select(v => v.VoitureID).Contains(l.VoitureID)).Count(),
                db.Locations.Where(l => My.Select(v => v.VoitureID).Contains(l.VoitureID)).Count(),
            };

            new System.Web.Helpers.Chart(width: 800, height: 400)
                .AddTitle("Nombre des etudiants par filiere")
                .AddSeries(
                chartType: "Column",
                xValue: XValues,
                yValues: YValues
                ).Write("png");
            return null;
        }
        public ActionResult MyChartLocataire()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var lo_id = db.Locataires.Where(x => x.UserID == user_id).FirstOrDefault().LocataireID;
            string[] XValues = new string[] { "Voitures", "Voutures  used", "Voitures unused"};
            int[] YValues = new int[] {
                db.Voitures.Count(),
                db.Locations.Where(l => l.LocataireID == lo_id && l.Status).Count(),
                db.Locations.Where(l => l.LocataireID == lo_id && !l.Status).Count()
            };

            new System.Web.Helpers.Chart(width: 800, height: 400)
                .AddTitle("Nombre des etudiants par filiere")
                .AddSeries(
                chartType: "Column",
                xValue: XValues,
                yValues: YValues
                ).Write("png");
            return null;
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            MasterModelProfile List = null;
            List<Agence> listAgence = null;
            List<Locataire> listLocataire = null;
            List<Voiture> listVoiture = null;
            List<Location> listLocation = null;

            if (User.IsInRole(MesConstants.RoleAdministrateur))
            {
                listAgence = db.Agences.ToList();
                listLocataire = db.Locataires.ToList();
                listVoiture = db.Voitures.ToList();
                listLocation = db.Locations.ToList();
            }
            else if (User.IsInRole(MesConstants.RoleAgence))
            {
                Agence myAgence = db.Agences.Where(v => v.UserID == user_id).FirstOrDefault();
                if (myAgence == null)
                {
                    return RedirectToAction("Agence", "Profile");
                }
                listVoiture = db.Voitures.Where(v => v.AgenceID == myAgence.AgenceID).ToList();
                var Vids = listVoiture.Select(v => v.VoitureID);
                listLocation = db.Locations.Where(v => Vids.Contains(v.VoitureID)).ToList();
            }
            else if (User.IsInRole(MesConstants.RoleLocataire))
            {

                Locataire myLocataire = db.Locataires.Where(v => v.UserID == user_id).FirstOrDefault();
                if (myLocataire == null)
                {
                    return RedirectToAction("Locataire", "Profile");
                }
                listLocation = db.Locations.Where(v => v.LocataireID == myLocataire.LocataireID).ToList();
            }

            List = new MasterModelProfile()
            {
                AgenceList = listAgence,
                LocationList = listLocation,
                LocataireList = listLocataire,
                VoitureList = listVoiture
            };
            return View(List);
        }

        [HttpGet]
        public ViewResult List(string list)
        {
            string user_id = db.Users.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            MasterModelProfile List = new MasterModelProfile();
            List<Agence> listAgence = null;
            List<Locataire> listLocataire = null;
            if (list == "Favoris")
            {
                var ids = db.ListFavoris.Where(l => l.UserID == user_id).Select(f => f.UserFavorisID);
                if (User.IsInRole(MesConstants.RoleAdministrateur))
                {
                    listAgence = db.Agences.Where(a => ids.Contains(a.UserID)).ToList();
                    listLocataire = db.Locataires.Where(a => ids.Contains(a.UserID)).ToList();
                }
                else if (User.IsInRole(MesConstants.RoleAgence))
                {
                    listLocataire = db.Locataires.Where(a => ids.Contains(a.UserID)).ToList();
                }
                else if (User.IsInRole(MesConstants.RoleLocataire))
                {
                    listAgence = db.Agences.Where(a => ids.Contains(a.UserID)).ToList();
                }
            }
            else if (list == "Noire")
            {

                var ids = db.ListNoire.Where(l => l.UserID == user_id).Select(f => f.UserNoireID);
                if (User.IsInRole(MesConstants.RoleAdministrateur))
                {
                    listAgence = db.Agences.Where(a => ids.Contains(a.UserID)).ToList();
                    listLocataire = db.Locataires.Where(a => ids.Contains(a.UserID)).ToList();
                }
                else if (User.IsInRole(MesConstants.RoleAgence))
                {
                    listLocataire = db.Locataires.Where(a => ids.Contains(a.UserID)).ToList();
                }
                else if (User.IsInRole(MesConstants.RoleLocataire))
                {
                    listAgence = db.Agences.Where(a => ids.Contains(a.UserID)).ToList();
                }
            }

            List.AgenceList = listAgence;
            List.LocataireList = listLocataire;
            return View(List);
        }
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}