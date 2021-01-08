using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsGroupProgram
    {

       [DataMember]
        public string Id { get; set; }
       [DataMember]
       public string Libelle { get; set; }

    }

}









