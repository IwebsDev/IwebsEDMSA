using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsLafactureClient:CsPrint 
    {
        [DataMember] public CsEnteteFacture  _LeEntatfac { get; set; } 
        [DataMember]  public List<CsRedevanceFacture> _LstRedFact { get; set; }
        [DataMember]  public List<CsProduitFacture > _LstProfact{ get; set; }

    }
}