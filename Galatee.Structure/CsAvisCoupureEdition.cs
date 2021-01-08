using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAvisCoupureEdition
    {
        [DataMember] public List<int> Tournees { get; set; }
        [DataMember] public List<int> Categories { get; set; }
        [DataMember] public List<int> Consomateur { get; set; }
        [DataMember] public string Site { get; set; }
        [DataMember] public string Centre { get; set; }
        [DataMember] public string AgentPia { get; set; }
        [DataMember] public string MatriculeDebut { get; set; }
        [DataMember] public string MatriculeFin { get; set; }
        [DataMember] public DateTime? Exigible { get; set; }
        [DataMember] public int? NombreFactureTotalClient { get; set; }
      
        [DataMember] public string IdCoupure { get; set; }
        [DataMember] public bool? ClientResilie { get; set; }
        [DataMember] public bool? ClientGroupe { get; set; }
        [DataMember] public decimal? MontantPeriode { get; set; }
        [DataMember] public string Periode { get; set; }
        [DataMember] public decimal? SoldeMinimum { get; set; }
        [DataMember] public int? NombreTotalDeClient { get; set; }

        [DataMember] public List<CsCentre> Centre_Campagne { get; set; }
        [DataMember] public bool IsReedition { get; set; }
        [DataMember] public string Matricule {get;set;}
        [DataMember] public string NomAgentPia {get;set;}
        [DataMember] public int idCentre { get; set; }
        [DataMember] public int idAgentPia { get; set; }
        [DataMember] public decimal? MontantRelancable { get; set; }
        [DataMember] public string TourneeDebut {get;set;}
        [DataMember] public string TourneeFin {get;set;}
        [DataMember] public string CategorieDebut {get;set;}
        [DataMember] public string CategorieFin {get;set;}

        [DataMember] public string referenceClientDebut {get;set;}
        [DataMember] public string referenceClientFin {get;set;}
        [DataMember] public string OrdreTourneDebut {get;set;}
        [DataMember] public string OrdreTourneFin {get;set;}


        [DataMember] public bool IsParAbonnement { get; set; }
        [DataMember] public int idRegroupement { get; set; }
        [DataMember] public string Regroupement {get;set;}

        [DataMember] public string PeriodeDebut {get;set;}
        [DataMember] public string PeriodeFin {get;set;}


        [DataMember] public List<CsRegCli> ListeRegroupement { get; set; }
    }
}
