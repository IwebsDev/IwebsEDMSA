using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEditionFactureFraude:CsPrint
    {

         [DataMember] public string Centre { get; set; }
         [DataMember] public string Client { get; set; }
         [DataMember] public string Ordre { get; set; }
         [DataMember] public string IdentificationUnique { get; set; } 
         [DataMember] public string Nomabon { get; set; }
         [DataMember] public string Email { get; set; }
         [DataMember] public string Telephone { get; set; }
         [DataMember] public string Commune { get; set; }
         [DataMember] public string Quartier { get; set; }
         [DataMember] public string Rue { get; set; }
         [DataMember] public string Porte { get; set; }
         [DataMember] public string Tournee { get; set; }
         [DataMember] public string OrdreTournee { get; set; }


        [DataMember] public int ConsommationEstimee { get; set; }
        [DataMember] public int ConsommationRetrogradation { get; set; }
        [DataMember] public int ConsommationDejaFacturee { get; set; }
        [DataMember] public int ConsommationAFacturer { get; set; }
        [DataMember] public int ConsommationMensuelleAFacturer { get; set; }

        [DataMember] public string FicheTraitement { get; set; }
        [DataMember] public string LIBELLESOURCECONTROLE { get; set; }
        [DataMember] public System.DateTime DateControle { get; set; }
        [DataMember] public string FicheControle { get; set; }

        [DataMember]  public Nullable<int> IndexCompteur { get; set; }

        /** Grille **/
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string Libelle { get; set; }
        [DataMember] public decimal PrixUnitaire { get; set; }
        [DataMember] public Nullable<bool> EstModifiable { get; set; }
        [DataMember] public int MontantHT { get; set; }
        [DataMember] public int MontantTva { get; set; }
        [DataMember] public int MontantTTC { get; set; }
        [DataMember] public int? Quantite { get; set; }
        [DataMember] public string TypeEtat { get; set; }
    
    }
}
