using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsProduit : CsPrint
    {
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int NBREDEPOINT  { get; set; }
        [DataMember] public int GESTIONTRANSFO { get; set; }
        [DataMember] public int MODESAISIE { get; set; }
        [DataMember] public string OriginalCODE { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public DateTime DATECREATION { get; set; }

        
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public bool IsNewRow { get; set; }
        [DataMember] public bool IsSelect { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }

        [DataMember] public DateTime? DATEFIN { get; set; }



        	
    }
    [Serializable]
    public class ProduitCollection : List<CsProduit>
    {
    }

}









