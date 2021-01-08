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
using Galatee.Silverlight.ServiceFacturation  ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Galatee.Silverlight.Resources.Index;


namespace Galatee.Silverlight.Facturation
{
    public partial class UcSaisieIndividuelle : ChildWindow, INotifyPropertyChanged
    {
        List<CsEvenement> ListeEvenement = new List<CsEvenement>();
        List<CsEvenement> InfoConsommationPrec = new List<CsEvenement>();
        List<CsEvenement> ListeEvenementNonFact = new List<CsEvenement>();
        List<Galatee.Silverlight.ServiceFacturation.CsCasind> LstCas = new List<Galatee.Silverlight.ServiceFacturation.CsCasind>();
        public event PropertyChangedEventHandler PropertyChanged;
        List<CsEvenement> ListeEvenementASaisi = new List<CsEvenement>();
        List<CsEvenement> EvenemntPageCourante = new List<CsEvenement>();
        CsEvenement LeEvtSelect;
        List<DataGridRow> Rows = new List<DataGridRow>();
        int Action = 1;   // 1 :Modification par defaut,2:creation,3:suppression
        int moyenneConso;
        bool passageFirst = false;
        bool IsEtatSaisie = true;
        bool IsConsoSaisie = false;
        int? AncConso = 0;
        //int NombreClientLot = 0;
        ObservableCollection<CsEvenement> listEvenemntCouranteDansLaGrid = new ObservableCollection<CsEvenement>();
        List<CsEvenement> listEvenemntPrecedentCompteur = new List<CsEvenement>();

        public UcSaisieIndividuelle()
        {
            InitializeComponent();
        }

