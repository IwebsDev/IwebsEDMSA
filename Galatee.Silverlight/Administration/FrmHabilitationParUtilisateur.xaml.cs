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
using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Administration
{
    public partial class  FrmHabilitationParUtilisateur  : ChildWindow
    {
        public FrmHabilitationParUtilisateur()
        {
            InitializeComponent();
            GetData();
        }
        List<int> lstIUSeru;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (this.txtUtilisater.Tag != null)
                {
                    lstIUSeru = new List<int>();
                    lstIUSeru.Add((int)this.txtUtilisater.Tag);
                    RetourneHabilitationUser(lstIUSeru);
                }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }
        private void RetourneHabilitationUser(List<int> ListidClient)
        {
            string key = Utility.getKey();
            int response = LoadingManager.BeginLoading(Galatee.Silverlight.Resources.Accueil.Langue.En_Cours);
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneProfilUtilisateurCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    LoadingManager.EndLoading(response);
                    return;
                }
                if (res.Result == null  )
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    LoadingManager.EndLoading(response);
                    return;
                }
                LoadingManager.EndLoading(response);
                Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true);
            };
            client.RetourneProfilUtilisateurAsync(lstIUSeru, key);
        
        }
        List<CsUtilisateur> _lstUserProfil = new List<CsUtilisateur>();
        void GetData()
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

                    List<ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<int> lstCentreHabil = new List<int>();
                    foreach (var item in lstCentreProfil)
                        lstCentreHabil.Add(item.PK_ID);

                    var lstUtilisateurDistnct = res.Result.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList().Select(t => new { t.PK_ID, t.MATRICULE, t.LIBELLE }).Distinct().ToList();
                    foreach (var item in lstUtilisateurDistnct)
                        _lstUserProfil.Add(new CsUtilisateur { PK_ID = item.PK_ID, MATRICULE = item.MATRICULE, LIBELLE = item.LIBELLE });
                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void galatee_OkClickedbtn_AgtLivreur(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtUtilisater.Text = utilisateur.MATRICULE;
                this.txtUtilisater.Tag = utilisateur.PK_ID;

            }

        }
        private void btn_AgtLivreur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_lstUserProfil != null && _lstUserProfil.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lstUserProfil.OrderBy(t => t.LIBELLE).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_AgtLivreur);
                    ctr.Show();

                }
                else
                {
                    Message.ShowInformation("Aucun utilisareur trouvée", "Information");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtAgt_Livreur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtUtilisater.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (_lstUserProfil != null && _lstUserProfil.Count() > 0)
                {
                    ServiceAdministration.CsUtilisateur leuser = _lstUserProfil.FirstOrDefault(t => t.MATRICULE == this.txtUtilisater.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleUtilisateur.Text = leuser.LIBELLE;
                        txtUtilisater.Tag = leuser.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtUtilisater.Focus();
                    }
                }
            }
        }
    }
}

