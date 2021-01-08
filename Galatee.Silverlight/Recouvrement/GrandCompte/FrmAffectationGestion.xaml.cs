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
using Galatee.Silverlight;
using System.IO;
using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.Tarification.Helper;
using Galatee.Silverlight.Classes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAffectationGestion : ChildWindow
    {

        #region Variables

        List<CsUtilisateur> donnesDatagrid = new List<CsUtilisateur>();
        List<ServiceRecouvrement.CsRegCli> ListRegCliAffecter_Selectionner = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsRegCli> ListRegCliAffecter_NonSelectionner = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>();


        #endregion

        #region Contructeurs

        public FrmAffectationGestion()
        {
            InitializeComponent();

            btn_affecter.Content = "<";
            RemplirCodeRegroupement();
            RetourneListeAllUser();
            RemplirAffectation();
        }

        #endregion

        #region Even Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<Galatee.Silverlight.ServiceRecouvrement.CsRegCli> ListRegCliAffecter = ((List<Galatee.Silverlight.ServiceRecouvrement.CsRegCli>)dgListeFraixParicipation.ItemsSource);
            var UtilisateurSelect = (int)this.txt_Gestionnaire.Tag ;
            SaveAffection(ListRegCliAffecter, UtilisateurSelect);
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var regcli = ((List<ServiceRecouvrement.CsRegCli>)dgListeFraixParicipation.ItemsSource);
            dgListeFraixParicipation.SelectedItems.Clear();

            foreach (var item in ListRegCliAffecter_Selectionner)
            {
                dgListeFraixParicipation.SelectedItems.Add(item);
            }
            ListRegCliAffecter_Selectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<ServiceRecouvrement.CsRegCli>(dgListeFraixParicipation, dgListeFraixParicipation_Copy);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListRegCliAffecter_Selectionner.Add((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation.SelectedItem);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListRegCliAffecter_Selectionner.Remove((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation.SelectedItem);
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            ListRegCliAffecter_NonSelectionner.Add((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation_Copy.SelectedItem);
        }
        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            ListRegCliAffecter_NonSelectionner.Remove((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation_Copy.SelectedItem);
        }

        private void btn_affecter_Click(object sender, RoutedEventArgs e)
        {
            dgListeFraixParicipation_Copy.SelectedItems.Clear();
            foreach (var item in ListRegCliAffecter_NonSelectionner)
            {
                List<string> lesIdCentre = new List<string>();
                if (this.Txt_LibelleCentre.Tag != null)
                {
                    lesIdCentre.AddRange((List<string>)this.Txt_LibelleCentre.Tag);
                    item.LstCentre = lesIdCentre;
                }
                dgListeFraixParicipation_Copy.SelectedItems.Add(item);
            }
            ListRegCliAffecter_NonSelectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<ServiceRecouvrement.CsRegCli>(dgListeFraixParicipation_Copy, dgListeFraixParicipation);
        }

        private void cbo_utilisateurGestionnaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReLoadingGrid();
        }

        #endregion

        #region Services
        List<CsUtilisateur> LstUser = new List<CsUtilisateur>();
        public void RetourneListeAllUser()
        {
            try
            {

                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }

                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<int> lstCentreHabil = new List<int>();
                    foreach (var item in lstCentreProfil)
                        lstCentreHabil.Add(item.PK_ID);
                    List<CsUtilisateur> _lstUserProfil = res.Result.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList();
                    donnesDatagrid = _lstUserProfil;

                    var listeTemp = (from t in donnesDatagrid
                                     select new
                                     {
                                         t.PK_ID,
                                         t.CENTREAFFICHER,
                                         t.CENTRE,
                                         t.MATRICULE,
                                         t.LOGINNAME,
                                         t.LIBELLE,
                                         t.LIBELLESTATUSCOMPTE,
                                         t.LIBELLEPERIMETREACTION
                                     }).Distinct().ToList();
                    foreach (var t in listeTemp)
                    {
                        CsUtilisateur user = new CsUtilisateur();
                        user.PK_ID = t.PK_ID;
                        user.CENTREAFFICHER = t.CENTREAFFICHER;
                        user.CENTRE = t.CENTRE;
                        user.MATRICULE = t.MATRICULE;
                        user.LOGINNAME = t.LOGINNAME;
                        user.LIBELLE = t.LIBELLE;
                        user.LIBELLESTATUSCOMPTE = t.LIBELLESTATUSCOMPTE;
                        user.LIBELLEPERIMETREACTION = t.LIBELLEPERIMETREACTION;
                        LstUser.Add(user);
                    }
                    //cbo_utilisateurGestionnaire.SelectedValue = "PK_ID";
                    //cbo_utilisateurGestionnaire.DisplayMemberPath = "LIBELLE";
                    //cbo_utilisateurGestionnaire.ItemsSource = LstUser.OrderBy(t=>t.LIBELLE);

                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void RemplirCodeRegroupement()
        {
            try
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LstCodeRegroupement = args.Result;

                    return;
                };
                service.RetourneCodeRegroupementAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void RemplirAffectation()
        {
            try
            {
                if (LstAffectation.Count != 0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    service.RemplirAffectationCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstAffectation = args.Result;

                        return;
                    };
                    service.RemplirAffectationAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void SaveAffection(List<Galatee.Silverlight.ServiceRecouvrement.CsRegCli> ListRegCliAffecter, int? ID_USER)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SaveAffectionCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == false)
                {
                    Message.ShowInformation("L'affectation ne s'est pas correctement effectuée veuillez reprendre l'opération", "Recouvrement");
                }
                Message.ShowInformation("Affectation effectuée avec succès", "Recouvrement");

                return;
            };
            service.SaveAffectionAsync(ListRegCliAffecter, ID_USER);
        }
        #endregion

        #region Methodes

        private void LoadGrid(List<Galatee.Silverlight.ServiceRecouvrement.CsRegCli> lst, DataGrid dg)
        {
            dg.ItemsSource = lst;
        }
        private void ReLoadingGrid()
        {
            var UtilisateurSelect = (int)this.txt_Gestionnaire.Tag ;
            var Affectation = LstAffectation.Where(a => a.FK_IDADMUTILISATEUR == UtilisateurSelect && a.ISACTIVE == true);

            if (Affectation != null)
            {
                var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGROUPEMENT);
                var RegCliNonAffecter = LstCodeRegroupement.Where(a => !ListIdRegcliCorrespondant.Contains(a.PK_ID)).ToList();

                var RegCliAffecter = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();

                LoadGrid(RegCliAffecter, this.dgListeFraixParicipation);
                LoadGrid(RegCliNonAffecter, this.dgListeFraixParicipation_Copy);
            }
        }

        #endregion

        private void btn_Gestionnaire_Click(object sender, RoutedEventArgs e)
        {
            if (LstUser != null && LstUser.Count() > 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstUser);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Gestionnaire);
                ctr.Show();

            }
            else
            {
                Message.ShowInformation("Aucun utilisareur trouvée", "Information");
            }
        }
        void galatee_OkClickedbtn_Gestionnaire(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsUtilisateur utilisateur = (CsUtilisateur)ctrs.MyObject;
                this.txt_Gestionnaire.Text = utilisateur.MATRICULE;
                this.txt_Gestionnaire.Tag = utilisateur.PK_ID;

            }

        }
        private void txt_Gestionnaire_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txt_Gestionnaire.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (LstUser != null && LstUser.Count() > 0)
                {
                    CsUtilisateur leuser = LstUser.FirstOrDefault(t => t.MATRICULE == this.txt_Gestionnaire.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleGestionnaire.Text = leuser.LIBELLE;
                        txt_Gestionnaire.Tag = leuser.PK_ID;
                        ReLoadingGrid();
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txt_Gestionnaire.Focus();
                    }
                }
            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lstCentreDistinct = new List<ServiceAccueil.CsCentre>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    var lesDistinct = SessionObject.LstCentre.Where(ip=>ip.CODE !="001" && ip.CODE !="002" 
                                                                     && ip.CODE != "003" && ip.CODE !="004").
                                                                     Select(u => new { u.CODE, u.LIBELLE }).Distinct();
                    foreach (var item in lesDistinct)
                    {
                        ServiceAccueil.CsCentre leCentr = new ServiceAccueil.CsCentre();
                        leCentr.CODE = item.CODE;
                        leCentr.LIBELLE = item.LIBELLE;
                        lstCentreDistinct.Add(leCentr);
                    }
                }


                if (lstCentreDistinct != null && lstCentreDistinct.Count != 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<Galatee.Silverlight.ServiceFacturation.CsLotri> leLotSelect = new List< Galatee.Silverlight.ServiceFacturation.CsLotri>();
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CODE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    foreach (ServiceAccueil.CsCentre item in lstCentreDistinct)
                    {
                        leLotSelect.Add(new ServiceFacturation.CsLotri() { 
                         CODE = item.CODE ,
                         LIBELLE= item.LIBELLE 
                         });
                    }
                    Galatee.Silverlight.Facturation.UcGenerique ctrl = new Galatee.Silverlight.Facturation.UcGenerique(leLotSelect, true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                    this.btn_Centre.IsEnabled = true;
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgCentre(object sender, EventArgs e)
        {
            
            Galatee.Silverlight.Facturation.UcGenerique ctrs = sender as Galatee.Silverlight.Facturation.UcGenerique;
            if (ctrs.isOkClick)
            {
                List<Galatee.Silverlight.ServiceFacturation.CsLotri> LesCentreeDuLot = (List<Galatee.Silverlight.ServiceFacturation.CsLotri>)ctrs.MyObjectList;
                if (LesCentreeDuLot != null && LesCentreeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (Galatee.Silverlight.ServiceFacturation.CsLotri item in LesCentreeDuLot)
                    {
                        if (passage == 1)
                            this.Txt_LibelleCentre.Text = item.CODE ;
                        else
                            this.Txt_LibelleCentre.Text = this.Txt_LibelleCentre.Text + "  " + item.CODE;
                        passage++;

                    }
                    this.Txt_LibelleCentre.Tag = LesCentreeDuLot.Select(o=>o.CODE ).ToList();
                }
            }
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((List<ServiceRecouvrement.CsRegCli >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceRecouvrement.CsRegCli SelectedObject = (ServiceRecouvrement.CsRegCli)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    ListRegCliAffecter_Selectionner.Add((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation.SelectedItem);
                }
                else
                {
                    SelectedObject.IsSelect = false;
                    ListRegCliAffecter_Selectionner.Remove((ServiceRecouvrement.CsRegCli)dgListeFraixParicipation.SelectedItem);

                }
            }
        }

        private void txt_Regroupement_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (this.dgListeFraixParicipation_Copy.ItemsSource != null && this.txt_Regroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                List<ServiceRecouvrement.CsRegCli> lstRegcli = LstCodeRegroupement.Where(y => y.CODE == this.txt_Regroupement.Text).ToList();
                this.dgListeFraixParicipation_Copy.ItemsSource = null;
                this.dgListeFraixParicipation_Copy.ItemsSource = lstRegcli;
            }
        }
    }
}



