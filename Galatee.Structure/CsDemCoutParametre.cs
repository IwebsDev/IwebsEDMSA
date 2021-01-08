using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDemCoutParametre
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string TDEM { get; set; }
        [DataMember]
        public string CATEGORIE { get; set; }
        [DataMember]
        public string COPER { get; set; }
        [DataMember]
        public string NATURE { get; set; }
        [DataMember]
        public string OBLIG { get; set; }
        [DataMember]
        public string AUTO { get; set; }
        [DataMember]
        public decimal? MONTANT { get; set; }
        [DataMember]
        public string TAXE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string LIBELLETAXE { get; set; }
        [DataMember]
        public decimal? MONTANTTAXE { get; set; }
    }
}















