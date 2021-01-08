using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class aCampagne : CsPrint
    {
         [DataMember]
        public Int64 Id { get; set; }
         [DataMember]
        public string IdCoupure { get; set; }
             [DataMember]
         public string COMPTEUR { get; set; }
         [DataMember]
        public string Centre { get; set; }
         [DataMember]
        public string Client { get; set; }
         [DataMember]
        public string Ordre { get; set; }
            [DataMember]
         public string FK_PRODUIT { get; set; }
            [DataMember]
            public string PRODUIT { get; set; }
            [DataMember]
            public string DC { get; set; }
         [DataMember]
        public decimal SoldeInitial { get; set; }
         [DataMember]
        public decimal? SoldeCourant { get; set; }
         [DataMember]
        public int NbreFactImpayes { get; set; }
         [DataMember]
        public int? NbreFactRegle { get; set; }
         [DataMember]
        public int IndexO { get; set; }
         [DataMember]
        public int? IndexE { get; set; }
         [DataMember]
         public int? FK_IDCLIENT { get; set; }
              [DataMember]
         public int FK_IDCAMPAGNE { get; set; }
              [DataMember]
              public int FK_IDOBSERVATION { get; set; }
         [DataMember]
        public decimal? Frais { get; set; }
         [DataMember]
        public string Nom { get; set; }
         [DataMember]
        public string Tournee { get; set; }
         [DataMember]
        public string Rue { get; set; }
         [DataMember]
        public string Porte { get; set; }
         [DataMember]
        public string Quartier { get; set; }
         [DataMember]
        public string CtrO { get; set; }
         [DataMember]
        public string CtrE { get; set; }
         [DataMember]
        public string MatrAgent { get; set; }
         [DataMember]
         public string MATRICULE { get; set; }
         [DataMember]
        public string NomAgent { get; set; }
         [DataMember]
        public string NomAgence { get; set; }
         [DataMember]
        public string NomQuartier { get; set; }
         [DataMember]
        public string Observation { get; set; }
         [DataMember]
        public string PeriodeRelance { get; set; }
         [DataMember]
        public DateTime DateExigibilite { get; set; }
         [DataMember]
        public string DebutTournee { get; set; }
         [DataMember]
        public string FinTournee { get; set; }
         [DataMember]
        public string DebutOrdTournee { get; set; }
         [DataMember]
        public string FinOrdTournee { get; set; }
         [DataMember]
        public decimal MontantRelance { get; set; }
         [DataMember]
        public DateTime? DateCoupure { get; set; }
         [DataMember]
        public DateTime? DateRemise { get; set; }
         [DataMember]
        public DateTime? DateRDV { get; set; }
         [DataMember]
        public string FirstRouteNumber { get; set; }
         [DataMember]
        public string LastRouteNumber { get; set; }
        public aCampagne()
        {
            IdCoupure = null;
            Centre = null;
            Client = null;
            Ordre = null;
            Nom = null;
            Tournee = null;
            Quartier = null;
            Rue = null;
            Porte = null;
            CtrO = null;
            CtrE = null;
            MatrAgent = null;
            NomAgent = null;
            NomAgence = null;
            Observation = null;
            NomQuartier = null;
            Id = 0;
            SoldeInitial = 0;
            SoldeCourant = 0;
            NbreFactImpayes = 0;
            NbreFactRegle = 0;
            Frais = 0;
            IndexE = 0;
            IndexO = 0;
            PeriodeRelance = null;
            DebutTournee = null;
            FinTournee = null;
            DebutOrdTournee = null;
            FinOrdTournee = null;
            MontantRelance = 0;
            DateRemise = null;
            DateRDV = null;
            DateCoupure = null;
            FirstRouteNumber = null;
            LastRouteNumber = null;
            IdCoupure = null;
        }

        public override string ToString()
        {
            return IdCoupure;
        }
    }
   

   



}
