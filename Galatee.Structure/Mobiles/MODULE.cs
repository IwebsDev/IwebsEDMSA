using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class MODULE
    {
        [DataMember]
        public string NOM { get; set; }

        //[DataMember]
        //public List<MODULE> SOUSMODULE { get; set; }

        [DataMember]
        public string RELEVEUR { get; set; }

        [DataMember]
        public int? ID { get; set; }

        [DataMember]
        public int? IDPARENT { get; set; }

        //public MODULE()
        //{
        //    //SOUSMODULE = new List<MODULE>();
        //}
    }
}
