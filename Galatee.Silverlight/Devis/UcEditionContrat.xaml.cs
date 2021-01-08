using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcEditionContrat : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
   
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public bool IsForAnalyse { get; set; }

        public UcEditionContrat()
        {
            InitializeComponent();
        }
        public UcEditionContrat( int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);
        }
        CsReglageCompteur ReglageCompt = null;
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = args.Result;
                    laDemandeSelect = laDetailDemande.LaDemande;

                    //if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                    //    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance) 
                    //    && laDetailDemande.LaDemande.PRODUIT ==SessionObject.Enumere.Electricite  )
                    //{
                    //    CsClient leClient = new CsClient();
                    //    leClient.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    //    leClient.CENTRE = laDetailDemande.LaDemande.CENTRE;
                    //    leClient.REFCLIENT = laDetailDemande.LaDemande.CLIENT;
                    //    leClient.ORDRE = laDetailDemande.LaDemande.ORDRE;
                    //    ChargerCompteDeResiliation(leClient);
                    //}
                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        ReglageCompt = new CsReglageCompteur();
                        int idreglageCpt = 0;
                        ReglageCompt =SessionObject.LstReglageCompteur.FirstOrDefault(t=>t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR );
                        if (ReglageCompt != null )
                            idreglageCpt = ReglageCompt.PK_ID ;
                        ChargerTarifClient(laDetailDemande.LaDemande.FK_IDCENTRE, laDetailDemande.LeClient.FK_IDCATEGORIE.Value , idreglageCpt, null, "0", laDetailDemande.LaDemande.FK_IDPRODUIT.Value );
                    }
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsAppareilsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    RenseignerInformationsAbonnement(laDetailDemande);
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
        }
        List<CsTarifClient> lstTarif = new List<CsTarifClient>();
        private void ChargerTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit)
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTarifClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                     
                    }
                    else
                    {
                        lstTarif = args.Result;
                        lstTarif.ForEach(t => t.REDEVANCE = t.REDEVANCE + " " + t.TRANCHE.ToString());
                    }
                };
                client.RetourneTarifClientAsync(idcentre, idcategorie, idreglageCompteur, idtypecomptage, propriotaire, idproduit);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des tarif", "Demande");
            }
        }


        List<CsLclient> lstFactureDuClient = null;
        private void ChargerCompteDeResiliation(CsClient _UnClient)
        {


            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerCompteDeResiliationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                {
                    Message.ShowInformation("Ce client n'existe pas", "RetourneListeFactureNonSolde");
                    return;
                }
                lstFactureDuClient = new List<CsLclient>();
                lstFactureDuClient = args.Result ;
                laDetailDemande.LaDemande.FK_IDCLIENT = lstFactureDuClient.First().FK_IDCLIENT;
                foreach (var item in lstFactureDuClient)
                {
                    if (item.COPER == SessionObject.Enumere.CoperRembAvance )
                        item.SOLDEFACTURE = item.SOLDEFACTURE;
                }
                lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
            };
            client.ChargerCompteDeResiliationAsync(_UnClient);
            client.CloseAsync();

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditerContrat(laDetailDemande);

                /** ZEG 28/09/2017 **/
                if (laDetailDemande.LaDemande.ISPRESTATION)
                    EditerContratPrestation(laDetailDemande);
                EdiderDevis();

                /****/

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
            }
        }

        void ctr_Closed(object sender, EventArgs e)
        {
            Accueil.FrmOptionEditionDevisMt ctrs = sender as Accueil.FrmOptionEditionDevisMt;
            if (ctrs.IsOKclick == true)
            {
                string DateFacture = ctrs.txt_DateFacture.Text;
                string NumeroFacture = ctrs.txt_NumeroFacture .Text;
                string ClientFacture = ctrs.txt_ClientFacture.Text;
                string Objet1 = ctrs.txt_ObjetLigne1.Text;
                string Objet2 = ctrs.txt_ObjetLigne2.Text;
                EdiderDevisMt(DateFacture, NumeroFacture, ClientFacture, Objet1, Objet2, SessionObject.LstRubriqueDevis);
                EdiderDevisAvanceMt(DateFacture, NumeroFacture, ClientFacture, Objet1, Objet2, SessionObject.LstRubriqueDevis);
            }

        }
        private void EditerContrat(CsDemande laDemande)
        {
            try
            {
                CsContrat leContrat = new CsContrat();
                leContrat.AGENCE = laDemande.LaDemande.LIBELLECENTRE;
                leContrat.BOITEPOSTALE = laDemande.LeClient.BOITEPOSTAL;
                leContrat.CATEGORIE = laDemande.LeClient.CATEGORIE;
                leContrat.CENTRE = laDemande.LaDemande.CENTRE;
                leContrat.CLIENT = laDemande.LaDemande.CLIENT;
                leContrat.CODCONSOMATEUR = laDemande.LeClient.CODECONSO;
                leContrat.CODETARIF = laDemande.Abonne == null ? string.Empty : laDemande.Abonne.TYPETARIF;
                leContrat.COMMUNE = laDemande.Ag.LIBELLECOMMUNE;
                leContrat.LIBELLEPRODUIT = laDemande.LaDemande.LIBELLEPRODUIT;
                leContrat.LONGUEURBRANCHEMENT = laDemande.Branchement.LONGBRT.ToString();

                leContrat.NATUREBRANCHEMENT = laDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                leContrat.NOMCLIENT = laDemande.LeClient.NOMABON;
                leContrat.NOMPROPRIETAIRE = laDemande.Ag.NOMP;
                leContrat.NUMDEMANDE = laDemande.LaDemande.NUMDEM;
                leContrat.ORDRE = laDemande.LaDemande.ORDRE;
                leContrat.PORTE = laDemande.Ag.PORTE;
                leContrat.PUISSANCESOUSCRITE = laDemande.LaDemande.PUISSANCESOUSCRITE == null ? laDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant)
                    : laDemande.LaDemande.PUISSANCESOUSCRITE.Value.ToString(SessionObject.FormatMontant);
                leContrat.QUARTIER = laDemande.Ag.LIBELLEQUARTIER;
                leContrat.QUARTIERCLIENT = laDemande.Ag.LIBELLEQUARTIER;
                leContrat.REGLAGEDISJONCTEUR = ReglageCompt != null ? ReglageCompt.REGLAGEMINI.Value.ToString() + "/" + ReglageCompt.REGLAGEMAXI.Value.ToString() : string.Empty;
                leContrat.CALIBRE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() : string.Empty;

                leContrat.FRAISPOLICE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() + "A" : string.Empty;
                leContrat.NBREFILS = ReglageCompt != null ? ReglageCompt.CODE.Substring(0, 1) : string.Empty;
                leContrat.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
                leContrat.TOTAL1 = laDetailDemande.Ag.TOURNEE + "   " + laDetailDemande.Ag.ORDTOUR;
                leContrat.NUMEROPIECE = laDetailDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE;
                leContrat.USAGE = laDetailDemande.LeClient.LIBELLECODECONSO;

                leContrat.MONTANTPARTICAPATION = laDetailDemande.LeClient.REGROUPEMENT;

                /** ZEG 24/09/2017 **/
                /*leContrat.RUE = laDemande.Ag.LIBELLERUE;
                leContrat.RUECLIENT = laDemande.Ag.LIBELLERUE;*/

                leContrat.RUE = laDemande.Ag.RUE;
                leContrat.RUECLIENT = laDemande.Ag.RUE;
                /** **/

                leContrat.TELEPHONE = laDemande.LeClient.TELEPHONE;
                leContrat.MONTANTAVANCE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU) == null ? string.Empty :
                    laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                List<CsContrat> lstContrat = new List<CsContrat>();
                lstContrat.Add(leContrat);
                foreach (ObjAPPAREILSDEVIS item in laDetailDemande.AppareilDevis)
                {
                    CsContrat leContrats = new CsContrat();
                    leContrats.APPAREIL = item.DESIGNATION;
                    leContrats.QUANTITE = item.NBRE.Value;
                    leContrats.OPTIONEDITION = "A";
                    lstContrat.Add(leContrats);
                }
                //List<ObjELEMENTDEVIS> lstDEvisTravaux = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV && !t.ISEXTENSION).ToList();
                List<ObjELEMENTDEVIS> lstDEvisTravaux = new List<ObjELEMENTDEVIS>();

                if (laDetailDemande.LaDemande.ISPRESTATION)
                    lstDEvisTravaux = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV && !t.ISEXTENSION).ToList();
                else
                    lstDEvisTravaux = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV).ToList();

                if (lstDEvisTravaux != null && lstDEvisTravaux.Count != 0)
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = lstDEvisTravaux.Sum(t => t.MONTANTTTC).Value;
                    leContratss.TOTALGENERAL = lstDEvisTravaux.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                    leContratss.TOTAL2 = (lstDEvisTravaux.FirstOrDefault().TAUXTAXE * 100).Value.ToString(SessionObject.FormatMontant) + "%";

                    leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperTRV).LIBELLE;
                    leContratss.OPTIONEDITION = "R";
                    if (leContratss.MONTANTRUBIQE > 0)
                        lstContrat.Add(leContratss);
                }
                List<ObjELEMENTDEVIS> lstParticipation = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperFAB).ToList();
                if (lstParticipation != null)
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = lstParticipation.Sum(t => t.MONTANTTTC).Value;

                    leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFAB).LIBELLE;
                    leContratss.OPTIONEDITION = "R";
                    if (leContratss.MONTANTRUBIQE > 0)
                        lstContrat.Add(leContratss);
                }
                List<ObjELEMENTDEVIS> lstTAutreElement = laDetailDemande.EltDevis.Where(t => t.CODECOPER != SessionObject.Enumere.CoperTRV
                                                                                            && t.CODECOPER != SessionObject.Enumere.CoperFAB).ToList();
                foreach (ObjELEMENTDEVIS item in lstTAutreElement)
                {

                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = item.MONTANTTTC.Value;

                    leContratss.RUBRIQUE = item.DESIGNATION;
                    leContratss.OPTIONEDITION = "R";
                    lstContrat.Add(leContratss);
                }


                if (lstTarif != null && lstTarif.Count != 0)
                    foreach (CsTarifClient item in lstTarif)
                    {
                        CsContrat leContratss = new CsContrat();
                        leContratss.REDEVANCE = item.REDEVANCE;
                        leContratss.TRANCHE = item.PLAGE;
                        leContratss.MONTANT = item.PRIXUNITAIRE;
                        leContratss.OPTIONEDITION = "T";
                        lstContrat.Add(leContratss);
                    }
                if (lstTarif != null && lstTarif.Count != 0)
                    for (int i = 0; i < 14 - (lstTarif.Count); i++)
                    {
                        CsContrat leContratss = new CsContrat();
                        leContratss.CHAMPVIDE = "";
                        leContratss.OPTIONEDITION = "V";
                        lstContrat.Add(leContratss);
                    }
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("pTypedemande", laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                Utility.ActionDirectOrientation<ServicePrintings.CsContrat, CsContrat>(lstContrat, param, SessionObject.CheminImpression, "ContratClient", "Accueil", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /** ZEG 28/09/2017 **/
        private void EditerContratPrestation(CsDemande laDemande)
        {
            try
            {
                CsContrat leContratPrestation = new CsContrat();
                leContratPrestation.AGENCE = laDemande.LaDemande.LIBELLECENTRE;
                leContratPrestation.BOITEPOSTALE = laDemande.LeClient.BOITEPOSTAL;
                leContratPrestation.CATEGORIE = laDemande.LeClient.CATEGORIE;
                leContratPrestation.CENTRE = laDemande.LaDemande.CENTRE;
                leContratPrestation.CLIENT = laDemande.LaDemande.CLIENT;
                leContratPrestation.CODCONSOMATEUR = laDemande.LeClient.CODECONSO;
                leContratPrestation.CODETARIF = laDemande.Abonne == null ? string.Empty : laDemande.Abonne.TYPETARIF;
                leContratPrestation.COMMUNE = laDemande.Ag.LIBELLECOMMUNE;
                leContratPrestation.LIBELLEPRODUIT = laDemande.LaDemande.LIBELLEPRODUIT;
                leContratPrestation.LONGUEURBRANCHEMENT = laDemande.Branchement.LONGBRT.ToString();

                leContratPrestation.NATUREBRANCHEMENT = laDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                leContratPrestation.NOMCLIENT = laDemande.LeClient.NOMABON;
                leContratPrestation.NOMPROPRIETAIRE = laDemande.Ag.NOMP;
                leContratPrestation.NUMDEMANDE = laDemande.LaDemande.NUMDEM;
                leContratPrestation.ORDRE = laDemande.LaDemande.ORDRE;
                leContratPrestation.PORTE = laDemande.Ag.PORTE;
                leContratPrestation.PUISSANCESOUSCRITE = laDemande.LaDemande.PUISSANCESOUSCRITE == null ? laDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant)
                    : laDemande.LaDemande.PUISSANCESOUSCRITE.Value.ToString(SessionObject.FormatMontant);
                leContratPrestation.QUARTIER = laDemande.Ag.LIBELLEQUARTIER;
                leContratPrestation.QUARTIERCLIENT = laDemande.Ag.LIBELLEQUARTIER;
                leContratPrestation.REGLAGEDISJONCTEUR = ReglageCompt != null ? ReglageCompt.REGLAGEMINI.Value.ToString() + "/" + ReglageCompt.REGLAGEMAXI.Value.ToString() : string.Empty;
                leContratPrestation.CALIBRE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() : string.Empty;

                leContratPrestation.FRAISPOLICE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() + "A" : string.Empty;
                leContratPrestation.NBREFILS = ReglageCompt != null ? ReglageCompt.CODE.Substring(0, 1) : string.Empty;
                leContratPrestation.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
                leContratPrestation.TOTAL1 = laDetailDemande.Ag.TOURNEE + "   " + laDetailDemande.Ag.ORDTOUR;
                leContratPrestation.NUMEROPIECE = laDetailDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE;
                leContratPrestation.USAGE = laDetailDemande.LeClient.LIBELLECODECONSO;

                leContratPrestation.MONTANTPARTICAPATION = laDetailDemande.LeClient.REGROUPEMENT;

                leContratPrestation.RUE = laDemande.Ag.RUE;
                leContratPrestation.RUECLIENT = laDemande.Ag.RUE;
                leContratPrestation.TELEPHONE = laDemande.LeClient.TELEPHONE;

                List<CsContrat> lstContratPestation = new List<CsContrat>();

                List<ObjELEMENTDEVIS> lstDEvisTravaux = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV && t.ISEXTENSION).ToList();
                if (lstDEvisTravaux != null && lstDEvisTravaux.Count != 0)
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = lstDEvisTravaux.Sum(t => t.MONTANTTTC).Value;
                    leContratss.TOTALGENERAL = lstDEvisTravaux.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                    leContratss.TOTAL2 = (lstDEvisTravaux.FirstOrDefault().TAUXTAXE * 100).Value.ToString(SessionObject.FormatMontant) + "%";

                    leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == "130").LIBELLE;
                    leContratss.OPTIONEDITION = "R";
                    if (leContratss.MONTANTRUBIQE > 0)
                        lstContratPestation.Add(leContratss);
                }


                if (lstTarif != null && lstTarif.Count != 0)
                    foreach (CsTarifClient item in lstTarif)
                    {
                        CsContrat leContratss = new CsContrat();
                        leContratss.REDEVANCE = item.REDEVANCE;
                        leContratss.TRANCHE = item.PLAGE;
                        leContratss.MONTANT = item.PRIXUNITAIRE;
                        leContratss.OPTIONEDITION = "T";
                        lstContratPestation.Add(leContratss);
                    }
                if (lstTarif != null && lstTarif.Count != 0)
                    for (int i = 0; i < 14 - (lstTarif.Count); i++)
                    {
                        CsContrat leContratss = new CsContrat();
                        leContratss.CHAMPVIDE = "";
                        leContratss.OPTIONEDITION = "V";
                        lstContratPestation.Add(leContratss);
                    }
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("pTypedemande", laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                param.Add("pPrestataire", laDetailDemande.LaDemande.PRESTATAIRE);
                param.Add("AGENCE", laDetailDemande.LaDemande.LIBELLECENTRE);
                param.Add("CATEGORIE", laDetailDemande.LeClient.CATEGORIE);
                param.Add("CLIENT", laDetailDemande.LaDemande.CLIENT);
                param.Add("ORDRE", laDetailDemande.LaDemande.ORDRE);
                param.Add("CODCONSOMATEUR", laDetailDemande.LeClient.CODECONSO);
                param.Add("CODETARIF", laDemande.Abonne == null ? string.Empty : laDemande.Abonne.TYPETARIF);
                param.Add("COMMUNE", laDemande.Ag.LIBELLECOMMUNE);
                param.Add("PRODUIT", laDemande.LaDemande.LIBELLEPRODUIT);
                param.Add("LONGUEUR", laDemande.Branchement.LONGBRT.ToString());
                param.Add("NATURE", laDemande.Branchement.LIBELLETYPEBRANCHEMENT);
                param.Add("NOMCLIENT", laDetailDemande.LeClient.NOMABON);
                param.Add("NOMPROPRIETAIRE", laDetailDemande.Ag.NOMP);
                param.Add("NUMDEMANDE", laDetailDemande.LaDemande.NUMDEM);
                param.Add("PORTE", laDemande.Ag.PORTE);
                param.Add("PUISSANCE", laDemande.LaDemande.PUISSANCESOUSCRITE == null ? laDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant)
                    : laDemande.LaDemande.PUISSANCESOUSCRITE.Value.ToString(SessionObject.FormatMontant));
                param.Add("QUARTIER", laDemande.Ag.LIBELLEQUARTIER);
                param.Add("REGLAGE", ReglageCompt != null ? ReglageCompt.REGLAGEMINI.Value.ToString() + "/" + ReglageCompt.REGLAGEMAXI.Value.ToString() : string.Empty);
                param.Add("CALIBRE", ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() : string.Empty);
                param.Add("AMPERAGE", ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() + "A" : string.Empty);
                param.Add("FILS", ReglageCompt != null ? ReglageCompt.CODE.Substring(0, 1) : string.Empty);
                param.Add("DATE", System.DateTime.Today.ToShortDateString());
                param.Add("TOURNEE", laDetailDemande.Ag.TOURNEE + "   " + laDetailDemande.Ag.ORDTOUR);
                param.Add("PIECE", laDetailDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE);
                param.Add("USAGE", laDetailDemande.LeClient.LIBELLECODECONSO);
                param.Add("REGROUPEMENT", laDetailDemande.LeClient.REGROUPEMENT);
                param.Add("RUE", laDetailDemande.Ag.RUE);
                param.Add("TELEPHONE", laDetailDemande.LeClient.TELEPHONE);



                Utility.ActionDirectOrientation<ServicePrintings.CsContrat, CsContrat>(lstContratPestation, param, SessionObject.CheminImpression, "PrestataireClient", "Accueil", true);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }



        private void EdiderDevisMt(string DateFacture, string NumeroFacture, string ClientFacture, string Objet1, string Objet2, List<CsRubriqueDevis> leRubriques)
        {
            decimal montantTotal = laDetailDemande.EltDevis.Where(u=>u.MONTANTTTC != null).Sum(t => (decimal)(t.MONTANTTTC));
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t=>t.CODECOPER != SessionObject.Enumere.CoperCAU ).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                
                LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                LaRubriqueDevis.PRIXTVA = (montantTotal * 18)/100;
                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                if (item.FK_IDRUBRIQUEDEVIS != null)
                    LaRubriqueDevis.SECTION = leRubriques.FirstOrDefault(t => t.PK_ID == item.FK_IDRUBRIQUEDEVIS).LIBELLE;
                else
                    LaRubriqueDevis.SECTION = "";

                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pDate", DateFacture );
            param.Add("pNumeroFacture", NumeroFacture );
            param.Add("pClient", ClientFacture );
            param.Add("pObjet1", Objet1);
            param.Add("pObjet2", Objet2);
            //Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, param, SessionObject.CheminImpression, "DevisMt", "Accueil", true);
            Utility.ActionExportation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, param, string.Empty, SessionObject.CheminImpression, "DevisMt", "Accueil", true, "doc");
        }



        private void EdiderDevisAvanceMt(string DateFacture, string NumeroFacture, string ClientFacture, string Objet1, string Objet2, List<CsRubriqueDevis> leRubriques)
        {
            decimal montantTotal = laDetailDemande.EltDevis.Sum(t => (decimal)(t.MONTANTTTC));
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DESIGNATION = item.DESIGNATION;

                LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                //LaRubriqueDevis.PRIXTVA = (montantTotal * 18) / 100;
                LaRubriqueDevis.TOTALDEVIS = montantTotal;

                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pDate", DateFacture);
            param.Add("pNumeroFacture", NumeroFacture);
            param.Add("pClient", ClientFacture);
            param.Add("pObjet1", Objet1);
            param.Add("pObjet2", Objet2);
            Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, param, SessionObject.CheminImpression, "DevisAvanceMt", "Accueil", true);
        }



        private void EdiderDevisApDp()
        {
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            List<string> lstCoperDevis = new List<string>();
            lstCoperDevis.Add(SessionObject.Enumere.CoperCAU);
            lstCoperDevis.Add(SessionObject.Enumere.CoperTRV);
            lstCoperDevis.Add(SessionObject.Enumere.CoperFPO);
            lstCoperDevis.Add(SessionObject.Enumere.CoperFDO);
            decimal montantTotal = lstFactureDuClient.First().SOLDECLIENT.Value;

            foreach (var item in lstFactureDuClient.Where(t => lstCoperDevis.Contains(t.COPER)).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DESIGNATION = item.LIBELLECOPER;
                LaRubriqueDevis.PRIXUNITAIRE = item.SOLDEFACTURE.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.SOLDEFACTURE);
                LaRubriqueDevis.SECTION = "D";
                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }
            List<string> lstCoperFacture = new List<string>();
            lstCoperFacture.Add(SessionObject.Enumere.CoperFact);

            foreach (var item in lstFactureDuClient.Where(t => lstCoperFacture.Contains(t.COPER)).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DESIGNATION = item.LIBELLECOPER;
                LaRubriqueDevis.PRIXUNITAIRE = item.SOLDEFACTURE.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.SOLDEFACTURE);
                LaRubriqueDevis.SECTION = "F";
                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }

            List<string> lstCoperRem = new List<string>();
            lstCoperRem.Add(SessionObject.Enumere.CoperRAC);
            foreach (var item in lstFactureDuClient.Where(t => lstCoperRem.Contains(t.COPER)).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DESIGNATION = item.LIBELLECOPER;
                LaRubriqueDevis.PRIXUNITAIRE = item.MONTANT.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANT) * (-1);
                LaRubriqueDevis.SECTION = "R";
                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }
            Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "DevisApDp", "Accueil", true);
        }
        private void EdiderDevis()
        {
            try
            {
                List<ObjELEMENTDEVIS> _List = new List<ObjELEMENTDEVIS>();

                decimal montantTotal = 0;

                if (laDetailDemande.LaDemande.ISPRESTATION)
                {
                    montantTotal = laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).Sum(t => (decimal)(t.MONTANTTTC));
                    _List.AddRange(laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).ToList());
                }
                else
                {
                    montantTotal = laDetailDemande.EltDevis.Sum(t => (decimal)(t.MONTANTTTC));
                    _List.AddRange(laDetailDemande.EltDevis);
                }

                List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
                foreach (ObjELEMENTDEVIS item in _List)
                {
                    CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                    LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                    LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                    LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                    LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                    LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                    LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                    LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                    LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                    if (item.CODECOPER != SessionObject.Enumere.CoperTRV)
                        LaRubriqueDevis.DESIGNATION = SessionObject.LstDesCopers.First(t => t.CODE == item.CODECOPER).LIBELLE;
                    else
                        LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                    LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                    LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                    LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                    LaRubriqueDevis.TOTALDEVIS = montantTotal;
                    LstDesRubriqueDevis.Add(LaRubriqueDevis);
                }
                //Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "Devis", "Accueil", true);
                Utility.ActionExportation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, string.Empty, SessionObject.CheminImpression, "Devis", "Accueil", true, "doc");

                //decimal montantTotalExtension = laDetailDemande.EltDevis.Where(y => y.ISEXTENSION).Sum(t => (decimal)(t.MONTANTTTC));
                //List<CsEditionDevis> LstDesRubriqueDevisExtension = new List<CsEditionDevis>();
                //foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.ISEXTENSION).ToList())
                //{
                //    CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                //    LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                //    LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                //    LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                //    LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                //    LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                //    LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                //    LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                //    LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                //    LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                //    LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                //    LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                //    LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                //    LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                //    LaRubriqueDevis.TOTALDEVIS = montantTotalExtension;
                //    LstDesRubriqueDevisExtension.Add(LaRubriqueDevis);
                //}
                //if (LstDesRubriqueDevisExtension.Count != 0)
                //    //Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevisExtension, null, SessionObject.CheminImpression, "DevisExtension", "Accueil", true);
                //    Utility.ActionExportation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevisExtension, null, string.Empty, SessionObject.CheminImpression, "DevisExtension", "Accueil", true, "doc");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void EdiderDevisExtension()
        {
            try
            {
                //decimal montantTotal = laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).Sum(t => (decimal)(t.MONTANTTTC));
                //List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
                //foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).ToList())
                //{
                //    CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                //    LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                //    LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                //    LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                //    LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                //    LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                //    LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                //    LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                //    LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                //    LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                //    if (item.CODECOPER != SessionObject.Enumere.CoperTRV)
                //        LaRubriqueDevis.DESIGNATION = SessionObject.LstDesCopers.First(t => t.CODE == item.CODECOPER).LIBELLE;
                //    else
                //        LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                //    LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                //    LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                //    LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                //    LaRubriqueDevis.TOTALDEVIS = montantTotal;
                //    LstDesRubriqueDevis.Add(LaRubriqueDevis);
                //}
                ////Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "Devis", "Accueil", true);
                //Utility.ActionExportation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, string.Empty, SessionObject.CheminImpression, "Devis", "Accueil", true, "doc");

                decimal montantTotalExtension = laDetailDemande.EltDevis.Where(y => y.ISEXTENSION).Sum(t => (decimal)(t.MONTANTTTC));
                List<CsEditionDevis> LstDesRubriqueDevisExtension = new List<CsEditionDevis>();
                foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.ISEXTENSION).ToList())
                {
                    CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                    LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                    LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                    LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                    LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                    LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                    LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                    LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                    LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                    LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                    LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                    LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                    LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                    LaRubriqueDevis.TOTALDEVIS = montantTotalExtension;
                    LstDesRubriqueDevisExtension.Add(LaRubriqueDevis);
                }
                if (LstDesRubriqueDevisExtension.Count != 0)
                    //Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevisExtension, null, SessionObject.CheminImpression, "DevisExtension", "Accueil", true);
                    Utility.ActionExportation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevisExtension, null, string.Empty, SessionObject.CheminImpression, "DevisExtension", "Accueil", true, "doc");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

        }
  

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);
                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT  : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;

                    /** ZEG 24/09/2017 **/
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                    /** **/
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = laDemande.LeClient != null && !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
                    txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.LeClient.ADRMAND1) ? laDemande.LeClient.ADRMAND1 : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;

                    /** ZEG 24/09/2017 **/

                    //Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    //Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibellePorte.Text = !string.IsNullOrEmpty(laDemande.Ag.PORTE) ? laDemande.Ag.PORTE : string.Empty;

                    /** **/

                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;

                    if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                    {
                       
                    }
                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsAppareilsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count > 0)
                {
                    dtgAppareils.ItemsSource = laDemande.AppareilDevis;
                    AfficherOuMasquer(tabItemAppareils, true);
                }
                else
                    AfficherOuMasquer(tabItemAppareils, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsFournitureDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    if (lademande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RemplirListeMateriel(lademande.EltDevis);
                    else
                    {
                        RemplirListeMaterielMT(lademande.EltDevis, SessionObject.LstRubriqueDevis);

                        //if (lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                        //    RemplirListeMaterielMT(lademande.EltDevis,SessionObject.LstRubriqueDevis );
                        //else
                        //    RemplirListeMateriel(lademande.EltDevis);
                    }
                    AfficherOuMasquer(tabItemFournitures, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des fournitures", "Validation devis");
            }
        }
        private void RenseignerInformationsAbonnement(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Abonne != null && laDemande.Abonne != null)
                {
                    this.Txt_CodePuissanceUtilise.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE.ToString();
                    this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;
                    this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.PUISSANCE.Value.ToString()) ? laDetailDemande.Abonne.PUISSANCE.Value.ToString() : string.Empty;

                    if (laDetailDemande.Abonne.PUISSANCE != null)
                        this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");
                    if (laDetailDemande.Abonne.PUISSANCEUTILISEE != null)
                        this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCEUTILISEE.Value).ToString("N2");
                    this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(laDetailDemande.Abonne.RISTOURNE.Value).ToString("N2");

                    this.Txt_CodeForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FORFAIT) ? string.Empty : laDetailDemande.Abonne.FORFAIT;
                    this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFORFAIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEFORFAIT;

                    this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                    this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                    this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.PERFAC) ? string.Empty : laDetailDemande.Abonne.PERFAC;
                    this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFREQUENCE) ? laDetailDemande.Abonne.LIBELLEFREQUENCE : string.Empty;

                    this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISREL) ? string.Empty : laDetailDemande.Abonne.MOISREL;
                    this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISIND) ? laDetailDemande.Abonne.LIBELLEMOISIND : string.Empty;

                    this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISFAC) ? string.Empty : laDetailDemande.Abonne.MOISFAC;
                    this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISFACT) ? laDetailDemande.Abonne.LIBELLEMOISFACT : string.Empty;

                    this.Txt_DateAbonnement.Text = (laDetailDemande.Abonne.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(laDetailDemande.Abonne.DABONNEMENT.Value).ToShortDateString();
                    AfficherOuMasquer(tabItemAbonnement, true);
                }
                else
                    AfficherOuMasquer(tabItemAbonnement, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeMateriel(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();
                lstFourExtension.ForEach(t => t.IsCOLORIE = false);
                lstFourBranchement.ForEach(t => t.IsCOLORIE = false);

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";


                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourExtension);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }
                if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale;
            if (dataGridForniture.ItemsSource != null)
            {
                Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
        }
 
        Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS> TotalRubrique = new Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS>();
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            try
            {
                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.LIBELLE = "----------------------------------";
                leSeparateur.ISDEFAULT = true;
                List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();

                foreach (CsRubriqueDevis item in leRubriques)
                {
                    bool MiseAZereLigne = false;
                    List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                    if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                    {
                        int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                        lstFourRubrique.ForEach(t => t.FK_IDCOPER = CoperTrv);
                        if (item.PK_ID == 1 && laDetailDemande.Branchement.CODEBRT == "0001")
                        {
                            decimal? MontantLigne = 0;

                            ObjELEMENTDEVIS leIncidence = lstEltDevis.FirstOrDefault(t => t.ISGENERE == true);
                            leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                            leIncidence.QUANTITE = 1;
                            leIncidence.FK_IDCOPER = CoperTrv;
                            leIncidence.MONTANTTAXE = 0;
                            leIncidence.MONTANTHT = 0;
                            leIncidence.MONTANTTTC = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE) * (-1);
                            if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                                lstFourRubrique.Add(leIncidence);
                            MontantLigne = lstFourRubrique.Sum(t => t.MONTANTTTC);
                            if (MontantLigne < 0)
                                MiseAZereLigne = true;

                        }
                        decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);
                        decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                        decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                        if (MiseAZereLigne == true)
                        { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }
                        ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                        leResultatBranchanchement.DESIGNATION = "Sous Total " + item.LIBELLE;
                        leResultatBranchanchement.IsCOLORIE = true;
                        leResultatBranchanchement.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leResultatBranchanchement.ISDEFAULT = true;
                        leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                        leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                        leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;

                        lstFourgenerale.AddRange(lstFourRubrique);
                        lstFourgenerale.Add(leSeparateur);
                        lstFourgenerale.Add(leResultatBranchanchement);
                        lstFourgenerale.Add(new ObjELEMENTDEVIS()
                        {
                            LIBELLE = "    "
                        });
                    }

                }
                if (lstFourgenerale.Count != 0)
                {
                    decimal? MontantTotRubrique = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC);
                    decimal? MontantTotRubriqueHt = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE);
                    if (MontantTotRubrique < 0)
                    { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }


                    ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                    leSurveillance.DESIGNATION = "Etude et surveillance ";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);
                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                    lstFourgenerale.Add(leSurveillance);


                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL FACTURE TRAVAUX ";
                    //leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = MontantTotRubrique;
                    leResultatGeneral.MONTANTTAXE = MontantTotRubriqueHt;
                    leResultatGeneral.MONTANTTTC = MontantTotRubriqueTaxe;
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
                ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
                leResultatGeneralaVANCE.DESIGNATION = "FACTURE AVANCE SUR CONSOMMATION ";
                //leResultatGeneralaVANCE.IsCOLORIE = true;
                leResultatGeneralaVANCE.ISDEFAULT = true;
                leResultatGeneralaVANCE.QUANTITE = lstEltDevis.FirstOrDefault(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).QUANTITE;
                leResultatGeneralaVANCE.MONTANTHT = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTHT);
                leResultatGeneralaVANCE.MONTANTTAXE = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTAXE);
                leResultatGeneralaVANCE.MONTANTTTC = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTTC);
                leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU).PK_ID;
                leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneralaVANCE);
                this.dataGridForniture.ItemsSource = null;
                this.dataGridForniture.ItemsSource = lstFourgenerale;

                this.Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement des couts", "");
            }
        }

        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }
        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();

                this.btn_transmetre.IsEnabled = false;
                this.CancelButton.IsEnabled = false;
                //OKButton_Click(null, null);

                EditerContrat(laDetailDemande);

                laDetailDemande.LaDemande.FK_IDETAPEENCOURE = laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE;
                lstDemande.Add(laDetailDemande.LaDemande);

                /** ZEG 28/09/2017 **/
                if (laDetailDemande.LaDemande.ISPRESTATION)
                {
                    EditerContratPrestation(laDetailDemande);
                    //EdiderDevisExtension();
                }
                //EdiderDevis();

                /****/

                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(lstDemande, true, this);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Demande");
            }
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            Thread.Sleep(5000);
            this.btn_transmetre.IsEnabled = true;
            this.CancelButton.IsEnabled = true;
        }

        private void LettreFactureSuiteApDp()
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.LettrageSuiteAPDPCompleted += (s, args) =>
            {
                this.btn_transmetre.IsEnabled = true;
                this.CancelButton.IsEnabled = true;
                if (args != null && args.Cancelled)
                    return;
                List<string> codes = new List<string>();
                codes.Add(laDetailDemande.InfoDemande.CODE);
                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);
                this.DialogResult = true;
            };
            client.LettrageSuiteAPDPAsync(laDetailDemande);
            client.CloseAsync();

        }
    }
}

