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
    
    public partial class ADMINISTRATION_INSTITUT
    {
        public int PK_ID { get; set; }
        public string NOMMANDATAIRE { get; set; }
        public string PRENOMMANDATAIRE { get; set; }
        public string RANGMANDATAIRE { get; set; }
        public string NOMSIGNATAIRE { get; set; }
        public string PRENOMSIGNATAIRE { get; set; }
        public string RANGSIGNATAIRE { get; set; }
        public Nullable<int> FK_IDCLIENT { get; set; }
        public string NOMABON { get; set; }
    
        public virtual CLIENT CLIENT { get; set; }
    }
}
