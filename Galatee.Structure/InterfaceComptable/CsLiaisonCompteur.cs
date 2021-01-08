using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure 
{
    public class CsLiaisonCompteur
    {
        public long LOCATION_ID  { get; set; }
        public long LEGAL_ENTITY_ID { get; set; }
        public long OWNER_LEGAL_ENTITY_ID { get; set; }
        public long AGR_ID { get; set; }
        public long RDP_ID { get; set; }
        public long METER_ID { get; set; }
        public long TRF_ID { get; set; }
        public string LEGAL_ENTITY_REF { get; set; }
        public string LEGAL_ENTITY_NAME { get; set; }
        public string MSNO { get; set; }
    }
}
