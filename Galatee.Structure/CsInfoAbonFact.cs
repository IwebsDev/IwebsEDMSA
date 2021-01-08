using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
    public class CsInfoAbonFact
    {
        [DataMember]
        public CsAbon  Abonne { get; set; }
        [DataMember]
        public CsBrt  Branchement { get; set; }
        [DataMember]
        public CsCanalisation  Compteur { get; set; }
        [DataMember]
        public CsAg  AdresseGeographique { get; set; }
        [DataMember]
        public CsTournee  Tournee { get; set; }
        [DataMember]
        public CsClient  Client { get; set; }
        [DataMember]
        public CsPageri  Pagerie { get; set; }
    }

}









