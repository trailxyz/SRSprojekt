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
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [StringLength(50, ErrorMessage = "Korisničko ime mora biti između 3 i 50 znakova", MinimumLength = 3)]
        public string KorisnikName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Neispravna email adresa")]
        [Required(ErrorMessage = "Email je obavezan")]
        public string Email { get; set; }

        public string Lozinka { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(50, ErrorMessage = "Prezime ne smije biti duže od 50 znakova")]
        public string Prezime { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(50, ErrorMessage = "Ime ne smije biti duže od 50 znakova")]
        public string Ime { get; set; }

        public string PrIm
        {
            get
            {
                return Prezime + " " + Ime;
            }
        }

        [Column("ovlast")]
        [Display(Name = "Ovlast")]
        [Required(ErrorMessage = "Ovlast je obavezna")]
        [ForeignKey("ovlast")]
        public string sifraOvlasti { get; set; }

        [Display(Name = "Ovlast")]
        public virtual ovlast ovlast { get; set; }

        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lozinka je obavezna")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora biti barem 6 znakova")]
        [NotMapped]
        public string UnosLozinka { get; set; }

        [Display(Name = "Ponovljena lozinka")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ponovljena lozinka je obavezna")]
        [Compare("UnosLozinka", ErrorMessage = "Lozinke moraju biti jednake")]
        [NotMapped]
        public string PUnosLozinka { get; set; }

    }
}