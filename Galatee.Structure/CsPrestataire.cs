using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    public class CsPrestataire : CsPrint
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }

        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
