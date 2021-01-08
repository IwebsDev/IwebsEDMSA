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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Resources.Index;


namespace Galatee.Silverlight.Facturation
{
    public partial class UcSaisieParPage : ChildWindow,INotifyPropertyChanged
    {
        public UcSaisieParPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        List<CsEvenement> ListeEvenementASaisi = new List<CsEvenement>();
        List<CsEvenement> ListeEvenementSaisi = new List<CsEvenement>();
        List<CsEvenement> ListeEvenementTotal = new List<CsEvenement>();
        List<CsLotri> ListeLot = new List<CsLotri>();
        CsEvenement  LeEvenement; 
        List<CsCasind> LstCas = new List<CsCasind>();
        CsEvenement LeEvtSelect;
        List<DataGridRow> Rows = new List<DataGridRow>();
        int NombreClientLot = 0;
        int NombreClientSaisie = 0;
        int NombreEvtSaisiClient = 0;
        bool passageFirst = false;
        bool IsEtatSaisie = true;
        ObservableCollection<CsEvenement> listEvenemntCouranteDansLaGrid = new ObservableCollection<CsEvenement>();

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
                return;
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

        public UcSaisieParPage(List<CsLotri> lstLot,List<CsEvenement> _ListeEvenement)
        {
            InitializeComponent();
            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtClient.IsEnabled = false;
            this.Btn_Recherche.IsEnabled = false;
            try
            {
                ListeLot = lstLot;
                ListeEvenementASaisi = _ListeEvenement.Where(t => string.IsNullOrEmpty(t.CAS)).ToList();
                ListeEvenementSaisi = _ListeEvenement.Where(t => !string.IsNullOrEmpty(t.CAS)).ToList();
                ListeEvenementTotal = _ListeEvenement;
                this.Txt_CasIndex.MaxLength = SessionObject.Enumere.TailleCas;
                NombreClientLot = _ListeEvenement.Count;
                 NombreClientSaisie = ListeEvenementSaisi.Count();
                this.txt_RapportSaisi.Text = NombreClientSaisie.ToString() + "/" + NombreClientLot.ToString();
                if (_ListeEvenement.First().PRODUIT ==SessionObject.Enumere.ElectriciteMT)
                {
                    if (ListeEvenementASaisi != null && ListeEvenementASaisi.Count != 0)
                    {
                        if (ListeEvenementASaisi.First().ORDREAFFICHAGE != 1)
                        {
                            NombreEvtSaisiClient = Convert.ToInt16(ListeEvenementASaisi.First().ORDREAFFICHAGE - 1);
                            ListeEvenementASaisi.AddRange(ListeEvenementSaisi.Where(t => t.FK_IDABON == ListeEvenementASaisi.First().FK_IDABON && t.ORDREAFFICHAGE < ListeEvenementASaisi.First().ORDREAFFICHAGE).OrderBy(t => t.ORDTOUR).ThenBy(u => u.CENTRE).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList());
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                Message.ShowError(EX, "Erreur");
            }
        }

        private void ScrollGrid()
        {
            try
            {
                if (passageFirst)
                {
                    dataGrid1.IsReadOnly  = false ;
                    dataGrid1.ScrollIntoView(dataGrid1.SelectedItem, dataGrid1.Columns[1]);
                    dataGrid1.UpdateLayout();
                    dataGrid1.IsReadOnly = true ;


                }
                else
                    passageFirst = true;


             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Txt_DateEvt.Text))
                {
                    Message.ShowInformation("Veuillez saisir la date", "Information");
                    return;
                }
                CsEvenement events = (CsEvenement)this.dataGrid1.SelectedItem;
                if (events.DATERELEVEPRECEDENTEFACTURE > Convert.ToDateTime(this.Txt_DateEvt.Text ))
                {
                    Message.ShowWarning ("La date de dernière relève est supérieure à la date saisie", "Attention");
                    return;
                }

                if (IsSupprimer)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Index", "Voulez-vous supprimer l'évènement ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.OK)
                        {
                            List<CsEvenement> LeEvtSelectClient = listEvenemntCouranteDansLaGrid.Where(ev => ev.CENTRE == events.CENTRE &&
                                                                                     ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                                     ev.PRODUIT == events.PRODUIT).ToList();
                            LeEvtSelectClient.ForEach(t => t.STATUS = SessionObject.Enumere.EvenementSupprimer);
                            LeEvtSelectClient.ForEach(t => t.MATRICULE = UserConnecte.matricule);
                            UpdateEvenementListe(LeEvtSelectClient);
                            return;
                        }
                        else
                            return;
                    };
                    ws.Show();
                   return;
                }
               
                if (string.IsNullOrEmpty(Txt_IndexEvt.Text) && string.IsNullOrEmpty(Txt_LibelleCas.Text) && string.IsNullOrEmpty(Txt_CasIndex.Text) && string.IsNullOrEmpty(Txt_Consomation.Text))
                {
                    Message.ShowInformation("Aucune donnée saisie. Veuillez renseigner les données de relevé.", "Attention");
                    return;
                }
                if (!string.IsNullOrEmpty(Txt_DateEvt.Text) && Convert.ToDateTime( Txt_DateEvt.Text)> System.DateTime .Today )
                {
                    Message.ShowInformation("La date du relevé doit être inférieure ou égale à la date du jour.", "Attention");
                    return;
                }
                dataGrid1.IsEnabled = true;
                LeEvtSelect.CAS = this.Txt_CasIndex.Text;
                if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text) && int.Parse(this.Txt_IndexEvt.Text) == LeEvtSelect.INDEXPRECEDENTEFACTURE && 
                    (string.IsNullOrEmpty( Txt_CasIndex.Text ) || Txt_CasIndex.Text == "00"))
                {
                    LeEvtSelect.CAS = "86";
                    LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                    LeEvtSelect.STATUS = SessionObject.Enumere.EvenementReleve;
                    LeEvtSelect.MATRICULE = UserConnecte.matricule;

                    CsCasind leCasSaisi = LstCas.FirstOrDefault(t => t.CODE == LeEvtSelect.CAS);
                    if (leCasSaisi != null)
                        LeEvtSelect.ENQUETE = leCasSaisi.ENQUETABLE ? "E" : string.Empty;
                    ValidationSaisieIndex(LeEvtSelect);
                    return;
                }
                if (string.IsNullOrEmpty(this.Txt_CasIndex.Text))
                {
                    LeCasRecherche = LstCas.FirstOrDefault(t => t.CODE == "00");
                    LeEvtSelect.CAS = "00";
                }
                LeEvtSelect = listEvenemntCouranteDansLaGrid.First(ev => ev.CENTRE == events.CENTRE &&
                                                                  ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                  ev.PRODUIT == events.PRODUIT && ev.POINT == events.POINT);

