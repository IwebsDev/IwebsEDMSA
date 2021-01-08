using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEnvoiMail : CsPrint
    {
        [DataMember] public string centre { get; set; }
        [DataMember] public string client { get; set; }
        [DataMember] public string ordre { get; set; }
        [DataMember] public string NomClient { get; set; }
        [DataMember] public string periode { get; set; }
        [DataMember] public string numfact { get; set; }
        [DataMember] public string Email { get; set; }
        [DataMember] public decimal montant { get; set; }
        [DataMember] public string Telephone { get; set; }

    }
}
