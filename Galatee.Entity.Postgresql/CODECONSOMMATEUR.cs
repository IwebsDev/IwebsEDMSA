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
    
    public partial class CODECONSOMMATEUR
    {
        public CODECONSOMMATEUR()
        {
            this.CLIENT = new HashSet<CLIENT>();
            this.DCLIENT = new HashSet<DCLIENT>();
        }
    
        public string CODE { get; set; }
        public string LIBELLE { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
    
        public virtual ICollection<CLIENT> CLIENT { get; set; }
        public virtual ICollection<DCLIENT> DCLIENT { get; set; }
    }
}
