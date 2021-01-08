using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
  public  class aDisconnection : CsPrint
    {
      //DETAIL
      [DataMember] public string CENTRE {get ;set;}
      [DataMember] public string  CLIENT {get;set;}
      [DataMember] public string ORDRE {get;set;}
      [DataMember] public string TOURNEE { get;set;}
      [DataMember] public string ORDTOUR { get;set;}
      [DataMember] public string NOMABON {get;set;}
      [DataMember] public string CATCLI {get;set;}
      [DataMember] public decimal SOLDEDUE {get;set;}
      [DataMember] public Int32  BILLSDUE {get;set;}
      [DataMember] public int  TOTALFACUTREDUE {get;set;}
      [DataMember] public decimal SOLDETOTAL {get;set;}
      [DataMember] public string PRODUIT {get;set;}
      [DataMember] public string COMPTEURELECT {get;set;}
      [DataMember] public int? ANCINDEXELECT {get;set;}
      [DataMember] public string COMPTEURWATER { get; set; }
      [DataMember] public int? ANCINDEXWATER { get; set; }
      [DataMember] public string Adresse { get; set; }
      [DataMember] public string PERIODEFACTURE { get; set; }
      [DataMember] public string NDOC { get; set; }
      [DataMember] public decimal ? MONTANTFACTURE { get; set; }
      [DataMember] public decimal? SOLDEFACTURE { get; set; } 
      [DataMember] public string RUE { get; set; }
      [DataMember] public string PORTE { get; set; }
      [DataMember] public string CODECONSO { get; set; }
      //GLOBAL
      [DataMember] public string IdCoupure {get ;set;}
      [DataMember] public string NomControler { get; set; }
      [DataMember] public string DebutTournee { get; set; }
      [DataMember] public string FinTournee { get; set; }
      [DataMember] public string PeriodeRelance { get; set; }
      [DataMember] public string DateExigibilite { get; set; }
      [DataMember] public decimal MontantRelance { get; set; }
      [DataMember] public string DebutCategorie { get; set; }
      [DataMember] public string FinCategorie { get; set; }
      [DataMember] public string Agent { get; set; }
      [DataMember] public string NombreClient { get; set; }
      [DataMember] public string NombreFactureDue { get; set; }
      [DataMember] public string FirstRouteNumber { get; set; }
      [DataMember] public string LastRouteNumber { get; set; }
      [DataMember] public decimal MontantCampagne { get; set; }

      [DataMember] public decimal? SOLDENAF{get;set;}


      [DataMember] public string REGROUPEMENT { get; set; }
      [DataMember] public string LIBELLEREGROUPEMENT { get; set; }
      [DataMember] public string ADRESSEREGROUPEMENT { get; set; }

    }
}
