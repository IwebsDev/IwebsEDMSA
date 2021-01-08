using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsGroupe : CsPrint
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public int ID_TYPE_GROUPE { get; set; }
        [DataMember]
        public string ID_ENTITE { get; set; }
        [DataMember]
        public int ID_TYPE_RECLAMATION { get; set; }
        [DataMember]
        public bool EST_SUPPRIME { get; set; }
        [DataMember]
        public int? ID_NATURE_BASE { get; set; }
        [DataMember]
        public string CREER_PAR { get; set; }
        [DataMember]
        public DateTime DATE_CREATION { get; set; }
        [DataMember]
        public string DERNIER_UTILISATEUR { get; set; }
        [DataMember]
        public DateTime? DATE_MODIFICATION { get; set; }
    }
}
