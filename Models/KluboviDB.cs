using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    public class KluboviDB
    {
        private static List<Klub> lista = new List<Klub>();
        private static bool listaInicijalizirana = false;
        public KluboviDB()
        {
            if (listaInicijalizirana == false)
            {
                listaInicijalizirana = true;

                lista.Add(new Klub()
                {
                    id_kluba = 1,
                    nazivK = "MEV klub",
                    adresaK = "ulica jorgovana 11",
                   brStol=15,
                }
               );
                lista.Add(new Klub()
                {
                    id_kluba = 2,
                    nazivK = "kaotik klub",
                    adresaK = "ulica tratincica 11",
                    brStol = 250,
                }
                   );
            }


        }
        public List<Klub> VratiListu()
        {
            return lista;
        }
        public void AzurirajKlub(Klub klub)
        {
            int klubIndex = lista.FindIndex(x => x.id_kluba == klub.id_kluba);
            lista[klubIndex] = klub;
        }
    }
}