using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsProgramMenu
    {
       [DataMember]
        public int? FK_IDGROUPPROGRAM { get; set; } // MODULE.FK_IDGROUPPROGRAM
       [DataMember]
       public string ProgName { get; set; }
       [DataMember]
       public int? ID { get; set; } // MODULE.FK_ID

       [DataMember]
       public string MENUTEXT { get; set; }

       [DataMember]
       public string ModuleName { get; set; } // MODULE.LIBELLE / CODE

       [DataMember]
       public string CODE { get; set; } //

       [DataMember]
       public string LIBELLE { get; set; } 

       [DataMember]
       public int? MAINMENUID { get; set; }

       [DataMember]
       public int? PK_MENUID { get; set; }

       [DataMember]
       public DateTime? DATECREATION { get; set; }

       [DataMember]
       public string USERCREATION { get; set; }

       [DataMember]
       public DateTime? DATEMODIFICATION { get; set; }

       [DataMember]
       public string USERMODIFICATION { get; set; }

       [DataMember]
       public string FK_CODEFONCTION { get; set; }
       [DataMember]
       public int? FK_IDPROGRAM { get; set; }

    }

}









