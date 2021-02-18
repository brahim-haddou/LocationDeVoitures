using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class ListFavoris
    {
        [Key]
        public int? ListFavorisID { get; set; }
        public string UserID { get; set; }
        [Required]
        public string UserFavorisID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser UserListFavoris { get; set; }

        [ForeignKey("UserFavorisID")]
        public virtual ApplicationUser UserFavoris { get; set; }
    }
}