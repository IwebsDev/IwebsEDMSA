using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFacture : CsPrint
    {
        [DataMember] public int PK_IDLCLIENT { get; set; }
        [DataMember] public int? PK_IDCLIENT { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public int FK_IDETAPEDEVIS { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDTYPEDEVIS { get; set; }
        [DataMember] public int FK_IDCATEGORIECLIENT { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ACQUITANTERIEUR { get; set; }
        [DataMember] public string  ORDRE { get; set; }
        [DataMember] public string   REFEM { get; set; }
        [DataMember] public string IDENTITY { get; set; }
        [DataMember] public string   NDOC { get; set; }
        [DataMember] public string   NATURE { get; set; }
        [DataMember] public string NUMDEVIS { get; set; }
        [DataMember] public string   LIBCOURT { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public string   DATEFACTURE { get; set; }
        [DataMember] public string   DC { get; set; }
        [DataMember] public string   DATEEXIGIBLE { get; set; }
        [DataMember] public DateTime? EXIGIBLE { get; set; }
        [DataMember] public decimal  MONTANTFACTURE { get; set; }
        [DataMember] public decimal? MONTANTFACTURED { get; set; }
        [DataMember] public decimal? MONTANTTAXE { get; set; }
        [DataMember] public decimal? MONTANTHT { get; set; }
        [DataMember] public DateTime? EXIGIBILITE { get; set; }
        [DataMember] public decimal  PENALITE { get; set; }
        [DataMember] public string CTAXE { get; set; }
        [DataMember] public string TAXE { get; set; }
        [DataMember] public decimal MONTANTPAYPARTIEL { get; set; }
        [DataMember] public string  NUMDEM { get; set; }
        [DataMember] public string FK_NUMDEVIS { get; set; }
        [DataMember] public decimal Tax { get; set; }

        [DataMember] public decimal MontantExigible { get; set; }
        [DataMember] public decimal MontantNonExigible { get; set; }
        [DataMember] public decimal SoldeApresPaiement { get; set; }

        [DataMember] public decimal  SOLDEFACTURE { get; set; }
        [DataMember] public decimal? SOLDEFACTURED { get; set; }
        [DataMember] public string CAISSE { get; set; }
        [DataMember] public string MatriculeCaiss { get; set; }
        [DataMember] public string SaisiePar { get; set; }
        [DataMember] public DateTime? DateEncaiss { get; set; }
        [DataMember] public Boolean     Selectionner { get; set; }
        [DataMember] public bool     traiter { get; set; }
        [DataMember] public string   TOP1 { get; set; }
        [DataMember] public string   CRET { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public decimal  AVANCE { get; set; }
        [DataMember] public decimal? AVANCED { get; set; }
        [DataMember] public int  NumEtape { get; set; }
        [DataMember] public decimal SoldeClient { get; set; }
        [DataMember] public decimal? MONTANT { get; set; }
        [DataMember] public decimal MONTANTNAF { get; set; }
        [DataMember] public DateTime ? DENR { get; set; }
        [DataMember] public string moiscompt { get; set; }
        [DataMember] public string STATUT { get; set; }
        [DataMember] public int IdMoratoire { get; set; }

        // CHK 28/02/2013
        [DataMember]
        public string TOURNEE { get; set; }

        [DataMember]
        public DateTime? DateAbonnement { get; set; }

        [DataMember]
        public int FK_IDCATEGORIE { get; set; }
        
        [DataMember]
        public string Categorie { get; set; }
        [DataMember]
        public string LibelleCategorie { get; set; }
        [DataMember]
        public DateTime? DCAISSE { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }

        [DataMember] public string Tdem { get; set; }
        [DataMember] public bool  IsExonerationTaxe { get; set; }

    }

}









