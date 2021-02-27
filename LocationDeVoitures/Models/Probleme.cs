using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class Probleme
    {
        [Key]
        public int ProblemeID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string UserPlaignantID { get; set; }
        public string UserDefendeurID { get; set; }

        [ForeignKey("UserPlaignantID")]
        public virtual ApplicationUser ApplicationUserProblemePlaignant { get; set; }

        [ForeignKey("UserDefendeurID")]
        public virtual ApplicationUser ApplicationUserProblemeDefendeur { get; set; }


    }
}