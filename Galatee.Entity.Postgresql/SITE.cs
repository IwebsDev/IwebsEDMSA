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
    
    public partial class SITE
    {
        public SITE()
        {
            this.CENTRE = new HashSet<CENTRE>();
            this.ORIGINELOT = new HashSet<ORIGINELOT>();
            this.CLIENTFRAUDE = new HashSet<CLIENTFRAUDE>();
        }
    
        public string CODE { get; set; }
        public string LIBELLE { get; set; }
        public string SERVEUR { get; set; }
        public string USERID { get; set; }
        public string PWD { get; set; }
        public string CATALOGUE { get; set; }
        public string NUMERODEMANDE { get; set; }
        public string NUMEROFACTURE { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
    
        public virtual ICollection<CENTRE> CENTRE { get; set; }
        public virtual ICollection<ORIGINELOT> ORIGINELOT { get; set; }
        public virtual ICollection<CLIENTFRAUDE> CLIENTFRAUDE { get; set; }
    }
}
