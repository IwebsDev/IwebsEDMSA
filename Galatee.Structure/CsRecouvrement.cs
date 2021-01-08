using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRecouvrement : CsPrint
    {
        [DataMember]
        public List<aBanque> Banque {set;get;}
        [DataMember]
        public List<CsCentre> Centre { set; get; }

        public CsRecouvrement()
        {
            Banque = new List<aBanque>();
            Centre = new List<CsCentre>();
        }
    }
}
