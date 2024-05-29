using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRSprojekt.Models
{
    [Table("stolovis")]
    public class Stolovi
    {
        [Key]
        [Column("sifra")]
        [Display(Name = "Sifra")]
        [Required(ErrorMessage = "{0} je obavezna")]
        [StringLength(5, ErrorMessage = "{0} mora biti duljine maksimalno {1} znakova")]
        public string Sifra { get; set; }
        
        [Column("brojstola")]
        [Display(Name = "Broj stola")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [StringLength(5, ErrorMessage = "{0} mora biti duljine maksimalno {1} znakova")]
        public string broj_stola { get; set; }
        [Column("zauzetost")]
        [Display(Name = "Slobodan")]
        [Required(ErrorMessage = "{0} je obavezna")]
        public bool zauzetost { get; set; }
    }
}