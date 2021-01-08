using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsProgram
    {

       [DataMember]
        public int Id { get; set; }
       [DataMember]
       public string IdGroupProgram { get; set; }
       [DataMember]
       public string Code { get; set; }
       [DataMember]
       public string Libelle { get; set; }

    }

}









