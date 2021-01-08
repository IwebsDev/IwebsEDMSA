using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsProfil
    {

        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDFONCTION { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string CODE { get; set; }

        [DataMember] public int FK_IDADMUTILISATEUR { get; set; }
        [DataMember] public int FK_IDPROFIL { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEDEBUT { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEFIN { get; set; }


        [DataMember] public string CODEFONCTION { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public string MODULE { get; set; }
        [DataMember] public bool  IsSelect { get; set; }
        [DataMember] public List<CsCentreProfil> LESCENTRESPROFIL { get; set; }


         
    }
}
