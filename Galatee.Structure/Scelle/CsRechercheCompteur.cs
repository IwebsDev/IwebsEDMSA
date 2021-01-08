using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsRechercheCompteur
    {
            [DataMember]
            public string Numero_Compteur{ get; set; }
            [DataMember]
            public Nullable<int> Type_Compteur_ID{ get; set; }
            [DataMember]
            public string StatutCompteur { get; set; }
            [DataMember]
            public Nullable<int> EtatCompteur_ID { get; set; }

            [DataMember]
            public string CapotMoteur_Numero_Scelle1 { get; set; }


            [DataMember]
            public Nullable<int> CapotMoteur_Couleur_Scelle1 { get; set; }

            [DataMember]
            public string CapotMoteur_Numero_Scelle2 { get; set; }

            [DataMember]
            public Nullable<int> CapotMoteur_Couleur_Scelle2 { get; set; }

            [DataMember]
            public Nullable<int> CapotMoteur_Numero_Scelle3 { get; set; }

            [DataMember]
            public Nullable<int> CapotMoteur_Couleur_Scelle3 { get; set; }

            [DataMember]
            public string Branchement_ID { get; set; }

            [DataMember]
            public string TYPE_COMPTEUR {get;set;}
            [DataMember]
            public string DIAMETRE{get;set;}
            [DataMember]
            public string MARQUE {get;set;}
            [DataMember]
            public Nullable<byte> CADRAN {get;set;}
            [DataMember]
            public string ANNEEFAB { get; set; }
            [DataMember]
            public string FONCTIONNEMENT { get; set; }
            [DataMember]
            private Nullable<System.DateTime> CapotMoteur_DateDePoseScelle3 { get; set; }
            [DataMember]
            public int CodeCentre { get; set; }
            [DataMember]
            public Nullable<System.Guid> Cache_Scelle { get; set; }
            [DataMember]
            public Nullable<int> FK_IDDIAMETRECOMPTEUR { get; set; }
            [DataMember]
            public Nullable<int> FK_IDMARQUECOMPTEUR { get; set; }
            [DataMember]
            public Nullable<int> FK_IDTYPECOMPTEUR { get; set; }
            [DataMember]
            public Nullable<int> Cache_Couleur_Scelle4 { get; set; }
            [DataMember]
            public Nullable<Guid> Cache_Scelle_ID { get; set; }
            [DataMember]
            public string Cache_Scelle_Numero_Scelle4 { get; set; }
            [DataMember]
            public Nullable<DateTime> Cache_sceller_DateDePoseScelle4 { get; set; }
            [DataMember]
            public string USERCREATION { get; set; }

            [DataMember]
            public Nullable<System.DateTime> DATECREATION { get; set; }

            [DataMember]
            public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
         
            [DataMember]
            public string USERMODIFICATION { get; set; }
      
      
    }
}
