using System.Collections.Generic;

namespace Galatee.Silverlight
{
    public static class UserConnecte
    {
        public static int PK_ID { get; set; }
        public static int FK_IDCENTRE { get; set; }
        public static int FK_IDFONCTION { get; set; }
        public static string Centre { get; set; }
        public static string LibelleCentre { get; set; }
        public static string matricule { get; set; }
        public static string CaisseSelect { get; set; }
        public static string MatriculeSelect { get; set; }
        public static string numcaisse { get; set; }
        public static string nomUtilisateur { get; set; }
        public static string codefontion { get; set; }
        public static string LibelleFonction { get; set; }
        public static string CentreSelected { get; set; }
        public static decimal DernierNumeroRecu { get; set; }
        public static bool   IsAppartienGroupeValidation { get; set; }
        public static int PerimetreAction { get; set; }
        public static List<string> ListeDesCentreProfil { get; set; }
        public static List<Galatee.Silverlight.ServiceAuthenInitialize.CsProfil> listeProfilUser { get; set; }

    }
}
