﻿using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    public class RezervatorController : Controller
    {
        rezervatori bazaPodataka = new rezervatori();
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
            rezervatori db = new rezervatori();
            return View(db);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            rezervatori rezervatori = new rezervatori();
            Rezervator rezervator = rezervatori.VratiListu().FirstOrDefault(x => x.Id == id);
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
            rezervatori rezervatori = new rezervatori();
            Rezervator rezervator = rezervatori.VratiListu().FirstOrDefault(x => x.Id == id);
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
                rezervatori rezervatori = new rezervatori();
                return RedirectToAction("Popis");
            }
            return View(r);
        }
    }
}