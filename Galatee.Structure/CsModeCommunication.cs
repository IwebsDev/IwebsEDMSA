using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsModeCommunication : CsPrint
    {
        [DataMember]  public int ID { get; set; }
        [DataMember]  public string LIBELLE { get; set; }
    }
}
