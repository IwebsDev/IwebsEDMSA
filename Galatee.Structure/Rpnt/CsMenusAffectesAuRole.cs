using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsMenusAffectesAuRole
    {
		
		
[DataMember]
		public Guid RoleID { get; set; }
[DataMember]
		public Int32 MenuID { get; set; }
[DataMember]
		public String MenuText { get; set; }
[DataMember]
		public Int32? MainMenuID { get; set; }
[DataMember]
		public Int32 MenuOrder { get; set; }
[DataMember]
		public Boolean IsActive { get; set; }
[DataMember]
		public String FormName { get; set; }
[DataMember]
		public Boolean? IsDock { get; set; }
[DataMember]
		public String IconName { get; set; }
[DataMember]
		public String SousControl { get; set; }


    }
}
