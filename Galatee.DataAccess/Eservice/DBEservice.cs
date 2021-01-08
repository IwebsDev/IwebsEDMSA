using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;
//using Microsoft.Reporting;
//using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Web.Mail;
using System.ServiceModel.Activation;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBEservice
    {
        public DBEservice()
        {

        }

        #region Envoi de mail

        // Envoi de facture par mail
        public List<CsEnteteFacture> ListeDesClientPourEnvoieMail(List<int> CentreClient, List<string> lstPeriode, bool sms, bool email)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.EserviceProcedure.ListeDesClientPourEnvoieMail( CentreClient, lstPeriode, sms, email);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsEnvoiMail> ListeDesClientPourEnvoieMailRegroupement(int IdClient, List<string> lstPeriode, List<string> lstRegroupement, bool sms, bool email)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.EserviceProcedure.ListeDesClientPourEnvoieMailRegroupement(IdClient, lstPeriode, lstRegroupement, sms, email);
                return Entities.GetEntityListFromQuery<CsEnvoiMail>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsEnvoiMail> ListeDesClientPourEnvoieMailDiffusion(string Centre, string Site, string Categorie, string Tourne,bool sms,bool email)
        {

            //sqlCommand.CommandText = "SPX_FACT_LISTEENVOIEMAIL";


            try
            {
                //DataTable dt = Galatee.Entity.Model.EserviceProcedure.ListeDesClientPourEnvoieMailDiffusion(Centre, Site, Categorie, Tourne, sms, email);
                DataTable dt = new DataTable();
                return Entities.GetEntityListFromQuery<CsEnvoiMail>(dt);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 #endregion

        #region Edition facture
        //Edition de facture
        public List<CsFactureClient> ListeDesFactures(string centre, string client, string ordre, string periode, string numFacture)
        {
            try
            {
                CsLafactureClient entfacs = new CsLafactureClient();
                Galatee.Entity.Model.CENTFAC dt = Galatee.Entity.Model.EserviceProcedure.ListeFactureABO07(centre, client, ordre, periode, numFacture).FirstOrDefault();
                entfacs._LstProfact = Entities.ConvertObject<CsProduitFacture, Galatee.Entity.Model.CPROFAC>(dt.CPROFAC.ToList());
                entfacs._LstRedFact = Entities.ConvertObject<CsRedevanceFacture, Galatee.Entity.Model.CREDFAC>(dt.CREDFAC.ToList());
                entfacs._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(dt);
                decimal? AncSolde = 0;
                //Galatee.Entity.Model.FonctionCaisse.RetourneSoldeClient(entfacs._LeEntatfac.FK_IDCLIENT);

                galadbEntities context = new galadbEntities();
                List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
                List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();

                List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
                int i = 0;
                foreach (CsProduitFacture  items in entfacs._LstProfact)
                {
                        foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(p => p.TRANCHE))
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                            //FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(x => x.PRODUIT == items.PRODUIT && x.CODE  == itemss.REDEVANCE && x.TRANCHE == itemss.TRANCHE).LIBELLE;
                            //FactureEdite.AncienReport = (AncSolde == 0 || AncSolde == null) ? 0 : AncSolde;
                            //new CsFactureClient().MajRedevanceFacture(ref FactureEdite, itemss);
                            //new CsFactureClient().MajProduitFacture(ref FactureEdite, items);
                            //new CsFactureClient().MajEnteteFacture(ref FactureEdite, entfacs._LeEntatfac);
                            FactureEdite.SoldeTotFTTC = FactureEdite.TotFTTC + AncSolde;
                            //FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(entfacs._LeEntatfac.DFAC) + TimeSpan.FromDays((int)entfacs._LeEntatfac.EXIG )).ToShortDateString();
                            i++;
                            FactureEdite.OrdreAffichage = i;
                            lstFactureEdite.Add(FactureEdite);
                        }
                }
                if (entfacs._LstRedFact.Count < 19)
                {
                    for (int j = i + 1; j <= 19; j++)
                    {
                        CsFactureClient FactureEdite = new CsFactureClient();
                        //if (entfacs._LstProfact != null)
                        //{
                        //    if (entfacs._LstProfact.Count > 0)
                        //        new CsFactureClient().MajProduitFacture(ref FactureEdite, entfacs._LstProfact[0]);
                        //}
                        //if (true)
                        //{
                        //    new CsFactureClient().MajEnteteFacture(ref FactureEdite, entfacs._LeEntatfac); 
                        //}
                       
                        FactureEdite.SoldeTotFTTC = FactureEdite.TotFTTC + AncSolde;
                        FactureEdite.OrdreAffichage = j;
                        lstFactureEdite.Add(FactureEdite);
                    }
                }
                        
                    context.Dispose();
                    return lstFactureEdite;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsRedevance> ListeDesREdevance()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.EserviceProcedure.ListeDesREdevance();
                return Entities.GetEntityListFromQuery<CsRedevance>(dt);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<CsProduit> ListeDesProduits()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousProduit();
                return Entities.GetEntityListFromQuery<CsProduit>(dt);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<CsCategorieClient> ListeDeCategorieClient()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneCategorieClient();
                return Entities.GetEntityListFromQuery<CsCategorieClient>(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public decimal? RetourneSoldeADate(int fk_idCentre, string centre, string client, string ordre, DateTime? date)
        {
            try
            {
                return Galatee.Entity.Model.FonctionCaisse.RetourneSoldeClientDate(fk_idCentre,centre, client, ordre, date);                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal? RetourneSoldeClient(int fk_idcentre, string centre, string client, string ordre)
        {
            try
            {
                return Galatee.Entity.Model.FonctionCaisse.RetourneSoldeClient(fk_idcentre,centre, client, ordre);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region SMS

        public static List<CsSms> GetSmsByStatutEnvoiNombreEnvoi(int pStatutEnvoi, int pNombreEnvoi)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsSms>(EserviceProcedure.SMS_RETOURNEByStatutEnvoiNombreEnvoi(pStatutEnvoi, pNombreEnvoi));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteSms(CsSms entity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteSms(List<CsSms> entityCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteSms(List<CsSms> entityCollection, galadbEntities pCommand)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(entityCollection), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertSms(CsSms entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertSms(List<CsSms> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertSms(List<CsSms> pEntityCollection, galadbEntities pCommand)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(pEntityCollection), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CsSms entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(List<CsSms> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SMS>(Entities.ConvertObject<Galatee.Entity.Model.SMS, CsSms>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

    

}




