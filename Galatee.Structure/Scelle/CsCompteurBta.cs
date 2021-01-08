using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
  public  class CsCompteurBta:CsPrint 
    {
         
        [DataMember]  public string Numero_Compteur { get; set; }
        [DataMember]  public int Type_Compteur_ID { get; set; }
        [DataMember]  public string Type_Compteur { get; set; }
        [DataMember]  public string StatutCompteur { get; set; }
        [DataMember]  public Nullable<int> EtatCompteur_ID { get; set; }
        [DataMember]  public Nullable<Guid> CapotMoteur_ID_Scelle1 { get; set; }
        [DataMember]  public Nullable<Guid> CapotMoteur_ID_Scelle2 { get; set; }
        [DataMember]  public Nullable<Guid> CapotMoteur_ID_Scelle3 { get; set; }
        [DataMember]  public Nullable<Guid> Cache_Scelle { get; set; }
        [DataMember]  public string MARQUE { get; set; }
        [DataMember]  public string TYPECOMPTEUR { get; set; }
        [DataMember]  public int PK_ID { get; set; }

        [DataMember] public Nullable<byte> CADRAN { get; set; }
        [DataMember] public string ANNEEFAB { get; set; }
        [DataMember] public string FONCTIONNEMENT { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public Nullable<int> FK_IDTYPECOMPTEUR { get; set; }
        [DataMember] public int FK_IDMARQUECOMPTEUR { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public Nullable<int> FK_IDCALIBRECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDREGLAGECOMPTEUR { get; set; }

         
        [DataMember] public string Numero_ScelleCapot_1 { get; set; }
        [DataMember] public string Numero_ScelleCapot_2 { get; set; }
        [DataMember] public string Numero_ScelleCapot_3 { get; set; }
        [DataMember] public string Numero_Cache_3 { get; set; }

        [DataMember] public string CALIBRE { get; set; }

        [DataMember]  public string NUMERO { get; set; }
        [DataMember]  public string ANCNUMEROCRT { get; set; }
        [DataMember]  public Nullable<Guid> AncCapotMoteur_ID_Scelle1 { get; set; }
        [DataMember]  public Nullable<Guid> AncCapotMoteur_ID_Scelle2 { get; set; }
        [DataMember]  public Nullable<Guid> AncCapotMoteur_ID_Scelle3 { get; set; }
        [DataMember]  public Nullable<Guid> AncCache_Scelle { get; set; }



        [DataMember]  public string ETAT { get; set; }
        [DataMember]  public string CODEPRODUIT { get; set; }
        [DataMember]  public string LIBELLEMARQUE { get; set; }
        [DataMember]  public string LIBELLEPRODUIT { get; set; }
        [DataMember]  public string LIBELLEETATCOMPTEUR { get; set; }
        [DataMember]  public string LIBELLETYPECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }

         
     }
         
}
