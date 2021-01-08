using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAdmRoles
    {
        //public enum AdmRolesEnum {Administration = 1}
         [DataMember]
        public string Centre { get; set; }
         [DataMember]
        public Guid RoleID { get; set; }
         [DataMember]
        public string CodeFonc { get; set; }
         [DataMember]
        public string RoleName { get; set; }
         [DataMember]
        public string RoleDisplayName { get; set; }
    }

}
