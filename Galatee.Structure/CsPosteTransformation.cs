using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsPosteTransformation : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string OriginalCODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDDEPARTHTA { get; set; }

        [DataMember] public string CODEDEPARTHTA { get; set; }
        [DataMember] public string LIBELLEDEPARTHTA { get; set; }

    }
 }









