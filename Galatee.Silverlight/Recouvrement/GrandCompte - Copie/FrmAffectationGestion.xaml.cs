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
            var UtilisateurSelect = ((CsUtilisateur)cbo_utilisateurGestionnaire.SelectedItem).PK_ID;
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

        public void RetourneListeAllUser()
        {
            try
            {

                AdministrationServiceClient client = new AdministrationServiceClient(Utility.Protocole(), Utility.EndPoint("Administration"));
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
                    List<CsUtilisateur> LstUser = new List<CsUtilisateur>();
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
                    cbo_utilisateurGestionnaire.SelectedValue = "PK_ID";
                    cbo_utilisateurGestionnaire.DisplayMemberPath = "LIBELLE";
                    cbo_utilisateurGestionnaire.ItemsSource = LstUser;

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
                    Message.ShowInformation("Affectation ne c'est pas correctement effectué veuillez reprendre l'opération", "Recouvrement");
                }
                Message.ShowInformation("Affectation effectué avec succes", "Recouvrement");

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
            var UtilisateurSelect = ((CsUtilisateur)cbo_utilisateurGestionnaire.SelectedItem).PK_ID;
            var Affectation = LstAffectation.Where(a => a.FK_IDADMUTILISATEUR == UtilisateurSelect && a.ISACTIVE == true);

            if (Affectation != null)
            {
                var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGLI);
                var RegCliNonAffecter = LstCodeRegroupement.Where(a => !ListIdRegcliCorrespondant.Contains(a.PK_ID)).ToList();

                var RegCliAffecter = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();

                LoadGrid(RegCliAffecter, this.dgListeFraixParicipation);
                LoadGrid(RegCliNonAffecter, this.dgListeFraixParicipation_Copy);
            }
        }

        #endregion

    }
}



