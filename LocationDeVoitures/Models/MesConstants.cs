using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationDeVoitures.Models
{
    public class MesConstants
    {
        public const string RoleAdministrateur = "Administrateur";
        public const string RoleAgence = "Agence";
        public const string RoleLocataire = "Locataire";
    }
    public enum Constants
    {
        Administrateur, Agence, Locataire
    }

    public enum Search
    {
        Couleur, Km, Marque, Module
    }
}