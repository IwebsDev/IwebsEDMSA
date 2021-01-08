using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsLibelle
    {
        #region Ag
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string CODE { get; set; }
        #endregion
        #region Administration
        [DataMember]
        public string Matricule { get; set; }
        [DataMember]
        public string PK_CODE { get; set; }
        [DataMember]
        public string ROLENAME { get; set; }
        [DataMember]
        public string PK_CODESITE { get; set; }
        [DataMember]
        public string CODESITE { get; set; }
        public int statusPerimetreAction { get; set; }
        public string fk_Centre { get; set; }
        public string fk_Site { get; set; }
        #endregion
    }

}









