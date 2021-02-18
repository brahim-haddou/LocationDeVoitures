using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class Offre
    {

        [Key]
        public int OffreID { get; set; }
        [Required]
        [RegularExpression(@"^(100\.00|100\.0|100)|([0-9]{1,2}){0,1}(\.[0-9]{1,2}){0,1}$", ErrorMessage = "Invalid Pourcentage. Pourcentage must be between 0.00 and 100.00")]
        public float Pourcentage { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Duree { get; set; }
        [Required]
        public int VoitureID { get; set; }

        [ForeignKey("VoitureID")]
        public virtual Voiture VoitureOffre { get; set; }
    }
}