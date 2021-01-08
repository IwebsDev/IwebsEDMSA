using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsMontantParCoper : CsPrint
    {
        [DataMember]
        public string PK_FK_COPER { get; set; }
        [DataMember]
        public string PK_FK_NATURE { get; set; }
        [DataMember]
        public string  LIBELLE1 { get; set; }
        [DataMember]
        public decimal? MONTANT1 { get; set; }
        [DataMember]
        public string FK_TAXE1 { get; set; }
        [DataMember]
        public string  LIBELLETAXE { get; set; }
        [DataMember]
        public decimal? MONTANTTAXE { get; set; }
    }
 }









