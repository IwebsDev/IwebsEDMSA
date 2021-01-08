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
using Galatee.Silverlight.ServiceAccueil   ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmReeditionEtats : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        int IdDemandeDevis = 0;
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;

        public bool IsForAnalyse { get; set; }

        public FrmReeditionEtats()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tabPieceJointe, false);
        }

        string leEtatExecuter = string.Empty;
        public FrmReeditionEtats(string typeEtat)
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tabPieceJointe, false);
            leEtatExecuter = typeEtat;

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
        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
            if (LstPiece.Count() > 0)
            {
                this.isPreuveSelectionnee = true;
                //EnabledDevisInformations(true);
            }
            else
            {
                this.isPreuveSelectionnee = false;
                //EnabledDevisInformations(false);
            }
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (laDemandeSelect != null)
                {
                    if (leEtatExecuter == SessionObject.ReeditionAccuser)
                        EditerAccuserDeReception();
                    if (leEtatExecuter == SessionObject.ReeditionContrat &&
                        laDetailDemande.LstCoutDemande != null &&
                        laDetailDemande.LstCoutDemande.Count != 0)
                    {
                        EditerContrat();
                        /** ZEG 28/09/2017 **/
                        if (laDetailDemande.LaDemande.ISPRESTATION)
                        {
                            EditerContratPrestation(laDetailDemande);
                            EdiderDevisExtension();
                        }
                        EdiderDevis();

                        /****/


                    }
                }
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
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





        private void EdiderDevis()
        {
            try
            {
                List<ObjELEMENTDEVIS> _List = new List<ObjELEMENTDEVIS>();
                List<CsDemandeDetailCout> _List2 = new List<CsDemandeDetailCout>();

                decimal montantTotal = 0;

                if (laDetailDemande.LaDemande.ISPRESTATION)
                {
                    montantTotal = laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).Sum(t => (decimal)(t.MONTANTTTC));
                    _List.AddRange(laDetailDemande.EltDevis.Where(t => !t.ISEXTENSION).ToList());
                }
                else
                {
                    if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count > 0)
                    {
                        montantTotal = laDetailDemande.EltDevis.Sum(t => (decimal)(t.MONTANTTTC));
                        _List.AddRange(laDetailDemande.EltDevis);
                    }
                     else if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count > 0)
                    {
                        montantTotal = laDetailDemande.LstCoutDemande.Sum(t => (decimal)(t.MONTANTHT + t.MONTANTTAXE));
                        _List2.AddRange(laDetailDemande.LstCoutDemande);
                    }

                }

                List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();

                if (_List != null && _List.Count > 0)
                {
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
                }
                else if (_List2 != null && _List2.Count > 0)
                {
                    foreach (CsDemandeDetailCout item in _List2)
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
                        if (item.COPER != SessionObject.Enumere.CoperTRV)
                            LaRubriqueDevis.DESIGNATION = SessionObject.LstDesCopers.First(t => t.CODE == item.COPER).LIBELLE;
                        else
                            LaRubriqueDevis.DESIGNATION = item.LIBELLE;
                        LaRubriqueDevis.QUANTITE = 1;
                        LaRubriqueDevis.PRIXUNITAIRE = item.MONTANTHT.Value;
                        LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTHT + item.MONTANTTAXE);
                        LaRubriqueDevis.TOTALDEVIS = montantTotal;
                        LstDesRubriqueDevis.Add(LaRubriqueDevis);
                    }
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





