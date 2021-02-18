using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class MasterModelProfile
    {
        public List<Agence> AgenceList { get; set; }

        public List<Locataire> LocataireList { get; set; }
        public List<Voiture> VoitureList { get; set; }
        public List<Location> LocationList { get; set; }
    }
}