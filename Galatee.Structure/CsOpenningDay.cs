using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsOpenningDay
    {
        [DataMember]
        public string DateOperationOuverture { get; set; }

        [DataMember]
        public string OperateurOuverture { get; set; }

        [DataMember]
        public string Caissiere { get; set; }

        [DataMember]
        public string DateEncaissement { get; set; }

        [DataMember]
        public string SaisiePar { get; set; }

        [DataMember]
        public string Raison { get; set; } 
    }

}









