using SRSprojekt.Misc;
using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SRSprojekt.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
    public class KorisniciController : Controller
    {
        BazaDB bazaDB = new BazaDB();
        // GET: Korisnici
        public ActionResult Index()
        {
            var listaKorisnika = bazaDB.KorisnikBaza.OrderBy(x => x.sifraOvlasti).ThenBy(x => x.Prezime).ToList();
            return View(listaKorisnika);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Prijava(string returnURL)
        {
            KPrijava model = new KPrijava();
            ViewBag.ReturnURL = returnURL;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Prijava(KPrijava model, string returnURL)
        {
            if (ModelState.IsValid)
            {
                var korisnikBaza = bazaDB.KorisnikBaza.FirstOrDefault(x => x.KorisnikName == model.KorisnickoIme);
                if (korisnikBaza != null)
                {
                    var passOK = korisnikBaza.Lozinka == Misc.pwdgen.Hash(model.lozinka);

                    if (passOK)
                    {
                        curUser prijavljeni = new curUser(korisnikBaza);
                        curUserSM serializeModel = new curUserSM();
                        serializeModel.CopyFromUser(prijavljeni);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string korisnickiPodaci = serializer.Serialize(serializeModel);

                        FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(1, prijavljeni.Identity.Name, DateTime.Now, DateTime.Now.AddDays(1), false, korisnickiPodaci);

                        string ticketEncrypted = FormsAuthentication.Encrypt(authenticationTicket);

                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncrypted);
                        Response.Cookies.Add(cookie);

                        if (!String.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(returnURL))
                        {
                            return Redirect(returnURL);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Neispravno korisnicko ime ili lozinka");
            return View(model);
        }
        [OverrideAuthorization]
        [Authorize]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registracija(Korisnik model)
        {
            if (!String.IsNullOrWhiteSpace(model.KorisnikName))
            {
                var korImeZauzeto = bazaDB.KorisnikBaza.Any(x => x.KorisnikName == model.KorisnikName);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("KorisnikName", "Korisničko ime je već zauzeto");
                }
            }
            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var emailZauzet = bazaDB.KorisnikBaza.Any(x => x.Email == model.Email);
                if (emailZauzet)
                {
                    ModelState.AddModelError("Email", "Email je već zauzet");
                }
            }

            if (ModelState.IsValid)
            {
                model.Lozinka = Misc.pwdgen.Hash(model.UnosLozinka);
                model.sifraOvlasti = "RE";

                bazaDB.KorisnikBaza.Add(model);
                bazaDB.SaveChanges();

                return View("RegistracijaOk");
            }

            var ovlasti = bazaDB.OvlastBaza.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }
    }
}