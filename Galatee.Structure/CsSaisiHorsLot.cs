using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsSaisiHorsLot
    {
        [DataMember] public List<CsCanalisation>  Compteurs  { get; set; }
        [DataMember] public List<CsSaisiIndexIndividuel>  IndexInfo  { get; set; }
        
        public CsSaisiHorsLot()
        {
            Compteurs = new List<CsCanalisation>();
            IndexInfo = new List<CsSaisiIndexIndividuel>();
        }
    }
}









