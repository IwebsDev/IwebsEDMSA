using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFactureClient : CsPrint
    {
        #region Liste des champs provenants de EnteteFacture
       [DataMember]  public string Lotri { get; set; }
       [DataMember]  public string Jet { get; set; }
       [DataMember]  public string Dr { get; set; }
       [DataMember]  public string Centre { get; set; }
       [DataMember]  public string Client { get; set; }
       [DataMember]  public string LibelleRegcli { get; set; }
       [DataMember]  public string Ordre { get; set; }
       [DataMember]  public string  dateExige { get; set; }
       [DataMember]  public string DenAbon { get; set; }
       [DataMember]  public string NomAbon { get; set; }
       [DataMember]  public string Denmand { get; set; }
       [DataMember]  public string Nommand { get; set; }
       [DataMember]  public string Adrmand1 { get; set; }
       [DataMember]  public string Adrmand2 { get; set; }
       [DataMember]  public string Cpos { get; set; }
       [DataMember]  public string Bureau { get; set; }
       [DataMember]  public string Banque { get; set; }
       [DataMember]  public string Guichet { get; set; }
       [DataMember]  public string Compte { get; set; }
       [DataMember]  public string Codconso { get; set; }
       [DataMember]  public string Catcli { get; set; }
       [DataMember]  public string Regcli { get; set; }
       [DataMember]  public string Ag { get; set; }
       [DataMember]  public string Commune { get; set; }
       [DataMember]  public string Quartier { get; set; }
       [DataMember]  public string Rue { get; set; }
       [DataMember]  public string Nomrue { get; set; }
       [DataMember]  public string Numrue { get; set; }
       [DataMember]  public string Comprue { get; set; }
       [DataMember]  public string Etage { get; set; }
       [DataMember]  public string Porte { get; set; }
       [DataMember]  public string Tournee { get; set; }
       [DataMember]  public string OrdTour { get; set; }
       [DataMember]  public string NbFac { get; set; }
       [DataMember]  public string Facture { get; set; }
       [DataMember]  public string  Dfac { get; set; }
       [DataMember]  public string Mes { get; set; }
       [DataMember]  public Decimal? TotFht { get; set; }
       [DataMember]  public Decimal? TotFTax { get; set; }
       [DataMember]  public Decimal? TotFTTC { get; set; }
       [DataMember]  public Decimal? SoldeTotFTTC { get; set; }
       [DataMember]  public string Nature { get; set; }
       [DataMember]  public string Periode { get; set; }
       [DataMember]  public int? Exig { get; set; }
       [DataMember]  public string Coper { get; set; }
       [DataMember]  public string Email { get; set; }
       [DataMember]  public string LibelleCateg { get; set; }
       [DataMember]  public int OrdreAffichage { get; set; }
       [DataMember]  public string EMAIL { get; set; }
       [DataMember]  public string TELEPHONE { get; set; }
       [DataMember]  public string LibelleCEntre { get; set; }
       [DataMember]  public string ContactCEntre { get; set; }
       [DataMember]  public string fk_idClient { get; set; }
       [DataMember]  public decimal  MontantTimbre { get; set; }
       [DataMember]  public bool?   ISFACTURE { get; set; }
       [DataMember]  public bool?   ISSMS { get; set; }
       [DataMember]  public string   PuissanceSouscrite { get; set; }
       [DataMember]  public string  PuissanceInstalle { get; set; }
       [DataMember]  public int? ConsoMAximetre { get; set; }

       [DataMember]  public string  PertesActives { get; set; }
       [DataMember]  public string PertesReactives { get; set; }

       [DataMember]  public string  TotActiveAvecPertes  { get; set; }
       [DataMember]  public string TotReactiveAvecPertes  { get; set; }

       [DataMember] public int?  ConsoActiveHPointesAfterPertes { get; set; }
       [DataMember] public int?  ConsoActiveHPleinesAfterPertes { get; set; }
       [DataMember] public int?  ConsoActiveHCreusesAfterPertes { get; set; }

       [DataMember]  public string TanPhi  { get; set; }
       [DataMember]  public string WrMr  { get; set; }
       [DataMember]  public string WaMa  { get; set; }
       [DataMember]  public int idEntfac  { get; set; }

       [DataMember]  public string Matricule { get; set; }

        #endregion

        #region Liste des champs provenants de ProduitFacture
         [DataMember]  public string Produit { get; set; }
         [DataMember]  public string Compteur { get; set; }
         [DataMember]  public string Diametre { get; set; }
         [DataMember]  public int? AIndex { get; set; }
         [DataMember]  public int? NIndex { get; set; }
         [DataMember]  public string  DateReleve { get; set; }
         [DataMember]  public string  DateRelevePrec{ get; set; }
         [DataMember]  public int? ConsoFac { get; set; }
         [DataMember]  public string  TypeCompteur{ get; set; }

        
        //MT
         [DataMember]  public int? AIndexPointes { get; set; }
         [DataMember]  public int? NIndexPointes { get; set; }
         [DataMember]  public decimal ? CeofPointe{ get; set; }
         [DataMember]  public int? ConsomationPointes { get; set; }

         [DataMember]  public int? AIndexPleine { get; set; }
         [DataMember]  public int? NIndexPleine { get; set; }
         [DataMember]  public decimal? CeofPleine{ get; set; }
         [DataMember]  public int? ConsomationPleine { get; set; }


         [DataMember]  public int? AIndexCreuse { get; set; }
         [DataMember]  public int? NIndexCreuse { get; set; }
         [DataMember]  public decimal? CeofCreuse{ get; set; }
         [DataMember]  public int? ConsomationCreuse { get; set; }

         [DataMember]  public int? AIndexHoraire{ get; set; }
         [DataMember]  public int? NIndexHoraire { get; set; }
         [DataMember]  public decimal? CeofHoraire{ get; set; }
         [DataMember]  public int? ConsomationHoraire { get; set; }

         [DataMember]  public int? AIndexReactif      {get; set; }
         [DataMember]  public int? NIndexReactif      { get; set; }
         [DataMember]  public decimal? CeofReactif    { get; set; }
         [DataMember]  public int? ConsomationReactif { get; set; }

        #endregion

        #region Liste des champs provenant de RedevanceFacture
        [DataMember] public string LibelleProduit { get; set; }
        [DataMember] public string Tranche { get; set; }
        [DataMember] public string LibelleTranche { get; set; }
        [DataMember] public int? Quantite { get; set; }
        [DataMember] public int? NbFactureRedevance { get; set; }
        [DataMember] public string Unite { get; set; }
        [DataMember] public string  Taxe { get; set; }
        [DataMember] public Decimal? BarPrix { get; set; }
        [DataMember] public Decimal? TotRedHT { get; set; }
        [DataMember] public Decimal? TotRedTax { get; set; }
        [DataMember] public Decimal? TotRedTTC { get; set; }
        [DataMember] public string  TypeEdition { get; set; }
        [DataMember] public string  CodeOperation { get; set; }
        [DataMember] public string  Redevance { get; set; }
        [DataMember] public string  Point { get; set; }
        #endregion




    }
} 