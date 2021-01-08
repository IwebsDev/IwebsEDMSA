using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsJournal : CsPrint
    {
        [DataMember]
        public string Ventes { get; set; }
        [DataMember]
        public string Encaiss { get; set; }
        [DataMember]
        public string PaieCaisse { get; set; }
        [DataMember]
        public string OperDivers { get; set; }
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string CodeAgence { get; set; }
    }
}
