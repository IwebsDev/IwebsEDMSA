using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDesktopGroup
    {
       
        [DataMember] public int? ID { get; set; }
        [DataMember] public string CodeModule { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public int ImageIndex { get; set; }
        [DataMember] public List<CsDesktopItem> SubItems { get; set; }
        [DataMember] public CsProfil ProfilDesktopItem { get; set; }
        [DataMember] public List<string> LstModuleIdProfil  { get; set; }
    
    }
}
