using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCompteClient : CsPrint
    {
        [DataMember] public List<CsLclient > LstReglement { get; set; }
        [DataMember] public List<CsLclient> LstFacture { get; set; }
        [DataMember] public List<CsLclient> ToutLClient { get; set; }
        [DataMember] public List<CsLclient> Impayes { get; set; }
        [DataMember] public string Ordre { get; set; }
    }
 }









