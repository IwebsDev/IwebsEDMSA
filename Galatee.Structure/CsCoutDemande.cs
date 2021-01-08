using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCoutDemande :CsPrint
    {
           [DataMember] public int PK_ID { get; set; }
           [DataMember] public string CENTRE { get; set; }
           [DataMember] public string PRODUIT { get; set; }
           [DataMember] public string TYPEDEMANDE { get; set; }
           [DataMember] public string COPER { get; set; }
           [DataMember] public string CATEGORIE { get; set; }
           [DataMember] public string REGLAGECOMPTEUR { get; set; }
           [DataMember] public string PUISSANCE { get; set; }
           [DataMember] public string TYPETARIF { get; set; }
           [DataMember] public Nullable<bool> OBLIGATOIRE { get; set; }
           [DataMember] public Nullable<bool> AUTOMATIQUE { get; set; }
           [DataMember] public Nullable<bool> SUBVENTIONNEE { get; set; }
           [DataMember] public Nullable<decimal> MONTANT { get; set; }
           [DataMember] public string TAXE { get; set; }
           [DataMember] public System.DateTime DATECREATION { get; set; }
           [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
           [DataMember] public string USERCREATION { get; set; }
           [DataMember] public string USERMODIFICATION { get; set; }
           [DataMember] public int FK_IDPRODUIT { get; set; }
           [DataMember] public int FK_IDCOPER { get; set; }
           [DataMember] public int FK_IDTYPEDEMANDE { get; set; }
           [DataMember] public int FK_IDCENTRE { get; set; }
           [DataMember] public int FK_IDTAXE { get; set; }
           [DataMember] public Nullable<int> FK_IDTYPETARIF { get; set; }
           [DataMember] public Nullable<int> FK_IDREGLAGECOMPTEUR { get; set; }
           [DataMember] public Nullable<int> FK_IDCATEGORIECLIENT { get; set; }
           [DataMember] public Nullable<int> FK_IDPUISSANCESOUSCRITE { get; set; }

           [DataMember] public string NATURE { get; set; }
           [DataMember] public string LIBELLECOPER { get; set; }
           [DataMember] public string LIBELLEREGLAGECOMPTEUR { get; set; }
           [DataMember] public string LIBELLETAXE { get; set; }
           [DataMember] public string LIBELLEPRODUIT { get; set; }
           [DataMember] public string LIBELLECENTRE { get; set; }
           [DataMember] public string LIBELLETYPEDEMANDE { get; set; }


    
    }
}















