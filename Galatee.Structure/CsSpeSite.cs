using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsSpeSite
    {
        # region Generale
        [DataMember]public string SITE { get; set; }
        [DataMember]public string NOM { get; set; }
        [DataMember]public Nullable<int> NUMDEM { get; set; }
        [DataMember]public string LOTREL { get; set; }
        [DataMember]public string LOTFAC { get; set; }
        [DataMember]public string TRANS { get; set; }
        [DataMember]public string LIENFAC { get; set; }
        [DataMember]public Nullable<int> STATUS_ATM { get; set; }
        [DataMember]public int PK_ID { get; set; }
        [DataMember]public System.DateTime DATECREATION { get; set; }
        [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]public string USERCREATION { get; set; }
        [DataMember]public string USERMODIFICATION { get; set; }
        [DataMember]public int FK_IDSITE { get; set; }

        #endregion

      
    }

}









