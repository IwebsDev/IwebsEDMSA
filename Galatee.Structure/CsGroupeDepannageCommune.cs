using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsGroupeDepannageCommune
    {
       [DataMember] public Nullable<System.Guid> FK_IDGROUPEVALIDATION { get; set; }
       [DataMember] public string CODECOMMUNE { get; set; }
       [DataMember] public int PK_ID { get; set; }
    }

}









