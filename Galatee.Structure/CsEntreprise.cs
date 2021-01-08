using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsEntreprise
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string NOM { get; set; }
        [DataMember]
        public string SIGLE { get; set; }
        [DataMember]
        public string SLOGAN { get; set; }
        [DataMember]
        public string ADRESSEPRINCIPALE { get; set; }
        [DataMember]
        public string ADRESSESECONDAIRE { get; set; }
        [DataMember]
        public string TELEPHONEPRINCIPAL { get; set; }
        [DataMember]
        public string TELEPHONESECONDAIRE { get; set; }
        [DataMember]
        public string FAXPRINCIPALE { get; set; }
        [DataMember]
        public string FAXSECONDAIRE { get; set; }
        [DataMember]
        public string EMAILPRINCIPALE { get; set; }
        [DataMember]
        public string EMAILSECONDAIRE { get; set; }
        [DataMember]
        public string ACTIVITE { get; set; }
        [DataMember]
        public string PAYS { get; set; }
        [DataMember]
        public string SITEINTERNET { get; set; }
        [DataMember]
        public byte[] LOGO { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}









