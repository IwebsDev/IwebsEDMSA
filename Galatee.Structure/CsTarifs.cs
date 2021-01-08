using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsTarif : CsPrint
    {
        [DataMember] public string CODE { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string TRANS { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public bool ESSUPPRIMER { get; set; }

        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }

        [DataMember] public string REGLAGECOMPTEUR  { get; set; }
        [DataMember] public string LIBELLEREGLAGECOMPTEUR  { get; set; }

        [DataMember] public string CATEGORIE { get; set; }
        [DataMember] public string LIBELLECATEGORIE { get; set; }
        [DataMember] public int FK_IDCATEGORIE { get; set; }

        [DataMember] public int FK_IDREGLAGECOMPTEUR { get; set; }
        [DataMember] public int FK_IDTYPETARIF { get; set; }

        [DataMember] public string PUISSANCE { get; set; }
        [DataMember] public decimal  VALEUR { get; set; }
        [DataMember] public int FK_IDPUISSANCE { get; set; }



        




    }

}
