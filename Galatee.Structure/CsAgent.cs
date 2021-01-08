using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsAgent
    {
       [DataMember] public bool   ACTIF         { get; set; }
       [DataMember] public string MATRICULE     { get; set; }
       [DataMember] public string NOM           { get; set; }
       [DataMember] public string PRENOM        { get; set; }
       [DataMember] public string COMPTEWINDOWS { get; set; }
       [DataMember] public int PK_IDAGENT       { get; set; }

       [DataMember] public string IDENTITE      { get; set; }

    }
}









