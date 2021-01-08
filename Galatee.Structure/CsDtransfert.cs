using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsDtransfert : CsPrint 
    {
       [DataMember] public string NUMDEM { get; set; }
       [DataMember] public int FK_IDCENTREORIGINE { get; set; }
       [DataMember] public int FK_IDCENTRETRANSFERT { get; set; }
       [DataMember] public int? FK_IDREGROUPEMENT { get; set; }
       [DataMember] public int FK_IDDEMANDE { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public bool TRANSFERIMPAYE { get; set; }

       [DataMember] public string LIBELLEREGROUPEMENT { get; set; }
       [DataMember] public string CODEREGROUPEMENT { get; set; }
       [DataMember] public string CODECENTRETRANSFERT { get; set; }

       [DataMember] public string LIBELLESITEORIGINE { get; set; }
       [DataMember] public string LIBELLECENTREORIGINE { get; set; }
       [DataMember] public string LIBELLECENTRETRANSFERT { get; set; }
       [DataMember] public string LIBELLESITETRANSFERT { get; set; }

    }
}









