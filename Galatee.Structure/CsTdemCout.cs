using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTdemCout
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string TDEM { get; set; }

        [DataMember]
        public string COPER1 { get; set; }
        [DataMember]
        public string OBLI1 { get; set; }
        [DataMember]
        public string AUTO1 { get; set; }
        [DataMember]
        public Decimal? MONTANT1 { get; set; }
        [DataMember]
        public string TAXE1 { get; set; }

        [DataMember]
        public string COPER5 { get; set; }
        [DataMember]
        public string OBLI5 { get; set; }
        [DataMember]
        public string AUTO5 { get; set; }
        [DataMember]
        public Decimal? MONTANT5 { get; set; }
        [DataMember]
        public string TAXE5 { get; set; }

        [DataMember]
        public string COPER4 { get; set; }
        [DataMember]
        public string OBLI4 { get; set; }
        [DataMember]
        public string AUTO4 { get; set; }
        [DataMember]
        public Decimal? MONTANT4 { get; set; }
        [DataMember]
        public string TAXE4 { get; set; }

        [DataMember]
        public string COPER2 { get; set; }
        [DataMember]
        public string OBLI2 { get; set; }
        [DataMember]
        public string AUTO2 { get; set; }
        [DataMember]
        public Decimal? MONTANT2 { get; set; }
        [DataMember]
        public string TAXE2 { get; set; }

        [DataMember]
        public string COPER3 { get; set; }
        [DataMember]
        public string OBLI3 { get; set; }
        [DataMember]
        public string AUTO3 { get; set; }
        [DataMember]
        public Decimal? MONTANT3 { get; set; }
        [DataMember]
        public string TAXE3 { get; set; }

        [DataMember]
        public DateTime? DMAJ { get; set; }

        [DataMember]
        public string TRANS { get; set; }

        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public byte[] ROWID { get; set; }
    }
}
