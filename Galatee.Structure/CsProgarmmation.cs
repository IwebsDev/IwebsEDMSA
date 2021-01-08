using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
     [DataContract]
    public class CsProgarmmation : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEPROGRAMME { get; set; }
        [DataMember] public Nullable<System.Guid> FK_IDEQUIPE { get; set; }
        [DataMember] public string  TYPEDEMANDE { get; set; }
        [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
        [DataMember] public Nullable<bool> ESTACTIF { get; set; }
        [DataMember] public Nullable<bool> ISCOMPTEURLIVRE { get; set; }
        [DataMember] public Nullable<bool> ISMATERIELLIVRE { get; set; }
        [DataMember] public Nullable<bool> ISMATERIELEXTLIVRE { get; set; }
        [DataMember] public string  RECPETEURCOMPTEUR { get; set; }
        [DataMember] public string  RECPETEURMATERIEL { get; set; }
        [DataMember] public Nullable<System.DateTime> DATESORTIECOMPTEUR { get; set; }
        [DataMember] public Nullable<System.DateTime> DATESORTIEMATERIEL { get; set; }

        [DataMember] public string  NOMABON { get; set; }
        [DataMember] public string  LIBELLETYPEDEMANDE { get; set; }
        [DataMember] public string  EQUIPE { get; set; }
        [DataMember] public string  NUMDEM { get; set; }
        [DataMember] public bool   ISPRESTATION { get; set; }

        [DataMember] public string  LIBELLEEQUIPE { get; set; }
        [DataMember] public int  NOMBRE { get; set; }
        [DataMember] public DateTime   DATETRANSMISSION { get; set; }
        [DataMember] public bool   IsSelect { get; set; }

        [DataMember] public string  NUMPROGRAMME { get; set; }

        [DataMember] public string  LIVREURCOMPTEUR { get; set; }
        [DataMember] public DateTime ?  DATELIVRAISONCOMPTEUR { get; set; }
        [DataMember] public string  RECEPTEURCOMPTEUR  { get; set; }

        [DataMember] public string  LIVREURMATERIEL { get; set; }
        [DataMember] public DateTime ?  DATELIVRAISONMATERIEL{ get; set; }
        [DataMember] public string  RECEPTEURMATERIEL  { get; set; }


         

    }
}
