using SRSprojekt.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    public class rezervacijasController : Controller
    {
        private readonly BazaDB db = new BazaDB();

        public ActionResult ReservationFailed()
        {
            return View();
        }

        public ActionResult ReservationSuccess()
        {
            return View();
        }

        public ActionResult Index()
        {
            System.Collections.Generic.List<rezervacija> allReservations = db.rezervacijaBaza
                                    .Include(r => r.brStola)
                                    .ToList();

            return View(allReservations);
        }

        public ActionResult Details(string id)
        {
            rezervacija rezervacija = db.rezervacijaBaza
                                .Include(r => r.brStola)
                                .FirstOrDefault(r => r.ID_rezervacije == id);

            if (rezervacija != null)
            {
                ViewBag.TableNumber = rezervacija.brStola?.broj_stola;
                ViewBag.IsOccupied = rezervacija.brStola?.zauzetost;

                return View(rezervacija);
            }

            return HttpNotFound();
        }

        public ActionResult Create()
        {
            System.Collections.Generic.List<SelectListItem> tables = db.StoloviBaza
                .Select(t => new SelectListItem
                {
                    Value = t.Sifra,
                    Text = t.broj_stola
                })
                .ToList();

            ViewBag.ID_stola = new SelectList(tables, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(rezervacija rezervacija)
        {
            System.Collections.Generic.List<SelectListItem> tables = db.StoloviBaza
                .Select(t => new SelectListItem
                {
                    Value = t.Sifra,
                    Text = t.broj_stola
                })
                .ToList();
            ViewBag.ID_stola = new SelectList(tables, "Value", "Text");

            if (ModelState.IsValid)
            {
                try
                {
                    rezervacija.ID_rezervacije = Guid.NewGuid().ToString();

                    bool isAvailable = CheckTableAvailability(rezervacija.ID_stola, rezervacija.DatVri);

                    if (isAvailable)
                    {
                        rezervacija.Zauzetost = true;

                        _ = db.rezervacijaBaza.Add(rezervacija);
                        _ = db.SaveChanges();

                        Stolovi table = db.StoloviBaza.FirstOrDefault(t => t.Sifra == rezervacija.ID_stola);
                        if (table != null)
                        {
                            table.zauzetost = true;
                            db.Entry(table).State = EntityState.Modified;
                            _ = db.SaveChanges();
                        }

                        return RedirectToAction("ReservationSuccess");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "The table is not available at the selected time.";
                        return RedirectToAction("ReservationFailed");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while saving the reservation.");
                }
            }

            return View(rezervacija);
        }



        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            return rezervacija == null ? HttpNotFound() : (ActionResult)View(rezervacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_rezervacije,ID_stola,Zauzetost,DatVri")] rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rezervacija).State = EntityState.Modified;
                _ = db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rezervacija);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            return rezervacija == null ? HttpNotFound() : (ActionResult)View(rezervacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            _ = db.rezervacijaBaza.Remove(rezervacija);
            _ = db.SaveChanges();
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

        public bool CheckTableAvailability(string ID_stola, DateTime requestedDateTime)
        {
            DateTime requestedDateOnly = requestedDateTime.Date;
            TimeSpan requestedTimeOnly = new TimeSpan(requestedDateTime.Hour, requestedDateTime.Minute, 0);

            System.Collections.Generic.List<rezervacija> existingReservations = db.rezervacijaBaza
                .Where(r => r.ID_stola == ID_stola)
                .ToList();

            bool isAvailable = !existingReservations.Any(r =>
                r.DatVri.Date == requestedDateOnly && r.DatVri.TimeOfDay.Hours == requestedTimeOnly.Hours && r.DatVri.TimeOfDay.Minutes == requestedTimeOnly.Minutes);

            return isAvailable;
        }
    }
}
