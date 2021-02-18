using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class MasterModelVoiture
    {
        public List<Voiture> VoitureList { get; set; }
        public List<Offre> OffreeList { get; set; }
        public List<ImagesVoiture> VoitureListImages { get; set; }
    }
}