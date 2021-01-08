using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSortieAutreMateriel
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDDEMANDE { get; set; }
        [DataMember]
        public int FK_IDTYPEMATERIEL { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string NOMBRE { get; set; }
        [DataMember]
        public string LIVRE { get; set; }
        [DataMember]
        public string RECU { get; set; }


    }
}
