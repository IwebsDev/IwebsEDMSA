using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailLot : CsPrint
    {
       [DataMember]public string NUMEROLOT { get; set; }
       [DataMember] public int IDLOT { get; set; }
       [DataMember] public string NUMEROLIGNE { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRE { get; set; }
       [DataMember] public string REFEM { get; set; }
       [DataMember] public string NDOC { get; set; }
       [DataMember] public decimal? MONTANT { get; set; }
       [DataMember] public string COPER { get; set; }
       [DataMember] public string NATURE { get; set; }
       [DataMember] public string MODEREG { get; set; }
       [DataMember] public string ACQUIT { get; set; }
       [DataMember] public System.DateTime? DATEPIECE { get; set; }
       [DataMember] public System.DateTime? DATESAISIE { get; set; }
       [DataMember] public System.DateTime? EXIGIBILITE { get; set; }
       [DataMember] public decimal? ECART { get; set; }
       [DataMember] public string CODEERR { get; set; }
       [DataMember] public string SENS { get; set; }
       [DataMember] public string REFERENCE { get; set; }
       [DataMember] public string MATRICULE { get; set; }
       [DataMember] public System.DateTime? DATETRAIT { get; set; }
       [DataMember] public string REFEMNDOC { get; set; }
       [DataMember] public string TOP1 { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDLOTCOMPECLIENT { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int? FK_IDLOT { get; set; }
       [DataMember] public int FK_IDCOPER { get; set; }
       [DataMember] public int? FK_IDNATURE { get; set; }
       [DataMember] public int FK_IDMATRICULE { get; set; }
       [DataMember] public int? FK_IDLIBELLETOP { get; set; }
       [DataMember] public int? FK_IDCLIENT { get; set; }
       [DataMember] public int? FK_IDLCLIENT { get; set; }
       [DataMember]
       public Nullable<bool> STATUT { get; set; }
       [DataMember]
       public decimal? MONTANT_AJUSTEMENT { get; set; }
        /* Autre*/
       [DataMember] public string  LIBELLELOT { get; set; }
       [DataMember] public string  TYPELOT { get; set; }
       [DataMember] public int IsACTION { get; set; }
        

        
    }

}









