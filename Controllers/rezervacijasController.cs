using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SRSprojekt.Models;

namespace SRSprojekt.Controllers
{
    public class rezervacijasController : Controller
    {
        private BazaDB db = new BazaDB();

        public ActionResult ReservationFailed()
        {
            return View();
        }

        public ActionResult ReservationSuccess()
        {
            return View();
        }

        // GET: rezervacijas
        public ActionResult Index()
        {
            // Load all reservations and include related Stolovi tables
            var allReservations = db.rezervacijaBaza
                                    .Include(r => r.brStola)  // Include related Stolovi tables
                                    .ToList();

            return View(allReservations);
        }

        // GET: rezervacijas/Details/5
        public ActionResult Details(string id)
        {
            // Load the reservation and include related table (Stolovi) using Include()
            var rezervacija = db.rezervacijaBaza
                                .Include(r => r.brStola)  // Include related Stolovi table
                                .FirstOrDefault(r => r.ID_rezervacije == id);

            if (rezervacija != null)
            {
                // Access the related table (Stolovi) through the navigation property
                var stolDetails = rezervacija.brStola;

                // Example: Display the table's number and whether it’s occupied
                ViewBag.TableNumber = stolDetails.broj_stola;
                ViewBag.IsOccupied = stolDetails.zauzetost;

                return View(rezervacija);
            }

            return HttpNotFound();
        }

        // GET: rezervacijas/Create
        public ActionResult Create()
        {
            // Populate the list of tables for the dropdown
            ViewBag.TableList = new SelectList(db.StoloviBaza, "Sifra", "broj_stola");
            return View();
        }




        // POST: rezervacijas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(rezervacija rezervacija)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                // Check table availability
                bool isAvailable = CheckTableAvailability(rezervacija.ID_stola, rezervacija.DatVri);

                if (isAvailable)
                {
                    // Proceed with reservation creation
                    rezervacija.Zauzetost = 1;
                    db.rezervacijaBaza.Add(rezervacija);
                    db.SaveChanges();

                    // Optionally mark the table as occupied
                    var table = db.StoloviBaza.FirstOrDefault(t => t.Sifra == rezervacija.ID_stola);
                    if (table != null)
                    {
                        table.zauzetost = true;
                        db.Entry(table).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("ReservationSuccess");
                }
                else
                {
                    // Return an error message if the table is not available
                    TempData["ErrorMessage"] = "The table is not available at the selected time.";
                    return RedirectToAction("ReservationFailed");
                }
            }

            // If validation failed, repopulate dropdown and return view
            ViewBag.TableList = new SelectList(db.StoloviBaza, "Sifra", "broj_stola", rezervacija.ID_stola);
            return View(rezervacija);
        }








        // GET: rezervacijas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            if (rezervacija == null)
            {
                return HttpNotFound();
            }
            return View(rezervacija);
        }

        // POST: rezervacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_stola,Zauzetost,DatVri")] rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rezervacija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rezervacija);
        }

        // GET: rezervacijas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            if (rezervacija == null)
            {
                return HttpNotFound();
            }
            return View(rezervacija);
        }

        // POST: rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            rezervacija rezervacija = db.rezervacijaBaza.Find(id);
            db.rezervacijaBaza.Remove(rezervacija);
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

        public bool CheckTableAvailability(string ID_stola, DateTime requestedDateTime)
        {
            // Convert the requestedDateTime to just the date part for comparison
            DateTime requestedDateOnly = requestedDateTime.Date;

            // Fetch all existing reservations for the table
            var existingReservations = db.rezervacijaBaza
                .Where(r => r.ID_stola == ID_stola)
                .ToList();  // Load all reservations into memory to perform in-memory filtering

            // Check if there is any reservation that conflicts with the requested date and time
            bool isAvailable = !existingReservations.Any(r =>
                r.DatVri.Date == requestedDateOnly && r.DatVri.TimeOfDay == requestedDateTime.TimeOfDay);

            return isAvailable;
        }




    }
}
