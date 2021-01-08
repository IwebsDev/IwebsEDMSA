using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    public class CsFourniture : CsPrint 
    {
        public int FK_IDTYPEDEMANDE { get; set; }
        public string REGLAGECOMPTEUR { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<bool> ISSUMMARY { get; set; }
        public Nullable<bool> ISADDITIONAL { get; set; }
        public Nullable<bool> ISEXTENSION { get; set; }
        public Nullable<bool> ISDEFAULT { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
        public int FK_IDPRODUIT { get; set; }
        public Nullable<int> FK_IDMATERIELDEVIS { get; set; }



        [DataMember] public string CODEMATERIEL { get; set; }
        [DataMember] public string LIBELLEMATERIEL { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public decimal PRIX_UNITAIRE { get; set; }
 
    }
}
