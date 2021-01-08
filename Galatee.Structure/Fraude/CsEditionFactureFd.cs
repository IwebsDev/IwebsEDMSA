using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Galatee.Structure
{
    public class CsEditionFactureFd : CsPrint 
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
         [DataMember] public string Mois { get; set; }
         [DataMember] public string Puissance { get; set; }
         [DataMember] public string Facture { get; set; }
         [DataMember] public string DateLimite { get; set; }
         [DataMember] public string Produit { get; set; }
         [DataMember] public string Usage { get; set; }
         [DataMember] public string Source { get; set; }
         [DataMember] public string DateControle { get; set; }
         [DataMember] public string Pv_Controle { get; set; }
         [DataMember] public string NumeroCompteur { get; set; }
         [DataMember] public string Calibre { get; set; }
         [DataMember] public string Index { get; set; }
         [DataMember] public string AnnomalieCompteur { get; set; }
         [DataMember] public string AnnomalieBranchement { get; set; }
         [DataMember] public string AutreAnnomalie { get; set; }
         [DataMember] public string Duree { get; set; }
         [DataMember] public string Datefacture { get; set; }
         [DataMember] public string DateExigibilite { get; set; }
         [DataMember] public string TypedeFacture { get; set; }
         [DataMember] public int ConsommationEstimee { get; set; }
         [DataMember] public int ConsommationRetrogradation { get; set; }
         [DataMember] public int ConsommationDejaFacturee { get; set; }
         [DataMember] public int ConsommationAFacturer { get; set; }
         [DataMember] public int ConsommationMensuelleAFacturer { get; set; }
         [DataMember] public string FicheTraitement { get; set; }
         [DataMember] public string FicheControle { get; set; }
         [DataMember] public string MontantLettre{ get; set; }
         [DataMember] public decimal  MontantTimbre{ get; set; }
         [DataMember] public decimal  MontantTotal{ get; set; }
         [DataMember] public decimal  MontantTotalConso{ get; set; }


        /** Grille **/
        [DataMember] public string Libelle { get; set; }
        [DataMember] public decimal PrixUnitaire { get; set; }
        [DataMember] public int MontantHT { get; set; }
        [DataMember] public int MontantTva { get; set; }
        [DataMember] public int MontantTTC { get; set; }
        [DataMember] public int? Quantite { get; set; }
        [DataMember] public string TypeEtat { get; set; }
    

    }
}