        public UcSaisieIndividuelle(List<CsEvenement> _ListeEvenement)
        {

            try
            {
                InitializeComponent();
                ListeEvenement = _ListeEvenement;
                ChargeListeDesCas();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        public UcSaisieIndividuelle(List<CsSaisiIndexIndividuel> _ListeEvenementCompteur)
        {

            try
            {
                InitializeComponent();
                foreach (CsSaisiIndexIndividuel item in _ListeEvenementCompteur)
                {
                    ListeEvenement.AddRange(item.EventPageri);
                    ListeEvenement.AddRange(item.EventPagisol );
                    ListeEvenement.AddRange(item.EventLotriNull );
                    listEvenemntPrecedentCompteur.AddRange(item.ConsoPrecedent);
                    if (ListeEvenement.FirstOrDefault(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT) != null)
                    {
                        this.dataGrid1.Columns[2].Header = "Comptage";
                        ListeEvenement.ForEach(t => t.DIAMETRE = t.TYPECOMPTAGE); 
                    }
                    
                }
                ChargeListeDesCas();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        public UcSaisieIndividuelle(List<CsEvenement> _ListeEvenement,List<CsEvenement> Infoconso,int? avgConso)
        {

            try
            {
                InitializeComponent();
                InfoConsommationPrec = Infoconso;
                ListeEvenement = _ListeEvenement;
                moyenneConso = avgConso.Value;
                ChargeListeDesCas();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        public UcSaisieIndividuelle(List<CsEvenement> _ListeEvenement, List<CsEvenement> Infoconso)
        {

            try
            {
                InitializeComponent();
                InfoConsommationPrec = Infoconso;
                ListeEvenement = _ListeEvenement;
                //moyenneConso = avgConso.Value;
                ChargeListeDesCas ();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        List<CsEvenement> OrdonnerListeEvenement(List<CsEvenement> pEvenement)
        {
            try
            {
                return pEvenement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillDataGridView(List<CsEvenement> _LstEvt)
        {
            try
            {
                foreach (var ev in _LstEvt.OrderBy(t=>t.ORDREAFFICHAGE ).OrderByDescending (o=>o.DATEEVT ))
                    listEvenemntCouranteDansLaGrid.Add(ev);

                if (listEvenemntPrecedentCompteur.First() == null)
                {
                    Message.Show("Ce client n'a pas d'evenement de pose", "Index");
                    return;
                }
                this.Txt_DateEvt.Text = System.DateTime.Today.ToShortDateString();
                dataGrid1.ItemsSource = listEvenemntCouranteDansLaGrid;
                dataGrid1.SelectedItem = listEvenemntCouranteDansLaGrid.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        private void ReinitialiseChamp()
        {
            try
            {
                this.Txt_DateEvt.Text = string.Empty;
                this.Txt_periode.Text = string.Empty;
                this.Txt_IndexEvt.Text = string.Empty;
                this.Txt_CasIndex.Text = string.Empty;
                this.Txt_Consomation.Text = string.Empty;
                //this.Txt_Enquete.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        private void ChargeListeDesCas()
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneListeDesCasCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowError("La liste des cas retournée est vide. Veuillez reessayer svp!", "Erreur");
                            return;
                        }

                        LstCas = args.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.RetourneListeDesCasAsync(string.Empty , string.Empty );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Txt_IndexEvt.Text) && string.IsNullOrEmpty(Txt_CasIndex.Text) && string.IsNullOrEmpty(Txt_IndexEvt.Text) && string.IsNullOrEmpty(Txt_Consomation.Text))
                {
                    Message.ShowInformation("Aucune donnée saisie. Renseignez les données de relevé svp !", "Erreur");
                    return;
                }
                dataGrid1.IsEnabled = true;
                bool sautDeSaisi = false;
                if (Txt_IndexEvt.Text.Equals("*"))
                    sautDeSaisi = true;
                if (Action == 2)
                {
                    if (dataGrid1.ItemsSource != null)
                    {
                        LeEvtSelect = GetElementEvenement(((List<CsEvenement>)dataGrid1.ItemsSource).Last());
                        LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                        LeEvtSelect.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);
                        LeEvtSelect.CAS = this.Txt_CasIndex.Text;
                        LeEvtSelect.PERIODE = this.Txt_periode.Text;
                        LeEvtSelect.CONSO = !string.IsNullOrEmpty(this.Txt_Consomation.Text) ? int.Parse(this.Txt_Consomation.Text) : LeEvtSelect.INDEXEVT - LeEvtSelect.INDEXPRECEDENTEFACTURE;
                        IsSaisieValider(LeEvtSelect, LstCas);

                    }
                }
                else if (Action == 1)
                {

                    if (string.IsNullOrEmpty(this.Txt_CasIndex.Text) || this.Txt_CasIndex.Text == "##") this.Txt_CasIndex.Text = "00";
                    CsEvenement events = (CsEvenement)this.dataGrid1.SelectedItem;
                    LeEvtSelect = new CsEvenement();
                    LeEvtSelect = listEvenemntCouranteDansLaGrid.First(ev => ev.CENTRE == events.CENTRE &&
                                                                      ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                      ev.PRODUIT == events.PRODUIT && ev.POINT == events.POINT && ev.NUMEVENEMENT == events.NUMEVENEMENT);

                    LeEvtSelect.CAS = this.Txt_CasIndex.Text;
                    LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                    if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                        LeEvtSelect.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);


                    LeEvtSelect.IsSaisi = true;
                    LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);


                    LeEvtSelect.IsSaisi = true;
                    LeEvtSelect.STATUS = SessionObject.Enumere.EvenementReleve;
                    LeEvtSelect.STATUSPAGERIE = 0;
                    LeEvtSelect.MATRICULE = UserConnecte.matricule;
                    LeEvtSelect.IsFromPageri = true;
                    //LeEvtSelect.ORDRESAISIE = OrdreSaisi;
                    VerifieCas(true);
                    if (!EstCasValider)
                        return;
                    if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                        LeEvtSelect.INDEXEVT = LeEvtSelect.INDEXPRECEDENTEFACTURE;

                    if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        List<CsEvenement> LeEvtSelectClient = listEvenemntCouranteDansLaGrid.Where(ev => ev.CENTRE == events.CENTRE &&
                                                                       ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                       ev.PRODUIT == events.PRODUIT).ToList();
                        if (LeEvtSelect.CAS != "10")
                            LeEvtSelectClient.Where(y => y.CAS != "10").ToList().ForEach(t => t.CAS = LeEvtSelect.CAS);
                    }


                    if (LeEvtSelect.CAS == "13" || LeEvtSelect.CAS == "27")
                        LeEvtSelect.COMPTEURAFFICHER = this.txt_NumCpt.Text;
                    else
                        this.txt_NumCpt.Text = string.Empty;

                    if (LeEvtSelect.CAS != "13" && LeEvtSelect.CAS != "27")
                        ValidationSaisieIndex(LeEvtSelect);
                    else
                        IsSaisieValider(LeEvtSelect, LstCas);
                }
                else if (Action == 3)
                {
                     var ws = new MessageBoxControl.MessageBoxChildWindow("Facturation", "Voulez vous supprimer l'evenement saisi ?", MessageBoxControl.MessageBoxButtons.YesNo , MessageBoxControl.MessageBoxIcon.Information);
                     ws.OnMessageBoxClosed += (l, results) =>
                     {
                         if (ws.Result == MessageBoxResult.OK)
                         {
                             CsEvenement events = (CsEvenement)this.dataGrid1.SelectedItem;
                             LeEvtSelect = new CsEvenement();
                             LeEvtSelect = listEvenemntCouranteDansLaGrid.First(ev => ev.CENTRE == events.CENTRE &&
                                                                               ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                               ev.PRODUIT == events.PRODUIT && ev.POINT == events.POINT && ev.NUMEVENEMENT == events.NUMEVENEMENT);

                             LeEvtSelect.STATUS = SessionObject.Enumere.EvenementSupprimer;
                             LeEvtSelect.USERCREATION  = UserConnecte.matricule ;
                             ValiderSaisi();
                         }
                         else return;
                     };
                     ws.Show();
                   
                }
                if (sautDeSaisi) // selectionne l'element suivant dans la grid
                {
                    int indexElementSelected = this.dataGrid1.SelectedIndex + 1;

                    if (indexElementSelected <= listEvenemntCouranteDansLaGrid.Count() - 1)
                    {
                        dataGrid1.SelectedIndex = indexElementSelected;
                        Txt_IndexEvt.Text = string.Empty;
                        dataGrid1.IsEnabled = false;
                    }
                    else
                    {
                        if (listEvenemntCouranteDansLaGrid.Count() > 0)
                        {
                            dataGrid1.SelectedItem = listEvenemntCouranteDansLaGrid.First();
                            Txt_IndexEvt.Text = string.Empty;
                            dataGrid1.IsEnabled = false;
                        }
                        else
                        {
                            Message.ShowInformation("Fin de la saisie d'index ", "Information");
                            this.DialogResult = true;
                        }
                    }
                    ScrollGrid();
                    Txt_IndexEvt.Text = string.Empty;
                    dataGrid1.IsEnabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
               Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        bool EstCasValider = true;
        CsCasind LeCasRecherche = new CsCasind();
        private void VerifieCas(bool isvalidation)
        {
            EstCasValider = true;
            // Saisie d'index
            if (LeCasRecherche.CODE != "05" && LeCasRecherche.CODE != "10"
                && LeCasRecherche.CODE != "13"
                && LeCasRecherche.CODE != "14"
                && LeCasRecherche.CODE != "27"
                && LeCasRecherche.CODE != "00" &&
                (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text) && int.Parse(this.Txt_IndexEvt.Text) < LeEvtSelect.INDEXPRECEDENTEFACTURE))
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Ce cas ne correspond pas a l'index saisi", "Alert");
                    IsEtatSaisie = false;
                    dataGrid1.IsEnabled = false;
                }
                EstCasValider = false;

                return;
            }

            if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeObligatoire)
            {
                if (string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation(Langue.msg_SaisiIndexObligatoire, "Alert");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;

                    return;

                }
            }
            if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
            {
                if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                {
                    //if (LeCasRecherche.CODE != "13" && LeCasRecherche.CODE != "27")
                    if ((LeCasRecherche.CODE == "01" || LeCasRecherche.CODE == "02" ||
                        LeCasRecherche.CODE == "04" || LeCasRecherche.CODE == "06" ||
                        LeCasRecherche.CODE == "18" || LeCasRecherche.CODE == "19" ||
                        LeCasRecherche.CODE == "22" || LeCasRecherche.CODE == "22") &&
                        (int.Parse(this.Txt_IndexEvt.Text) != LeEvtSelect.INDEXPRECEDENTEFACTURE))
                    {
                        if (isvalidation)
                        {
                            Message.ShowInformation(Langue.msg_SaisiIndexInterdite, "Alert");
                            IsEtatSaisie = false;
                            dataGrid1.IsEnabled = false;
                            LeEvtSelect.INDEXEVT = null;
                            this.Txt_IndexEvt.Text = string.Empty;
                        }
                        EstCasValider = false;
                        return;
                    }
                }
                else
                    this.Txt_IndexEvt.IsReadOnly = true;
            }
            // Saisie de la consomation
            if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeObligatoire)
            {
                if (!string.IsNullOrEmpty(this.Txt_Consomation.Text))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation(Langue.msg_SaisiConsoObligatoire, "Alert");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;
                }

            }
            else if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeInterdit)
            {
                if (!string.IsNullOrEmpty(this.Txt_Consomation.Text.ToString()))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation(Langue.msg_SaisiConsoInterdite, "Alert");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                        this.Txt_Consomation.Text = string.Empty;
                    }
                    EstCasValider = false;

                    return;
                }
                else
                    this.Txt_Consomation.IsReadOnly = true;
            }
            if (LeCasRecherche.CODE == "05")
            {
                if (LeEvtSelect.INDEXEVT > LeEvtSelect.INDEXPRECEDENTEFACTURE)
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("Ce cas ne correspond pas a l'index saisi", "Alert");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;

                    return;

                }
            }
            if (LeCasRecherche.SAISIECOMPTEUR == SessionObject.Enumere.CodeObligatoire)
            {
                if (string.IsNullOrEmpty(this.txt_NumCpt.Text))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("La saisie du compteur est obligatoire", "Alert");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;
                }
            }

        }
        private void ValidationSaisieIndex(CsEvenement LaSaise)
        {
            try
            {
                CsEvenement LeEvt = new CsEvenement();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.VerificationNumeroCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    bool IsCompteurValide = args.Result;
                    if (IsCompteurValide)
                    {
                        IsSaisieValider(LeEvtSelect, LstCas);
                    }
                    else
                    {
                        Message.ShowWarning("Numéro de compteur different", "Facturation");
                        LeEvtSelect.CAS = "85";
                        ValiderSaisi();
                    }
                };
                service.VerificationNumeroCompteurAsync(LaSaise);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void RetourneAZero(CsEvenement LaSaise, string leCasAMettre)
        {
            try
            {
                //CsCanalisation LaCanalisation = new  CsCanalisation();
                //FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                //service.RetourneCanalisationbyIdEvenementCompleted += (s, args) =>
                //{
                //    if (args != null && args.Cancelled)
                //        return;
                //    LaCanalisation = args.Result;

                LeEvtSelect.CAS = leCasAMettre;
                if (LaSaise.CADRAN.ToString() == "0")
                {
                    Message.ShowWarning(Galatee.Silverlight.Resources.Accueil.Langue.msgCadranIndeterminer, Langue.libelleModule);
                    LeEvtSelect.INDEXEVT = null;
                    LeEvtSelect.CONSO = null; ;
                    return;
                }
                else
                {
                    int Roue = LaSaise.CADRAN == null ? 1 : int.Parse(LaSaise.CADRAN.ToString());
                    int initval = 9;
                    int? Indexmax = int.Parse(initval.ToString().PadLeft(Roue, '9'));
                    LeEvtSelect.CONSO = ((Indexmax - LaSaise.INDEXPRECEDENTEFACTURE) + (int.Parse(this.Txt_IndexEvt.Text) + 1));
                    LeEvtSelect.ENQUETE = LeCasRecherche.ENQUETABLE ? "E" : string.Empty;
                }
                ValiderSaisi();
                //};
                //service.RetourneCanalisationbyIdEvenementAsync(LaSaise.FK_IDCANALISATION );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void IsSaisieValider(CsEvenement LaSaisie, List<CsCasind> LstCas)
        {
            LeEvtSelect = new CsEvenement();
            LeEvtSelect = LaSaisie;
            CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LaSaisie.CAS);
            if (LeCasRecherche == null)
            {
                Message.ShowInformation(Langue.msg_CasInexistant, "Erreur");
                IsEtatSaisie = false;
                dataGrid1.IsEnabled = false;
                return;
            }
            IsCasValider(LaSaisie, LeCasRecherche);
        }
        private void RetourneEvenemntPose(CsEvenement LaSaise)
        {
            try
            {
                CsEvenement LeEvt = new CsEvenement();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneEvenementPoseCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LeEvt = args.Result;
                    if (LeEvt != null)
                    {
                        LaSaise.INDEXPRECEDENTEFACTURE = LeEvt.INDEXEVT;
                        LaSaise.CONSO = LeEvt.INDEXEVT - LaSaise.INDEXEVT;
                        LaSaise.FK_IDCANALISATION = LeEvt.FK_IDCANALISATION;
                        LaSaise.NUMEVENEMENT = LeEvt.NUMEVENEMENT + 1;
                        LaSaise.TYPECOMPTEUR = LeEvt.TYPECOMPTEUR;
                        LaSaise.DERPERF = LeEvt.DERPERF;
                        LaSaise.DERPERFNPREC = LeEvt.DERPERFNPREC;
                        LaSaise.INDEXPRECEDENTEFACTURE = LeEvt.INDEXPRECEDENTEFACTURE;
                        LaSaise.DATERELEVEPRECEDENTEFACTURE = LeEvt.DATERELEVEPRECEDENTEFACTURE;
                        LaSaise.CASPRECEDENTEFACTURE = LeEvt.CASPRECEDENTEFACTURE;
                        LaSaise.PERIODEPRECEDENTEFACTURE = LeEvt.PERIODEPRECEDENTEFACTURE;
                        LaSaise.NOUVCOMPTEUR = this.txt_NumCpt.Text; ;
                        LaSaise.ISEVTPOSETROUVE = true;
                        LaSaise.STATUS = SessionObject.Enumere.EvenementSupprimer;
                        ValiderSaisi();
                    }
                    else
                    {
                        Message.ShowWarning("Les données de changement du compteur(" + LaSaise.COMPTEUR + ") n'ont pas été saisies" + "\n\r" + "Ce index ne sera pas pris en compte pour cette facturation", "Facturation");
                        if (!string.IsNullOrEmpty(this.txt_NumCpt.Text))
                        {
                            this.txt_NumCpt.Text = string.Empty;
                            this.txt_NumCpt.IsReadOnly = true;
                        }

                        LaSaise.NOUVCOMPTEUR = this.txt_NumCpt.Text;
                        LaSaise.CONSO = null;
                        ValiderSaisi();

                        //ReinitialiserGrid();

                        return;
                    }
                };
                service.RetourneEvenementPoseAsync(LaSaise);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void IsCasValider(CsEvenement LaSaise, CsCasind leCasSaisi)
        {
            if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
            {
                if (int.Parse(this.Txt_IndexEvt.Text) < LaSaise.INDEXPRECEDENTEFACTURE &&
                    LaSaise.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                {
                    if (leCasSaisi.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                    {
                        LeEvtSelect.INDEXEVT = null;
                        LeEvtSelect.CONSO = null;
                        LeEvtSelect.IsSaisi = true;
                        dataGrid1.IsEnabled = true;
                        ValiderSaisi();
                        return;
                    }
                    if (leCasSaisi.CODE == "13")
                    {
                        RetourneEvenemntPose(LeEvtSelect);
                        return;
                    }

                    if (LeEvtSelect.CAS == "10")  // Ne pas afficher le message de retour a zero
                    {
                        RetourneAZero(LaSaise, LeEvtSelect.CAS);
                        return;
                    }
                    else if (LeEvtSelect.CAS == "05" || LeEvtSelect.CAS == "14" || LeEvtSelect.CAS == "27" || LeEvtSelect.CAS == "13")
                        ValiderSaisi();

                    else
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_RetourAZero, MessageBoxControl.MessageBoxButtons.YesNoCancel, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = "82";
                                ValiderSaisi();
                            }
                            else if (ws.Result == MessageBoxResult.OK)
                                RetourneAZero(LaSaise, "10");
                        };
                        ws.Show();
                    }
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
            {
                int? MoyenneComparaisonSup = LaSaise.CONSOMOYENNEPRECEDENTEFACTURE * 2;
                int? MoyenneComparaisonInf = (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE) * 2;
                if (LaSaise.CAS == "00" && MoyenneComparaisonInf < LaSaise.CONSOMOYENNEPRECEDENTEFACTURE)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoFaible, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            LeEvtSelect.CAS = string.Empty;
                            LeEvtSelect.INDEXEVT = null;
                            LeEvtSelect.CONSO = null;
                            LeEvtSelect.IsSaisi = false;
                            dataGrid1.IsEnabled = false;
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            LaSaise.CAS = "84";
                            ValiderSaisi();
                        }
                    };
                    ws.Show();
                    return;
                }
                if (LaSaise.CAS == "00" && (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE) > MoyenneComparaisonSup)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoForte, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            LeEvtSelect.CAS = string.Empty;
                            LeEvtSelect.INDEXEVT = null;
                            LeEvtSelect.CONSO = null;
                            LeEvtSelect.IsSaisi = false;
                            dataGrid1.IsEnabled = false;
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            LaSaise.CAS = "83";
                            ValiderSaisi();
                        }
                    };
                    ws.Show();
                    return;
                }
            }
            ValiderSaisi();

        }

      

        void desableProgressBar()
        {
            //progressBar1.IsIndeterminate = false;
            //progressBar1.Visibility = Visibility.Collapsed;
            //OKButton.IsEnabled = false;
        }

        void allowProgressBar()
        {
            //OKButton.IsEnabled = true;
            //progressBar1.IsEnabled = true;
            //progressBar1.Visibility = Visibility.Visible;
            //progressBar1.IsIndeterminate = true;
        }

        private void InsererEvenement(CsEvenement _LEvenement)
        {
            try
            {

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.InsererEvenementCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            allowProgressBar();
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur de la mise à jour du lot.Veuillez réessayer svp!", "Erreur");
                            allowProgressBar();
                            return;
                        }
                        allowProgressBar();
                        ReinitialiserGrid();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.InsererEvenementAsync(_LEvenement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateEvenement(CsEvenement _ListEvenement)
        {
            try
            {
                List<CsEvenement> listeevnt = new List<CsEvenement>();
                listeevnt.Add(_ListEvenement);

                //if ((IndexASelectionner == 5) || (_ListEvenement.Count == ListeEvenement.Count))
                //{
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.MisAJourEvenementCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            allowProgressBar();
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur de la mise à jour du lot.Veuillez réessayer svp!", "Erreur");
                            allowProgressBar();
                            return;
                        }
                        allowProgressBar();
                       
                        //RemoveElementGridApresInsert(args.Result);
                        ReinitialiserGrid();
                        
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.MisAJourEvenementAsync(listeevnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ReinitialiserGrid()
        {
            try
            {
                int indexElementSelected = this.dataGrid1.SelectedIndex + 1;
                if (indexElementSelected <= listEvenemntCouranteDansLaGrid.Count() - 1)
                {
                    dataGrid1.IsReadOnly = true;
                    dataGrid1.SelectedIndex = indexElementSelected;
                    dataGrid1.IsReadOnly = false;
                    ChangeSelectedItemColor();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemoveElementGridApresInsert(CsEvenement LeEvtSelect)
        {
            try
            {
                dataGrid1.IsEnabled = true;
                CsEvenement index = listEvenemntCouranteDansLaGrid.FirstOrDefault();
                listEvenemntCouranteDansLaGrid.Remove(index);
                listEvenemntCouranteDansLaGrid.Add(LeEvtSelect);
                ChangeSelectedItemColor();
                dataGrid1.IsEnabled = false;
                dataGrid1.IsReadOnly = true;
                //lvwResultat.UpdateLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChangeSelectedItemColor()
        {
            try
            {
                //to get the current row binding value
                CsEvenement currentRow = (CsEvenement)dataGrid1.SelectedItem;

                //to read the currentRow
                DataGridRow selectedRow = Rows[dataGrid1.SelectedIndex];
                //color row
                var backgroundRectangle = SearchFrameworkElement(selectedRow, "BackgroundRectangle") as Rectangle;
                if (backgroundRectangle != null)
                {
                    backgroundRectangle.Fill = new SolidColorBrush(Colors.Cyan);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static FrameworkElement SearchFrameworkElement(FrameworkElement parentFrameworkElement, string childFrameworkElementNameToSearch)
        {
            try
            {
                FrameworkElement childFrameworkElementFound = null;
                SearchFrameworkElement(parentFrameworkElement, ref childFrameworkElementFound, childFrameworkElementNameToSearch);
                return childFrameworkElementFound;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SearchFrameworkElement(FrameworkElement parentFrameworkElement, ref FrameworkElement childFrameworkElementToFind, string childFrameworkElementName)
        {
            try
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(parentFrameworkElement);
                if (childrenCount > 0)
                {
                    FrameworkElement childFrameworkElement = null;
                    for (int i = 0; i < childrenCount; i++)
                    {
                        childFrameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(parentFrameworkElement, i);
                        if (childFrameworkElement != null && childFrameworkElement.Name.Equals(childFrameworkElementName))
                        {
                            childFrameworkElementToFind = childFrameworkElement;
                            return;
                        }
                        SearchFrameworkElement(childFrameworkElement, ref childFrameworkElementToFind, childFrameworkElementName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void ValiderSaisi()
        {
            try
            {

                CsCasind leCasSaisi = LstCas.FirstOrDefault(t => t.CODE == LeEvtSelect.CAS);
                if (leCasSaisi.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                    LeEvtSelect.CONSO = null;


                LeEvtSelect.ENQUETE = leCasSaisi.ENQUETABLE ? "E" : string.Empty;
                if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                    LeEvtSelect.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);

                if (LeEvtSelect.CONSO == null && LeEvtSelect.CAS != "13") //Pour eviter les ca 10 
                {
                    if (LeEvtSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                    {
                        if (string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                            LeEvtSelect.CONSO = null;
                        else
                            LeEvtSelect.CONSO = string.IsNullOrEmpty(this.Txt_Consomation.Text) ? (int?)((int.Parse(this.Txt_IndexEvt.Text) - LeEvtSelect.INDEXPRECEDENTEFACTURE) * (LeEvtSelect.COEFLECT == 0 ? 1 : LeEvtSelect.COEFLECT)) : int.Parse(this.Txt_Consomation.Text);
                    }
                    else
                        LeEvtSelect.CONSO = LeEvtSelect.INDEXEVT;
                }

                allowProgressBar();
                UpdateEvenement(LeEvtSelect);
                ReinitialiserGrid();
                if (!string.IsNullOrEmpty(this.txt_NumCpt.Text))
                {
                    this.txt_NumCpt.Text = string.Empty;
                    this.txt_NumCpt.IsReadOnly = true;
                }
                if (this.Txt_IndexEvt.IsReadOnly)
                    this.Txt_IndexEvt.IsReadOnly = false;

                if (this.Txt_Consomation.IsReadOnly)
                    this.Txt_Consomation.IsReadOnly = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void MiseAJourEvenement(CsEvenement _LeEvenementSelect,int _Action)
        //{

        //    try
        //    {
        //        CsEvenement _LeEvenementRecherche = ListeEvenementNonFact.FirstOrDefault(p => p.NUMEVENEMENT == _LeEvenementSelect.NUMEVENEMENT);
        //        if (_LeEvenementRecherche != null)
        //        {
        //            if (_Action == 2)
        //            {
        //                CsEvenement _LeEvenementCree = GetElementEvenement(_LeEvenementRecherche);
        //                _LeEvenementCree.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
        //                _LeEvenementCree.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);
        //                _LeEvenementCree.CAS = this.Txt_Cas.Text;
        //                //_LeEvenementCree.PERIODE = this.Txt_periode.Text;
        //                _LeEvenementCree.CONSO = !string.IsNullOrEmpty(this.Txt_ConsoSaisie.Text) ? int.Parse(this.Txt_ConsoSaisie.Text) : _LeEvenementCree.INDEXEVT - _LeEvenementCree.INDEXEVTPRECEDENT;
        //                if (new  Galatee.Silverlight.Index.ClasseMethodeGenerique().IsSaisieValider(_LeEvenementCree, LstCas))
        //                {
        //                    ActionEvenement(_LeEvenementCree, _Action);
        //                    ListeEvenementNonFact.Add(_LeEvenementCree);
        //                    UpdateDataGrid(ListeEvenementNonFact);
        //                }
        //            }
        //            else if (_Action == 1)
        //            {

        //                _LeEvenementRecherche.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);

        //                _LeEvenementRecherche.INDEXEVT = null;
        //                //if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
        //                _LeEvenementRecherche.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);

        //                _LeEvenementRecherche.CAS = this.Txt_Cas.Text;
        //                _LeEvenementRecherche.PERIODE = this.Txt_periode.Text;
        //                _LeEvenementRecherche.CONSO = !string.IsNullOrEmpty(this.Txt_ConsoSaisie.Text) ? int.Parse(this.Txt_ConsoSaisie.Text) : _LeEvenementRecherche.INDEXEVT - _LeEvenementRecherche.INDEXEVTPRECEDENT;
        //                if (new  Galatee.Silverlight.Index.ClasseMethodeGenerique().IsSaisieValider(_LeEvenementRecherche, LstCas))
        //                {
        //                    ActionEvenement(_LeEvenementRecherche, _Action);
        //                    UpdateDataGrid(ListeEvenementNonFact);
        //                }
        //            }
        //            else if (_Action == 3)
        //            {
        //                _LeEvenementRecherche.STATUS = SessionObject.Enumere.EvenementSupprimer;
        //                ActionEvenement(_LeEvenementRecherche, _Action);
        //                foreach (CsEvenement item in ListeEvenementNonFact)
        //                {
        //                    CsEvenement _LeEvtSupprimer = ListeEvenementNonFact.FirstOrDefault(p => p.NUMEVENEMENT == _LeEvenementRecherche.NUMEVENEMENT);
        //                    if (_LeEvtSupprimer != null)
        //                        ListeEvenementNonFact.Remove(_LeEvtSupprimer);
        //                }
        //                UpdateDataGrid(ListeEvenementNonFact);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
        //}



        public CsEvenement  GetElementEvenement(CsEvenement  _LeEvt)
        {
            try
            {
                CsEvenement _LeEvenement = new CsEvenement()
                   {
                       CENTRE = _LeEvt.CENTRE,
                       CLIENT = _LeEvt.CLIENT,
                       ORDRE = _LeEvt.ORDRE,
                       PRODUIT = _LeEvt.PRODUIT,
                       POINT = _LeEvt.POINT,
                       NUMEVENEMENT = _LeEvt.NUMEVENEMENT + 1,
                       COMPTEUR = _LeEvt.COMPTEUR,
                       DATEEVT = _LeEvt.DATEEVT,
                       PERIODE = _LeEvt.PERIODE,
                       CODEEVT = _LeEvt.CODEEVT,
                       INDEXEVT = _LeEvt.INDEXEVT,
                       CAS = _LeEvt.CAS,
                       ENQUETE = _LeEvt.ENQUETE,
                       CONSO = _LeEvt.CONSO,
                       CONSONONFACTUREE = _LeEvt.CONSONONFACTUREE,
                       LOTRI = _LeEvt.LOTRI,
                       FACTURE = _LeEvt.FACTURE,
                       SURFACTURATION = _LeEvt.SURFACTURATION,
                       STATUS = _LeEvt.STATUS,
                       TYPECONSO = _LeEvt.TYPECONSO,
                       DIAMETRE = _LeEvt.DIAMETRE,
                       FACTTOT = _LeEvt.FACTTOT,
                       MATRICULE = _LeEvt.MATRICULE,
                       FACPER = _LeEvt.FACPER,
                       QTEAREG = _LeEvt.QTEAREG,
                       DERPERF = _LeEvt.DERPERF,
                       DERPERFN = _LeEvt.DERPERFN,
                       CONSOFAC = _LeEvt.CONSOFAC,
                       REGIMPUTE = _LeEvt.REGIMPUTE,
                       REGCONSO = _LeEvt.REGCONSO,
                       COEFLECT = _LeEvt.COEFLECT,
                       COEFCOMPTAGE = _LeEvt.COEFCOMPTAGE,
                       TYPECOMPTEUR = _LeEvt.TYPECOMPTEUR,
                       COEFK1 = _LeEvt.COEFK1,
                       COEFK2 = _LeEvt.COEFK2
                   };
                return _LeEvenement;
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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Action = 1;
                FillDataGridView(ListeEvenement);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }

        }

        private void RenseignerInfoConsommationPrecedente(CsEvenement InfoConsommationPrec)
        {
            try
            {


                this.Txt_CasLibelle.Text = InfoConsommationPrec.CAS == null ? string.Empty : InfoConsommationPrec.CAS;
                this.Txt_ReadingPrec.Text = InfoConsommationPrec.INDEXEVT == null ? string.Empty : InfoConsommationPrec.INDEXEVT.ToString();
                this.Txt_periodePrec.Text = InfoConsommationPrec.PERIODE;
                //this.Txt_CasLibelle.Text = evenement.LIBELLECASPRECEDENT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_creation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Action = 2;
                this.Txt_periode.Visibility = System.Windows.Visibility.Visible;
                this.lbl_Periode.Visibility = System.Windows.Visibility.Visible;
                ReinitialiseChamp();
            }
            catch (Exception ex)
            {
               Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void MettreAJourChampEvenementSelectionne(CsEvenement evenement)
        {
            try
            {
                this.lb_Batch.Content = evenement.LOTRI == null ? string.Empty : evenement.LOTRI;
                this.lb_Centre.Content = evenement.CENTRE == null ? string.Empty : evenement.CENTRE;
                this.Txt_periode.Text = evenement.PERIODE == null ? string.Empty : evenement.PERIODE;
                this.lb_Client.Content = evenement.CLIENT == null ? string.Empty : evenement.CLIENT;
                this.lb_point.Content =  evenement.POINT.ToString();
                this.lb_Produit.Content = evenement.PRODUIT == null ? string.Empty : evenement.PRODUIT.ToString();
                this.lb_Releveur.Content = evenement.RELEVEUR == null ? string.Empty : evenement.RELEVEUR.ToString();

                //this.Txt_Diametre.Text = evenement.DIAMETRE == null ? string.Empty : evenement.DIAMETRE;
                //this.Txt_Compteur.Text = evenement.COMPTEUR == null ? string.Empty : evenement.COMPTEUR;

                //this.Txt_DateEvt.Text = evenement.DATEEVT == null ? string.Empty:(Convert.ToDateTime  (evenement.DATEEVT)).ToShortDateString();
                //this.Txt_IndexEvt.Text = evenement.INDEXEVT == null ? string.Empty: evenement.INDEXEVT.ToString();
                //this.Txt_ConsoCalc.Text = evenement.CONSO == null ? string.Empty:evenement.CONSO.ToString();
                //this.Txt_ConsoCalc.Text = "1209";
                this.Txt_Consomation.Text = evenement.CONSO == null ? string.Empty : evenement.CONSO.ToString();
                this.Txt_CasIndex.Text = evenement.CASEVENEMENT == null ? string.Empty : evenement.CASEVENEMENT;
                //this.Txt_Enquete.Text = evenement.ENQUETE == null ? string.Empty : evenement.ENQUETE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ScrollGrid()
        {
            try
            {
                if (passageFirst)
                {
                    dataGrid1.IsEnabled = true;
                    dataGrid1.ScrollIntoView(dataGrid1.SelectedItem, dataGrid1.Columns[1]);
                    dataGrid1.UpdateLayout();
                    //dataGrid1.IsEnabled = false;
                }
                else
                    passageFirst = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsEvenement evenement = dataGrid1.SelectedItem as CsEvenement;
                if (evenement == null)
                    return;
                AncConso = evenement.CONSO;
                Txt_IndexEvt.Focus();
       
                CsEvenement InfoConsommationPrec = listEvenemntPrecedentCompteur.FirstOrDefault(u => u.POINT == evenement.POINT);
                evenement.CONSOPRECEDENT = InfoConsommationPrec.CONSO;
                evenement.INDEXPRECEDENTEFACTURE = InfoConsommationPrec.INDEXEVT; 
                MettreAJourChampEvenementSelectionne(evenement);
                RenseignerInfoConsommationPrecedente(InfoConsommationPrec);
                ScrollGrid();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Txt_IndexEvt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.dataGrid1.SelectedItem != null && Action != 2)
                {
                    //CsEvenement LeEvtSelect = (CsEvenement)this.dataGrid1.SelectedItem;
                    //if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT && LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageMaximetre)
                    //    this.Txt_ConsoCalc.Text = (string.IsNullOrEmpty(this.Txt_IndexEvt.Text) ? 0 : int.Parse(this.Txt_IndexEvt.Text)).ToString();
                    //else
                    //this.Txt_ConsoCalc.Text = ((string.IsNullOrEmpty(this.Txt_IndexEvt.Text) ? 0 : int.Parse(this.Txt_IndexEvt.Text)) - int.Parse(this.Txt_ReadingPrec.Text)).ToString();
                }
            }
            catch (Exception ex)
            {
               Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btn_supprime_Click(object sender, RoutedEventArgs e)
        {
                Action = 3;
        }

        private void dataGrid1_LoadingRow_1(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var oldRows = Rows.Where(w => w.DataContext.Equals(e.Row.DataContext)).ToList();
                foreach (var row in oldRows)
                {
                    Rows.Remove(row);
                }
                Rows.Add(e.Row);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Txt_IndexEvt_GotFocus_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeSelectedItemColor();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }  
        }

        private void Txt_ConsoSaisie_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.dataGrid1.SelectedItem != null && Action != 2)
            {
                //CsEvenement LeEvtSelect = (CsEvenement)this.dataGrid1.SelectedItem;
                //LeEvtSelect.CONSOSAISIE = (string.IsNullOrEmpty(this.Txt_Consomation.Text) ? 0 : int.Parse(this.Txt_Consomation.Text));
                //if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT && LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageMaximetre)
                //    this.Txt_ConsoCalc.Text = (string.IsNullOrEmpty(this.Txt_IndexEvt.Text) ? 0 : int.Parse(this.Txt_IndexEvt.Text)).ToString();
                //else
                //    this.Txt_ConsoCalc.Text = ((string.IsNullOrEmpty(this.Txt_IndexEvt.Text) ? 0 : int.Parse(this.Txt_IndexEvt.Text)) - int.Parse(this.Txt_ReadingPrec.Text)).ToString();
            }
        }

        private void Txt_CasIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.Txt_IndexEvt.IsReadOnly = false;
                this.Txt_Consomation.IsReadOnly = false;
                if (this.Txt_CasIndex.Text.Length == SessionObject.Enumere.TailleCas)
                {
                    if (this.Txt_CasIndex.Text == "13" || this.Txt_CasIndex.Text == "27")
                        this.txt_NumCpt.IsReadOnly = false;
                    else
                        this.txt_NumCpt.IsReadOnly = true;

                    LeCasRecherche = RetourneLibelleCas(this.Txt_CasIndex.Text);
                    if (LeCasRecherche == null)
                    {
                        Message.ShowInformation("Cas interdit", "Information");
                        EstCasValider = false;
                        return;
                    }
                    VerifieCas(false);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
        private CsCasind RetourneLibelleCas(string LeCas)
        {
            try
            {
                CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LeCas);
                if (LeCasRecherche != null)
                    return LeCasRecherche;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Txt_DateEvt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_DateEvt.Text.Length == SessionObject.Enumere.TailleDate)
                {
                    if (!ClasseMethodeGenerique.IsDateValide(this.Txt_DateEvt.Text))
                        Message.Show("Date invalide", "Error");
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }



    }
}

