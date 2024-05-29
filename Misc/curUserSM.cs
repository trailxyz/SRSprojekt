using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRSprojekt.Misc
{
    public class curUserSM
    {
        public string KorisnickoIme {  get; set; }
        public string PrIm { get; set; }
        public string Ovlast { get; set; }

        internal void CopyFromUser(curUser user)
        {
            this.KorisnickoIme= user.KorisnickoIme;
            this.PrIm= user.PrIm;
            this.Ovlast= user.Ovlast;
        }
    }
}