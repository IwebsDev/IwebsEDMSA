using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCorrespondance : CsPrint
    {
        [DataMember]
        public string ClasseCompte { get; set; }
        [DataMember]
        public string Ci { get; set; }
        [DataMember]
        public string Filiere { get; set; }
        [DataMember]
        public string SScompte { get; set; }
        [DataMember]
        public string Localisation { get; set; }
        [DataMember]
        public string CodeAgence { get; set; }
    }
}
