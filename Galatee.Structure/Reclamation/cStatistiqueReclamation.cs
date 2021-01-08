using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Galatee.Structure
{
    public class cStatistiqueReclamation:CsPrint 
    {
       [DataMember] public string IdCentre { get; set; }
       [DataMember] public string IdSite { get; set; }
       [DataMember] public string LibelleCentre { get; set; }
       [DataMember] public int NombreEnDebut { get; set; }
       [DataMember] public int NombreOuverte { get; set; }
       [DataMember] public int NombreFerme { get; set; }
       [DataMember] public int NombreEnFin { get; set; }
       [DataMember] public DateTime DateDebut { get; set; }
       [DataMember] public DateTime DateFin { get; set; }
       [DataMember] public decimal TauxTraitement { get; set; }
       [DataMember] public DateTime? DateOuverture { get; set; }
       [DataMember] public DateTime? DateValidation { get; set; }
       [DataMember] public int Duree { get; set; }
       [DataMember] public int TotalDuree { get; set; }
       [DataMember] public decimal MoyenneTraitement { get; set; }
       [DataMember] public byte IdTypeReclamation { get; set; }
       [DataMember] public byte IdGroupeTypeReclamation { get; set; }
       [DataMember] public string LibelleGroupeTypeReclamation { get; set; }
       [DataMember] public string LibelleTypeReclamation { get; set; }
       [DataMember] public decimal TauxFrTraitees { get; set; }
       [DataMember] public int NombreFrTraitees { get; set; }
       [DataMember] public decimal DureeMoyenneTraitement { get; set; }
       [DataMember] public decimal DureeMoyenneFrEnFinPeriode { get; set; }
       [DataMember] public int TotalNombreOuverte { get; set; }
       [DataMember] public int TotalNombreEnFin { get; set; }
       [DataMember] public int TotalNombreEnDebut { get; set; }
       [DataMember] public int TotalNombreFerme { get; set; }
       [DataMember] public decimal TotalMoyenneTraitement { get; set; }
       [DataMember] public decimal TotalTauxTraitement { get; set; }
       [DataMember]
       public int DureeDeCloture { get; set; }
       [DataMember]
       public int DureeMoyEnDebut { get; set; }
       [DataMember]
       public int DureeMoyOuvert { get; set; }
       [DataMember]
       public int DureeMoyEnFin { get; set; }

       [DataMember]
       public int NombreTraiteAvant15Jr { get; set; }
       [DataMember]
       public int DureeMoyTraiteAvant15Jr { get; set; }
       [DataMember]
       public int TauxTraitementAvant15Jr { get; set; }
       [DataMember]
       public int NombreTraiteApres15Jr { get; set; }
       [DataMember]
       public int DureeMoyTraiteApres15Jr { get; set; }
       [DataMember]
       public int TauxTraitementApres15Jr { get; set; }

        [DataMember]public int NombreFermeAvant15Jr           { get; set; }
        [DataMember]public int DureeMoyFermeAvant15Jr         { get; set; }
        [DataMember]public int TauxFermetureAvant15Jr         { get; set; }
        [DataMember]public int NombreFermeApres15Jr           { get; set; }
        [DataMember]public int DureeMoyFermeApres15Jr         { get; set; }
        [DataMember]public int TauxFermetureApres15Jr         { get; set; }
    }
}
