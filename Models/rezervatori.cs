using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    public class rezervatori
    {
         static List<Rezervator> lista = new List<Rezervator>();
         static bool listaInicijalizirana = false;
        public rezervatori()
        {
            if (listaInicijalizirana == false)
            {
                listaInicijalizirana = true;

                lista.Add(new Rezervator()
                {
                    Id = 1,
                    Ime = "Tvrtko",
                    Prezime = "Tvrdi",
                    Email = "daovojemojmail@gmail.com",
                    Mobitel = "+385995908762",
                    datumRod = new DateTime(2002,01,03)
                }
               );
                lista.Add(new Rezervator()
                {
                    Id = 2,
                    Ime = "Softko",
                    Prezime = "Softi'",
                    Email = "mikserzajabuke@gmail.com",
                    Mobitel = "+38598307744",
                    datumRod = new DateTime(2000, 01, 04)
                }
                    );
            }


        }
        public List<Rezervator> VratiListu()
        {
            return lista;
        }
        public void AzurirajRezervatora(Rezervator rezervator)
        {
            int rezervatorIndex = lista.FindIndex(x => x.Id == rezervator.Id);
            lista[rezervatorIndex] = rezervator;
        }
    }
}