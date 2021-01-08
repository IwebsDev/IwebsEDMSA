using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjPIECEIDENTITE
    {
        [DataMember]
        public int PK_ID { get; set; }

        //[DataMember]
        //public int OriginalPK_ID { get; set; }

        [DataMember]
        public String LIBELLE { get; set; }

        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}
