using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsParametre 
    {
        [DataMember]
        public string GestionAutoMoisCompt { get; set; }
          [DataMember]
        public string TypeExportFichier { get; set; }
        [DataMember]
        public string ModeTraitement { get; set; }
             [DataMember]
        public string CheminFichier { get; set; }
             [DataMember]
             public string CompteAnalytique { get; set; }
    }
}