/** Zeg **/

        private void EditerAccuserDeReception()
        {
            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
            laDetailDemande.LaDemande.NOMCLIENT = string.IsNullOrEmpty(laDetailDemande.LeClient.NOMABON) ? string.Empty : laDetailDemande.LeClient.NOMABON;
            laDetailDemande.LaDemande.LIBELLETYPEDEMANDE = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLETYPEDEMANDE) ? string.Empty : laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
            laDetailDemande.LaDemande.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM; 
            //laDetailDemande.LaDemande.CLIENT = string.IsNullOrEmpty(laDetailDemande.Ag.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Ag.LIBELLEQUARTIER; ;
            //laDetailDemande.LaDemande.ADRESSE1CLIENT = ((CsCentre)this.Cbo_Centre.SelectedItem).TELRENSEIGNEMENT;
            laDetailDemande.LaDemande.LIBELLECENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
            laDetailDemande.LaDemande.LIBELLECOMMUNE = string.IsNullOrEmpty(laDetailDemande.Ag.LIBELLECOMMUNE) ? string.Empty : laDetailDemande.Ag.LIBELLECOMMUNE;
            laDetailDemande.LaDemande.LIBELLEQUARTIER = string.IsNullOrEmpty(laDetailDemande.Ag.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Ag.LIBELLEQUARTIER;
            laDetailDemande.LaDemande.ANNOTATION = string.IsNullOrEmpty(laDetailDemande.LeClient.TELEPHONE) ? string.Empty : laDetailDemande.LeClient.TELEPHONE;
            laDetailDemande.LaDemande.NOMPERE = string.IsNullOrEmpty(laDetailDemande.LeClient.TELEPHONE) ? string.Empty : laDetailDemande.Ag.RUE;
            laDetailDemande.LaDemande.NOMMERE = string.IsNullOrEmpty(laDetailDemande.LeClient.PORTE) ? string.Empty : laDetailDemande.Ag.PORTE;
            laDetailDemande.LaDemande.LIBELLEPRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
            laDetailDemande.LaDemande.LIBELLE = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;

            leDemandeAEditer.Add(laDetailDemande.LaDemande);
            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
        
        }
        private void EditerContrat()
        {
            CsContrat leContrat = new CsContrat();
            leContrat.AGENCE = laDetailDemande.LaDemande.LIBELLECENTRE;
            leContrat.BOITEPOSTALE = laDetailDemande.LeClient.BOITEPOSTAL;
            leContrat.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
            leContrat.CENTRE = laDetailDemande.LaDemande.CENTRE;
            leContrat.CLIENT = laDetailDemande.LaDemande.CLIENT;
            leContrat.CODCONSOMATEUR = laDetailDemande.LeClient.CODECONSO;
            leContrat.CODETARIF = laDetailDemande.Abonne == null ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
            leContrat.COMMUNE = laDetailDemande.Ag.LIBELLECOMMUNE;
            leContrat.LIBELLEPRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
            leContrat.LONGUEURBRANCHEMENT = laDetailDemande.Branchement.LONGBRT.ToString();

            leContrat.NATUREBRANCHEMENT = laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
            leContrat.NOMCLIENT = laDetailDemande.LeClient.NOMABON;
            leContrat.NOMPROPRIETAIRE = laDetailDemande.Ag.NOMP;
            leContrat.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
            leContrat.ORDRE = laDetailDemande.LaDemande.ORDRE;
            leContrat.PORTE = laDetailDemande.Ag.PORTE;
            leContrat.RUE  = laDetailDemande.Ag.RUE ;

            if (laDetailDemande.LaDemande.PUISSANCESOUSCRITE != null)
                leContrat.PUISSANCESOUSCRITE = laDetailDemande.LaDemande.PUISSANCESOUSCRITE.Value.ToString(SessionObject.FormatMontant);
            else if (laDetailDemande.Abonne.PUISSANCE != null)
                leContrat.PUISSANCESOUSCRITE = laDetailDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant);

            leContrat.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
            leContrat.QUARTIERCLIENT = laDetailDemande.Ag.LIBELLEQUARTIER;
            leContrat.REGLAGEDISJONCTEUR = ReglageCompt != null ? ReglageCompt.REGLAGEMINI.Value.ToString() + "/" + ReglageCompt.REGLAGEMAXI.Value.ToString() : string.Empty;
            leContrat.CALIBRE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() : string.Empty;

            leContrat.FRAISPOLICE = ReglageCompt != null ? ReglageCompt.REGLAGE.Value.ToString() + "A" : string.Empty;
            leContrat.NBREFILS = ReglageCompt != null ? ReglageCompt.CODE.Substring(0, 1) : string.Empty;
            leContrat.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
            leContrat.TOTAL1 = laDetailDemande.Ag.TOURNEE + "   " + laDetailDemande.Ag.ORDTOUR;
            leContrat.NUMEROPIECE = laDetailDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDetailDemande.LeClient.NUMEROPIECEIDENTITE;
            leContrat.USAGE = laDetailDemande.LeClient.LIBELLECODECONSO;

            leContrat.MONTANTPARTICAPATION = laDetailDemande.LeClient.REGROUPEMENT;

            leContrat.RUE = laDetailDemande.Ag.LIBELLERUE;
            leContrat.RUECLIENT = laDetailDemande.Ag.LIBELLERUE;
            leContrat.TELEPHONE = laDetailDemande.LeClient.TELEPHONE;

            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count > 0)
            {
                leContrat.MONTANTAVANCE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU) == null ? string.Empty :
                laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            }
            else if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count > 0)
            {
                leContrat.MONTANTAVANCE = laDetailDemande.LstCoutDemande.Where(t => t.COPER == SessionObject.Enumere.CoperCAU) == null ? string.Empty :
                laDetailDemande.LstCoutDemande.Where(t => t.COPER == SessionObject.Enumere.CoperCAU).Sum(t => t.MONTANTHT + t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            }
    


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



            if (lstDEvisTravaux != null && lstDEvisTravaux.Count > 0)
            {
                CsContrat leContratss = new CsContrat();
                leContratss.MONTANTRUBIQE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV).Sum(t => t.MONTANTTTC).Value;
                leContratss.TOTALGENERAL = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperTRV).Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                leContratss.TOTAL2 = (laDetailDemande.EltDevis.FirstOrDefault().TAUXTAXE * 100).Value.ToString(SessionObject.FormatMontant) + "%";

                leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperTRV).LIBELLE;
                leContratss.OPTIONEDITION = "R";
                if (leContratss.MONTANTRUBIQE > 0)
                    lstContrat.Add(leContratss);
            }
            if (laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperFAB) != null)
            {
                CsContrat leContratss = new CsContrat();
                leContratss.MONTANTRUBIQE = laDetailDemande.EltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperFAB).Sum(t => t.MONTANTTTC).Value;

                leContratss.RUBRIQUE = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFAB).LIBELLE;
                leContratss.OPTIONEDITION = "R";
                if (leContratss.MONTANTRUBIQE > 0)
                    lstContrat.Add(leContratss);
            }

            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count > 0)
            {
                foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.CODECOPER != SessionObject.Enumere.CoperTRV && t.CODECOPER != SessionObject.Enumere.CoperFAB))
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = item.MONTANTTTC.Value;

                    leContratss.RUBRIQUE = item.DESIGNATION;
                    leContratss.OPTIONEDITION = "R";
                    lstContrat.Add(leContratss);
                }
            }
            else if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count > 0)
            {
                foreach (CsDemandeDetailCout item in laDetailDemande.LstCoutDemande.Where(t => t.COPER != SessionObject.Enumere.CoperTRV && t.COPER != SessionObject.Enumere.CoperFAB))
                {
                    CsContrat leContratss = new CsContrat();
                    leContratss.MONTANTRUBIQE = item.MONTANTHT.Value + item.MONTANTTAXE.Value;

                    leContratss.RUBRIQUE = item.LIBELLE;
                    leContratss.OPTIONEDITION = "R";
                    lstContrat.Add(leContratss);
                }
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
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_NumeroDemande.Text))
                    RetourneDemandeByNumero(Txt_NumeroDemande.Text);
                else
                {
                    Message.ShowInformation("Saisir le numero de la demande", "Demande");
                    return;
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        CsReglageCompteur ReglageCompt = null;
        private void RetourneDemandeByNumero(string Numerodemande)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;

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
                        this.txtCentre.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLECENTRE) ? string.Empty : laDemandeSelect.LIBELLECENTRE;  
                        this.txtSite.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLESITE) ? string.Empty : laDemandeSelect.LIBELLESITE; 
                     
                        if (laDemandeSelect.ISSUPPRIME==true )
                        {
                            this.OKButton.IsEnabled = false;
                            Message.ShowInformation("Demande déja supprimée", "Demande");
                            return;
                        }
                        else
                        {
                            if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                            {
                                ReglageCompt = new CsReglageCompteur();
                                int idreglageCpt = 0;
                                ReglageCompt = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR);
                                if (ReglageCompt != null)
                                    idreglageCpt = ReglageCompt.PK_ID;
                                ChargerTarifClient(laDetailDemande.LaDemande.FK_IDCENTRE, laDetailDemande.LeClient.FK_IDCATEGORIE.Value, idreglageCpt, null, "0", laDetailDemande.LaDemande.FK_IDPRODUIT.Value);
                            }


                            RemplireOngletClient(laDetailDemande.LeClient);
                            RemplirOngletAbonnement(laDetailDemande.Abonne );
                            RemplireOngletFacture(laDetailDemande.LstCoutDemande);
                            RenseignerInformationsDevis(laDetailDemande);
                        }
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetDevisByNumDemandeAsync(Numerodemande);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement de la demande", "Demande");
            }
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

        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                     CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;

                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {
                    AfficherOuMasquer(tabItemClient, true );

                    this.Txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                    //this.Txt_Telephone1.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                    //this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    //this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    //this.txt_NINA.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                    //this.tab12_Txt_DateModif.Text = string.IsNullOrEmpty(_LeClient.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATEMODIFICATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplirOngletAbonnement(CsAbon  _LeAbon)
        {
            if (_LeAbon != null)
            {
                AfficherOuMasquer(tabItemAbon, true);

                this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? _LeAbon.TYPETARIF : string.Empty;
                this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(_LeAbon.PUISSANCE.Value.ToString()) ? _LeAbon.PUISSANCE.Value.ToString() : string.Empty;

                if (_LeAbon.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbon.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbon.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbon.PUISSANCEUTILISEE.Value).ToString("N2");

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbon.FORFAIT) ? string.Empty : _LeAbon.FORFAIT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbon.LIBELLEFORFAIT) ? string.Empty : _LeAbon.LIBELLEFORFAIT;

                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? string.Empty : _LeAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLETARIF) ? _LeAbon.LIBELLETARIF : string.Empty;

                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbon.PERFAC) ? string.Empty : _LeAbon.PERFAC;
                this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEFREQUENCE) ? _LeAbon.LIBELLEFREQUENCE : string.Empty;

                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbon.MOISREL) ? string.Empty : _LeAbon.MOISREL;
                this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISIND) ? _LeAbon.LIBELLEMOISIND : string.Empty;

                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbon.MOISFAC) ? string.Empty : _LeAbon.MOISFAC;
                this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISFACT) ? _LeAbon.LIBELLEMOISFACT : string.Empty;

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ?string.Empty  : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }

        private void RemplireOngletFacture(List<CsDemandeDetailCout>  _LesFactClient)
        {
            try
            {
                if (_LesFactClient != null && _LesFactClient.Count != 0)
                {
                    AfficherOuMasquer(tabItemCompte, true);

                    _LesFactClient.ForEach(t => t.MONTANTTTC = t.MONTANTHT + t.MONTANTTAXE);
                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = _LesFactClient;
                    this.Txt_TotalHt.Text = _LesFactClient.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe.Text = _LesFactClient.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTTC.Text = _LesFactClient.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
      
    }
}

