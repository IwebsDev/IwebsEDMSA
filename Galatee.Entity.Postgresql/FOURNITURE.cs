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
    
    public partial class FOURNITURE
    {
        public int FK_IDTYPEDEMANDE { get; set; }
        public string REGLAGECOMPTEUR { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<bool> ISSUMMARY { get; set; }
        public Nullable<bool> ISADDITIONAL { get; set; }
        public Nullable<bool> ISEXTENSION { get; set; }
        public Nullable<bool> ISDEFAULT { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
        public int FK_IDPRODUIT { get; set; }
        public Nullable<int> FK_IDMATERIELDEVIS { get; set; }
    
        public virtual MATERIELDEVIS MATERIELDEVIS { get; set; }
        public virtual PRODUIT PRODUIT { get; set; }
        public virtual TYPEDEMANDE TYPEDEMANDE { get; set; }
    }
}