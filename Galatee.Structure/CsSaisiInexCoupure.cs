using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSaisiInexCoupure
    {
        [DataMember] public List<CsTypeCoupure > Observations { get; set; }
        [DataMember] public List<CsDetailCampagne > CampagneParCoupure { get; set; }
        [DataMember] public List<CsDetailCampagne > CampagneParClient { get; set; }

        public CsSaisiInexCoupure()
        {
            Observations = new List<CsTypeCoupure>();
            CampagneParCoupure = new List<CsDetailCampagne >();
            CampagneParClient = new List<CsDetailCampagne >();
        }
    }
}
