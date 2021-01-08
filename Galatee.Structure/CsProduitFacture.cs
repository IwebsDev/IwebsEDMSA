using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsProduitFacture : CsPrint
    {
       [DataMember]public string CODE { get; set; }
       [DataMember]public string LOTRI { get; set; }
       [DataMember]public string JET { get; set; }
       [DataMember]public string DR { get; set; }
       [DataMember]public string TOURNEE { get; set; }
       [DataMember]public string ORDTOUR { get; set; }
       [DataMember]public string CENTRE { get; set; }
       [DataMember]public string CLIENT { get; set; }
       [DataMember]public string ORDRE { get; set; }
       [DataMember]public string FACTURE { get; set; }
       [DataMember]public string PRODUIT { get; set; }
       [DataMember]public string COMPTEUR { get; set; }
       [DataMember]public string REGLAGECOMPTEUR { get; set; }
       [DataMember]public string TYPECOMPTAGE { get; set; }
       [DataMember]public Nullable<decimal> COEFLECT { get; set; }
       [DataMember]public Nullable<int> POINT { get; set; }
       [DataMember]public Nullable<decimal> PUISSANCE { get; set; }

       [DataMember]public string ROWIDCO { get; set; }
       [DataMember]public string DERPERF { get; set; }
       [DataMember]public string DERPERFN { get; set; }
       [DataMember]public Nullable<int> REGCONSO { get; set; }
       [DataMember]public Nullable<int> REGFAC { get; set; }
       [DataMember]public string TFAC { get; set; }
       [DataMember]public Nullable<int> LIENRED { get; set; }
       [DataMember]public Nullable<int> CONSOFAC { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEEVT { get; set; }
       [DataMember]public string PERIODE { get; set; }
       [DataMember]public Nullable<int> AINDEX { get; set; }
       [DataMember]public Nullable<int> NINDEX { get; set; }
       [DataMember]public string CAS { get; set; }
       [DataMember]public Nullable<int> CONSO { get; set; }
       [DataMember]public Nullable<decimal> TOTPROHT { get; set; }
       [DataMember]public Nullable<decimal> TOTPROTAX { get; set; }
       [DataMember]public Nullable<decimal> TOTPROTTC { get; set; }
       [DataMember]public string ADERPERF { get; set; }
       [DataMember]public string ADERPERFN { get; set; }
       [DataMember]public Nullable<int> REGIMPUTE { get; set; }
       [DataMember]public string TYPECOMPTEUR { get; set; }
       [DataMember]public string REGROU { get; set; }
       [DataMember]public Nullable<System.DateTime> DEVPRE { get; set; }
       [DataMember]public Nullable<int> NBREDTOT { get; set; }
       [DataMember]public Nullable<int> STATUS { get; set; }
       [DataMember]public string LIENFAC { get; set; }
       [DataMember]public Nullable<int> EVENEMENT { get; set; }
       [DataMember]public string TOPMAJ { get; set; }
       [DataMember]public Nullable<int> TOPANNUL { get; set; }
       [DataMember]public Nullable<decimal> PUISSANCEINSTALLEE { get; set; }
       [DataMember]public Nullable<int> COEFCOMPTAGE { get; set; }
       [DataMember]public string BRANCHEMENT { get; set; }
       [DataMember]public Nullable<decimal> COEFK1 { get; set; }
       [DataMember]public Nullable<decimal> COEFK2 { get; set; }
       [DataMember]public Nullable<decimal> PERTESACTIVES { get; set; }
       [DataMember]public Nullable<decimal> PERTESREACTIVES { get; set; }
       [DataMember]public Nullable<int> COEFFAC { get; set; }
       [DataMember]public int PK_ID { get; set; }
       [DataMember]public Nullable<int> FK_IDENTFAC { get; set; }
       [DataMember]public Nullable<int> FK_IDEVENEMENT { get; set; }
       [DataMember]public System.DateTime DATECREATION { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember]public string USERCREATION { get; set; }
       [DataMember]public string USERMODIFICATION { get; set; }
       [DataMember]public int FK_IDPRODUIT { get; set; }
       [DataMember]public int FK_IDCAS { get; set; }
       [DataMember]public int FK_IDCENTRE { get; set; }
       [DataMember]public int FK_IDABON { get; set; }

        /*Autre*/
       [DataMember]public string LIBELLEPRODUIT { get; set; }
       [DataMember]public bool  AVEC_PERTE { get; set; }
       [DataMember]public string ANNEE { get; set; }


    }
}
