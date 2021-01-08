using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMobileInit
    {
        //[DataMember]
        //public DataTable Releveur { get; set; }

        //[DataMember]
        //public DataTable AdmMenu { get; set; }

        //[DataMember]
        //public DataTable Client { get; set; }

        //[DataMember]
        //public DataTable Abon { get; set; }

        //[DataMember]
        //public DataTable Historique { get; set; }

        //[DataMember]
        //public DataTable Tournee { get; set; }

        //[DataMember]
        //public DataTable Ag { get; set; }

        //[DataMember]
        //public DataTable Canalisation { get; set; }

        [DataMember]
        public List<CASIND> CasIndex { get; set; }

        //[DataMember]
        //public DataTable Evenement { get; set; }

        //[DataMember]
        //public DataTable Strategie { get; set; }

    }
}
