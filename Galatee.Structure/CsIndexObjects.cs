using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsIndexObjects
    {
        [DataMember]
        public List<CsLotri> LotIndex { get; set; }

        [DataMember]
        public List<string> Categories { get; set; }

        [DataMember]
        public string Centre { get; set; }

        [DataMember]
        public string AgentPia { get; set; }

        [DataMember]
        public string MatriculeDebut { get; set; }

        [DataMember]
        public string MatriculeFin { get; set; }

        [DataMember]
        public DateTime? Exigible { get; set; }

        [DataMember]
        public int? TotalClient { get; set; }

        [DataMember]
        public int? TotalFacture { get; set; }

        [DataMember]
        public int? IdCoupure { get; set; }

        [DataMember]
        public bool? ClientResilie { get; set; }

        [DataMember]
        public bool? ClientGroupe { get; set; }

        [DataMember]
        public decimal? MontantPeriode { get; set; }

        [DataMember]
        public decimal? SoldeClient { get; set; }

        [DataMember]
        public string Periode { get; set; }

        [DataMember]
        public List<CsCentre> Centres { get; set; }

    }
}
