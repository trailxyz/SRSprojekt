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
            BazaDB db = new BazaDB();
            return View(db);
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
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            BazaDB rezervatori = new BazaDB();
            Rezervator rezervator = rezervatori.RezervatorBaza.FirstOrDefault(x => x.Id == id);
            if (rezervator == null)
            {
                return HttpNotFound();
            }
            return View(rezervator);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Rezervator r)
        {
            if (ModelState.IsValid)
            {
               bazaPodataka.Entry(r).State=System.Data.Entity.EntityState.Modified;
                bazaPodataka.SaveChanges();
                return RedirectToAction("Popis");
            }
           
            return View(r);
        }
    }
}
