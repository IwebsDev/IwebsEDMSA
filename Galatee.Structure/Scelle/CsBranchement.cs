using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CsBranchement
    {
        public string Branchement_ID { get; set; }
        public int? TypeBranchement_ID { get; set; }
        public string Statut_Branchement { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string NumClient { get; set; }
        public string NomClient { get; set; }
        public string CodeExploitation { get; set; }
        public string CompteurBTA_Numero { get; set; }
        public Guid? Cache_borne_ID { get; set; }
        public Guid? Tableau_bta_ID { get; set; }
        public Guid? CCP_bta_ID { get; set; }
        public Guid? Disjoncteur_bta_ID { get; set; }
        public Guid? Grille_Bta_ID { get; set; }
        public DateTime? GrilleBTA_DateRattachement { get; set; }
        public Guid? ID_GrillePrecedenteDuBranchement { get; set; }
        public decimal? TauxSecurisation { get; set; }
        public string ID_Zone { get; set; }
        public double? PS { get; set; }
        public string Tarif { get; set; }
        public string Numero_Scelle { get; set; }
        public string numScelleCCp { get; set; }
        public string numScelleCapot { get; set; }
        public string numScelleCache1 { get; set; }
        public string numScelleCache2 { get; set; }
        public string numScelleCache3 { get; set; }
        public string numScelleDijoncteur1 { get; set; }
        public string numScelleDijoncteur2 { get; set; }
        public string numScelleDijoncteur3 { get; set; }
        public string numScelleTableau1 { get; set; }
        public string numScelleTableau2 { get; set; }
        public string numScelleTableau3 { get; set; }
        public string numScelleGrille1 { get; set; }
        public string numScelleGrille2 { get; set; }
        public string numScelleGrille3 { get; set; }

    }
}
