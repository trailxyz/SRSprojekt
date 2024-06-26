using SRSprojekt.Misc;
using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.OvlastenaOsobaKluba + "," + OvlastiKorisnik.Administrator)]

    public class StatistikaController : Controller
    {

        private BazaDB _context = new BazaDB();

        public ActionResult Index(bool? zauzetost = null) 
        {
            var itemsData = _context.StoloviBaza.ToList(); 

            ViewBag.ItemsData = itemsData;

            return View();
        }
    }
}