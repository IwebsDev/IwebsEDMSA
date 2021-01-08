using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsComptable : CsPrint
    {
        [DataMember]
        public string MOISCOMPTA { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string NATURE { get; set; }
        [DataMember]
        public string COMMUNE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string REDEVANCE { get; set; }
        [DataMember]
        public string CTAX { get; set; }
        [DataMember]
        public string CODEJOURNAL { get; set; }
        [DataMember]
        public decimal? REDHT { get; set; }
        [DataMember]
        public decimal? REDTAXE { get; set; }
    }
 }









