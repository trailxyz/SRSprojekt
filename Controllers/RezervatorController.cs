using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    public class RezervatorController : Controller
    {
        BazaDB bazaPodataka = new BazaDB();
        // GET: Korisnici
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Sustav rezervacija";
            ViewBag.Fakultet = "Međimursko veleučilište u Čakovcu";
            return View();
        }

        public ActionResult Popis()
        {
            var njanjanja = bazaPodataka.RezervatorBaza.ToList();
            return View(njanjanja);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            BazaDB rezervatori = new BazaDB();
          
            Rezervator rezervator = rezervatori.RezervatorBaza.FirstOrDefault(x => x.Id == id);
            if (rezervator == null)
            {
                return RedirectToAction("Popis");
            }
            return View(rezervator);

        }

        public ActionResult Azuriraj(int? id)
        {
            Rezervator rezervator = null;
            if (!id.HasValue)
            {
                rezervator = new Rezervator();
                ViewBag.Title = "Unos novog rezervatora";
                ViewBag.NoviRezervator = true;
                // return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {

                rezervator = bazaPodataka.RezervatorBaza.FirstOrDefault(x => x.Id == id);
                if (rezervator == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Title = "Azuriranje podataka o rezervatoru";
                ViewBag.NoviRezervator = false;

            }
            return View(rezervator);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Rezervator r)
            
        {
            DateTime punoljetnost = DateTime.Now.AddYears(-18);
            if(r.datumRod > punoljetnost)
            {
                ModelState.AddModelError("datumRod", "Rezervator mora biti osoba starija od 18");
            }
            if (ModelState.IsValid)
            {
                if (r.Id != 0)
                {
                    bazaPodataka.Entry(r).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.RezervatorBaza.Add(r);
                }
                bazaPodataka.SaveChanges();


                return RedirectToAction("Popis");
            }
            if (r.Id == 0)
            {
                ViewBag.Title = "Kreiranje rezervatora";
                ViewBag.NoviRezervator = true;
            }
            else
            {
                ViewBag.Title = "Azuriranje podataka o rezervatoru";
                ViewBag.NoviRezervator = false;
            }

            return View(r);
        }
        public ActionResult Brisi(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Popis");
            }

            Rezervator re = bazaPodataka.RezervatorBaza.FirstOrDefault(x => x.Id == id);

            if (re == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja kluba";
            return View(re);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Rezervator re = bazaPodataka.RezervatorBaza.FirstOrDefault(x => x.Id == id);
            if (re == null)
            {
                return HttpNotFound();
            }
            bazaPodataka.RezervatorBaza.Remove(re);
            bazaPodataka.SaveChanges();
            return View("BrisiStatus");
        }
    }
}
