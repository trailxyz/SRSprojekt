using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    [Table("ovlastenaosobakluba")]
    public class oKlub
    {
        [Column("IdKorisnik")]
        [Key]
        [Display(Name = "ID Korisnika")]
        public int Id { get; set; }

        [Column("imeKorisnik")]
        [Display(Name = "Ime odgovorne osobe")]
        [Required]
        public string Ime { get; set; }

        [Column("prezimeKorisnik")]
        [Display(Name = "Prezime odgovorne osobe")]
        [Required]
        public string Prezime { get; set; }

        [Column("oibKorisnik")]
        [Display(Name = "OIB odgovorne osobe")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} mora biti duljine {1} znakova")]
        public string Oib { get; set; }

        [Column("emailKorisnik")]
        [Display(Name = "Email odgovorne osobe")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("mobKorisnik")]
        [Display(Name = "Mobitel odgovorne osobe")]
        [Required(ErrorMessage = "{0} je obavezan")]
        public string Mobitel { get; set; }
    }
}