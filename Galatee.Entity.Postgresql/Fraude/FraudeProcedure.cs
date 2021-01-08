using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Galatee.Structure;


namespace Galatee.Entity.Model
{
     public class FraudeProcedure
    {

         public static DataTable RetourneListeSousControle()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _SousControle = context.SOURCECONTROLE;
                     query =from p in _SousControle
                     select new
                     {
                         p.PK_ID,
                         p.Libelle

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneListeMoyendeDenonciation()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _SousControle = context.MOYENDENONCIATION;

                     query =
                     from p in _SousControle


                     select new
                     {
                         p.PK_ID,
                         p.Libelle

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneListeClient()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _Client = context.CLIENT;

                     query =
                     from p in _Client


                     select new
                     {
                         p.PK_ID,
                         p.REFCLIENT,
                         p.NOMABON,

                         

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneClient(int fk_idcentre, string pCentre, string pClient,string Ordre)
         {
             try
             {
                // string ordreMax = string.Empty;
                 using (galadbEntities context = new galadbEntities())
                 {
                     var client = context.CLIENT;
                     var query = (from _LeCLIENT in client
                                  from _abon in _LeCLIENT.ABON
                                  where _LeCLIENT.CENTRE == pCentre && _LeCLIENT.REFCLIENT == pClient
                                         && _LeCLIENT.FK_IDCENTRE == fk_idcentre 
                                         && (string.IsNullOrEmpty(Ordre) || _LeCLIENT.ORDRE == Ordre) 
                                  select new
                                  {
                                      _LeCLIENT.CENTRE,
                                      _LeCLIENT.REFCLIENT,
                                      _LeCLIENT.ORDRE,
                                      _LeCLIENT.DENABON,
                                      _LeCLIENT.NOMABON,
                                      _LeCLIENT.DENMAND,
                                      _LeCLIENT.NOMMAND,
                                      _LeCLIENT.ADRMAND1,
                                      _LeCLIENT.ADRMAND2,
                                      _LeCLIENT.CPOS,
                                      _LeCLIENT.BUREAU,
                                      _LeCLIENT.DINC,
                                      _LeCLIENT.NOMTIT,
                                      _LeCLIENT.BANQUE,
                                      _LeCLIENT.GUICHET,
                                      _LeCLIENT.COMPTE,
                                      _LeCLIENT.RIB,
                                      _LeCLIENT.PROPRIO,
                                      _LeCLIENT.CODECONSO,
                                      _LeCLIENT.CATEGORIE,
                                      _LeCLIENT.CODERELANCE,
                                      _LeCLIENT.NOMCOD,
                                      _LeCLIENT.MOISNAIS,
                                      _LeCLIENT.ANNAIS,
                                      _LeCLIENT.NOMPERE,
                                      _LeCLIENT.NOMMERE,
                                      _LeCLIENT.NATIONNALITE,
                                      _LeCLIENT.CNI,
                                      _LeCLIENT.TELEPHONE,
                                      _LeCLIENT.MATRICULE,
                                      _LeCLIENT.REGROUPEMENT,
                                      _LeCLIENT.REGEDIT,
                                      _LeCLIENT.FACTURE,
                                      _LeCLIENT.DMAJ,
                                      _LeCLIENT.REFERENCEPUPITRE,
                                      _LeCLIENT.PAYEUR,
                                      _LeCLIENT.SOUSACTIVITE,
                                      _LeCLIENT.AGENTFACTURE,
                                      _LeCLIENT.AGENTRECOUVR,
                                      _LeCLIENT.AGENTASSAINI,
                                      _LeCLIENT.REGROUCONTRAT,
                                      _LeCLIENT.INSPECTION,
                                      _LeCLIENT.REGLEMENT,
                                      _LeCLIENT.DECRET,
                                      _LeCLIENT.CONVENTION,
                                      _LeCLIENT.REFERENCEATM,
                                      _LeCLIENT.PK_ID,
                                      _LeCLIENT.DATECREATION,
                                      _LeCLIENT.DATEMODIFICATION,
                                      _LeCLIENT.USERCREATION,
                                      _LeCLIENT.USERMODIFICATION,
                                      _LeCLIENT.FK_IDMODEPAIEMENT,
                                      _LeCLIENT.FK_IDAG,
                                      _LeCLIENT.FK_IDCODECONSO,
                                      _LeCLIENT.FK_IDCATEGORIE,
                                      _LeCLIENT.FK_IDRELANCE,
                                      _LeCLIENT.FK_IDNATIONALITE,
                                      _LeCLIENT.FK_IDREGROUPEMENT,
                                      _LeCLIENT.FK_IDCENTRE,
                                      _LeCLIENT.FK_TYPECLIENT,
                                      _LeCLIENT.FK_IDUSAGE,
                                      _LeCLIENT.FK_IDPROPRIETAIRE,
                                      _LeCLIENT.MODEPAIEMENT,
                                      _LeCLIENT.FAX,
                                      _LeCLIENT.AG.FK_IDCOMMUNE,
                                      _LeCLIENT.AG.FK_IDQUARTIER,
                                      _LeCLIENT.AG.FK_IDRUE,
                                      _LeCLIENT.AG.PORTE,
                                      _LeCLIENT.AG.FK_IDSECTEUR,
                                      _abon.PRODUIT,
                                      _abon.FK_IDPRODUIT ,
                                      _LeCLIENT.CODEIDENTIFICATIONNATIONALE,
                                      LIBELLEMODEPAIEMENT = _LeCLIENT.MODEPAIEMENT1.LIBELLE,
                                      LIBELLECODECONSO = _LeCLIENT.CODECONSOMMATEUR.LIBELLE,
                                      LIBELLECATEGORIE = _LeCLIENT.CATEGORIECLIENT.LIBELLE,
                                      LIBELLERELANCE = _LeCLIENT.RELANCE.LIBELLE,
                                      LIBELLENATIONALITE = _LeCLIENT.NATIONALITE.LIBELLE,
                                      LIBELLEPAYEUR = _LeCLIENT.PAYEUR1.NOM,
                                      LIBELLEREGCLI = _LeCLIENT.REGROUPEMENT1.NOM,
                                      LIBELLESITE = _LeCLIENT.CENTRE1.SITE.LIBELLE,
                                      LIBELLECENTRE = _LeCLIENT.CENTRE1.LIBELLE,
                                      LIBELLETYPEPIECE = _LeCLIENT.PIECEIDENTITE.LIBELLE,
                                      LIBELLEUSAGE = _LeCLIENT.USAGE.LIBELLE 
                                  });
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
               //  return ordreMax;

             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneListeClientFraude(string IdentificationUnique, int pkCentre)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _Client = context.CLIENTFRAUDE;
                      query = (from p in _Client
                               where p.IdentificationUnique == IdentificationUnique && p.FK_IDCENTRE == pkCentre


                     select new
                     {
                         p.PK_ID,
                         p.FK_IDCENTRE,
                         p.Client,
                         p.Centre,
                         p.Ordre,
                         p.IdentificationUnique,
                         p.Nomabon,
                         p.Email,
                         p.Telephone,
                         p.Commune,
                         p.Quartier,
                         p.Rue,
                         p.Porte,
                         p.Tournee,
                         p.OrdreTournee,
                         p.Secteur,
                         p.ContratAbonnement,
                         p.ContratBranchement,
                         p.DateContratAbonnement,
                         p.DateContratBranchement,
                         p.PuissanceSouscrite,
                         p.PuissanceInstallee
                        
                     });
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneListeMarqueDisjoncteur()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _MarqueDISJONCTEUR = context.MARQUEDISJONCTEUR; 

                     query =
                     from p in _MarqueDISJONCTEUR


                     select new
                     {
                         p.PK_ID,
                         p.Libelle,
                      

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneListePhaseCompteur()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _PhaseCompteur = context.PHASECOMPTEUR;

                     query =
                     from p in _PhaseCompteur


                     select new
                     {
                         p.PK_ID,
                         p.LIBELLE,
                         p.CODE,

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneTypeDeFraude()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _TypeDeFraude = context.TYPEFRAUDE;

                     query =
                     from p in _TypeDeFraude


                     select new
                     {
                         p.PK_ID,
                         p.Libelle,
                       p.FK_IDORGANEFRAUDE,

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneActionSurCompteur()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _ActionSurCompteur= context.ACTIONSURCOMPTEUR;

                     query =
                     from p in _ActionSurCompteur


                     select new
                     {
                         p.PK_ID,
                         p.LIBELLE,
                        
                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneOrganeFraude()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _OrganeFraude = context.ORGANEFRAUDE;

                     query =
                     from p in _OrganeFraude


                     select new
                     {
                         p.PK_ID,
                         p.Libelle,
                         p.Code,

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneQualiteExpert()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _QualiteExpert = context.QUALITEEXPERT;

                     query =
                     from p in _QualiteExpert


                     select new
                     {
                         p.PK_ID,
                         p.Libelle,
                         

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneDecisonfrd()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _DECISIONFRAUDE = context.DECISIONFRAUDE;

                     query =
                     from p in _DECISIONFRAUDE


                     select new
                     {
                         p.PK_ID,
                         p.Libelle,


                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneFraude()
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             { 

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _FRAUDE = context.FRAUDE;

                     query =
                     from p in _FRAUDE

                     select new
                     {
                         p.PK_ID,
                         p.BordereauTransmission,
                         p.MontantCaution,
                         p.DateReclamation,
                         p.MotifReclamation,
                         p.DateEtape,
                         p.FicheTraitement,
                         p.OrdreTraitement,
                         p.IsConvocationRespectee,
                         p.IsFraudeConfirmee,
                         p.FK_IDCLIENTFRAUDE,
                         p.FK_IDDENONCIATEUR,
                         p.FK_IDSOURCECONTROLE,
                         p.FK_IDDEMANDE,

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneFraudeDemande(int fk_IDDEMANDE)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _FRAUDE = context.FRAUDE;

                     query =
                     from p in _FRAUDE
                     where
                        p.FK_IDDEMANDE == fk_IDDEMANDE

                     select new
                     {
                         p.PK_ID,
                         p.IsConvocationRespectee,
                         p.IsFraudeConfirmee,
                         p.MontantCaution,
                         p.DateReclamation,
                         p.MotifReclamation,
                         p.DateCreation,
                         p.DateEtape,
                         p.FicheTraitement,
                         p.OrdreTraitement,
                         p.BordereauTransmission,
                         p.FK_IDCLIENTFRAUDE,
                         p.FK_IDDENONCIATEUR,
                         p.FK_IDSOURCECONTROLE,
                         p.FK_IDDECISIONFRAUDE,
                         p.FK_IDDEMANDE 
                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public static DataTable RetourneCompteurFraude(int fk_CltFraude)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     string libelle_Produit;
                     IEnumerable<object> query = null;
                     var _COMPTEURFRAUDE = context.COMPTEURFRAUDE;

                     query =
                     from p in _COMPTEURFRAUDE
                     where
                        p.FK_IDCLIENTFRAUDE == fk_CltFraude

                     select new
                     {
                         p.PK_ID,
                         p.NumeroCompteur ,
                         p.IndexCompteur,
                         p.RefPlombCompteur,
                         p.NumeroPince,
                         p.CertificatPlombage,
                         p.RefPlombCacheBorne ,
                         p.RefPlombCoffretFusible,
                         p.RefPlombCoffretSecurite,
                         p. Bordereau ,
                         p.IsEcartIndex ,
                         p.FK_IDCLIENTFRAUDE ,
                         p.FK_IDCONTROLE,
                         p.FK_IDPRODUIT ,
                         p. FK_IDMARQUECOMPTEUR ,
                         p. FK_IDTYPECOMPTEUR ,
                         p.FK_IDANOMALIECOMPTEUR,
                         p.FK_IDANOMALIECACHEBORNE,
                         p.FK_IDANOMALIEBRANCHEMENT,
                         p.FK_IDPHASECOMPTEUR,
                         p.FK_IDCALIBRECOMPTEUR,
                         p.FK_IDREGLAGE,
                         p.FK_IDACTIONSURCOMPTEUR,
                         p.FK_IDUSAGEPRODUIT,
                         p.FK_IDTYPEDISJONCTEUR,
                         p.FK_IDMARQUEDISJONCTEUR,
                         p.PRODUIT,
                         p.TYPECOMPTEUR,
                         p.MARQUE,
                         p.USAGEPRODUIT,
                         libelle_Produit= p.PRODUIT1.LIBELLE,
                         libelle_usage = p.USAGE.LIBELLE  ,
                         libelle_AnnomalieBranchement= p.TYPEFRAUDE.Libelle ,
                         libelle_AnnomalieCompteur = p.TYPEFRAUDE1.Libelle,
                         libelle_AutreAnnomalie = p.TYPEFRAUDE2.Libelle,
                         libelle_Calibre = p.CALIBRECOMPTEUR.LIBELLE 
 
                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneClientDemande(int fkclientFrds)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
             try
             {

                 using (galadbEntities context = new galadbEntities())
                 {
                     IEnumerable<object> query = null;
                     var _CLIENTFRAUDE = context.CLIENTFRAUDE;

                     query =
                     from p in _CLIENTFRAUDE
                     where
                        p.PK_ID == fkclientFrds

                     select new
                     {
                         p.PK_ID,
                        p.Nomabon,
                        p.Client,
                        p.Commune,
                        p.FK_IDCENTRE,
                        p.ContratAbonnement,
                        p.ContratBranchement,
                        p.DateContratAbonnement,
                        p.DateContratBranchement,

                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);

                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         //public static DataTable RetourneClientFraude()

        public static DataTable RetourneControle()
      {
          //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
          try
          {

              using (galadbEntities context = new galadbEntities())
              {
                  IEnumerable<object> query = null;
                  var _FRAUDE = context.CONTROLE;

                  query =
                  from p in _FRAUDE


                  select new
                  {
                      p.PK_ID,
                     p.FicheControle,
                     p.Ordonnateur,
                     p.NomExpert,
                     p.DateControle,
                     p.HeureControle,
                     p.DescriptionIrregulariteEtObservations,
                     p.IsAbonneOuRepresentantPresent,
                     p.CourantAdmissibleParCable,
                     p.IsFraudeAveree,
                     p.IsConvocationRemise,
                     p.CommissariatPolicePresent,
                     p.OrdreTraitement,
                     p.FK_IDCLIENTFRAUDE,
                     p.FK_IDFRAUDE,
                     p.FK_IDQUALITEEXPERT,
                     p.FK_IDPROCESVERBALSCANNE
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);

              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

        public static DataTable RetourneControleur()
      {
          //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
          try
          {

              using (galadbEntities context = new galadbEntities())
              {
                  IEnumerable<object> query = null;
                  var _CONTROLEUR = context.CONTROLEUR;

                  query =
                  from p in _CONTROLEUR


                  select new
                  {
                      p.PK_ID,
                      p.FK_IDCONTROLE,
                      p.FK_IDUSERCONTROLEUR,
                      p.IsChefEquipe,
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);

              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

        public static DataTable RetourneCompteurFraude()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    string Libelle_ACTIONSURCOMPTEUR, Libelle_Produit;
                    IEnumerable<object> query = null;
                    var _COMPTEURFRAUDE = context.COMPTEURFRAUDE;

                    query =
                    from p in _COMPTEURFRAUDE


                    select new
                    {
                        p.PK_ID,
                        p.RefPlombCompteur,
                        p.CertificatPlombage,
                        p.RefPlombCacheBorne,
                        p.RefPlombCoffretFusible,
                        p.RefPlombCoffretSecurite,
                        p.Bordereau,
                        p.IsEcartIndex,
                        p.FK_IDACTIONSURCOMPTEUR,
                        Libelle_ACTIONSURCOMPTEUR=p.ACTIONSURCOMPTEUR.LIBELLE,
                        p.FK_IDCLIENTFRAUDE,
                        p.FK_IDPRODUIT,
                        Libelle_Produit=p.PRODUIT1.LIBELLE,
                        p.FK_IDREGLAGE,
                        p.FK_IDTYPECOMPTEUR,
                        p.FK_IDTYPEDISJONCTEUR,
                        p.FK_IDUSAGEPRODUIT,
                        p.FK_IDCALIBRECOMPTEUR,
                        p.FK_IDCONTROLE,
                        p.FK_IDPHASECOMPTEUR,
                        p.FK_IDANOMALIEBRANCHEMENT,
                        p.FK_IDANOMALIECACHEBORNE,
                        p.FK_IDANOMALIECOMPTEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneAPPAREILRECENSE(int idControle)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                 
                    IEnumerable<object> query = null;
                    var _APPAREILRECENSE = context.APPAREILRECENSE;

                    query =
                      from p in _APPAREILRECENSE
                      where
                         p.FK_IDCONTROLE == idControle
                      select new
                      {
                          p.PK_ID,
                          p.PUISSANCEUNITAIRE,
                          p.NOMBRE,
                          p.FK_IDAPPAREIL,
                          p.FK_IDCONTROLE,
                          p.FK_IDPRODUIT,
                         p.CODEAPPAREIL,
                         p.OBSERVATION,
                        
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneAPPAREILSUTILISE(int idControle)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    string DESIGNATION;
                    int Consommation, Journaliere, Mensuelle, estimee, TEMPSUTILISATION;
                    IEnumerable<object> query = null;
                    var _APPAREILSUTILISE = context.APPAREILSUTILISE;

                   query =
                     from p in _APPAREILSUTILISE
                     where
                        p.FK_IDAUDITION == idControle
                     select new
                    {
                    p.PK_ID,
                    p.APPAREILS.DESIGNATION,
                    p.APPAREILS.TEMPSUTILISATION,
                    p.PUISSANCEUNITAIRE,
                    Consommation= p.PUISSANCEUNITAIRE* p.APPAREILS.TEMPSUTILISATION,
                    Journaliere = (p.PUISSANCEUNITAIRE * p.APPAREILS.TEMPSUTILISATION )/ 100,
                    p.NOMBRE,
                    Mensuelle =(((p.PUISSANCEUNITAIRE * p.APPAREILS.TEMPSUTILISATION) / 100)*p.NOMBRE)*30,
                    p.FK_IDAPPAREIL,
                    p.FK_IDAUDITION,
                    p.FK_IDPRODUIT,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneTranche()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    
                    IEnumerable<object> query = null;
                    var _TRANCHEFRAUDE = context.TRANCHEFRAUDE;

                    query =
                      from p in _TRANCHEFRAUDE
                      
                      select new
                      {
                         p.PK_ID,
                         p.LIBELLE,
                         p.NUMTRANCHE,
                         p.PRIXUNITAIRE,
                         p.FK_IDPHASECOMPTEUR,
                         p.FK_IDPRODUIT,
                         p.FK_IDREGLAGECOMPTEUR,
                         p.CONSOMAXI,
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetournePresentationEdm()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _PRESTATIONEDM = context.PRESTATIONEDM;

                    query =
                      from p in _PRESTATIONEDM

                      select new
                      {
                         p.PK_ID,
                         p.Libelle,
                         p.PrixUnitaire,
                         p.EstModifiable,
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetournePRESTATIONREMBOURSABLE()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _PRESTATIONREMBOURSABLE = context.PRESTATIONREMBOURSABLE;

                    query =
                      from p in _PRESTATIONREMBOURSABLE

                      select new
                      {
                          p.PK_ID,
                          p.Libelle,
                          p.PrixUnitaire,
                          p.EstModifiable,
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneMoisDejaFacturee( int Fk_IDConsommation)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _MOISDEJAFACTURES = context.MOISDEJAFACTURES;

                    query =
                      from p in _MOISDEJAFACTURES
                      where
                       p.FK_IDCONSOMMATION == Fk_IDConsommation
                      select new
                      {
                          p.PK_ID,
                          p.ConsoDejaFacturee,
                          p.CONSOMMATION,
                          p.OrdreMois,
                          p.FK_IDCONSOMMATION,
                          p.FK_IDPRODUIT,
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneRegularisation()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {

                    IEnumerable<object> query = null;
                    var _REGULARISATION = context.REGULARISATION;

                    query =
                      from p in _REGULARISATION

                      select new
                      {
                          p.PK_ID,
                          p.Libelle,
                          p.PrixUnitaire,
                          p.EstModifiable,
                          p.FK_IDPHASECOMPTEUR,
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneUsage()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Usage = context.USAGE;

                    query =
                    from p in _Usage


                    select new
                    {
                        p.PK_ID,
                        p.LIBELLE,
                        p.CODE,
                      
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public static void InsertionFraudeControle(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {

            try
            {
                galadbEntities contextinter = new galadbEntities();
                CLIENTFRAUDE _LeClient = new CLIENTFRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENTFRAUDE, CsClientFraude>(pDemandeFrd.ClientFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.FK_IDSITE = null;
                    #endregion
                }
                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.Fraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _FRAUDE.DateCreation = DateTime.Now;
                    #endregion
                }

                CONTROLE _CONTROLE = new CONTROLE();
                if (pDemandeFrd.Controle != null)
                {
                    _CONTROLE = Entities.ConvertObject<CONTROLE, CsControle>(pDemandeFrd.Controle);
                    #region Vider les propriété d'identification sur l'objet client
                    _CONTROLE.FK_IDFRAUDE = _FRAUDE.PK_ID;
                    _CONTROLE.FK_IDCLIENTFRAUDE = _LeClient.PK_ID;

                    #endregion
                }
                CONTROLEUR _CONTROLEUR = new CONTROLEUR();
                if (pDemandeFrd.Controleur != null)
                {
                    _CONTROLEUR = Entities.ConvertObject<CONTROLEUR, CsControleur>(pDemandeFrd.Controleur);
                    _CONTROLEUR.FK_IDCONTROLE = _CONTROLE.PK_ID;

                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                COMPTEURFRAUDE _COMPTEURFRAUDE = new COMPTEURFRAUDE();
                if (pDemandeFrd.CompteurFraude != null)
                {
                    _COMPTEURFRAUDE = Entities.ConvertObject<COMPTEURFRAUDE, CsCompteurFraude>(pDemandeFrd.CompteurFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _COMPTEURFRAUDE.FK_IDCONTROLE = _CONTROLE.PK_ID;
                    _COMPTEURFRAUDE.FK_IDCLIENTFRAUDE = _LeClient.PK_ID;
                    #endregion
                }

                List<APPAREILRECENSE> _APPAREILRECENSE = new List<APPAREILRECENSE>();
                if (pDemandeFrd.AppareilRecenseFrd != null)
                {
                    _APPAREILRECENSE = Entities.ConvertObject<APPAREILRECENSE, CsAppareilRecenseFrd>(pDemandeFrd.AppareilRecenseFrd);
                    #region Vider les propriété d'identification sur l'objet client
                  
                    #endregion
                }

             //   _LeClient.FK_IDAG = _LeAG.PK_ID;


                Entities.UpdateEntity<CLIENTFRAUDE>(_LeClient, pContext);

                Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);

                Entities.InsertEntity<CONTROLE>(_CONTROLE, pContext);
                Entities.InsertEntity<CONTROLEUR>(_CONTROLEUR, pContext);

                Entities.InsertEntity<COMPTEURFRAUDE>(_COMPTEURFRAUDE, pContext);

                foreach (APPAREILRECENSE item in _APPAREILRECENSE)
                {
                    item.FK_IDCONTROLE = _CONTROLE.PK_ID;
                    item.FK_IDPRODUIT = (int)_COMPTEURFRAUDE.FK_IDPRODUIT;
                   

                }
                Entities.InsertEntity<APPAREILRECENSE>(_APPAREILRECENSE, pContext);

               
             
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static void InsertionFraudeAudite(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();
                CLIENTFRAUDE _LeClient = new CLIENTFRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENTFRAUDE, CsClientFraude>(pDemandeFrd.ClientFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.FK_IDSITE = null;
                    #endregion
                }
                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.Fraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _FRAUDE.DateCreation = DateTime.Now;
                    #endregion
                }

                COMPTEURFRAUDE _COMPTEURFRAUDE = new COMPTEURFRAUDE();
                if (pDemandeFrd.Controle != null)
                {
                    _COMPTEURFRAUDE = Entities.ConvertObject<COMPTEURFRAUDE, CsCompteurFraude>(pDemandeFrd.CompteurFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                List<APPAREILSUTILISE>_APPAREILSUTILISE = new List<APPAREILSUTILISE>();
                if (pDemandeFrd.AppareilUtiliserFrd != null)
                {
                    _APPAREILSUTILISE = Entities.ConvertObject<APPAREILSUTILISE, CsAppareilUtiliserFrd>(pDemandeFrd.AppareilUtiliserFrd);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                AUDITION _AUDITION = new AUDITION();
                if (pDemandeFrd.AuditionFraude != null)
                {
                    _AUDITION = Entities.ConvertObject<AUDITION, CsAuditionFraude>(pDemandeFrd.AuditionFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _AUDITION.FK_IDFRAUDE = _FRAUDE.PK_ID;

              
                    #endregion
                }
                

                Entities.UpdateEntity<CLIENTFRAUDE>(_LeClient, pContext);
                Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);
                Entities.UpdateEntity<COMPTEURFRAUDE>(_COMPTEURFRAUDE, pContext);
                Entities.InsertEntity<AUDITION>(_AUDITION, pContext);
                foreach (APPAREILSUTILISE item in _APPAREILSUTILISE)
                {
                    item.FK_IDAUDITION =  _AUDITION.PK_ID;
                    item.FK_IDPRODUIT = (int)_COMPTEURFRAUDE.FK_IDPRODUIT;
                }
               
                Entities.InsertEntity<APPAREILSUTILISE>(_APPAREILSUTILISE, pContext);


            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static void InsertionFraudeConsommation(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();
                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.Fraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _FRAUDE.DateCreation = DateTime.Now;
                    #endregion
                }
                CONSOMMATION _CONSOMMATION = new CONSOMMATION();
                if (pDemandeFrd.ConsommationFrd != null)
                {
                    _CONSOMMATION = Entities.ConvertObject<CONSOMMATION, CsConsommationFrd>(pDemandeFrd.ConsommationFrd);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                List<MOISDEJAFACTURES> _MOISDEJAFACTURES = new List<MOISDEJAFACTURES>();
                if (pDemandeFrd.MoisDejaFactures != null)
                {
                    _MOISDEJAFACTURES = Entities.ConvertObject<MOISDEJAFACTURES, CsMoisDejaFactures>(pDemandeFrd.MoisDejaFactures);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                foreach (MOISDEJAFACTURES item in _MOISDEJAFACTURES)
                {
                    item.FK_IDCONSOMMATION = _CONSOMMATION.PK_ID;
                }
                Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);
                Entities.InsertEntity<CONSOMMATION>(_CONSOMMATION, pContext);
                Entities.InsertEntity<MOISDEJAFACTURES>(_MOISDEJAFACTURES, pContext);


            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static void InsertionFraudeEmissionFacture(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();

               
                List<DETAILparTRANCHE> _DETAILparTRANCHE = new List<DETAILparTRANCHE>();
                if (pDemandeFrd.DETAILparTRANCHE != null)
                {
                    _DETAILparTRANCHE = Entities.ConvertObject<DETAILparTRANCHE, CsDETAILparTRANCHE>(pDemandeFrd.DETAILparTRANCHE);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                foreach (DETAILparTRANCHE item in _DETAILparTRANCHE)
                {
                    item.FK_IDPRODUIT = (int)pDemandeFrd.CompteurFraude.FK_IDPRODUIT;
                }
                List<DETAILparPRESTATIONEDM> _DETAILparPRESTATIONEDM = new List<DETAILparPRESTATIONEDM>();
                if (pDemandeFrd.DetailParPresentationEdm != null)
                {
                    _DETAILparPRESTATIONEDM = Entities.ConvertObject<DETAILparPRESTATIONEDM, CsDetailParPresentationEdm>(pDemandeFrd.DetailParPresentationEdm);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                foreach (DETAILparPRESTATIONEDM item in _DETAILparPRESTATIONEDM)
                {
                    item.FK_IDPRODUIT = (int)pDemandeFrd.CompteurFraude.FK_IDPRODUIT;
                }
                List<DETAILparPRESTATIONREMBOURSABLE> _DETAILparPRESTATIONREMBOURSABLE = new List<DETAILparPRESTATIONREMBOURSABLE>();
                if (pDemandeFrd.DETAILparPRESTATIONREMBOURSABLE != null)
                {
                    _DETAILparPRESTATIONREMBOURSABLE = Entities.ConvertObject<DETAILparPRESTATIONREMBOURSABLE, CsDETAILparPRESTATIONREMBOURSABLE>(pDemandeFrd.DETAILparPRESTATIONREMBOURSABLE);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                foreach (DETAILparPRESTATIONREMBOURSABLE item in _DETAILparPRESTATIONREMBOURSABLE)
                {
                    item.FK_IDPRODUIT = (int)pDemandeFrd.CompteurFraude.FK_IDPRODUIT;
                }
                List<DETAILparREGULARISATION> _DETAILparREGULARISATION = new List<DETAILparREGULARISATION>();
                if (pDemandeFrd.DETAILparREGULARISATION != null)
                {
                    _DETAILparREGULARISATION = Entities.ConvertObject<DETAILparREGULARISATION, CsDETAILparREGULARISATION>(pDemandeFrd.DETAILparREGULARISATION);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                foreach (DETAILparREGULARISATION item in _DETAILparREGULARISATION)
                {
                    item.FK_IDPRODUIT = (int)pDemandeFrd.CompteurFraude.FK_IDPRODUIT;
                }
                CONSOMMATION _CONSOMMATION = new CONSOMMATION();
                if (pDemandeFrd.ConsommationFrd != null)
                {
                    _CONSOMMATION = Entities.ConvertObject<CONSOMMATION, CsConsommationFrd>(pDemandeFrd.ConsommationFrd);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                FACTUREFRAUDE _FACTUREFRAUDE = new FACTUREFRAUDE();
                if (pDemandeFrd.FactureFraude != null)
                {
                    _FACTUREFRAUDE = Entities.ConvertObject<FACTUREFRAUDE, CsFactureFraude>(pDemandeFrd.FactureFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _FACTUREFRAUDE.DATECREATION = DateTime.Now;
                    #endregion
                }
              
                //Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);
                Entities.UpdateEntity<CONSOMMATION>(_CONSOMMATION, pContext);
                Entities.InsertEntity<DETAILparTRANCHE>(_DETAILparTRANCHE, pContext);
                Entities.InsertEntity<DETAILparPRESTATIONEDM>(_DETAILparPRESTATIONEDM, pContext);
                Entities.InsertEntity<DETAILparPRESTATIONREMBOURSABLE>(_DETAILparPRESTATIONREMBOURSABLE, pContext);
                Entities.InsertEntity<DETAILparREGULARISATION>(_DETAILparREGULARISATION, pContext);
                Entities.InsertEntity<FACTUREFRAUDE>(_FACTUREFRAUDE, pContext);


            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static void InsertionControleIndex(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();

                COMPTEURFRAUDE _COMPTEURFRAUDE = new COMPTEURFRAUDE();
                if (pDemandeFrd.Controle != null)
                {
                    _COMPTEURFRAUDE = Entities.ConvertObject<COMPTEURFRAUDE, CsCompteurFraude>(pDemandeFrd.CompteurFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }


                Entities.UpdateEntity<COMPTEURFRAUDE>(_COMPTEURFRAUDE, pContext);

            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }
     
         public static void MiseAjourFraudeControle(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {

            try
            {
                galadbEntities contextinter = new galadbEntities();
                CLIENTFRAUDE _LeClient = new CLIENTFRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENTFRAUDE, CsClientFraude>(pDemandeFrd.ClientFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.FK_IDSITE = null;
                    #endregion
                }
                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                CONTROLE _CONTROLE = new CONTROLE();
                if (pDemandeFrd.Controle != null)
                {
                    _CONTROLE = Entities.ConvertObject<CONTROLE, CsControle>(pDemandeFrd.Controle);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }
                CONTROLEUR _CONTROLEUR = new CONTROLEUR();
                if (pDemandeFrd.Controleur != null)
                {
                    _CONTROLEUR = Entities.ConvertObject<CONTROLEUR, CsControleur>(pDemandeFrd.Controleur);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                COMPTEURFRAUDE _COMPTEURFRAUDE = new COMPTEURFRAUDE();
                if (pDemandeFrd.CompteurFraude != null)
                {
                    _COMPTEURFRAUDE = Entities.ConvertObject<COMPTEURFRAUDE, CsCompteurFraude>(pDemandeFrd.CompteurFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                APPAREILSUTILISE _APPAREILSUTILISE = new APPAREILSUTILISE();
                if (pDemandeFrd.AppareilUtiliserFrd != null)
                {
                    _CONTROLEUR = Entities.ConvertObject<CONTROLEUR, CsControleur>(pDemandeFrd.Controleur);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                //   _LeClient.FK_IDAG = _LeAG.PK_ID;

                _CONTROLE.FK_IDCLIENTFRAUDE = _LeClient.PK_ID;

                Entities.UpdateEntity<CLIENTFRAUDE>(_LeClient, pContext);

                Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);

                Entities.UpdateEntity<CONTROLE>(_CONTROLE, pContext);

                Entities.UpdateEntity<CONTROLEUR>(_CONTROLEUR, pContext);

                Entities.UpdateEntity<COMPTEURFRAUDE>(_COMPTEURFRAUDE, pContext);

             // a revoir   Entities.UpdateEntity<APPAREILSUTILISE>(_APPAREILSUTILISE, pContext);



            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static void MiseAjourFraudeAudite(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {


            try
            {
                galadbEntities contextinter = new galadbEntities();
                CLIENTFRAUDE _LeClient = new CLIENTFRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    _LeClient = Entities.ConvertObject<CLIENTFRAUDE, CsClientFraude>(pDemandeFrd.ClientFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    _LeClient.FK_IDSITE = null;
                    #endregion
                }
                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.Fraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                COMPTEURFRAUDE _COMPTEURFRAUDE = new COMPTEURFRAUDE();
                if (pDemandeFrd.Controle != null)
                {
                    _COMPTEURFRAUDE = Entities.ConvertObject<COMPTEURFRAUDE, CsCompteurFraude>(pDemandeFrd.CompteurFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                List<APPAREILRECENSE> _APPAREILRECENSE = new List<APPAREILRECENSE>();
                if (pDemandeFrd.AppareilRecenseFrd != null)
                {
                    _APPAREILRECENSE = Entities.ConvertObject<APPAREILRECENSE, CsAppareilRecenseFrd>(pDemandeFrd.AppareilRecenseFrd);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                AUDITION _AUDITION = new AUDITION();
                if (pDemandeFrd.AuditionFraude != null)
                {
                    _AUDITION = Entities.ConvertObject<AUDITION, CsAuditionFraude>(pDemandeFrd.AuditionFraude);
                    #region Vider les propriété d'identification sur l'objet client
                    #endregion
                }

                //   _LeClient.FK_IDAG = _LeAG.PK_ID;

                // _CONTROLE.FK_IDCLIENTFRAUDE = _LeClient.PK_ID;

                Entities.UpdateEntity<CLIENTFRAUDE>(_LeClient, pContext);
                Entities.UpdateEntity<FRAUDE>(_FRAUDE, pContext);
                Entities.UpdateEntity<COMPTEURFRAUDE>(_COMPTEURFRAUDE, pContext);
                Entities.InsertEntity<APPAREILRECENSE>(_APPAREILRECENSE, pContext);
                Entities.UpdateEntity<AUDITION>(_AUDITION, pContext);


            }

           // catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            catch (Exception ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }


        public static string InsertDemandeFraude(CsDemandeFraude pDemandeFrd, galadbEntities pContext)
        {
            try
            {

                galadbEntities contextinter = new galadbEntities();
                CLIENTFRAUDE _CLIENTFRAUDE = new CLIENTFRAUDE();
                if (pDemandeFrd.ClientFraude != null)
                {
                    //CsAg Ag = Entities.ConvertObject<CsAg, AG>(context.AG.FirstOrDefault(p => p.CLIENT == ClientFrd.Client));
                    DataTable dts = Galatee.Entity.Model.AccueilProcedures.RetourneAG(pDemandeFrd.ClientFraude.FK_IDCENTRE.Value, pDemandeFrd.ClientFraude.Centre, pDemandeFrd.ClientFraude.Client, string.Empty);
                    CsAg Ag = Entities.GetEntityFromQuery<CsAg>(dts);

                    //ClientFrd.FK_IDCENTRE = null;
                    pDemandeFrd.ClientFraude.FK_IDSITE = null;
                    if (Ag != null)
                    {
                        pDemandeFrd.ClientFraude.FK_IDCENTRE = Ag.FK_IDCENTRE;
                        pDemandeFrd.ClientFraude.Centre = Ag.CENTRE;
                        pDemandeFrd.ClientFraude.Commune = Ag.COMMUNE;
                        pDemandeFrd.ClientFraude.FK_IDCOMMUNE = Ag.FK_IDCOMMUNE;
                        pDemandeFrd.ClientFraude.FK_IDQUARTIER = Ag.FK_IDQUARTIER;
                        pDemandeFrd.ClientFraude.Quartier = Ag.QUARTIER;
                        pDemandeFrd.ClientFraude.FK_RUE = Ag.FK_IDRUE;
                        pDemandeFrd.ClientFraude.Rue = Ag.RUE;
                        pDemandeFrd.ClientFraude.FK_SECTEUR = Ag.FK_IDSECTEUR;
                        pDemandeFrd.ClientFraude.Secteur = Ag.SECTEUR;
                        pDemandeFrd.Denonciateur.FK_IDLOCALISATION = Ag.FK_IDCENTRE;
                    }


                    _CLIENTFRAUDE = Entities.ConvertObject<CLIENTFRAUDE, CsClientFraude>(pDemandeFrd.ClientFraude);
                    if (pDemandeFrd.ClientFraude.PK_ID != 0)
                    {
                        _CLIENTFRAUDE.FK_IDCENTRE = null;
                        Entities.UpdateEntity<CLIENTFRAUDE>(_CLIENTFRAUDE, pContext);
                        pContext.SaveChanges();
                    }
                    else
                    {
                        Entities.InsertEntity<CLIENTFRAUDE>(_CLIENTFRAUDE, pContext);
                        pContext.SaveChanges();
                    }

                    #region Vider les propriété d'identification sur l'objet DENONCIATEUR
                    #endregion
                }

                DENONCIATEUR _DENONCIATEUR = new DENONCIATEUR();
                if (pDemandeFrd.Denonciateur != null)
                {
                    _DENONCIATEUR.FK_IDLOCALISATION = (int)pDemandeFrd.ClientFraude.FK_IDCENTRE;
                    _DENONCIATEUR = Entities.ConvertObject<DENONCIATEUR, CsDenonciateur>(pDemandeFrd.Denonciateur);
                    #region Vider les propriété d'identification sur l'objet DENONCIATEUR
                    #endregion
                }


                DEMANDE _DEMANDE = new DEMANDE();
                if (pDemandeFrd.LaDemande != null)
                {
                    _DEMANDE = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemandeFrd.LaDemande);
                    #region Vider les propriété d'identification sur l'objet DENONCIATEUR


                    #endregion
                }

                FRAUDE _FRAUDE = new FRAUDE();
                if (pDemandeFrd.Fraude != null)
                {
                    _FRAUDE = Entities.ConvertObject<FRAUDE, CsFraude>(pDemandeFrd.Fraude);
                    #region Vider les propriété d'identification sur l'objet client


                    #endregion
                }


                //   _LeClient.FK_IDAG = _LeAG.PK_ID;

                // _CONTROLE.FK_IDCLIENTFRAUDE = _LeClient.PK_ID;

                Entities.InsertEntity<DENONCIATEUR>(_DENONCIATEUR, pContext);
                Entities.InsertEntity<DEMANDE>(_DEMANDE, pContext);
                _FRAUDE.FK_IDDENONCIATEUR = _DENONCIATEUR.PK_ID;
                _FRAUDE.FK_IDDECISIONFRAUDE = null;
                //_FRAUDE.FK_IDSOURCECONTROLE = null;
                _FRAUDE.FicheTraitement = _DEMANDE.NUMDEM;
                _FRAUDE.FK_IDDEMANDE = _DEMANDE.PK_ID;
                _FRAUDE.FK_IDCLIENTFRAUDE = _CLIENTFRAUDE.PK_ID;

                Entities.InsertEntity<FRAUDE>(_FRAUDE, pContext);
                contextinter.Dispose();
                return _DEMANDE.PK_ID + "." + _DEMANDE.NUMDEM;

            }

           // catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            catch (DbEntityValidationException ex)
            {
                //Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //return exceptionMessage;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static DataTable RetourneEvenement(int fk_idcentre, string pCentre, string pClient, string pOrdre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //string produit = string.IsNullOrEmpty(pProduit) ? "" : pProduit.ToLower();
                    //string point = pPoint == null ? "" : pPoint.ToString();
                    //string Evenement = pEvenement == null ? "" : pEvenement.ToString();
                    //string vide = "";

                    IEnumerable<object> query = null;
                    var _EVENEMENT = context.EVENEMENT;

                    query =
                    (from _LeEVENEMENT in _EVENEMENT
                     where _LeEVENEMENT.CENTRE == pCentre &&
                           _LeEVENEMENT.CLIENT == pClient &&
                           _LeEVENEMENT.ORDRE == pOrdre &&
                           _LeEVENEMENT.FK_IDCENTRE == fk_idcentre
                     //&&
                     //(produit.Equals(vide) || _LeEVENEMENT.PRODUIT == pProduit) &&
                     //(point.Equals(vide) || _LeEVENEMENT.POINT == pPoint) &&
                     //(Evenement.Equals(vide) || _LeEVENEMENT.NUMEVENEMENT == pEvenement
                     //)

                     select new
                     {
                         _LeEVENEMENT.CENTRE,
                         _LeEVENEMENT.CLIENT,
                         _LeEVENEMENT.PRODUIT,
                         _LeEVENEMENT.POINT,
                         _LeEVENEMENT.NUMEVENEMENT,
                         _LeEVENEMENT.ORDRE,
                         _LeEVENEMENT.COMPTEUR,
                         _LeEVENEMENT.DATEEVT,
                         _LeEVENEMENT.PERIODE,
                         _LeEVENEMENT.CODEEVT,
                         _LeEVENEMENT.INDEXEVT,
                         _LeEVENEMENT.CAS,
                         _LeEVENEMENT.ENQUETE,
                         _LeEVENEMENT.CONSO,
                         _LeEVENEMENT.CONSONONFACTUREE,
                         _LeEVENEMENT.LOTRI,
                         _LeEVENEMENT.FACTURE,
                         _LeEVENEMENT.SURFACTURATION,
                         _LeEVENEMENT.STATUS,
                         _LeEVENEMENT.TYPECONSO,
                         _LeEVENEMENT.REGLAGECOMPTEUR,
                         _LeEVENEMENT.TYPETARIF,
                         _LeEVENEMENT.FORFAIT,
                         _LeEVENEMENT.CATEGORIE,
                         _LeEVENEMENT.CODECONSO,
                         _LeEVENEMENT.PROPRIO,
                         _LeEVENEMENT.MODEPAIEMENT,
                         _LeEVENEMENT.MATRICULE,
                         _LeEVENEMENT.FACPER,
                         _LeEVENEMENT.QTEAREG,
                         _LeEVENEMENT.DERPERF,
                         _LeEVENEMENT.DERPERFN,
                         _LeEVENEMENT.CONSOFAC,
                         _LeEVENEMENT.REGIMPUTE,
                         _LeEVENEMENT.REGCONSO,
                         _LeEVENEMENT.COEFLECT,
                         _LeEVENEMENT.COEFCOMPTAGE,
                         _LeEVENEMENT.PUISSANCE,
                         _LeEVENEMENT.TYPECOMPTAGE,
                         _LeEVENEMENT.TYPECOMPTEUR,
                         _LeEVENEMENT.COEFK1,
                         _LeEVENEMENT.COEFK2,
                         _LeEVENEMENT.COEFFAC,
                         _LeEVENEMENT.USERCREATION,
                         _LeEVENEMENT.DATECREATION,
                         _LeEVENEMENT.DATEMODIFICATION,
                         _LeEVENEMENT.USERMODIFICATION,
                         _LeEVENEMENT.FK_IDABON,
                         _LeEVENEMENT.PK_ID,
                         PK_IDEVENT = _LeEVENEMENT.PK_ID,
                         //_LeEVENEMENT.FK_IDCAS,
                         _LeEVENEMENT.FK_IDCENTRE,
                         _LeEVENEMENT.FK_IDPRODUIT,
                         _LeEVENEMENT.FK_IDCANALISATION,
                         _LeEVENEMENT.FK_IDCOMPTEUR
                     });
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
      


    }
}
