using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Parametrage
{
    public partial class CwCasReleveIndex : ChildWindow
    {
        List<CsCasind> listForInsertOrUpdate = null;
        ObservableCollection<CsCasind> donnesDatagrid = new ObservableCollection<CsCasind>();
        private CsCasind ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public List<CsCasind>  MyCasind { get; set; }
        public CwCasReleveIndex()
        {
            InitializeComponent();
        }
       
        public CwCasReleveIndex(CsCasind pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                if (pObject != null)
                    ObjetSelectionnee = pObject;
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCasind>;

                this.CboEnqueteNonConfirmee.ItemsSource = this.ConstituerListeGroupeBoxFacturation();
                this.CboEnqueteNonConfirmee.DisplayMemberPath = "LIBELLE";
                this.CboEnqueteNonConfirmee.SelectedValuePath = "CODE";

                this.CboSansEnqueteOuConfirmee.ItemsSource = this.ConstituerListeGroupeBoxFacturation();
                this.CboSansEnqueteOuConfirmee.DisplayMemberPath = "LIBELLE";
                this.CboSansEnqueteOuConfirmee.SelectedValuePath = "CODE";

                this.CboIndex.ItemsSource = this.ConstituerListeGroupeBoxSaisie();
                this.CboIndex.DisplayMemberPath = "LIBELLE";
                this.CboIndex.SelectedValuePath = "CODE";

                this.CboCompteur.ItemsSource = this.ConstituerListeGroupeBoxSaisie();
                this.CboCompteur.DisplayMemberPath = "LIBELLE";
                this.CboCompteur.SelectedValuePath = "CODE";

                this.CboConsommation.ItemsSource = this.ConstituerListeGroupeBoxSaisie();
                this.CboConsommation.DisplayMemberPath = "LIBELLE";
                this.CboConsommation.SelectedValuePath = "CODE";

                RemplirListeDesCentreExistant(ObjetSelectionnee);

                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        RemplirComboInformationComplementaire(ObjetSelectionnee);
                        ChargerDonnees(ObjetSelectionnee);
                        OKButton.IsEnabled = false;
                        //TxtCas.IsReadOnly = true;
                    }
                }
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }

        private void ChargerDonnees(CsCasind pCasind)
        {
            try
            {
                TxtCas.Text = !string.IsNullOrEmpty(pCasind.CODE) ? pCasind.CODE : string.Empty;
                TxtLibelle.Text = !string.IsNullOrEmpty(pCasind.LIBELLE) ? pCasind.LIBELLE : string.Empty;
                //TxtLienSurLaFacture.Text = !string.IsNullOrEmpty(pCasind.LIBFAC) ? pCasind.LIBFAC : string.Empty;

                foreach (CsLibelle index in CboIndex.ItemsSource)
                {
                    if (index.LIBELLE == pCasind.SAISIEINDEX)
                    {
                        CboIndex.SelectedItem = index;
                        break;
                    }
                }

                foreach (CsLibelle compteur in CboCompteur.ItemsSource)
                {
                    if (compteur.LIBELLE == pCasind.SAISIECOMPTEUR)
                    {
                        CboCompteur.SelectedItem = compteur;
                        break;
                    }
                }

                foreach (CsLibelle consommation in CboConsommation.ItemsSource)
                {
                    if (consommation.LIBELLE == pCasind.SAISIECONSO)
                    {
                        CboConsommation.SelectedItem = consommation;
                        break;
                    }
                }

                foreach (CsLibelle enqueteNonConfirmee in CboEnqueteNonConfirmee.ItemsSource)
                {
                    if (enqueteNonConfirmee.CODE == pCasind.APRESENQUETE)
                    {
                        CboEnqueteNonConfirmee.SelectedItem = enqueteNonConfirmee;
                        break;
                    }
                }

                foreach (CsLibelle SansEnqueteOuConfirmee in CboSansEnqueteOuConfirmee.ItemsSource)
                {
                    if (SansEnqueteOuConfirmee.CODE == pCasind.SANSENQUETE)
                    {
                        CboSansEnqueteOuConfirmee.SelectedItem = SansEnqueteOuConfirmee;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesCentreExistant(CsCasind pCasind)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CasDeReleve);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        //this.CboCentre.ItemsSource = args.Result;
                        //this.CboCentre.DisplayMemberPath = "Libelle";
                        //this.CboCentre.SelectedValuePath = "PK_CodeCentre";

                        //if (pCasind != null)
                        //{
                        //    foreach (CsCentre centre in CboCentre.ItemsSource)
                        //    {
                        //        if (centre.PK_ID == pCasind.FK_IDCENTRE)
                        //        {
                        //            CboCentre.SelectedItem = centre;
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private List<CsLibelle> ConstituerListeGroupeBoxSaisie()
        {
            try
            {
                List<CsLibelle> ListeLibelle = new List<CsLibelle>();
                CsLibelle Facultatif = new CsLibelle();
                Facultatif.CODE = "F";
                Facultatif.LIBELLE = Languages.LibFacultatif;
                ListeLibelle.Add(Facultatif);

                CsLibelle Interdit = new CsLibelle();

                Interdit.CODE = "I";
                Interdit.LIBELLE = Languages.LibInterdit;
                ListeLibelle.Add(Interdit);

                CsLibelle Obligatoire = new CsLibelle();

                Obligatoire.CODE = "O";
                Obligatoire.LIBELLE = Languages.LibObligatoire;
                ListeLibelle.Add(Obligatoire);

                return ListeLibelle;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<CsCasind> ConstruireListeComboInformationComplementaire(CsCasind pCasind)
        {
            List<CsCasind> ListeEcrasable = new List<CsCasind>();
            try
            {
                CsCasind CasEcrasable1 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN1))
                {
                    CasEcrasable1.CODE = pCasind.CASGEN1.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN1.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable1.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable1);
                        }
                    }
                    
                }

                CsCasind CasEcrasable2 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN2))
                {
                    CasEcrasable2.CODE = pCasind.CASGEN2.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN2.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable2.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable2);
                        }
                    }
                    
                }

                CsCasind CasEcrasable3 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN3))
                {
                    CasEcrasable3.CODE = pCasind.CASGEN3.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN3.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable3.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable3);
                        }
                    }
                    
                }

                CsCasind CasEcrasable4 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN4))
                {
                    CasEcrasable4.CODE = pCasind.CASGEN4.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN4.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable4.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable4);
                        }
                    }
                    
                }

                CsCasind CasEcrasable5 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN5))
                {
                    CasEcrasable5.CODE = pCasind.CASGEN5.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN5.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable5.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable5);
                        }
                    }
                    
                }
                CsCasind CasEcrasable6 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN6))
                {
                    CasEcrasable6.CODE = pCasind.CASGEN6.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN6.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable6.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable6);
                        }
                    }
                    
                }
                CsCasind CasEcrasable7 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN7))
                {
                    CasEcrasable7.CODE = pCasind.CASGEN7.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN7.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable7.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable7);
                        }
                    }
                    
                }
                CsCasind CasEcrasable8 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN8))
                {
                    CasEcrasable8.CODE = pCasind.CASGEN8.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN8.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable8.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable8);
                        }
                    }
                    
                }
                CsCasind CasEcrasable9 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN9))
                {
                    CasEcrasable9.CODE = pCasind.CASGEN9.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN9.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable9.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable9);
                        }
                    }
                    
                }
                CsCasind CasEcrasable10 = new CsCasind();
                if (!string.IsNullOrEmpty(pCasind.CASGEN10))
                {
                    CasEcrasable10.CODE = pCasind.CASGEN10.ToString();
                    if (donnesDatagrid != null && donnesDatagrid.Count > 0)
                    {
                        var libelle = donnesDatagrid.FirstOrDefault(p => p.CODE == pCasind.CASGEN10.ToString());
                        if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                        {
                            CasEcrasable10.LIBELLE = libelle.LIBELLE;
                            ListeEcrasable.Add(CasEcrasable10);
                        }
                    }
                    
                }
                return ListeEcrasable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RemplirComboInformationComplementaire(CsCasind pCasind)
        {
            try
            {
                List<CsCasind> ListEcrasable = new List<CsCasind>();
                ListEcrasable = this.ConstruireListeComboInformationComplementaire(pCasind);
                if (ListEcrasable != null && ListEcrasable.Count > 0)
                {
                    this.CboCasEcrasable.Items.Clear();
                    CboCasEcrasable.ItemsSource = null;
                    foreach (CsCasind item in ListEcrasable)
                    {
                        this.CboCasEcrasable.Items.Add(item);
                    }
                    this.CboCasEcrasable.DisplayMemberPath = "LIBELLE";
                    this.CboCasEcrasable.SelectedValuePath = "PK_ID";
                    //CboCasEcrasable.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsLibelle> ConstituerListeGroupeBoxFacturation()
        {
            List<CsLibelle> ListeLibelle = new List<CsLibelle>();
            try
            {
                CsLibelle Normale = new CsLibelle();
                Normale.CODE = "1";
                Normale.LIBELLE = Languages.LibNormale;
                ListeLibelle.Add(Normale);

                CsLibelle Forfait = new CsLibelle();
                Forfait.CODE = "2";
                Forfait.LIBELLE = Languages.LibForfait;
                ListeLibelle.Add(Forfait);

                CsLibelle BloqueSansRegularisation = new CsLibelle();
                BloqueSansRegularisation.CODE = "3";
                BloqueSansRegularisation.LIBELLE = Languages.LibBloqueeSansRegularisation;
                ListeLibelle.Add(BloqueSansRegularisation);

                CsLibelle SansTarifUnitaire = new CsLibelle();
                SansTarifUnitaire.CODE = "4";
                SansTarifUnitaire.LIBELLE = Languages.LibSansTarifUnitaire;
                ListeLibelle.Add(SansTarifUnitaire);

                CsLibelle SansTarifAnnuel = new CsLibelle();
                SansTarifAnnuel.CODE = "5";
                SansTarifAnnuel.LIBELLE = Languages.LibSansTarifAnnuel;
                ListeLibelle.Add(SansTarifAnnuel);

                CsLibelle ForfaitSansRegularisation = new CsLibelle();
                ForfaitSansRegularisation.CODE = "6";
                ForfaitSansRegularisation.LIBELLE = Languages.LibForfaitSansRegularisation;
                ListeLibelle.Add(ForfaitSansRegularisation);

                CsLibelle BloqueAvecRegulation = new CsLibelle();
                BloqueAvecRegulation.CODE = "7";
                BloqueAvecRegulation.LIBELLE = Languages.LibBloqueeAvecRegularisation;
                ListeLibelle.Add(BloqueAvecRegulation);

                CsLibelle EstimeAvecRegularisation = new CsLibelle();
                EstimeAvecRegularisation.CODE = "8";
                EstimeAvecRegularisation.LIBELLE = Languages.LibEstimeeAvecRegularisation;
                ListeLibelle.Add(EstimeAvecRegularisation);

                CsLibelle EstimeSansRegularisation = new CsLibelle();
                EstimeSansRegularisation.CODE = "9";
                EstimeSansRegularisation.LIBELLE = Languages.LibEstimeeSansRegularisation;
                ListeLibelle.Add(EstimeSansRegularisation);

                return ListeLibelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtCas.Text) && !string.IsNullOrEmpty(TxtLibelle.Text)
                    && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsCasind> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCasind>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var casind = new CsCasind();
                    casind.CODE = TxtCas.Text;
                    casind.LIBELLE = TxtLibelle.Text;
                    //casind.LIBFAC = TxtLienSurLaFacture.Text;
                    //if (CboCentre.SelectedItem != null)
                    //{
                    //    casind.CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE;
                    //    casind.FK_IDCENTRE = ((CsCentre)CboCentre.SelectedItem).PK_ID;
                    //}
                    if (CboIndex.SelectedItem != null)
                        casind.SAISIEINDEX = ((CsLibelle)CboIndex.SelectedItem).CODE;

                    if (CboCompteur.SelectedItem != null)
                        casind.SAISIECOMPTEUR = ((CsLibelle)CboCompteur.SelectedItem).CODE;

                    if (CboConsommation.SelectedItem != null)
                        casind.SAISIECONSO = ((CsLibelle)CboConsommation.SelectedItem).CODE;

                    if (CboEnqueteNonConfirmee.SelectedItem != null)
                        casind.APRESENQUETE = ((CsLibelle)CboEnqueteNonConfirmee.SelectedItem).CODE;

                    if (CboSansEnqueteOuConfirmee.SelectedItem != null)
                        casind.SANSENQUETE = ((CsLibelle)CboSansEnqueteOuConfirmee.SelectedItem).CODE;

                    RenseignerLesCasgen(casind);

                    casind.DATECREATION = DateTime.Now;
                    casind.USERCREATION = UserConnecte.matricule;

                    if (!string.IsNullOrEmpty(TxtCas.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == casind.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(casind);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {

                    ObjetSelectionnee.CODE = TxtCas.Text;
                    ObjetSelectionnee.LIBELLE = TxtLibelle.Text;
                    //ObjetSelectionnee.LIBFAC = TxtLienSurLaFacture.Text;
                    //if (CboCentre.SelectedItem != null)
                    //{
                    //    ObjetSelectionnee.FK_IDCENTRE = ((CsCentre)CboCentre.SelectedItem).PK_ID;
                    //    ObjetSelectionnee.CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE;
                    //}
                    if (CboIndex.SelectedItem != null)
                        ObjetSelectionnee.SAISIEINDEX = ((CsLibelle)CboIndex.SelectedItem).CODE;

                    if (CboCompteur.SelectedItem != null)
                        ObjetSelectionnee.SAISIECOMPTEUR = ((CsLibelle)CboCompteur.SelectedItem).CODE;

                    if (CboConsommation.SelectedItem != null)
                        ObjetSelectionnee.SAISIECONSO = ((CsLibelle)CboConsommation.SelectedItem).CODE;

                    if (CboEnqueteNonConfirmee.SelectedItem != null)
                        ObjetSelectionnee.APRESENQUETE = ((CsLibelle)CboEnqueteNonConfirmee.SelectedItem).CODE;

                    if (CboSansEnqueteOuConfirmee.SelectedItem != null)
                        ObjetSelectionnee.SANSENQUETE = ((CsLibelle)CboSansEnqueteOuConfirmee.SelectedItem).CODE;

                    RenseignerLesCasgen(ObjetSelectionnee);

                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerLesCasgen(CsCasind pCasind)
        {
            List<string> ColListName = new List<string>();
            CsCasind item = null;
            try
            {
                if (CboCasEcrasable.Items.Count > 0)
                {
                    if (MyCasind == null)
                    {
                        MyCasind = new List<CsCasind>();
                        for (int i = 0; i < CboCasEcrasable.Items.Count; i++)
                        {
                            item = new CsCasind();
                            item = (CsCasind)CboCasEcrasable.Items[i];
                            MyCasind.Add(item);
                            string colName = "CASGEN" + (i + 1).ToString();
                            ColListName.Add(colName);
                        }
                    }
                    var properties = pCasind.GetType().GetProperties();
                    foreach (var f in properties)
                    {
                        if (ColListName.Contains(f.Name.ToUpper()))
                        {
                            int index = int.Parse(f.Name.Substring(f.Name.Length - 1, 1));
                            var casreleve= MyCasind[index-1];
                            string casgen = casreleve.CODE;
                            f.SetValue(pCasind, casgen, null);
                        }
                    }
                }
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
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertCasindCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate);
                                    DialogResult = true;
                                };
                                service.InsertCasindAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCasindCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate);
                                    DialogResult = true;
                                };
                                service.UpdateCasindAsync(listForInsertOrUpdate);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void UpdateParentList(List<CsCasind> pListeObjet)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    if (pListeObjet != null && pListeObjet.Count > 0)
                        foreach (var item in pListeObjet)
                        {
                            donnesDatagrid.Add(item);
                        }
                }
                else if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    foreach (var item in pListeObjet)
                    {
                        donnesDatagrid.Remove(item);
                        donnesDatagrid.Add(item);
                    }
                }
                donnesDatagrid.OrderBy(p => p.CODE);
                DialogResult = true;
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

        private void BtnCasEcrasable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.GetCasindEcrasableByCasCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.Parametrage);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        if (args.Result != null)
                        {
                            //List<Galatee.Silverlight.ServiceCaisse.CParametre> ListeParam = new List<ServiceCaisse.CParametre>();
                            //foreach (var item in args.Result)
                            //{
                            //    Galatee.Silverlight.ServiceCaisse.CParametre parametre = new ServiceCaisse.CParametre();
                            //    parametre.VALEUR = item.PK_CAS;
                            //    parametre.LIBELLE = item.LIBELLE;
                            //    ListeParam.Add(parametre);
                            //}
                            List<object> _LstCasDeReleve = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstCasDeReleve, "PK_CAS", "LIBELLE", Languages.CasDeReleve);
                            ctr.Closed += new EventHandler(form_Closed);
                            ctr.Show();

                            //ListeParam.OrderBy(p => p.VALEUR);
                            //Shared.UcListe form = new Shared.UcListe(ListeParam, true);
                            //form.Closed += form_Closed;
                            //form.Title = "Liste des cas écrasables";
                            //form.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Languages.Parametrage);
                    }
                };
                client.GetCasindEcrasableByCasAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }

        private void form_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((Galatee.Silverlight.MainView.UcListeGenerique)sender);
                    if (form != null)
                    {
                        if (form.MyObjectList != null)
                        {
                           CboCasEcrasable.ItemsSource = form.MyObjectList.Cast<CsCasind>().ToList();
                           CboCasEcrasable.SelectedValuePath = "PK_ID";
                           CboCasEcrasable.DisplayMemberPath = "LIBELLE";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtCas_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }

        private void TxtLibelle_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }

        private void CboCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }
    }
}

