using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
        [DataContract]
        public class CsAdmMenu
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

            //NOT NEEDED
            [DataMember]
            public int MENUORDER { get; set; }
            [DataMember]
            public bool ISACTIVE { get; set; }
            [DataMember]
            public string FORMENAME { get; set; }
            [DataMember]
            public Nullable<bool> ISDOCK { get; set; }
            [DataMember]
            public string ICONNAME { get; set; }
            [DataMember]
            public bool ISCONTROLE { get; set; }
            [DataMember]
            public Nullable<int> TYPEREEDITION { get; set; }

            [DataMember]
            public System.DateTime DATECREATION { get; set; }
            [DataMember]
            public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
            [DataMember]
            public string USERCREATION { get; set; }
            [DataMember]
            public string USERMODIFICATION { get; set; }
            

            [DataMember]
            public virtual MODULE MODULE1 { get; set; }
            //[DataMember]
            //public virtual ICollection<HABILITATIONPROGRAM> HABILITATIONPROGRAM { get; set; }
            //[DataMember]
            //public virtual ICollection<MENUSDUPROFIL> MENUSDUPROFIL { get; set; }
            [DataMember]
            public List<CsMenuDuProfil> lstMenuProfil { get; set; }


        
    }
}
