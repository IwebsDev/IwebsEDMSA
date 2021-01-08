using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
    public class CsClasseurClient:CsPrint 
    {
    
        #region  collection object
        [DataMember]
        public List<CsEvenement> LstEvenement { get; set; }
        [DataMember]
        public List<CsFacture > LstFacture { get; set; }
        [DataMember]
        public List<CsCanalisation> LstCanalistion { get; set; }
        [DataMember]
        public List<CsAbon > LstAbonnement { get; set; }
        [DataMember]
        public List<CsBrt > LstBranchement { get; set; }
        #endregion

        #region Object
        [DataMember]
        public CsAg Ag { get; set; }
        [DataMember]
        public CsClient LeClient { get; set; }
        [DataMember]
        public CsCompteClient  LeCompteClient { get; set; }
        #endregion
      


    }
   
}









