using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCritereLot : CsPrint
    {
        [DataMember] public CsCentre LeCentre { get; set; }
        [DataMember] public List<CsProduit> LesProduit { get; set; }
        [DataMember] public List<CsCategorieClient> LesCategorie { get; set; }
        [DataMember] public List<CsPeriodiciteFacturation> LesPeriodicite { get; set; }
        [DataMember] public List<CsTournee > LesTournees { get; set; }
        [DataMember] public List<CsReleveur  > LesReleveur { get; set; }
    }
 }









