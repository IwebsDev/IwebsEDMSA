using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsModuleAdmMenu
    {
        // CsModuleAdmMenu
        [DataMember]
        public string MODULE { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string MENUTEXT { get; set; }
        [DataMember]
        public Nullable<int> MAINMENUID { get; set; }
        [DataMember]
        public int FK_IDMODULE { get; set; }

        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}
