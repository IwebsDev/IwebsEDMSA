using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCaisse : CsPrint
    {
           [DataMember] public string NUMCAISSE { get; set; }
           [DataMember] public string ACQUIT { get; set; }
           [DataMember] public string BORDEREAU { get; set; }
           [DataMember] public Nullable<decimal> FONDCAISSE { get; set; }
           [DataMember] public string COMPTE { get; set; }
           [DataMember] public string LIBELLE { get; set; }
           [DataMember] public string TYPECAISSE { get; set; }
           [DataMember] public string USERCREATION { get; set; }
           [DataMember] public System.DateTime DATECREATION { get; set; }
           [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
           [DataMember] public string USERMODIFICATION { get; set; }
           [DataMember] public int PK_ID { get; set; }
           [DataMember] public string CENTRE { get; set; }
           [DataMember] public string LIBELLECENTRE { get; set; }
          
           [DataMember] public int FK_IDCENTRE { get; set; }
           [DataMember] public Nullable<bool> ESTATTRIBUEE { get; set; }        

           [DataMember] public DateTime  DATECAISSE { get; set; }
           [DataMember] public int FK_HABILITATIONCAISSE{ get; set; }
           [DataMember] public int FK_IDADMUTILISATEUR { get; set; }
           [DataMember] public DateTime DATE_DEBUT{ get; set; }
           [DataMember] public DateTime DATE_FIN{ get; set; }
        
    }

}









