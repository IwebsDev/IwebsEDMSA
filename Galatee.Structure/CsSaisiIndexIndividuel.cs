using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsSaisiIndexIndividuel
    {
        [DataMember]
        public int? CONSOMOYENNE { get; set; }
        [DataMember]
        public int POINT { get; set; }
        [DataMember]
        public List<CsEvenement> ConsoPrecedent { get; set; }
        [DataMember]
        public List<CsEvenement> EventLotriNull { get; set; }
        [DataMember]
        public List<CsEvenement> EventPageri { get; set; }
        [DataMember]
        public List<CsEvenement> EventPagisol { get; set; }
        public CsSaisiIndexIndividuel()
        {
            ConsoPrecedent = new List<CsEvenement>();
            EventLotriNull = new List<CsEvenement>();
            EventPageri = new List<CsEvenement>();
            EventPagisol = new List<CsEvenement>();
        }
    }
}









