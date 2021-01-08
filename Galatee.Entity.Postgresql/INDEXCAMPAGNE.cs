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
    
    public partial class INDEXCAMPAGNE
    {
        public int PK_ID { get; set; }
        public string IDCOUPURE { get; set; }
        public string CENTRE { get; set; }
        public string CLIENT { get; set; }
        public string ORDRE { get; set; }
        public Nullable<decimal> MONTANT { get; set; }
        public Nullable<int> INDEXO { get; set; }
        public Nullable<int> INDEXE { get; set; }
        public string CODEOBSERVATION { get; set; }
        public Nullable<System.DateTime> DATECOUPURE { get; set; }
        public Nullable<System.DateTime> DATERDV { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDCAMPAGNE { get; set; }
        public Nullable<int> FK_IDOBSERVATION { get; set; }
        public int FK_TYPECOUPURE { get; set; }
        public string COMPTEUR { get; set; }
        public int FK_IDCLIENT { get; set; }
    
        public virtual CAMPAGNE CAMPAGNE { get; set; }
        public virtual CENTRE CENTRE1 { get; set; }
        public virtual CLIENT CLIENT1 { get; set; }
        public virtual OBSERVATION OBSERVATION { get; set; }
        public virtual TYPECOUPURE TYPECOUPURE { get; set; }
    }
}