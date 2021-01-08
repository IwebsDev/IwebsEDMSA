using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Galatee.Structure
{
    [DataContract]
    public class CSMenuGalatee
    {
        [DataMember]
        public int? Tdem { get; set; }
        [DataMember] public int MenuID { get; set; }
        [DataMember] public string MenuText { get; set; }
        [DataMember] public int MainMenuID { get; set; }
        [DataMember] public int MenuOrder { get; set; }
        [DataMember] public string IsActive { get; set; }
        [DataMember] public string FormName { get; set; }
        [DataMember] public bool IsDock { get; set; }
        [DataMember] public string IconName { get; set; }
        [DataMember] public bool IsControl { get; set; }
        [DataMember] public string CodeFonction { get; set; }
        [DataMember] public string Module { get; set; }
        [DataMember] public int IdGroupProgram { get; set; }
        [DataMember] public byte? TypeReedition { get; set; }
        public int? FK_IDGROUPPROGRAM { get; set; }
        [DataMember] public string FK_CODEFONCTION { get; set; }
        [DataMember] public int? FK_IDPROGRAM { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int? PK_ID { get; set; }
        [DataMember] public int? ID { get; set; }

        [DataMember] public int? FK_IDPROFIL { get; set; }

        
    }

}









