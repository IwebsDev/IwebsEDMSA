using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Galatee.Structure

{
    [DataContract]
    public class CsCONNEXIONACTIVE
    {


        [DataMember]
        public String MATRICULEUSERCONNECTE { get; set; }
        [DataMember]
        public DateTime DATEDECONNEXION { get; set; }
        [DataMember]
        public String NOMMACHINE { get; set; }
        [DataMember]
        public String CODEUO { get; set; }


    }
}
