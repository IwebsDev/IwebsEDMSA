using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure
{
    [DataContract]
    public class ObjDOCUMENTSCANNE
    {


        [DataMember] public Guid PK_ID { get; set; }
        [DataMember] public Guid OriginalPK_ID { get; set; }
        [DataMember] public String NOMDOCUMENT { get; set; }

        /*18/02/2019 */
        [DataMember] public String CHEMININIT { get; set; }
        [DataMember] public String CHEMINCOPY { get; set; }
        [DataMember] public String NOMDUFICHIER { get; set; }
        
        /***/

        [DataMember] public int FK_IDDEMANDE { get; set; }
        [DataMember] public bool ISNEW { get; set; }
        [DataMember] public byte[] CONTENU { get; set; }
        [DataMember] public bool IsAutorisation { get; set; }
        [DataMember] public bool ISUPDATE { get; set; }
        [DataMember] public bool ISTOREMOVE { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public Nullable<int> FK_IDTYPEDOCUMENT { get; set; }
        [DataMember] public string CODETYPEDOC { get; set; }
        [DataMember] public bool ismigre { get; set; }

        

    }
}
