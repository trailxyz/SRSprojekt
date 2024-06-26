using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRSprojekt.Models
{
    [Table("rezervator")]
    public class Rezervator
    {
        [Column("idRez")]
        [Key]
        [Display(Name = "ID Rezervatora")]
        public int Id { get; set; }

        [Column("imeRez")]
        [Display(Name = "Ime rezervatora")]
        [Required]
        public string Ime { get; set; }

        [Column("prezimeRez")]
        [Display(Name = "Prezime rezervatora")]
        [Required]
        public string Prezime { get; set; }

        [Column("emailRez")]
        [Display(Name = "Email rezervatora")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("mobRez")]
        [Display(Name = "Mobitel rezervatora")]
        [Required(ErrorMessage = "{0} je obavezan")]
        public string Mobitel { get; set; }

        [Column("datumRodRez")]
        [Display(Name = "Datum rođenja rezervatora")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [DataType(DataType.Date)]
        public DateTime datumRod { get; set; }

        public string ImePrezime
        {
            get
            {
                return Ime + " " + Prezime;
            }
        }

        public virtual ICollection<Stolovi> Stolovi { get; set; }
    }
}
