using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Galatee.Structure
{
    [Serializable]
    [DataContract]
    public class CsRegrou
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string REGROU { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string NOM { get; set; }
        [DataMember]
        public int CUBGEN { get; set; }
        [DataMember]
        public int CUBDIV { get; set; }
        [DataMember]
        public DateTime DMAJ { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public byte[] ROWID { get; set; }
              
    }


}









