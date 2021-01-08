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
using Galatee.Silverlight.ServiceFacturation ;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Galatee.Silverlight.Facturation
{
    public partial class UcSaisieIndividuellePlus : ChildWindow, INotifyPropertyChanged
    {
        List<CsEvenement> ListeEvenement = new List<CsEvenement>();
        List<CsEvenement> InfoConsommationPrec = new List<CsEvenement>();
        List<CsEvenement> ListeEvenementNonFact = new List<CsEvenement>();
        List<CsCasind> LstCas = new List<CsCasind>();
        public event PropertyChangedEventHandler PropertyChanged;
        List<CsEvenement> ListeEvenementASaisi = new List<CsEvenement>();
        List<CsEvenement> EvenemntPageCourante = new List<CsEvenement>();
        CsEvenement LeEvtSelect;
        CsCanalisation compteurSelected = new CsCanalisation();
        string lotriEncours = string.Empty;
        List<DataGridRow> Rows = new List<DataGridRow>();
        int Action = 1;   // 1 :Modification par defaut,2:creation,3:suppression
        int NombreClientLot = 0;
        int moyenneConso;
        bool passageFirst = false;
        bool IsEtatSaisie = true;
        //int NombreClientLot = 0;
        CsSaisiHorsLot horslot = new CsSaisiHorsLot();
        ObservableCollection<CsEvenement> listEvenemntCouranteDansLaGrid = new ObservableCollection<CsEvenement>();

        public UcSaisieIndividuellePlus()
        {
            InitializeComponent();
        }

        public UcSaisieIndividuellePlus(List<CsEvenement> _ListeEvenement)
        {

            try
            {
                InitializeComponent();

                ListeEvenement = _ListeEvenement;
                ChargeListeDesCas(null, null);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcSaisieIndividuellePlus(List<CsEvenement> _ListeEvenement, List<CsEvenement> Infoconso, int? avgConso)
        {

            try
            {
                InitializeComponent();
                InfoConsommationPrec = Infoconso;
                ListeEvenement = _ListeEvenement;
                moyenneConso = avgConso.Value;
                ChargeListeDesCas(null, null);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcSaisieIndividuellePlus(CsSaisiHorsLot hlot,string centre,string client,string tourne ,string produit,string periode,string lotri)
        {

            try
            {
                InitializeComponent();
                horslot = hlot;
                lotriEncours = lotri;
                InitialiserInterface(horslot, centre, client, tourne, produit, periode, lotri);
                ChargerComboPoint(horslot.Compteurs);
                ChargeListeDesCas(null, null);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void InitialiserInterface(CsSaisiHorsLot horslot, string centre, string client, string tourne, string produit,string periode,string lotri)
        {
            try
            {
                lbl_centre.Content  = centre;
                lbl_adresse.Content = client;
                lbl_typecompt.Content = horslot.Compteurs.First().LIBELLETYPECOMPTEUR;
                lbl_tournee.Content = tourne;
                txt_periode.Text = periode;
                lbl_Produit.Content = produit;
                this.Txt_DateEvt.Text = System.DateTime.Today.ToShortDateString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerComboPoint(List<CsCanalisation> list)
        {
            try
            {
                 this.Cbo_Compteur.ItemsSource = null;
                 this.Cbo_Compteur.ItemsSource = list;
                 this.Cbo_Compteur.DisplayMemberPath = "POINT";
                 if(list.Count == 1)
                    this.Cbo_Compteur.SelectedItem = list.First();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
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
                foreach (var ev in _LstEvt)
                    listEvenemntCouranteDansLaGrid.Add(ev);
                //this.Txt_DateEvt.Text = System.DateTime.Today.ToShortDateString();
                this.Txt_DateEvt.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void FillDataGrid(List<CsEvenement> _LstEvt)
        {
            try
            {
                //NombreClientLot = _LstEvt.Count;
                List<CsEvenement> evenemntasaisi = OrdonnerListeEvenement(_LstEvt.Take(SessionObject.NombreElementPageSaisiIndex).ToList());
                int counts = evenemntasaisi.Count;

                EvenemntPageCourante.AddRange(evenemntasaisi);

                foreach (var e in evenemntasaisi)
                    listEvenemntCouranteDansLaGrid.Add(e);

                for (int i = 0; i < counts; i++)
                    _LstEvt.Remove(EvenemntPageCourante[i]);

                //this.lb_Centre.Content = _ListeEvenement[0].CENTRE;
                //this.lb_Client.Content = _ListeEvenement[0].CLIENT;
                //this.lb_point.Content = _ListeEvenement[0].POINT;
                //this.lb_Produit.Content = _ListeEvenement[0].PRODUIT;
                //this.lb_Batch.Content = _ListeEvenement[0].LOTRI;
                ////this.lb_Releveur.Content = _ListeEvenement[0].FK_RELEVEUR;


                //ListeEvenementNonFact = _ListeEvenement.Where(p => (p.STATUS != SessionObject.Enumere.EvenementFacture && p.STATUS != SessionObject.Enumere.EvenementMisAJour && p.STATUS != SessionObject.Enumere.EvenementSupprimer && p.STATUS != SessionObject.Enumere.EvenementPurger)).ToList();
                //foreach (CsEvenement item in ListeEvenementNonFact)
                //{
                //    if (_ListeEvenement.FirstOrDefault(p => p.EVENEMENT == item.EVENEMENT) != null)
                //        _ListeEvenement.Remove(item);
                //}

                //dataGrid1.ItemsSource = null;
                //dataGrid1.ItemsSource = ListeEvenementNonFact;

                //if (ListeEvenementNonFact.Count != 0)
                //    dataGrid1.SelectedItem = ListeEvenementNonFact[0];

                //this.Txt_periodePrec.Text = ListeEvenementNonFact[0].PERIODEPRECEDENT == null ? string.Empty : ListeEvenementNonFact[0].PERIODEPRECEDENT;
                //this.Txt_ReadingPrec.Text = ListeEvenementNonFact[0].INDEXEVTPRECEDENT == null ? string.Empty : ListeEvenementNonFact[0].INDEXEVTPRECEDENT.ToString();

                //this.Txt_DateEvt.Text = ListeEvenementNonFact[0].DATEEVT == null ?string.Empty: ListeEvenementNonFact[0].DATEEVT.Value.Date.ToString();
                //this.Txt_periode.Text = ListeEvenementNonFact[0].PERIODE == null ? string.Empty : ListeEvenementNonFact[0].PERIODE;
                //this.Txt_Diametre.Text = ListeEvenementNonFact[0].DIAMETRE == null ? string.Empty : ListeEvenementNonFact[0].DIAMETRE;

                //this.Txt_Compteur.Text = ListeEvenementNonFact[0].COMPTEUR == null ? string.Empty : ListeEvenementNonFact[0].COMPTEUR;
                //this.Txt_IndexEvt.Text = ListeEvenementNonFact[0].INDEXEVT == null ? string.Empty : ListeEvenementNonFact[0].INDEXEVT.ToString();
                //this.Txt_Cas.Text = ListeEvenementNonFact[0].CAS == null ? string.Empty : ListeEvenementNonFact[0].CAS;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void UpdateDataGrid(List<CsEvenement> _ListeEvenement)
        {
            try
            {
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
                //this.Txt_periodePrec.Text = string.Empty;
                this.Txt_IndexEvt.Text = string.Empty;
                this.Txt_Cas.Text = string.Empty;
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        private void ChargeListeDesCas(string LeCentre, string LeCas)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));

                service.RetourneListeDesCasCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("", "");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowError("", "");
                            return;
                        }

                        LstCas = args.Result;
                    }
                    catch (Exception ex)
                    {
                      Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.RetourneListeDesCasAsync(LeCentre, LeCas);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
              
            }
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Txt_Cas.Text)) this.Txt_Cas.Text = "00";

                LeEvtSelect.CAS = this.Txt_Cas.Text;
                LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                    LeEvtSelect.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);

                LeEvtSelect.CONSO = string.IsNullOrEmpty(this.Txt_ConsoSaisie.Text) ? LeEvtSelect.INDEXEVT - LeEvtSelect.INDEXPRECEDENTEFACTURE : int.Parse(this.Txt_ConsoSaisie.Text);
                //LeEvtSelect.CONSO = string.IsNullOrEmpty(this.Txt_ConsoCalc.Text) ? LeEvtSelect.INDEXEVT - LeEvtSelect.INDEXEVTPRECEDENT : int.Parse(this.Txt_ConsoCalc.Text);
                LeEvtSelect.IsSaisi = true;
                LeEvtSelect.STATUS = SessionObject.Enumere.EvenementReleve;
                LeEvtSelect.INDEXPRECEDENTEFACTURE = LeEvtSelect.INDEXPRECEDENTEFACTURE;
                //LeEvtSelect.NUMEVENEMENT = InfoConsommationPrec.First().NUMEVENEMENT;
                LeEvtSelect.CONSOMOYENNEPRECEDENTEFACTURE = LeEvtSelect.CONSOMOYENNEPRECEDENTEFACTURE;
                LeEvtSelect.STATUSPAGERIE = 0;
                LeEvtSelect.MATRICULE = UserConnecte.matricule;
                LeEvtSelect.DATECREATION = DateTime.Now.Date;
                LeEvtSelect.USERCREATION = UserConnecte.matricule;
                LeEvtSelect.LOTRI = lotriEncours;
                LeEvtSelect.FK_IDCANALISATION = compteurSelected.PK_ID;
                LeEvtSelect.FK_IDPRODUIT = compteurSelected.FK_IDPRODUIT;
                LeEvtSelect.FK_IDCENTRE = compteurSelected.FK_IDCENTRE;
                LeEvtSelect.PERIODE = txt_periode.Text;
                IsSaisieValider(LeEvtSelect, LstCas);
            }
            catch (Exception ex)
            {
               Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
            //this.DialogResult = true;
        }

        void IsSaisieValider(CsEvenement LaSaisie, List<CsCasind> LstCas)
        {
            LeEvtSelect = new CsEvenement();
            LeEvtSelect = LaSaisie;

            CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LaSaisie.CAS);
            if (LeCasRecherche == null)
            {
                Message.ShowInformation("Cas interdit", "Erreur");
                IsEtatSaisie = false;
                return;
            }
            else
            {
                // Saisie d'index
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeObligatoire)
                {
                    if (string.IsNullOrEmpty(LaSaisie.INDEXEVT.ToString()))
                    {
                        Message.ShowInformation("La saisie de l'index est obligatoire", "Alert");
                        IsEtatSaisie = false;
                        return;

                    }
                }
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                {
                    if (!string.IsNullOrEmpty(LaSaisie.INDEXEVT.ToString()))
                    {
                        Message.ShowInformation("La saisie de l'index est Interdite", "Alert");
                        IsEtatSaisie = false;
                        return;

                    }
                }
                //
                // Saisie de la consomation
                if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeObligatoire)
                {
                    if (string.IsNullOrEmpty(LaSaisie.CONSO.ToString()))
                    {
                        Message.ShowInformation("La saisie de la consommation est obligatoire", "Alert");
                        IsEtatSaisie = false;
                        return;
                    }
                }
                else if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeInterdit)
                {
                    if (!string.IsNullOrEmpty(LaSaisie.CONSO.ToString()))
                    {
                        Message.ShowInformation("La saisie de la consommation est Interdit", "Alert");
                        IsEtatSaisie = false;
                        return;
                    }
                }


            }
            IsCasValider(LaSaisie);
        }

        void IsCasValider(CsEvenement LaSaise)
        {
            if (LaSaise.INDEXEVT < LaSaise.INDEXPRECEDENTEFACTURE)
            {
                var ws = new MessageBoxControl.MessageBoxChildWindow("Information", "Retour à zéro !", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Warning);
                ws.OnMessageBoxClosed += (l, results) =>
                {
                    if (ws.Result == MessageBoxResult.No)
                    {
                        LeEvtSelect.CAS = string.Empty;
                        LeEvtSelect.INDEXEVT = null;
                        LeEvtSelect.CONSO = null;
                        LeEvtSelect.IsSaisi = false;
                    }
                    else if (ws.Result == MessageBoxResult.OK)
                    {
                        LaSaise.CAS = "04";
                        ValiderSaisi();
                    }
                };
                ws.Show();
                return;
            }
            if (LaSaise.CAS == "00" && LaSaise.CONSOMOYENNEPRECEDENTEFACTURE > LaSaise.CONSO)
            {
                var ws = new MessageBoxControl.MessageBoxChildWindow("Information", "Consumsion too low", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                ws.OnMessageBoxClosed += (l, results) =>
                {
                    if (ws.Result == MessageBoxResult.No)
                    {
                        LeEvtSelect.CAS = string.Empty;
                        LeEvtSelect.INDEXEVT = null;
                        LeEvtSelect.CONSO = null;
                        LeEvtSelect.IsSaisi = false;
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
            if (LaSaise.CAS != "00")
            {
                var ws = new MessageBoxControl.MessageBoxChildWindow("Information", "Confirmer vous la saisie", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                ws.OnMessageBoxClosed += (l, results) =>
                {
                    if (ws.Result == MessageBoxResult.No)
                    {
                        LeEvtSelect.CAS = string.Empty;
                        LeEvtSelect.INDEXEVT = null;
                        LeEvtSelect.CONSO = null;
                        LeEvtSelect.IsSaisi = false;
                    }
                    else if (ws.Result == MessageBoxResult.OK)
                    {
                        ValiderSaisi();
                    }
                };
                ws.Show();
                return;
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

        private void InsertPageriEvenement(CsEvenement _ListEvenement)
        {
            int result = LoadingManager.BeginLoading("Insertion en cours...");

            try
            {
                btn_Valider.IsEnabled = false;
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));

                service.CreationClientIndexHorsLotCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            btn_Valider.IsEnabled = true;
                            LoadingManager.EndLoading(result);
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur d'insertion des informations du client hors lot.", "Erreur");
                            btn_Valider.IsEnabled = true;
                            LoadingManager.EndLoading(result);
                            return;
                        }

                        Message.ShowInformation("Insertion des index effectuée avec succès", "Erreur");

                        if (horslot.Compteurs.Count == 1)
                        {
                            this.Close();
                            LoadingManager.EndLoading(result);
                        }
                        else if (horslot.Compteurs.Count > 1)
                        {
                            Cbo_Compteur.SelectedItem = horslot.Compteurs[1];
                            btn_Valider.IsEnabled = true;
                            LoadingManager.EndLoading(result);
                        }

                      
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                        btn_Valider.IsEnabled = true;
                        LoadingManager.EndLoading(result);
                    }
                };
                service.CreationClientIndexHorsLotAsync(_ListEvenement);
                //}
                //else
                //    UpdateDataGrid(ListeEvenement, IndexASelectionner);

            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(result);
                throw ex;
            }
        }

        private void RemoveElementGridApresInsert(CsEvenement LeEvtSelect)
        {
            try
            {
                CsEvenement index = listEvenemntCouranteDansLaGrid.FirstOrDefault(f => f.CENTRE == LeEvtSelect.CENTRE && f.CLIENT == LeEvtSelect.CLIENT && f.ORDRE == LeEvtSelect.ORDRE && f.PRODUIT == LeEvtSelect.PRODUIT && f.POINT == LeEvtSelect.POINT && f.NUMEVENEMENT == LeEvtSelect.NUMEVENEMENT);
                listEvenemntCouranteDansLaGrid.Remove(index);
                ChangeSelectedItemColor();
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


        private void ReinitialiserGrid()
        {
            try
            {
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
                allowProgressBar();
                InsertPageriEvenement(LeEvtSelect);
                //MiseAjourSaisie(LeEvtSelect);
                ReinitialiserGrid();
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
        //                if (new Galatee.Silverlight.Index.ClasseMethodeGenerique().IsSaisieValider(_LeEvenementCree, LstCas))
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
        //                _LeEvenementRecherche.PERIODE = this.txt_periode.Text;
        //                _LeEvenementRecherche.CONSO = !string.IsNullOrEmpty(this.Txt_ConsoSaisie.Text) ? int.Parse(this.Txt_ConsoSaisie.Text) : _LeEvenementRecherche.INDEXEVT - _LeEvenementRecherche.INDEXEVTPRECEDENT;
        //                if (new Galatee.Silverlight.Index.ClasseMethodeGenerique().IsSaisieValider(_LeEvenementRecherche, LstCas))
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

        //private void ActionEvenement(CsEvenement _LeEvenement, int _Action)
        //{
        //    try
        //    {
        //        //InsererUnEvenement
        //        if (_Action == 2)
        //        {
        //            IndexServiceClient service = new IndexServiceClient(Utility.Protocole(), Utility.EndPoint("Index"));
        //            service.InsererUnEvenementCompleted += (s, args) =>
        //            {
        //                if (args != null && args.Cancelled)
        //                {
        //                    Message.ShowError("", "");
        //                    return;
        //                }

        //                if (args.Result == null )
        //                {
        //                    Message.ShowError("", "");
        //                    return;
        //                }
        //            };
        //            service.InsererUnEvenementAsync(_LeEvenement);
                   
        //        }
        //        else if (_Action == 1 || _Action == 3)
        //        {
        //            IndexServiceClient service = new IndexServiceClient(Utility.Protocole(), Utility.EndPoint("Index"));
        //            service.UpdateUnEvenementCompleted += (s, args) =>
        //            {
        //                try
        //                {
        //                    if (args != null && args.Cancelled)
        //                    {
        //                        Message.ShowError("", "");
        //                        return;
        //                    }

        //                    //if (args.Result == null)
        //                    //{
        //                    //    Message.ShowError("", "");
        //                    //    return;
        //                    //}
        //                }
        //                catch (Exception ex)
        //                {
        //                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                }
        //            };
        //            service.UpdateUnEvenementAsync(_LeEvenement);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
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
                       NUMEVENEMENT = _LeEvt.NUMEVENEMENT,
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
                       //FACTTOT = _LeEvt.FACTTOT,
                       //DMAJ = _LeEvt.DMAJ,
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
                //RenseignerInfoConsommationPrecedente(InfoConsommationPrec);
                FillDataGridView(ListeEvenement);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }

        }

        private void RenseignerInfoConsommationPrecedente(List<CsEvenement> InfoConsommationPrec)
        {
            try
            {
                CsEvenement evenement = InfoConsommationPrec.FirstOrDefault();

                this.Txt_CasLibellePrec.Text = evenement.LIBELLECASPRECEDENT;
                this.Txt_ReadingPrec.Text = evenement.INDEXPRECEDENTEFACTURE == null ? string.Empty : evenement.INDEXPRECEDENTEFACTURE.ToString();
                this.Txt_periodePrec.Text = evenement.PERIODEPRECEDENT;
                this.txt_coefPrec.Text  = evenement.COEFLECT == null ? string.Empty : evenement.COEFLECT.ToString();
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
                this.Txt_DateEvt.Text = evenement.DATEEVT == null ? string.Empty: evenement.DATEEVT.ToString();
                this.Txt_IndexEvt.Text = evenement.INDEXEVT == null ? string.Empty: evenement.INDEXEVT.ToString();
                //this.Txt_ConsoCalc.Text = evenement.CONSO == null ? string.Empty:evenement.CONSO.ToString();
                this.Txt_ConsoSaisie.Text = evenement.CONSO == null? string.Empty: evenement.CONSO.ToString();
                this.Txt_Cas.Text = evenement.CASEVENEMENT == null ? string.Empty : evenement.CASEVENEMENT;
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
                //int indexPrecedent = lvwResultat.SelectedIndex - 1;
                if (passageFirst)
                {
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
                Txt_IndexEvt.Focus();
                //MettreAJourChampEvenementSelectionne(evenement);
                ScrollGrid();

                //CsEvenement evenement = dataGrid1.SelectedItem as CsEvenement;
                //if (evenement != null)
                //    MettreAJourChampEvenementSelectionne(evenement);

                //CsEvenement LeEvtSelect = (CsEvenement)this.dataGrid1.SelectedItem;
                //if (LeEvtSelect != null)
                //{
                //    this.Txt_ReadingPrec.Text = LeEvtSelect.INDEXEVTPRECEDENT.ToString();
                //    this.Txt_periodePrec.Text = LeEvtSelect.PERIODEPRECEDENT;

                //    CsCasind LeCasCourant = LstCas.FirstOrDefault(p => p.PK_CAS == LeEvtSelect.CAS);
                //    if (LeCasCourant != null)
                //        this.Txt_CasLibelle.Text = string.IsNullOrEmpty(LeCasCourant.LIBELLE) ? string.Empty : LeCasCourant.LIBELLE;
                //    //
                //    this.Txt_DateEvt.Text = LeEvtSelect.DATEEVT.ToString();
                //    this.Txt_periode.Text = LeEvtSelect.PERIODE.ToString();
                //    this.Txt_Cas.Text = LeEvtSelect.CASPRECEDENT == null ? string.Empty : LeEvtSelect.CASPRECEDENT;
                //    this.Txt_Diametre.Text = LeEvtSelect.DIAMETRE == null ? string.Empty : LeEvtSelect.DIAMETRE;
                //    this.Txt_Compteur.Text = LeEvtSelect.COMPTEUR == null ? string.Empty : LeEvtSelect.COMPTEUR;
                //    this.Txt_IndexEvt.Text = LeEvtSelect.INDEXEVT == null ? string.Empty : LeEvtSelect.INDEXEVT.ToString();
                //    this.Txt_ConsoCalc.Text = LeEvtSelect.CONSO == null ? string.Empty : LeEvtSelect.CONSO.ToString();
                //    this.Txt_ConsoSaisie.Text = LeEvtSelect.CONSOSAISIE == null ? string.Empty : LeEvtSelect.CONSOSAISIE.ToString();
                //    this.Txt_Cas.Text = LeEvtSelect.CAS == null ? string.Empty : LeEvtSelect.CAS;
                //    this.Txt_Enquete.Text = LeEvtSelect.ENQUETE == null ? string.Empty : LeEvtSelect.ENQUETE;
                //}
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
                //ChangeSelectedItemColor();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }  
        }

        private void Cbo_Compteur_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsCanalisation compteur = Cbo_Compteur.SelectedItem as CsCanalisation ;
                compteurSelected = compteur;
                if(compteur == null)
                    return ;
                
                CsSaisiIndexIndividuel prec = horslot.IndexInfo.FirstOrDefault(i => i.POINT == compteur.POINT);
                LeEvtSelect = prec.ConsoPrecedent.First();
                RenseignerInfoConsommationPrecedente(prec.ConsoPrecedent);
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, "Erreur");
            }
        }

        private void ChargerDonneeCompteurSelectionne(CsCanalisation compteur)
        {
            try
            {
                CsSaisiIndexIndividuel prec = horslot.IndexInfo.FirstOrDefault(i => i.POINT == compteur.POINT);

                // renseigner les infos de l'évenement précédent

                Txt_periodePrec.Text = prec.ConsoPrecedent.First().PERIODE;
                Txt_ReadingPrec.Text = prec.ConsoPrecedent.First().INDEXEVT == null ? string.Empty:prec.ConsoPrecedent.First().INDEXEVT.ToString();
                Txt_CasLibellePrec.Text = prec.ConsoPrecedent.First().LIBELLECASPRECEDENT;
                txt_coefPrec.Text = prec.ConsoPrecedent.First().COEFLECT == null ?string.Empty: prec.ConsoPrecedent.First().COEFLECT.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

