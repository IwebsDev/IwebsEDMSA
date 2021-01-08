using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFactureClientRegrouper : CsPrint
    {
        #region Liste des champs provenants de EnteteFacture
       [DataMember]  public string NUMEROFACTURE { get; set; }
       [DataMember]  public string REGROUPEMENT { get; set; }
       [DataMember]  public string LIBELLEREGROUPEMENT { get; set; }
       [DataMember]  public string PERIODEFACTURATION { get; set; }
       [DataMember]  public string NOMBRECLIENT { get; set; }
       [DataMember]  public int QUANTITE { get; set; }
       [DataMember]  public int REDEVANCECONSOMMATION { get; set; }
        #endregion
    }
} 