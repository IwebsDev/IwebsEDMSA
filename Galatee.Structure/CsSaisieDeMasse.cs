using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSaisieDeMasse
    {
    
        #region  collection object
        [DataMember]
        public List<CsDetailLot > LstDetailLot { get; set; }
        #endregion

        #region Object
        [DataMember]
        public CsLotCompteClient LotCompteClient { get; set; }
        #endregion
      


    }
   
}









