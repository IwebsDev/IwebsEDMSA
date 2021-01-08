using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsPayeur
    {
               [DataMember] public Nullable<int> PAYEUR1 { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public string TELEPHONE { get; set; }
        [DataMember] public string FAX { get; set; }
        [DataMember] public string EMAIL { get; set; }
        [DataMember] public Nullable<short> ACCEPTFACTURE { get; set; }
        [DataMember] public Nullable<short> ACCEPTPENALITE { get; set; }
        [DataMember] public string REGISTRE { get; set; }
        [DataMember] public string CODENATIONAL { get; set; }
        [DataMember] public string ACTIVITE { get; set; }
        [DataMember] public string DISTRIBUTION { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string COMMUNE { get; set; }
        [DataMember] public string QUARTIER { get; set; }
        [DataMember] public string CODERUE { get; set; }
        [DataMember] public string NUMRUE { get; set; }
        [DataMember] public string NOMRUE { get; set; }
        [DataMember] public string COMPRUE { get; set; }
        [DataMember] public string CPOS { get; set; }
        [DataMember] public string CADR { get; set; }
        [DataMember] public int PK_ID { get; set; }
    }
}









