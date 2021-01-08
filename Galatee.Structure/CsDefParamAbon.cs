using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDefParamAbon : CsPrint
    {
        [DataMember]
        public string PK_FK_CENTRE { get; set; }
        [DataMember]
        public string PK_CODE { get; set; }
        [DataMember]
        public string PK_PARAM { get; set; }
        [DataMember]
        public string PK_FK_PRODUIT { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string GROUPE { get; set; }
        [DataMember]
        public string SOUSGROUPE { get; set; }
        [DataMember]
        public string FORMAT { get; set; }
        [DataMember]
        public short? FK_MODESAISIE { get; set; }
        [DataMember]
        public string FK_CODERECHERCHE { get; set; }
        [DataMember]
        public string FK_TRAITEMENT { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public string OriginalCENTRE { get; set; }
        [DataMember]
        public string OriginalCODE { get; set; }
        [DataMember]
        public string OriginalPRODUIT { get; set; }
        [DataMember]
        public string OriginalPARAM { get; set; }
        [DataMember]
        public string LIBELLECENTRE { get; set; }
        [DataMember]
        public string LIBELLECODE { get; set; }
        [DataMember]
        public string LIBELLEPRODUIT { get; set; }
        [DataMember]
        public string LIBELLEPARAM { get; set; }
    }
}
