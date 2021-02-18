using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class Voiture
    {
        [Key]
        public int VoitureID { get; set; }
        [Required]
        public string Matricule { get; set; }
        [Required]
        [RegularExpression(@"\d{1,20}.\d{0,2}", ErrorMessage = "Invalid Price. Please use the format of XXXX.XX .")]
        public decimal Prix { get; set; }
        [Required]
        [RegularExpression(@".*\d{4}.*", ErrorMessage = "Please enter a 4 digit year")]
        public int Module { get; set; }
        [Required]
        public string Marque { get; set; }
        [Required]
        [RegularExpression(@"\d{1,20}.\d{0,2}", ErrorMessage = "Invalid Price. Please use the format of XXXX.XX .")]
        public float Km { get; set; }
        [Required]
        public string Couleur { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int AgenceID { get; set; }

        [ForeignKey("AgenceID")]
        public virtual Agence Agence { get; set; }
    }
}