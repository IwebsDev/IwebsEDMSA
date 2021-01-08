using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHabilitationUser
    {
		
		
[DataMember]
		public String LoginName { get; set; }
[DataMember]
		public Guid RoleID { get; set; }
[DataMember]
		public String RoleName { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String LibelleUO { get; set; }
[DataMember]
		public DateTime Date_debut_validite { get; set; }
[DataMember]
		public DateTime? Date_fin_validite { get; set; }
[DataMember]
		public String RoleDisplayName { get; set; }
[DataMember]
		public String MatriculeAgent { get; set; }


    }
}
