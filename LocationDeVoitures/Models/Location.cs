using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        public int VoitureID { get; set; }

        public int LocataireID { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Duree { get; set; }
        [Required]
        public string ChoiseDePayment { get; set; }
        public bool Status { get; set; }

        [ForeignKey("VoitureID")]
        public virtual Voiture VoitureLocation { get; set; }

        [ForeignKey("LocataireID")]
        public virtual Locataire LocataireLocation { get; set; }
    }
}