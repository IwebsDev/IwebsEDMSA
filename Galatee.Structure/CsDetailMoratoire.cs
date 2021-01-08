using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailMoratoire : CsPrint
    {
       [DataMember]public int PK_ID { get; set; }
       [DataMember]public string CENTRE { get; set; }
       [DataMember]public string CLIENT { get; set; }
       [DataMember]public string ORDRE { get; set; }
       [DataMember]public string REFEM { get; set; }
       [DataMember]public string NDOC { get; set; }
       [DataMember]public string NATURE { get; set; }
       [DataMember]public string COPER { get; set; }
       [DataMember]public System.DateTime DENR { get; set; }
       [DataMember]public Nullable<int> EXIG { get; set; }
       [DataMember]public Nullable<decimal> MONTANT { get; set; }
       [DataMember]public string CAPUR { get; set; }
       [DataMember]public string CRET { get; set; }
       [DataMember]public string MODEREG { get; set; }
       [DataMember]public string DC { get; set; }
       [DataMember]public string ORIGINE { get; set; }
       [DataMember]public string CAISSE { get; set; }
       [DataMember]public Nullable<decimal> ECART { get; set; }
       [DataMember]public string MOISCOMPT { get; set; }
       [DataMember]public string TOP1 { get; set; }
       [DataMember]public Nullable<System.DateTime> EXIGIBILITE { get; set; }
       [DataMember]public Nullable<decimal> FRAISDERETARD { get; set; }
       [DataMember]public Nullable<int> REFERENCEPUPITRE { get; set; }
       [DataMember]public Nullable<int> IDLOT { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEVALEUR { get; set; }
       [DataMember]public string REFERENCE { get; set; }
       [DataMember]public string REFEMNDOC { get; set; }
       [DataMember]public string ACQUIT { get; set; }
       [DataMember]public string MATRICULE { get; set; }
       [DataMember]public Nullable<decimal> TAXESADEDUIRE { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEFLAG { get; set; }
       [DataMember]public Nullable<decimal> MONTANTTVA { get; set; }
       [DataMember]public string IDCOUPURE { get; set; }
       [DataMember]public string AGENT_COUPURE { get; set; }
       [DataMember]public Nullable<System.DateTime> RDV_COUPURE { get; set; }
       [DataMember]public string NUMCHEQ { get; set; }
       [DataMember]public string OBSERVATION_COUPURE { get; set; }
       [DataMember]public string USERCREATION { get; set; }
       [DataMember]public Nullable<System.DateTime>  DATECREATION { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember]public string USERMODIFICATION { get; set; }
       [DataMember]public string BANQUE { get; set; }
       [DataMember]public string GUICHET { get; set; }
       [DataMember]public int FK_IDCENTRE { get; set; }
       [DataMember]public int FK_IDNATURE { get; set; }
       [DataMember]public int FK_IDADMUTILISATEUR { get; set; }
       [DataMember]public int FK_IDCOPER { get; set; }
       [DataMember]public int FK_IDLIBELLETOP { get; set; }
       [DataMember]public int FK_IDCLIENT { get; set; }
       [DataMember]public int FK_IDLCLIENT { get; set; }
       [DataMember] public Nullable<int>  FK_IDNAF { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEENCAISSEMENT { get; set; }
       [DataMember]public int IDMORATOIRE { get; set; }
       [DataMember]public int FK_IDMORATOIRE { get; set; }
       [DataMember]public Nullable<decimal> MONTANTPAYE { get; set; }
       [DataMember]public string NOMABON { get; set; }
       [DataMember]public int STATUS { get; set; }
       [DataMember]public Nullable<System.DateTime> DATECAISSE { get; set; }

    }

}









