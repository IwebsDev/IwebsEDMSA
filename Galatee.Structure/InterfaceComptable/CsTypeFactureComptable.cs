using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeFactureComptable
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public List<string> LISTCOPER { get; set; }
        [DataMember]
        public List<CsTypeCompte> LISTTYPECOMPTE { get; set; }


    }
}