                LeEvtSelect.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                LeEvtSelect.STATUS = SessionObject.Enumere.EvenementReleve;
                LeEvtSelect.STATUSPAGERIE = 0;
                LeEvtSelect.MATRICULE = UserConnecte.matricule;
                LeEvtSelect.NOUVEAUCADRAN = null;
                if (!string.IsNullOrEmpty(this.Txt_NbrRoue.Text))
                LeEvtSelect.NOUVEAUCADRAN =  byte.Parse(this.Txt_NbrRoue.Text);
                
                LeEvtSelect.IsFromPageri = true;
                VerifieCas(true );
                if (!EstCasValider)
                    return;
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                    LeEvtSelect.INDEXEVT = LeEvtSelect.INDEXPRECEDENTEFACTURE;

                if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                  List <CsEvenement>  LeEvtSelectClient = listEvenemntCouranteDansLaGrid.Where (ev => ev.CENTRE == events.CENTRE &&
                                                                 ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                                 ev.PRODUIT == events.PRODUIT ).ToList();
                    string CasMin = "50";
                    bool validerUpdate = true;
                  //if ((LeEvtSelect.CAS != "10") &&
                  //    (LeEvtSelect.CAS != "00") &&
                  //   int.Parse(LeEvtSelect.CAS) <= int.Parse(CasMin))
                  //{
                    if ((LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit )  &&
                       int.Parse(LeEvtSelect.CAS) <= int.Parse(CasMin))
                    {
                      string leCas = this.Txt_CasIndex.Text;
                      List<CsEvenement> lstEvt = new List<CsEvenement>();
                      foreach (CsEvenement item in LeEvtSelectClient)
                      {
                          item.CAS = leCas;
                          item.INDEXEVT = item.INDEXPRECEDENTEFACTURE;
                          if (LeCasRecherche.SAISIEINDEX != SessionObject.Enumere.CodeObligatoire &&
                              LeCasRecherche.SAISIECOMPTEUR != SessionObject.Enumere.CodeObligatoire &&
                              LeCasRecherche.SAISIECONSO != SessionObject.Enumere.CodeObligatoire)
                          {

                              item.DATEEVT = Convert.ToDateTime(this.Txt_DateEvt.Text);
                              item.IsSaisi = true;
                              item.STATUS = SessionObject.Enumere.EvenementReleve;
                              item.STATUSPAGERIE = 0;
                              item.MATRICULE = UserConnecte.matricule;
                              item.IsFromPageri = true;
                              item.CONSO = 0;
                              lstEvt.Add(item);
                          }
                          else
                              validerUpdate = false;
                      }
                      if (validerUpdate)
                      {
                          UpdateEvenementListe(lstEvt);
                          return;
                      }
                  }
                }

                if (LeEvtSelect.CAS == "13" || LeEvtSelect.CAS == "27")
                {
                    if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        List<CsEvenement> LeEvtSelectClient = listEvenemntCouranteDansLaGrid.Where(ev => ev.CENTRE == events.CENTRE &&
                                                 ev.CLIENT == events.CLIENT && ev.ORDRE == events.ORDRE &&
                                                 ev.PRODUIT == events.PRODUIT).ToList();

                        LeEvtSelectClient.ForEach(t => t.COMPTEURAFFICHER = (t.COMPTEUR.Substring(0, 5) + this.txt_NumCpt.Text));
                        LeEvtSelectClient.ForEach(t => t.NOUVCOMPTEUR = (t.COMPTEUR.Substring(0, 5) + this.txt_NumCpt.Text));
                    }
                    else
                    {
                        LeEvtSelect.COMPTEURAFFICHER = this.txt_NumCpt.Text;
                        LeEvtSelect.NOUVCOMPTEUR = this.txt_NumCpt.Text;
                    }
                }
                else
                    this.txt_NumCpt.Text = string.Empty;

                if (LeEvtSelect.CAS != "13" && LeEvtSelect.CAS != "27")
                    ValidationSaisieIndex(LeEvtSelect);
                else
                    IsSaisieValider(LeEvtSelect, LstCas);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        CsCanalisation CanalisationClient(List<CsEvenement> events, string centre, string client, string ordre, int point)
        {
            try
            {
                CsEvenement ev = events.First(e => e.CENTRE == centre && e.CLIENT == client && e.ORDRE == ordre &&
                                                    e.POINT == point);
                CsCanalisation c = new CsCanalisation() {
                               TYPECOMPTEUR = ev.TYPECOMPTEUR,
                               REGLAGECOMPTEUR = ev.REGLAGECOMPTEUR
                };

                return c;
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
                    //Txt_IndexEvt.Text = string.Empty;
                    //this.Txt_CasIndex.Text = string.Empty;
                    this.Txt_Consomation.Text = string.Empty;
                    dataGrid1.IsReadOnly  = false;
                }
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
                listEvenemntCouranteDansLaGrid.Clear();
                foreach (var ev in _LstEvt)
                    listEvenemntCouranteDansLaGrid.Add(ev);
                if (listEvenemntCouranteDansLaGrid != null && listEvenemntCouranteDansLaGrid.Count != 0)
                {
                    dataGrid1.ItemsSource = listEvenemntCouranteDansLaGrid;
                    if (listEvenemntCouranteDansLaGrid.FirstOrDefault(t => t.CAS != null && string.IsNullOrEmpty(t.CAS)) != null)
                        dataGrid1.SelectedItem = listEvenemntCouranteDansLaGrid.First(t => string.IsNullOrEmpty(t.CAS));
                    else
                        dataGrid1.SelectedItem = listEvenemntCouranteDansLaGrid.First();
                    if (IsMiseJour)
                        this.Txt_DateEvt.Text = dataGrid1.SelectedItem != null && ((CsEvenement)dataGrid1.SelectedItem ).DATEEVT != null ?((CsEvenement)dataGrid1.SelectedItem ).DATEEVT .Value.ToShortDateString():string.Empty  ;
                    this.Txt_DateEvt.Focus();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError ("Erreur au remplissage de la grille", "FillDataGridView");
            }

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
                    LaSaise.STATUS = SessionObject.Enumere.EvenementSupprimer;
                    if (LeEvt != null)
                    {
                        LaSaise.INDEXPRECEDENTEFACTURE = LeEvt.INDEXEVT;
                        LaSaise.CONSO = LeEvt.INDEXEVT - LaSaise.INDEXEVT;
                        LaSaise.FK_IDCANALISATION = LeEvt.FK_IDCANALISATION;
                        LaSaise.NUMEVENEMENT = LeEvt.NUMEVENEMENT + 1;
                        LaSaise.TYPECOMPTEUR = LeEvt.TYPECOMPTEUR ;
                        LaSaise.DERPERF = LeEvt.DERPERF;
                        LaSaise.DERPERFNPREC = LeEvt.DERPERFNPREC;
                        LaSaise.INDEXPRECEDENTEFACTURE = LeEvt.INDEXPRECEDENTEFACTURE;
                        LaSaise.DATERELEVEPRECEDENTEFACTURE = LeEvt.DATERELEVEPRECEDENTEFACTURE;
                        LaSaise.CASPRECEDENTEFACTURE = LeEvt.CASPRECEDENTEFACTURE;
                        LaSaise.PERIODEPRECEDENTEFACTURE = LeEvt.PERIODEPRECEDENTEFACTURE;
                        LaSaise.NOUVCOMPTEUR = this.txt_NumCpt.Text; ;
                        LaSaise.ISEVTPOSETROUVE = true  ;
                        ValiderSaisi();
                    }
                    else
                    {
                        LaSaise.NOUVCOMPTEUR = this.txt_NumCpt.Text;
                        Message.ShowWarning("Les données de changement du compteur(" + LaSaise.COMPTEUR + ") par le ( " + LaSaise.NOUVCOMPTEUR + " ) n'ont pas été saisies" + "\n\r" + " Cet index ne sera pas pris en compte pour cette facturation", "Facturation");
                        if (!string.IsNullOrEmpty(this.txt_NumCpt.Text))
                        {
                            this.txt_NumCpt.Text = string.Empty;
                            this.txt_NumCpt.IsReadOnly = true;
                        }

                        LaSaise.CONSO  =null;
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

        private void RetourneAZero(CsEvenement  LaSaise,string leCasAMettre)
        {
            try
            {
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
                    string leCadran = (string.IsNullOrEmpty(this.Txt_NbrRoue.Text )) ? LaSaise.CADRAN.ToString() : this.Txt_NbrRoue.Text;
                    int Roue = LaSaise.CADRAN == null ? 1 : int.Parse(leCadran);
                    int initval = 9;
                    int? Indexmax = int.Parse(initval.ToString().PadLeft(Roue, '9'));
                    LeEvtSelect.CONSO = ((Indexmax - LaSaise.INDEXPRECEDENTEFACTURE) + (int.Parse(this.Txt_IndexEvt.Text) + 1));
                    LeEvtSelect.ENQUETE = LeCasRecherche.ENQUETABLE ? "E" : string.Empty;
                }
                ValiderSaisi();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargeListeDesCas(null, null);
                if (ListeEvenementASaisi != null && ListeEvenementASaisi.Count != 0)
                {
                    this.Txt_Releveur.Text = ListeEvenementASaisi.First().RELEVEUR == null ? string.Empty : ListeEvenementASaisi.First().RELEVEUR.ToString(); ;
                    this.Txt_batchNum.Text = ListeEvenementASaisi.First().LOTRI;
                    this.Txt_Centre.Text = ListeEvenementASaisi.First().CENTRE;
                    this.Txt_Period.Text = ListeEvenementASaisi.First().PERIODE;
                    CsEvenement leEvtCourant = ListeEvenementASaisi.First();
                    if (leEvtCourant.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        FillDataGridView(ListeEvenementTotal.Where(t => t.FK_IDABON == leEvtCourant.FK_IDABON).ToList());
                    else
                    {
                        if (ListeEvenementASaisi != null && ListeEvenementASaisi.Count != 0)
                            FillDataGridView(ListeEvenementASaisi);
                    }

                }
                else if (ListeEvenementTotal != null && ListeEvenementTotal.Count != 0)
                {
                    this.Txt_Releveur.Text = ListeEvenementTotal.First().RELEVEUR == null ? string.Empty : ListeEvenementASaisi.First().RELEVEUR.ToString(); ;
                    this.Txt_batchNum.Text = ListeEvenementTotal.First().LOTRI;
                    this.Txt_Centre.Text = ListeEvenementTotal.First().CENTRE;
                    this.Txt_Period.Text = ListeEvenementTotal.First().PERIODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
	            {
                    //to get the current row binding value
                    CsEvenement evenement = dataGrid1.SelectedItem as CsEvenement;
                    if (evenement == null)
                        return;

                    LeEvtSelect = listEvenemntCouranteDansLaGrid.First(ev => ev.CENTRE == evenement.CENTRE &&
                                                                     ev.CLIENT == evenement.CLIENT && ev.ORDRE == evenement.ORDRE &&
                                                                     ev.PRODUIT == evenement.PRODUIT && ev.POINT == evenement.POINT);

                    Txt_IndexEvt.Focus();

                    MettreAJourChampEvenementSelectionne(evenement);
                    ScrollGrid();

	            }
	            catch (Exception ex)
	            {
                    Message.ShowError(ex, "Erreur");
		        }
            
        }

        private void ChargeListeDesCas(string LeCentre,string LeCas)
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
                            Message.ShowError("La liste des cas retournée est vide.", "Erreur");
                            return;
                        }

                        LstCas = args.Result;
                    }
                    catch (Exception ex)
                    {
                      Message.ShowError(ex, "Erreur");
                    }
                };
                service.RetourneListeDesCasAsync(LeCentre, LeCas);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private CsCasind  RetourneLibelleCas(string LeCas)
        {
            try
            {
                CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LeCas);
                if (LeCasRecherche != null)
                    return LeCasRecherche;
                else
                    return  null ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertEvenementListe(List<CsEvenement> _ListEvenement)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.InsererLstEvenementCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur de mise à jour du lot.", "Erreur");
                            return;
                        }
                        if (_ListEvenement.First().ISAJOUTLOT == true)
                        {
                          List<CsEvenement> lesEvtSupprime =   ListeEvenementTotal.Where(p => p.LOTRI == _ListEvenement.First().LOTRI && p.FK_IDABON == _ListEvenement.First().FK_IDABON).ToList();
                            foreach (CsEvenement item in lesEvtSupprime)
	                            {
		                          ListeEvenementSaisi.Add (item);
                                  ListeEvenementASaisi.Remove (item);
	                            }
                        }
                        NombreClientSaisie = NombreClientSaisie + 6;
                        this.txt_RapportSaisi.Text = NombreClientSaisie.ToString() + "/" + NombreClientLot.ToString();
                        List<CsEvenement> lstNonSaisie = listEvenemntCouranteDansLaGrid.Where(f => string.IsNullOrEmpty(f.CAS)).ToList();
                        RemoveElementGridApresInsert();


                        this.Txt_LibelleCas.Text = string.Empty;
                        this.Txt_CasIndex.Text = string.Empty;
                        this.Txt_IndexEvt.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.InsererLstEvenementAsync(_ListEvenement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateEvenementListe(List<CsEvenement> _ListEvenement)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.MisAJourEvenementIndexCompleted  += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur de mise à jour du lot.", "Erreur");
                            return;
                        }
                        if (IsSupprimer )
                        {
                            Btn_ContinuerSaisie_Click(null, null);
                            IsSupprimer = false;
                            RemoveElementGridApresInsert();
                            this.txtClient.Text = string.Empty;
                            this.Rd_Supprimer.IsChecked = false;
                            return;
                        }
                        if (!IsMiseJour && !IsSupprimer)
                            NombreClientSaisie = NombreClientSaisie + 6;

                        this.txt_RapportSaisi.Text = NombreClientSaisie.ToString() + "/" + NombreClientLot.ToString();
                        List<CsEvenement> lstNonSaisie = listEvenemntCouranteDansLaGrid.Where(f => string.IsNullOrEmpty(f.CAS)).ToList();
                        ReloadDatagraid();
                   
                        this.Txt_LibelleCas.Text = string.Empty;
                        this.Txt_CasIndex.Text = string.Empty;
                        this.Txt_IndexEvt.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.MisAJourEvenementIndexAsync (_ListEvenement);
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
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.MisAJourEvenementIndexCompleted  += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur survenue à la mise à jour.", "Erreur");
                            string error = args.Error.InnerException.ToString();
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur de mise à jour du lot.", "Erreur");
                            return;
                        }
                        this.Txt_IndexEvt.Text = string.Empty;
                        if (ListeEvenementSaisi.FirstOrDefault(t => t.PK_ID == _ListEvenement.PK_ID  && t.POINT == _ListEvenement.POINT) == null)
                        {
                            NombreEvtSaisiClient = NombreEvtSaisiClient + 1;
                            NombreClientSaisie = NombreClientSaisie + 1;
                            ListeEvenementSaisi.Add(_ListEvenement);
                            ListeEvenementASaisi.Remove(_ListEvenement);
                        }
                        this.txt_RapportSaisi.Text = NombreClientSaisie.ToString() + "/" + NombreClientLot.ToString();
                       List<CsEvenement> lstNonSaisie = listEvenemntCouranteDansLaGrid.Where (f => string.IsNullOrEmpty(f.CAS) ).ToList();
                       if (NombreEvtSaisiClient == 6 || (lstNonSaisie.Count == 0 && !IsMiseJour ))
                       {
                           if (IsMiseJour)
                           {
                               IsMiseJour = false;
                               this.Rd_Modifier.IsChecked = false;
                               this.txtClient.Text = string.Empty;
                           }
                           NombreEvtSaisiClient = 0;
                           if (listeevnt.First().PRODUIT != SessionObject.Enumere.ElectriciteMT)
                               RemoveElementGridApresInsert();
                           else
                               ReloadDatagraid();
                         
                       }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.MisAJourEvenementIndexAsync (listeevnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txt_index_GotFocus_1(object sender, RoutedEventArgs e)
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

        private void RemoveElementGridApresInsert()
        {
            try
            {
                dataGrid1.IsReadOnly = false;
                ObservableCollection<CsEvenement> listEvenemntCouranteDansLaGridTemp = new ObservableCollection<CsEvenement>();

                List<CsEvenement> lstNonSaisie = listEvenemntCouranteDansLaGrid.Where(f => string.IsNullOrEmpty(f.CAS) && f.STATUS != SessionObject.Enumere.EvenementSupprimer).ToList();
                if (lstNonSaisie != null && lstNonSaisie.Count !=0 )
                {
                    foreach (CsEvenement item in lstNonSaisie)
                        listEvenemntCouranteDansLaGridTemp.Add(item);
                    listEvenemntCouranteDansLaGrid = listEvenemntCouranteDansLaGridTemp;
                    dataGrid1.ItemsSource = null;
                    dataGrid1.ItemsSource = listEvenemntCouranteDansLaGrid;
                    if (listEvenemntCouranteDansLaGrid.Count != 0)
                        dataGrid1.SelectedItem = listEvenemntCouranteDansLaGrid.First();
                    this.Txt_IndexEvt.Focus();
                    dataGrid1.IsReadOnly = true;
                }
                else
                {
                  dataGrid1.ItemsSource = null;
                  if (!IsMiseJour)
                    Message.ShowInformation("Saisie de lot terminé " +
                                "\n\r" + "      " + Galatee.Silverlight.Resources.Index.Langue.MsgCalculeFacture, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ReloadDatagraid()
        {
            try
            {
                dataGrid1.IsReadOnly = false;
                List<CsEvenement> lstNonSaisie = ListeEvenementASaisi.Where(f => string.IsNullOrEmpty(f.CAS) && f.STATUS != SessionObject.Enumere.EvenementSupprimer).ToList();
                if (lstNonSaisie != null && lstNonSaisie.Count != 0)
                {
                    CsEvenement leEvtCourant = lstNonSaisie.First();
                    FillDataGridView(ListeEvenementTotal.Where(t => t.FK_IDABON == leEvtCourant.FK_IDABON).ToList());
                    this.Txt_IndexEvt.Focus();
                    dataGrid1.IsReadOnly = true;
                }
                else
                {
                    dataGrid1.ItemsSource = null;
                    if (!IsMiseJour)
                        Message.ShowInformation("Saisie de lot terminé " +
                                        "\n\r" + "      " + Galatee.Silverlight.Resources.Index.Langue.MsgCalculeFacture, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void MettreAJourChampEvenementSelectionne(CsEvenement evenement)
        {
            try
            {
                    this.Txt_Ordtour.Text = evenement.ORDTOUR == null ? string.Empty  : evenement.ORDTOUR;
                    this.Txt_Point.Text = evenement.POINT == null ? string.Empty  : evenement.POINT.ToString(); 
                    this.Txt_derniereConsoFact.Text = evenement.CONSOFACPRECEDENT==null  ? string.Empty  : evenement.CONSOFACPRECEDENT.ToString();
                    this.Txt_CasPrecedent.Text = evenement.CASPRECEDENTEFACTURE == null ? string.Empty : evenement.CASPRECEDENTEFACTURE.ToString();

                    if (evenement.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        this.Txt_diametre.Text = evenement.REGLAGECOMPTEUR == null ? string.Empty : evenement.REGLAGECOMPTEUR;
                    else
                    {
                        this.Txt_diametre.Text = evenement.TYPECOMPTAGE == null ? string.Empty  : evenement.TYPECOMPTAGE; 
                        this.label19.Content = "Type comptage";
                    }

                    this.Txt_Tourne.Text = string.IsNullOrEmpty(evenement.TOURNEE) ? string.Empty  : evenement.TOURNEE;
                    this.Txt_Releveur.Text = string.IsNullOrEmpty(evenement.LIBELLERELEVEUR) ? string.Empty : evenement.LIBELLERELEVEUR;
                    this.Txt_TypeCompteur.Text = string.IsNullOrEmpty(evenement.LIBELLETYPECOMPTEUR) ? string.Empty : evenement.LIBELLETYPECOMPTEUR;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");
            }
        }



        bool EstCasValider = true;
        private void VerifieCas(bool isvalidation)
        {
            this.Txt_IndexEvt.IsReadOnly = false;
            this.Txt_Consomation.IsReadOnly = true;
            EstCasValider = true;
            // Saisie d'index
            if (!string.IsNullOrEmpty( this.Txt_CasIndex.Text) && int.Parse(this.Txt_CasIndex.Text) > 50)
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Cas d'index non saisi", "Attention");
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
                        Message.ShowInformation(Langue.msg_SaisiIndexObligatoire, "Attention");
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
                    if (int.Parse(this.Txt_IndexEvt.Text) != LeEvtSelect.INDEXPRECEDENTEFACTURE)
                    {
                        if (isvalidation)
                        {
                            Message.ShowInformation(Langue.msg_SaisiIndexInterdite, "Attention");
                            IsEtatSaisie = false;
                            dataGrid1.IsEnabled = false;
                            LeEvtSelect.INDEXEVT = null;
                            this.Txt_IndexEvt.Text = string.Empty;
                            this.Txt_IndexEvt.IsReadOnly = true;
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
                this.Txt_Consomation.IsEnabled = true;
                this.Txt_Consomation.IsReadOnly = false;
                //if (!string.IsNullOrEmpty(this.Txt_Consomation.Text)) ZEG 15/09/2017
                if (string.IsNullOrEmpty(this.Txt_Consomation.Text)) // ZEG 15/09/2017
                {

                    if (isvalidation)
                    {
                        Message.ShowInformation(Langue.msg_SaisiConsoObligatoire, "Attention");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;

                }
                else
                    LeEvtSelect.CONSO = int.Parse(this.Txt_Consomation.Text); // ZEG 15/09/2017
            }
            else if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeInterdit)
            {
                if (LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire &&
                    !string.IsNullOrEmpty(this.Txt_CasIndex.Text) &&
                    this.Txt_CasIndex.Text == "10")
                {
                    if (string.IsNullOrEmpty(this.Txt_Consomation.Text.ToString()))
                    this.Txt_Consomation.IsReadOnly = false;
                    return;
                }
                if (!string.IsNullOrEmpty(this.Txt_Consomation.Text.ToString()))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation(Langue.msg_SaisiConsoInterdite, "Attention");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                        this.Txt_Consomation.Text = string.Empty;
                        this.Txt_Consomation.IsReadOnly = true;
                    }
                    EstCasValider = false;

                    return;
                }
                else
                    this.Txt_Consomation.IsReadOnly = true;
            }


            if (LeCasRecherche.SAISIECOMPTEUR == SessionObject.Enumere.CodeObligatoire)
            {
                if (string.IsNullOrEmpty(this.txt_NumCpt.Text))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("La saisie du compteur est obligatoire", "Attention");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;
                }
            }



            if (LeCasRecherche.CODE != "05" && LeCasRecherche.CODE != "10" &&
                LeCasRecherche.CODE != "13" && LeCasRecherche.CODE != "00" &&
                LeEvtSelect.TYPECOMPTEUR  != SessionObject.Enumere.TypeComptageMaximetre  &&
                (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text) && int.Parse(this.Txt_IndexEvt.Text) < LeEvtSelect.INDEXPRECEDENTEFACTURE))
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                    IsEtatSaisie = false;
                    dataGrid1.IsEnabled = false;
                }
                EstCasValider = false;
                return;
            }
            if (LeCasRecherche.CODE == "05" &&
               (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text) && int.Parse(this.Txt_IndexEvt.Text) > LeEvtSelect.INDEXPRECEDENTEFACTURE))
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                    IsEtatSaisie = false;
                    dataGrid1.IsEnabled = false;
                }
                EstCasValider = false;
                return;
            }

 




            if (LeCasRecherche.CODE == "05" && !string.IsNullOrEmpty(Txt_IndexEvt.Text))
            {
                if (int.Parse(Txt_IndexEvt.Text) >= LeEvtSelect.INDEXPRECEDENTEFACTURE)
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                        IsEtatSaisie = false;
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;
                }
            }

        
        }



        CsCasind LeCasRecherche = new CsCasind();
        private void Txt_CasIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.Txt_IndexEvt .IsReadOnly = false;
                this.Txt_Consomation.IsReadOnly = false;

                if (this.Txt_CasIndex.Text.Length == SessionObject.Enumere.TailleCas)
                {

                    if (this.Txt_CasIndex.Text == "13" || this.Txt_CasIndex.Text == "27")
                        this.txt_NumCpt.IsReadOnly = false;
                    else
                        this.txt_NumCpt.IsReadOnly = true;

                    LeCasRecherche = RetourneLibelleCas(this.Txt_CasIndex.Text);
                    if (LeCasRecherche == null )
                    {
                        Message.ShowInformation("Cas inexistant","Information");
                        EstCasValider = false;
                        return;
                    }
                    this.Txt_LibelleCas.Text = LeCasRecherche.LIBELLE;

                    if (string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                        VerifieCas(false);
                    else
                        VerifieCas(true);
                   

                }
            }
            catch (Exception ex)
            {
                 Message.ShowError(ex,"Erreur");
            }

        }

        private void Txt_DateEvt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_DateEvt.Text.Length == SessionObject.Enumere.TailleDate)
                {
                    if (!ClasseMethodeGenerique.IsDateValide(this.Txt_DateEvt.Text))
                        Message.Show("Date non valide", "Attention");
                }
            }
            catch (Exception ex)
            {
                 Message.ShowError(ex,"Erreur");
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
             Message.ShowError(ex,"Erreur");
            }
        }



        private void Txt_LibelleCas_GotFocus_1(object sender, RoutedEventArgs e)
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

        void IsSaisieValider(CsEvenement LaSaisie, List<CsCasind> LstCas)
        {
            LeEvtSelect = new CsEvenement();
            LeEvtSelect = LaSaisie;
            CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LaSaisie.CAS);
            if (LeCasRecherche == null)
            {
                Message.ShowInformation(Langue.msg_CasInexistant , "Erreur");
                IsEtatSaisie = false;
                dataGrid1.IsEnabled = false;
                return;
            }
            IsCasValider(LaSaisie, LeCasRecherche);
        }

        void IsCasValider(CsEvenement LaSaise,CsCasind leCasSaisi)
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
                        dataGrid1.IsEnabled = true;
                        ValiderSaisi();
                        return;
                    }
                    if (leCasSaisi.CODE == "13")
                    {
                        RetourneEvenemntPose(LeEvtSelect);
                        return;
                    }

                    if (LeEvtSelect.CAS == "10" && LeEvtSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageHoraire )  // Ne pas afficher le message de retour a zero
                    {
                        RetourneAZero(LaSaise, LeEvtSelect.CAS);
                        return;
                    }
                    else if (LeEvtSelect.CAS == "05" || 
                        LeEvtSelect.CAS == "14" || 
                        LeEvtSelect.CAS == "27" || 
                        LeEvtSelect.CAS == "13" || 
                        (LeEvtSelect.CAS == "10" && LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire))
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
            if(!string.IsNullOrEmpty( this.Txt_IndexEvt.Text ) && (int.Parse(this.Txt_IndexEvt.Text ) > LaSaise.INDEXPRECEDENTEFACTURE )) 
            {
                    int? MoyenneComparaisonSup = LaSaise.CONSOMOYENNEPRECEDENTEFACTURE * 2;
                    int? MoyenneComparaisonInf = (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE ) * 2;
                    if (LaSaise.CAS == "00" && MoyenneComparaisonInf < LaSaise.CONSOMOYENNEPRECEDENTEFACTURE 
                        && LaSaise.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre
                        && LaSaise.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageHoraire )
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoFaible, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = string.Empty;
                                LeEvtSelect.INDEXEVT = null;
                                LeEvtSelect.CONSO = null;
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
                    if (LaSaise.CAS == "00" && (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE) > MoyenneComparaisonSup &&
                        LaSaise.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre &&
                        LaSaise.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageHoraire 
                        )
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoForte , MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = string.Empty;
                                LeEvtSelect.INDEXEVT = null;
                                LeEvtSelect.CONSO = null;
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
                    if (LaSaise.CAS == "00" &&  LaSaise.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageMaximetre && 
                        int.Parse(this.Txt_IndexEvt.Text )> LaSaise.PUISSANCE )
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.Msg_DepassementPuissance, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = string.Empty;
                                LeEvtSelect.INDEXEVT = null;
                                LeEvtSelect.CONSO = null;
                                dataGrid1.IsEnabled = false;
                            }
                            else if (ws.Result == MessageBoxResult.OK)
                            {
                                LaSaise.CAS = "80";
                                ValiderSaisi();
                            }
                        };
                        ws.Show();
                        return;
                    }
                    if (LaSaise.CAS == "00" && LaSaise.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire && 
                        (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE) > SessionObject.Enumere.BorneConsohoraire + 72 )
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoForte, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = string.Empty;
                                LeEvtSelect.INDEXEVT = null;
                                LeEvtSelect.CONSO = null;
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
                    if (LaSaise.CAS == "00" && LaSaise.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire && 
                        (int.Parse(this.Txt_IndexEvt.Text) - LaSaise.INDEXPRECEDENTEFACTURE) < SessionObject.Enumere.BorneConsohoraire - 72)
                    {
                        var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Langue.msg_ConsoFaible , MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        ws.OnMessageBoxClosed += (l, results) =>
                        {
                            if (ws.Result == MessageBoxResult.No)
                            {
                                LeEvtSelect.CAS = string.Empty;
                                LeEvtSelect.INDEXEVT = null;
                                LeEvtSelect.CONSO = null;
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
         }
            ValiderSaisi();

        }

         private void ValiderSaisi()
         {
             try
             {

                 CsCasind leCasSaisi = LstCas.FirstOrDefault(t => t.CODE == LeEvtSelect.CAS );
                 if (leCasSaisi.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                 {
                     if (string.IsNullOrEmpty(this.Txt_Consomation.Text)) // ZEG 15/09/2017
                         LeEvtSelect.CONSO = null;
                 }


                   LeEvtSelect.ENQUETE = leCasSaisi.ENQUETABLE ? "E" : string.Empty;
                   if (!string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                       LeEvtSelect.INDEXEVT = int.Parse(this.Txt_IndexEvt.Text);

                   if ((LeEvtSelect.CONSO == null && LeEvtSelect.CAS != "13")||(LeEvtSelect.IsSaisi  )) //Pour eviter les ca 10 
                   {
                       if (LeEvtSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                       {
                           if (string.IsNullOrEmpty(this.Txt_IndexEvt.Text))
                           {
                               if (string.IsNullOrEmpty(this.Txt_Consomation.Text)) // ZEG 15/09/2017
                                   LeEvtSelect.CONSO = 0;
                           }
                           else if (LeEvtSelect.CAS != "10")
                               //LeEvtSelect.CONSO = string.IsNullOrEmpty(this.Txt_Consomation.Text) ? (int?)((int.Parse(this.Txt_IndexEvt.Text) - (LeEvtSelect.INDEXPRECEDENTEFACTURE == null ? 0 : LeEvtSelect.INDEXPRECEDENTEFACTURE)) * (LeEvtSelect.COEFLECT == 0 ? 1 : LeEvtSelect.COEFLECT)) : int.Parse(this.Txt_Consomation.Text);
                               LeEvtSelect.CONSO = (int?)((int.Parse(this.Txt_IndexEvt.Text) - (LeEvtSelect.INDEXPRECEDENTEFACTURE == null ? 0 : LeEvtSelect.INDEXPRECEDENTEFACTURE)));
                       }
                       if (LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire && LeEvtSelect.CAS == "10")
                           LeEvtSelect.CONSO = string.IsNullOrEmpty(this.Txt_Consomation.Text) ? 0 :int.Parse(this.Txt_Consomation.Text);

                       if (LeEvtSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageMaximetre)
                           LeEvtSelect.CONSO = LeEvtSelect.INDEXEVT != null ? LeEvtSelect.INDEXEVT : 0;
                   }
                   LeEvtSelect.IsSaisi = true;
                   if (IsAjout)
                   {
                       var ws = new MessageBoxControl.MessageBoxChildWindow("Index", "Voulez vous ajouter l'evenement ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                       ws.OnMessageBoxClosed += (l, results) =>
                       {
                           if (ws.Result == MessageBoxResult.OK)
                           {
                               List<CsEvenement> LeEvtSelectClient = listEvenemntCouranteDansLaGrid.Where(ev => ev.CENTRE == LeEvtSelect.CENTRE &&
                                                                                        ev.CLIENT == LeEvtSelect.CLIENT && ev.ORDRE == LeEvtSelect.ORDRE &&
                                                                                        ev.PRODUIT == LeEvtSelect.PRODUIT && ev.POINT == LeEvtSelect.POINT ).ToList();
                               LeEvtSelectClient.ForEach(t => t.STATUS = SessionObject.Enumere.EvenementReleve );
                               LeEvtSelectClient.ForEach(t => t.MATRICULE = UserConnecte.matricule);
                               LeEvtSelectClient.ForEach(t => t.USERCREATION  = UserConnecte.matricule);
                               LeEvtSelectClient.ForEach(t => t.LOTRI = ListeLot.First().NUMLOTRI);
                               LeEvtSelectClient.ForEach(t => t.PERIODE = ListeLot.First().PERIODE);
                               if (ListeEvenementTotal.FirstOrDefault(o=>o.FK_IDABON ==LeEvtSelectClient.First().FK_IDABON && o.LOTRI ==LeEvtSelectClient.First().LOTRI ) != null )
                                    LeEvtSelectClient.ForEach(t => t.ISAJOUTLOT  = true );
                               InsertEvenementListe(LeEvtSelectClient);
                               return;
                           }
                           else
                               return;
                       };
                       ws.Show();
                       return;
                   }

                   UpdateEvenement(LeEvtSelect);
                   if (!string.IsNullOrEmpty(this.txt_NumCpt.Text))
                   {
                       if (LeEvtSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                       {
                           if (listEvenemntCouranteDansLaGrid.FirstOrDefault(ev => ev.CENTRE == LeEvtSelect.CENTRE &&
                                                                 ev.CLIENT == LeEvtSelect.CLIENT && ev.ORDRE == LeEvtSelect.ORDRE &&
                                                                 ev.PRODUIT == LeEvtSelect.PRODUIT && ev.STATUS== SessionObject.Enumere.EvenementCree ) != null)
                           {

                               this.txt_NumCpt.IsReadOnly = true;
                               this.Txt_CasIndex.IsReadOnly = true;
                           }
                           else
                           {
                               this.txt_NumCpt.Text = string.Empty;
                               this.txt_NumCpt.IsReadOnly = true;
                               this.Txt_CasIndex.IsReadOnly = false;
                               this.Txt_LibelleCas.Text = string.Empty;
                               this.Txt_CasIndex.Text = string.Empty;
                               this.Txt_IndexEvt.Text = string.Empty;
                           }

                       }
                       else
                       {
                           this.txt_NumCpt.Text = string.Empty;
                           this.txt_NumCpt.IsReadOnly = true;
                           this.Txt_CasIndex.IsReadOnly = false ;
                           this.Txt_LibelleCas.Text = string.Empty;
                           this.Txt_CasIndex.Text = string.Empty;
                           this.Txt_IndexEvt.Text = string.Empty;
                       }
                   }
                   else
                   {
                       this.txt_NumCpt.Text = string.Empty;
                       this.txt_NumCpt.IsReadOnly = true;
                       this.Txt_CasIndex.IsReadOnly = false;
                       this.Txt_LibelleCas.Text = string.Empty;
                       this.Txt_CasIndex.Text = string.Empty;
                       this.Txt_IndexEvt.Text = string.Empty;

                     
                   }
                   ReinitialiserGrid();
                   if (this.Txt_IndexEvt.IsReadOnly)
                       this.Txt_IndexEvt.IsReadOnly = false ;

                     this.Txt_Consomation.IsReadOnly = true  ;
                     this.Txt_NbrRoue.Text = string.Empty;
                     this.Txt_NbrRoue.IsReadOnly = true;
                     this.chkmodifRoue.IsChecked = false ;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         private void ChildWindow_KeyUp_1(object sender, KeyEventArgs e)
         {
             if (e.Key.Equals(Key.Enter))
             {
                 OKButton_Click(sender, null);
             }
         }
         bool IsMiseJour = false;
         bool IsSupprimer = false;
         bool IsAjout = false;
         private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
         {
             if (Rd_Modifier.IsChecked == true)
             {
                 NombreEvtSaisiClient = 0;
                 IsMiseJour = true;
                 List<CsEvenement> lesEvtRecherche = ListeEvenementTotal.Where(t => t.CLIENT == this.txtClient.Text).ToList();
                 if (lesEvtRecherche != null && lesEvtRecherche.Count != 0)
                     FillDataGridView(lesEvtRecherche);
                 else
                 {
                     IsMiseJour = false;
                     Message.ShowInformation("Client non trouvé", "Facturation");
                     return;
                 }
             }
             else if (Rd_Ajouter.IsChecked == true)
             {
                 IsAjout = true;
                 VerifieClientEvenement(ListeLot, this.txtClient.Text);
             }
             else if (Rd_Supprimer.IsChecked == true)
             {
                 IsSupprimer = true;
                 NombreEvtSaisiClient = 0;
                 IsMiseJour = true;
                 List<CsEvenement> lesEvtRecherche = ListeEvenementTotal.Where(t => t.CLIENT == this.txtClient.Text).ToList();
                 if (lesEvtRecherche != null && lesEvtRecherche.Count != 0)
                 {
                     FillDataGridView(lesEvtRecherche);
                     this.dataGrid1.IsReadOnly = true;
                 }
                 else
                 {
                     IsMiseJour = false;
                     Message.ShowInformation("Client non trouvé", "Facturation");
                     return;
                 }
             }
                  
         }

         private void VerifieClientEvenement(List<CsLotri> lesLot,string  LeClient)
         {
             FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
             service.VerifieClientCompleted += (s, args) =>
             {
                 if (args != null && args.Cancelled)
                     return;
                 List<CsClient>   Resultat = args.Result;

                 if (Resultat != null)
                 {
                     List<int> lesIdCentreLot = lesLot.Select(t => t.FK_IDCENTRE).ToList();
                     List<int> lesIdProduitLot = lesLot.Select(t => t.FK_IDPRODUIT.Value).ToList();
                     List<int> lesIdCategorieLot = lesLot.Select(t => t.FK_IDCATEGORIECLIENT.Value).ToList();
                     List<int> lesIdTourneeLot = lesLot.Select(t => t.FK_IDTOURNEE.Value).ToList();

                     List<CsClient> lesClientLotri = Resultat.Where(t =>
                                                                        lesIdCentreLot.Contains(t.FK_IDCENTRE.Value) &&
                                                                        lesIdCategorieLot.Contains(t.FK_IDCATEGORIE.Value) &&
                                                                        lesIdTourneeLot.Contains(t.FK_IDTOURNEE) &&
                                                                        lesIdProduitLot.Contains(t.FK_IDPRODUIT.Value)).ToList();
                     if (lesClientLotri.Count == 0)
                     {
                         Message.ShowInformation("Le client ne correspond pas au lot","Facturation");
                         return;
                     }

                     FacturationServiceClient services = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                     services.VerifieExisteLotClientCompleted += (ss, argss) =>
                     {
                         if (argss != null && args.Cancelled)
                             return;
                         bool Resultats = argss.Result ;
                         if (Resultats == true)
                         {

                             var ws = new MessageBoxControl.MessageBoxChildWindow("index", "Un événement non facturé existe sur ce client . " + "\n\r " + "Il sera supprimé par la facture isolé", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                             ws.OnMessageBoxClosed += (l, results) =>
                             {
                                 if (ws.Result == MessageBoxResult.OK)
                                 {
                                     CsClient leClient = new CsClient();
                                     leClient.CENTRE = lesClientLotri.First().CENTRE;
                                     leClient.REFCLIENT = lesClientLotri.First().REFCLIENT;
                                     leClient.ORDRE = lesClientLotri.First().ORDRE;
                                     leClient.PRODUIT = lesClientLotri.First().PRODUIT;

                                     FacturationServiceClient ClientSrv = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                                     ClientSrv.RetourneEvenementClientCompleted += (e, argsss) =>
                                     {
                                         if (argss != null && args.Cancelled)
                                             return;
                                         List<CsEvenement> Res = argsss.Result;
                                         FillDataGridView(Res);
                                     };
                                     ClientSrv.RetourneEvenementClientAsync(lesClientLotri.First(), LeClient);
                                 }
                                 else
                                     return;
                             };
                             ws.Show();
                         }
                         else
                         {
                             CsClient leClient = new CsClient();
                             leClient.CENTRE = lesClientLotri.First().CENTRE;
                             leClient.REFCLIENT = lesClientLotri.First().REFCLIENT;
                             leClient.ORDRE = lesClientLotri.First().ORDRE;
                             leClient.PRODUIT = lesClientLotri.First().PRODUIT;

                             FacturationServiceClient ClientSrv = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                             ClientSrv.RetourneEvenementClientCompleted += (e, argsss) =>
                             {
                                 if (argss != null && args.Cancelled)
                                     return;
                                 List<CsEvenement> Res = argsss.Result;
                                 FillDataGridView(Res);
                             };
                             ClientSrv.RetourneEvenementClientAsync(lesClientLotri.First(), LeClient);
                         
                         }
                  


                     };
                     services.VerifieExisteLotClientAsync(lesClientLotri.First(), lesLot.First().NUMLOTRI );
                 
                 }
                 else
                 {
                     Message.ShowInformation("Le client n'existe pas","Facturation");
                     return;
                 }
             };
             service.VerifieClientAsync(lesLot, LeClient);
         }
   

         private void Btn_ContinuerSaisie_Click(object sender, RoutedEventArgs e)
         {
             IsMiseJour = false;
             IsAjout = false;
             IsSupprimer = false;
             FillDataGridView(ListeEvenementASaisi);
         }

         private void Rd_Ajouter_Checked(object sender, RoutedEventArgs e)
         {
             this.Btn_Recherche.IsEnabled = true;
             this.txtClient.IsEnabled = true;
         }

         private void Rd_Modifier_Checked(object sender, RoutedEventArgs e)
         {
             this.Btn_Recherche.IsEnabled = true;
             this.txtClient.IsEnabled = true;
         }

         private void Rd_Supprimer_Checked(object sender, RoutedEventArgs e)
         {
             this.Btn_Recherche.IsEnabled = true;
             this.txtClient.IsEnabled = true;
         }

         private void chkmodifRoue_Checked(object sender, RoutedEventArgs e)
         {
             this.Txt_NbrRoue.IsReadOnly = false;
         }

         private void chkmodifRoue_UnChecked(object sender, RoutedEventArgs e)
         {
             this.Txt_NbrRoue.IsReadOnly  = true ;
             this.Txt_NbrRoue.Text = string.Empty;
         }
    }
}

