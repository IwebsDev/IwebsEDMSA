using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    class CsContenantCritere
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string TABLEREFERENCE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string TABLEDONNEES { get; set; }
        [DataMember]
        public string COLONNEDONNEES { get; set; }
        [DataMember]
        public byte TAILLE { get; set; }
        [DataMember]
        public bool AVECPRODUIT { get; set; }
        [DataMember]
        public virtual ICollection<CsCtarcomp> CTARCOMP { get; set; }

    }
}
