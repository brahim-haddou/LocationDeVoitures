using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class Administrateur
    {
        [Key]
        public int AdministrateurID { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{2}[0-9]{5}", ErrorMessage = "Please enter a valide CIN")]
        public string CIN { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Ville { get; set; }
        [Required]
        [Phone]
        [RegularExpression(@"^(?:(?:(?:\+|00)212[\s]?(?:[\s]?\(0\)[\s]?)?)|0){1}(?:5[\s.-]?[2-3]|6[\s.-]?[13-9]){1}[0-9]{1}(?:[\s.-]?\d{2}){3}$", ErrorMessage = "Please enter a valide phone number")]
        public string Tel { get; set; }
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser UserAdministrateur { get; set; }
    }
}