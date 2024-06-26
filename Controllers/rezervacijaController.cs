using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SRSprojekt.Controllers
{
    public class rezervacijaController : Controller
    {
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string message, string BrS, string Ime, string Prezime, string BrT, string Email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mail1 = new MailAddress("paup@mislovic.com", "SRS sustav");
                    var mail2 = new MailAddress(Email, "Pošiljatelj");
                    var password = "Paup747";
                    var brS = BrS;
                    var ime = Ime;
                    var prezime = Prezime;
                    var brT = BrT;
                    var email = Email;
                    var poruka = message;
                    var smtp = new SmtpClient
                    {
                        Host = "mail.mislovic.com",
                        Port = 587,
                        EnableSsl = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(mail1.Address, password)
                    };
                    using (var sustav = new MailMessage(mail1, mail1)
                    {
                        Subject = "Rezervacija za stol " + brS,
                        Body = "/OVO JE AUTOMATIZIRANA PORUKA/ \n" + "Ime: " + ime + "\nPrezime: " + prezime + "\nBroj telefona: " + brT + "\nKontakt email: " + email + "\nželi rezervirati stol: " + brS + " sa posebnim zahtjevom: " + poruka,
                    })
                    using (var rezervator = new MailMessage(mail1, mail2)
                    {
                        Subject = "Rezervacija za stol " + brS,
                        Body = "/OVO JE AUTOMATIZIRANA PORUKA/ \n Želite rezervirati stol sa sljedećim podacima\n" + "Ime: " + ime + "\nPrezime: " + prezime + "\nBroj telefona: " + brT + "\nKontakt email: " + email + "\nstol: " + brS + "\nposebni zahtjev:" + poruka,
                    })
                    {
                        smtp.Send(sustav);
                        smtp.Send(rezervator);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
        public ActionResult SendEmailK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmailK(string ipo, string BrS, string Ime, string Adresa, string CR, string Email, string ko)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mail1 = new MailAddress("paup@mislovic.com", "SRS sustav");
                    var mail2 = new MailAddress(Email, "Pošiljatelj");
                    var password = "Paup747";
                    var brS = BrS;
                    var ime = Ime;
                    var adresa = Adresa;
                    var cijenovni_rang = CR;
                    var email = Email;
                    var IPO = ipo;
                    var KO = ko;
                    var smtp = new SmtpClient
                    {
                        Host = "mail.mislovic.com",
                        Port = 587,
                        EnableSsl = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(mail1.Address, password)
                    };
                    using (var sustav = new MailMessage(mail1, mail1)
                    {
                        Subject = "Registracija kluba " + ime,
                        Body = "/OVO JE AUTOMATIZIRANA PORUKA/ \n" + "Klub ime: " + ime + "\nadresa: " + adresa + "\ncijenovni rang: " + cijenovni_rang + "\nbroj stolova: " + brS + 
                        "\nželi se registrirati u sustav. \n KONTAKT OSOBA\n Ime i prezime: " + IPO + "\nkontakt: " + KO,
                    })
                    using (var registrator = new MailMessage(mail1, mail2)
                    {
                        Subject = "Rezervacija za stol " + brS,
                        Body = "/OVO JE AUTOMATIZIRANA PORUKA/ \n Želite registrirati stol sa sljedećim podacima\n" + "Klub ime: " + ime + "\nadresa: " + adresa + "\ncijenovni rang: " + cijenovni_rang + "\nbroj stolova: " + brS + "\n KONTAKT OSOBA\\n Ime i prezime: \" + IPO + \"\\nkontakt: \" + KO"
                    })
                    {
                        smtp.Send(sustav);
                        smtp.Send(registrator);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}