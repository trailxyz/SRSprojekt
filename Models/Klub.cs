using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    [Table("klub")]
    public class Klub
    {
        [Required]
        [Key]
        public long id_kluba { get; set; }

        [Column("nazivK")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Naziv kluba")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string nazivK { get; set; } = "";

        [Column("adresaK")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Adresa kluba")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string adresaK { get; set; } = "";

        [Column("brStol")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Broj stolova")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int brStol { get; set; } = 0;

        [Column("cijenovni_rang")]
        [Required(ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Cijenovni rang kluba")]
        public cijenovni_rang Cijenovni_Rang { get; set; } 

        public string PuniNaziv
        {
            get
            {
                return  "(" + id_kluba + ")" + " " + nazivK;
            }
        }
        

    }

    
}
