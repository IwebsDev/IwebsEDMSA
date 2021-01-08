using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using Inova.Tools.Utilities;

namespace Galatee.Structure
{
    [DataContract]
  public  class CsMArqueDisjoncteur
  {
         [DataMember]
          public int PK_ID { get; set; }
         [DataMember]
         public string Libelle { get; set; }
       

    
    }
}
