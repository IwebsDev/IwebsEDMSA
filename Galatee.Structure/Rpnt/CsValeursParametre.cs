using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
    [DataContract]
    public class CsValeursParametre
    {
        [DataMember]
        public Int32 Parametre_ID { get; set; }
        [DataMember]
        public Int32 TypeValeur { get; set; }
        [DataMember]
        public String LibelleParametre { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Int32? ValeurGlobaleInt { get; set; }
        [DataMember]
        public String ValeurGlobaleChaine { get; set; }
        [DataMember]
        public Decimal? ValeurGlobaleDecimal { get; set; }
        [DataMember]
        public Int32? ValeurInt { get; set; }
        [DataMember]
        public String ValeurChaine { get; set; }
        [DataMember]
        public Decimal? ValeurDec { get; set; }
        [DataMember]
        public String CodeExploitation { get; set; }
        [DataMember]
        public String Explotation_libelle { get; set; }
    }
}
