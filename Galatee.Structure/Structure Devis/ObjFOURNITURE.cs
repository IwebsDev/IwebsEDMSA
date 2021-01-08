using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Galatee.Structure

{
    [DataContract]
    public class ObjFOURNITURE : CsPrint
    {
       [DataMember] public string  CODE { get; set; }
       [DataMember] public int FK_IDMATERIELDEVIS { get; set; }
       [DataMember] public int FK_IDTYPEDEMANDE { get; set; }
       [DataMember] public string CODEPRODUIT { get; set; }
       [DataMember] public Nullable<decimal> COUTUNITAIRE  { get; set; }
       [DataMember] public Nullable<decimal> COUTUNITAIRE_FOURNITURE { get; set; }
       [DataMember] public Nullable<decimal> COUTUNITAIRE_POSE { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public string DIAMETRE { get; set; }
       [DataMember] public Nullable<int> QUANTITY { get; set; }
       [DataMember] public Nullable<bool> ISSUMMARY { get; set; }
       [DataMember] public Nullable<bool> ISADDITIONAL { get; set; }
       [DataMember] public Nullable<bool> ISEXTENSION { get; set; }
       [DataMember] public Nullable<bool> ISDEFAULT { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public string PRIX_UNITAIRE_TODISPLAY { get; set; }
       [DataMember] public Boolean ISDISTANCE { get; set; }
       [DataMember] public decimal?  TAUXTAXE { get; set; }
       [DataMember] public string  RUBRIQUE { get; set; }

        
        // Autre
        [DataMember] public Boolean IsFOURNITUREETPOSE { get; set; }
        [DataMember] public Boolean IsPOSE { get; set; }
        [DataMember] public Boolean IsFOURNITURE { get; set; }
        [DataMember] public Boolean IsPRESTATION { get; set; }
        [DataMember] public String UTILISE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLETYPEDEMANDE { get; set; }
        [DataMember] public decimal?  MONTANTHT { get; set; }
        [DataMember] public decimal?  MONTANTTAXE { get; set; }
        [DataMember] public decimal?  MONTANTTC { get; set; }
        [DataMember] public decimal   PRIX { get; set; }




    }
}
