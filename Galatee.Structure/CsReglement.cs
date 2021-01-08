using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Galatee.Structure
{
    [Serializable]
    [DataContract]
    public class CsReglement : CsPrint
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string REFEM { get; set; }
        [DataMember] public string ACQUITANTERIEUR { get; set; }
        [DataMember] public int IdMoratoire { get; set; }
        [DataMember] public int? PK_IDCLIENT { get; set; }
        [DataMember] public int PK_IDTRANSCAISS { get; set; }
        [DataMember] public string NDOC { get; set; }
        [DataMember] public string NATURE { get; set; }
        [DataMember] public string LIBELLEMODREG { get; set; }
        [DataMember] public string LIBCOURT { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public DateTime? DATEFACTURE { get; set; }
        [DataMember] public string CAPUR { get; set; }
        [DataMember] public string IDENTITY { get; set; }
        [DataMember] public string CRET { get; set; }
        [DataMember] public string MODEREG { get; set; }
        [DataMember] public string DC { get; set; }
        [DataMember] public string ORIGINE { get; set; }
        [DataMember] public string CAISSE { get; set; }
        [DataMember] public decimal? ECART { get; set; }
        [DataMember] public decimal? MONTANTNAF { get; set; }
        [DataMember] public string MOISCOMPT { get; set; }
        [DataMember] public string ACQUIT { get; set; }
        [DataMember] public string TOP1 { get; set; }
        [DataMember] public decimal? PERCU { get; set; }
        [DataMember] public decimal? MONTANTPAYMENTANTICIPE { get; set; }
        [DataMember] public decimal? RENDU { get; set; }
        [DataMember] public string PLACE { get; set; }
        [DataMember] public string BANQUE { get; set; }
        [DataMember] public string GUICHET { get; set; }
        [DataMember] public string MOISCOMPTABLE { get; set; }
        [DataMember] public string NUMDEM { get; set; }
        [DataMember] public string NUMDEVIS { get; set; }
        [DataMember] public string NUMCHEQUE { get; set; }
        [DataMember] public string NUMCHEQ { get; set; }

        [DataMember] public string SAISIPAR { get; set; }
        [DataMember] public string NOMOPERATEUR { get; set; }

        [DataMember] public DateTime? DATEENCAISSEMENT { get; set; }
        [DataMember] public DateTime? DATEEXIGIBLE { get; set; }
        [DataMember] public decimal? MONTANTPAYE { get; set; }
        [DataMember] public string MONTANTPAYEFORMATE { get; set; }
        [DataMember] public decimal? PENALITE { get; set; }
        [DataMember] public decimal? SOLDEFACTURE { get; set; }
        [DataMember] public int FK_IDETAPEDEVIS { get; set; }
        [DataMember]
        public string NumCaiss { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDTYPEDEVIS { get; set; }
        [DataMember] public string MATRICULE { get; set; }
        [DataMember] public string NOMCAISSIERE { get; set; }

        [DataMember] public DateTime? DTRANS { get; set; }
        [DataMember] public DateTime? DATEVALEUR { get; set; }
        [DataMember] public decimal? FRAISRETARD { get; set; }
        [DataMember] public int? REFERENCEPUPITRE { get; set; }
        [DataMember] public int? IDLOT { get; set; }
        [DataMember] public string REFERENCE { get; set; }
        [DataMember] public string REFEMNDOC { get; set; }
        [DataMember] public decimal? TAXESADEDUIRE { get; set; }
        [DataMember] public DateTime? DATEFLAG { get; set; }
        [DataMember] public decimal? MONTANTTVA { get; set; }
        [DataMember] public string IDCOUPURE { get; set; }
        [DataMember] public string AGENT_COUPURE { get; set; }
        [DataMember] public DateTime? RDV_COUPURE { get; set; }
        [DataMember] public string OBSERVATION_COUPURE { get; set; }
        [DataMember] public string REFFERENCEACQUIT { get; set; }
        [DataMember] public string REFFERENCECLIENT { get; set; }
        [DataMember] public string TOPLIBELLEREGLAGECOMPTEUR { get; set; }
        [DataMember] public string CANCELLATION { get; set; }
        [DataMember] public string NOMCLIENT { get; set; }
        [DataMember] public decimal? SOLDEANTERIEUR { get; set; }
        [DataMember] public string SOLDEANTERIEURFORMATE { get; set; }
        [DataMember] public decimal? SOLDEALADATE { get; set; }
        [DataMember] public string ZONE { get; set; }
        [DataMember] public decimal? MONTANTFACTURE { get; set; }
        [DataMember] public string PERIODE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string SOLDEALADATEFORMATE { get; set; }
        [DataMember] public string NOMAGENTMANUEL { get; set; }
        [DataMember] public string LIBENCAISS { get; set; }

        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string TOPANNUL { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public string POSTE { get; set; }
        [DataMember] public string FK_IDPOSTE { get; set; }
        [DataMember] public bool  IsSELECT { get; set; }
        [DataMember] public bool  IsDEMANDEANNULATION{ get; set; }
        [DataMember] public string  MOTIFANNULATION{ get; set; }
        [DataMember] public string LIBELLEAGENCE { get; set; }
        [DataMember] public decimal  MONTANTTIMBRE { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public string LIBELLESITE { get; set; }




    }
}









