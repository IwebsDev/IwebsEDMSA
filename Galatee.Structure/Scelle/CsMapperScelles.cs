using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsMapperScelles
    {
        public Nullable<int> Status_ID { get; set; }
        public System.DateTime Date_creation_scelle { get; set; }
        public Nullable<int> Origine_scelle { get; set; }
        public Nullable<int> provenance_Scelle_ID { get; set; }
        public string lot_ID { get; set; }
        public Nullable<int> Couleur_Scelle { get; set; }
        public Nullable<System.DateTime> DateDePose { get; set; }
        public Nullable<System.DateTime> DateDeRupture { get; set; }
        public Nullable<int> TypeOrganeScelle { get; set; }
        public string Reference_OrganeOuBranchementOuRaccordement_Scelle { get; set; }
        public System.Guid Id_Scelle { get; set; }
        public string Numero_Scelle { get; set; }
        public Nullable<int> agence_centre_Origine { get; set; }
        public Nullable<int> agence_centre_Appartenance { get; set; }
        public Nullable<int> CodeCentre { get; set; }
    }
}
