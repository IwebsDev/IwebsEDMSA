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
    
    public partial class RefStatusLotScelle
    {
        public RefStatusLotScelle()
        {
            this.tbLot = new HashSet<tbLot>();
        }
    
        public int Status_ID { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<tbLot> tbLot { get; set; }
    }
}