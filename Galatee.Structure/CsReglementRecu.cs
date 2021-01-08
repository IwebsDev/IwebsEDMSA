using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Galatee.Structure
{
    [DataContract]
    public class CsReglementRecu : CsPrint
    {
        [DataMember] public string CENTREENCAISSE { get; set; }
        [DataMember] public string SITEENCAISSEMENT { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string REFEM { get; set; }
        [DataMember] public string NDOC { get; set; }
        [DataMember] public string LIBELLEMODREG { get; set; }
        [DataMember] public DateTime? DATEFACTURE { get; set; }
        [DataMember] public string CAISSE { get; set; }
        [DataMember] public string ACQUIT { get; set; }
        [DataMember] public decimal? PERCU { get; set; }
        [DataMember] public decimal? RENDU { get; set; }
        [DataMember] public DateTime? DATEENCAISSEMENT { get; set; }
        [DataMember] public decimal? MONTANTPAYE { get; set; }
        [DataMember] public decimal? MONTANTTIMBRE { get; set; }
        [DataMember] public decimal? MONTANTHT { get; set; }
        [DataMember] public string MATRICULE { get; set; }
        [DataMember] public string NOMCAISSIERE { get; set; }
        [DataMember] public string BANQUE { get; set; }
        [DataMember] public string NUMCHEQ { get; set; }
        [DataMember] public string NOMCLIENT { get; set; }
        [DataMember] public decimal? SOLDEANTERIEUR { get; set; }
        [DataMember] public decimal? SOLDEALADATE { get; set; }
        [DataMember] public decimal? MONTANTEXIGIBLE { get; set; }
        [DataMember] public decimal? MONTANTNONEXIGIBLE { get; set; }
        [DataMember]  public string TOPANNUL { get; set; }
        [DataMember]  public string LIBELLEFACTURE { get; set; }
        [DataMember]  public string TYPEEDITION { get; set; }

    }
}









