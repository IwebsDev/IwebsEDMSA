using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCommentaireRejet
    {
        [DataMember]
        public System.Guid PK_ID { get; set; }
        [DataMember]
        public System.DateTime DATECOMMENTAIRE { get; set; }
        [DataMember]
        public string COMMENTAIRE { get; set; }
        [DataMember]
        public byte[] PIECE_JOINTE { get; set; }
        [DataMember]
        public string CODEDEMANDE { get; set; }
        [DataMember]
        public System.Guid FK_IDDEMANDE { get; set; }
        [DataMember]
        public int FK_IDETAPE { get; set; }
        [DataMember]
        public string UTILISATEUR { get; set; }
        [DataMember]
        public string NOMUTILISATEUR { get; set; }
    }
}
