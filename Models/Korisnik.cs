using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    [Table("korisnici")]
    public class Korisnik
    {
        [Key]
        [Column("korisnicko_ime")]
        [Display(Name = "Korisnicko ime")]
        [Required]
        public string KorisnikName { get; set; }


        [Display(Name = "Korisnicko ime")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string Lozinka { get; set; }

        [Display(Name = "Prezime")]
        [Required]
        public string Prezime { get; set; }

        [Display(Name = "Ime")]
        [Required]
        public string Ime { get; set; }

        public string PrIm
        {
            get
            {
                return Prezime + " " + Ime;
            }
        }

        [Column("ovlast")]
        [Display(Name ="Ovlast")]
        [Required]
        [ForeignKey("ovlast")]
        public string sifraOvlasti { get; set; }

        [Display(Name ="Ovlast")]
        public virtual ovlast ovlast { get; set; }

        [Display(Name ="Lozinka")]
        [DataType(DataType.Password)]
        [Required]
        [NotMapped]
        public string UnosLozinka { get; set; }

        [Display(Name = "Ponovljena lozinka")]
        [DataType(DataType.Password)]
        [Required]
        [NotMapped]
        [Compare("UnosLozinka", ErrorMessage = "Lozinke moraju biti jednake")]
        public string PUnosLozinka { get; set; }

    }
}