using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SRSprojekt.Misc
{
    public class curUser : IPrincipal
    {
        public string KorisnickoIme { get; set; }
        public string PrIm { get; set; }
        public string Ovlast { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string ovlast)
        {
            if (Ovlast == ovlast) return true;
            return false;
        }

        public curUser(Korisnik kor)
        {
            this.Identity = new GenericIdentity(kor.KorisnikName);
            this.KorisnickoIme = kor.KorisnikName;
            this.PrIm = kor.PrIm;
            this.Ovlast = kor.sifraOvlasti;
        }

        public curUser(string korisnickoIme)
        {
            this.Identity=new GenericIdentity(korisnickoIme);
            this.KorisnickoIme= korisnickoIme;
        }

    }
}