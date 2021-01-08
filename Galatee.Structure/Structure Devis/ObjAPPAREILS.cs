using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjAPPAREILS : CsPrint
    {
        [DataMember]public string CODEAPPAREIL { get; set; }
        [DataMember]public int PK_ID { get; set; }
        [DataMember]public int PK_IDAPPAREILDEVIS { get; set; } // pour conserver le PK_ID de chaque appareil de devis
        [DataMember]public String DESIGNATION { get; set; }
        [DataMember]public String DETAILS { get; set; }
        [DataMember]public int NOMBRE { get; set; }
        [DataMember]public int TEMPSUTILISATION { get; set; }
        [DataMember]public int PUISSANCE { get; set; }
        [DataMember]public string DISPLAYLABEL { get; set; }
        [DataMember]public DateTime? DATECREATION { get; set; }
        [DataMember]public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]public string USERCREATION { get; set; }
        [DataMember]public string USERMODIFICATION { get; set; }

    }
}
