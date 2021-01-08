using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsPrecontentieuxDechargement : CsPrint 
    {
     [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }
     [DataMember] public string CENTRE { get; set; }
     [DataMember] public string CLIENT { get; set; }
     [DataMember] public string ORDRE { get; set; }
     [DataMember] public string NUMCOMPTEUR { get; set; }
     [DataMember] public Nullable<bool> DECEDEAVECAYANTDROIT { get; set; }
     [DataMember] public Nullable<bool> DECEDESANSAYANTDROIT { get; set; }
     [DataMember] public Nullable<bool> LOCATAIRE { get; set; }
     [DataMember] public Nullable<bool> ABSENTAVECCONTACT { get; set; }
     [DataMember] public Nullable<bool> PROPRIETAIRE { get; set; }
     [DataMember] public Nullable<bool> POINTINTROUVABLE { get; set; }
     [DataMember] public Nullable<bool> CONSOMATEURINTROUVABLE { get; set; }
     [DataMember] public Nullable<bool> INVITATIONDEPOSE { get; set; }
     [DataMember] public Nullable<int> FK_IDCLIENT { get; set; }
     [DataMember] public int PK_ID { get; set; }
     [DataMember] public List<CsPrecontentieuxAutreClient> ListAutreClient { get; set; }

    }
}









