using System;
//using System;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace Galatee.Structure
{
    [DataContract]
    public class CImprimante
    {
        [DataMember]
        public string NomImprimante { get; set; }
    }

}









