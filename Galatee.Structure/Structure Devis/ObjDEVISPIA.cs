using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjDEVISPIA
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public String NUMDEVIS { get; set; }
        [DataMember]
        public int FK_IDDEVIS { get; set; }
        [DataMember]
        public int FK_IDUSER { get; set; }
        [DataMember]
        public int ORDRE { get; set; }
        [DataMember]
        public String MATRICULEPIA { get; set; }
        [DataMember]
        public DateTime? DATEPIA { get; set; }
        [DataMember]
        public String NOMMETREUR { get; set; }
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
