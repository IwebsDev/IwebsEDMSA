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
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Caisse;
using Galatee.Silverlight;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmModificationMoratoire : ChildWindow
    {
        public FrmModificationMoratoire()
        {
            InitializeComponent();
            ChargerListDesSite();
            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
            Initialisation();
        }
        string MoisComptable = string.Empty;
        int frequenceMoratoire;
        int selectedoccurMoratoire = 0;
        long montantEcheance = 0;

        string amountselected = string.Empty;
        string nombreEchaeance = string.Empty;

        List<ServiceRecouvrement.CsDetailMoratoire> listeDetailsMoratoires = new List<ServiceRecouvrement.CsDetailMoratoire>();
        int refClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        int TailleReferenceClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        decimal soldeFactureSelect = 0;

        List<string> listeAmountMoratoire = new List<string>();
        List<string> listeDateMoratoire = new List<string>();

        CsClient _UnClient;
        List<CsLclient> listFacture = new List<CsLclient>();
        List<CsLclient> lstFactureDeMoratoire = new List<CsLclient>();

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<int?> lesIdCentre = new List<int?>();
        void ChargerListDesSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lesIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lesIdCentre.Add(item.PK_ID);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void txtReference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtReference.Text.Length == refClient)
                {
                    AfficherImpayes(txtReference.Text);

                }
                else
                {
                    if (string.IsNullOrEmpty(txtReference.Text))
                        CleanIHM();
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        string centre = string.Empty;
        string client = string.Empty;
        string ordre = string.Empty;
        private void AfficherImpayes(string refClient)
        {
            try
            {
                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;

                centre = refClient.Substring(0, SessionObject.Enumere.TailleCentre);
                client = refClient.Substring(debutClient, SessionObject.Enumere.TailleClient);
                ordre = refClient.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);

                RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.Protocole(), Utility.EndPoint("Recouvrement"));
                srv.TestClientExistCompleted += new EventHandler<TestClientExistCompletedEventArgs>(TestClientExist);// Call back sur la methode TestClientTxi
                srv.TestClientExistAsync(centre, client, ordre);
            }
            catch (Exception ex)
            {
                CleanIHM();
                throw ex;
            }
        }
        void TestClientExist(object sender, TestClientExistCompletedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (e.Cancelled || e.Error != null)
                    return;
                if (e.Result == null || e.Result.Count == 0)
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    Message.ShowError("La référence client ne correspond à aucun abonné ", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;

                }
                else
                {

                    txtClientName.Text = e.Result[0].NOMABON;
                    txtClientAdresse.Text = e.Result[0].ADRMAND1;
                    _UnClient = new CsClient();
                    _UnClient = e.Result[0];
                    RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.Protocole(), Utility.EndPoint("Recouvrement"));
                    srv.RetourneMoratoireDuClientCompleted += (es, res) =>
                    {
                        try
                        {
                            if (res.Cancelled || res.Error != null)
                            {
                                Message.ShowError("Erreur survenue lors de l'appel serveur.", "Erreur");
                                CleanIHM();
                                return;
                            }

                            if (res.Result == null || res.Result.Count == 0)
                            {
                                Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
                                CleanIHM();
                                return;
                            }

                            listeDetailsMoratoires = res.Result;
                            if (listeDetailsMoratoires != null)
                            {
                                listeDetailsMoratoires.ForEach(t => t.FK_IDCLIENT = _UnClient.PK_ID);
                                FillDetailData(listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).ToList());
                            }
                            
                            btnOk.IsEnabled = true;
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        }
                        catch (Exception ex)
                        {
                            CleanIHM();
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                        }
                    };
                    srv.RetourneMoratoireDuClientAsync(centre, client, ordre);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void FillDetailData(List<ServiceRecouvrement.CsDetailMoratoire> l)
        {

            try
            {

                for (int j = 0; j <= 10; j++)
                {
                    j++;
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + j);
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + j);
                    amounts.Text = string.Empty;
                    dateBox.Text = string.Empty;
                }

                int i = 0;
                List<ServiceRecouvrement.CsDetailMoratoire> listeIsCret = new List<ServiceRecouvrement.CsDetailMoratoire>();
                foreach (ServiceRecouvrement.CsDetailMoratoire moratoires in l)
                {

                    // fill les données des échéances 
                    i++;

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);

                    if (!string.IsNullOrEmpty(moratoires.CRET))
                    {
                        listeIsCret.Add(moratoires);
                        amounts.Text = moratoires.MONTANT == null ? string.Empty : moratoires.MONTANT.Value.ToString(SessionObject.FormatMontant);
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                    else
                    {
                        // listeIsCret.Add(moratoires);
                        amounts.Text = moratoires.MONTANT == null ? string.Empty : moratoires.MONTANT.Value.ToString(SessionObject.FormatMontant);
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                }

                Decimal fraisFacture = listeIsCret.Sum(p => p.FRAISDERETARD == null ? 0 : p.FRAISDERETARD.Value);
                Decimal montantFacture = listeIsCret.Sum(p => p.MONTANT == null ? 0 : p.MONTANT.Value);
                amountselected = txtSoldeDue.Text = (montantFacture - fraisFacture).ToString(SessionObject.FormatMontant);
                nombreEchaeance = listeIsCret.Count.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                _UnClient = (CsClient)ctrs.MyObject;
                List<CsLclient> lstFactureSelect = listFacture.Where(t => t.FK_IDCLIENT == _UnClient.PK_ID).ToList();
                listFacture = lstFactureSelect;

                if (listFacture.First().IsPAIEMENTANTICIPE)
                {
                    Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
                    CleanIHM();
                    return;
                }
                listFacture.ForEach(t => t.REFERENCE = t.NDOC + "           " + t.REFEM + "             " + t.SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant));
                soldeFactureSelect = lstFactureSelect.First().SOLDECLIENT.Value;
                SessionObject.moisComptable = lstFactureSelect.FirstOrDefault().MOISCOMPT;
                txtClientName.Text = listFacture.FirstOrDefault().NOM;
                lstFactureDeMoratoire = lstFactureSelect;
            }
        }

        private void Cbo_ListeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        /// <summary>
        /// Permet l'initialisation des controles de IHM
        /// 
        /// </summary>
        void Initialisation()
        {
            /// Peupler la combobox des nombre d'installation

            try
            {
                List<string> listeValue = new List<string>();
                for (int i = 2; i < 13; i++)
                    listeValue.Add(i.ToString());
                cboNoInstall.ItemsSource = listeValue;
                cboNoInstall.SelectedItem = listeValue[0];

                rdbMonth.IsChecked = true;
                //btnCalcul.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Inititalise tous les controles de l'IHM pour une nouvelle
        /// saisie de données
        /// </summary>
        /// 

        void CleanIHM()
        {
            try
            {
                for (int i = 1; i < 13; i++)
                {

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox date = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    amounts.Text = string.Empty;
                    date.Text = string.Empty;

                }
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                txtClientName.Text = string.Empty;
                txtReference.Text = string.Empty;
                txtClientAdresse.Text = string.Empty;
                //txtDue.Text = string.Empty;

                if (listFacture != null && listFacture.Count > 0)
                    listFacture.Clear();
                if (listeDateMoratoire != null && listeDateMoratoire.Count > 0)
                    listeDateMoratoire.Clear();
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                    listeAmountMoratoire.Clear();



                rdbMonth.IsChecked = true;
                frequenceMoratoire = 1;

                txtSoldeDue.Text = string.Empty;


                if (listFacture != null && listFacture.Count > 0)
                    listFacture.Clear();

                if (lstFactureDeMoratoire != null && lstFactureDeMoratoire.Count > 0)
                    lstFactureDeMoratoire.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void ClearAmountTextBox()
        {
            try
            {
                for (int i = 1; i < 13; i++)
                {

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox date = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    amounts.Text = string.Empty;
                    date.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCalcul_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int   NombreEchaeance = int.Parse(cboNoInstall.SelectedItem.ToString());
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = false;
                CalculerMoratoire(NombreEchaeance, soldeFactureSelect, 0, false);
                this.btnOk.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        /// <summary>
        /// Permet de peupler la liste des montants des occurrences du moratoires d'une facture donnée
        /// </summary>
        /// <param name="frequence"></param>
        void InitialiseMontantMoratoires(int NombreEchaeance, decimal montantSolde)
        {
            try
            {
                listeAmountMoratoire.Clear();

                double montantFacture = Convert.ToDouble(montantSolde);
                double reste = Math.Round(montantFacture % NombreEchaeance, 2);
                double partieEntiere = montantFacture - reste;
                double diviseur = partieEntiere / NombreEchaeance;
                for (int i = 0; i < NombreEchaeance; i++)
                {
                    if (i == NombreEchaeance - 1)
                    {
                        double montant = reste + diviseur;
                        montantEcheance = Convert.ToInt64(diviseur);
                        listeAmountMoratoire.Add(montant.ToString(SessionObject.FormatMontant));
                    }
                    else
                        listeAmountMoratoire.Add(diviseur.ToString(SessionObject.FormatMontant));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void InitialiseMontantMoratoiresTouteFactures(int frequence, decimal soldeFacture)
        {
            try
            {
                listeAmountMoratoire.Clear();

                double montantdesFacture = Convert.ToDouble(soldeFacture);
                double reste = Math.Round(montantdesFacture % frequence, 2);
                double partieEntiere = montantdesFacture - reste;
                double diviseur = partieEntiere / frequence;
                for (int i = 0; i < frequence; i++)
                {
                    if (i == int.Parse(cboNoInstall.SelectedItem.ToString()) - 1)
                    {
                        double montant = reste + diviseur;
                        listeAmountMoratoire.Add(montant.ToString(SessionObject.FormatMontant));
                    }
                    else
                        listeAmountMoratoire.Add(diviseur.ToString(SessionObject.FormatMontant));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Permet de peupler la liste des dates des occurrences du moratoires
        /// </summary>
        /// <param name="frequenceMoratoire"></param>
        /// <param name="NombreOccurMor"></param>
        void InitialiserDateMoratoiresParFacture(int frequenceMoratoire, int NombreOccurMor, DateTime DateDebut)
        {
            try
            {
                DateTime now = new DateTime();
                if (DateDebut == System.DateTime.Today)
                    now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                else
                    now = new DateTime(DateDebut.Year, DateDebut.Month, DateDebut.Day);

                listeDateMoratoire.Clear();

                for (int i = 0; i < NombreOccurMor; i++)
                {
                    if (i == 0)
                        listeDateMoratoire.Add(now.ToString("d"));
                    else
                    {
                        string datte = string.Empty;

                        if (frequenceMoratoire == 1)
                            now = now.AddMonths(1);
                        else
                            if (frequenceMoratoire == 2)
                                now = now.AddDays(14);
                            else
                                now = now.AddDays(7);

                        datte = now.ToString("d");
                        listeDateMoratoire.Add(datte);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void rdbMoratoireFacture_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMoratoireEntier_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMoratoireFacture_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMonth_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Month;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erruer");
            }
        }

        private void rdbFornight_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.ForNight;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbWeek_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Week;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReference.Text))
                    return;
                if (IsProposition)
                {
                    decimal montantTotal = 0;
                    if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                    {
                        int i = 0;
                        foreach (string amount in listeAmountMoratoire)
                        {
                            i++;
                            TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                            if (!string.IsNullOrEmpty(amounts.Text))
                                montantTotal = montantTotal + Convert.ToDecimal(amounts.Text);
                        }
                    }
                    if (montantTotal > Convert.ToDecimal(soldeFactureSelect))
                    {
                        Message.ShowInformation("Montant des moratoires supperieur au montant de facture selectionnée", "Moratoire");
                        return;
                    }
                }

                btnOk.IsEnabled = false;
                List<CsDetailMoratoire> listeMORA = new List<CsDetailMoratoire>();
                listeMORA.AddRange(CreateListeFromDetailMoratoire(listeDetailsMoratoires));
                this.prgBar.Visibility = System.Windows.Visibility.Visible;

                RecouvrementServiceClient cl = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                cl.UpdateDetailMoratoireCompleted += (s, results) =>
                {
                    try
                    {
                        this.prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        if (results.Cancelled || results.Result == false)
                        {
                            Message.Show(results.Error.Message, Galatee.Silverlight.Resources.Langue.wcf_error);
                            return;
                        }
                        btnOk.IsEnabled = true;
                        string reffact = string.Empty;
                        int i = 0;
                        foreach (var item in lstFactureDeMoratoire)
                        {
                            if (i == 6) break;
                            reffact = reffact + item.REFEM + "-" + item.NDOC + "    ";
                            i++;
                        }
                        ////impression du recu du moratoire
                        //Dictionary<string, string> param = new Dictionary<string, string>();
                        //param.Add("pCentre", lstFactureDeMoratoire.First().CENTRE);
                        //param.Add("pClient", lstFactureDeMoratoire.First().CLIENT);
                        //param.Add("pOrdre", lstFactureDeMoratoire.First().ORDRE);
                        //param.Add("pNombreEcheance", (listeMORA.Count - 1).ToString());
                        //param.Add("pMontantGlobal", soldeFactureSelect.ToString(SessionObject.FormatMontant));
                        //param.Add("pName", txtClientName.Text);
                        //param.Add("pNombreFacture", lstFactureDeMoratoire.Count().ToString());
                        //param.Add("pReferencefacture", reffact);
                        //param.Add("pOperationMor", "MORATOIRE");

                       
                        //param.Add("pExigibilite", listeMORA.First(t => t.EXIGIBILITE != null).DC.ToString());
                        ////Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(listeMORA.Where(t => t.EXIGIBILITE != null).ToList(), param, SessionObject.CheminImpression, "moratoire", "Recouvrement", true);
                        //CleanIHM();
                    }
                    catch (Exception ex)
                    {
                        this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        Message.ShowError(ex, "Erreur");
                    }
                };
                cl.UpdateDetailMoratoireAsync(listeMORA);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }


        List<CsDetailMoratoire> CreateListeFromDetailMoratoire(List<CsDetailMoratoire > lesfactureMoratoire)
        {
            try
            {
                string refem = MoisComptable;
                if (lesfactureMoratoire.Count == 1)
                    refem = lesfactureMoratoire.First().REFEM;

                List<CsDetailMoratoire> moratoires = new List<CsDetailMoratoire>();
                selectedoccurMoratoire = int.Parse(cboNoInstall.SelectedItem.ToString());
                for (int i = 0; i < selectedoccurMoratoire + 1; i++)
                {
                    int inc = i + 1;
                    TextBox amounts = null;
                    TextBox dates = null;
                    if (i != 0)
                    {
                        amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                        dates = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    }
                    if (i == 0) continue;

                    CsDetailMoratoire echeanceMoratoire = new CsDetailMoratoire()
                    {
                        COPER = SessionObject.Enumere.CoperMOR,
                        REFEM = MoisComptable,
                        CENTRE = lesfactureMoratoire.First().CENTRE,
                        CLIENT = lesfactureMoratoire.First().CLIENT,
                        ORDRE = lesfactureMoratoire.First().ORDRE,
                        CRET = SessionObject.Cret()[i - 1],
                        NATURE = SessionObject.Enumere.NatureMor,
                        DATEVALEUR = null,
                        DC = SessionObject.Enumere.Debit,
                        MONTANT = Convert.ToDecimal(amounts.Text),
                        EXIGIBILITE = DateTime.Parse(dates.Text),
                        TOP1 = SessionObject.Enumere.TopMoratoire,
                        MATRICULE = UserConnecte.matricule,
                        DATECREATION = DateTime.Now.Date,
                        USERCREATION = UserConnecte.matricule,
                        FK_IDCLIENT = lesfactureMoratoire.First().FK_IDCLIENT,
                        FK_IDCENTRE = lesfactureMoratoire.First().FK_IDCENTRE,
                        FK_IDADMUTILISATEUR = UserConnecte.PK_ID,
                        FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperMOR).PK_ID,
                        FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID,
                        IDMORATOIRE = lesfactureMoratoire.First().IDMORATOIRE,
                        FK_IDMORATOIRE = lesfactureMoratoire.First().IDMORATOIRE  
                    };
                    CsDetailMoratoire leAncMor = lesfactureMoratoire.FirstOrDefault(t => t.CRET == echeanceMoratoire.CRET);
                    if (leAncMor != null)
                    {
                        echeanceMoratoire.REFEM = leAncMor.REFEM;
                        echeanceMoratoire.NDOC  = leAncMor.NDOC ;
                    }

                    moratoires.Add(echeanceMoratoire);
                }

                return moratoires;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cboNoInstall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void txtAmount1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtAmount1.Text))
                {

                    btnOk.IsEnabled = true;
                }
                else
                    btnOk.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void txtReference_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }



        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmRechercheClient recherche = new FrmRechercheClient();
                recherche.Closed += (s, es) =>
                {
                    try
                    {
                        txtReference.Text = string.IsNullOrEmpty(recherche.CustomerRef) ? string.Empty : recherche.CustomerRef;
                        txtClientName.Text = string.IsNullOrEmpty(recherche.CustomerName) ? string.Empty : recherche.CustomerName;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                recherche.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        bool IsProposition = false;

        private void CalculerMoratoire(int NombreEchaeance, decimal montant, int position, bool isProposition)
        {
            try
            {

                InitialiseMontantMoratoires(NombreEchaeance, montant);
                InitialiserDateMoratoiresParFacture(frequenceMoratoire, NombreEchaeance, System.DateTime.Today);
                if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                {
                    int i = position;
                    foreach (string amount in listeAmountMoratoire)
                    {
                        i++;
                        TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                        amounts.Text = amount;
                        amounts.IsReadOnly = !isProposition;
                    }
                }

                if (listeDateMoratoire.Count > 0 && listeDateMoratoire != null)
                {
                    int i = 0;
                    foreach (string date in listeDateMoratoire)
                    {
                        i++;
                        TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                        dateBox.Text = date;
                        //btnPrintQuote.IsEnabled = !isProposition;
                        dateBox.IsReadOnly = !isProposition;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CalculerMoratoire(int frequence, decimal montant, int position, bool isProposition, DateTime DateDebut)
        {
            try
            {
                    InitialiseMontantMoratoires(frequence, montant);
                    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, DateDebut);
                if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                {
                    int i = position;
                    foreach (string amount in listeAmountMoratoire)
                    {
                        i++;
                        TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                        amounts.Text = amount;
                        amounts.IsReadOnly = !isProposition;
                    }
                }

                if (listeDateMoratoire.Count > 0 && listeDateMoratoire != null)
                {
                    int i = 0;
                    foreach (string date in listeDateMoratoire)
                    {
                        i++;
                        TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                        dateBox.Text = date;
                        //btnPrintQuote.IsEnabled = !isProposition;
                        dateBox.IsReadOnly = !isProposition;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnPrintQuote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 int  NombreEchaeance = 0;
                if (listeDetailsMoratoires != null && listeDetailsMoratoires.Count != 0)
                {
                    DateTime dt1 = listeDetailsMoratoires[1].EXIGIBILITE.Value;
                    DateTime dt2 = listeDetailsMoratoires[2].EXIGIBILITE.Value;
                    int datfreqence = (dt2 - dt1).Days;

                    if (datfreqence == 15)
                        frequenceMoratoire = (int)SessionObject.frequenceMoratoire.ForNight;
                    else if (datfreqence == 6)
                        frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Week;
                    else
                        frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Month;

                     NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();

                }
                NombreEchaeance = int.Parse(cboNoInstall.SelectedItem.ToString());
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                CalculerMoratoire(NombreEchaeance, soldeFactureSelect, 0, IsProposition);

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void txtAmount2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void btn_factureClient_Click(object sender, RoutedEventArgs e)
        {

            FrmdetailleFacture ctrl = new FrmdetailleFacture(listFacture);
            ctrl.Closed += new EventHandler(galatee_OkClickedFavtureClient);
            ctrl.Show();
        }

        private void galatee_OkClickedFavtureClient(object sender, EventArgs e)
        {
        }
        private void txtAmount1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount1.Text))
            {
                txtAmount1.Text = Convert.ToDecimal(txtAmount1.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                for (int i = 1; i <= 1; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 1, (soldeFactureSelect - MontantRestant), 1, true);
            }
        }
        private void txtAmount2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount2.Text))
            {
                txtAmount2.Text = Convert.ToDecimal(txtAmount2.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 2; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 2, (soldeFactureSelect - MontantRestant), 2, true);
            }
        }
        private void txtAmount3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount3.Text))
            {
                txtAmount3.Text = Convert.ToDecimal(txtAmount3.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 3; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 3, (soldeFactureSelect - MontantRestant), 3, true);
            }
        }
        private void txtAmount4_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount4.Text))
            {
                txtAmount4.Text = Convert.ToDecimal(txtAmount4.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;

                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 4; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 4, (soldeFactureSelect - MontantRestant), 4, true);
            }
        }
        private void txtAmount5_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount5.Text))
            {
                txtAmount5.Text = Convert.ToDecimal(txtAmount5.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 5; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 5, (soldeFactureSelect - MontantRestant), 5, true);
            }
        }
        private void txtAmount6_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount6.Text))
            {
                txtAmount6.Text = Convert.ToDecimal(txtAmount6.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 6; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 6, (soldeFactureSelect - MontantRestant), 6, true);
            }
        }
        private void txtAmount7_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount7.Text))
            {
                txtAmount7.Text = Convert.ToDecimal(txtAmount7.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;

                IsProposition = true;
                for (int i = 1; i <= 7; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 7, (soldeFactureSelect - MontantRestant), 7, true);
            }
        }
        private void txtAmount8_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount8.Text))
            {
                txtAmount8.Text = Convert.ToDecimal(txtAmount8.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = true;
                for (int i = 1; i <= 8; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 8, (soldeFactureSelect - MontantRestant), 8, true);
            }
        }
        private void txtAmount9_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount9.Text))
            {
                txtAmount9.Text = Convert.ToDecimal(txtAmount9.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = true;
                for (int i = 1; i <= 9; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 9, (soldeFactureSelect - MontantRestant), 9, true);
            }
        }
        private void txtAmount10_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount10.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount10.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = true;
                for (int i = 1; i <= 10; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 10, (soldeFactureSelect - MontantRestant), 10, true);
            }
        }
        private void txtAmount11_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount11.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount11.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = true;
                for (int i = 1; i <= 11; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 11, (soldeFactureSelect - MontantRestant), 11, true);
            }
        }
        private void txtAmount12_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount12.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount12.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int NombreEchaeance = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Count();
                soldeFactureSelect = listeDetailsMoratoires.Where(t => t.EXIGIBILITE != null).Sum(t => t.MONTANT).Value;
                IsProposition = true;
                for (int i = 1; i <= 12; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(NombreEchaeance - 12, (soldeFactureSelect - MontantRestant), 12, true);
            }
        }

        private void txtDate1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDate1.Text))
            {
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, Convert.ToDateTime(txtDate1.Text));
                int i = 0;
                foreach (string date in listeDateMoratoire)
                {
                    i++;
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    dateBox.Text = date;
                }


            }
        }
    }
}


