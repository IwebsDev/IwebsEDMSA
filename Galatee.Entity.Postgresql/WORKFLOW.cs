//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galatee.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WORKFLOW
    {
        public WORKFLOW()
        {
            this.RWORFKLOWCENTRE = new HashSet<RWORFKLOWCENTRE>();
        }
    
        public System.Guid PK_ID { get; set; }
        public string WORKFLOWNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CODE { get; set; }
        public Nullable<int> FK_IDTABLE_TRAVAIL { get; set; }
    
        public virtual ICollection<RWORFKLOWCENTRE> RWORFKLOWCENTRE { get; set; }
    }
}
