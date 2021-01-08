using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Galatee.Silverlight.Library;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
    public partial class UcReceptionLotScellesMagasinGeneral : ChildWindow
    {
        List<CsLotMagasinGeneral> listForInsertOrUpdate = new List<CsLotMagasinGeneral>();
        List<CsLotMagasinGeneral> listObjetForInsert = new List<CsLotMagasinGeneral>();
        List<CsLotMagasinGeneral> listSaisieScelleGrid = new List<CsLotMagasinGeneral>();
        List<CsRefOrigineScelle> ListOrigineScelle = new List<CsRefOrigineScelle>();
        List<CsLotMagasinGeneral> ListLotMagasinGeneral = new List<CsLotMagasinGeneral>();
        List<CsLotMagasinGeneral> ListSaisie = new List<CsLotMagasinGeneral>();
       
        
          CsRefOrigineScelle ListOginescellebyId=new CsRefOrigineScelle();
          bool resultat = false;
       // UCtrlListeReceptionLotsMagasinGeneral
        ObservableCollection<CsLotMagasinGeneral> donnesDatagrid = new ObservableCollection<CsLotMagasinGeneral>();
         Dictionary<string, string> ListeNumerosSaisis;
        private CsLotMagasinGeneral ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public CsLotMagasinGeneral ObjetSelectionne { get; set; }
        
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion
        public UcReceptionLotScellesMagasinGeneral()
        {
            try
            {
                InitializeComponent();
                Translate();
                listSaisieScelleGrid.Add(new CsLotMagasinGeneral { Numero_depart=" ",
                                                                   Numero_fin = " "
                });
                dtgrdlReceptionScelle.ItemsSource = listSaisieScelleGrid;
                RemplirListeCmbDesActivitesExistant();
                RemplirListeCmbDesFournisseursExistant();
                RemplirListeCmbDesOrigineScellesExistant();
                GetLongueurIDScelleSelon();
                GetListLotmagasingeneral();
                RetourneListeAllCouleurScelle();
                GetData();
                ModeExecution = SessionObject.ExecMode.Creation;

               // dtgrdlReceptionScelle.ItemsSource = donnesDatagrid;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Scelles);
            }
        }
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<CsLotMagasinGeneral> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }
        private void GetData()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllLotMagasinGeneralCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            //if (item.DateReception.Date == DateTime.Now.Date)
                                DonnesDatagrid.Add(item);
                        }
                    dtgrdScelle.ItemsSource = DonnesDatagrid.Where(i=>i.Nbre_Scelles >0);
                };
                client.SelectAllLotMagasinGeneralAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UcReceptionLotScellesMagasinGeneral(CsLotMagasinGeneral pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var LotMagasinGeneral = new CsLotMagasinGeneral();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(LotMagasinGeneral, pObject as CsLotMagasinGeneral);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                listSaisieScelleGrid.Clear();
                listSaisieScelleGrid.Add(new CsLotMagasinGeneral
                {
                    Numero_depart = " ",
                    Numero_fin = " "
                });
                dtgrdlReceptionScelle.ItemsSource  = listSaisieScelleGrid;
                RemplirListeCmbDesActivitesExistant();
                RemplirListeCmbDesFournisseursExistant();
                RemplirListeCmbDesOrigineScellesExistant();
                GetListLotmagasingeneral();
                GetLongueurIDScelleSelon();
                btn_ajout.IsEnabled = true;

              
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        List<CsActivite> lstActivite = (List<CsActivite>)this.Cbo_Activite.ItemsSource;
                        if (lstActivite != null)
                            Cbo_Activite.SelectedItem = lstActivite.FirstOrDefault(t => t.Activite_ID == ObjetSelectionnee.Activite_ID);

                        List<CsRefOrigineScelle> lstorg = (List<CsRefOrigineScelle>)this.Cbo_OrigneScelle.ItemsSource;
                        if (lstorg != null)
                            Cbo_OrigneScelle.SelectedItem = lstorg.FirstOrDefault(t => t.Origine_ID == ObjetSelectionnee.Origine_ID);
                        List<CsRefFournisseurs> lsfrs = (List<CsRefFournisseurs>)this.Cbo_frs.ItemsSource;
                        if (lsfrs != null)
                            Cbo_frs.SelectedItem = lsfrs.FirstOrDefault(t => t.Fournisseur_ID == ObjetSelectionnee.Fournisseur_ID);
                        List<CsCouleurActivite> Lscoul = (List<CsCouleurActivite>)this.Cbo_Couleurs.ItemsSource;
                        if (lsfrs != null)
                            Cbo_Couleurs.SelectedItem = Lscoul.FirstOrDefault(t => t.Couleur_ID == ObjetSelectionnee.Couleur_ID);
                        listSaisieScelleGrid.Clear();
                        listSaisieScelleGrid.Add(new CsLotMagasinGeneral
                        {
                            Numero_depart = ObjetSelectionnee.Numero_depart,
                            Numero_fin = ObjetSelectionnee.Numero_fin
                        });
                        dtgrdlReceptionScelle.ItemsSource = listSaisieScelleGrid;
                        btn_ajout.IsEnabled = false;

                        
                        
                      }
                 
                        
                 }
                
                //if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                //{
                //    AllInOne.ActivateControlsFromXaml(LayoutRoot,false);
                //    btn_ajout.IsEnabled = true;
                //}
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

        public bool ElementADelete(CsLotMagasinGeneral listSaisieScelleGrid)
        {
            try
            {
                if (listSaisieScelleGrid != null)
                {
                    var selected = listSaisieScelleGrid;

                    if (selected != null)
                    {
                        IScelleServiceClient delete = new IScelleServiceClient(Utility.Protocole(),
                                                                            Utility.EndPoint("Scelles"));
                        delete.DeleteLotMagasinGeneralCompleted += (del, argDel) =>
                        {
                            if (argDel.Cancelled || argDel.Error != null)
                            {
                                Message.ShowError(argDel.Error.Message, Languages.LibelleReceptionScelle);
                                return ;
                            }

                            if (argDel.Result == false)
                            {
                                Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.LibelleReceptionScelle);
                                return ;
                            }
                           //return argDel.Result;
                            
                        };


                        delete.DeleteLotMagasinGeneralAsync(selected);
                        
                    }
                    return true;
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                    //return false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
                return false;
            }
        }
    
        private void Translate()
        {
            try
            {
                Title = Languages.Scelles;
                //btnOk.Content = Languages.OK;
                //Btn_Reinitialiser.Content = Languages.Annuler;
                //GboCodeDepart.Header = Languages.InformationsCodePoste;
                //lab_Code.Content = Languages.Code;
                //lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeCmbDesActivitesExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeActiviteCompleted += (ssender, args) =>
                {   if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        this.Cbo_Activite.ItemsSource = args.Result;
                        this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
                        this.Cbo_Activite.SelectedValuePath = "Activite_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsActivite Activite in Cbo_Activite.ItemsSource)
                            {
                                if (Activite.Activite_ID == ObjetSelectionnee.Activite_ID)
                                {
                                    Cbo_Activite.SelectedItem = Activite;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.RetourneListeActiviteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeCmbDesFournisseursExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeFournisseursCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        this.Cbo_frs.ItemsSource = args.Result;
                        this.Cbo_frs.DisplayMemberPath = "Fournisseur_Libelle";
                        this.Cbo_frs.SelectedValuePath = "Fournisseur_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsRefFournisseurs frs in Cbo_frs.ItemsSource)
                            {
                                if (frs.Fournisseur_ID == ObjetSelectionnee.Fournisseur_ID)
                                {
                                    Cbo_frs.SelectedItem = frs;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.RetourneListeFournisseursAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeCmbDesOrigineScellesExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeOrigineScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        this.Cbo_OrigneScelle.ItemsSource = args.Result;
                        this.Cbo_OrigneScelle.DisplayMemberPath = "Origine_Libelle";
                        this.Cbo_OrigneScelle.SelectedValuePath = "Origine_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsRefOrigineScelle OgSll in Cbo_OrigneScelle.ItemsSource)
                            {
                                if (OgSll.Origine_ID == ObjetSelectionnee.Origine_ID)
                                {
                                    Cbo_OrigneScelle.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.RetourneListeOrigineScelleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeCmbCouleurExistant( int Activite_ID)
        {
            try
            {
                    if (lstCouleurActivite != null )
                    {
                        List<CsCouleurActivite> lstActiviteCouleur = new List<CsCouleurActivite>();
                        lstActiviteCouleur= lstCouleurActivite.Where(t => t.Activite_ID == Activite_ID).ToList();
                        this.Cbo_Couleurs.ItemsSource = lstCouleurActivite;
                        this.Cbo_Couleurs.DisplayMemberPath = "Couleur_libelle";
                        this.Cbo_Couleurs.SelectedValuePath = "Couleur_ID";

                        if (lstActiviteCouleur != null && lstActiviteCouleur.Count ==1)
                            this.Cbo_Couleurs.SelectedItem = lstActiviteCouleur.First();
                        
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValiderListeSaisieSelonDonnees(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        {
            try
            {
                if (ListeSaisie == null || ListeSaisie.Count == 0)
                {
                    return false;
                }

                bool result = true;
                string NumeroDebut = "";
                string NumeroFin = "";
                List<CsLotMagasinGeneral> LotsTemp;
                foreach (KeyValuePair<string, string> curItem in ListeSaisie)
                {
                    NumeroDebut = curItem.Key;
                    NumeroFin = curItem.Value;

                    //- Test numéro début

                    LotsTemp = ListLotMagasinGeneral.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroDebut))
                                                                     && (Int32.Parse(NumeroDebut) <= Int32.Parse(l.Numero_fin))
                                                                     && l.Origine_ID == OrigineLotsDeLaListe).ToList();
                    if (LotsTemp != null && LotsTemp.Count > 0)
                    {
                        result = false;
                        break;
                    }

                    //- Test numéro fin
                    LotsTemp = ListLotMagasinGeneral.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroFin))
                                                                     && (Int32.Parse(NumeroFin) <= Int32.Parse(l.Numero_fin))
                                                                     && l.Origine_ID == OrigineLotsDeLaListe).ToList();
                    if (LotsTemp != null && LotsTemp.Count > 0)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValiderNouveauxNumeroDuLotAModifierEnBase(string Id_LotAModifier, string newNumeroDebut, string newNumeroFin, int OrigineLot, ref string MessageErreur)
        {
            MessageErreur = "";

            if (string.IsNullOrEmpty(Id_LotAModifier) || string.IsNullOrEmpty(newNumeroDebut) || string.IsNullOrEmpty(newNumeroFin))
            {

                return false;
            }

            //- On crée une liste temporaire de travail
            //Dictionary<string, string> ListeTemp = new Dictionary<string, string>();
            //- Test numéro début
            bool result = true;
            string NumeroDebut = "";
            string NumeroFin = "";
            List<CsLotMagasinGeneral> LotsTemp;
            LotsTemp = ListLotMagasinGeneral.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroDebut))
                                                              && (Int32.Parse(NumeroDebut) <= Int32.Parse(l.Numero_fin))
                                                              && l.Origine_ID == OrigineLot).ToList();
            if (LotsTemp != null && LotsTemp.Count > 0)
            {
                result = false;

            }
            if (LotsTemp != null && LotsTemp.Count > 0)
            {
                return false;
            }

            //- Test numéro fin
            LotsTemp = ListLotMagasinGeneral.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroFin))
                                                             && (Int32.Parse(NumeroFin) <= Int32.Parse(l.Numero_fin))
                                                             && l.Origine_ID == OrigineLot).ToList();
            if (LotsTemp != null && LotsTemp.Count > 0)
            {
                result = false;

            }
            if (LotsTemp != null && LotsTemp.Count > 0)
            {
                MessageErreur = "Le numéro de scellé '" + newNumeroFin + "' est déjà inclus dans le lot '" + LotsTemp[0].Id_LotMagasinGeneral + "' en base";
                return false;
            }

            return true;
        }

      
        private void VerifierSaisie()
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                //    && CboCentre.SelectedItem != null)
                //    btnOk.IsEnabled = true;
                //else
                //{
                //    btnOk.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsLotMagasinGeneral> GetInformationsFromScreen()
        {
           
            try
            {
                var GENERALE = SessionObject.LstCentre.FirstOrDefault(c => c.CODE == SessionObject.Enumere.Generale);
                int IDMAGAZINGENERALE = GENERALE != null ? GENERALE.PK_ID : 0;
           
               var listObjetForInsertOrUpdate = new List<CsLotMagasinGeneral>();
                string NumeroDebut = "";
                string NumeroFin = "";
                int Origine_ID = ((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Origine_ID;
                //listSaisieScelleGrid = (List<CsLotMagasinGeneral>)dtgrdlReceptionScelle.ItemsSource;
                //if (listSaisieScelleGrid != null)
                ListSaisie.Clear();
                 ListSaisie.AddRange(((List<CsLotMagasinGeneral>)dtgrdlReceptionScelle.ItemsSource).Where(t=>!string.IsNullOrEmpty ( t.Numero_depart) && !string.IsNullOrEmpty(t.Numero_fin)));
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    if (ValidationDesNumerosSaisis(ref ListeNumerosSaisis, ListSaisie, Origine_ID))
                    {
                        foreach (CsLotMagasinGeneral element in ListSaisie)
                        {
                            NumeroDebut = element.Numero_depart.Trim().PadLeft(((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Longueur_ScelleID , '0');
                            NumeroFin = element.Numero_fin.Trim().PadLeft(((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Longueur_ScelleID , '0');

                            var LotMagasinGeneral = new CsLotMagasinGeneral
                            {
                                Id_LotMagasinGeneral = NumeroDebut + "_" + NumeroFin + "_" + Origine_ID.ToString(),
                                Origine_ID = ((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Origine_ID,
                                StatutLot_ID = 0,
                                Fournisseur_ID = ((CsRefFournisseurs)Cbo_frs.SelectedItem).Fournisseur_ID,
                                Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID,
                                Couleur_ID = ((CsCouleurActivite)Cbo_Couleurs.SelectedItem).Couleur_ID,
                                Matricule_AgentReception = Int32.Parse(UserConnecte.matricule),
                                DateReception = DateTime.Now,
                                Date_DerniereModif = DateTime.Now,
                                Numero_depart = NumeroDebut,
                                Numero_fin = NumeroFin,
                                Nbre_Scelles=int.Parse(NumeroFin) -int.Parse(NumeroDebut) +1,
                                CodeCentre = IDMAGAZINGENERALE
                            };
                            listObjetForInsertOrUpdate.Add(LotMagasinGeneral);
                           
                        }

                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    if (ValidationSaisie(ref ListeNumerosSaisis, ListSaisie, Origine_ID))
                    {
                        foreach (CsLotMagasinGeneral element in ListSaisie)
                        {
                            NumeroDebut = element.Numero_depart;
                            NumeroFin = element.Numero_fin;
                        //ObjetSelectionnee.Id_LotMagasinGeneral = NumeroDebut + "_" + NumeroFin + "_" + Origine_ID.ToString();
                        ObjetSelectionnee. Origine_ID = ((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Origine_ID;
                        ObjetSelectionnee.StatutLot_ID =0;
                        ObjetSelectionnee.  Fournisseur_ID = ((CsRefFournisseurs)Cbo_frs.SelectedItem).Fournisseur_ID;
                        ObjetSelectionnee.  Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID;
                        ObjetSelectionnee.  Couleur_ID = ((CsCouleurActivite)Cbo_Couleurs.SelectedItem).Couleur_ID;
                        ObjetSelectionnee. Matricule_AgentReception = Int32.Parse(UserConnecte.matricule);
                        ObjetSelectionnee. DateReception = DateTime.Now;
                        ObjetSelectionnee. Date_DerniereModif = DateTime.Now;
                        ObjetSelectionnee.   Numero_depart = NumeroDebut;
                        ObjetSelectionnee. Numero_fin = NumeroFin;
                        ObjetSelectionnee.Nbre_Scelles = int.Parse(NumeroFin) - int.Parse(NumeroDebut) +1 ;
                        ObjetSelectionnee.CodeCentre = IDMAGAZINGENERALE;
                        listObjetForInsertOrUpdate.Add(ObjetSelectionnee);

                        }
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Suppression )
                {
                    
                       
                }
                return listObjetForInsertOrUpdate;
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
                return null;
            }


 
        }

      

        private void GetDataNew()
        {
            //int back = 0;
            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllLotMagasinGeneralCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Commune);
                        //LoadingManager.EndLoading(back);
                        return;
                    }

                    donnesDatagrid.Clear();
                    if (args.Result != null)

                        foreach (var pLotmagasinGenerale in args.Result)
                        {
                            donnesDatagrid.Add(pLotmagasinGenerale);
                        }
                   // dtgrdlReceptionScelle
                    //DonnesDatagrid.OrderBy(p => p.PK_ID);
                    //DonnesDatagrid.Distinct();
                    //dtgrdlReceptionScelle.ItemsSource = DonnesDatagrid;
                    //LoadingManager.EndLoading(back);
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.SelectAllLotMagasinGeneralAsync();
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
                throw ex;
            }
        }

        private void UpdateParentList(CsLotMagasinGeneral pLotMagasinGeneral)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {

                    GetDataNew();
                    //donnesDatagrid.Add(pCommune);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {

                    GetDataNew();
                    //var commune = donnesDatagrid.First(p => p.PK_ID == pCommune.PK_ID);
                    //donnesDatagrid.Remove(commune);
                    //donnesDatagrid.Add(pCommune);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
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
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation ||
                            (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                        {
                            listForInsertOrUpdate = GetInformationsFromScreen();

                            if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                            {
                                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                                {

                                    if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                                    {
                                        service.InsertLotMagasinGeneralCompleted += (snder, insertR) =>
                                        {
                                            if (insertR.Cancelled ||
                                                insertR.Error != null)
                                            {
                                                Message.ShowError(insertR.Error.Message, Languages.Commune);
                                                return;
                                            }
                                            if (!insertR.Result)
                                            {
                                                Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Commune);
                                                return;
                                            }
                                            //UpdateParentList(listForInsertOrUpdate[0]);
                                            //OnEvent(null);
                                            Message.ShowInformation("Opération effectuée avec succès", Languages.Commune);
                                            this.dtgrdlReceptionScelle.ItemsSource = null;

                                            GetData();
                                        };
                                        service.InsertLotMagasinGeneralAsync(listForInsertOrUpdate);
                                    }
                                }
                                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                                {
                                    service.UpdateLotMagasinGeneralCompleted += (snder, UpdateR) =>
                                    {
                                        if (UpdateR.Cancelled ||
                                            UpdateR.Error != null)
                                        {
                                            Message.ShowError(UpdateR.Error.Message, Languages.Commune);
                                            return;
                                        }
                                        if (!UpdateR.Result)
                                        {
                                            Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Commune);
                                            return;
                                        }
                                        //UpdateParentList(listForInsertOrUpdate[0]);
                                        //OnEvent(null);
                                        //DialogResult = true;
                                        this.dtgrdlReceptionScelle.ItemsSource = null;
                                        Message.ShowInformation("Opération effectuée avec succès", Languages.Commune);
                                        GetData();
                                    };
                                    service.UpdateLotMagasinGeneralAsync(listForInsertOrUpdate);
                                }

                            }
                        }
                        else
                        {
                            if ((CsLotMagasinGeneral)this.dtgrdScelle.SelectedItem != null && (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Suppression)
                            {
                                service.DeleteLotMagasinGeneralCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Commune);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Commune);
                                        return;
                                    }
                                    this.dtgrdlReceptionScelle.ItemsSource = null;
                                    Message.ShowInformation("Opération effectuée avec succès", Languages.Commune);
                                    GetData();
                                };
                                service.DeleteLotMagasinGeneralAsync((CsLotMagasinGeneral)this.dtgrdScelle.SelectedItem);
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
                Message.Show(ex.Message, Languages.Commune);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Scelles);
            }
        }
       

        public  bool IsValideIDScelle(object candidat, int Origine_ID)
        {
            bool estscelle = false;

            try
            {
                if (candidat != null)
                {
                    string candidate = Convert.ToString(candidat);

                    int resultat = -1;
                   
                    int longueur = ListOrigineScelle.FirstOrDefault(c => c.Origine_ID == (int)Origine_ID).Longueur_ScelleID;
                    estscelle = (candidate.Length == longueur && int.TryParse(candidate, out resultat));
                    if (!estscelle )
                    Message.Show("La taille du scelle ne correspond pas au paramétrage", "Scelle");
                }
                return estscelle;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Scelles);
                
                throw ex;
            }
        }

    
        public void GetLongueurIDScelleSelon()
        {

            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeOrigineScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    ListOrigineScelle = args.Result;

                    ////LoadingManager.EndLoading(back);
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.RetourneListeOrigineScelleAsync();
              

            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
                throw ex;

                Message.ShowError(ex.Message, Languages.Scelles);

            }

        }

        List<CsCouleurActivite> lstCouleurActivite = new List<CsCouleurActivite>();
        public void RetourneListeAllCouleurScelle()
        {

            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListeAllCouleurScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    lstCouleurActivite = args.Result;
                };
                client.RetourneListeAllCouleurScelleAsync();


            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Scelles);

            }

        }

        public void GetListLotmagasingeneral()
        {

            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllLotMagasinGeneralCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    ListLotMagasinGeneral = args.Result;

                    ////LoadingManager.EndLoading(back);
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.SelectAllLotMagasinGeneralAsync();


            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
                throw ex;

                Message.ShowError(ex.Message, Languages.Scelles);

            }

        }
        public bool ValidationDesNumerosSaisis(ref Dictionary<string, string> ListeNumerosSaisis, List<CsLotMagasinGeneral> listSaisieScelleGrid, int OrigineDesScelles_ID)
        {
            bool result = true;
            string strDebutFormate = string.Empty;
            string strFinFormate = string.Empty;
            string strDebut = string.Empty;
            string strFin = string.Empty;
            int intNumeroDebut = 0;
            int intNumeroFin = 0;
            try
            {
                int LongeurIDScelle = ListOrigineScelle.FirstOrDefault(c => c.Origine_ID == (int)OrigineDesScelles_ID).Longueur_ScelleID;
                if (ListeNumerosSaisis == null)
                    ListeNumerosSaisis = new Dictionary<string, string>();
                else
                    ListeNumerosSaisis.Clear();


                // - Gestion de tous les éléments sauf le dernier
                for (int i = 0; i < listSaisieScelleGrid.Count ; i++)
                {
                    strDebutFormate = string.Empty;
                    strFinFormate = string.Empty;

                    strDebut = listSaisieScelleGrid[i].Numero_depart.Trim();
                    strFin = listSaisieScelleGrid[i].Numero_fin.Trim();

                    //- Vérifier que des données ont été saisies au niveau des cellules
                    if (string.IsNullOrEmpty(strDebut) || string.IsNullOrEmpty(strFin))
                    {

                        if (i == (listSaisieScelleGrid.Count - 1)
                            && (string.IsNullOrEmpty(strDebut) && string.IsNullOrEmpty(strFin)))
                        {
                            //  - Il s'agit de la dernière ligne. On n'est pas en erreur si aucune des cellules n'est renseignée
                            //  - Il s'agit d'une ligne vide en ajout placée automatiquement dans le DataGrid
                            return false;
                        }

                        break;
                    }

                    //- Vérifier que les informations saisies sont des numéros de scellés valides
                    //if (!ScellePresenter.IsValideIDScelle(strDebut.PadLeft(Constantes.Longueur_ScelleID, '0')))
                    if (!IsValideIDScelle(strDebut.PadLeft(LongeurIDScelle, '0'), OrigineDesScelles_ID))
                    {
                        result = false;
                        break;
                    }
                    //- Vérifier que le numéro de début est inférieur au numéro de fin
                    intNumeroDebut = int.Parse(strDebut);
                    intNumeroFin = int.Parse(strFin);
                    if (intNumeroFin <= intNumeroDebut)
                    {
                        Message.Show("Le numéro de début est supérieur au numéro de fin", "Scelle");
                        result = false;
                        break;
                    }

                    //- Vérifier que les numéros saisis ne sont pas inclus dans des lots déjà saisis
                    if (ListeNumerosSaisis.Count(s => (Int32.Parse(s.Key) <= intNumeroDebut) && (intNumeroDebut <= Int32.Parse(s.Value))) > 0)
                    {
                        Message.Show("Le numéro saisi existe déja dans un lot", "Scelle");
                        result = false;
                        break;
                    }
                    if (ListeNumerosSaisis.Count(s => (Int32.Parse(s.Key) <= intNumeroFin) && (intNumeroFin <= Int32.Parse(s.Value))) > 0)
                    {
                        Message.Show("Le numéro saisi existe déja dans un lot", "Scelle");
                        result = false;
                        break;
                    }

                    ListeNumerosSaisis.Add(strDebut.PadLeft(LongeurIDScelle, '0'), strFin.PadLeft(LongeurIDScelle, '0'));
                }
                if (!result)
                    return false;

                if (ListeNumerosSaisis.Count < (listSaisieScelleGrid.Count - 1))
                {
                    return false;
                }

                //- Vérifier les éléments saisis avec les lots en base
                return ValiderListeSaisieSelonDonnees(ListeNumerosSaisis, OrigineDesScelles_ID);
            }
            catch (Exception ex)
            {
                throw ex;
              Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);

            }
       }
        public bool ValidationSaisie(ref Dictionary<string, string> ListeNumerosSaisis, List<CsLotMagasinGeneral> listSaisieScelleGrid, int OrigineDesScelles_ID)
        {
            bool result = true;
            string strDebutFormate = string.Empty;
            string strFinFormate = string.Empty;
            string strDebut = string.Empty;
            string strFin = string.Empty;
            int intNumeroDebut = 0;
            int intNumeroFin = 0;
            try
            {
                int LongeurIDScelle = ListOrigineScelle.FirstOrDefault(c => c.Origine_ID == (int)OrigineDesScelles_ID).Longueur_ScelleID;
                if (ListeNumerosSaisis == null)
                    ListeNumerosSaisis = new Dictionary<string, string>();
                else
                    ListeNumerosSaisis.Clear();


                // - Gestion de tous les éléments sauf le dernier
                for (int i = 0; i < listSaisieScelleGrid.Count; i++)
                {
                    strDebutFormate = string.Empty;
                    strFinFormate = string.Empty;

                    strDebut = listSaisieScelleGrid[i].Numero_depart.Trim();
                    strFin = listSaisieScelleGrid[i].Numero_fin.Trim();

                    //- Vérifier que des données ont été saisies au niveau des cellules
                    if (string.IsNullOrEmpty(strDebut) || string.IsNullOrEmpty(strFin))
                    {

                        if (i == (listSaisieScelleGrid.Count - 1)
                            && (string.IsNullOrEmpty(strDebut) && string.IsNullOrEmpty(strFin)))
                        {
                            //  - Il s'agit de la dernière ligne. On n'est pas en erreur si aucune des cellules n'est renseignée
                            //  - Il s'agit d'une ligne vide en ajout placée automatiquement dans le DataGrid
                            return false;
                        }

                        break;
                    }

                    //- Vérifier que les informations saisies sont des numéros de scellés valides
                    //if (!ScellePresenter.IsValideIDScelle(strDebut.PadLeft(Constantes.Longueur_ScelleID, '0')))
                    if (!IsValideIDScelle(strDebut.PadLeft(LongeurIDScelle, '0'), OrigineDesScelles_ID))
                    {
                        result = false;
                        break;
                    }
                    //if (!ScellePresenter.IsValideIDScelle(strFin.PadLeft(Constantes.Longueur_ScelleID, '0')))
                    if (!IsValideIDScelle(strFin.PadLeft(LongeurIDScelle, '0'), OrigineDesScelles_ID))
                    {
                        result = false;
                        break;
                    }
                    //- Vérifier que le numéro de début est inférieur au numéro de fin
                    intNumeroDebut = int.Parse(strDebut);
                    intNumeroFin = int.Parse(strFin);
                    if (intNumeroFin <= intNumeroDebut)
                    {
                        result = false;
                        break;
                    }

                    //- Vérifier que les numéros saisis ne sont pas inclus dans des lots déjà saisis
                    if (ListeNumerosSaisis.Count(s => (Int32.Parse(s.Key) <= intNumeroDebut) && (intNumeroDebut <= Int32.Parse(s.Value))) > 0)
                    {
                        result = false;
                        break;
                    }
                    if (ListeNumerosSaisis.Count(s => (Int32.Parse(s.Key) <= intNumeroFin) && (intNumeroFin <= Int32.Parse(s.Value))) > 0)
                    {
                        result = false;
                        break;
                    }

                    ListeNumerosSaisis.Add(strDebut.PadLeft(LongeurIDScelle, '0'), strFin.PadLeft(LongeurIDScelle, '0'));
                }
                if (!result)
                    return false;

                if (ListeNumerosSaisis.Count < (listSaisieScelleGrid.Count - 1))
                {
                    return false;
                }

                //- Vérifier les éléments saisis avec les lots en base
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);

            }
        }

      
   

        //public bool ValiderSaisieAvecLotsEnBase(Dictionary<string, string> lesLots, int OrigineLots)
        //{

        //    try
        //    {

        //        //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
        //       // LayoutRoot.Cursor = Cursors.Wait;
        //        IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
        //        client.ValiderListeSaisieSelonDonneesEnBaseCompleted += (ssender, args) =>
        //        {
        //            if (args.Cancelled || args.Error != null)
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                string error = args.Error.Message;
        //                Message.Show(error, Languages.Parametrage);
        //                //LoadingManager.EndLoading(back);
        //                return;
        //            }
        //            if (args.Result == null)
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
        //                //LoadingManager.EndLoading(back);
        //                return;
        //            }
        //            string NumeroDebut = "";
        //            string NumeroFin = "";
        //            int Origine_ID = ((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Origine_ID;

        //            resultat = args.Result;
        //            if(resultat==true)

        //            {
                       
        //                }
        //              }
        //            ////LoadingManager.EndLoading(back);
        //            //LayoutRoot.Cursor = Cursors.Arrow;

        //        };
        //        client.ValiderListeSaisieSelonDonneesEnBaseAsync(lesLots, OrigineLots);
        //        return resultat;

        //    }
        //    catch (Exception ex)
        //    {
        //        //LayoutRoot.Cursor = Cursors.Arrow;
        //        ////LoadingManager.EndLoading(back);
        //        throw ex;
        //        return false;

        //    }
        //    return false;
        //}
   
        private void dtgrdlReceptionScelle_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);
            }
        }

       
        private void Cbo_Activite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID;
                RemplirListeCmbCouleurExistant(Activite_ID);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void dtgrdlReceptionScelle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdlReceptionScelle.SelectedItem as CsLotMagasinGeneral;
                SessionObject.objectSelected = dtgrdlReceptionScelle.SelectedItem as CsLotMagasinGeneral;
                SessionObject.gridUtilisateur = dtgrdlReceptionScelle;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        List<CsLotMagasinGeneral> listnew = new List<CsLotMagasinGeneral>();
        private void btn_ajout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ////donnesDatagrid.Add((List<CsLotMagasinGeneral>)dtgrdlReceptionScelle.ItemsSource);
                //var obj=(List<CsLotMagasinGeneral>)dtgrdlReceptionScelle.ItemsSource;
                //if (dtgrdlReceptionScelle.ItemsSource != null && obj.Count() > 0)
                //{
                //    //ListSaisie.Clear();
                //    listnew.AddRange((List<CsLotMagasinGeneral>)dtgrdlReceptionScelle.ItemsSource);
                //}
                //listnew.Add(new CsLotMagasinGeneral
                //{
                //    Numero_depart = " ",
                //    Numero_fin = " "
                //});



                if (!string.IsNullOrEmpty(this.txt_NumeroDepart.Text) &&
                    !string.IsNullOrEmpty(this.txt_NumeroFin.Text) &&
                    Cbo_OrigneScelle.SelectedItem != null && Cbo_frs.SelectedItem != null &&  Cbo_Couleurs.SelectedItem != null)
                {
                    int  Origine_ID = ((CsRefOrigineScelle)Cbo_OrigneScelle.SelectedItem).Origine_ID;
                    int Fournisseur_ID = ((CsRefFournisseurs)Cbo_frs.SelectedItem).Fournisseur_ID;
                    int Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID;
                    int Couleur_ID = ((CsCouleurActivite)Cbo_Couleurs.SelectedItem).Couleur_ID;
                    List<int> numScelle = new List<int>();
                    listnew.Add(new CsLotMagasinGeneral
                    {
                        Numero_depart = this.txt_NumeroDepart.Text,
                        Numero_fin = this.txt_NumeroFin.Text
                    });
                }

                dtgrdlReceptionScelle.ItemsSource = null;
                dtgrdlReceptionScelle.ItemsSource = listnew;
                txt_NumeroDepart.Text = string.Empty;
                txt_NumeroFin.Text = string.Empty;
               // videChamps();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btn_Modifier_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dtgrdScelle.SelectedItem != null)
                {
                    ObjetSelectionnee = (CsLotMagasinGeneral)this.dtgrdScelle.SelectedItem;
                    if (ObjetSelectionnee.StatutLot_ID != 0)
                    {
                        Message.ShowInformation("Ce lot ne peut ètre modifier ", "info");
                        ObjetSelectionnee = null;
                        return;
                    }
                    ModeExecution = SessionObject.ExecMode.Modification;
                    if (SessionObject.LstDesCouleur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Couleurs.ItemsSource = SessionObject.LstDesCouleur;
                            this.Cbo_Couleurs.DisplayMemberPath = "Couleur_libelle";
                            this.Cbo_Couleurs.SelectedValuePath = "Couleur_ID";

                            foreach (CsCouleurActivite OgSll in Cbo_Couleurs.ItemsSource)
                            {
                                if (OgSll.Couleur_ID == ObjetSelectionnee.Couleur_ID)
                                {
                                    Cbo_Couleurs.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }
                    }
                    if (SessionObject.LstDesFournisseur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_frs.ItemsSource = SessionObject.LstDesFournisseur;
                            this.Cbo_frs.DisplayMemberPath = "Fournisseur_Libelle";
                            this.Cbo_frs.SelectedValuePath = "Fournisseur_ID";
                            foreach (CsRefFournisseurs frs in Cbo_frs.ItemsSource)
                            {
                                if (frs.Fournisseur_ID == ObjetSelectionnee.Fournisseur_ID)
                                {
                                    Cbo_frs.SelectedItem = frs;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesOrigineScelle.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_OrigneScelle.ItemsSource = SessionObject.LstDesOrigineScelle;
                            this.Cbo_OrigneScelle.DisplayMemberPath = "Origine_Libelle";
                            this.Cbo_OrigneScelle.SelectedValuePath = "Origine_ID";
                            foreach (CsRefOrigineScelle OgSll in Cbo_OrigneScelle.ItemsSource)
                            {
                                if (OgSll.Origine_ID == ObjetSelectionnee.Origine_ID)
                                {
                                    Cbo_OrigneScelle.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesActivitee.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Activite.ItemsSource = SessionObject.LstDesActivitee;
                            this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
                            this.Cbo_Activite.SelectedValuePath = "Activite_ID";
                            foreach (CsActivite Activite in Cbo_Activite.ItemsSource)
                            {
                                if (Activite.Activite_ID == ObjetSelectionnee.Activite_ID)
                                {
                                    Cbo_Activite.SelectedItem = Activite;
                                    break;
                                }
                            }
                        }
                    }
                    List<CsLotMagasinGeneral> lstLotScelle = new List<CsLotMagasinGeneral>();
                    //lstLotScelle.Add(new CsLotMagasinGeneral
                    //{
                    //    Numero_depart = ObjetSelectionnee.Numero_depart,
                    //    Numero_fin = ObjetSelectionnee.Numero_fin

                    //});
                    txt_NumeroDepart.Text = ObjetSelectionnee.Numero_depart;
                    txt_NumeroFin.Text = ObjetSelectionnee.Numero_fin;

                    dtgrdlReceptionScelle.ItemsSource = lstLotScelle;
                    //btn_ajout.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des données", "Erreur");
            }
        }

        private void btn_Supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dtgrdScelle.SelectedItem != null)
                {
                    ObjetSelectionnee = (CsLotMagasinGeneral)this.dtgrdScelle.SelectedItem;
                    if (ObjetSelectionnee.StatutLot_ID != 0)
                    {
                        Message.ShowInformation("Ce lot ne peut ètre modifier ", "info");
                        ObjetSelectionnee = null;
                        return;
                    }
                    ModeExecution = SessionObject.ExecMode.Suppression ;
                    if (SessionObject.LstDesCouleur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Couleurs.ItemsSource = SessionObject.LstDesCouleur;
                            this.Cbo_Couleurs.DisplayMemberPath = "Couleur_libelle";
                            this.Cbo_Couleurs.SelectedValuePath = "Couleur_ID";

                            foreach (CsCouleurActivite OgSll in Cbo_Couleurs.ItemsSource)
                            {
                                if (OgSll.Couleur_ID == ObjetSelectionnee.Couleur_ID)
                                {
                                    Cbo_Couleurs.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }
                    }
                    if (SessionObject.LstDesFournisseur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_frs.ItemsSource = SessionObject.LstDesFournisseur;
                            this.Cbo_frs.DisplayMemberPath = "Fournisseur_Libelle";
                            this.Cbo_frs.SelectedValuePath = "Fournisseur_ID";
                            foreach (CsRefFournisseurs frs in Cbo_frs.ItemsSource)
                            {
                                if (frs.Fournisseur_ID == ObjetSelectionnee.Fournisseur_ID)
                                {
                                    Cbo_frs.SelectedItem = frs;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesOrigineScelle.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_OrigneScelle.ItemsSource = SessionObject.LstDesOrigineScelle;
                            this.Cbo_OrigneScelle.DisplayMemberPath = "Origine_Libelle";
                            this.Cbo_OrigneScelle.SelectedValuePath = "Origine_ID";
                            foreach (CsRefOrigineScelle OgSll in Cbo_OrigneScelle.ItemsSource)
                            {
                                if (OgSll.Origine_ID == ObjetSelectionnee.Origine_ID)
                                {
                                    Cbo_OrigneScelle.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesActivitee.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Activite.ItemsSource = SessionObject.LstDesActivitee;
                            this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
                            this.Cbo_Activite.SelectedValuePath = "Activite_ID";
                            foreach (CsActivite Activite in Cbo_Activite.ItemsSource)
                            {
                                if (Activite.Activite_ID == ObjetSelectionnee.Activite_ID)
                                {
                                    Cbo_Activite.SelectedItem = Activite;
                                    break;
                                }
                            }
                        }
                    }
                    List<CsLotMagasinGeneral> lstLotScelle = new List<CsLotMagasinGeneral>();
                    lstLotScelle.Add(new CsLotMagasinGeneral
                    {
                        Numero_depart = ObjetSelectionnee.Numero_depart,
                        Numero_fin = ObjetSelectionnee.Numero_fin
                    });
                    dtgrdlReceptionScelle.ItemsSource = lstLotScelle;
                    btn_ajout.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des données", "Erreur");
            }
        }

        private void dtgrdlReceptionScelle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dtgrdlReceptionScelle.SelectedItem != null)
                {
                    ObjetSelectionnee = (CsLotMagasinGeneral)this.dtgrdScelle.SelectedItem;
                    if (ObjetSelectionnee.StatutLot_ID != 0)
                    {
                        Message.ShowInformation("Ce lot ne peut ètre modifier ", "info");
                        ObjetSelectionnee = null;
                        return;
                    }
                    ModeExecution = SessionObject.ExecMode.Modification;
                    if (SessionObject.LstDesCouleur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Couleurs.ItemsSource = SessionObject.LstDesCouleur;
                            this.Cbo_Couleurs.DisplayMemberPath = "Couleur_libelle";
                            this.Cbo_Couleurs.SelectedValuePath = "Couleur_ID";

                            foreach (CsCouleurActivite OgSll in Cbo_Couleurs.ItemsSource)
                            {
                                if (OgSll.Couleur_ID == ObjetSelectionnee.Couleur_ID)
                                {
                                    Cbo_Couleurs.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }
                    }
                    if (SessionObject.LstDesFournisseur.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_frs.ItemsSource = SessionObject.LstDesFournisseur;
                            this.Cbo_frs.DisplayMemberPath = "Fournisseur_Libelle";
                            this.Cbo_frs.SelectedValuePath = "Fournisseur_ID";
                            foreach (CsRefFournisseurs frs in Cbo_frs.ItemsSource)
                            {
                                if (frs.Fournisseur_ID == ObjetSelectionnee.Fournisseur_ID)
                                {
                                    Cbo_frs.SelectedItem = frs;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesOrigineScelle.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_OrigneScelle.ItemsSource = SessionObject.LstDesOrigineScelle;
                            this.Cbo_OrigneScelle.DisplayMemberPath = "Origine_Libelle";
                            this.Cbo_OrigneScelle.SelectedValuePath = "Origine_ID";
                            foreach (CsRefOrigineScelle OgSll in Cbo_OrigneScelle.ItemsSource)
                            {
                                if (OgSll.Origine_ID == ObjetSelectionnee.Origine_ID)
                                {
                                    Cbo_OrigneScelle.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        }

                    }
                    if (SessionObject.LstDesActivitee.Count != 0)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            this.Cbo_Activite.ItemsSource = SessionObject.LstDesActivitee;
                            this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
                            this.Cbo_Activite.SelectedValuePath = "Activite_ID";
                            foreach (CsActivite Activite in Cbo_Activite.ItemsSource)
                            {
                                if (Activite.Activite_ID == ObjetSelectionnee.Activite_ID)
                                {
                                    Cbo_Activite.SelectedItem = Activite;
                                    break;
                                }
                            }
                        }
                    }
                    List<CsLotMagasinGeneral> lstLotScelle = new List<CsLotMagasinGeneral>();
                    lstLotScelle.Add(new CsLotMagasinGeneral
                    {
                        Numero_depart = ObjetSelectionnee.Numero_depart,
                        Numero_fin = ObjetSelectionnee.Numero_fin

                    });
                    txt_NumeroDepart.Text = ObjetSelectionnee.Numero_depart;
                    txt_NumeroFin.Text = ObjetSelectionnee.Numero_fin;

                    dtgrdlReceptionScelle.ItemsSource = lstLotScelle;
                    btn_ajout.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des données", "Erreur");
            }



        }

        public void videChamps()
        {
            txt_NumeroDepart.Text = string.Empty;
            txt_NumeroFin.Text = string.Empty;
            Cbo_Activite.SelectedItem = null;
            Cbo_Couleurs.SelectedItem = null;
            Cbo_frs.SelectedItem = null;
            Cbo_OrigneScelle.SelectedItem = null;
           
        
        }

     

    }
}

