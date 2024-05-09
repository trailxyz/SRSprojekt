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

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Naziv kluba")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string naziv { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Adresa kluba")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string adresa { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} je obavezan podatak.")]
        [Display(Name = "Broj stolova")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int broj_stolova { get; set; } = 0;

        public cijenovni_rang Cijenovni_Rang { get; set; }


    }

    
}
