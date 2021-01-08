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
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;


namespace Galatee.Silverlight.Parametrage
{
    public partial class UcMarqueModel : ChildWindow
    {
        List<CsMarque_Modele> ListdesModelesfonctMarq = new List<CsMarque_Modele>();
        private CsMarque_Modele ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsMarque_Modele> listForInsertOrUpdate = new List<CsMarque_Modele>();
        List<CsMarque_Modele> listInitial = new List<CsMarque_Modele>();
        public UcMarqueModel(List<CsMarque_Modele> lstInit)
        {
            InitializeComponent();
            RemplirListeCmbDesModelesMarqueExistant();
            ChargerProduit();
            ChargerMarque();
            ChargerModele();
            listInitial = lstInit;
        }
        public UcMarqueModel(List<CsMarque_Modele> lstInit,CsMarque_Modele laMarqueModel)
        {
            InitializeComponent();
            RemplirListeCmbDesModelesMarqueExistant();
            ChargerProduit();
            ChargerMarque();
            ChargerModele();
            ObjetSelectionnee = laMarqueModel;
        }
        public UcMarqueModel(CsMarque_Modele pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                var MarqueModele = new CsMarque_Modele();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(MarqueModele, pObject as CsMarque_Modele);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                // dtgrdlReceptionScelle.ItemsSource = donnesDatagrid;
                RemplirListeCmbDesModelesMarqueExistant();
                ChargerProduit();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                      
                        List<CsMarque_Modele> lstMaqmMdt = ListdesModelesfonctMarq;
                        if (lstMaqmMdt != null)
                        {
                            Cbo_Modele.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MODELE_ID  == ObjetSelectionnee.MODELE_ID);
                            Cbo_Marque.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MARQUE_ID == ObjetSelectionnee.MARQUE_ID);
                        }
                        txt_NombreScelleCache.Text = ObjetSelectionnee.Nbre_scel_cache.ToString();
                        txt_NombreScelleCache.Text = ObjetSelectionnee.Nbre_scel_capot.ToString();
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

        private void RemplirListeCmbDesModelesMarqueExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceParametrage.ParametrageClient client = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMarqueModeleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees,Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    else
                    {
                        ListdesModelesfonctMarq = args.Result;

                    }
                };
                client.SelectAllMarqueModeleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                {
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque != null && SessionObject.LstMarque.Count != 0)
                {
                    Cbo_Marque.ItemsSource = null;
                    Cbo_Marque.DisplayMemberPath = "LIBELLE";
                    Cbo_Marque.ItemsSource = SessionObject.LstMarque;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneToutMarqueCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstMarque = res.Result;
                    Cbo_Marque.ItemsSource = null;
                    Cbo_Marque.DisplayMemberPath = "LIBELLE";
                    Cbo_Marque.ItemsSource = SessionObject.LstMarque;
                };
                service1.RetourneToutMarqueAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerModele()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllModelCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CoperDemande);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_Modele.ItemsSource = null;
                        Cbo_Modele.DisplayMemberPath = "Libelle_Modele";
                        Cbo_Modele.ItemsSource = args.Result;
                    }
                };
                client.SelectAllModelAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsMarque_Modele > GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsMarque_Modele>();
            try
            {

                if (ObjetSelectionnee == null )
                {

                    var tCompteur = new CsMarque_Modele
                        {
                            MARQUE_ID = ((ServiceAccueil.CsMarqueCompteur )Cbo_Marque.SelectedItem).PK_ID ,
                            Libelle_MArque = ((ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).LIBELLE ,
                            MODELE_ID = ((CsMarque_Modele)Cbo_Modele.SelectedItem).MODELE_ID,
                            Libelle_Modele = ((CsMarque_Modele)Cbo_Modele.SelectedItem).Libelle_Modele,
                            Produit_ID = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID,
                            Libelle_Produit  = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).LIBELLE ,
                            Nbre_scel_capot =int.Parse(this.txt_nbreScelleCapot.Text),
                            Nbre_scel_cache = int.Parse(this.txt_NombreScelleCache.Text) ,
                            PK_ID = Guid.NewGuid()
                        };
                        listObjetForInsertOrUpdate.Add(tCompteur);
                }

                if (ObjetSelectionnee != null)
                {
                    ObjetSelectionnee.MARQUE_ID = ((ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).PK_ID ;
                    ObjetSelectionnee.Libelle_MArque = ((ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).LIBELLE ;
                    ObjetSelectionnee.MODELE_ID = ((CsMarque_Modele)Cbo_Modele.SelectedItem).MODELE_ID;
                    ObjetSelectionnee.Libelle_Modele = ((CsMarque_Modele)Cbo_Modele.SelectedItem).Libelle_Modele;
                    ObjetSelectionnee.Produit_ID = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                    ObjetSelectionnee.Libelle_Produit  = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).LIBELLE ;
                    ObjetSelectionnee.Nbre_scel_capot =int.Parse(this.txt_nbreScelleCapot.Text);
                    ObjetSelectionnee.Nbre_scel_cache = int.Parse(this.txt_NombreScelleCache.Text);
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.EtatDuCompteur);
                return null;
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

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if (ObjetSelectionnee == null )
                            {
                                if(listInitial != null && listInitial.FirstOrDefault(t=>t.Produit_ID ==listForInsertOrUpdate.First().Produit_ID  &&  t.MARQUE_ID == listForInsertOrUpdate.First().MARQUE_ID && t.MODELE_ID ==listForInsertOrUpdate.First().MARQUE_ID )!= null )
                                {
                                    Message.ShowWarning("Ce model a deja été paramétré", "Parametrage");
                                    listForInsertOrUpdate.Clear();
                                    return ;
                                }
                                if (listForInsertOrUpdate.First().Nbre_scel_cache > 3 || listForInsertOrUpdate.First().Nbre_scel_capot > 3)
                                {
                                    Message.ShowWarning("Le nombre de scellé ne dois pas exeder 3", "Parametrage");
                                    listForInsertOrUpdate.Clear();
                                    return;
                                }
                                service.InsertMarqueModeleCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                       insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    this.DialogResult = false;
                                };
                                service.InsertMarqueModeleAsync(listForInsertOrUpdate);
                               }

                            if (ObjetSelectionnee != null)
                            {
                                service.UpdateMarqueModeleCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    this.DialogResult = false;
                                };
                                service.UpdateMarqueModeleAsync(listForInsertOrUpdate);
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
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

    }
}

