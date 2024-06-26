using SRSprojekt.Misc;
using SRSprojekt.Models;
using SRSprojekt.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.OvlastenaOsobaKluba + "," + OvlastiKorisnik.Administrator)]

    public class StoloviController : Controller
    {
        public BazaDB db = new BazaDB()
            ;

        public ActionResult Index()
        {
            List<Stolovi> stolovi;

            using (var dbContext = new BazaDB())
            {

                stolovi = dbContext.StoloviBaza.Include(s => s.aktivniR).ToList();
            }


            return View(stolovi);

        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stolovi stolovi = db.StoloviBaza.Find(id);
            if (stolovi == null)
            {
                return HttpNotFound();
            }
            return View(stolovi);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sifra,broj_stola,zauzetost")] Stolovi stolovi)
        {
            if (ModelState.IsValid)
            {
                db.StoloviBaza.Add(stolovi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stolovi);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stolovi stolovi = db.StoloviBaza.Find(id);
            if (stolovi == null)
            {
                return HttpNotFound();
            }

            ViewBag.RezervatorList = new SelectList(db.RezervatorBaza, "Id", "ImePrezime", stolovi.sifraR);
            return View(stolovi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sifra,broj_stola,zauzetost,sifraR")] Stolovi stolovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stolovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RezervatorList = new SelectList(db.RezervatorBaza, "Id", "ImePrezime", stolovi.sifraR);
            return View(stolovi);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stolovi stolovi = db.StoloviBaza.Find(id);
            if (stolovi == null)
            {
                return HttpNotFound();
            }
            return View(stolovi);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Stolovi stolovi = db.StoloviBaza.Find(id);
            db.StoloviBaza.Remove(stolovi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult reset()
        {
            try
            {
                var items = db.StoloviBaza.Where(e => e.zauzetost == true).ToList();
                foreach (var item in items)
                {
                    item.zauzetost = false;

                    if (string.IsNullOrEmpty(item.Sifra))
                    {
                        item.Sifra = "0"; 
                    }
                    if (string.IsNullOrEmpty(item.broj_stola))
                    {
                        item.broj_stola = "Stol 1"; 
                    }
                }
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Entity: {0} Property: {1} Error: {2}", validationErrors.Entry.Entity.GetType().Name, validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw; 
            }

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult IspisStolova(string broj_stola, string zauzetost, string rez, string sort, int? page)
        {
          

            ViewBag.Sortiranje = sort;
            ViewBag.NazivSort = String.IsNullOrEmpty(sort) ? "naziv_desc" : "";
            ViewBag.SmjerSort = sort == "rez" ? "brS" : "rez";
            ViewBag.rez = rez;
            ViewBag.brst = broj_stola;
            ViewBag.zau = zauzetost;

            var stolovi = db.StoloviBaza.ToList();

         
            if (!String.IsNullOrWhiteSpace(broj_stola))
            {
                stolovi = stolovi.Where(x => x.broj_stola == broj_stola).ToList();
            }


            if (!String.IsNullOrWhiteSpace(rez))
            {
                stolovi = stolovi.Where(x => x.aktivniR.ImePrezime == rez).ToList();
            }

            switch (sort)
            {
                case "naziv_desc":
                    stolovi = stolovi.OrderByDescending(s => s.aktivniR.ImePrezime).ToList();
                    break;
                case "rez":
                    stolovi = stolovi.OrderBy(s => s.zauzetost).ToList();
                    break;
                case "brS":
                    stolovi = stolovi.OrderByDescending(s => s.broj_stola).ToList();
                    break;
                default:
                    stolovi = stolovi.OrderBy(s => s.aktivniR?.ImePrezime).ToList();
                    break;
            }

            RezervacijeReport rezervacijeReport = new RezervacijeReport();
            rezervacijeReport.ListaStolova(stolovi);

            return File(rezervacijeReport.Podaci, System.Net.Mime.MediaTypeNames.Application.Pdf,
                "PopisStolova.pdf");
        }



    }
}
