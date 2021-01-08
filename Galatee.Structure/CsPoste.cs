using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsPoste: CsPrint 
    {
         [DataMember] public int PK_ID { get; set; }
         [DataMember] public string CODECENTRE { get; set; }
         [DataMember] public string NOMPOSTE { get; set; }
         [DataMember] public string IPADRESSE { get; set; }
         [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }
         [DataMember] public string NUMCAISSE { get; set; }
         [DataMember] public Nullable<int> FK_IDCAISSE { get; set; }

         [DataMember] public bool  IsSelect { get; set; }
         [DataMember] public string MATRICULE { get; set; }


    }

}









