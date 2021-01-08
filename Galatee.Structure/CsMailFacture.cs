using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMailFacture : CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string AddresseDeFacturation { get; set; }
        [DataMember]
        public string NomClient { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string FACTURE { get; set; }
        [DataMember]
        public string TRANCHE { get; set; }
        [DataMember]
        public string LIBELLEREDEVANCE { get; set; }
        [DataMember]
        public int QUANTITE { get; set; }
        [DataMember]
        public decimal BARPRIX { get; set; }
        [DataMember]
        public decimal REDHT { get; set; }
        [DataMember]
        public decimal REDTAXE { get; set; }
        [DataMember]
        public string IDENTITE { get; set; }
        [DataMember]
        public string ADRESSE { get; set; }
        [DataMember]
        public string NUMCOMPT { get; set; }
        [DataMember]
        public string DIAMETRE { get; set; }
        [DataMember]
        public string ETATFACTURE { get; set; }
        [DataMember]
        public int ANCINDEX { get; set; }
        [DataMember]
        public int NOUVINDEX { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string numfact { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}
