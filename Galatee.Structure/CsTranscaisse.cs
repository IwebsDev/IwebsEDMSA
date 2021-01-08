using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsTranscaisse:CsPrint
    {
       [DataMember]public string CENTRE { get; set; }
       [DataMember]public string CLIENT { get; set; }
       [DataMember]public string ORDRE { get; set; }
       [DataMember]public string CAISSE { get; set; }
       [DataMember]public string ACQUIT { get; set; }
       [DataMember]public string MATRICULE { get; set; }
       [DataMember]public string NDOC { get; set; }
       [DataMember]public string REFEM { get; set; }
       [DataMember]public string NATURE { get; set; }
       [DataMember]public decimal? MONTANT { get; set; }
       [DataMember]public string DC { get; set; }
       [DataMember]public string COPER { get; set; }
       [DataMember]public decimal? PERCU { get; set; }
       [DataMember]public decimal? RENDU { get; set; }
       [DataMember]public string MODEREG { get; set; }
       [DataMember]public string PLACE { get; set; }
       [DataMember]public System.DateTime? DTRANS { get; set; }
       [DataMember]public System.DateTime? DEXIG { get; set; }
       [DataMember]public string BANQUE { get; set; }
       [DataMember]public string GUICHET { get; set; }
       [DataMember]public string ORIGINE { get; set; }
       [DataMember]public decimal? ECART { get; set; }
       [DataMember]public string TOPANNUL { get; set; }
       [DataMember]public string MOISCOMPT { get; set; }
       [DataMember]public string TOP1 { get; set; }
       [DataMember]public string TOURNEE { get; set; }
       [DataMember]public string NUMDEM { get; set; }
       [DataMember]public System.DateTime? DATEVALEUR { get; set; }
       [DataMember]public System.DateTime? DATEFLAG { get; set; }
       [DataMember]public string NUMDEVIS { get; set; }
       [DataMember]public string NUMCHEQ { get; set; }
       [DataMember]public string SAISIPAR { get; set; }
       [DataMember]public System.DateTime? DATEENCAISSEMENT { get; set; }
       [DataMember]public string CANCELLATION { get; set; }
       [DataMember]public string USERCREATION { get; set; }
       [DataMember]public System.DateTime DATECREATION { get; set; }
       [DataMember]public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember]public string USERMODIFICATION { get; set; }
       [DataMember]public int PK_ID { get; set; }
       [DataMember]public int FK_IDCENTRE { get; set; }
       [DataMember]public int FK_IDCAISSE { get; set; }
       [DataMember]public int FK_IDNATURE { get; set; }
       [DataMember]public int FK_IDMODEREG { get; set; }
       [DataMember]public int FK_IDLIBELLETOP { get; set; }
       [DataMember]public int FK_IDMATRICULE { get; set; }
       [DataMember]public int FK_IDCOPER { get; set; }

       [DataMember] public string POSTE { get; set; }
       [DataMember] public string FK_IDPOSTE { get; set; }


        /*autre*/
       [DataMember]public string NOMCLIENT { get; set; }       
       [DataMember]public string LIBELLEMODREG { get; set; }
       [DataMember]public string NOMCAISSIERE { get; set; }
       [DataMember]public string NOMOPERATEUR { get; set; }
       [DataMember] public string LIBELLECOPER { get; set; }
       [DataMember] public string LIBELLENATURE { get; set; }
       [DataMember] public int IDLOT { get; set; }


       [DataMember]public decimal SOLDECLIENT { get; set; }
    }
}









