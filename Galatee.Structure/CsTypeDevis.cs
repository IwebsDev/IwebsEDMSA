﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    public class CsTypeDevis : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public int? FK_IDPRODUIT { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public int? FK_IDTDEM { get; set; }
        [DataMember] public string TDEM { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }

        public override string ToString()
        {
            return this.LIBELLE;
        }
    }
}
