using SRSprojekt.Misc;
using SRSprojekt.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
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

        public ActionResult Popis(string ime)
        {
            var oklub = bazaPodataka.ovlastenaosobakluba.ToList();

           // string kombajn = ime + prezime;
           
            if (!String.IsNullOrEmpty(ime))
            {
                oklub = oklub.Where(x => x.ImePrezime.ToUpper().Contains(ime.ToUpper())).ToList();
            }

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
                ViewBag.NoviOvlasteni = true;
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
                ViewBag.NoviOvlasteni = false;

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
            if (o.Id == 0)
            {
                ViewBag.Title = "Dodavanje ovlastene osobe";
                ViewBag.NoviOvlasteni = true;
            }
            else
            {
                ViewBag.Title = "Azuriranje podataka o ovlastenoj osobi";
                ViewBag.NoviOvlasteni = false;
            }
            return View(o);

        }

        public ActionResult Brisi(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Popis");
            }

            oKlub o = bazaPodataka.ovlastenaosobakluba.FirstOrDefault(x => x.Id == id);

            if (o == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja ovlastene osobe";
            return View(o);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            oKlub o = bazaPodataka.ovlastenaosobakluba.FirstOrDefault(x => x.Id == id);
            if (o == null)
            {
                return HttpNotFound();
            }
            bazaPodataka.ovlastenaosobakluba.Remove(o);
            bazaPodataka.SaveChanges();
            return View("BrisiStatus");
        }
        
    }
}
