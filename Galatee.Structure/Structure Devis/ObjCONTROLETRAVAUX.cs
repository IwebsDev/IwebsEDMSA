using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjCONTROLETRAVAUX
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDMATRICULE { get; set; }
        [DataMember]
        public int FK_IDDEVIS { get; set; }
        [DataMember]
        public String NUMDEVIS { get; set; }
        [DataMember]
        public int ORDRE { get; set; }
        [DataMember]
        public String MATRICULECHEFEQUIPE { get; set; }
        [DataMember]
        public String NOMCHEFEQUIPE { get; set; }
        [DataMember]
        public String METMOYCONTROLE { get; set; }
        [DataMember]
        public DateTime? DATECONTROLE { get; set; }
        [DataMember]
        public String VOLUMETERTRVX { get; set; }
        [DataMember]
        public String DEGRADATIONVOIE { get; set; }
        [DataMember]
        public int NOTE { get; set; }
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
