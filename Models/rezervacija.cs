using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    [Table("rezervacija")]
    public class rezervacija
    {
        [Key]
        [Column("id_rezervacije")]
        [Display(Name = "ID rezervacije")]
        [StringLength(255, ErrorMessage = "{0} mora biti duljine maksimalno {1} znakova")]
        public string ID_rezervacije { get; set; }

        [Column("idStola")]
        [Display(Name = "Broj stola")]
        [Required(ErrorMessage = "{0} je obavezna")]
        public string ID_stola { get; set; }

        [Display(Name = "Broj stola")]
        public Stolovi brStola { get; set; }

        [Column("zauz")]
        [Display(Name = "Zauzetost")]
        [Required(ErrorMessage = "{0} je obavezna")]
        public bool Zauzetost { get; set; }

        [Column("datVri")]
        [Display(Name = "Datum i vrijeme")]
        [Required(ErrorMessage = "{0} je obavezna")]
        public DateTime DatVri { get; set; }


    }
}