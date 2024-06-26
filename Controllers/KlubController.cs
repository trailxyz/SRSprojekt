using SRSprojekt.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using SRSprojekt.Misc;

namespace SRSprojekt.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
    public class KlubController : Controller
    {
        BazaDB bazaPodataka = new BazaDB();
        // GET: Korisnici
       
        public ActionResult Index()
        {
            ViewBag.Title = "Sustav rezervacija";
            ViewBag.Fakultet = "Međimursko veleučilište u Čakovcu";
            return View();
        }

        public ActionResult Popis()
        {
            var popis = bazaPodataka.KlubBaza.ToList();



            return View(popis);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            BazaDB db = new BazaDB();
            Klub klub = db.KlubBaza.FirstOrDefault(x => x.id_kluba == id);
            if (klub == null)
            {
                return RedirectToAction("Popis");
            }
            return View(klub);

        }

        public ActionResult Azuriraj(int? id)
        {
            Klub klub = null;
            if (!id.HasValue)
            {
                klub = new Klub();
                ViewBag.Title = "Unos novog kluba";
                ViewBag.NoviKlub = true;
                // return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {

                klub = bazaPodataka.KlubBaza.FirstOrDefault(x => x.id_kluba == id);
                if (klub == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Title = "Azuriranje podataka o klubu";
                ViewBag.NoviKlub = false;

            }


            return View(klub);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Klub k)
        {
            if (ModelState.IsValid)
            {
                if (k.id_kluba != 0)
                {
                    bazaPodataka.Entry(k).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.KlubBaza.Add(k);
                }
                bazaPodataka.SaveChanges();


                return RedirectToAction("Popis");
            }
            if (k.id_kluba == 0)
            {
                ViewBag.Title = "Kreiranje kluba";
                ViewBag.NoviKlub = true;
            }
            else
            {
                ViewBag.Title = "Azuriranje podataka o klubu";
                ViewBag.NoviKlub = false;
            }
            return View(k);

        }

        public ActionResult Brisi(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Popis");
            }

            Klub k = bazaPodataka.KlubBaza.FirstOrDefault(x => x.id_kluba == id);

            if (k == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja kluba";
            return View(k);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Klub k = bazaPodataka.KlubBaza.FirstOrDefault(x => x.id_kluba == id);
            if (k == null)
            {
                return HttpNotFound();
            }
            bazaPodataka.KlubBaza.Remove(k);
            bazaPodataka.SaveChanges();
            return View("BrisiStatus");
        }

        
    }
}


