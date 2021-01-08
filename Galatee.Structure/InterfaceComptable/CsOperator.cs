using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsOperator
    {
        [DataMember] public long OPERATOR_ID { get; set; }
        [DataMember] public string OPERATOR_NAME { get; set; }
    }
}
