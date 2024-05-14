using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    public class oKlubController : Controller
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
            var oklub = bazaPodataka.ovlastenaosobakluba.ToList();



            return View(oklub);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            BazaDB db = new BazaDB();
            oKlub ovlasteni = db.ovlastenaosobakluba.FirstOrDefault(x => x.Id == id);
            if (ovlasteni == null)
            {
                return RedirectToAction("Popis");
            }
            return View(ovlasteni);

        }

        public ActionResult Azuriraj(int? id)
        {
            oKlub ovlasteni = null;
            if (!id.HasValue)
            {
                ovlasteni = new oKlub();
                ViewBag.Title = "Unos nove ovlastene osobe kluba";
                ViewBag.NovaOsoba = true;
                // return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {

                ovlasteni = bazaPodataka.ovlastenaosobakluba.FirstOrDefault(x => x.Id == id);
                if (ovlasteni == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Title = "Azuriranje podataka o ovlastenoj osobi kluba";
                ViewBag.NovaOsoba = false;

            }


            return View(ovlasteni);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(oKlub o)
        {
            if (!OIB.checkOIB(o.Oib))
            {
                ModelState.AddModelError("Oib", "Neispravan OIB");
            }
            if (ModelState.IsValid)
            {
                if (o.Id != 0)
                {
                    bazaPodataka.Entry(o).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.ovlastenaosobakluba.Add(o);
                }
                bazaPodataka.SaveChanges();


                return RedirectToAction("Popis");
            }
            return View(o);
        }
    }
}