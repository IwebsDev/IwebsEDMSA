using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    public class CstbLotsDeControleBTA
    {
        [DataMember]
        public System.Guid Lot_ID { get; set; }
        [DataMember]
        public string Libelle_Lot { get; set; }
        [DataMember]
        public int StatutLot_ID { get; set; }
        [DataMember]
        public System.Guid Campagne_ID { get; set; }
        [DataMember]
        public System.DateTime DateCreation { get; set; }
        [DataMember]
        public string MatriculeAgentCreation { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateFermeture { get; set; }
        [DataMember]
        public string MatriculeAgentControleur { get; set; }
        [DataMember]
        public int NbreElementsDuLot { get; set; }
        [DataMember]
        public Nullable<int> Critere_TypeClient { get; set; }
        [DataMember]
        public Nullable<int> Critere_GroupeDeFacturation { get; set; }
        [DataMember]
        public string Critere_TypeTarif { get; set; }
        [DataMember]
        public Nullable<int> Critere_TypeCompteur { get; set; }
        [DataMember]
        public Nullable<int> Critere_IdTournee { get; set; }



        [DataMember]
        public List<Galatee.Structure.Rpnt.CstbElementsLotDeControleBTA> ListElementLot { get; set; }
    }
}
