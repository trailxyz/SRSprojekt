using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SRSprojekt.Models
{
    public class KPrijava
    {
        [Required]
        [Display(Name ="Korisnicko ime")]
        public string KorisnickoIme { get; set; }

        [Required]
        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        public string lozinka { get; set; }
    }
}