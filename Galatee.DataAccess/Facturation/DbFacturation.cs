using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.Reporting.WebForms;
using System.IO;
using Galatee.Tools;
using Galatee.Entity.Model;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    public class DbFacturation
    {

        public DbFacturation()
        {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                //ConnectionString = Session.GetSqlConnexionString();
                Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
                ConnectionString = Session.GetSqlConnexionString();

            }
            catch (Exception)
            {
                throw;
            }
        }
        SqlConnection sqlConnection;
        SqlConnection sqlConnectionAbo07;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;
        private string Abo07ConnectionString;

        #region Edition de factures

   
        public List<CsEvenement> ChargerEvenementLot(CsLotri leLots)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ChargerEvenementLot(leLots);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEnteteFacture> retourneFacturePourDuplicat(int idcentre, string centre, string client, string ordre, string periode, string numFacture)
        {
            try
            {
                List<CsEnteteFacture> listeFinal = new List<Structure.CsEnteteFacture>();
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ListeFactureABO07(idcentre, centre, client, ordre, periode, numFacture);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsEnteteFacture > ListeDesFacturesAnnulation(int idcentre, string centre, string client, string ordre, string periode, string numFacture)
        {
            try
            {
                List<CsEnteteFacture> listeFinal = new List<Structure.CsEnteteFacture>();
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.ListeFactureABO07(idcentre, centre, client, ordre, periode, numFacture);
                return  Entities.GetEntityListFromQuery <CsEnteteFacture>(dt);
                //foreach (CsEnteteFacture item in lesFacture.Where(t=>t.TOTFTTC> 0))
                //{
                //    if (FonctionCaisse.RetourneSoldeDocument(item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE, item.FACTURE, item.PERIODE) > 0)
                //        listeFinal.Add(item);
                //}
                //return lesFacture;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public List<CsEnteteFacture> FacturesPourAnnulation(int idcentre, string centre, string client, string ordre)
        {
            cn = new SqlConnection(Abo07ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_LISTEFACTURES_POURANNULATION";

            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = ordre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt); ;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }




        public bool VerifierPaiementFacture(CsEnteteFacture laFacture)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetournePaiementFacture(laFacture);
                List<CsLclient> Reglement = Entities.GetEntityListFromQuery<CsLclient >(dt);

                if (Reglement.Count != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsLafactureClient> ListeDesFacturesAnnulationGroupe(int idcentre, string centre, string client, string ordre, string periode, string numFacture)
        {
            try
            {
                List<CsLafactureClient> lstFactureEdite = new List<CsLafactureClient>();
                ABO07Entities ctx = new ABO07Entities();
                List<Galatee.Entity.Model.CENTFAC> dt = Galatee.Entity.Model.FacturationProcedure.ListeFactureABO07(idcentre, centre, client, ordre, periode, numFacture, ctx);
                foreach (CENTFAC item in dt)
                {
                    CsLafactureClient laFactureClient = new Structure.CsLafactureClient();
                    laFactureClient._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(item);
                    laFactureClient._LstProfact = Entities.ConvertObject<CsProduitFacture, Galatee.Entity.Model.CPROFAC>(item.CPROFAC.ToList());
                    laFactureClient._LstRedFact = Entities.ConvertObject<CsRedevanceFacture, Galatee.Entity.Model.CREDFAC>(item.CREDFAC.ToList());
                    lstFactureEdite.Add(laFactureClient);
                }
                ctx.Dispose();
                return lstFactureEdite;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ValiderAnnulationFacture(CsEnteteFacture laFacture)
        {
            try
            {
                galadbEntities contextTemp = new galadbEntities();
                int IdCoper = contextTemp.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperFact).PK_ID;
                int IdUser = contextTemp.ADMUTILISATEUR.FirstOrDefault(p => p.MATRICULE == laFacture.MATRICULE).PK_ID;
                int IdLibelleTop = contextTemp.LIBELLETOP.FirstOrDefault(p => p.CODE == Enumere.TopFacturation).PK_ID;
                List<LCLIENT> lstFactute = new List<LCLIENT>();
                LCLIENT laFactureAnnulationPaiement = new LCLIENT();
                List<TRANSCAISB> LeReglement = contextTemp.TRANSCAISB.Where(t => t.FK_IDCENTRE == laFacture.FK_IDCENTRE &&
                                                                               t.CENTRE == laFacture.CENTRE &&
                                                                               t.CLIENT == laFacture.CLIENT &&
                                                                               t.ORDRE == laFacture.ORDRE &&
                                                                               t.REFEM == laFacture.PERIODE &&
                                                                               t.NDOC == laFacture.FACTURE ).ToList();
                if (LeReglement != null && LeReglement.Count != 0)
                {
                    COPER  Coper = contextTemp.COPER.FirstOrDefault(p => p.CODE == Enumere.CoperAnnulationTransaction);
                    LCLIENT laFactureAnnule = contextTemp.LCLIENT.FirstOrDefault(t => t.FK_IDCENTRE == laFacture.FK_IDCENTRE &&
                                                                               t.CENTRE == laFacture.CENTRE &&
                                                                               t.CLIENT == laFacture.CLIENT &&
                                                                               t.ORDRE == laFacture.ORDRE &&
                                                                               t.REFEM == laFacture.PERIODE &&
                                                                               t.NDOC == laFacture.FACTURE);

                    CsLclient leFactureAnnulation = Entities.ConvertObject<CsLclient, LCLIENT>(laFactureAnnule);
                    leFactureAnnulation.FK_IDCLIENT = laFacture.FK_IDCLIENT;
                    leFactureAnnulation.FK_IDLIBELLETOP = IdLibelleTop;
                    leFactureAnnulation.FK_IDCOPER = Coper.PK_ID;
                    leFactureAnnulation.COPER = Coper.CODE;
                    leFactureAnnulation.MONTANT = LeReglement.Sum(y=>y.MONTANT);
                    leFactureAnnulation.FK_IDADMUTILISATEUR = IdUser;
                    leFactureAnnulation.USERCREATION = laFacture.MATRICULE;
                    lstFactute.Add(Galatee.Tools.Utility.RetourneCopyObjet<LCLIENT>(Entities.ConvertObject<LCLIENT, CsLclient>(leFactureAnnulation)));

                    LCLIENT laFactureNaf = new DBEncaissement().RetourneFactureNaf(leFactureAnnulation);
                    laFactureNaf.ACQUIT = new DBEncaissement().RetourneNumFactureNaf(leFactureAnnulation.FK_IDCENTRE);

                    List<TRANSCAISB> transcais = new List<TRANSCAISB>();
                    TRANSCAISB transcaiss = Entities.ConvertObject<TRANSCAISB, TRANSCAISB>(LeReglement.First());
                    transcaiss.NDOC = laFactureNaf.ACQUIT;
                    transcais.Add(transcaiss);
                    laFactureNaf.TRANSCAISB = transcais;

                    lstFactute.Add(laFactureNaf);

                }

                CsLafactureClient leDetailFact = FacturationProcedure.RetourneFactureDetailPourAnnulation(laFacture);
                leDetailFact._LeEntatfac.TOTFHT = -1 * leDetailFact._LeEntatfac.TOTFHT;
                leDetailFact._LeEntatfac.TOTFTAX = -1 * leDetailFact._LeEntatfac.TOTFTAX;
                leDetailFact._LeEntatfac.TOTFTTC = -1 * leDetailFact._LeEntatfac.TOTFTTC;
                leDetailFact._LeEntatfac.COPER = Enumere.CoperAnnulationFacture;
                leDetailFact._LeEntatfac.LOTRI =leDetailFact._LeEntatfac.CENTRE + Enumere.FactureAnnulatinIndex;
                leDetailFact._LeEntatfac.PK_ID = 0;
                leDetailFact._LeEntatfac.DATECREATION = System.DateTime.Now;
                contextTemp.Dispose();

                foreach (CsProduitFacture item in leDetailFact._LstProfact )
                {
                    item.TOTPROHT = -1 * item.TOTPROHT;
                    item.TOTPROTAX = -1 * item.TOTPROTAX;
                    item.TOTPROTTC = -1 * item.TOTPROTTC;
                    item.LOTRI = item.CENTRE + Enumere.FactureAnnulatinIndex;

                    item.PK_ID = 0;
                    item.DATECREATION = System.DateTime.Now;
                }
                foreach (CsRedevanceFacture item in leDetailFact._LstRedFact )
                {
                    item.TOTREDHT = -1 * item.TOTREDHT;
                    item.TOTREDTAX = -1 * item.TOTREDTAX;
                    item.TOTREDTTC = -1 * item.TOTREDTTC;
                    item.LOTRI = item.CENTRE + Enumere.FactureAnnulatinIndex;
                    item.PK_ID = 0;
                    item.DATECREATION = System.DateTime.Now;

                }
                CENTFAC laFactureAbo07 = new CENTFAC();
                laFactureAbo07 = Entities.ConvertObject<CENTFAC, CsEnteteFacture>(leDetailFact._LeEntatfac);
                laFactureAbo07.CPROFAC = Entities.ConvertObject<CPROFAC, CsProduitFacture>(leDetailFact._LstProfact.ToList());
                laFactureAbo07.CREDFAC = Entities.ConvertObject<CREDFAC, CsRedevanceFacture>(leDetailFact._LstRedFact.ToList());


                ENTFAC laFacturegaladb = new ENTFAC();
                laFacturegaladb = Entities.ConvertObject<ENTFAC, CsEnteteFacture>(leDetailFact._LeEntatfac );
                laFacturegaladb.PROFAC = Entities.ConvertObject<PROFAC, CsProduitFacture>(leDetailFact._LstProfact .ToList());
                laFacturegaladb.REDFAC = Entities.ConvertObject<REDFAC, CsRedevanceFacture >(leDetailFact._LstRedFact .ToList());


                LCLIENT lclient = new LCLIENT();
                lclient.CENTRE = laFacture.CENTRE;
                lclient.CLIENT = laFacture.CLIENT;
                lclient.ORDRE = laFacture.ORDRE;
                lclient.REFEM = laFacture.PERIODE;
                lclient.NDOC = laFacture.FACTURE;
                lclient.COPER = Enumere.CoperAnnulationFacture ;
                lclient.DENR = Convert.ToDateTime(laFacture.DFAC);
                lclient.MONTANT =-1 * laFacture.TOTFTTC;
                lclient.MONTANTTVA = -1 * laFacture.TOTFTAX;
                lclient.ORIGINE = laFacture.CENTRE;
                lclient.ECART = 0;
                lclient.DC = "D";
                lclient.MOISCOMPT = laFacture.MOISCOMPTA;
                lclient.TOP1 = "1";
                //lclient.EXIGIBILITE = leDetailFact.e
                lclient.MATRICULE = laFacture.MATRICULE;
                lclient.FK_IDCLIENT = laFacture.FK_IDCLIENT;
                lclient.FK_IDCOPER = IdCoper;
                lclient.FK_IDCENTRE = laFacture.FK_IDCENTRE;
                lclient.FK_IDADMUTILISATEUR = IdUser;
                lclient.FK_IDLIBELLETOP = IdLibelleTop;
                lclient.DATEMODIFICATION = System.DateTime.Now;
                lclient.DATECREATION = System.DateTime.Now;
                lclient.USERCREATION = laFacture.MATRICULE;
                lclient.USERMODIFICATION = laFacture.MATRICULE;
                lstFactute.Add(lclient);

                int resultInsert = -1;
                galadbEntities ctx1 = new galadbEntities();
                ABO07Entities ctx2 = new ABO07Entities();

                List<int> lstIdEvenet = leDetailFact._LstProfact.Select(t=>t.FK_IDEVENEMENT.Value ).ToList();
                List<Galatee.Entity.Model.EVENEMENT> lstEvt = ctx1.EVENEMENT.Where(y=>lstIdEvenet.Contains(y.PK_ID)).ToList();
                lstEvt.ForEach(y=>y.STATUS =Enumere.EvenementSupprimer);
                Entities.InsertEntity<LCLIENT>(lstFactute, ctx1);
                resultInsert = ctx1.SaveChanges();
                if (resultInsert != -1)
                {
                    Entities.InsertEntityAbo07<CENTFAC>(laFactureAbo07, ctx2);
                    ctx2.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LCLIENT RetourneFactureDemande(TRANSCAISB  FactureAregle)
        {
            try
            {
                LCLIENT laFactureDemande = new LCLIENT();
                laFactureDemande.CENTRE = FactureAregle.CENTRE;
                laFactureDemande.CLIENT = FactureAregle.CLIENT;
                laFactureDemande.ORDRE = FactureAregle.ORDRE;
                laFactureDemande.REFEM = FactureAregle.REFEM;
                laFactureDemande.NDOC = FactureAregle.NDOC;
                laFactureDemande.COPER = FactureAregle.COPER;
                laFactureDemande.DENR = System.DateTime.Today ;
                laFactureDemande.EXIG = 1;
                laFactureDemande.MONTANT = FactureAregle.MONTANT;
                laFactureDemande.DC = Enumere.Debit ;
                laFactureDemande.MOISCOMPT = System.DateTime.Today.Year.ToString()+ System.DateTime.Today.Month.ToString("00");
                laFactureDemande.TOP1 = FactureAregle.TOP1;
                laFactureDemande.EXIGIBILITE = System.DateTime.Today ;
                laFactureDemande.DATEVALEUR = FactureAregle.DATEVALEUR;
                laFactureDemande.ACQUIT = FactureAregle.ACQUIT;
                laFactureDemande.MATRICULE = FactureAregle.MATRICULE;
                laFactureDemande.DATEFLAG = FactureAregle.DATEFLAG;
                laFactureDemande.MONTANTTVA = 0;
                laFactureDemande.NUMCHEQ = FactureAregle.NUMCHEQ;
                laFactureDemande.USERCREATION = FactureAregle.USERCREATION;
                laFactureDemande.DATECREATION = FactureAregle.DATECREATION;
                laFactureDemande.DATEMODIFICATION = FactureAregle.DATEMODIFICATION;
                laFactureDemande.USERMODIFICATION = FactureAregle.USERMODIFICATION;
                laFactureDemande.BANQUE = FactureAregle.BANQUE;
                laFactureDemande.GUICHET = FactureAregle.GUICHET;
                laFactureDemande.FK_IDCENTRE = FactureAregle.FK_IDCENTRE;
                laFactureDemande.POSTE = FactureAregle.POSTE;

                return laFactureDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> RetourneListeDeJets(string LotRI)
        { try
            {
                try
                {
                    return Galatee.Entity.Model.FacturationProcedure.RetourneListeDeJets(LotRI);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " :" + ex.Message);
            }
        }

        public List<String> RetourneListeDePeriodes()
        {
                  
            try
            {
                try
                {                    
                    return Galatee.Entity.Model.FacturationProcedure.RetourneListeDePeriodes();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " :" + ex.Message);
            }
           
        }


        public List<CsEnteteFacture> RetourneClientDuneBorne(string centre, string client, string lotRi, string periode)
        {
            try
            {
                try
                {
                    DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetourneClientDuneBorne(centre, client, lotRi, periode);
                    return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " :" + ex.Message);
            }
        }

        public List<CsFactureClient> RetourneFacturesRegroupement( List<string> regroupement, List<string> LstPeriode, List<string> Produit, string rdlc)
        {
            try
            {

                DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetourneEntfacRegroupement(regroupement,LstPeriode, Produit);
                List<CsEnteteFacture> lstEnteteFacture = Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);

                DataTable dts = Galatee.Entity.Model.FacturationProcedure.RetourneProfactRegroupement(regroupement, LstPeriode, Produit);
                List<CsProduitFacture> lstProduitFacture = Entities.GetEntityListFromQuery<CsProduitFacture>(dts);

                DataTable dtss = Galatee.Entity.Model.FacturationProcedure.RetourneRedfactRegroupement(regroupement, LstPeriode, Produit);
                List<CsRedevanceFacture> lstRedevanceFacture = Entities.GetEntityListFromQuery<CsRedevanceFacture>(dtss);

                List<CsFactureClient> resultat = new List<Structure.CsFactureClient>();
                if (rdlc != "FactureRegroupe")
                {
                foreach (string item in Produit)
                {
                    if (lstEnteteFacture.Where(t => t.PRODUIT == item).ToList().Count == 0) continue;
                    if (item == Enumere.ElectriciteMT)
                    {
                        if (rdlc == "FactureSimple")
                            resultat.AddRange(EditionFactureMt(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                        else if (rdlc == "FactureDetail")
                            resultat.AddRange(EditionFactureBorderauxMT(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                    }
                    else
                    {

                        if (rdlc == "FactureSimple")
                        {
                            if (item == Enumere.Eau)
                                resultat.AddRange(EditionFactureEau(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                            else
                                resultat.AddRange(EditionFacture(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                        }
                        else if (rdlc == "FactureDetail")
                            resultat.AddRange(EditionFactureBorderaux(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                        else if (rdlc == "FactureSimpleO")
                            resultat.AddRange(EditionFactureEau(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                    }

                    if (rdlc == "BordereauSimple")
                        resultat.AddRange(EditionFactureBordereauSimple(lstEnteteFacture.Where(t => t.PRODUIT == item).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                }
                }
                if (rdlc == "FactureRegroupe")
                    resultat.AddRange(EditionFactureRegrouper(lstEnteteFacture, lstProduitFacture, lstRedevanceFacture));
                return resultat; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CsFactureClient> RetourneFacturesAbo07(List<CsEnteteFacture> lstClient, string rdlc)
        {
            try
            {
                
                List<CsEnteteFacture> lstEnteteFacture = this.RetourneEnteteFacturesAbo07Spx(lstClient.Select(p=>p.PK_ID).ToList());
                List<CsProduitFacture> lstProduitFacture = this.RetourneProfacFacturesAbo07Spx(lstClient.Select(p => p.PK_ID).ToList());
                List<CsRedevanceFacture> lstRedevanceFacture = this.RetourneRedfacFacturesAbo07Spx(lstClient.Select(p => p.PK_ID).ToList());

                if (lstProduitFacture.First().PRODUIT == Enumere.ElectriciteMT)
                {
                    if (rdlc == "FactureSimpleMT")
                        return EditionFactureMt(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    else if (rdlc == "FactureDetailMT")
                        return EditionFactureBorderauxMT(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture.OrderBy(o => o.POINT).ToList(), lstRedevanceFacture.OrderBy(o => o.REDEVANCE).ToList());
                }
                else
                {
                    if (rdlc == "FactureSimple")
                    {
                        if (lstProduitFacture.First().PRODUIT == Enumere.Eau)
                            return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                        else
                            return EditionFacture(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);

                    }
                    else if (rdlc == "FactureDetail")
                        return EditionFactureBorderaux(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    else if (rdlc == "FactureSimpleO")
                        return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                }
                if (rdlc == "BordereauSimple")
                    return EditionFactureBordereauSimple(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                else return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsFactureClient> RetourneFactures(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient, bool DejaMiseAjour, bool IsLotIsole, string rdlc)
        {
            try
            {

                List<CsEnteteFacture> lstEnteteFacture = RetourneEnteteFacturesEditionSpx(leLotSelect,laTournee,leClient,DejaMiseAjour,IsLotIsole);

                List<CsProduitFacture> lstProduitFacture = RetourneProfacFacturesEditionSpx(leLotSelect, laTournee, leClient, DejaMiseAjour, IsLotIsole);

                List<CsRedevanceFacture> lstRedevanceFacture = RetourneRedfacFacturesEditionSpx(leLotSelect, laTournee, leClient, DejaMiseAjour, IsLotIsole);

                if (leLotSelect.PRODUIT == Enumere.ElectriciteMT)
                {
                    if (rdlc == "FactureSimpleMT")
                        return EditionFactureMt(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    else if (rdlc == "FactureDetailMT")
                        return EditionFactureBorderauxMT(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(l=>l.POINT).ToList(), lstRedevanceFacture);
                }
                else
                {
                    if (rdlc == "FactureSimple")
                    {
                        if (leLotSelect.PRODUIT == Enumere.Eau) 
                            return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                        else
                            return EditionFacture(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    }
                    else if (rdlc == "FactureDetail")
                        return EditionFactureBorderaux(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    else if (rdlc == "FactureSimpleO")
                        return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);

                }
                if (rdlc == "BordereauSimple" || rdlc =="BordereauSimpleSGC")
                    return EditionFactureBordereauSimple(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                else return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsFactureClient> RetourneFacturesPeriode(List<CsCentre> lstIdcentre, string periodeDebut, string periodeFin, string centreTournee,
    string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
    string centreStop, string clientStop, string rdlc, bool IsLotIsole)
        {
            try
            {
                List<CsEnteteFacture> lstEnteteFacture = new List<Structure.CsEnteteFacture>();
                List<CsProduitFacture> lstProduitFacture = new List<Structure.CsProduitFacture>();
                List<CsRedevanceFacture> lstRedevanceFacture = new List<Structure.CsRedevanceFacture>();
                foreach (CsCentre item in lstIdcentre)
                {
                    lstEnteteFacture = RetourneEnteteFacturesEditionPeriodeSpx(item,periodeDebut,periodeFin, centreTournee, tourneeDebut, tourneeFin,
                                    centreReprise, clientReprise, centreStop, clientStop, IsLotIsole);

                    lstProduitFacture = RetourneProfacFacturesEditionPeriodeSpx(item, periodeDebut, periodeFin, centreTournee, tourneeDebut, tourneeFin,
                         centreReprise, clientReprise, centreStop, clientStop, IsLotIsole);


                    lstRedevanceFacture = RetourneRedfacFacturesEditionPeriodeSpx(item, periodeDebut, periodeFin, centreTournee, tourneeDebut, tourneeFin,
                                         centreReprise, clientReprise, centreStop, clientStop, IsLotIsole);
                }
                List<string> lstProduit = lstProduitFacture.Select(t => t.PRODUIT).ToList();

                foreach (var item in lstProduit)
                {
                    if (item == Enumere.ElectriciteMT)
                    {
                        if (rdlc == "FactureSimpleMT")
                            return EditionFactureMt(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                        else if (rdlc == "FactureDetailMT")
                            return EditionFactureBorderauxMT(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    }
                    else
                    {
                        if (rdlc == "FactureSimple")
                        {
                            if (item == Enumere.Eau)
                                return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                            else 
                                return EditionFacture(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                        }
                        else if (rdlc == "FactureDetail")
                            return EditionFactureBorderaux(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                        else if (rdlc == "FactureSimpleO")
                            return EditionFactureEau(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                    } 
                }
                
                if (rdlc == "BordereauSimple" || rdlc == "BordereauSimpleSGC")
                    return EditionFactureBordereauSimple(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
                else return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsAnnomalie > RetourneAnnomalieFactures(CsLotri leLotSelect, string centreTournee,
    string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
    string centreStop, string clientStop, string periodeSelectionne,  bool IsLotIsole)
        {
            try
            {

                DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetourneAnnomalieFactures(leLotSelect, centreTournee, tourneeDebut, tourneeFin,
                                   centreReprise, clientReprise, centreStop, clientStop,
                                   periodeSelectionne, IsLotIsole);
                return  Entities.GetEntityListFromQuery<CsAnnomalie>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsFactureClient> EditionFactureResiliation(List<CsFactureBrut>lesFacture )
        {
            try
            {
                List<CsEntfac> lstEntfac = new List<Structure.CsEntfac>();
                if (lesFacture.FirstOrDefault().PRODUIT != Enumere.ElectriciteMT )
                    lstEntfac = new DBCalcul().ConstruireEntfac(lesFacture);
                else
                    lstEntfac = new DBCalcul().ConstruireEntfacMt(lesFacture);

                List<CsEnteteFacture> lstEnteteFacture = new List<CsEnteteFacture>();
                List<CsProduitFacture> lstProduitFacture = new List<CsProduitFacture>();
                List<CsRedevanceFacture> lstRedevanceFacture = new List<CsRedevanceFacture>();
                int i = 1;
                foreach (CsEntfac item in lstEntfac)
                {
                    item.PK_ID = i;
                    item.LstProfac.ForEach(t=>t.FK_IDENTFAC = i);
                    item.lstRedfac.ForEach(t => t.FK_IDENTFAC = i);
                    lstEnteteFacture.Add(Utility.ConvertEntity<CsEnteteFacture, CsEntfac>(item)); 
                    lstProduitFacture.AddRange(item.LstProfac);
                    lstRedevanceFacture.AddRange(item.lstRedfac);
                }

                if (lesFacture.First().PRODUIT == Enumere.ElectriciteMT)
                    return EditionFactureMt(lstEnteteFacture, lstProduitFacture, lstRedevanceFacture);
                else if (lesFacture.First().PRODUIT == Enumere.Eau)
                    return EditionFactureEau(lstEnteteFacture, lstProduitFacture, lstRedevanceFacture);
                else
                    return EditionFacture(lstEnteteFacture, lstProduitFacture, lstRedevanceFacture);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<CsAnnomalie> RetourneControleFactures(CsLotri leLotSelect)
        {
            try
            {

                DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetourneControleFactures(leLotSelect);
                return Entities.GetEntityListFromQuery<CsAnnomalie>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CsFactureClient> EditionFactureBorderauxMT(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            galadbEntities context = new galadbEntities();
            List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
            List<TRANCHEREDEVANCE> _lstTrancheRedevence = context.TRANCHEREDEVANCE.ToList();
            List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
            List<CATEGORIECLIENT> _lstCategorie = context.CATEGORIECLIENT.ToList();
            List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
            List<COMMUNE> _lstCommune = context.COMMUNE.ToList();
            List<QUARTIER> _lstQuartier = context.QUARTIER.ToList();

            List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
            foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
            {

                CsLafactureClient entfacs = new CsLafactureClient();
                entfacs._LeEntatfac = item;
                entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                int i = 0;

                string LibelleCategorie = _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT) != null ? _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).CODE + " " + _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).LIBELLE : string.Empty;
                string LibelleRegroupent = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT) != null ? _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).CODE + " " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).NOM : string.Empty;
                string LibelleCommune = _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE) != null ? _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE).LIBELLE : string.Empty;
                string LibelleQurtier = _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER) != null ? _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER).LIBELLE : string.Empty;

                foreach (CsProduitFacture items in entfacs._LstProfact.OrderBy(u=>u.POINT ))
                {
                    CsFactureClient FactureEdite = new CsFactureClient();
                    FactureEdite.Centre = item.CENTRE;
                    FactureEdite.Client = item.CLIENT;
                    FactureEdite.Ordre = item.ORDRE;
                    FactureEdite.Produit = items.PRODUIT;

                    FactureEdite.Regcli = item.REGROUPEMENT;
                    FactureEdite.LibelleCateg = LibelleCategorie;
                    FactureEdite.LibelleRegcli= LibelleRegroupent;
                    FactureEdite.Commune = LibelleCommune;
                    FactureEdite.Quartier = LibelleQurtier;

                    FactureEdite.Tournee = item.TOURNEE;
                    FactureEdite.OrdTour = item.ORDTOUR;
                    FactureEdite.NomAbon = item.NOMABON;
                    FactureEdite.Adrmand1 = item.ADRMAND1;
                    FactureEdite.Adrmand2 = item.ADRMAND2;
                    FactureEdite.Etage = item.ETAGE;
                    FactureEdite.Porte = item.PORTE;
                    FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                    FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                    FactureEdite.Periode = item.PERIODE;
                    FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                    FactureEdite.TotFht = item.TOTFHT;
                    FactureEdite.TotFTax = item.TOTFTAX;
                    FactureEdite.TotFTTC = item.TOTFTTC;
                    FactureEdite.Facture = item.FACTURE;
                    FactureEdite.Lotri = item.LOTRI;

                    // Profac

                    FactureEdite.LibelleProduit = items.LIBELLEPRODUIT;
                    FactureEdite.NIndex = items.NINDEX;
                    FactureEdite.AIndex = items.AINDEX;
                    FactureEdite.ConsoFac = items.CONSO ;
                    FactureEdite.Diametre = items.PUISSANCE.ToString();
                    FactureEdite.Compteur = items.COMPTEUR;
                    FactureEdite.Point = items.POINT.ToString();
                    FactureEdite.DateReleve = entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                    FactureEdite.DateRelevePrec = Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString();
                    FactureEdite.OrdreAffichage = i;
                    i++;
                    lstFactureEdite.Add(FactureEdite);
                }
                foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(t => t.REDEVANCE).ThenBy(p => p.TRANCHE))
                {
                    CsFactureClient FactureEdite = new CsFactureClient();
                    //Entfac
                    List<TRANCHEREDEVANCE> lstrancheRed = _lstTrancheRedevence.Where(x => x.REDEVANCE.CODE == itemss.REDEVANCE && x.REDEVANCE.PRODUIT == itemss.PRODUIT).ToList();
                    FactureEdite.Centre = item.CENTRE;
                    FactureEdite.Client = item.CLIENT;
                    FactureEdite.Ordre = item.ORDRE;

                    FactureEdite.Produit = entfacs._LstProfact.First().PRODUIT ;
                    FactureEdite.Regcli = item.REGROUPEMENT;
                    FactureEdite.LibelleCateg = LibelleCategorie;
                    FactureEdite.LibelleRegcli = LibelleRegroupent;
                    FactureEdite.Commune = LibelleCommune;
                    FactureEdite.Quartier = LibelleQurtier;

                    FactureEdite.Tournee = item.TOURNEE;
                    FactureEdite.OrdTour = item.ORDTOUR;
                    FactureEdite.NomAbon = item.NOMABON;
                    FactureEdite.Adrmand1 = item.ADRMAND1;
                    FactureEdite.Adrmand2 = item.ADRMAND2;
                    FactureEdite.Etage = item.ETAGE;
                    FactureEdite.Porte = item.PORTE;
                    FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                    FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                    FactureEdite.Periode = item.PERIODE;
                    FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                    FactureEdite.TotFht = item.TOTFHT;
                    FactureEdite.TotFTax = item.TOTFTAX;
                    FactureEdite.TotFTTC = item.TOTFTTC;
                    FactureEdite.Facture = item.FACTURE;
                    FactureEdite.Lotri = item.LOTRI;

                    // Profac
                    FactureEdite.NIndex = 0;
                    FactureEdite.AIndex = 0;
                    FactureEdite.Diametre = "0";
                    FactureEdite.Point = itemss.REDEVANCE;
                    FactureEdite.DateReleve = entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                    FactureEdite.DateRelevePrec = Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString();

                    //Redfac
                    byte Ordre = System.Convert.ToByte(itemss.TRANCHE);
                    if (itemss.UNITE == Enumere.CODEUNITE)
                        FactureEdite.Compteur = _lstRedevence.FirstOrDefault(t => t.CODE == itemss.REDEVANCE).LIBELLE;
                    else
                        FactureEdite.Compteur = _lstTrancheRedevence.FirstOrDefault(x => x.REDEVANCE.CODE == itemss.REDEVANCE && x.REDEVANCE.PRODUIT == itemss.PRODUIT && x.ORDRE == Ordre).LIBELLE.Trim();
                    FactureEdite.ConsoFac = itemss.QUANTITE;
                    FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                    FactureEdite.BarPrix = itemss.BARPRIX;
                    FactureEdite.TotRedHT = itemss.TOTREDHT;
                    FactureEdite.TotRedTax = itemss.TOTREDTAX;
                    FactureEdite.TotRedTTC = itemss.TOTREDTTC;
                    FactureEdite.OrdreAffichage = i;
                    i++;
                    lstFactureEdite.Add(FactureEdite);
                    

            }
            }
            context.Dispose();
            return lstFactureEdite;
        }
        public List<CsFactureClient> EditionFactureBorderaux(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            galadbEntities context = new galadbEntities();
            List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
            List<TRANCHEREDEVANCE> _lstTrancheRedevence = context.TRANCHEREDEVANCE.ToList();
            List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
            List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
            List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
            //foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => t.TOTFTTC != 0).ToList())
            foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
            {

                CsLafactureClient entfacs = new CsLafactureClient();
                entfacs._LeEntatfac = item;
                entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                foreach (CsProduitFacture items in entfacs._LstProfact)
                {
                    int i = 0;
                    foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(t => t.REDEVANCE).ThenBy(p => p.TRANCHE))
                    {
                        CsFactureClient FactureEdite = new CsFactureClient();
                        //Entfac
                        List<TRANCHEREDEVANCE> lstrancheRed = _lstTrancheRedevence.Where(x => x.REDEVANCE.CODE == itemss.REDEVANCE && x.REDEVANCE.PRODUIT == itemss.PRODUIT).ToList();
                        FactureEdite.Centre = item.CENTRE;
                        FactureEdite.Client = item.CLIENT;
                        FactureEdite.Ordre = item.ORDRE;
                        FactureEdite.Produit = items.PRODUIT;

                        FactureEdite.Tournee = item.TOURNEE;
                        FactureEdite.OrdTour = item.ORDTOUR;
                        FactureEdite.NomAbon = item.NOMABON;
                        FactureEdite.Adrmand1 = item.ADRMAND1;
                        FactureEdite.Adrmand2 = item.ADRMAND2;
                        FactureEdite.Etage = item.ETAGE;
                        FactureEdite.Porte = item.PORTE;
                        FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                        FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                        FactureEdite.Periode = item.PERIODE;
                        FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                        FactureEdite.TotFht = item.TOTFHT;
                        FactureEdite.TotFTax = item.TOTFTAX;
                        FactureEdite.TotFTTC = item.TOTFTTC;
                        FactureEdite.Facture = item.FACTURE;
                        FactureEdite.Lotri = item.LOTRI;
                        FactureEdite.Ag  = items.TYPECOMPTAGE ;

                        // Profac
                        FactureEdite.LibelleProduit = items.LIBELLEPRODUIT;
                        FactureEdite.NIndex = items.NINDEX;
                        FactureEdite.AIndex = items.AINDEX;
                        FactureEdite.ConsoFac = items.CONSOFAC;
                        FactureEdite.Diametre = items.PUISSANCE.ToString();
                        FactureEdite.Compteur = items.COMPTEUR;
                        FactureEdite.Point  = items.POINT.ToString();
                        FactureEdite.DateReleve = entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                        FactureEdite.DateRelevePrec = Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString();

                        //Redfac
                        byte Ordre = System.Convert.ToByte(itemss.TRANCHE);
                        if (itemss.UNITE == Enumere.CODEUNITE)
                            FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(t => t.CODE == itemss.REDEVANCE).LIBELLE;
                        else
                        {
                            TRANCHEREDEVANCE latranche = _lstTrancheRedevence.FirstOrDefault(x => x.REDEVANCE.CODE == itemss.REDEVANCE && x.REDEVANCE.FK_IDPRODUIT == itemss.FK_IDPRODUIT && x.ORDRE == Ordre);
                            if (latranche != null)
                                FactureEdite.LibelleTranche = latranche.LIBELLE;
                        }
                        FactureEdite.Quantite = itemss.QUANTITE;
                        FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                        FactureEdite.BarPrix = itemss.BARPRIX;
                        FactureEdite.TotRedHT = itemss.TOTREDHT;
                        FactureEdite.TotRedTax = itemss.TOTREDTAX;
                        FactureEdite.TotRedTTC = itemss.TOTREDTTC;
                        FactureEdite.Redevance = itemss.REDEVANCE;
                        if (!string.IsNullOrEmpty(item.REGROUPEMENT))
                        {
                            REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT);
                            if (leRegroup != null)
                            {
                                FactureEdite.Regcli = leRegroup.CODE;
                                FactureEdite.LibelleRegcli = leRegroup.NOM;
                            }
                        }
                        FactureEdite.OrdreAffichage = i;
                        i++;
                        lstFactureEdite.Add(FactureEdite);
                    }
                   
                }
            }
            context.Dispose();
            return lstFactureEdite;
        }
        public List<CsFactureClient> EditionFactureMt(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            galadbEntities context = new galadbEntities();
            List<TYPECOMPTEUR> _lstTypeCompteur = context.TYPECOMPTEUR.ToList();
            List<TYPECOMPTAGE > _lstTypeComptage = context.TYPECOMPTAGE.ToList();
            List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
            List<CATEGORIECLIENT > _lstCategorie = context.CATEGORIECLIENT .ToList();
            List<TRANCHEREDEVANCE> _lstTrancheRedevence = context.TRANCHEREDEVANCE.ToList();
            List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
            List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
            List<COMMUNE> _lstCommune = context.COMMUNE.ToList();
            List<QUARTIER> _lstQuartier = context.QUARTIER.ToList();

            List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
            List<Galatee.Entity.Model.FRAISTIMBRE> _lstTimbre = context.FRAISTIMBRE.ToList();

            List<string> lstTypeCompteEdite = (new string[] { Enumere.ACTIF_HPLEINES, Enumere.ACTIF_HCREUSES, Enumere.ACTIF_HPOINTES }).ToList();
            List<string> lstTypeRedevanceEdite = (new string[] { Enumere.REDEVANCEENTRETIEN ,Enumere.REDEVANCEDEPASSEMENT , Enumere.REDEVANCEMAJORATION , Enumere.REDEVANCEPRIMEFIXE  }).ToList();
            foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
            {
                List<CsFactureClient> lstFactureResume = new List<CsFactureClient>();

                CsLafactureClient entfacs = new CsLafactureClient();
                entfacs._LeEntatfac = item;
                entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();

                //List<CsLclient> lstImpaye = new DBEncaissement().RetourneListeFactureNonSolde(item.CENTRE, item.CLIENT, item.ORDRE, item.FK_IDCLIENT);
                Galatee.Entity.Model.FRAISTIMBRE leFraistimbre = _lstTimbre.FirstOrDefault(p => item.TOTFTTC >= p.BORNEINF && item.TOTFTTC <= (p.BORNESUP + p.FRAIS));
                bool IsForfait = false ;
                if (entfacs._LstProfact.FirstOrDefault(u => u.TFAC == "2" || u.TFAC == "6") != null)
                    IsForfait = true;

                string LibelleCategorie = _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT) != null ? "Catégorie : " + _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).CODE  +"  " +_lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).LIBELLE : string.Empty;
                string LibelleRegroupent = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT) != null ? "Regroupement : " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).CODE + " " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).NOM : string.Empty;
                string LibelleCommune = _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE) != null ? _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE).LIBELLE : string.Empty;
                string LibelleQurtier = _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER) != null ? _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER).LIBELLE : string.Empty;

                CsEnteteFacture leEnteteFac = new Structure.CsEnteteFacture();
                leEnteteFac = GenereCodeBar(item);

                CsFactureClient FactureEditeInit = new CsFactureClient();
                FactureEditeInit.Centre = item.CENTRE;
                FactureEditeInit.Client = item.CLIENT;
                FactureEditeInit.Ordre = item.ORDRE;
                FactureEditeInit.idEntfac  = item.PK_ID ;
                FactureEditeInit.Produit = lstProduitFacture.First().PRODUIT ;

                FactureEditeInit.DenAbon = leEnteteFac.DENABON;
                FactureEditeInit.Denmand = leEnteteFac.DENMAND;

                FactureEditeInit.LibelleCateg = LibelleCategorie;
                FactureEditeInit.LibelleRegcli = LibelleRegroupent;
                FactureEditeInit.Commune = LibelleCommune;
                FactureEditeInit.Quartier = LibelleQurtier;

                FactureEditeInit.Regcli = item.REGROUPEMENT;
                FactureEditeInit.Cpos = IsForfait==true ? "1" : string.Empty ;

                FactureEditeInit.Lotri  = item.LOTRI ;
                FactureEditeInit.Tournee = item.TOURNEE;
                FactureEditeInit.OrdTour = item.ORDTOUR;
                FactureEditeInit.NomAbon = item.NOMABON;
                FactureEditeInit.Adrmand1 = item.ADRMAND1;
                FactureEditeInit.Adrmand2 = item.ADRMAND2;
                FactureEditeInit.Etage = item.ETAGE;
                FactureEditeInit.Porte = item.PORTE;
                FactureEditeInit.LibelleCEntre = item.LIBELLECENTRE;
                FactureEditeInit.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                FactureEditeInit.Periode = item.PERIODE;
                FactureEditeInit.Facture = item.FACTURE;
                FactureEditeInit.CodeOperation = item.COPER;
                FactureEditeInit.Regcli  = item.REGROUPEMENT ;
                FactureEditeInit.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();

                FactureEditeInit.DateReleve = entfacs._LstProfact.First().DATEEVT == null ? string.Empty : entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                FactureEditeInit.DateRelevePrec = entfacs._LstProfact.First().DATEEVT != null ? Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString() : string.Empty ;
                FactureEditeInit.TotFht = item.TOTFHT;
                FactureEditeInit.TotFTax = item.TOTFTAX;
                FactureEditeInit.TotFTTC = item.TOTFTTC;
                FactureEditeInit.TotRedTTC = item.TOTFTTC;
                if (leFraistimbre != null)
                    FactureEditeInit.MontantTimbre = leFraistimbre.FRAIS.Value;
                else
                    FactureEditeInit.MontantTimbre = 0;
                FactureEditeInit.TypeEdition = "E";
                FactureEditeInit.PuissanceSouscrite = entfacs._LstProfact.First().PUISSANCE.ToString();
                FactureEditeInit.PuissanceInstalle = entfacs._LstProfact.First().PUISSANCEINSTALLEE.ToString();

                CsFactureClient FactureProfactEdite = new CsFactureClient();
                CsQtFacMt lesConso = ConsoParTypeCompteurMt(entfacs._LstProfact, _lstTypeComptage);

                FactureEditeInit.PertesActives = lesConso.PertesActives;
                FactureEditeInit.PertesReactives = lesConso.PertesReactives;
                FactureEditeInit.TotActiveAvecPertes = lesConso.TotActiveAvecPertes;
                FactureEditeInit.TotReactiveAvecPertes = lesConso.TotReactiveAvecPertes;
                FactureEditeInit.TanPhi = lesConso.TanPhi;
                FactureEditeInit.WaMa = lesConso.WaMa;
                FactureEditeInit.WrMr = lesConso.WrMr;

                foreach (CsProduitFacture items in entfacs._LstProfact)
                {
                    //if (!lstTypeCompteEdite.Contains(items.TYPECOMPTEUR)) continue;
                    FactureProfactEdite.Centre = item.CENTRE;
                    FactureProfactEdite.Client = item.CLIENT;
                    FactureProfactEdite.Ordre = item.ORDRE;
                    FactureProfactEdite.Produit = items.PRODUIT;
                    FactureProfactEdite.idEntfac = item.PK_ID;

                    FactureProfactEdite.Regcli  = item.REGROUPEMENT ;
                    FactureProfactEdite.LibelleCateg = LibelleCategorie;
                    FactureProfactEdite.LibelleRegcli = LibelleRegroupent;
                    FactureProfactEdite.Commune = LibelleCommune;
                    FactureProfactEdite.Quartier = LibelleQurtier;
                    FactureProfactEdite.Cpos = IsForfait == true ? "1" : string.Empty;

                    FactureProfactEdite.Lotri = item.LOTRI;
                    FactureProfactEdite.Compteur = "N° " + items.COMPTEUR.Substring(4,items.COMPTEUR.Length -4);

                    
                    if (items.TYPECOMPTEUR == Enumere.ACTIF_HPOINTES)
                    {
                        FactureProfactEdite.NIndexPointes = items.NINDEX;
                        FactureProfactEdite.AIndexPointes = items.AINDEX;
                        FactureProfactEdite.CeofPointe = items.COEFLECT;
                        FactureProfactEdite.ConsomationPointes = items.CONSO;
                        FactureEditeInit.ConsoActiveHPointesAfterPertes = items.CONSOFAC;

                    }
                    if (items.TYPECOMPTEUR == Enumere.ACTIF_HPLEINES )
                    {
                        FactureProfactEdite.NIndexPleine = items.NINDEX;
                        FactureProfactEdite.AIndexPleine = items.AINDEX;
                        FactureProfactEdite.CeofPleine = items.COEFLECT;
                        FactureProfactEdite.ConsomationPleine = items.CONSO;
                        FactureEditeInit.ConsoActiveHPleinesAfterPertes = items.CONSOFAC ;
                    }
                    if (items.TYPECOMPTEUR == Enumere.ACTIF_HCREUSES )
                    {
                        FactureProfactEdite.NIndexCreuse = items.NINDEX;
                        FactureProfactEdite.AIndexCreuse = items.AINDEX;
                        FactureProfactEdite.CeofCreuse = items.COEFLECT;
                        FactureProfactEdite.ConsomationCreuse = items.CONSO ;
                        FactureEditeInit.ConsoActiveHCreusesAfterPertes = items.CONSOFAC;

                    }
                    if (items.TYPECOMPTEUR == Enumere.HORAIRE)
                    {
                        FactureProfactEdite.NIndexHoraire = items.NINDEX;
                        FactureProfactEdite.AIndexHoraire = items.AINDEX;
                        FactureProfactEdite.CeofHoraire = items.COEFLECT;
                        FactureProfactEdite.ConsomationHoraire = items.CONSO ;
                    }
                    if (items.TYPECOMPTEUR == Enumere.REACTIF)
                    {
                        FactureProfactEdite.NIndexReactif= items.NINDEX;
                        FactureProfactEdite.AIndexReactif = items.AINDEX;
                        FactureProfactEdite.CeofReactif = items.COEFLECT;
                        FactureProfactEdite.ConsomationReactif = items.CONSO ;
                    }
                    if (items.TYPECOMPTEUR == Enumere.MAXIMETRE )
                        FactureEditeInit.ConsoMAximetre = items.CONSOFAC ;
                    
                    FactureProfactEdite.TypeEdition = "P";
                    FactureProfactEdite.TypeCompteur = _lstTypeCompteur.FirstOrDefault(t => t.PRODUIT == items.PRODUIT && t.CODE == items.TYPECOMPTEUR).LIBELLE;
                }
                lstFactureEdite.Add(FactureEditeInit);
                lstFactureEdite.Add(FactureProfactEdite);
                CsFactureClient FactureEditeResume = Utility.RetourneCopyObjet<CsFactureClient>(FactureProfactEdite);
                FactureEditeResume.TypeEdition = "R";
                lstFactureEdite.Add(FactureEditeResume);

                int i = 0;
                foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(t=>t.REDEVANCE))
                {
                    CsFactureClient FactureEdite = new CsFactureClient();
                    FactureEdite.Centre = item.CENTRE;
                    FactureEdite.Client = item.CLIENT;
                    FactureEdite.Ordre = item.ORDRE;
                    FactureProfactEdite.idEntfac = item.PK_ID;
                    FactureEdite.Produit = lstProduitFacture.First().PRODUIT;

                    FactureEdite.Regcli  = item.REGROUPEMENT ;
                    FactureEdite.LibelleCateg = LibelleCategorie;
                    FactureEdite.LibelleRegcli = LibelleRegroupent;
                    FactureEdite.Commune = LibelleCommune;
                    FactureEdite.Quartier = LibelleQurtier;

                    FactureEdite.Cpos = IsForfait == true ? "1" : string.Empty;

                    FactureEdite.Lotri = item.LOTRI ;
                    FactureEdite.Periode = item.PERIODE;
                    FactureEdite.TotFht = item.TOTFHT;
                    FactureEdite.TotFTax = item.TOTFTAX;
                    FactureEdite.TotFTTC = item.TOTFTTC;
                    byte Ordre = System.Convert.ToByte(itemss.TRANCHE);
                    FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(t => t.CODE == itemss.REDEVANCE).LIBELLE;
                    if (lstTypeRedevanceEdite.Contains(itemss.REDEVANCE))
                    {
                        FactureEdite.Quantite = 0;
                        FactureEdite.BarPrix = 0;
                    }
                    else
                    {
                        FactureEdite.Quantite = itemss.QUANTITE;
                        FactureEdite.BarPrix = itemss.BARPRIX;
                    }
                    FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                    FactureEdite.TotRedHT = itemss.TOTREDHT;
                    FactureEdite.TotRedTax = itemss.TOTREDTAX;
                    FactureEdite.TotRedTTC = itemss.TOTREDTTC;
                    FactureEdite.Bureau = (itemss.TAXE * 100).Value.ToString("#,0.") + "%";
                    if (leFraistimbre != null)
                        FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                    else
                        FactureEdite.MontantTimbre = 0;
                    FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                    FactureEdite.Facture = item.FACTURE;
                    FactureEdite.CodeOperation = item.COPER;

                    FactureEdite.TypeEdition = "D";
                    FactureEdite.OrdreAffichage = i;
                    lstFactureEdite.Add(FactureEdite);
                    i++;
                }
            }
            context.Dispose();
            return lstFactureEdite;
        }
        public List<CsFactureClient> EditionFactureBordereauSimple(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            galadbEntities context = new galadbEntities();
            List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
            List<CATEGORIECLIENT> _lstCategorie = context.CATEGORIECLIENT.ToList();
            List<Galatee.Entity.Model.FRAISTIMBRE> _lstTimbre = context.FRAISTIMBRE.ToList();
            List<COMMUNE> _lstCommune = context.COMMUNE.ToList();
            List<QUARTIER> _lstQuartier = context.QUARTIER.ToList();
            int? exigibilite = lstEnteteFacture.Where(y => y.EXIG != null).OrderByDescending(u => u.EXIG).First().EXIG;
            DateTime? dfact = lstEnteteFacture.Where(y => y.DFAC != null).OrderByDescending(u => u.DFAC).First().DFAC;
            string  exigib = Convert.ToDateTime(Convert.ToDateTime(dfact) + TimeSpan.FromDays((int)exigibilite)).ToShortDateString();

            List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
            int nbr = lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).Count();
            foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
            {
                List<CsFactureClient> lstFactureResume = new List<CsFactureClient>();

                CsLafactureClient entfacs = new CsLafactureClient();
                entfacs._LeEntatfac = item;
                entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();

                string LibelleCategorie = _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT) != null ? _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).CODE + " " + _lstCategorie.FirstOrDefault(t => t.CODE == item.CATEGORIECLIENT).LIBELLE : string.Empty;
                string LibelleRegroupent = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT) != null ? _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).CODE + " " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).NOM : string.Empty;
                string LibelleCommune = _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE) != null ? _lstCommune.FirstOrDefault(t => t.PK_ID == item.FK_IDCOMMUNE).LIBELLE : string.Empty;
                string LibelleQurtier = _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER) != null ? _lstQuartier.FirstOrDefault(t => t.PK_ID == item.FK_IDQUARTIER).LIBELLE : string.Empty;

                List<CsLclient> lstImpaye = new List<Structure.CsLclient>();

                CsFactureClient FactureEditeInit = new CsFactureClient();
                FactureEditeInit.Centre = item.CENTRE;
                FactureEditeInit.Client = item.CLIENT;
                FactureEditeInit.Ordre = item.ORDRE;
                FactureEditeInit.idEntfac = item.PK_ID;

                FactureEditeInit.Produit = entfacs._LstProfact.First().PRODUIT;
                FactureEditeInit.Regcli = item.REGROUPEMENT ;
                FactureEditeInit.LibelleRegcli = LibelleRegroupent;
                FactureEditeInit.LibelleCateg = LibelleCategorie;
                FactureEditeInit.Commune = LibelleCommune;
                FactureEditeInit.Quartier = LibelleQurtier;

                FactureEditeInit.Lotri =  entfacs._LstProfact.First().LOTRI ;
                FactureEditeInit.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                FactureEditeInit.Periode = FormatPeriodeMMAAAA(item.PERIODE);

              
                FactureEditeInit.Tournee = item.TOURNEE;
                FactureEditeInit.OrdTour = item.ORDTOUR;
                FactureEditeInit.NomAbon = item.NOMABON;
                FactureEditeInit.Adrmand1 = item.ADRMAND1;
                FactureEditeInit.Adrmand2 = item.ADRMAND2;
                FactureEditeInit.Facture = item.FACTURE;
                FactureEditeInit.AIndex = entfacs._LstProfact.First().AINDEX;
                FactureEditeInit.NIndex = entfacs._LstProfact.First().NINDEX;
                if (entfacs._LstProfact.First().PRODUIT == Enumere.ElectriciteMT)
                {
                    List<string> lstTypeCompteur = new List<string>() {Enumere.ACTIF_HPOINTES , Enumere.ACTIF_HPLEINES,Enumere.ACTIF_HCREUSES };
                    FactureEditeInit.Compteur = entfacs._LstProfact.First().COMPTEUR.Substring(5, entfacs._LstProfact.First().COMPTEUR.Length - 5);
                    FactureEditeInit.ConsoFac = entfacs._LstProfact.Where(t => lstTypeCompteur.Contains(t.TYPECOMPTEUR)).Sum(o=>o.CONSOFAC );
                }
                else
                    FactureEditeInit.Compteur = entfacs._LstProfact.First().COMPTEUR;
                FactureEditeInit.ConsoFac  = entfacs._LstProfact.First().CONSOFAC ;
                FactureEditeInit.TotFht = item.TOTFHT;
                FactureEditeInit.TotFTax = item.TOTFTAX;
                FactureEditeInit.TotFTTC = item.TOTFTTC;
                FactureEditeInit.TotRedTTC = item.TOTFTTC;
                FactureEditeInit.TypeEdition = "R";
                lstFactureEdite.Add(FactureEditeInit);
            }
            context.Dispose();
            lstFactureEdite.ForEach(t => t.dateExige = exigib);
            return lstFactureEdite;
        }



        public List<CsFactureClient> EditionFactureEau(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            string leClient = string.Empty;
            try
            {
                galadbEntities context = new galadbEntities();
                List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
                List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
                List<CENTRE> _lstCentre = context.CENTRE.ToList();
                List<PRODUIT> _lstProduit = context.PRODUIT.ToList();
                List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
                List<MESSAGE> _lstMessage = context.MESSAGE.ToList();
                List<COMMUNE> _lstCommune = context.COMMUNE.ToList();
                List<QUARTIER> _lstQuartier = context.QUARTIER.ToList();
                List<CATEGORIECLIENT> _lstCategorie = context.CATEGORIECLIENT.ToList();
                List<DENOMINATION> _lstDenomination = context.DENOMINATION.ToList();
                List<Galatee.Entity.Model.FRAISTIMBRE> _lstTimbre = context.FRAISTIMBRE.ToList();
                List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();

                foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
                {
                    List<CsFactureClient> lstFactureEditeParClient = new List<CsFactureClient>();
                    List<CsFactureClient> lstFactureResume = new List<CsFactureClient>();
                    leClient = item.CLIENT;
                    CsLafactureClient entfacs = new CsLafactureClient();
                    entfacs._LeEntatfac = item;
                    entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();

                    string civilite = item.DENABON;


                    if (entfacs._LstProfact.Count == 0) continue;

                    if (string.IsNullOrEmpty(item.LIBELLECENTRE) && item.FK_IDCENTRE > 0)
                        item.LIBELLECENTRE = _lstCentre.FirstOrDefault(d => d.PK_ID == item.FK_IDCENTRE).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLECATEGORIE) && item.FK_IDCATEGORIECLIENT > 0)
                        item.LIBELLECATEGORIE = _lstCategorie.FirstOrDefault(d => d.PK_ID == item.FK_IDCATEGORIECLIENT).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLECOMMUNE) && item.FK_IDCOMMUNE > 0)
                        item.LIBELLECOMMUNE = _lstCommune.FirstOrDefault(d => d.PK_ID == item.FK_IDCOMMUNE).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLEQUARTIER) && item.FK_IDQUARTIER > 0)
                        item.LIBELLEQUARTIER = _lstQuartier.FirstOrDefault(d => d.PK_ID == item.FK_IDQUARTIER).LIBELLE;


                    bool IsForfait = false;
                    if (entfacs._LstProfact.FirstOrDefault(u => u.TFAC == "2" || u.TFAC == "6") != null)
                        IsForfait = true;

                    string LibelleCategorie = item.LIBELLECATEGORIE;
                    string LibelleRegroupent = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT) != null ? _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).CODE + " " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).NOM : string.Empty;
                    string LibelleCommune = item.LIBELLECOMMUNE;
                    string LibelleQurtier = item.LIBELLEQUARTIER;
                    item.MATRICULE = item.MATRICULE;

                    List<CsLclient> lstImpaye = new List<Structure.CsLclient>();
                    CsEnteteFacture leEnteteFac = new Structure.CsEnteteFacture();
                    leEnteteFac = GenereCodeBar(item);


                    Galatee.Entity.Model.FRAISTIMBRE leFraistimbre = _lstTimbre.FirstOrDefault(p => item.TOTFTTC >= p.BORNEINF && item.TOTFTTC <= (p.BORNESUP + p.FRAIS));
                    CsFactureClient FactureEditeInit = new CsFactureClient();
                    FactureEditeInit.Centre = item.CENTRE;
                    FactureEditeInit.Client = item.CLIENT;
                    FactureEditeInit.Ordre = item.ORDRE;
                    FactureEditeInit.Lotri = item.LOTRI;
                    FactureEditeInit.idEntfac = item.PK_ID;
                    if (!string.IsNullOrEmpty(item.MATRICULE))
                        FactureEditeInit.Matricule = "Matricule  : " + item.MATRICULE;

                    FactureEditeInit.DenAbon = leEnteteFac.DENABON;
                    FactureEditeInit.Denmand = leEnteteFac.DENMAND;
                    FactureEditeInit.Produit = lstProduitFacture.First().PRODUIT;

                    FactureEditeInit.Regcli = item.REGROUPEMENT;
                    FactureEditeInit.Cpos = IsForfait == true ? "1" : string.Empty;
                    FactureEditeInit.LibelleRegcli = LibelleRegroupent;
                    FactureEditeInit.LibelleCateg = LibelleCategorie;
                    FactureEditeInit.Commune = LibelleCommune;
                    FactureEditeInit.Quartier = LibelleQurtier;

                    FactureEditeInit.ISFACTURE = item.ISFACTUREEMAIL;
                    FactureEditeInit.ISSMS = item.ISFACTURESMS;
                    FactureEditeInit.TELEPHONE = item.TELEPHONE;
                    FactureEditeInit.EMAIL = item.EMAIL;

                    CENTRE leCentreClient = _lstCentre.FirstOrDefault(t => t.PK_ID == item.FK_IDCENTRE);
                    MESSAGE leMessage = _lstMessage.FirstOrDefault(t => t.SITE == leCentreClient.CODESITE && item.DFAC >= t.DEBUT_VALIDITE && item.DFAC <= t.FIN_VALIDITE);
                    if (leMessage != null)
                        FactureEditeInit.Mes = leMessage.TEXTE;

                    if (string.IsNullOrEmpty(item.REGROUPEMENT))
                    {

                        lstImpaye = new DBEncaissement().RetourneListeFactureNonSoldeSpx(item.CENTRE, item.CLIENT, item.ORDRE, item.FK_IDCENTRE);
                        //lstImpaye = new List<Structure.CsLclient>();
                        if (lstImpaye != null && lstImpaye.Count != 0)
                        {
                            CsLclient leFactureEstElleDejaMiseAJour = lstImpaye.FirstOrDefault(t => t.NDOC == item.FACTURE && t.REFEM == item.PERIODE);
                            if (leFactureEstElleDejaMiseAJour != null)
                            {
                                FactureEditeInit.SoldeTotFTTC = lstImpaye.Sum(o => o.SOLDEFACTURE);
                                lstImpaye = lstImpaye.Where(t => t.NDOC != item.FACTURE && t.REFEM != item.PERIODE).ToList();
                            }
                            else
                                FactureEditeInit.SoldeTotFTTC = lstImpaye.Sum(o => o.SOLDEFACTURE) + item.TOTFTTC;
                        }
                        else
                            FactureEditeInit.SoldeTotFTTC = item.TOTFTTC;
                    }
                    else
                    {
                        REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT);
                        if (leRegroup != null)
                            FactureEditeInit.Regcli = leRegroup.CODE + " " + leRegroup.NOM;
                        FactureEditeInit.SoldeTotFTTC = item.TOTFTTC;
                    }

                    FactureEditeInit.CodeOperation = Enumere.FactureGeneraleCoper;
                    FactureEditeInit.Tournee = item.TOURNEE;
                    FactureEditeInit.OrdTour = item.ORDTOUR;
                    //if (!string.IsNullOrEmpty(civilite) && _lstDenomination.FirstOrDefault(t => t.CODE == civilite) != null)
                    //    FactureEditeInit.NomAbon = _lstDenomination.FirstOrDefault(t => t.CODE == civilite).LIBELLE + " " + item.NOMABON;
                    //else
                        FactureEditeInit.NomAbon = item.NOMABON;
                    FactureEditeInit.Adrmand1 = item.ADRMAND1;
                    FactureEditeInit.Adrmand2 = item.ADRMAND2;
                    FactureEditeInit.Etage = item.ETAGE;
                    FactureEditeInit.Porte = item.PORTE;
                    FactureEditeInit.Rue = item.RUE;
                    FactureEditeInit.LibelleCEntre = item.LIBELLECENTRE;
                    FactureEditeInit.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                    FactureEditeInit.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                    FactureEditeInit.Facture = item.FACTURE;
                    FactureEditeInit.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                    FactureEditeInit.TotFht = item.TOTFHT;
                    FactureEditeInit.TotFTax = item.TOTFTAX;
                    FactureEditeInit.TotFTTC = item.TOTFTTC;
                    FactureEditeInit.TotRedTTC = item.TOTFTTC;
                    if (leFraistimbre != null)
                        FactureEditeInit.MontantTimbre = leFraistimbre.FRAIS.Value;
                    else
                        FactureEditeInit.MontantTimbre = 0;

                    FactureEditeInit.TypeEdition = "R";
                    FactureEditeInit.DateReleve = entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                    if (entfacs._LstRedFact != null && entfacs._LstRedFact.Count > 0)
                        FactureEditeInit.DateRelevePrec = Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString();
                    else
                        FactureEditeInit.DateRelevePrec = entfacs._LstProfact.First().DEVPRE.Value.ToShortDateString();
                    lstFactureEditeParClient.Add(FactureEditeInit);
                    lstFactureResume.Add(FactureEditeInit);

                    if (lstImpaye != null)  // Rapel impayes du client
                    {
                        int Nbrs = 0;
                        foreach (CsLclient itemimpayes in lstImpaye.OrderByDescending(t => t.REFEM))
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Lotri = item.LOTRI;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE;

                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;

                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.CodeOperation = item.COPER;
                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue = item.RUE;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.Periode = FormatPeriodeMMAAAA(itemimpayes.REFEM);
                            FactureEdite.Facture = itemimpayes.NDOC;
                            FactureEdite.TotRedTTC = itemimpayes.SOLDEFACTURE;
                            FactureEdite.dateExige = itemimpayes.EXIGIBILITE != null ? itemimpayes.EXIGIBILITE.Value.ToShortDateString() : string.Empty;
                            FactureEdite.TotFht = item.TOTFHT;
                            FactureEdite.TotFTax = item.TOTFTAX;
                            FactureEdite.TotFTTC = item.TOTFTTC;
                            FactureEdite.TypeEdition = "R";
                            FactureEdite.Regcli = item.REGROUPEMENT; ;
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;
                            lstFactureEditeParClient.Add(FactureEdite);
                            lstFactureResume.Add(FactureEditeInit);
                            Nbrs++;
                            if (Nbrs == 3) break;
                        }
                    }

                    if (lstFactureResume.Where(t => t.TypeEdition == "R").ToList().Count < 5)
                    {
                        int nbr = lstFactureResume.Where(t => t.TypeEdition == "R").ToList().Count;
                        for (int j = 0 + 1; j <= 4 - nbr; j++)
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Lotri = item.LOTRI;
                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE;


                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;

                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue = item.RUE;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.TotFht = item.TOTFHT;
                            FactureEdite.TotFTax = item.TOTFTAX;
                            FactureEdite.TotFTTC = item.TOTFTTC;
                            FactureEdite.OrdreAffichage = j;
                            FactureEdite.TypeEdition = "R";
                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;
                            lstFactureEditeParClient.Add(FactureEdite);
                        }
                    }

                    foreach (CsProduitFacture items in entfacs._LstProfact)
                    {
                        int i = 0;
                        CsFactureClient FactureEditee = new CsFactureClient();
                        FactureEditee.Centre = items.CENTRE;
                        FactureEditee.Client = items.CLIENT;
                        FactureEditee.Ordre = items.ORDRE;
                        FactureEditee.Lotri = item.LOTRI;
                        FactureEditee.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditee.Matricule = "Matricule  : " + item.MATRICULE;

                        FactureEditee.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditee.Regcli = item.REGROUPEMENT;
                        FactureEditee.LibelleRegcli = LibelleRegroupent;
                        FactureEditee.LibelleCateg = LibelleCategorie;
                        FactureEditee.Commune = LibelleCommune;
                        FactureEditee.Quartier = LibelleQurtier;

                        if (string.IsNullOrEmpty(items.LIBELLEPRODUIT) && _lstProduit != null && _lstProduit.Count > 0)
                            items.LIBELLEPRODUIT = _lstProduit.FirstOrDefault(d => d.CODE == items.PRODUIT).LIBELLE;

                        if (!string.IsNullOrEmpty(items.LIBELLEPRODUIT) && items.LIBELLEPRODUIT.Length > 7)
                            items.LIBELLEPRODUIT = items.LIBELLEPRODUIT.Substring(0, 7) + ".";


                        FactureEditee.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditee.ISSMS = item.ISFACTURESMS;
                        FactureEditee.TELEPHONE = item.TELEPHONE;
                        FactureEditee.EMAIL = item.EMAIL;

                        FactureEditee.LibelleTranche = items.LIBELLEPRODUIT + " " + items.COMPTEUR + " " + items.REGLAGECOMPTEUR;
                        FactureEditee.NIndex = items.NINDEX;
                        FactureEditee.AIndex = items.AINDEX;
                        FactureEditee.Quantite = items.CONSOFAC;
                        FactureEditee.Unite = string.Empty;

                        if (items.TFAC == "2" || items.TFAC == "8")
                            FactureEditee.Unite = " - " + items.CONSOFAC.ToString();

                        if ((items.TFAC == "1" || items.TFAC == "5") && (items.CONSO > items.CONSOFAC))
                            FactureEditee.Unite = " * " + (items.CONSO - items.CONSOFAC).ToString();

                        FactureEditee.OrdreAffichage = i;
                        FactureEditee.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditee.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditee.MontantTimbre = 0;
                        FactureEditee.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditee);
                        i++;
                        decimal? MontantTaxable = 0;
                        decimal? MontantTvaTaxable = 0;
                        decimal? MontantExoneration = 0;
                        decimal? MontantTVAExoneration = 0;
                        decimal? MontantSansTaxe = 0;


                        foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(t => t.REDEVANCE).ThenBy(p => p.TRANCHE))
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE;


                            if (string.IsNullOrEmpty(itemss.LIBELLEREDEVANCE))
                                itemss.LIBELLEREDEVANCE = _lstRedevence.FirstOrDefault(d => d.CODE == itemss.REDEVANCE && d.PRODUIT == itemss.PRODUIT).LIBELLE;


                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;
                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue = item.RUE;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                            FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                            FactureEdite.TotFht = item.TOTFHT;
                            byte Ordre = System.Convert.ToByte(itemss.TRANCHE);
                            if (itemss.UNITE == Enumere.CODEUNITE)
                                FactureEdite.LibelleTranche = itemss.LIBELLEREDEVANCE;
                            else
                            {
                                if (!string.IsNullOrEmpty(itemss.LIBELLETRANCHE))
                                    FactureEdite.LibelleTranche = itemss.LIBELLETRANCHE;
                                else
                                    FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(d => d.CODE == itemss.REDEVANCE && d.PRODUIT == itemss.PRODUIT).TRANCHEREDEVANCE.FirstOrDefault(e => e.ORDRE == Ordre).LIBELLE;
                            }

                            FactureEdite.Quantite = itemss.QUANTITE;

                            if (itemss.TOTREDTAX > 0)
                            {
                                MontantTaxable = MontantTaxable + itemss.TOTREDHT;
                                MontantTvaTaxable = MontantTvaTaxable + itemss.TOTREDTAX;
                            }
                            else
                                MontantSansTaxe = MontantSansTaxe + itemss.TOTREDHT;

                            FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                            if (itemss.REDEVANCE == "63")
                            {
                                MontantExoneration = MontantExoneration + itemss.TOTREDTAX;
                                FactureEdite.BarPrix = null;
                            }
                            else
                                FactureEdite.BarPrix = itemss.BARPRIX;

                            FactureEdite.TotRedHT = itemss.TOTREDHT;
                            FactureEdite.TotRedTax = itemss.TOTREDTAX >= 0 ? 0 : itemss.TOTREDTAX;
                            FactureEdite.TotRedTTC = 0;

                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;


                            FactureEdite.OrdreAffichage = i;
                            FactureEdite.TypeEdition = "D";
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;


                            if (itemss.REDEVANCE != "63")
                            {
                                i++;
                                lstFactureEditeParClient.Add(FactureEdite);
                            }
                        }
                        // Ligne vide
                        CsFactureClient FactureEditeeesVide = new CsFactureClient();
                        FactureEditeeesVide.Centre = items.CENTRE;
                        FactureEditeeesVide.Client = items.CLIENT;
                        FactureEditeeesVide.Ordre = items.ORDRE;
                        FactureEditeeesVide.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesVide.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeeesVide.Matricule = "Matricule  : " + item.MATRICULE;


                        FactureEditeeesVide.Regcli = item.REGROUPEMENT;
                        FactureEditeeesVide.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesVide.LibelleCateg = LibelleCategorie;
                        FactureEditeeesVide.Commune = LibelleCommune;
                        FactureEditeeesVide.Quartier = LibelleQurtier;

                        FactureEditeeesVide.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesVide.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesVide.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesVide.EMAIL = item.EMAIL;

                        FactureEditeeesVide.LibelleTranche = "";
                        FactureEditeeesVide.TotRedTax = null;
                        FactureEditeeesVide.TotRedTTC = null;
                        FactureEditeeesVide.OrdreAffichage = i + 1;
                        FactureEditeeesVide.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesVide.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesVide.MontantTimbre = 0;
                        FactureEditeeesVide.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesVide);


                        // TVA Electricité
                        CsFactureClient FactureEditees = new CsFactureClient();
                        FactureEditees.Centre = items.CENTRE;
                        FactureEditees.Client = items.CLIENT;
                        FactureEditees.Ordre = items.ORDRE;
                        FactureEditees.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditees.idEntfac = item.PK_ID;

                        FactureEditees.Regcli = item.REGROUPEMENT;
                        FactureEditees.LibelleRegcli = LibelleRegroupent;
                        FactureEditees.LibelleCateg = LibelleCategorie;
                        FactureEditees.Commune = LibelleCommune;
                        FactureEditees.Quartier = LibelleQurtier;

                        FactureEditees.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditees.ISSMS = item.ISFACTURESMS;
                        FactureEditees.TELEPHONE = item.TELEPHONE;
                        FactureEditees.EMAIL = item.EMAIL;
                        //FactureEditees.LibelleTranche = "TVA18% ";
                        CsCtax tx = new DBCTAX().GetAll().FirstOrDefault(t => t.CODE == "01");
                        if (tx != null)
                            FactureEditees.LibelleTranche = tx.LIBELLE;
                        else
                            FactureEditees.LibelleTranche = "TVA 18% ";

                        FactureEditees.TotRedTax = MontantTvaTaxable;
                        FactureEditees.TotRedHT = MontantTaxable;
                        FactureEditees.OrdreAffichage = i + 1;


                        if (leFraistimbre != null)
                            FactureEditees.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditees.MontantTimbre = 0;


                        FactureEditees.TypeEdition = "D";
                        FactureEditees.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditees);
                       /*
                        * //
                        // ExoTVA Electricité
                        CsFactureClient FactureEditeesExo = new CsFactureClient();
                        FactureEditeesExo.Centre = items.CENTRE;
                        FactureEditeesExo.Client = items.CLIENT;
                        FactureEditeesExo.Ordre = items.ORDRE;
                        FactureEditeesExo.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeesExo.idEntfac = item.PK_ID;

                        FactureEditeesExo.Regcli = item.REGROUPEMENT;
                        FactureEditeesExo.LibelleRegcli = LibelleRegroupent;
                        FactureEditeesExo.LibelleCateg = LibelleCategorie;
                        FactureEditeesExo.Commune = LibelleCommune;
                        FactureEditeesExo.Quartier = LibelleQurtier;

                        FactureEditeesExo.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeesExo.ISSMS = item.ISFACTURESMS;
                        FactureEditeesExo.TELEPHONE = item.TELEPHONE;
                        FactureEditeesExo.EMAIL = item.EMAIL;

                        FactureEditeesExo.LibelleTranche = "Exonération TVA";
                        FactureEditeesExo.TotRedTax = MontantExoneration;
                        FactureEditeesExo.TotRedHT = 0;
                        FactureEditeesExo.OrdreAffichage = i + 1;

                        if (leFraistimbre != null)
                            FactureEditeesExo.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeesExo.MontantTimbre = 0;


                        FactureEditeesExo.TypeEdition = "D";
                        FactureEditeesExo.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeesExo);

                        //
                        // Sous total Redevance sans taxe
                        CsFactureClient FactureEditeeesSansTaxe = new CsFactureClient();
                        FactureEditeeesSansTaxe.Centre = items.CENTRE;
                        FactureEditeeesSansTaxe.Client = items.CLIENT;
                        FactureEditeeesSansTaxe.Ordre = items.ORDRE;
                        FactureEditeeesSansTaxe.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesSansTaxe.idEntfac = item.PK_ID;


                        FactureEditeeesSansTaxe.Regcli = item.REGROUPEMENT;
                        FactureEditeeesSansTaxe.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesSansTaxe.LibelleCateg = LibelleCategorie;
                        FactureEditeeesSansTaxe.Commune = LibelleCommune;
                        FactureEditeeesSansTaxe.Quartier = LibelleQurtier;

                        FactureEditeeesSansTaxe.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesSansTaxe.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesSansTaxe.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesSansTaxe.EMAIL = item.EMAIL;

                        FactureEditeeesSansTaxe.LibelleTranche = "Sous total HT";
                        FactureEditeeesSansTaxe.TotRedTax = null;
                        FactureEditeeesSansTaxe.TotRedTTC = null;
                        //FactureEditeeesSansTaxe.TotRedHT = MontantSansTaxe; //TOTAL des non soumis à taxe 
                        FactureEditeeesSansTaxe.TotRedHT = items.TOTPROHT;


                        FactureEditeeesSansTaxe.OrdreAffichage = i;
                        FactureEditeeesSansTaxe.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesSansTaxe.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesSansTaxe.MontantTimbre = 0;
                        FactureEditeeesSansTaxe.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesSansTaxe);
                        */

                        //
                        // Ligne vide
                        CsFactureClient FactureEditeeesVide1 = new CsFactureClient();
                        FactureEditeeesVide1.Centre = items.CENTRE;
                        FactureEditeeesVide1.Client = items.CLIENT;
                        FactureEditeeesVide1.Ordre = items.ORDRE;
                        FactureEditeeesVide1.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesVide1.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeeesVide1.Matricule = "Matricule  : " + item.MATRICULE;

                        FactureEditeeesVide1.Regcli = item.REGROUPEMENT;
                        FactureEditeeesVide1.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesVide1.LibelleCateg = LibelleCategorie;
                        FactureEditeeesVide1.Commune = LibelleCommune;
                        FactureEditeeesVide1.Quartier = LibelleQurtier;


                        FactureEditeeesVide1.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesVide1.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesVide1.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesVide1.EMAIL = item.EMAIL;

                        FactureEditeeesVide1.LibelleTranche = "";
                        FactureEditeeesVide1.TotRedTax = null;
                        FactureEditeeesVide1.TotRedTTC = null;
                        FactureEditeeesVide1.OrdreAffichage = i + 1;
                        FactureEditeeesVide1.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesVide1.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesVide1.MontantTimbre = 0;
                        FactureEditeeesVide1.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesVide1);

                        // TTC
                        CsFactureClient FactureEditeees = new CsFactureClient();
                        FactureEditeees.Centre = items.CENTRE;
                        FactureEditeees.Client = items.CLIENT;
                        FactureEditeees.Ordre = items.ORDRE;
                        FactureEditeees.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeees.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeees.Matricule = "Matricule  : " + item.MATRICULE;

                        FactureEditeees.Regcli = item.REGROUPEMENT;
                        FactureEditeees.LibelleRegcli = LibelleRegroupent;
                        FactureEditeees.LibelleCateg = LibelleCategorie;
                        FactureEditeees.Commune = LibelleCommune;
                        FactureEditeees.Quartier = LibelleQurtier;

                        FactureEditeees.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeees.ISSMS = item.ISFACTURESMS;
                        FactureEditeees.TELEPHONE = item.TELEPHONE;
                        FactureEditeees.EMAIL = item.EMAIL;

                        FactureEditeees.LibelleTranche = "Sous total TTC";
                        FactureEditeees.TotRedTax = 0;
                        FactureEditeees.TotRedTTC = item.TOTFTTC;
                        FactureEditeees.OrdreAffichage = i;
                        FactureEditeees.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditees.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditees.MontantTimbre = 0;
                        FactureEditees.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeees);
                        //
                        int nbreDetail = lstFactureEditeParClient.Where(u => u.TypeEdition == "D").ToList().Count;
                        if (nbreDetail < 12)
                        {
                            int NbrSupplement = 12 - nbreDetail;
                            for (int j = 1; j <= NbrSupplement; j++)
                            {
                                CsFactureClient FactureEdite = new CsFactureClient();
                                FactureEdite.Centre = item.CENTRE;
                                FactureEdite.Client = item.CLIENT;
                                FactureEdite.Ordre = item.ORDRE;
                                FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                                FactureEdite.idEntfac = item.PK_ID;
                                if (!string.IsNullOrEmpty(item.MATRICULE))
                                    FactureEdite.Matricule = "Matricule  : " + item.MATRICULE;


                                FactureEdite.Regcli = item.REGROUPEMENT;
                                FactureEdite.LibelleRegcli = LibelleRegroupent;
                                FactureEdite.LibelleCateg = LibelleCategorie;
                                FactureEdite.Commune = LibelleCommune;
                                FactureEdite.Quartier = LibelleQurtier;

                                FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                                FactureEdite.ISSMS = item.ISFACTURESMS;
                                FactureEdite.TELEPHONE = item.TELEPHONE;
                                FactureEdite.EMAIL = item.EMAIL;
                                FactureEdite.Tournee = item.TOURNEE;
                                FactureEdite.OrdTour = item.ORDTOUR;
                                FactureEdite.NomAbon = item.NOMABON;
                                FactureEdite.Adrmand1 = item.ADRMAND1;
                                FactureEdite.Adrmand2 = item.ADRMAND2;
                                FactureEdite.Etage = item.ETAGE;
                                FactureEdite.Porte = item.PORTE;
                                FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                                FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                                FactureEdite.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                                FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                                FactureEdite.TotFht = item.TOTFHT;
                                FactureEdite.TotFTax = item.TOTFTAX;
                                FactureEdite.TotFTTC = item.TOTFTTC;
                                if (leFraistimbre != null)
                                    FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                                else
                                    FactureEdite.MontantTimbre = 0;

                                FactureEdite.OrdreAffichage = j;
                                FactureEdite.TypeEdition = "D";
                                FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;
                                lstFactureEditeParClient.Add(FactureEdite);
                            }
                        }
                    }
                    lstFactureEdite.AddRange(lstFactureEditeParClient);
                }
                context.Dispose();
                return lstFactureEdite;
            }
            catch (Exception es)
            {
                string cli = leClient;
                throw es;
            }
        }



        public List<CsFactureClient> EditionFacture(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            string leClient = string.Empty;
            try
            {
                galadbEntities context = new galadbEntities();
                List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
                List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
                List<CENTRE> _lstCentre = context.CENTRE.ToList();
                List<PRODUIT> _lstProduit = context.PRODUIT.ToList();
                List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
                List<MESSAGE> _lstMessage = context.MESSAGE.ToList();
                List<COMMUNE> _lstCommune = context.COMMUNE.ToList();
                List<QUARTIER> _lstQuartier = context.QUARTIER.ToList();
                List<CATEGORIECLIENT> _lstCategorie = context.CATEGORIECLIENT.ToList();
                List<Galatee.Entity.Model.FRAISTIMBRE> _lstTimbre = context.FRAISTIMBRE.ToList();
                List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();

                foreach (CsEnteteFacture item in lstEnteteFacture.Where(t => (t.TOTFTTC != null && t.TOTFTTC != 0)).ToList())
                {
                    List<CsFactureClient> lstFactureEditeParClient = new List<CsFactureClient>();
                    List<CsFactureClient> lstFactureResume = new List<CsFactureClient>();
                    leClient = item.CLIENT;
                    CsLafactureClient entfacs = new CsLafactureClient();
                    entfacs._LeEntatfac = item;
                    entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();

                   

                    if (entfacs._LstProfact.Count == 0) continue;

                    if (string.IsNullOrEmpty(item.LIBELLECENTRE) && item.FK_IDCENTRE > 0)
                        item.LIBELLECENTRE = _lstCentre.FirstOrDefault(d => d.PK_ID == item.FK_IDCENTRE).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLECATEGORIE) && item.FK_IDCATEGORIECLIENT > 0)
                        item.LIBELLECATEGORIE = _lstCategorie.FirstOrDefault(d => d.PK_ID == item.FK_IDCATEGORIECLIENT).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLECOMMUNE) && item.FK_IDCOMMUNE > 0)
                        item.LIBELLECOMMUNE = _lstCommune.FirstOrDefault(d => d.PK_ID == item.FK_IDCOMMUNE).LIBELLE;

                    if (string.IsNullOrEmpty(item.LIBELLEQUARTIER) && item.FK_IDQUARTIER > 0)
                        item.LIBELLEQUARTIER = _lstQuartier.FirstOrDefault(d => d.PK_ID == item.FK_IDQUARTIER).LIBELLE;

                    
                    bool IsForfait = false;
                    if (entfacs._LstProfact.FirstOrDefault(u => u.TFAC == "2" || u.TFAC == "6") != null)
                        IsForfait = true;

                    string LibelleCategorie =item.LIBELLECATEGORIE;
                    string LibelleRegroupent = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT) != null ? _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).CODE + " " + _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT).NOM : string.Empty;
                    string LibelleCommune = item.LIBELLECOMMUNE;
                    string LibelleQurtier = item.LIBELLEQUARTIER;
                    item.MATRICULE = item.MATRICULE;

                    List<CsLclient> lstImpaye = new List<Structure.CsLclient>();
                    CsEnteteFacture leEnteteFac = new Structure.CsEnteteFacture();
                    leEnteteFac = GenereCodeBar(item);


                    Galatee.Entity.Model.FRAISTIMBRE leFraistimbre = _lstTimbre.FirstOrDefault(p => item.TOTFTTC >= p.BORNEINF && item.TOTFTTC <= (p.BORNESUP + p.FRAIS));
                    CsFactureClient FactureEditeInit = new CsFactureClient();
                    FactureEditeInit.Centre = item.CENTRE;
                    FactureEditeInit.Client = item.CLIENT;
                    FactureEditeInit.Ordre = item.ORDRE;
                    FactureEditeInit.Lotri = item.LOTRI;
                    FactureEditeInit.idEntfac = item.PK_ID;
                    if(!string.IsNullOrEmpty(item.MATRICULE ))
                        FactureEditeInit.Matricule = "Matricule  : " + item.MATRICULE; 

                    FactureEditeInit.DenAbon = leEnteteFac.DENABON;
                    FactureEditeInit.Denmand  = leEnteteFac.DENMAND ;
                    FactureEditeInit.Produit = lstProduitFacture.First().PRODUIT;

                    FactureEditeInit.Regcli = item.REGROUPEMENT;
                    FactureEditeInit.Cpos = IsForfait == true ? "1" : string.Empty;
                    FactureEditeInit.LibelleRegcli = LibelleRegroupent;
                    FactureEditeInit.LibelleCateg = LibelleCategorie;
                    FactureEditeInit.Commune = LibelleCommune;
                    FactureEditeInit.Quartier = LibelleQurtier;

                    FactureEditeInit.ISFACTURE = item.ISFACTUREEMAIL;
                    FactureEditeInit.ISSMS = item.ISFACTURESMS;
                    FactureEditeInit.TELEPHONE = item.TELEPHONE;
                    FactureEditeInit.EMAIL = item.EMAIL;

                    CENTRE leCentreClient = _lstCentre.FirstOrDefault(t => t.PK_ID == item.FK_IDCENTRE);
                    MESSAGE leMessage = _lstMessage.FirstOrDefault(t => t.SITE == leCentreClient.CODESITE && item.DFAC >= t.DEBUT_VALIDITE && item.DFAC <= t.FIN_VALIDITE);
                    if (leMessage != null)
                        FactureEditeInit.Mes = leMessage.TEXTE;

                    if (string.IsNullOrEmpty(item.REGROUPEMENT))
                    {

                        lstImpaye = new DBEncaissement().RetourneListeFactureNonSoldeSpx(item.CENTRE, item.CLIENT, item.ORDRE, item.FK_IDCENTRE );
                        //lstImpaye = new List<Structure.CsLclient>();
                        if (lstImpaye != null && lstImpaye.Count != 0)
                        {
                            CsLclient leFactureEstElleDejaMiseAJour = lstImpaye.FirstOrDefault(t => t.NDOC == item.FACTURE && t.REFEM == item.PERIODE);
                            if (leFactureEstElleDejaMiseAJour != null )
                            {
                                FactureEditeInit.SoldeTotFTTC = lstImpaye.Sum(o => o.SOLDEFACTURE);
                                lstImpaye = lstImpaye.Where(t => t.NDOC != item.FACTURE && t.REFEM != item.PERIODE).ToList();
                            }
                            else
                                FactureEditeInit.SoldeTotFTTC = lstImpaye.Sum(o => o.SOLDEFACTURE) + item.TOTFTTC;
                        }
                        else
                            FactureEditeInit.SoldeTotFTTC = item.TOTFTTC;
                    }
                    else
                    {
                        REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == item.REGROUPEMENT);
                        if (leRegroup != null)
                            FactureEditeInit.Regcli = leRegroup.CODE + " " + leRegroup.NOM;
                        FactureEditeInit.SoldeTotFTTC = item.TOTFTTC;
                    }

                    FactureEditeInit.CodeOperation = Enumere.FactureGeneraleCoper;
                    FactureEditeInit.Tournee = item.TOURNEE;
                    FactureEditeInit.OrdTour = item.ORDTOUR;
                    FactureEditeInit.NomAbon = item.NOMABON;
                    FactureEditeInit.Adrmand1 = item.ADRMAND1;
                    FactureEditeInit.Adrmand2 = item.ADRMAND2;
                    FactureEditeInit.Etage = item.ETAGE;
                    FactureEditeInit.Porte = item.PORTE;
                    FactureEditeInit.Rue  = item.RUE ;
                    FactureEditeInit.LibelleCEntre = item.LIBELLECENTRE;
                    FactureEditeInit.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                    FactureEditeInit.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                    FactureEditeInit.Facture = item.FACTURE;
                    FactureEditeInit.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                    FactureEditeInit.TotFht = item.TOTFHT;
                    FactureEditeInit.TotFTax = item.TOTFTAX;
                    FactureEditeInit.TotFTTC = item.TOTFTTC;
                    FactureEditeInit.TotRedTTC = item.TOTFTTC;
                    if (leFraistimbre != null)
                        FactureEditeInit.MontantTimbre = leFraistimbre.FRAIS.Value;
                    else
                        FactureEditeInit.MontantTimbre = 0;

                    FactureEditeInit.TypeEdition = "R";
                    FactureEditeInit.DateReleve = entfacs._LstProfact.First().DATEEVT.Value.ToShortDateString();
                    if (entfacs._LstRedFact != null && entfacs._LstRedFact.Count > 0)
                        FactureEditeInit.DateRelevePrec = Convert.ToDateTime((entfacs._LstProfact.First().DATEEVT - TimeSpan.FromDays((int)entfacs._LstRedFact.First().NBJOUR))).ToShortDateString();
                    else
                        FactureEditeInit.DateRelevePrec = entfacs._LstProfact.First().DEVPRE.Value.ToShortDateString();
                    lstFactureEditeParClient.Add(FactureEditeInit);
                    lstFactureResume.Add(FactureEditeInit);

                    if (lstImpaye != null)  // Rapel impayes du client
                    {
                        int Nbrs = 0;
                        foreach (CsLclient itemimpayes in lstImpaye.OrderByDescending(t => t.REFEM))
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Lotri = item.LOTRI;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE; 

                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;

                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.CodeOperation = item.COPER;
                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue = item.RUE;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.Periode = FormatPeriodeMMAAAA(itemimpayes.REFEM);
                            FactureEdite.Facture = itemimpayes.NDOC;
                            FactureEdite.TotRedTTC = itemimpayes.SOLDEFACTURE ;
                            FactureEdite.dateExige = itemimpayes.EXIGIBILITE != null ? itemimpayes.EXIGIBILITE.Value.ToShortDateString() : string.Empty;
                            FactureEdite.TotFht = item.TOTFHT;
                            FactureEdite.TotFTax = item.TOTFTAX;
                            FactureEdite.TotFTTC = item.TOTFTTC;
                            FactureEdite.TypeEdition = "R";
                            FactureEdite.Regcli = item.REGROUPEMENT; ;
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;
                            lstFactureEditeParClient.Add(FactureEdite);
                            lstFactureResume.Add(FactureEditeInit);
                            Nbrs++;
                            if (Nbrs == 3) break;
                        }
                    }

                    if (lstFactureResume.Where(t => t.TypeEdition == "R").ToList().Count < 5)
                    {
                        int nbr = lstFactureResume.Where(t => t.TypeEdition == "R").ToList().Count;
                        for (int j = 0 + 1; j <= 4 - nbr; j++)
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Lotri = item.LOTRI;
                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE; 


                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;

                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue  = item.RUE ;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.TotFht = item.TOTFHT;
                            FactureEdite.TotFTax = item.TOTFTAX;
                            FactureEdite.TotFTTC = item.TOTFTTC;
                            FactureEdite.OrdreAffichage = j;
                            FactureEdite.TypeEdition = "R";
                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;
                            lstFactureEditeParClient.Add(FactureEdite);
                        }
                    }

                    foreach (CsProduitFacture items in entfacs._LstProfact)
                    {
                        int i = 0;
                        CsFactureClient FactureEditee = new CsFactureClient();
                        FactureEditee.Centre = items.CENTRE;
                        FactureEditee.Client = items.CLIENT;
                        FactureEditee.Ordre = items.ORDRE;
                        FactureEditee.Lotri = item.LOTRI;
                        FactureEditee.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditee.Matricule = "Matricule  : " + item.MATRICULE; 

                        FactureEditee.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditee.Regcli = item.REGROUPEMENT;
                        FactureEditee.LibelleRegcli = LibelleRegroupent;
                        FactureEditee.LibelleCateg = LibelleCategorie;
                        FactureEditee.Commune = LibelleCommune;
                        FactureEditee.Quartier = LibelleQurtier;

                        if (string.IsNullOrEmpty(items.LIBELLEPRODUIT) && _lstProduit != null && _lstProduit.Count > 0)
                            items.LIBELLEPRODUIT = _lstProduit.FirstOrDefault(d => d.CODE == items.PRODUIT).LIBELLE;

                        if (!string.IsNullOrEmpty(items.LIBELLEPRODUIT) && items.LIBELLEPRODUIT.Length > 7)
                            items.LIBELLEPRODUIT = items.LIBELLEPRODUIT.Substring(0, 7) + ".";
                        

                        FactureEditee.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditee.ISSMS = item.ISFACTURESMS;
                        FactureEditee.TELEPHONE = item.TELEPHONE;
                        FactureEditee.EMAIL = item.EMAIL;

                        FactureEditee.LibelleTranche = items.LIBELLEPRODUIT + " " + items.COMPTEUR + " " + items.REGLAGECOMPTEUR;
                        FactureEditee.NIndex = items.NINDEX;
                        FactureEditee.AIndex = items.AINDEX;
                        FactureEditee.Quantite = items.CONSOFAC;
                        FactureEditee.Unite = string.Empty;

                        if (items.TFAC == "2" || items.TFAC =="8")
                            FactureEditee.Unite = " - " + items.CONSOFAC.ToString();

                        if ((items.TFAC == "1" || items.TFAC =="5") && (items.CONSO > items.CONSOFAC))
                            FactureEditee.Unite = " * " + (items.CONSO - items.CONSOFAC).ToString();

                        FactureEditee.OrdreAffichage = i;
                        FactureEditee.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditee.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditee.MontantTimbre = 0;
                        FactureEditee.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditee);
                        i++;
                        decimal? MontantTaxable = 0;
                        decimal? MontantTvaTaxable = 0;
                        decimal? MontantExoneration = 0;
                        decimal? MontantTVAExoneration = 0;
                        decimal? MontantSansTaxe = 0;


                        foreach (CsRedevanceFacture itemss in entfacs._LstRedFact.OrderBy(t => t.REDEVANCE).ThenBy(p => p.TRANCHE))
                        {
                            CsFactureClient FactureEdite = new CsFactureClient();
                            FactureEdite.Centre = item.CENTRE;
                            FactureEdite.Client = item.CLIENT;
                            FactureEdite.Ordre = item.ORDRE;
                            FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                            FactureEdite.idEntfac = item.PK_ID;

                            if (!string.IsNullOrEmpty(item.MATRICULE))
                                FactureEdite.Matricule = "Matricule  : " + item.MATRICULE;


                            if (string.IsNullOrEmpty(itemss.LIBELLEREDEVANCE))
                                itemss.LIBELLEREDEVANCE = _lstRedevence.FirstOrDefault(d => d.CODE == itemss.REDEVANCE && d.PRODUIT == itemss.PRODUIT).LIBELLE;


                            FactureEdite.Regcli = item.REGROUPEMENT;
                            FactureEdite.LibelleRegcli = LibelleRegroupent;
                            FactureEdite.LibelleCateg = LibelleCategorie;
                            FactureEdite.Commune = LibelleCommune;
                            FactureEdite.Quartier = LibelleQurtier;
                            FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                            FactureEdite.ISSMS = item.ISFACTURESMS;
                            FactureEdite.TELEPHONE = item.TELEPHONE;
                            FactureEdite.EMAIL = item.EMAIL;

                            FactureEdite.Tournee = item.TOURNEE;
                            FactureEdite.OrdTour = item.ORDTOUR;
                            FactureEdite.NomAbon = item.NOMABON;
                            FactureEdite.Adrmand1 = item.ADRMAND1;
                            FactureEdite.Adrmand2 = item.ADRMAND2;
                            FactureEdite.Etage = item.ETAGE;
                            FactureEdite.Porte = item.PORTE;
                            FactureEdite.Rue  = item.RUE ;
                            FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                            FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                            FactureEdite.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                            FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                            FactureEdite.TotFht = item.TOTFHT;
                            byte Ordre = System.Convert.ToByte(itemss.TRANCHE);
                            if (itemss.UNITE == Enumere.CODEUNITE)
                                FactureEdite.LibelleTranche = itemss.LIBELLEREDEVANCE;
                            else
                            {
                                if (!string.IsNullOrEmpty(itemss.LIBELLETRANCHE))
                                    FactureEdite.LibelleTranche = itemss.LIBELLETRANCHE;
                                else
                                    FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(d => d.CODE == itemss.REDEVANCE && d.PRODUIT == itemss.PRODUIT).TRANCHEREDEVANCE.FirstOrDefault(e => e.ORDRE == Ordre).LIBELLE;
                            }

                            FactureEdite.Quantite = itemss.QUANTITE;

                            if (itemss.TOTREDTAX > 0)
                            {
                                MontantTaxable = MontantTaxable + itemss.TOTREDHT;
                                MontantTvaTaxable = MontantTvaTaxable + itemss.TOTREDTAX;
                            }
                            else
                                MontantSansTaxe = MontantSansTaxe + itemss.TOTREDHT;

                            FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == itemss.UNITE).LIBELLE;
                            if (itemss.REDEVANCE == "63")
                            {
                                MontantExoneration = MontantExoneration + itemss.TOTREDTAX;
                                FactureEdite.BarPrix = null;
                            }
                            else
                                FactureEdite.BarPrix = itemss.BARPRIX;

                            FactureEdite.TotRedHT = itemss.TOTREDHT;
                            FactureEdite.TotRedTax = itemss.TOTREDTAX >= 0 ? 0 : itemss.TOTREDTAX;
                            FactureEdite.TotRedTTC = 0;

                            if (leFraistimbre != null)
                                FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                            else
                                FactureEdite.MontantTimbre = 0;


                            FactureEdite.OrdreAffichage = i;
                            FactureEdite.TypeEdition = "D";
                            FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;


                            if (itemss.REDEVANCE != "63")
                            {
                                i++;
                                lstFactureEditeParClient.Add(FactureEdite);
                            }
                        }
                        // Ligne vide
                        CsFactureClient FactureEditeeesVide = new CsFactureClient();
                        FactureEditeeesVide.Centre = items.CENTRE;
                        FactureEditeeesVide.Client = items.CLIENT;
                        FactureEditeeesVide.Ordre = items.ORDRE;
                        FactureEditeeesVide.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesVide.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeeesVide.Matricule = "Matricule  : " + item.MATRICULE; 


                        FactureEditeeesVide.Regcli = item.REGROUPEMENT;
                        FactureEditeeesVide.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesVide.LibelleCateg = LibelleCategorie;
                        FactureEditeeesVide.Commune = LibelleCommune;
                        FactureEditeeesVide.Quartier = LibelleQurtier;

                        FactureEditeeesVide.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesVide.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesVide.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesVide.EMAIL = item.EMAIL;

                        FactureEditeeesVide.LibelleTranche = "";
                        FactureEditeeesVide.TotRedTax = null;
                        FactureEditeeesVide.TotRedTTC = null;
                        FactureEditeeesVide.OrdreAffichage = i + 1;
                        FactureEditeeesVide.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesVide.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesVide.MontantTimbre = 0;
                        FactureEditeeesVide.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesVide);


                        // TVA Electricité
                        CsFactureClient FactureEditees = new CsFactureClient();
                        FactureEditees.Centre = items.CENTRE;
                        FactureEditees.Client = items.CLIENT;
                        FactureEditees.Ordre = items.ORDRE;
                        FactureEditees.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditees.idEntfac = item.PK_ID;

                        FactureEditees.Regcli = item.REGROUPEMENT;
                        FactureEditees.LibelleRegcli = LibelleRegroupent;
                        FactureEditees.LibelleCateg = LibelleCategorie;
                        FactureEditees.Commune = LibelleCommune;
                        FactureEditees.Quartier = LibelleQurtier;

                        FactureEditees.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditees.ISSMS = item.ISFACTURESMS;
                        FactureEditees.TELEPHONE = item.TELEPHONE;
                        FactureEditees.EMAIL = item.EMAIL;

                        //string taxe = new DBCTAX().GetAll().Where(t=>t.CODE =

                        //FactureEditees.LibelleTranche = string.Format("TVA {0}% Electricité", );
                        FactureEditees.LibelleTranche = "TVA18% Electricité";

                        FactureEditees.TotRedTax = MontantTvaTaxable;
                        FactureEditees.TotRedHT = MontantTaxable;
                        FactureEditees.OrdreAffichage = i + 1;


                        if (leFraistimbre != null)
                            FactureEditees.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditees.MontantTimbre = 0;


                        FactureEditees.TypeEdition = "D";
                        FactureEditees.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditees);
                        //
                        // ExoTVA Electricité
                        CsFactureClient FactureEditeesExo = new CsFactureClient();
                        FactureEditeesExo.Centre = items.CENTRE;
                        FactureEditeesExo.Client = items.CLIENT;
                        FactureEditeesExo.Ordre = items.ORDRE;
                        FactureEditeesExo.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeesExo.idEntfac = item.PK_ID;

                        FactureEditeesExo.Regcli = item.REGROUPEMENT;
                        FactureEditeesExo.LibelleRegcli = LibelleRegroupent;
                        FactureEditeesExo.LibelleCateg = LibelleCategorie;
                        FactureEditeesExo.Commune = LibelleCommune;
                        FactureEditeesExo.Quartier = LibelleQurtier;

                        FactureEditeesExo.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeesExo.ISSMS = item.ISFACTURESMS;
                        FactureEditeesExo.TELEPHONE = item.TELEPHONE;
                        FactureEditeesExo.EMAIL = item.EMAIL;

                        FactureEditeesExo.LibelleTranche = "Exonération TVA";
                        FactureEditeesExo.TotRedTax = MontantExoneration;
                        FactureEditeesExo.TotRedHT = 0;
                        FactureEditeesExo.OrdreAffichage = i + 1;

                        if (leFraistimbre != null)
                            FactureEditeesExo.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeesExo.MontantTimbre = 0;


                        FactureEditeesExo.TypeEdition = "D";
                        FactureEditeesExo.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeesExo);
                        //
                        // Sous total Redevance sans taxe
                        CsFactureClient FactureEditeeesSansTaxe = new CsFactureClient();
                        FactureEditeeesSansTaxe.Centre = items.CENTRE;
                        FactureEditeeesSansTaxe.Client = items.CLIENT;
                        FactureEditeeesSansTaxe.Ordre = items.ORDRE;
                        FactureEditeeesSansTaxe.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesSansTaxe.idEntfac = item.PK_ID;


                        FactureEditeeesSansTaxe.Regcli = item.REGROUPEMENT;
                        FactureEditeeesSansTaxe.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesSansTaxe.LibelleCateg = LibelleCategorie;
                        FactureEditeeesSansTaxe.Commune = LibelleCommune;
                        FactureEditeeesSansTaxe.Quartier = LibelleQurtier;

                        FactureEditeeesSansTaxe.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesSansTaxe.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesSansTaxe.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesSansTaxe.EMAIL = item.EMAIL;

                        FactureEditeeesSansTaxe.LibelleTranche = "Sous total redevance sans taxe";
                        FactureEditeeesSansTaxe.TotRedTax = null;
                        FactureEditeeesSansTaxe.TotRedTTC = null;
                        FactureEditeeesSansTaxe.TotRedHT = MontantSansTaxe;


                        FactureEditeeesSansTaxe.OrdreAffichage = i;
                        FactureEditeeesSansTaxe.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesSansTaxe.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesSansTaxe.MontantTimbre = 0;
                        FactureEditeeesSansTaxe.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesSansTaxe);
                        //
                        // Ligne vide
                        CsFactureClient FactureEditeeesVide1 = new CsFactureClient();
                        FactureEditeeesVide1.Centre = items.CENTRE;
                        FactureEditeeesVide1.Client = items.CLIENT;
                        FactureEditeeesVide1.Ordre = items.ORDRE;
                        FactureEditeeesVide1.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeeesVide1.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeeesVide1.Matricule = "Matricule  : " + item.MATRICULE; 

                        FactureEditeeesVide1.Regcli = item.REGROUPEMENT;
                        FactureEditeeesVide1.LibelleRegcli = LibelleRegroupent;
                        FactureEditeeesVide1.LibelleCateg = LibelleCategorie;
                        FactureEditeeesVide1.Commune = LibelleCommune;
                        FactureEditeeesVide1.Quartier = LibelleQurtier;


                        FactureEditeeesVide1.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeeesVide1.ISSMS = item.ISFACTURESMS;
                        FactureEditeeesVide1.TELEPHONE = item.TELEPHONE;
                        FactureEditeeesVide1.EMAIL = item.EMAIL;

                        FactureEditeeesVide1.LibelleTranche = "";
                        FactureEditeeesVide1.TotRedTax = null;
                        FactureEditeeesVide1.TotRedTTC = null;
                        FactureEditeeesVide1.OrdreAffichage = i + 1;
                        FactureEditeeesVide1.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditeeesVide1.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditeeesVide1.MontantTimbre = 0;
                        FactureEditeeesVide1.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeeesVide1);

                        // TTC
                        CsFactureClient FactureEditeees = new CsFactureClient();
                        FactureEditeees.Centre = items.CENTRE;
                        FactureEditeees.Client = items.CLIENT;
                        FactureEditeees.Ordre = items.ORDRE;
                        FactureEditeees.Produit = lstProduitFacture.First().PRODUIT;
                        FactureEditeees.idEntfac = item.PK_ID;

                        if (!string.IsNullOrEmpty(item.MATRICULE))
                            FactureEditeees.Matricule = "Matricule  : " + item.MATRICULE; 

                        FactureEditeees.Regcli = item.REGROUPEMENT;
                        FactureEditeees.LibelleRegcli = LibelleRegroupent;
                        FactureEditeees.LibelleCateg = LibelleCategorie;
                        FactureEditeees.Commune = LibelleCommune;
                        FactureEditeees.Quartier = LibelleQurtier;

                        FactureEditeees.ISFACTURE = item.ISFACTUREEMAIL;
                        FactureEditeees.ISSMS = item.ISFACTURESMS;
                        FactureEditeees.TELEPHONE = item.TELEPHONE;
                        FactureEditeees.EMAIL = item.EMAIL;

                        FactureEditeees.LibelleTranche = "TTC";
                        FactureEditeees.TotRedTax = 0;
                        FactureEditeees.TotRedTTC = item.TOTFTTC;
                        FactureEditeees.OrdreAffichage = i;
                        FactureEditeees.TypeEdition = "D";
                        if (leFraistimbre != null)
                            FactureEditees.MontantTimbre = leFraistimbre.FRAIS.Value;
                        else
                            FactureEditees.MontantTimbre = 0;
                        FactureEditees.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;

                        lstFactureEditeParClient.Add(FactureEditeees);
                        //
                        int nbreDetail = lstFactureEditeParClient.Where(u => u.TypeEdition == "D").ToList().Count;
                        if (nbreDetail < 12)
                        {
                            int NbrSupplement = 12 - nbreDetail;
                            for (int j = 1; j <= NbrSupplement; j++)
                            {
                                CsFactureClient FactureEdite = new CsFactureClient();
                                FactureEdite.Centre = item.CENTRE;
                                FactureEdite.Client = item.CLIENT;
                                FactureEdite.Ordre = item.ORDRE;
                                FactureEdite.Produit = lstProduitFacture.First().PRODUIT;
                                FactureEdite.idEntfac = item.PK_ID;
                                if (!string.IsNullOrEmpty(item.MATRICULE))
                                    FactureEdite.Matricule = "Matricule  : " + item.MATRICULE; 


                                FactureEdite.Regcli = item.REGROUPEMENT;
                                FactureEdite.LibelleRegcli = LibelleRegroupent;
                                FactureEdite.LibelleCateg = LibelleCategorie;
                                FactureEdite.Commune = LibelleCommune;
                                FactureEdite.Quartier = LibelleQurtier;

                                FactureEdite.ISFACTURE = item.ISFACTUREEMAIL;
                                FactureEdite.ISSMS = item.ISFACTURESMS;
                                FactureEdite.TELEPHONE = item.TELEPHONE;
                                FactureEdite.EMAIL = item.EMAIL;
                                FactureEdite.Tournee = item.TOURNEE;
                                FactureEdite.OrdTour = item.ORDTOUR;
                                FactureEdite.NomAbon = item.NOMABON;
                                FactureEdite.Adrmand1 = item.ADRMAND1;
                                FactureEdite.Adrmand2 = item.ADRMAND2;
                                FactureEdite.Etage = item.ETAGE;
                                FactureEdite.Porte = item.PORTE;
                                FactureEdite.LibelleCEntre = item.LIBELLECENTRE;
                                FactureEdite.Dfac = Convert.ToDateTime(item.DFAC).ToShortDateString();
                                FactureEdite.Periode = FormatPeriodeMMAAAA(item.PERIODE);
                                FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG)).ToShortDateString();
                                FactureEdite.TotFht = item.TOTFHT;
                                FactureEdite.TotFTax = item.TOTFTAX;
                                FactureEdite.TotFTTC = item.TOTFTTC;
                                if (leFraistimbre != null)
                                    FactureEdite.MontantTimbre = leFraistimbre.FRAIS.Value;
                                else
                                    FactureEdite.MontantTimbre = 0;

                                FactureEdite.OrdreAffichage = j;
                                FactureEdite.TypeEdition = "D";
                                FactureEdite.SoldeTotFTTC = FactureEditeInit.SoldeTotFTTC;
                                lstFactureEditeParClient.Add(FactureEdite);
                            }
                        }
                    }
                    lstFactureEdite.AddRange(lstFactureEditeParClient);
                }
                context.Dispose();
                return lstFactureEdite;
            }
            catch (Exception es)
            {
                string cli = leClient;
                throw es;
            }
        }
        private CsEnteteFacture GenereCodeBar(CsEnteteFacture laFacture)
        {
            laFacture.EXIG = laFacture.EXIG != null ? laFacture.EXIG : 0 ;
            string Cle1 = CalculCle(laFacture.CENTRE + laFacture.CLIENT.ToString().PadLeft(12, '0') + laFacture.ORDRE);
            laFacture.DENABON = "1" + laFacture.CENTRE + laFacture.CLIENT.ToString().PadLeft(12, '0') + laFacture.ORDRE + Cle1 + laFacture.PERIODE.Substring(4, 2) + laFacture.PERIODE.Substring(0, 4) + laFacture.FACTURE.ToString().PadLeft(6, '0');

            string[] Splitval = laFacture.TOTFTTC.ToString().Split(',');
            int PartieEntier =int.Parse( Splitval[0]);
            string ParamCleBarcode2 = laFacture.DENABON + laFacture.CATEGORIECLIENT + laFacture.DFAC.Value.ToShortDateString().Substring(0, 2) +
                laFacture.DFAC.Value.ToShortDateString().Substring(3, 2) + laFacture.DFAC.Value.ToShortDateString().Substring(8, 2) + laFacture.EXIG.ToString().PadLeft(4, '0') + (PartieEntier).ToString().PadLeft(10, '0') + "00000000";

            string Cle2 = CalculCleControle(ParamCleBarcode2);
            laFacture.DENMAND  = laFacture.CATEGORIECLIENT  + laFacture.DFAC.Value.ToShortDateString().Substring(0, 2) +
                laFacture.DFAC.Value.ToShortDateString().Substring(3, 2) + laFacture.DFAC.Value.ToShortDateString().Substring(8, 2) + 
                laFacture.EXIG.ToString().PadLeft(4,'0') + (PartieEntier).ToString().PadLeft(10, '0') + "00000000" + Cle2;
            return laFacture;
        }

        string CalculCle(string Parametre)
        {
            int Cle = 0;
            int longueur = Parametre.Length;
            for (int i = 0; i < longueur; i++)
               Cle = Cle + ((int)(Parametre[i]-'0') * (longueur - i));

            Cle = (Cle % 97) + 1;
            int taille = Cle.ToString().Length;
            if (taille > 2)
                return Cle.ToString().Substring(taille - 2, 2);
            else
                return Cle.ToString().PadLeft(2, '0');
        }
        
        string CalculCleControle(string Parametre)
        {
            int Cle = 0;
            int longueur = Parametre.Length;
            for (int i = 0; i < longueur; i++)
                Cle = Cle + ((int)(Parametre[i] - '0') * (i+1));
            Cle = (Cle % 97) + 1;
            int taille = Cle.ToString().Length;
            if (taille > 2)
                return Cle.ToString().Substring(taille - 2,2);
            else
                return Cle.ToString().PadLeft(2,'0');

        }
        public  string FormatPeriodeMMAAAA(string periode)
        {
            if (periode.Length == 6)
                return periode.Substring(4, 2).PadLeft(2, '0') + "/" + periode.Substring(0, 4);
            else return string.Empty;
        }
       //public List<CsFactureClient> RetourneFacturesAbo07(List<CsEnteteFacture> lstClient, string rdlc)
       // {
       //     try
       //     {

       //         List<CsEnteteFacture> lstEnteteFacture = lstClient;

       //         DataTable dts = Galatee.Entity.Model.FacturationProcedure.RetourneCProfact(lstClient);
       //         List<CsProduitFacture> lstProduitFacture = Entities.GetEntityListFromQuery<CsProduitFacture>(dts);


       //         DataTable dtss = Galatee.Entity.Model.FacturationProcedure.RetourneCRedfact(lstClient);
       //         List<CsRedevanceFacture> lstRedevanceFacture = Entities.GetEntityListFromQuery<CsRedevanceFacture>(dtss);

       //         if (lstProduitFacture.First().PRODUIT == Enumere.ElectriciteMT)
       //         {
       //             if (rdlc == "FactureSimpleMT")
       //                 return EditionFactureMt(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
       //             else if (rdlc == "FactureDetailMT")
       //                 return EditionFactureBorderauxMT(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture.OrderBy(o => o.POINT).ToList(), lstRedevanceFacture.OrderBy(o => o.REDEVANCE ).ToList());
       //         }
       //         else
       //         {
       //             if (rdlc == "FactureSimple")
       //                 return EditionFacture(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
       //             else if (rdlc == "FactureDetail")
       //                 return EditionFactureBorderaux(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
       //         }
       //         if (rdlc == "BordereauSimple")
       //             return EditionFactureBordereauSimple(lstEnteteFacture.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ToList(), lstProduitFacture, lstRedevanceFacture);
       //         else return null;

       //     }
       //     catch (Exception ex)
       //     {
       //          throw new Exception(ex.Message);
       //     }
       // }



       public List<CsFactureClient> EditionFactureRegrouper(List<CsEnteteFacture> lstEnteteFacture, List<CsProduitFacture> lstProduitFacture, List<CsRedevanceFacture> lstRedevanceFacture)
        {
            string client = string.Empty;

            try
            {
                galadbEntities context = new galadbEntities();
                List<UNITECOMPTAGE> _lstUniteComptage = context.UNITECOMPTAGE.ToList();
                List<TRANCHEREDEVANCE> _lstTrancheRedevence = context.TRANCHEREDEVANCE.ToList();
                List<REDEVANCE> _lstRedevence = context.REDEVANCE.ToList();
                List<REGROUPEMENT> _lstRegroupement = context.REGROUPEMENT.ToList();
                List<PRODUIT> _lstProduit = context.PRODUIT.ToList();
                List<CENTRE> _lstCentre = context.CENTRE.ToList();
                List<CsFactureClient> lstFactureEdite = new List<CsFactureClient>();
                List<CsRegCli> lstDesRegroupement = new List<Structure.CsRegCli>();
                List<CsProduit> lstDesProduit = new List<CsProduit>();
                const string Devise = "FCFA";
                const string Centime = " ";

                int? exigibilite = lstEnteteFacture.Where(y => y.EXIG != null).OrderByDescending(u => u.EXIG).First().EXIG;
                DateTime? dfact = lstEnteteFacture.Where(y => y.DFAC != null).OrderByDescending(u => u.DFAC).First().DFAC;
                string exigib = Convert.ToDateTime(Convert.ToDateTime(dfact) + TimeSpan.FromDays((int)exigibilite)).ToShortDateString();

                var lstRegroupementDistnct = lstEnteteFacture.Select(t => new { t.REGROUPEMENT }).Distinct().ToList();
                foreach (var item in lstRegroupementDistnct)
                    lstDesRegroupement.Add(new CsRegCli { CODE = item.REGROUPEMENT });

                var lstProduitDistnct = lstProduitFacture.Select(t => new { t.PRODUIT }).Distinct().ToList();
                foreach (var item in lstProduitDistnct)
                    lstDesProduit.Add(new CsProduit { CODE = item.PRODUIT });

                List<string> lstTypeRedevanceConso = (new string[] { Enumere.REDEVANCECONSOMMATIONBT, Enumere.REDEVANCECONSOACTIVE, Enumere.REDEVANCECONSOPOINTE, Enumere.REDEVANCECONSOPLEINE, Enumere.REDEVANCECONSOCREUSE }).ToList();
                List<string> lstTypeRedevanceAutre = (new string[] { Enumere.REDEVANCEENTRETIEN, Enumere.REDEVANCEMAJORATION, Enumere.REDEVANCEPRIMEFIXE, Enumere.REDEVANCEDEPASSEMENT }).ToList();

                foreach (CsRegCli regroup in lstDesRegroupement)
                {

                    List<CsEnteteFacture> lstEnteteFacturerGroupe = lstEnteteFacture.Where(t => t.REGROUPEMENT == regroup.CODE).ToList();
                    if (lstEnteteFacturerGroupe == null || lstEnteteFacturerGroupe.Count == 0) continue;
                    decimal? TotHTRegroup = lstEnteteFacturerGroupe.Sum(t => t.TOTFHT);
                    decimal? TotTTCRegroup = lstEnteteFacturerGroupe.Sum(t => t.TOTFTTC);
                    decimal? TotTaxeRegroup = lstEnteteFacturerGroupe.Sum(t => t.TOTFTAX);
                    string MontantLettre = Galatee.Tools.ChiffresEnLettres.AmountInWords((double)TotTTCRegroup, Devise, Centime, "fr-FR");

                    foreach (CsProduit Produit in lstDesProduit)
                    {
                        List<CsEnteteFacture> lstEnteteFacturerGroupProduit = lstEnteteFacture.Where(t => t.REGROUPEMENT == regroup.CODE && t.PRODUIT == Produit.CODE && t.TOTFTTC > 0).ToList();
                        if (lstEnteteFacturerGroupProduit == null || lstEnteteFacturerGroupProduit.Count == 0) continue;

                        List<CsRedevanceFacture> lesRedevanceFacture = new List<Structure.CsRedevanceFacture>();
                        lesRedevanceFacture.AddRange(lstRedevanceFacture.Where(t => lstEnteteFacturerGroupProduit.Select(o => o.PK_ID).Contains(t.FK_IDENTFAC.Value)).ToList());
                        List<CsRedevanceFacture> lstDesRedevanceDistinct = new List<CsRedevanceFacture>();
                        var lstRedevanceDistnct = lesRedevanceFacture.Select(t => new { t.REDEVANCE, t.UNITE }).Distinct().ToList();
                        foreach (var red in lstRedevanceDistnct)
                            lstDesRedevanceDistinct.Add(new CsRedevanceFacture { REDEVANCE = red.REDEVANCE, UNITE = red.UNITE });

                        int? QuantiteMt;
                        REGROUPEMENT leRegroup1 = _lstRegroupement.FirstOrDefault(t => t.CODE == regroup.CODE);
                        if (Produit.CODE == Enumere.ElectriciteMT)
                        {
                            #region Redevance conso
                            CsFactureClient FactureEdite = new CsFactureClient();
                            List<CsRedevanceFacture> leDetailRedevance = lesRedevanceFacture.Where(t => lstTypeRedevanceConso.Contains(t.REDEVANCE)).ToList();

                            FactureEdite.NomAbon = lstEnteteFacturerGroupProduit.First().NOMABON;
                            FactureEdite.Periode = lstEnteteFacturerGroupProduit.First().PERIODE;
                            FactureEdite.dateExige = Convert.ToDateTime(Convert.ToDateTime(lstEnteteFacturerGroupProduit.First().DFAC) + TimeSpan.FromDays((int)lstEnteteFacturerGroupProduit.First().EXIG)).ToShortDateString();
                            FactureEdite.LibelleTranche = "Consomm. Actif tolal";
                            //FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == redevance.UNITE).LIBELLE;
                            FactureEdite.TotRedHT = leDetailRedevance.Sum(t => t.TOTREDHT);
                            FactureEdite.TotRedTax = leDetailRedevance.Sum(t => t.TOTREDTAX);
                            FactureEdite.TotRedTTC = leDetailRedevance.Sum(t => t.TOTREDTTC);
                            FactureEdite.TotFht = TotHTRegroup;
                            FactureEdite.TotFTax = TotTaxeRegroup;
                            FactureEdite.TotFTTC = TotTTCRegroup;
                            QuantiteMt = leDetailRedevance.Sum(t => t.QUANTITE);
                            FactureEdite.Quantite = QuantiteMt;
                            FactureEdite.Produit = lstEnteteFacturerGroupProduit.First().PRODUIT;
                            FactureEdite.LibelleProduit = _lstProduit.FirstOrDefault(o => o.CODE == FactureEdite.Produit).LIBELLE;
                            FactureEdite.NbFac = lstEnteteFacturerGroupe.Where(t =>t.TOTFTTC > 0).Count().ToString();
                            FactureEdite.Nature = lstEnteteFacturerGroupProduit.Count().ToString();
                            FactureEdite.Dr = MontantLettre;
                            REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == regroup.CODE);
                            if (leRegroup != null)
                            {
                                FactureEdite.Bureau = leRegroup.ADRESSE;
                                FactureEdite.Regcli = leRegroup.CODE;
                                FactureEdite.LibelleRegcli = leRegroup.NOM;
                            }
                            lstFactureEdite.Add(FactureEdite);
                            List<CsRedevanceFacture> lesRedevance = lstDesRedevanceDistinct.Where(u => !lstTypeRedevanceConso.Contains(u.REDEVANCE)).ToList();
                            foreach (CsRedevanceFacture redevance in lesRedevance.Where(t => t.REDEVANCE != null).ToList())
                            {
                                List<CsRedevanceFacture> leDetailRedevanceFixe = lesRedevanceFacture.Where(t => t.REDEVANCE == redevance.REDEVANCE).ToList();
                                client = redevance.CLIENT;
                                CsFactureClient FactureEditeFixe = new CsFactureClient();
                                List<TRANCHEREDEVANCE> lstrancheRed = _lstTrancheRedevence.Where(x => x.REDEVANCE.CODE == redevance.REDEVANCE && x.REDEVANCE.PRODUIT == redevance.PRODUIT).ToList();
                                FactureEditeFixe.NomAbon = lstEnteteFacturerGroupProduit.First().NOMABON;
                                FactureEditeFixe.Periode = lstEnteteFacturerGroupProduit.First().PERIODE;
                                FactureEditeFixe.LibelleTranche = _lstRedevence.FirstOrDefault(t => t.CODE == redevance.REDEVANCE).LIBELLE;
                                FactureEditeFixe.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == redevance.UNITE).LIBELLE;
                                FactureEditeFixe.TotRedHT = leDetailRedevanceFixe.Sum(t => t.TOTREDHT);
                                FactureEditeFixe.TotRedTax = leDetailRedevanceFixe.Sum(t => t.TOTREDTAX);
                                FactureEditeFixe.TotRedTTC = leDetailRedevanceFixe.Sum(t => t.TOTREDTTC);
                                FactureEditeFixe.TotFht = TotHTRegroup;
                                FactureEditeFixe.TotFTax = TotTaxeRegroup;
                                FactureEditeFixe.TotFTTC = TotTTCRegroup;
                                //FactureEditeFixe.Quantite = leDetailRedevanceFixe.Sum(t => t.QUANTITE);
                                FactureEditeFixe.Produit = lstEnteteFacturerGroupProduit.First().PRODUIT;
                                FactureEditeFixe.LibelleProduit = _lstProduit.FirstOrDefault(o => o.CODE == FactureEdite.Produit).LIBELLE;
                                FactureEditeFixe.NbFac = lstEnteteFacturerGroupe.Where(t => t.TOTFTTC > 0).Count().ToString();
                                FactureEditeFixe.Nature = lstEnteteFacturerGroupProduit.Count().ToString();
                                FactureEditeFixe.Dr = MontantLettre;
                                if (leRegroup != null)
                                {
                                    FactureEditeFixe.Bureau = leRegroup.ADRESSE;
                                    FactureEditeFixe.Regcli = leRegroup.CODE;
                                    FactureEditeFixe.LibelleRegcli = leRegroup.NOM;
                                }
                                lstFactureEdite.Add(FactureEditeFixe);
                            }
                            if (lstFactureEdite.Where(t => t.Produit == Produit.CODE).ToList().Count < 8)
                            {
                                int NbreLigneVide = 8;
                                if (lstDesProduit.Count == 1)
                                    NbreLigneVide = 25;
                                for (int i = 0; i < NbreLigneVide - lstFactureEdite.Where(t => t.Produit == Produit.CODE).ToList().Count; i++)
                                {
                                    CsFactureClient FactureEditeFixeMt = new CsFactureClient();
                                    FactureEditeFixeMt.NomAbon = lstEnteteFacturerGroupProduit.First().NOMABON;
                                    FactureEditeFixeMt.Periode = lstEnteteFacturerGroupProduit.First().PERIODE;
                                    FactureEditeFixeMt.Produit = lstEnteteFacturerGroupProduit.First().PRODUIT;
                                    FactureEditeFixeMt.LibelleProduit = _lstProduit.FirstOrDefault(o => o.CODE == FactureEdite.Produit).LIBELLE;
                                    FactureEditeFixeMt.NbFac = lstEnteteFacturerGroupe.Where(t =>t.TOTFTTC > 0).Count().ToString();
                                    FactureEditeFixeMt.Nature = lstEnteteFacturerGroupProduit.Count().ToString();
                                    FactureEditeFixeMt.Dr = MontantLettre;
                                    if (leRegroup != null)
                                    {
                                        FactureEditeFixeMt.Bureau = leRegroup.ADRESSE;
                                        FactureEditeFixeMt.Regcli = leRegroup.CODE;
                                        FactureEditeFixeMt.LibelleRegcli = leRegroup.NOM;
                                    }
                                    lstFactureEdite.Add(FactureEditeFixeMt);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            foreach (CsRedevanceFacture redevance in lstDesRedevanceDistinct.Where(t => t.REDEVANCE != null).ToList())
                            {
                                List<CsRedevanceFacture> leDetailRedevance = lesRedevanceFacture.Where(t => t.REDEVANCE == redevance.REDEVANCE).ToList();
                                client = redevance.CLIENT;

                                CsFactureClient FactureEdite = new CsFactureClient();
                                List<TRANCHEREDEVANCE> lstrancheRed = _lstTrancheRedevence.Where(x => x.REDEVANCE.CODE == redevance.REDEVANCE && x.REDEVANCE.PRODUIT == redevance.PRODUIT).ToList();
                                FactureEdite.NomAbon = lstEnteteFacturerGroupProduit.First().NOMABON;
                                FactureEdite.Periode = lstEnteteFacturerGroupProduit.First().PERIODE;
                                FactureEdite.LibelleTranche = _lstRedevence.FirstOrDefault(t => t.CODE == redevance.REDEVANCE).LIBELLE;
                                FactureEdite.Unite = _lstUniteComptage.FirstOrDefault(x => x.CODE == redevance.UNITE).LIBELLE;
                                FactureEdite.TotRedHT = leDetailRedevance.Sum(t => t.TOTREDHT);
                                FactureEdite.TotRedTax = leDetailRedevance.Sum(t => t.TOTREDTAX);
                                FactureEdite.TotRedTTC = leDetailRedevance.Sum(t => t.TOTREDTTC);
                                FactureEdite.TotFht = TotHTRegroup;
                                FactureEdite.TotFTax = TotTaxeRegroup;
                                FactureEdite.TotFTTC = TotTTCRegroup;
                                if (lstTypeRedevanceConso.Contains(redevance.REDEVANCE))
                                    FactureEdite.Quantite = leDetailRedevance.Sum(t => t.QUANTITE);

                                FactureEdite.Produit = lstEnteteFacturerGroupProduit.First().PRODUIT;
                                FactureEdite.LibelleProduit = "BASSE TENSION ";
                                FactureEdite.NbFac = lstEnteteFacturerGroupe.Where(t =>t.TOTFTTC > 0).Count().ToString();
                                FactureEdite.Nature = lstEnteteFacturerGroupProduit.Count().ToString();

                                FactureEdite.Dr = MontantLettre;
                                REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == regroup.CODE );
                                if (leRegroup != null)
                                {
                                    FactureEdite.Bureau = leRegroup.ADRESSE;
                                    FactureEdite.Regcli = leRegroup.CODE;
                                    FactureEdite.LibelleRegcli = leRegroup.NOM;
                                }
                                lstFactureEdite.Add(FactureEdite);
                            }
                            if (lstFactureEdite.Where(t => t.Produit == Produit.CODE).ToList().Count < 8)
                            {
                                int NbreLigneVide = 8;
                                if (lstDesProduit.Count == 1)
                                    NbreLigneVide = 25;
                                for (int i = 0; i < NbreLigneVide - lstFactureEdite.Where(t => t.Produit == Produit.CODE).ToList().Count; i++)
                                {
                                    CsFactureClient FactureEditeBt = new CsFactureClient();
                                    FactureEditeBt.NomAbon = lstEnteteFacturerGroupProduit.First().NOMABON;
                                    FactureEditeBt.Periode = lstEnteteFacturerGroupProduit.First().PERIODE;
                                    FactureEditeBt.Produit = lstEnteteFacturerGroupProduit.First().PRODUIT;
                                    FactureEditeBt.LibelleProduit = "BASSE TENSION ";
                                    FactureEditeBt.NbFac = lstEnteteFacturerGroupe.Where(t =>t.TOTFTTC > 0).Count().ToString();
                                    FactureEditeBt.Nature = lstEnteteFacturerGroupProduit.Count().ToString();
                                    FactureEditeBt.Dr = MontantLettre;
                                    REGROUPEMENT leRegroup = _lstRegroupement.FirstOrDefault(t => t.CODE == regroup.CODE);
                                    if (leRegroup != null)
                                    {
                                        FactureEditeBt.Bureau = leRegroup.ADRESSE;
                                        FactureEditeBt.Regcli = leRegroup.CODE;
                                        FactureEditeBt.LibelleRegcli = leRegroup.NOM;
                                    }
                                    lstFactureEdite.Add(FactureEditeBt);
                                }
                            }
                        }
                    }
                }
                context.Dispose();
                lstFactureEdite.ForEach(t => t.dateExige = exigib);
                return lstFactureEdite;
            }
            catch (Exception ex)
            {
                string ll = client;
                throw;
            }
        }


        //private CsQtFacMt ConsoParTypeCompteurMt(List<CsProduitFacture> _LstEvenement, List<TYPECOMPTAGE> lstTypeCompte)
        //{
        //    try
        //    {
        //        CsQtFacMt ConsoMt = new CsQtFacMt();
        //        TYPECOMPTAGE leTypeComptage = lstTypeCompte.FirstOrDefault(t => t.CODE == _LstEvenement.FirstOrDefault().TYPECOMPTAGE);
        //        if (leTypeComptage == null) return null;

        //        foreach (var _LeEvenement in _LstEvenement)
        //        {

        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF)
        //            {
        //                ConsoMt.dConsoActive = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
        //                ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;
        //                continue;
        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPOINTES)
        //            {
        //                ConsoMt.dConsoActHPointes = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
        //                ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;

        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPLEINES)
        //            {
        //                ConsoMt.dConsoActHPleines = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
        //                ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;

        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HCREUSES)
        //            {
        //                ConsoMt.dConsoActHCreuses = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
        //                ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;

        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.REACTIF)
        //            {
        //                ConsoMt.dConsoReactive = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKr1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
        //                ConsoMt.dKr2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;

        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.HORAIRE)
        //            {
        //                ConsoMt.dConsoHoraire = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;

        //            }
        //            if (_LeEvenement.TYPECOMPTEUR == Enumere.MAXIMETRE)
        //            {
        //                ConsoMt.dConsoMaximetre = Convert.ToDecimal(_LeEvenement.CONSO.Value);
        //                ConsoMt.dKimp = _LeEvenement.COEFLECT.Value;
        //                if (ConsoMt.dKimp == 0)
        //                    ConsoMt.dKimp = 1;
        //                ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

        //                continue;
        //            }
        //        }

        //        // Calcul de la consommation active = somme de la conso des trois compteurs ou du compteur actif à un seul cadran
        //        ConsoMt.dConsoTotaleActive = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses);

        //        // Calcul des pertes actives.
        //        ConsoMt.dPertesActives = ((ConsoMt.dKa1 * ConsoMt.dConsoTotaleActive) + (ConsoMt.dKa2 * ConsoMt.dConsoHoraire));
        //        ConsoMt.PertesActives = "(      " + ConsoMt.dKa1.ToString("#,0.00#") + "      X       " + ConsoMt.dConsoTotaleActive.ToString("#,0.") + "      )" + "       +       " + "     (   " + ConsoMt.dKa2.ToString("#,0.00#") + "    X    " + ConsoMt.dConsoHoraire.ToString("#,0.") + "    )    " + "               =     " + ConsoMt.dPertesActives.ToString("#,0.");
        //        // Calcul des pertes réactives
        //        ConsoMt.dPertesReactives = ((ConsoMt.dKr1 * ConsoMt.dConsoReactive) + (ConsoMt.dKr2 * ConsoMt.dConsoHoraire));
        //        ConsoMt.PertesReactives = "(      " + ConsoMt.dKr1.ToString("#,0.00#") + "      X       " + ConsoMt.dConsoReactive.ToString("#,0.") + "          )" + "       +       " + "      (   " + ConsoMt.dKr2.ToString("#,0.00#") + "    X    " + ConsoMt.dConsoHoraire.ToString("#,0.") + "    )    " + "                 =     " + ConsoMt.dPertesReactives.ToString("#,0.");

        //        // Calcul de la consommation totale active y compris les pertes
        //        ConsoMt.dConsoTotActiveAvecPertes = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses + ConsoMt.dPertesActives);
        //        ConsoMt.TotActiveAvecPertes = "       " + ConsoMt.dConsoTotaleActive.ToString("#,0.") + "          +          " + ConsoMt.dPertesActives.ToString("#,0.") + "                                  =          " + ConsoMt.dConsoTotActiveAvecPertes.ToString("#,0.");
        //        // Calcul de la consommation totale réactive y compris les pertes
        //        ConsoMt.dConsoTotReactiveAvecPertes = (ConsoMt.dConsoReactive + ConsoMt.dPertesReactives);
        //        ConsoMt.TotReactiveAvecPertes = "     " + ConsoMt.dConsoReactive.ToString("#,0.") + "   +   " + ConsoMt.dPertesReactives.ToString("#,0.") + "                        =   " + ConsoMt.dConsoTotReactiveAvecPertes.ToString("#,0.");

        //        // calcul de Tangente Phi
        //        // On force tangente phi à 0 si le dénominateur est nul pour éviter un débordement
        //        if (ConsoMt.dConsoTotActiveAvecPertes == 0)
        //        {

        //            ConsoMt.dTanPhi = 0;
        //            ConsoMt.TanPhi = "0";
        //        }
        //        else
        //        {
        //            ConsoMt.dTanPhi = (ConsoMt.dConsoTotReactiveAvecPertes / ConsoMt.dConsoTotActiveAvecPertes);
        //            ConsoMt.TanPhi = ConsoMt.dTanPhi.ToString("#,0.00#");
        //            ConsoMt.WaMa = ConsoMt.dConsoTotaleActive.ToString("#,0.") + "   +   " + ConsoMt.dPertesActives.ToString("#,0.");
        //            ConsoMt.WrMr = ConsoMt.dConsoReactive.ToString("#,0.") + "   +   " + ConsoMt.dPertesReactives.ToString("#,0.");
        //        }
        //        return ConsoMt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private CsQtFacMt ConsoParTypeCompteurMt(List<CsProduitFacture> _LstEvenement, List<TYPECOMPTAGE> lstTypeCompte)
        {
            try
            {
                CsQtFacMt ConsoMt = new CsQtFacMt();
                foreach (var _LeEvenement in _LstEvenement)
                {
                   
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF)
                        {
                            ConsoMt.dConsoActive = Convert.ToDecimal(_LeEvenement.CONSO .Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;
                            continue;
                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPOINTES)
                        {
                            ConsoMt.dConsoActHPointes = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HPLEINES)
                        {
                            ConsoMt.dConsoActHPleines = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.ACTIF_HCREUSES)
                        {
                            ConsoMt.dConsoActHCreuses = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKa1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKa2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.REACTIF)
                        {
                            ConsoMt.dConsoReactive = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKr1 = _LeEvenement.COEFK1 != null ? _LeEvenement.COEFK1.Value : 0;
                            ConsoMt.dKr2 = _LeEvenement.COEFK2 != null ? _LeEvenement.COEFK2.Value : 0;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.HORAIRE)
                        {
                            ConsoMt.dConsoHoraire = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;

                        }
                        if (_LeEvenement.TYPECOMPTEUR == Enumere.MAXIMETRE)
                        {
                            ConsoMt.dConsoMaximetre = Convert.ToDecimal(_LeEvenement.CONSO.Value);
                            ConsoMt.dKimp = _LeEvenement.COEFLECT.Value;
                            if (ConsoMt.dKimp == 0)
                                ConsoMt.dKimp = 1;
                            ConsoMt.dCumulRegConsoDeb += _LeEvenement.REGCONSO != null ? _LeEvenement.REGCONSO.Value : 0;

                            continue;
                        }
                    }
                
                // Calcul de la consommation active = somme de la conso des trois compteurs ou du compteur actif à un seul cadran
                ConsoMt.dConsoTotaleActive = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses);

                // Calcul des pertes actives.
                ConsoMt.dPertesActives = ((ConsoMt.dKa1 * ConsoMt.dConsoTotaleActive) + (ConsoMt.dKa2 * ConsoMt.dConsoHoraire));
                ConsoMt.PertesActives = "(      " + ConsoMt.dKa1.ToString("#,0.00#") + "      X       " + ConsoMt.dConsoTotaleActive.ToString("#,0.") + "      )" + "       +       " + "     (   " + ConsoMt.dKa2.ToString("#,0.00#") + "    X    " + ConsoMt.dConsoHoraire.ToString("#,0.") + "    )    " + "               =     " + ConsoMt.dPertesActives.ToString("#,0.");
                // Calcul des pertes réactives
                ConsoMt.dPertesReactives = ((ConsoMt.dKr1 * ConsoMt.dConsoReactive) + (ConsoMt.dKr2 * ConsoMt.dConsoHoraire));
                ConsoMt.PertesReactives = "(      " + ConsoMt.dKr1.ToString("#,0.00#") + "      X       " + ConsoMt.dConsoReactive.ToString("#,0.") + "          )" + "       +       " + "      (   " + ConsoMt.dKr2.ToString("#,0.00#") + "    X    " + ConsoMt.dConsoHoraire.ToString("#,0.") + "    )    " + "                 =     " + ConsoMt.dPertesReactives.ToString("#,0.");

                // Calcul de la consommation totale active y compris les pertes
                ConsoMt.dConsoTotActiveAvecPertes = (ConsoMt.dConsoActive + ConsoMt.dConsoActHPointes + ConsoMt.dConsoActHPleines + ConsoMt.dConsoActHCreuses + ConsoMt.dPertesActives);
                ConsoMt.TotActiveAvecPertes = "       " + ConsoMt.dConsoTotaleActive.ToString("#,0.") + "          +          " + ConsoMt.dPertesActives.ToString("#,0.") + "                                  =          " + ConsoMt.dConsoTotActiveAvecPertes.ToString("#,0.");
                // Calcul de la consommation totale réactive y compris les pertes
                ConsoMt.dConsoTotReactiveAvecPertes = (ConsoMt.dConsoReactive + ConsoMt.dPertesReactives);
                ConsoMt.TotReactiveAvecPertes = "     " + ConsoMt.dConsoReactive.ToString("#,0.") + "   +   " + ConsoMt.dPertesReactives.ToString("#,0.") + "                        =   " + ConsoMt.dConsoTotReactiveAvecPertes.ToString("#,0.");

                // calcul de Tangente Phi
                // On force tangente phi à 0 si le dénominateur est nul pour éviter un débordement
                if (ConsoMt.dConsoTotActiveAvecPertes == 0)
                {

                    ConsoMt.dTanPhi = 0;
                    ConsoMt.TanPhi = "0";
                }
                else
                {
                    ConsoMt.dTanPhi = (ConsoMt.dConsoTotReactiveAvecPertes / ConsoMt.dConsoTotActiveAvecPertes);
                    ConsoMt.TanPhi = ConsoMt.dTanPhi.ToString("#,0.00#");
                    ConsoMt.WaMa = ConsoMt.dConsoTotaleActive.ToString("#,0.") + "   +   " + ConsoMt.dPertesActives.ToString("#,0.");
                    ConsoMt.WrMr = ConsoMt.dConsoReactive.ToString("#,0.") + "   +   " + ConsoMt.dPertesReactives.ToString("#,0.");
                }
                return ConsoMt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public List<CsLafactureClient> RetourneDuplicatas(string centre, string client, string ordre)
        {
            try
            {
                try
                {

                   List<CsLafactureClient> result = new List<CsLafactureClient>();
                    //_tt = Galatee.Entity.Model.EserviceProcedure.ListeFactureABO07(centre, client, ordre, periode, numFacture);
                   List<Galatee.Entity.Model.CENTFAC> dt = Galatee.Entity.Model.EserviceProcedure.ListeFactureDuplicata(centre, client, ordre, null, null);

                   foreach (Galatee.Entity.Model.CENTFAC item in dt)
                   {
                       CsLafactureClient _tt = new CsLafactureClient();
                       _tt._LstProfact = Entities.ConvertObject<CsProduitFacture, Galatee.Entity.Model.CPROFAC>(item.CPROFAC.ToList());
                       _tt._LstRedFact = Entities.ConvertObject<CsRedevanceFacture, Galatee.Entity.Model.CREDFAC>(item.CREDFAC.ToList());
                       _tt._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(item);

                       result.Add(_tt);
                   }


                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

               
            }
            catch (Exception ex)
            {
                throw new Exception("RetourneDuplicatas" + " :" + ex.Message);
            }
         
        }

    

        #endregion

        #region Mise a jour des factures

        #region Methodes de recuperation
        public List<CsLotri> retourneListeAMaj()
        {
            try
            {
                return Galatee.Entity.Model.FacturationProcedure.retourneListeAMaj();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

            //string mode = string.Empty;
            //string majAbon = string.Empty;
            //string majCompteur = string.Empty;

            //cn = new SqlConnection(ConnectionString);

            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            //cmd.CommandTimeout = 360;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "SPX_FAC_RETOURNE_LOTRI_MAJ";


            //try
            //{
            //    if (cn.State == ConnectionState.Closed)
            //        cn.Open();

            //    SqlDataReader reader = cmd.ExecuteReader();
            //    List<CsLotri> listeDesLotri = new List<CsLotri>();

            //    while (reader.Read())
            //    {
            //        CsLotri lot = new CsLotri();
            //        lot.NUMLOTRI = (Convert.IsDBNull(reader["Lotri"])) ? String.Empty : (String)reader["Lotri"];
            //        lot.JET = (Convert.IsDBNull(reader["Jet"])) ? String.Empty : (String)reader["Jet"];
            //        listeDesLotri.Add(lot);
            //    }
            //    reader.Close();

            //    string lotSelectione = string.Empty;
            //    string jetSelectionne = string.Empty;
            //    List<CsLotri> rows = new List<CsLotri>();


            //    // POur chaque lotri retourné, on verifie s'il doit faire l'objet de mise à jour               
            //    cmd.CommandText = "SPX_FAC_LISTE_DE_LOT_A_MAJ";

            //    foreach (CsLotri lot in listeDesLotri)
            //    {
            //        cmd.Parameters.Clear();
            //        cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 20).Value = (string.IsNullOrEmpty(lot.NUMLOTRI) ? null : lot.NUMLOTRI);
            //        reader = cmd.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            CsLotri lotri = new CsLotri();
            //            lotri.NUMLOTRI = (Convert.IsDBNull(reader["Lotri"])) ? String.Empty : (String)reader["Lotri"];
            //            lotri.JET = (Convert.IsDBNull(reader["Jet"])) ? String.Empty : (String)reader["Jet"];
            //            //lotri.TopMaj = (Convert.IsDBNull(reader["EtatFac1"])) ? String.Empty : (String)reader["EtatFac1"];
            //            lotri.PERIODE = (Convert.IsDBNull(reader["EtatFac5"])) ? String.Empty : (String)reader["EtatFac5"];

            //            if (lotri.NUMLOTRI == lotSelectione && lotri.JET == jetSelectionne)
            //            {
            //                rows.RemoveAll(f => f.NUMLOTRI == lotSelectione && f.JET == jetSelectionne);
            //                throw new Exception("Il y a plusieurs lignes pour le couple (lot:" + lotSelectione + ", jet:" + jetSelectionne + ")" +
            //                                    "avec des ETATS differents. \nLa mise à jour de ce couple est impossible.");
            //            }

            //            /*
            //            if (entFact.TopMaj == "P" || entFact.TopMaj == "T")
            //                mode = "Partiel";
            //            else
            //                mode = "Definif";

            //            if (entFact.Periode == "A" || entFact.Periode == "M")
            //                majAbon = "Fait";
            //            else
            //                majAbon = "A faire";

            //            if (entFact.Periode == "M")
            //                majCompteur = "Fait";
            //            else
            //                majCompteur = "A faire";

            //            rows.Add(entFact);

            //            lotSelectione = entFact.Lotri;
            //            jetSelectionne = entFact.Jet;
            //             * */

            //            rows.Add(lotri);
            //        }
            //        reader.Close();
            //    }

            //    return rows;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(cmd.CommandText + " :" + ex.Message);
            //}
            //finally
            //{
            //    if (cn.State == ConnectionState.Open)
            //        cn.Close(); // Fermeture de la connection 
            //    cmd.Dispose();
            //}
        }

        /// <summary>
        /// Retourne la liste des mois comptables contenus dans la table arrete
        /// </summary>
        /// <param name="statut"></param>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <returns></returns>
        public List<string> retourneMoisComptable(string statut, string dateDebut, string dateFin)
        {
            try
            {
                return Galatee.Entity.Model.FacturationProcedure.retourneMoisComptable(statut,dateDebut,dateFin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



 
        }


      
        

        public List<ENTFAC> retourneListeEntFac(string lotri, galadbEntities pcontext)
        {
            try
            {
                #region Traitement
                return Galatee.Entity.Model.FacturationProcedure.retourneListeEntFac(lotri, pcontext);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLafactureClient>  retourneListeEntFac(CsLotri  lotri,bool IsISole)
        {
            try
            {
                DataTable dtEnt = Galatee.Entity.Model.FacturationProcedure.RetourneEntfact(lotri, IsISole);
                List<CsEnteteFacture> lstEnteteFacture = Entities.GetEntityListFromQuery<CsEnteteFacture>(dtEnt);

                DataTable dtProf = Galatee.Entity.Model.FacturationProcedure.RetourneProfact(lotri, IsISole);
                List<CsProduitFacture> lstProduitFacture = Entities.GetEntityListFromQuery<CsProduitFacture>(dtProf);

                DataTable dtRed = Galatee.Entity.Model.FacturationProcedure.RetourneRedfact(lotri, IsISole);
                List<CsRedevanceFacture> lstRedevanceFacture = Entities.GetEntityListFromQuery<CsRedevanceFacture>(dtRed);
                List<CsLafactureClient> lstFactureClient = new List<CsLafactureClient>();
                foreach (CsEnteteFacture item in lstEnteteFacture)
                {

                    CsLafactureClient entfacs = new CsLafactureClient();
                    entfacs._LeEntatfac = item;
                    entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    lstFactureClient.Add(entfacs);
                }
                return lstFactureClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLafactureClient> retourneListeEvenemenetEntFac(CsLotri lotri, bool IsISole)
        {
            try
            {
                DataTable dtEnt = Galatee.Entity.Model.FacturationProcedure.RetourneEntfact(lotri, IsISole);
                List<CsEnteteFacture> lstEnteteFacture = Entities.GetEntityListFromQuery<CsEnteteFacture>(dtEnt);

                DataTable dtProf = Galatee.Entity.Model.FacturationProcedure.RetourneProfact(lotri, IsISole);
                List<CsProduitFacture> lstProduitFacture = Entities.GetEntityListFromQuery<CsProduitFacture>(dtProf);

                DataTable dtRed = Galatee.Entity.Model.FacturationProcedure.RetourneRedfact(lotri, IsISole);
                List<CsRedevanceFacture> lstRedevanceFacture = Entities.GetEntityListFromQuery<CsRedevanceFacture>(dtRed);
                List<CsLafactureClient> lstFactureClient = new List<CsLafactureClient>();
                foreach (CsEnteteFacture item in lstEnteteFacture)
                {

                    CsLafactureClient entfacs = new CsLafactureClient();
                    entfacs._LeEntatfac = item;
                    entfacs._LstProfact = lstProduitFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    entfacs._LstRedFact = lstRedevanceFacture.Where(t => t.FK_IDENTFAC == item.PK_ID).ToList();
                    lstFactureClient.Add(entfacs);
                }
                return lstFactureClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CsProduitFacture> RetourneListeProFac(string lotri, string jet, string centre, string client, string ordre, string lienfac, SqlCommand command)
        public List<CsProduitFacture> RetourneListeProFac(string lotri, string jet, string centre, string client, string ordre, string lienfac)
        {



            #region Déclaration de variables
            List<CsProduitFacture> rows = new List<CsProduitFacture>();
            #endregion
            try
            {
                #region Traitement
                rows = Galatee.Entity.Model.FacturationProcedure.RetourneListeProFac(lotri, jet, centre, client, ordre, lienfac);

                return rows;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }



            ////cn = new SqlConnection(ConnectionString);
 
        }

        //public List<CsRedevanceFacture> RetourneListeRedFac(string lotri, string jet, string centre, string client, string ordre, string lienfac, SqlCommand command)
        public List<CsRedevanceFacture> RetourneListeRedFac(string lotri, string jet, string centre, string client, string ordre, string lienfac)
        {



            #region Déclaration de variables
            List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
            #endregion
            try
            {
                #region Traitement
                rows = Galatee.Entity.Model.FacturationProcedure.RetourneListeRedFac(lotri, jet, centre, client, ordre, lienfac);

                return rows;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }






        //    //cn = new SqlConnection(ConnectionString);

        //    //command = new SqlCommand();
        //    //command.Connection = cn;
        //    command.CommandTimeout = 360;
        //    command.CommandType = CommandType.StoredProcedure;

            //try
            //{
                //if (command.Connection.State == ConnectionState.Closed)
                //    command.Connection.Open();

                //// Selection de toutes les factures presentent dans ce lot
                //command.CommandText = "SPX_FAC_SELECT_RED_FAC";
                //command.Parameters.Clear();
                //command.Parameters.Add("@lotri", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lotri) ? null : lotri;
                //command.Parameters.Add("@jet", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(jet) ? null : jet;
                //command.Parameters.Add("@centre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(centre) ? null : centre;
                //command.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(client) ? null : client;
                //command.Parameters.Add("@ordre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(ordre) ? null : ordre;
                //command.Parameters.Add("@lienfac", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lienfac) ? null : lienfac;
                //DBBase.SetDBNullParametre(command.Parameters);

                //SqlDataReader reader = command.ExecuteReader();
                //List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
                //while (reader.Read())
                //{
                //    CsRedevanceFacture fac = new CsRedevanceFacture();
                //    //fac.Lotri = (Convert.IsDBNull(reader["lotri"])) ? String.Empty : (String)reader["lotri"];
                //    //fac.Jet = (Convert.IsDBNull(reader["jet"])) ? String.Empty : (String)reader["jet"];
                //    //fac.Centre = (Convert.IsDBNull(reader["Centre"])) ? String.Empty : (String)reader["Centre"];
                //    //fac.Client = (Convert.IsDBNull(reader["Client"])) ? String.Empty : (String)reader["Client"];
                //    //fac.OrdreD = (Convert.IsDBNull(reader["OrdreD"])) ? String.Empty : (String)reader["OrdreD"];
                //    ////fac.RowIdCl = (Convert.IsDBNull(reader["RowIdCl"])) ? String.Empty : (String)reader["RowIdCl"];
                //    //fac.LienRed = (Convert.IsDBNull(reader["LienRed"])) ? null : (Int16?)reader["LienRed"];
                //    //fac.Facture = (Convert.IsDBNull(reader["Facture"])) ? String.Empty : (String)reader["Facture"];
                //    //fac.Redevance = (Convert.IsDBNull(reader["Redevance"])) ? String.Empty : (String)reader["Redevance"];
                //    //fac.Tranche = (Convert.IsDBNull(reader["Tranche"])) ? String.Empty : (String)reader["Tranche"];
                //    //fac.OrdreD = (Convert.IsDBNull(reader["OrdreD"])) ? String.Empty : (String)reader["OrdreD"];
                //    //fac.Quantite = (Convert.IsDBNull(reader["Quantite"])) ? null : (int?)reader["Quantite"];
                //    //fac.Unite = (Convert.IsDBNull(reader["Unite"])) ? String.Empty : (String)reader["Unite"];
                //    //fac.BarPrix = (Convert.IsDBNull(reader["BarPrix"])) ? null : (decimal?)reader["BarPrix"];
                //    //fac.Taxe = (Convert.IsDBNull(reader["Taxe"])) ? null : (decimal?)reader["Taxe"];
                //    //fac.CTax = (Convert.IsDBNull(reader["CTax"])) ? String.Empty : (String)reader["CTax"];
                //    //fac.DApp = (Convert.IsDBNull(reader["DApp"])) ? null : (DateTime?)DateTime.Parse((string)reader["DApp"]);
                //    //fac.Critere = (Convert.IsDBNull(reader["Critere"])) ? String.Empty : (String)reader["Critere"];
                //    //fac.Variante = (Convert.IsDBNull(reader["Variante"])) ? null : (Int16?)reader["Variante"];
                //    ////fac.TotRedHT = (Convert.IsDBNull(reader["TotRedHT"])) ? null : (decimal?)reader["TotRedHT"];
                //    ////fac.TotRedTax = (Convert.IsDBNull(reader["TotRedTax"])) ? null : (decimal?)reader["TotRedTax"];
                //    ////fac.TotRedTTC = (Convert.IsDBNull(reader["TotRedTTC"])) ? null : (decimal?)reader["TotRedTTC"];
                //    //fac.Param1 = (Convert.IsDBNull(reader["Param1"])) ? String.Empty : (String)reader["Param1"];
                //    //fac.Param2 = (Convert.IsDBNull(reader["Param2"])) ? String.Empty : (String)reader["Param2"];
                //    //fac.Param3 = (Convert.IsDBNull(reader["Param3"])) ? String.Empty : (String)reader["Param3"];
                //    //fac.Param4 = (Convert.IsDBNull(reader["Param4"])) ? String.Empty : (String)reader["Param4"];
                //    //fac.Param5 = (Convert.IsDBNull(reader["Param5"])) ? String.Empty : (String)reader["Param5"];
                //    //fac.Param6 = (Convert.IsDBNull(reader["Param6"])) ? String.Empty : (String)reader["Param6"];
                //    //fac.NbJour = (Convert.IsDBNull(reader["NbJour"])) ? null : (Int16?)reader["NbJour"];
                //    //fac.DebutApplication = (Convert.IsDBNull(reader["DebutApplication"])) ? null : (DateTime?)DateTime.Parse((string)reader["DebutApplication"]);
                //    //fac.FinApplication = (Convert.IsDBNull(reader["FinApplication"])) ? null : (DateTime?)DateTime.Parse((string)reader["FinApplication"]);
                //    //fac.Nature = (Convert.IsDBNull(reader["Nature"])) ? String.Empty : (String)reader["Nature"];
                //    //fac.LienFac = (Convert.IsDBNull(reader["LienFac"])) ? String.Empty : (String)reader["LienFac"];
                //    //fac.TopMAJ = (Convert.IsDBNull(reader["TopMAJ"])) ? String.Empty : (String)reader["TopMAJ"];
                //    //fac.Periode = (Convert.IsDBNull(reader["Periode"])) ? String.Empty : (String)reader["Periode"];
                //    //fac.Produit = (Convert.IsDBNull(reader["Produit"])) ? String.Empty : (String)reader["Produit"];
                //    //fac.Formule = (Convert.IsDBNull(reader["Formule"])) ? String.Empty : (String)reader["Formule"];
                //    ////fac.TopAnnul = (Convert.IsDBNull(reader["TopAnnul"])) ? null : (short?)reader["TopAnnul"];
                //    //fac.BarBorneDebut = (Convert.IsDBNull(reader["BarBorneDebut"])) ? 0 : (int?)reader["BarBorneDebut"];
                //    //fac.BarBorneFin = (Convert.IsDBNull(reader["BarBorneFin"])) ? 0 : (int?)reader["BarBorneFin"];

                //    rows.Add(fac);
                //}
                //reader.Close();
            //    return rows;
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(command.CommandText + " :" + ex.Message);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
        }

        //public CsCanalisation RecupererCanalisation(string centre, string client, string produit, int? point, SqlCommand command)
        public CsCanalisation RecupererCanalisation(string centre, string client, string produit, int? point)

        {
            #region Déclaration de variables
            //List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
            #endregion
            try
            {
                #region Traitement
                return Galatee.Entity.Model.FacturationProcedure.RecupererCanalisation(centre, client, produit, point);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public List<CsEvenement> RecupererEvenements(string centre, string ag, string produit, int? point, string lotri, int? status, SqlCommand command)
        public List<Galatee.Entity.Model.EVENEMENT> RecupererEvenements(int? fk_idEvt, galadbEntities pcontext)
        {
            #region Déclaration de variables
            //List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
            #endregion
            try
            {
                #region Traitement
                return Galatee.Entity.Model.FacturationProcedure.RecupererEvenements(fk_idEvt, pcontext);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public int RecupererSommeProfac(string lotri, string jet, string regroup, SqlCommand command)
        public int? RecupererSommeProfac(string lotri, string jet, string regroup)

        {




                #region Déclaration de variables
            //List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
            #endregion
            try
            {
                #region Traitement
                return Galatee.Entity.Model.FacturationProcedure.RecupererSommeProfac(lotri,  jet,  regroup);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }




            ////cn = new SqlConnection(ConnectionString);

            ////command = new SqlCommand();
            ////command.Connection = cn;
            //command.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;

            //try
            //{
            //    if (command.Connection.State == ConnectionState.Closed)
            //        command.Connection.Open();

            //    // Selection de toutes les factures presentent dans ce lot
            //    command.CommandText = "SPX_FAC_SEL_PROFAC_CONSOFAC_PPDIV";
            //    command.Parameters.Clear();
            //    command.Parameters.Add("@lotri", SqlDbType.VarChar, 12).Value = string.IsNullOrEmpty(lotri) ? null : lotri;
            //    command.Parameters.Add("@jet", SqlDbType.VarChar, 12).Value = string.IsNullOrEmpty(jet) ? null : jet;
            //    command.Parameters.Add("@regrou", SqlDbType.VarChar, 4).Value = string.IsNullOrEmpty(regroup) ? null : regroup;

            //    DBBase.SetDBNullParametre(command.Parameters);

            //    SqlDataReader reader = command.ExecuteReader();
                //int somme = 0;
                //while (reader.Read())
                //{
                //    somme = (Convert.IsDBNull(reader["somme"])) ? 0 : (int)reader["somme"];
                //}

                //reader.Close();

        //        return somme;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception(command.CommandText + " :" + ex.Message);
        //throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //if (cn.State == ConnectionState.Open)
        //        //    cn.Close(); // Fermeture de la connection 
        //        //command.Dispose();
            //}
        }

        public List<CsClientLotri> RetourneClientsLot(string lot)
        {


            #region Déclaration de variables
            List<CsClientLotri> rows = new List<CsClientLotri>();
            #endregion
            try
            {
                #region Traitement
                rows = Galatee.Entity.Model.FacturationProcedure.RetourneClientsLot(lot);

                return rows;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }

            ////throw new NotImplementedException();
            //cn = new SqlConnection(ConnectionString);

            //SqlCommand command = new SqlCommand();
            //command.Connection = cn;
            //command.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;          

            //try
            //{               
                //if (command.Connection.State == ConnectionState.Closed)
                //    command.Connection.Open();

                //// Selection de toutes les factures presentent dans ce lot
                //command.CommandText = "SPX_FAC_SELECT_EVENEMENT_LOTRI";
                //command.Parameters.Clear();
                ////command.Parameters.Add("@lotri", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lot) ? null : lot;
                //command.Parameters.AddWithValue("@lotri", string.IsNullOrEmpty(lot) ? null : lot);

                //DBBase.SetDBNullParametre(command.Parameters);

                //SqlDataReader reader = command.ExecuteReader();
                //List<CsClientLotri> rows = new List<CsClientLotri>();
                //while (reader.Read())
                //{
                //    CsClientLotri can = new CsClientLotri();
                //    //can.CENTRE = (Convert.IsDBNull(reader["centre"])) ? String.Empty : (String)reader["centre"];
                //    //can.PRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? String.Empty : (String)reader["PRODUIT"];
                //    //can.FK_POINT = (Convert.IsDBNull(reader["POINT"])) ? 0 : (Int16)reader["POINT"];
                //    ////can.eve = (Convert.IsDBNull(reader["EVENEMENT"])) ? 0 : (int)reader["EVENEMENT"];
                //    //can.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? String.Empty : (String)reader["ORDRE"];
                //    //can.CLIENT = (Convert.IsDBNull(reader["ag"])) ? String.Empty : (String)reader["ag"];
                //    can.PERIODE = (Convert.IsDBNull(reader["periode"])) ? String.Empty : (String)reader["periode"];

                //    rows.Add(can);
                //}

                //reader.Close();
            //    return rows;
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(command.CommandText + " :" + ex.Message);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
        }

        //public List<CsEvenement> RetourneEvenementsHistorique(out DateTime ? datePrec,out string dateAbon, string ag, string periode, int? point, string produit, string centre, string ordre,SqlCommand command)
        public List<CsEvenement> RetourneEvenementsHistorique(out DateTime? datePrec, out string dateAbon, string ag, string periode, int? point, string produit, string centre, string ordre)
        {
            #region Déclaration de variables
            List<CsEvenement> rows = new List<CsEvenement>();
            #endregion
            try
            {
                #region Traitement
                rows = Galatee.Entity.Model.FacturationProcedure.RetourneEvenementsHistorique(out datePrec,out dateAbon, ag, periode, point, produit, centre, ordre);

                return rows;
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }





            ////throw new NotImplementedException();
            ////cn = new SqlConnection(ConnectionString);            
            ////SqlCommand command = new SqlCommand();
            ////command.Connection = cn;
            //command.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;

            //try
            //{
                //if (command.Connection.State == ConnectionState.Closed)
                //    command.Connection.Open();

                //datePrec = null;
                //dateAbon = string.Empty;

                //// Selection de toutes les factures presentent dans ce lot
                //command.CommandText = "SPX_FAC_SELECT_EVENEMENT_HISTORIQUE";
                //command.Parameters.Clear();
                //command.Parameters.Add("@AG", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(ag) ? null : ag;
                //command.Parameters.Add("@PERIODE", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(periode) ? null : periode;
                //command.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(produit) ? null : produit;
                //command.Parameters.Add("@ORDRE", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(ordre) ? null : ordre;
                //command.Parameters.Add("@CENTRE", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(centre) ? null : centre;
                //command.Parameters.Add("@POINT", SqlDbType.SmallInt, 10).Value = point == null ? null : point;

                //DBBase.SetDBNullParametre(command.Parameters);

                //SqlDataReader reader = command.ExecuteReader();
                //List<CsEvenement> rows = new List<CsEvenement>();
                //while (reader.Read())
                //{
                //    CsEvenement can = new CsEvenement();
                //    can.CODEEVT = (Convert.IsDBNull(reader["CodeEvt"])) ? String.Empty : (String)reader["CodeEvt"];
                //    //can.CAS = (Convert.IsDBNull(reader["Cas"])) ? String.Empty : (String)reader["Cas"];
                //    //can.DATEEVT = (Convert.IsDBNull(reader["DATEEVT"])) ? String.Empty : (String)reader["DATEEVT"];
                //    can.CONSO = (Convert.IsDBNull(reader["Conso"])) ? 0 : (int?)reader["Conso"];
                //    //can.TYPECONSO = (Convert.IsDBNull(reader["TypeConso"])) ? 0 : (Int16?)reader["TypeConso"];
                //    can.CONSONONFACTUREE = (Convert.IsDBNull(reader["ConsoNonFacturee"])) ? 0 : (int?)reader["ConsoNonFacturee"];
                //    can.SURFACTURATION = (Convert.IsDBNull(reader["SURFACTURATION"])) ? 0 : (int?)reader["SURFACTURATION"];
                //    can.QTEAREG = (Convert.IsDBNull(reader["QteAReg"])) ? 0 : (int?)reader["QteAReg"];
                //    can.CONSOFAC = (Convert.IsDBNull(reader["ConsoFac"])) ? 0 : (int?)reader["ConsoFac"];
                //    //datePrec = (Convert.IsDBNull(reader["DatePrecEvt"])) ? String.Empty : ((DateTime)reader["DatePrecEvt"]).ToString();
                //    dateAbon = (Convert.IsDBNull(reader["DateAbon"])) ? String.Empty : ((DateTime)reader["DateAbon"]).ToString();
                //dateAbon = DateTime.MinValue.ToString();
                //datePrec = DateTime.MinValue;
                //    rows.Add(can);
                //}

                //reader.Close();

            //    return rows;
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(command.CommandText + " :" + ex.Message);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
        }


        #endregion

        #region Methodes de mise à jour ou d'insertion

        //public bool MajCEntFacAnnule(string centre, string client, string ordre, string facture, string periode)
        //{
        //    cn = new SqlConnection(ConnectionString);

        //    //cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandTimeout = 360;
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        // Selection de toutes les factures presentent dans ce lot
        //        cmd.CommandText = "SPX_FAC_UPDATE_CENTFAC_TYPENC";
        //        cmd.Parameters.Add("@centre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(centre) ? null : centre;
        //        cmd.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(client) ? null : client;
        //        cmd.Parameters.Add("@ordre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(ordre) ? null : ordre;
        //        cmd.Parameters.Add("@facture", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(facture) ? null : facture;
        //        cmd.Parameters.Add("@periode", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(periode) ? null : periode;
        //        DBBase.SetDBNullParametre(cmd.Parameters);

        //        int retour = cmd.ExecuteNonQuery();

        //        //List<CsRedevanceFacture> rows = new List<CsRedevanceFacture>();
        //        //while (reader.Read())
        //        //{

        //        //}
        //        //reader.Close();
        //        //return rows;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(cmd.CommandText + " :" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        /// <summary>
        /// Mise à jour de la table client
        /// </summary>
        /// <param name="rowId"></param>
        /// <param name="centre"></param>
        /// <param name="client"></param>
        /// <param name="ordre"></param>
        /// <param name="facture"></param>
        /// <returns></returns>
        public int MiseAJourClient(string rowId, string centre, string client, string ordre, string facture,SqlCommand command)
        {
            ////cn = new SqlConnection(ConnectionString);

            ////cmd = new SqlCommand();
            ////cmd.Connection = cn;
            ////cmd.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;

            try
            {
                //if (command.Connection.State == ConnectionState.Closed)
                //    command.Connection.Open();

                //// Selection de toutes les factures presentent dans ce lot
                //command.CommandText = "SPX_FAC_UPDATE_CLIENT";
                //command.Parameters.Clear();
                //command.Parameters.Add("@rowId", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(rowId) ? null : rowId;
                //command.Parameters.Add("@centre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(centre) ? null : centre;
                //command.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(client) ? null : client;
                //command.Parameters.Add("@ordre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(ordre) ? null : ordre;
                //command.Parameters.Add("@facture", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(facture) ? null : facture;
                ////command.Parameters.Add("@lienfac", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lienfac) ? null : lienfac;
                //DBBase.SetDBNullParametre(command.Parameters);

                //SqlDataReader reader = command.ExecuteReader();
                List<CsProduitFacture> rows = new List<CsProduitFacture>();
                //while (reader.Read())
                //{
                //    //CsProduitFacture fac = new CsProduitFacture();
                //    //fac.Lotri = (Convert.IsDBNull(reader["lotri"])) ? String.Empty : (String)reader["lotri"];
                //    //fac.Jet = (Convert.IsDBNull(reader["jet"])) ? String.Empty : (String)reader["jet"];
                //    //fac.Centre = (Convert.IsDBNull(reader["Centre"])) ? String.Empty : (String)reader["Centre"];
                //    //fac.Client = (Convert.IsDBNull(reader["Client"])) ? String.Empty : (String)reader["Client"];
                //    //fac.Ordre = (Convert.IsDBNull(reader["Ordre"])) ? String.Empty : (String)reader["Ordre"];
                //    ////fac.RowIdCl = (Convert.IsDBNull(reader["RowIdCl"])) ? String.Empty : (String)reader["RowIdCl"];
                //    //fac.LienFac = (Convert.IsDBNull(reader["LienFac"])) ? String.Empty : (String)reader["LienFac"];
                //    //fac.Facture = (Convert.IsDBNull(reader["Facture"])) ? String.Empty : (String)reader["Facture"];

                //    //rows.Add(fac);
                //}
                //reader.Close();
                return 1;
                //return rows;
            }
            catch (Exception ex)
            {
                //throw new Exception(command.CommandText + " :" + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (cn.State == ConnectionState.Open)
                //    cn.Close(); // Fermeture de la connection 
                //command.Dispose();
            }
        }

        //public void MiseAJourEvenements(List<CsEvenement> events, CsProduitFacture profac, SqlCommand command)
                    public void MiseAJourEvenements(List<CsEvenement> events, CsProduitFacture profac, object command)

        {




            #region Déclaration de variables
            //List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
            #endregion
            try
            {
                #region Traitement
                Galatee.Entity.Model.FacturationProcedure.MiseAJourEvenements(events, profac, command);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }





            ////throw new NotImplementedException();
            //DataTable table = Tools.Utility.ListToDataTable(events);

            ////cn = new SqlConnection(ConnectionString);

            ////command = new SqlCommand();
            ////command.Connection = cn;
            ////command.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandText = "SPX_FAC_MAJ_EVENEMENT";
            
            //try
            //{
                //foreach (CsEvenement evenement in events)
                //{
                //    command.Parameters.Clear();
                //    //command.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(evenement.CENTRE) ? null : evenement.CENTRE;
                //    //command.Parameters.Add("@ag", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(evenement.CLIENT) ? null : evenement.CLIENT;
                //    //command.Parameters.Add("@produit", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(evenement.PRODUIT) ? null : evenement.PRODUIT;
                //    command.Parameters.Add("@facper", SqlDbType.VarChar, 7).Value = string.IsNullOrEmpty(evenement.FACPER) ? null : evenement.FACPER;
                //    command.Parameters.Add("@facture", SqlDbType.VarChar, 7).Value = string.IsNullOrEmpty(evenement.FACTURE) ? null : evenement.FACTURE;
                //    command.Parameters.Add("@lotri", SqlDbType.VarChar, 7).Value = string.IsNullOrEmpty(evenement.LOTRI) ? null : evenement.LOTRI;
                //    //command.Parameters.Add("@matricule", SqlDbType.VarChar, 7).Value = string.IsNullOrEmpty(evenement.MATRICULE) ? null : evenement.MATRICULE;
                //    //command.Parameters.Add("@point", SqlDbType.SmallInt, 7).Value = evenement.POINT == 0 ? 0 : evenement.POINT;
                //    //command.Parameters.Add("@status", SqlDbType.SmallInt, 7).Value = evenement.STATUS == null ? 0 : evenement.STATUS;
                //    //command.Parameters.Add("@EVENEMENT", SqlDbType.SmallInt, 7).Value = evenement.EVENEMENT == 0 ? 0 : evenement.EVENEMENT;
                //    command.Parameters.Add("@qteareg", SqlDbType.Int, 7).Value = evenement.QTEAREG == null ? 0 : evenement.QTEAREG;
                //    command.Parameters.Add("@consononfacturee", SqlDbType.Int, 7).Value = evenement.CONSONONFACTUREE == null ? 0 : evenement.CONSONONFACTUREE;
                //    command.Parameters.Add("@surfacturation", SqlDbType.Int, 7).Value = evenement.SURFACTURATION == null ? 0 : evenement.SURFACTURATION;
                //    //command.Parameters.Add("@typeconso", SqlDbType.SmallInt, 7).Value = evenement.TYPECONSO == null ? 0 : evenement.TYPECONSO;
                //    //command.Parameters.Add("@dmaj", SqlDbType.DateTime, 7).Value = string.IsNullOrEmpty(evenement.DMAJ) ? DateTime.Today : DateTime.Parse(evenement.DMAJ);

                //   // command.Parameters.AddWithValue("@TABLE_EVT", table);

                //    DBBase.SetDBNullParametre(command.Parameters);

                //    if (command.Connection.State == ConnectionState.Closed)
                //        command.Connection.Open();

                //    int retour = command.ExecuteNonQuery();
                //}
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(command.CommandText + " :" + ex.Message);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
        }

        //public int RecupererNbrPieces  

        //public void lancerMaj(string lotri)
        //{
        //    cn = new SqlConnection(ConnectionString);

        //    //cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandTimeout = 360;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "";
        //    cmd.Parameters.Add("@lotri", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lotri) ? null : lotri;

        //    DBBase.SetDBNullParametre(cmd.Parameters);

        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
        //        while (reader.Read())
        //        {
        //            CsEnteteFacture fac = new CsEnteteFacture();
        //            fac.Lotri = (Convert.IsDBNull(reader["lotri"])) ? String.Empty : (String)reader["lotri"];

        //            rows.Add(fac);
        //        }
        //        reader.Close();
        //        //return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(cmd.CommandText + " :" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }

        //}

        //public int MajLotri_Exig_Action(CsLotri lot, string action, int? nombre, decimal? montant, string matricule, SqlCommand command)
        public void MajLotri_Exig_Action(CsLotri lot, string action, int? nombre, decimal? montant, string matricule, galadbEntities pContext)
        {
            #region Déclaration de variables
            //List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
            #endregion
            try
            {
                #region Traitement
                Galatee.Entity.Model.FacturationProcedure.MajLotri_Exig_Action(lot, action, nombre, montant, matricule, pContext);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }





        //    //cn = new SqlConnection(ConnectionString);

        //    //cmd = new SqlCommand();
        //    //command.Connection = cn;
        //    //command.CommandTimeout = 360;
        //    command.CommandType = CommandType.StoredProcedure;

            //try
            //{
                //if (command.Connection.State == ConnectionState.Closed)
                //    command.Connection.Open();

                //// Selection de toutes les factures presentent dans ce lot
                //command.CommandText = "SPX_FAC_MAJ_LOTRI_EXIG_ACTION";
                //command.Parameters.Clear();
                //command.Parameters.Add("@JET", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lot.JET) ? null : lot.JET;
                //command.Parameters.Add("@PERIODE", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lot.PERIODE) ? null : lot.PERIODE;
                //command.Parameters.Add("@action", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(action) ? null : action;
                //command.Parameters.Add("@nombre", SqlDbType.Int, 10).Value = nombre == null ? 0 : nombre;
                //command.Parameters.Add("@montant", SqlDbType.Decimal, 10).Value = montant == null ? 0 : montant;
                //command.Parameters.Add("@matricule", SqlDbType.VarChar, 12).Value =string.IsNullOrEmpty(matricule)? null : matricule;
                //command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "O";
                //command.Parameters.Add("@lotri", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lot.NUMLOTRI) ? null : lot.NUMLOTRI;
                ////command.Parameters.Add("@centre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(lot.FK_CENTRE) ? null : lot.FK_CENTRE;
                ////command.Parameters.Add("@dexig", SqlDbType.DateTime, 10).Value = string.IsNullOrEmpty(lot.DATEEXIG) ? DateTime.Today : DateTime.Parse(lot.DATEEXIG);

                //DBBase.SetDBNullParametre(command.Parameters);

                //int retour = command.ExecuteNonQuery();
                //int retour =-1;
                ////List<CsEvenement> rows = new List<CsEvenement>();
                ////while (reader.Read())
                ////{
                ////    CsEvenement can = new CsEvenement();
                ////    can.CODEEVT = (Convert.IsDBNull(reader["CodeEvt"])) ? String.Empty : (String)reader["CodeEvt"];
                ////    can.CAS = (Convert.IsDBNull(reader["Cas"])) ? String.Empty : (String)reader["Cas"];
                ////    can.DATEEVT = (Convert.IsDBNull(reader["DATEEVT"])) ? String.Empty : (String)reader["DATEEVT"];
                ////    can.CONSO = (Convert.IsDBNull(reader["Conso"])) ? 0 : (int?)reader["Conso"];
                ////    can.TYPECONSO = (Convert.IsDBNull(reader["TypeConso"])) ? 0 : (int?)reader["TypeConso"];
                ////    can.CONSONONFACTUREE = (Convert.IsDBNull(reader["ConsoNonFacturee"])) ? 0 : (int?)reader["ConsoNonFacturee"];
                ////    can.SURFACTURATION = (Convert.IsDBNull(reader["SURFACTURATION"])) ? 0 : (int?)reader["SURFACTURATION"];
                ////    can.QTEAREG = (Convert.IsDBNull(reader["QteAReg"])) ? 0 : (int?)reader["QteAReg"];
                ////    can.CONSOFAC = (Convert.IsDBNull(reader["ConsoFac"])) ? 0 : (int?)reader["ConsoFac"];

                ////    rows.Add(can);
                ////}

                ////reader.Close();

                ////return rows;

            //    return retour;
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(command.CommandText + " :" + ex.Message);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
        }

        //public int MajHistorique(CsHistorique historiqueClient, int? debitAnnuel, SqlCommand command)
        public void MajHistorique(CsHistorique historiqueClient, int? debitAnnuel, object command)

        {





            #region Déclaration de variables
            //List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
            #endregion
            try
            {
                #region Traitement
                Galatee.Entity.Model.FacturationProcedure.MajHistorique(historiqueClient,  debitAnnuel,  command);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }




            ////cn = new SqlConnection(ConnectionString);

            ////command = new SqlCommand();
            ////command.Connection = cn;
            ////command.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;

            //try
            //{
            //    if (command.Connection.State == ConnectionState.Closed)
            //        command.Connection.Open();

            //    command.CommandText = "SPX_FAC_MAJ_HISTORIQUE";
            //    command.Parameters.Clear();
            //    command.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(historiqueClient.FK_CLIENT) ? null : historiqueClient.CLIENT;
            //    command.Parameters.Add("@periode", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.PERIODE) ? null : historiqueClient.PERIODE;
            //    command.Parameters.Add("@point", SqlDbType.SmallInt, 10).Value = historiqueClient.POINT == 0 ? 0 : historiqueClient.POINT;
            //    command.Parameters.Add("@produit", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.PRODUIT) ? null : historiqueClient.PRODUIT;
            //    command.Parameters.Add("@ordre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.ORDRE) ? null : historiqueClient.ORDRE;
            //    command.Parameters.Add("@centre", SqlDbType.VarChar, 12).Value = string.IsNullOrEmpty(historiqueClient.CENTRE) ? null : historiqueClient.CENTRE;
            //    command.Parameters.Add("@cumcons", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMCONS == 0 ? 0 : historiqueClient.CUMCONS;
            //    command.Parameters.Add("@CumConsFacturee", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMCONSFACTUREE == 0 ? 0 : historiqueClient.CUMCONSFACTUREE;
            //    command.Parameters.Add("@CumPer", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMPER == 0 ? 0 : historiqueClient.CUMPER;
            //    command.Parameters.Add("@debitAnnuel", SqlDbType.SmallInt, 10).Value = debitAnnuel == null ? null : debitAnnuel;

            //    DBBase.SetDBNullParametre(command.Parameters);

            //    int retour = command.ExecuteNonQuery();

            //    return retour;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(command.CommandText + " :" + ex.Message);
            //}
            //finally
            //{
            //    //if (cn.State == ConnectionState.Open)
            //    //    cn.Close(); // Fermeture de la connection 
            //    //command.Dispose();
            //}
            //return -1;
        }
        
        public void InsererLclient(List<CsLclient> LignesCompte, object command)

        {
            ////cn = new SqlConnection(ConnectionString);

            ////cmd = new SqlCommand();
            ////cmd.Connection = cn;
            ////cmd.CommandTimeout = 360;
            //command.CommandType = CommandType.StoredProcedure;

            try
            {
            //    if (command.Connection.State == ConnectionState.Closed)
            //        command.Connection.Open();
            //    command.Parameters.Clear();

            //    Utility.InsertionEnBloc(LignesCompte, "LCLIENT", command);
            }
            catch (Exception ex)
            {
                //throw new Exception(command.CommandText + " :" + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                //if (cn.State == ConnectionState.Open)
                //    cn.Close(); // Fermeture de la connection 
                //command.Dispose();
            }


            //    cmd.CommandText = "SPX_FAC_MAJ_HISTORIQUE";
            //    cmd.Parameters.Add("@client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(historiqueClient.CLIENT) ? null : historiqueClient.CLIENT;
            //    cmd.Parameters.Add("@periode", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.PERIODE) ? null : historiqueClient.PERIODE;
            //    cmd.Parameters.Add("@point", SqlDbType.SmallInt, 10).Value = historiqueClient.POINT == 0 ? 0 : historiqueClient.POINT;
            //    cmd.Parameters.Add("@produit", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.PRODUIT) ? null : historiqueClient.PRODUIT;
            //    cmd.Parameters.Add("@ordre", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(historiqueClient.ORDRE) ? null : historiqueClient.ORDRE;
            //    cmd.Parameters.Add("@centre", SqlDbType.VarChar, 12).Value = string.IsNullOrEmpty(historiqueClient.CENTRE) ? null : historiqueClient.CENTRE;
            //    cmd.Parameters.Add("@cumcons", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMCONS == 0 ? 0 : historiqueClient.CUMCONS;
            //    cmd.Parameters.Add("@CumConsFacturee", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMCONSFACTUREE == 0 ? 0 : historiqueClient.CUMCONSFACTUREE;
            //    cmd.Parameters.Add("@CumPer", SqlDbType.SmallInt, 10).Value = historiqueClient.CUMPER == 0 ? 0 : historiqueClient.CUMPER;
            //    cmd.Parameters.Add("@debitAnnuel", SqlDbType.SmallInt, 10).Value = debitAnnuel == null ? null : debitAnnuel;

            //    DBBase.SetDBNullParametre(cmd.Parameters);

            //    int retour = cmd.ExecuteNonQuery();

            //    return retour;

        }

        //public int MiseAJour_Entfac_ProFac_RedFac_Annul(CsEnteteFacture entfacs, SqlCommand command)
        public void MiseAJour_Entfac_ProFac_RedFac_Annul(CsEnteteFacture entfacs, object command)

        {

            #region Déclaration de variables
            //List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
            #endregion
            try
            {
                #region Traitement
                Galatee.Entity.Model.FacturationProcedure.MiseAJour_Entfac_ProFac_RedFac_Annul(entfacs, command);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Update_Entfac_ProFac_RedFac_Annul(CsLafactureClient lstEntfact, galadbEntities pContext)
        {
            try
            {
                #region Traitement

                CsLafactureClient entfacs = new CsLafactureClient();
                List< PROFAC> lsProfact = Entities.ConvertObject<Galatee.Entity.Model.PROFAC, CsProduitFacture>(lstEntfact._LstProfact );
                List< REDFAC> lstRedfac = Entities.ConvertObject<Galatee.Entity.Model.REDFAC, CsRedevanceFacture>(lstEntfact._LstRedFact);
                ENTFAC leEntfac = Entities.ConvertObject<Galatee.Entity.Model.ENTFAC, CsEnteteFacture>(lstEntfact._LeEntatfac  );

                Entities.UpdateEntity<ENTFAC>(leEntfac, pContext);
                Entities.UpdateEntity<REDFAC>(lstRedfac, pContext);
                Entities.UpdateEntity<PROFAC>(lsProfact, pContext);
              

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #endregion


        public bool MiseAJourTableStatistiques(List<CsEnteteFacture> entfacs, List<CsProduitFacture> tousLesProfacs, List<CsRedevanceFacture> tousLesRedFacs)
        {
            #region Déclaration de variables
            //List<CsEnteteFacture> rows = new List<CsEnteteFacture>();
            #endregion
            try
            {
                #region Traitement
                return Galatee.Entity.Model.FacturationProcedure.MiseAJourTableStatistiques(entfacs,tousLesProfacs, tousLesRedFacs);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CsStatFacturation MiseAjourLots(List<CsLotri> lots, bool IsResiliation)
        {
            SqlCommand cmd = DBBase.InitTransaction(ConnectionString);

            DbFacturation db = new DbFacturation();
            CsStatFacturation result = new CsStatFacturation();
            try
            {
                string matricule = lots.First().MATRICULE;
                int nombre = 0;
                decimal ? Montant = 0;
                int volume = 0;
                var lesLotDist = lots.Select(y => new { y.FK_IDCENTRE,y.NUMLOTRI  , y.JET , y.MOISCOMPTA ,y.MATRICULE  }).Distinct();
                foreach (var  lot in lesLotDist)
                {
                    CsStatFacturation leResultat = MiseAJourDuLotCalculer(lot.FK_IDCENTRE, lot.NUMLOTRI, lot.JET, lot.MOISCOMPTA, lot.MATRICULE, cmd);
                      nombre = nombre + leResultat.NombreCalcule;
                      Montant = Montant + leResultat.Montant;
                      volume = volume + leResultat.VolumeCalcule;
                }
                result.Montant = Montant;
                result.NombreCalcule = nombre;
                result.VolumeCalcule = volume;

                cmd.Transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }

        }

        public CsStatFacturation MiseAJourDuLotCalculer(int IdCentre, string Lotri, string Jet, string MoisComptable, string matricule, SqlCommand cmd)
        {
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_MISE_A_JOUR_FACTURATION";

            if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();

            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@lotri", SqlDbType.VarChar,8).Value = Lotri;
            cmd.Parameters.Add("@jet", SqlDbType.VarChar,2).Value = Jet;
            cmd.Parameters.Add("@moiscompt", SqlDbType.VarChar, 6).Value = MoisComptable;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 5).Value = matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                CsStatFacturation c = new CsStatFacturation();

                if (reader.Read())
                {
                    c.NombreCalcule = (Convert.IsDBNull(reader["NOMBRECALCULE"])) ? 0 : (int)reader["NOMBRECALCULE"];
                    c.Montant = (Convert.IsDBNull(reader["MONTANT"])) ? 0 : (decimal)reader["MONTANT"];
                    c.VolumeCalcule = (Convert.IsDBNull(reader["CONSOTOTAL"])) ? 0 : (int)reader["CONSOTOTAL"];
                }

                reader.Close();

                return c;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " ("+ Lotri + ") : " + ex.Message);
            }
        }





        public bool InsererLclient(List<LCLIENT> LignesCompte, galadbEntities pContext)
        {
            try
            {
                return Entities.InsertEntity<LCLIENT>(LignesCompte, pContext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }
        public bool UpdateEvenement(List<Galatee.Entity.Model.EVENEMENT> LignesEvent, galadbEntities pContext)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.EVENEMENT>(LignesEvent, pContext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }
        private void TraitementEvenements(ref List<CsEvenement> events, CsProduitFacture  profac, bool ExistClientAnnulle, string matricule)
        {
            Boolean IsEvenementDePose = false;
            #region parcours des evenements
            foreach (CsEvenement evenement in events)
            {
                if (evenement.CODEEVT == Enumere.EvenementCodeNormale && evenement.CAS == Enumere.CasPoseCompteur)
                    IsEvenementDePose = true;

                if (evenement.NUMEVENEMENT == profac.EVENEMENT)
                {
                    #region traitements des evenements hors pause
                    if (!IsEvenementDePose)
                    {
                        if (evenement.STATUS == Enumere.EvenementMisAJour ||
                            evenement.STATUS == Enumere.EvenementPurger ||
                            evenement.STATUS == Enumere.EvenementFacture)
                        {
                            if (profac.REGROU != "-1")
                            {
                                if (
                                    (!ExistClientAnnulle && (evenement.DATEEVT == profac.DATEEVT) && (evenement.POINT == profac.POINT) &&
                                    (evenement.PERIODE == profac.PERIODE || (evenement.CONSO == 0 && evenement.PERIODE != profac.PERIODE)) &&
                                    evenement.INDEXEVT == profac.NINDEX &&
                                    (evenement.CAS == profac.CAS || (profac.TYPECOMPTEUR  == Enumere.CompteurPrincipal && profac.TFAC == Enumere.CodeForfait)))
                                    ||
                                    (ExistClientAnnulle &&
                                    (evenement.CODEEVT == "50" || evenement.STATUS == Enumere.EvenementMisAJour || evenement.STATUS == Enumere.EvenementPurger))
                                  )
                                {
                                    evenement.FACPER = profac.PERIODE;
                                    if (ExistClientAnnulle)
                                        evenement.FACTURE = profac.LIENFAC;

                                    int? sommeProfac = 0;
                                    if (profac.TFAC == Enumere.CodeNormale)
                                    {
                                        if (profac.TYPECOMPTEUR  == Enumere.CompteurPrincipal)
                                        {
                                            int? sommeTemp = RecupererSommeProfac(profac.LOTRI, profac.JET, profac.REGROU);
                                            sommeProfac = evenement.CONSO - (int?)sommeTemp;
                                        }
                                        else
                                        {
                                            if (profac.CONSOFAC > 0)
                                                sommeProfac = evenement.CONSO - profac.CONSOFAC;
                                            else
                                                sommeProfac = evenement.CONSO + profac.CONSOFAC;
                                        }

                                        if (sommeProfac > 0)
                                        {
                                            if (profac.TFAC == Enumere.CodeNormale)
                                                evenement.CONSONONFACTUREE = sommeProfac;
                                        }
                                        else
                                        {
                                            if (profac.TFAC == Enumere.CodeNormale)
                                            {
                                                if (evenement.CAS == "00") // Pour cas Normal
                                                    evenement.SURFACTURATION = sommeProfac * (-1);
                                                else
                                                    evenement.SURFACTURATION = 0;
                                            }
                                        }
                                    }
                                    else
                                        evenement.CONSONONFACTUREE = evenement.CONSO;
                                }
                                if (ExistClientAnnulle)
                                {
                                    if (evenement.STATUS == Enumere.EvenementMisAJour || evenement.STATUS == Enumere.EvenementPurger)
                                        evenement.STATUS = Enumere.EvenementAnnule;
                                    else
                                    { /* Anomalie num 53, code 2 */ }
                                }
                                else
                                    evenement.STATUS = Enumere.EvenementMisAJour;

                                int typeConso;
                                bool retour = Int32.TryParse(profac.TFAC, out typeConso);
                                evenement.TYPECONSO = retour ? (int?)typeConso : null;

                                profac.REGROU = "-1";
                                evenement.DATEMODIFICATION = System.DateTime.Now ;

                            }
                        }
                    }
                    #endregion

                    #region traitements des evenements de pose
                    else
                    {
                        // evenement.FACPER = facPer
                        if (ExistClientAnnulle)
                        {
                            if (evenement.STATUS == Enumere.EvenementMisAJour || evenement.STATUS == Enumere.EvenementPurger)
                                evenement.STATUS = Enumere.EvenementAnnule;
                            else
                            { /* anomalie 53,2 */ }
                        }
                        else
                            evenement.STATUS = Enumere.EvenementMisAJour;
                        if (evenement.DATEEVT == profac.DATEEVT && profac.CAS == Enumere.CasPoseCompteur)
                        {
                            evenement.TYPECONSO = Int32.Parse(profac.TFAC);
                            profac.REGROU = "-1";
                        }
                    }

                    #endregion
                }

            }
            #endregion
        }


        public void CommitTransaction(object command)
        {
           
            try
            {
                #region Traitement
                 Galatee.Entity.Model.FacturationProcedure.CommitTransaction(command);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsAction> retourneActionFact(CsLotri leLot)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.retourneActionFact(leLot);
                return Entities.GetEntityListFromQuery<CsAction>(dt);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RetirerClientLotFact(List<CsEvenement> lstEvenetLot)
        {
            SqlCommand leConnect = Galatee.Tools.Utility.OpenTransation(ConnectionString);
            try
            {
                var lesEvtDist = lstEvenetLot.Select(y => new { y.FK_IDCENTRE, y.CENTRE , y.CLIENT ,y.ORDRE ,y.LOTRI ,y.STATUS,y.PERIODE ,y.FACTURE   }).Distinct();
                foreach (var item in lesEvtDist)
                    RetirerClientLotFact(item.LOTRI, item.FK_IDCENTRE, item.CENTRE, item.CLIENT, item.ORDRE, item.STATUS.Value,item.PERIODE ,item.FACTURE , leConnect);
              return   Galatee.Tools.Utility.Commit(leConnect);
            }
            catch (Exception ex)
            {
                Galatee.Tools.Utility.Rollback(leConnect);
                return false;
            }
        }
        private void   RetirerClientLotFact(string lotri,int Fk_idcentre,string  Centre,string Client ,
                                           string Ordre, int Status, string periode, string facture, SqlCommand cmd)
        {
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_REJETERCLIENT";
            if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@lotri", SqlDbType.VarChar, 8).Value = lotri;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = Fk_idcentre;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = Centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = Ordre;
            cmd.Parameters.Add("@Facture", SqlDbType.VarChar, 9).Value = facture;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = periode;
            cmd.Parameters.Add("@Status", SqlDbType.Int ).Value = Status;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                  cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }


        public bool ValiderCreationFactureIsole(List<CsEvenement> lstEvenement)
        {
            SupprimerEvenementIsoleExistant(lstEvenement);
            bool? resultat = Galatee.Entity.Model.FacturationProcedure.ValiderCreationFactureIsole(lstEvenement);
            bool result = new DBIndex().CreationCtarCompt(lstEvenement.First().FK_IDABON.Value, lstEvenement.First().LOTRI, lstEvenement.First().PERIODE, lstEvenement.First().USERCREATION, lstEvenement.First().FK_IDCENTRE, lstEvenement.First().FK_IDPRODUIT, lstEvenement.First().DATEEVT);
            return result;
        }




        public void SupprimerEvenementIsoleExistant(List<CsEvenement> lstEvenement)
        {

            cn = new SqlConnection(ConnectionString);


            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_DELETE_EVENEMENT_ISOLE";

            cmd.Parameters.Clear();

            cmd.Parameters.Add("@FK_IDABON", SqlDbType.Int).Value = lstEvenement.First().FK_IDABON.Value;
            cmd.Parameters.Add("@LOTRI", SqlDbType.VarChar, 8).Value = lstEvenement.First().LOTRI;
            cmd.Parameters.Add("@PERIODE", SqlDbType.VarChar, 6).Value = lstEvenement.First().PERIODE;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar, 2).Value = lstEvenement.First().PRODUIT;

            DBBase.SetDBNullParametre(cmd.Parameters);

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }



            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }



        public void UpdateEvtfacteBulk(List<CsMapperEvenement> _Levt, SqlCommand pContext)
        {
            try
            {
                List<string> ProprieteAModifier = new List<string>();
                ProprieteAModifier.Add("STATUS");
                ProprieteAModifier.Add("USERMODIFICATION");
                ProprieteAModifier.Add("DATEMODIFICATION");

                List<string> ProprieteDeJointure = new List<string>();
                ProprieteDeJointure.Add("PK_ID");

                string Sufixe = string.Empty;
                var properties = _Levt.First().GetType().GetProperties();

                int NbrPropertie = properties.Count();
                int Passage = 0;
                foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                {
                    Passage += 1;
                    var type = f.PropertyType.FullName;
                    string TypeVal = type.ToString();
                    if (type.Contains("System.String"))
                        TypeVal = "Varchar(10)";
                    else if (type.Contains("System.Int32") || type.Contains("System.Int16"))
                        TypeVal = "int";
                    else if (type.Contains("System.Boolean"))
                        TypeVal = "bit";
                    else if (type.Contains("System.DateTime"))
                        TypeVal = "datetime";
                    else if (type.Contains("System.Decimal"))
                        TypeVal = "numeric(38, 10)";
                    else if (type.Contains("System.Byte"))
                        TypeVal = "tinyint";

                    if (Passage == NbrPropertie)
                        Sufixe += f.Name + " " + TypeVal + " NULL ";
                    else
                        Sufixe += f.Name + " " + TypeVal + " NULL " + " ,";
                }
                string CreationTableTemp = "CREATE TABLE #TmpTable( " + Sufixe + ")";

                DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(_Levt);
                Galatee.Tools.Utility.UpdateData(ProprieteAModifier, ProprieteDeJointure, TablePere, "EVENEMENT", CreationTableTemp, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Redevance
        public List<CsRedevance> LoadAllRedevance()
        {
            List<CsRedevance> list = new List<CsRedevance>(); 
            List<CsRedevance> nlist = new List<CsRedevance>(); 
            try
            {
                //return FacturationProcedure.LoadAllRedevance();
                //list = FacturationProcedure.LoadAllRedevance();

                //ADO .Net from Entity : Stephen 29-01-2019 
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("REDEVANCE");
                //list = Galatee.Tools.Utility.GetEntityListFromQuery<CsRedevance>(dt);
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsRedevance> _LstItem = new List<CsRedevance>();
                _LstItem = db.LoadAllRedevance();
                return _LstItem;
                
                foreach (CsRedevance st in list)
                {
                    st.TRANCHEREDEVANCE.AddRange(RemplirTrancheRedevance(st.PK_ID));
                    nlist.Add(st);
                }
                return nlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private List<CsTrancheRedevence> RemplirTrancheRedevance(int idRedevance)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_GET_TRRANCHE_REDEVANCE";

            cmd.Parameters.Clear();

            cmd.Parameters.Add("@idRedevance", SqlDbType.Int ).Value = idRedevance;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<CsTrancheRedevence> list = new List<CsTrancheRedevence>();
                CsTrancheRedevence c = null;

                while (reader.Read())
                {
                    c = new CsTrancheRedevence();

                    c.PK_ID = (Convert.IsDBNull(reader["PK_ID"])) ? 0 : (int)reader["PK_ID"];
                    c.FK_IDREDEVANCE = (Convert.IsDBNull(reader["FK_IDREDEVANCE"])) ? 0 : (int)reader["FK_IDREDEVANCE"];
                    c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? (byte)0 : (byte)reader["ORDRE"];
                    c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (string)reader["LIBELLE"];
                    c.GRATUIT = (Convert.IsDBNull(reader["GRATUIT"])) ? false : (bool)reader["GRATUIT"];
                    list.Add(c);
                }

                reader.Close();

                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }

        
        }


        public bool SaveRedevance(List<CsRedevance> ListeLotsScelleRecuToUpdate, List<CsRedevance> ListeLotsScelleRecuToInserte, List<CsRedevance> ListeLotsScelleRecuToDelete)
        {
            //try
            //{
            //    return FacturationProcedure.SaveRedevance(ListeLotsScelleRecuToUpdate, ListeLotsScelleRecuToInserte, ListeLotsScelleRecuToDelete);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return true;
        }
        #endregion

        public List<CsEnteteFacture> retourneFactureDecroissance(List<int> lstIdCentre, CsLotri leLot)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.retourneFactureDecroissance(lstIdCentre,leLot);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt).OrderByDescending(t=>t.TOTFTTC).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsClient> VerifieClient(List<CsLotri> lesLot, string Client)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.RetourneClientLot(Client, lesLot.Select(y=>y.FK_IDCENTRE).ToList());
                List<CsClient> lstClient = Entities.GetEntityListFromQuery<CsClient >(dt).ToList();

                if (lstClient != null && lstClient.Count != 0)
                {
                    foreach (var item in lstClient)
                    {
                        string OrdreMax = lstClient.Where(t=> t.FK_IDCENTRE == item.FK_IDCENTRE && t.FK_IDABON == item.FK_IDABON) .Max(cl => cl.ORDRE);
                        item.ORDRE = OrdreMax;
                    }
                    return lstClient;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool  VerifieExisteLotClient(CsClient LeClient ,string Numlot)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.FacturationProcedure.VerifieClientExisteLot(LeClient, Numlot);
                List<CsEvenement> lstEvnt = Entities.GetEntityListFromQuery<CsEvenement>(dt).ToList();
                if (lstEvnt != null && lstEvnt.Count != 0)
                    return true ;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false ;
            }
        }
        public List<CsEvenement> RetourneEvenementCorrectionIndex(CsClient LeClient)
        {
            return RetourneEvenementCorrectionIndexSpx(LeClient.FK_IDCENTRE.Value, LeClient.FK_IDABON.Value, LeClient.FK_IDPRODUIT.Value, LeClient.PERIODE);
        }
        public List<CsEvenement> RetourneEvenementCorrectionIndexSpx(int fk_idCentre, int fk_idAbonnement, int Fk_idproduit, string periode)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_SELECT_FAC_EVENEMENTCLIENTCORRECTIONINDEX";

            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = fk_idCentre;
            cmd.Parameters.Add("@IdAbonnement", SqlDbType.Int).Value = fk_idAbonnement;
            cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = Fk_idproduit;
            cmd.Parameters.Add("@nombrePeriodeEstimationConso", SqlDbType.Int).Value = 12;
            cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public List<CsEvenement> RetourneEvenement(CsClient LeClient)
        {
            return RetourneEvenementSpx(LeClient.FK_IDCENTRE.Value, LeClient.FK_IDABON.Value, LeClient.FK_IDPRODUIT.Value, LeClient.PERIODE);
        }
        public List<CsEvenement> RetourneEvenementSpx(int fk_idCentre, int fk_idAbonnement, int Fk_idproduit, string periode)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_SELECT_EVENEMENTCLIENT";

            cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = fk_idCentre;
            cmd.Parameters.Add("@IdAbonnement", SqlDbType.Int).Value = fk_idAbonnement;
            cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = Fk_idproduit;
            cmd.Parameters.Add("@nombrePeriodeEstimationConso", SqlDbType.Int).Value = 12;
            cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement >(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEnteteFacture> RetourneEnteteFacturesAbo07Spx(List<int> lstIdEntFac)
        {

            cn = new SqlConnection(Abo07ConnectionString);
            string LstIdEntfac = DBBase.RetourneStringListeObject(lstIdEntFac);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_CENTFAC";
            cmd.Parameters.Add("@FK_IdCENTFAC", SqlDbType.VarChar, int.MaxValue).Value = LstIdEntfac;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsProduitFacture> RetourneProfacFacturesAbo07Spx(List<int> lstIdEntFac)
        {

            cn = new SqlConnection(Abo07ConnectionString);
            string LstIdEntfac = DBBase.RetourneStringListeObject(lstIdEntFac);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_CPROFAC";
            cmd.Parameters.Add("@FK_IdCENTFAC", SqlDbType.VarChar, int.MaxValue).Value = LstIdEntfac;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsProduitFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsRedevanceFacture> RetourneRedfacFacturesAbo07Spx(List<int> lstIdEntFac)
        {

            cn = new SqlConnection(Abo07ConnectionString);
            string LstIdEntfac = DBBase.RetourneStringListeObject(lstIdEntFac);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_CREDFAC";
            cmd.Parameters.Add("@FK_IdCENTFAC", SqlDbType.VarChar, int.MaxValue).Value = LstIdEntfac;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEnteteFacture> RetourneEnteteFacturesEditionSpx(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient, bool DejaMiseAjour, bool IsLotIsole)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_ENTFAC";

            cmd.Parameters.Add("@lotri", SqlDbType.VarChar ,8).Value = leLotSelect.NUMLOTRI ;
            cmd.Parameters.Add("@jet", SqlDbType.VarChar, 2).Value = leLotSelect.JET ;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leLotSelect.CENTRE ;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leLotSelect.FK_IDCENTRE;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar ,6).Value = leLotSelect.USERCREATION ;

            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = leClient.CENTRE ;
            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value =leClient.REFCLIENT ;
            cmd.Parameters.Add("@OrdreReprise", SqlDbType.VarChar, 2).Value = leClient.ORDRE ;

            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = laTournee.CENTRE ;
            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = laTournee.TOURNEDEBUT ;
            cmd.Parameters.Add("@TourneeFin", SqlDbType.VarChar, 20).Value = laTournee.TOURNEFIN ;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = leLotSelect.PERIODE;
            cmd.Parameters.Add("@IsoleDejaMiseAjour", SqlDbType.VarChar, 6).Value = DejaMiseAjour;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + leLotSelect.NUMLOTRI + ") : " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsProduitFacture> RetourneProfacFacturesEditionSpx(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient, bool DejaMiseAjour, bool IsLotIsole)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_PROFAC";

            cmd.Parameters.Add("@lotri", SqlDbType.VarChar ,8).Value = leLotSelect.NUMLOTRI;
            cmd.Parameters.Add("@jet", SqlDbType.VarChar, 2).Value = leLotSelect.JET;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leLotSelect.CENTRE;

            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leLotSelect.FK_IDCENTRE;
            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = leClient.CENTRE ;
            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value = leClient.REFCLIENT ;
            cmd.Parameters.Add("@OrdreReprise", SqlDbType.VarChar, 2).Value = leClient.ORDRE;


            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = laTournee.CENTRE ;
            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = laTournee.TOURNEDEBUT ;
            cmd.Parameters.Add("@TourneeFin", SqlDbType.VarChar, 20).Value = laTournee.TOURNEFIN ;

            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = leLotSelect.USERCREATION;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = leLotSelect.PERIODE;
            cmd.Parameters.Add("@IsoleDejaMiseAjour", SqlDbType.VarChar, 6).Value = DejaMiseAjour;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsProduitFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + leLotSelect.NUMLOTRI + ") : " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsRedevanceFacture> RetourneRedfacFacturesEditionSpx(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient, bool DejaMiseAjour, bool IsLotIsole)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_REDFAC";

            cmd.Parameters.Add("@lotri", SqlDbType.VarChar ,8).Value = leLotSelect.NUMLOTRI;
            cmd.Parameters.Add("@jet", SqlDbType.VarChar, 2).Value = leLotSelect.JET;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leLotSelect.CENTRE;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leLotSelect.FK_IDCENTRE;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 6).Value = leLotSelect.USERCREATION;

            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = leClient.CENTRE ;
            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value = leClient.REFCLIENT ;
            cmd.Parameters.Add("@OrdreReprise", SqlDbType.VarChar, 2).Value = leClient.ORDRE;

            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = laTournee.CENTRE ;
            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = laTournee.TOURNEDEBUT ;
            cmd.Parameters.Add("@TourneeFin", SqlDbType.VarChar, 20).Value = laTournee.TOURNEFIN ;

            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = leLotSelect.PERIODE;
            cmd.Parameters.Add("@IsoleDejaMiseAjour", SqlDbType.VarChar, 6).Value = DejaMiseAjour;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + " (" + leLotSelect.NUMLOTRI + ") : " + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEnteteFacture> RetourneEnteteFacturesEditionPeriodeSpx(CsCentre leCentre, string PeriodeDebut, string PeriodeFin, string centreTournee,
                     string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
                     string centreStop, string clientStop, bool IsLotIsole)
                        {

                            cn = new SqlConnection(ConnectionString);

                            cmd = new SqlCommand();
                            cmd.Connection = cn;
                            cmd.CommandTimeout = 3000;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SPX_FAC_RETOURNE_ENTFACPERIODE";

                            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leCentre.CODE ;
                            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leCentre.PK_ID ;

                            cmd.Parameters.Add("@PeriodeDebut", SqlDbType.VarChar, 6).Value = PeriodeDebut ;
                            cmd.Parameters.Add("@PeriodeFin", SqlDbType.VarChar, 6).Value = PeriodeFin ;

                            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = centreReprise;
                            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value = clientReprise;

                            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = centreTournee;
                            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = tourneeDebut;
                            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
                            DBBase.SetDBNullParametre(cmd.Parameters);
                            try
                            {
                                if (cn.State == ConnectionState.Closed)
                                    cn.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(cmd.CommandText + ":" + ex.Message);
                            }
                            finally
                            {
                                if (cn.State == ConnectionState.Open)
                                    cn.Close(); // Fermeture de la connection 
                                cmd.Dispose();
                            }
                        }

        public List<CsProduitFacture> RetourneProfacFacturesEditionPeriodeSpx(CsCentre leCentre, string PeriodeDebut, string PeriodeFin, string centreTournee,
                     string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
                     string centreStop, string clientStop, bool IsLotIsole)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_PROFACPERIODE";

            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leCentre.CODE ;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leCentre.PK_ID ;

            cmd.Parameters.Add("@PeriodeDebut", SqlDbType.VarChar, 6).Value = PeriodeDebut;
            cmd.Parameters.Add("@PeriodeFin", SqlDbType.VarChar, 6).Value = PeriodeFin;

            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = centreReprise;
            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value = clientReprise;

            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = centreTournee;
            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = tourneeDebut;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsProduitFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsRedevanceFacture> RetourneRedfacFacturesEditionPeriodeSpx(CsCentre leCentre, string PeriodeDebut, string PeriodeFin, string centreTournee,
                     string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
                     string centreStop, string clientStop, bool IsLotIsole)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_REDFACPERIODE";

            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = leCentre.CODE ;
            cmd.Parameters.Add("@Fk_idcentre", SqlDbType.Int).Value = leCentre.PK_ID ;

            cmd.Parameters.Add("@PeriodeDebut", SqlDbType.VarChar, 6).Value = PeriodeDebut;
            cmd.Parameters.Add("@PeriodeFin", SqlDbType.VarChar, 6).Value = PeriodeFin;

            cmd.Parameters.Add("@CentreReprise", SqlDbType.VarChar, 3).Value = centreReprise;
            cmd.Parameters.Add("@ClientReprise", SqlDbType.VarChar, 20).Value = clientReprise;

            cmd.Parameters.Add("@CentreTournee", SqlDbType.VarChar, 3).Value = centreTournee;
            cmd.Parameters.Add("@TourneeDebut", SqlDbType.VarChar, 20).Value = tourneeDebut;
            cmd.Parameters.Add("@IsIsole", SqlDbType.Bit).Value = IsLotIsole;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public List<CsEnteteFacture> RetourneEnteteFacturesEditionRegroupeSpx(string regroupementDebut, string regroupementFin, string LstPeriode, string Produit, int? idcentre)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_ENTFAC_REGROUPEMENT";

            cmd.Parameters.Add("@regroupementDebut", SqlDbType.VarChar, 10).Value = regroupementDebut;
            cmd.Parameters.Add("@regroupementFin", SqlDbType.VarChar, 10).Value = regroupementFin;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = LstPeriode;
            cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = Produit;
            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEnteteFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsProduitFacture> RetourneProfacFacturesEditionRegroupeSpx(string regroupementDebut, string regroupementFin, string LstPeriode, string Produit, int? idcentre)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_PROFAC_REGROUPEMENT";

            cmd.Parameters.Add("@regroupementDebut", SqlDbType.VarChar, 10).Value = regroupementDebut;
            cmd.Parameters.Add("@regroupementFin", SqlDbType.VarChar, 10).Value = regroupementFin;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = LstPeriode;
            cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = Produit;
            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsProduitFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsRedevanceFacture> RetourneRedfacFacturesEditionRegroupeSpx(string regroupementDebut, string regroupementFin, string LstPeriode, string Produit, int? idcentre)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_RETOURNE_REDFAC_REGROUPEMENT";

            cmd.Parameters.Add("@regroupementDebut", SqlDbType.VarChar, 10).Value = regroupementDebut;
            cmd.Parameters.Add("@regroupementFin", SqlDbType.VarChar, 10).Value = regroupementFin;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = LstPeriode;
            cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = Produit;
            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public List<CsFactureClient> RetourneFacturesRegroupement(string regroupementDebut,string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre,string rdlc)
        {
            try
            {
                List<CsEnteteFacture> lstEnteteFacture = new List<Structure.CsEnteteFacture>();
                List<CsProduitFacture> lstProduitFacture = new List<Structure.CsProduitFacture>();
                List<CsRedevanceFacture> lstRedevanceFacture = new List<Structure.CsRedevanceFacture>();

                foreach (string periode in LstPeriode)
                {
                    foreach (string produit in Produit)
                    {
                        lstEnteteFacture.AddRange(RetourneEnteteFacturesEditionRegroupeSpx(regroupementDebut, regroupementFin, periode, produit, idcentre));
                        lstProduitFacture.AddRange(RetourneProfacFacturesEditionRegroupeSpx(regroupementDebut, regroupementFin, periode, produit, idcentre));
                        lstRedevanceFacture.AddRange(RetourneRedfacFacturesEditionRegroupeSpx(regroupementDebut, regroupementFin, periode, produit, idcentre));
                    }
                }
                List<CsFactureClient> resultat = new List<Structure.CsFactureClient>();
                if (rdlc != "FactureRegroupe")
                {
                    foreach (string item in Produit)
                    {
                        if (lstEnteteFacture.Where(t => t.PRODUIT == item).ToList().Count == 0) continue;
                        if (item == Enumere.ElectriciteMT)
                        {
                            if (rdlc == "FactureSimple")
                                resultat.AddRange(EditionFactureMt(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                            else if (rdlc == "FactureDetail")
                                resultat.AddRange(EditionFactureBorderauxMT(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                        }
                        else
                        {

                            if (rdlc == "FactureSimple")
                            {
                                if (item == Enumere.Eau)
                                    resultat.AddRange(EditionFactureEau(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList().ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                                else
                                    resultat.AddRange(EditionFacture(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList().ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                            }
                            else if (rdlc == "FactureDetail")
                                resultat.AddRange(EditionFactureBorderaux(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList().ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                            else if (rdlc == "FactureSimpleO")
                                resultat.AddRange(EditionFactureEau(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList().ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                        }

                        if (rdlc == "BordereauSimple")
                            resultat.AddRange(EditionFactureBordereauSimple(lstEnteteFacture.Where(t => t.PRODUIT == item).OrderBy(o => o.REGROUPEMENT).ThenBy(p => p.FK_IDCENTRE).ThenBy(m => m.ORDTOUR).ToList().ToList(), lstProduitFacture.Where(t => t.PRODUIT == item).ToList(), lstRedevanceFacture.Where(t => t.PRODUIT == item).ToList()));
                    }
                }
                if (rdlc == "FactureRegroupe")
                    resultat.AddRange(EditionFactureRegrouper(lstEnteteFacture, lstProduitFacture, lstRedevanceFacture));
                return resultat;

            }
            catch (Exception ex )
            {
                throw;
            }
        }

        public List<CsLotri> ChargerLotriNonMisAJours(List<int> lstCentre)
        {
            try
            {
                List<CsLotri> lesLot = Galatee.Entity.Model.FacturationProcedure.ChargerLotriNonMisAJours(lstCentre);
                return lesLot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 

 

        public List<CsLotri> ChargerLotriPourRejetClient(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant,string Periode)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourRejetClientSpx(item.Key, item.Value, UserConnect, IsLotCourant, Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriPourEditionIndex(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourEditionIndexSPX(item.Key, item.Value, IsLotCourant, Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriPourSaisie(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode, string UserConnect)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourSaisieSPX(item.Value,  IsLotCourant, Periode,UserConnect));
                return lstLotri;
               
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriPourEdition(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant, string Periode)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourEditionSpx(item.Key, item.Value, UserConnect, IsLotCourant, Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriPourDefacturation(Dictionary<string, List<int>> lstSiteCentre, string UserConnect,bool IsValidation)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourDefacturationSpx(item.Key, item.Value, UserConnect,IsValidation));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsLotri > ChargerLotriPourCalcul(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant, string Periode)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourCalculSpx(item.Key, item.Value, UserConnect, IsLotCourant, Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLotri> ChargerLotriPourMiseAJour(List<int> lstCentre, string UserConnect)
        {
            try
            {
              return   ChargerLotriPourMiseAJourSpx(lstCentre, UserConnect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotri> ChargerLotriPourEnquete(Dictionary<string, List<int>> lstSiteCentre,bool IsLotCourant, string Periode)
        {
            try
            {
                List<CsLotri> lstLotri = new List<Structure.CsLotri>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerLotriPourEnqueteSpx(item.Key, item.Value, Periode, IsLotCourant));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsLotri> ChargerLotriPourRejetClientSpx(string CodeSite, List<int> lstCentre, string Matricule, bool IsLotCourant,string Periode)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURREJETFACTURE";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue ).Value = idcentre;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsLotri> ChargerLotriPourEditionIndexSPX(string CodeSite, List<int> lstCentre, bool IsFacturationCourante, string Periode)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOUREDITIONINDEX";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@IsFacturationCourante", SqlDbType.Bit).Value = IsFacturationCourante;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsLotri> ChargerLotriPourSaisieSPX( List<int> lstCentre, bool IsFacturationCourante,string Periode,string Matricule)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURSAISIE";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
            cmd.Parameters.Add("@IsFacturationCourante", SqlDbType.Bit).Value = IsFacturationCourante;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsLotri> ChargerLotriPourEditionSpx(string CodeSite, List<int> lstCentre, string Matricule, bool IsLotCourant,string Periode)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOUREDITION";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IsFacturationCourante", SqlDbType.Bit).Value = IsLotCourant;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsLotri> ChargerLotriPourDefacturationSpx(string CodeSite, List<int> lstCentre, string Matricule, bool IsValidation)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURDEFACTURATION";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue ).Value = idcentre;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IsValidation", SqlDbType.Bit).Value = IsValidation;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsLotri> ChargerLotriPourCalculSpx(string CodeSite, List<int> lstCentre, string Matricule, bool IsLotCourant, string Periode)
        {

            cn = new SqlConnection(ConnectionString);

            string idcentre = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURCALCUL";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;
            cmd.Parameters.Add("@IsFacturationCourante", SqlDbType.Bit).Value = IsLotCourant;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt) ;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public List<CsLotri> ChargerLotriPourEnqueteSpx(string CodeSite, List<int> lstCentre, string Periode, bool IsFacturationCourante)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURENQUETE";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue ).Value = idcentre;
            cmd.Parameters.Add("@IsFacturationCourante", SqlDbType.Bit).Value = IsFacturationCourante;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;


            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


     
        public List<CsLotri> ChargerLotriPourMiseAJourSpx(List<int> lstCentre ,string Matricule)
        {

            cn = new SqlConnection(ConnectionString);
            string idcentre = DBBase.RetourneStringListeObject(lstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERLOTPOURMISEAJOUR";

            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = Matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotri>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEvenement> ChargerDetailCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode, List<string> ListCas)
        {
            try
            {
                List<CsEvenement> lstLotri = new List<Structure.CsEvenement>();
                foreach (var item in lstSiteCentre)
                {
                    foreach (var Cas in ListCas)
                        lstLotri.AddRange(ChargerDetailCasFactureSpx(item.Key, item.Value, Lotri, Periode, Cas));
                }
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> ChargerCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode)
        {
            try
            {
                List<CsEvenement> lstLotri = new List<Structure.CsEvenement>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerCasFactureSpx(item.Key, item.Value, Lotri, Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> ChargerDetailCasFactureSpx(string CodeSite, List<int> lstCentre, string lotri, string Periode, string Cas)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 30000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERCASFACTURATIONDETAIL";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = lotri;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
            cmd.Parameters.Add("@Cas", SqlDbType.VarChar, 2).Value = Cas;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsEvenement> ChargerCasFactureSpx(string CodeSite, List<int> lstCentre, string lotri, string Periode)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 30000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_CHARGERCASFACTURATION";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = lotri;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEvenement> ChargerClientNonConstituer(Dictionary<string, List<int>> lstSiteCentre,string Periode)
        {
            try
            {
                List<CsEvenement> lstLotri = new List<Structure.CsEvenement>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(ChargerClientNonConstituerSpx(item.Key, item.Value,Periode));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void InsertAffectationReleveur(CsTournee tournrel, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_FAC_INSERT_TOURNEERELEVEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@FK_IDTOURNEE", SqlDbType.Int).Value = tournrel.FK_IDTOURNEE;
            cmds.Parameters.Add("@FK_IDRELEVEUR", SqlDbType.Int).Value = tournrel.FK_IDRELEVEUR;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = tournrel.USERCREATION;
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }


        public List<CsEvenement> ChargerClientNonConstituerSpx(string CodeSite, List<int> lstCentre, string Periode)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_FAC_ABONNEACTIFNONCONSTITUER";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsEvenement> EtatStatistique(Dictionary<string, List<int>> lstSiteCentre, string Periode, string Lotri,string TypeEtat)
        {
            try
            {
                List<CsEvenement> lstLotri = new List<Structure.CsEvenement>();
                foreach (var item in lstSiteCentre)
                    lstLotri.AddRange(EtatStatistiqueSpx(item.Key, item.Value, Periode, Lotri, TypeEtat));
                return lstLotri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsEvenement> EtatStatistiqueSpx(string CodeSite, List<int> lstCentre, string Periode, string Lotri, string TypeEtat)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            if (TypeEtat == "1" )
            cmd.CommandText = "SPX_FAC_ABONNECONSTITUERNONSAISI";
            else if (TypeEtat == "2")
                cmd.CommandText = "SPX_FAC_ABONNECONSTITUERNONCALCULER";
            else if (TypeEtat == "3")
                cmd.CommandText = "SPX_FAC_ABONNECALCULENONMAJ";
            
           

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
            cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = Lotri ;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsComparaisonFacture> ControleFacturationSpx(List<int> LstCentre, string PerideDebut, string Lotri1, string PeriodeFin,string Lotri2, bool IsMT)
        {

            cn = new SqlConnection(Abo07ConnectionString);

            string Centre = DBBase.RetourneStringListeObject(LstCentre);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (IsMT)
                cmd.CommandText = "SPX_FAC_VERIFIEFACTURATIONMT";
            else
                cmd.CommandText = "SPX_FAC_VERIFIEFACTURATION";


            cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = Centre;
            cmd.Parameters.Add("@Periode1", SqlDbType.VarChar,6).Value = PerideDebut;
            cmd.Parameters.Add("@Lotri1", SqlDbType.VarChar,8).Value = Lotri1;
            cmd.Parameters.Add("@Periode2", SqlDbType.VarChar,6).Value = PeriodeFin;
            cmd.Parameters.Add("@Lotri2", SqlDbType.VarChar,8).Value = Lotri2;


            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsComparaisonFacture>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }



        public  bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter)
        {
            SqlCommand laCommande = new SqlCommand();
            laCommande = DBBase.InitTransaction(ConnectionString);

            try
            {

                foreach (CsTournee item in ListeDesTourneAAffecter)
                {
                    if (laCommande.Transaction == null)
                        laCommande = DBBase.InitTransaction(ConnectionString);

                    InsertAffectationReleveur(item, laCommande);
                    laCommande.Transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }
        }



/*
        public  bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter)
        {
            try
            {
                List<int?> lstIdTourne = ListeDesTourneAAffecter.Select(u => u.FK_IDTOURNEE).ToList();
                int? IdReleveur = ListeDesTourneAAffecter.First().FK_IDRELEVEUR;

                List<TOURNEERELEVEUR> TourneReleveur = new List<TOURNEERELEVEUR>();
                List<TOURNEERELEVEUR> TourneReleveurAutreAfermer = new List<TOURNEERELEVEUR>();
                using (galadbEntities ctx = new galadbEntities())
                {
                    //On récupere les ancienne tournées du releveur concerné
                    TourneReleveur = ctx.TOURNEERELEVEUR.Where(t => t.FK_IDRELEVEUR == IdReleveur && t.DATEFIN == null).ToList();

                    //On récupere les nouvelles tournées à attribué  mais qui n'était pas liées au releveur concerné qui était peut etre attribué à un autres
                    //parmis les trné ki n l8 appartienne pa kl sn ceux kon veu lui affecté maintenan
                    TourneReleveurAutreAfermer = ctx.TOURNEERELEVEUR.Where(t => t.FK_IDRELEVEUR != IdReleveur && lstIdTourne.Contains(t.FK_IDTOURNEE)).ToList();
                }
                //On récupere les ancienne tournées du releveur concerné qui ne font pas partie des nouvelles tournées à attribuées
                //lstIdTourne des acn tourne du gar kon veu lui desaffecté
                List<TOURNEERELEVEUR> TournePiaAFermer = TourneReleveur.Where(p => !lstIdTourne.Contains(p.FK_IDTOURNEE)).ToList();

                //On ajoute les nouvelles tournées à attribué  mais qui n'était pas liées au releveur qui était peut etre attribué à un autres aux ancienne tournées du releveur qui ne font pas partie des nouvelles tournées à attribuées
                TournePiaAFermer.AddRange(TourneReleveurAutreAfermer);

                List<TOURNEERELEVEUR> TourneeAFermer = new List<TOURNEERELEVEUR>();
                foreach (CsTournee item in ListeDesTourneAAffecter)
                {
                    foreach (TOURNEERELEVEUR st in TournePiaAFermer)
                    {
                        if (item.PK_ID == st.FK_IDTOURNEE)
                        {
                            st.DATECREATION = item.DATECREATION;
                            st.USERCREATION = item.USERCREATION;
                            st.USERMODIFICATION = item.USERMODIFICATION;
                            st.DATEMODIFICATION = item.DATEMODIFICATION;
                            TourneeAFermer.Add(st);
                        }
                    }
                }

                TourneeAFermer.ForEach(t => t.DATEFIN = System.DateTime.Today);

                //toute les ancienne tournées 
                List<int> lstIdTourneAnc = TourneReleveur.Select(t => t.FK_IDTOURNEE).ToList();
                List<TOURNEERELEVEUR> TourneParPIAAjouter = Entities.ConvertObject<TOURNEERELEVEUR, CsTournee>(ListeDesTourneAAffecter.Where(p => !lstIdTourneAnc.Contains(p.FK_IDTOURNEE.Value) && p.FK_IDTOURNEE != 0).ToList());
                TourneParPIAAjouter.ForEach(o => o.FK_IDRELEVEUR = IdReleveur.Value);
                TourneParPIAAjouter.ForEach(o => o.DATEFIN = null);
                TourneParPIAAjouter.ForEach(o => o.USERCREATION = o.USERMODIFICATION);
                TourneParPIAAjouter.ForEach(o => o.DATECREATION = o.DATEMODIFICATION.Value);
                TourneParPIAAjouter.ForEach(o => o.DATEMODIFICATION = null);
                TourneParPIAAjouter.ForEach(o => o.USERMODIFICATION = null);

                int res = -1;
                using (galadbEntities context = new galadbEntities())
                {
                    if (TournePiaAFermer != null && TournePiaAFermer.Count != 0)
                        Entities.UpdateEntity<Galatee.Entity.Model.TOURNEERELEVEUR>(TourneeAFermer, context);
                    Entities.InsertEntity<Galatee.Entity.Model.TOURNEERELEVEUR>(TourneParPIAAjouter, context);
                    res = context.SaveChanges();
                }
                return res == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
*/

        #region Sylla 19/05/2017
        public void CreationLotIsole(List<CsEvenement> lstEvenement)
        {
            try
            {
                galadbEntities pcontext = new galadbEntities();
                FacturationProcedure.CreationLotIsole(lstEvenement, new galadbEntities(), pcontext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}


