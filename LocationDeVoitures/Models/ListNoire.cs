using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class ListNoire
    {
        [Key]
        public int? ListNoireID { get; set; }
        public string UserID { get; set; }
        [Required]
        public string UserNoireID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser UserListNoire { get; set; }

        [ForeignKey("UserNoireID")]
        public virtual ApplicationUser UserNoire { get; set; }
    }
}