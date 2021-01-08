using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsDemandeReversement : CsPrint
    {
     [DataMember] public int PK_ID { get; set; }
     [DataMember] public int FK_IDHABILCAISSE { get; set; }
     [DataMember] public Nullable<int> STATUT { get; set; }
     [DataMember] public string RAISONDEMANDE { get; set; }
     [DataMember] public string RAISONREJET { get; set; }
     [DataMember] public string USERCREATION { get; set; }
     [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }
     [DataMember] public string USERMODIFICATION { get; set; }
     [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
     [DataMember] public bool  IsSELECT { get; set; }
     [DataMember] public string  NOMCAISSE { get; set; }
     [DataMember] public Nullable<System.DateTime> DATECAISSE { get; set; }
     [DataMember] public string MATRICULE { get; set; }


    }
}









