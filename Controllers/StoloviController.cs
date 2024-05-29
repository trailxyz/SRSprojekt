using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using SRSprojekt.Models;

namespace SRSprojekt.Controllers
{
    public class StoloviController : Controller
    {
        private BazaDB db = new BazaDB()
            ;
        
        // GET: Stolovis
        public ActionResult Index()
        {
            return View(db.StoloviBaza.ToList());
        }

        // GET: Stolovis/Details/5
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

        // GET: Stolovis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stolovis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Stolovis/Edit/5
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
            return View(stolovi);
        }

        // POST: Stolovis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sifra,broj_stola,zauzetost")] Stolovi stolovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stolovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stolovi);
        }

        // GET: Stolovis/Delete/5
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

        // POST: Stolovis/Delete/5
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
        public ActionResult Statistika() {
            var dataPoints = db.StoloviBaza
          .Select(item => new { item.zauzetost })
          .GroupBy(item => item.zauzetost)
          .Select(group => new
          {
              Label = group.Key ? "Active" : "Inactive",
              Y = group.Count()
          })
          .ToList();

            if (!dataPoints.Any())
            {
                dataPoints.Add(new { Label = "No Data", Y = 0 });
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

           

            

            return View();
        }
    }
}
