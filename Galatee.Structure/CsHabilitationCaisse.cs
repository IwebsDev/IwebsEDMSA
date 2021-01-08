using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsHabilitationCaisse:CsPrint
    {
       [DataMember] public string NUMCAISSE { get; set; }
       [DataMember] public string MATRICULE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATE_DEBUT { get; set; }
       [DataMember] public Nullable<System.DateTime> DATE_FIN { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string POSTE { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public Nullable<decimal> FONDCAISSE { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDCAISSE { get; set; }
       [DataMember] public int FK_IDCAISSIERE { get; set; }
       [DataMember] public Nullable<decimal> MONTANTENCAISSE { get; set; }
       [DataMember] public Nullable<decimal> MONTANTREVERSER { get; set; }
       [DataMember] public Nullable<decimal> ECART { get; set; }
       [DataMember] public string NOMCAISSE { get; set; }
       [DataMember] public string SITECAISSE { get; set; }
       [DataMember] public string AGENCECAISSE { get; set; }
       [DataMember] public bool  ESTATRIBUER { get; set; }
       [DataMember] public bool IsDEMANDEREVERSEMENT { get; set; }
       [DataMember] public string ACQUIT { get; set; }
       [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }

       [DataMember] public bool IsCAISSECOURANTE { get; set; }


    }
}









