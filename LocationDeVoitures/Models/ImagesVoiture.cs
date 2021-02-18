using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class ImagesVoiture
    {
        [Key]
        public int ImagesVoitureID { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public int VoitureID { get; set; }

        [ForeignKey("VoitureID")]
        public virtual Voiture Voiture { get; set; }
    }
}