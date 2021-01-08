using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    public class CstbElementsLotDeControleBTA
    {
        [DataMember]
        public System.Guid Lot_ID { get; set; }
        [DataMember]
        public string Contrat_ID { get; set; }
        [DataMember]
        public System.DateTime DateSelection { get; set; }
        [DataMember]
        public System.DateTime DateAffectationLot { get; set; }
        [DataMember]
        public Nullable<System.Guid> ResultatControle_ID { get; set; }
        [DataMember]
        public int Methode_ID { get; set; }
        [DataMember]
        public int Debut_PerAA { get; set; }
        [DataMember]
        public int Debut_PerMM { get; set; }
        [DataMember]
        public int Fin_PerAA { get; set; }
        [DataMember]
        public int Fin_PerMM { get; set; }
        [DataMember]
        public double Difference { get; set; }
        [DataMember]
        public Nullable<double> C_MChuteConso_ConsoEnChute_Valeur { get; set; }
        [DataMember]
        public Nullable<int> C_MChuteConso_ConsoEnChute_PerAA { get; set; }
        [DataMember]
        public Nullable<int> C_MChuteConso_ConsoEnChute_PerMM { get; set; }
        [DataMember]
        public Nullable<int> C_MChuteConso_ConsoPrecedente_PerAA { get; set; }
        [DataMember]
        public Nullable<int> C_MChuteConso_ConsoPrecedente_PerMM { get; set; }
        [DataMember]
        public Nullable<double> C_MConsoFaible_ConsoJournaliereAttendue { get; set; }
        [DataMember]
        public Nullable<double> C_MConsoFaible_ConsommationFacturee { get; set; }
        [DataMember]
        public Nullable<int> C_MConsoFaible_NbreJoursDeConso { get; set; }
    }
}
