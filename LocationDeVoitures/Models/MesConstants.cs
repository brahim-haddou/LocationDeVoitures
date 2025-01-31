﻿using System;
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
        Agence, Locataire
    }
    public enum ChoixDePaiement
    {
        Carte, Chèque, Espèce, Paypal
    }
    public enum Search
    {
        Couleur, Km, Marque, Module
    }
}