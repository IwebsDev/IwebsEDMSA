using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSocietePrive
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string NUMEROREGISTRECOMMERCE { get; set; }
        [DataMember] public Nullable<decimal> CAPITAL { get; set; }
        [DataMember] public string IDENTIFICATIONFISCALE { get; set; }
        [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }
        [DataMember] public string SIEGE { get; set; }
        [DataMember] public Nullable<int> FK_IDCLIENT { get; set; }
        [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
        [DataMember] public string NOMMANDATAIRE { get; set; }
        [DataMember] public string PRENOMMANDATAIRE { get; set; }
        [DataMember] public string RANGMANDATAIRE { get; set; }
        [DataMember] public string NOMSIGNATAIRE { get; set; }
        [DataMember] public string PRENOMSIGNATAIRE { get; set; }
        [DataMember] public string RANGSIGNATAIRE { get; set; }
        [DataMember] public Nullable<int> FK_IDSTATUTJURIQUE { get; set; }
        [DataMember] public CsStatutJuridique STATUTJURIQUE { get; set; }
        [DataMember] public string NOMABON { get; set; }

        [DataMember] public string LIBELLESTATUSJURIDIQUE { get; set; }

        
    }
}
