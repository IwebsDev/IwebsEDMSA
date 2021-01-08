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
using Galatee.Silverlight.ServiceRecouvrement;
using System.ComponentModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class UserClientPicker : ChildWindow
    {
        public UserClientPicker()
        {
            InitializeComponent();

            RemplirCentre();
        }

        public event EventHandler Closed;

        string campaign = string.Empty;

        public string Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        bool _desbaleprint = false;
        bool _returnvalue = false;

        public UserClientPicker(bool returnbackvalue)
        {
            InitializeComponent();
            _returnvalue = returnbackvalue;
            btnOk.Visibility = Visibility.Visible;
            RemplirCentre();
        }

        aClient _campagnes = null;
        [Description("Retourne les campagnes issues de la recherche ou sélectionnées dans la liste")]
        public aClient client
        {
            get { return this._campagnes; }
            set { _campagnes = value; }
        }

        List<aClient> RecupererLesElementsSelectionnes()
        {
            List<aClient> cls = new List<aClient>();
            foreach (aClient lvi in this.lvwResultat.SelectedItems)
            {
                aClient cl = lvi;
                cls.Add(cl);
            }
            return cls;
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

         void RemplirCentre()
          {
              CsCentre centre;
              RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
              client.SelectCentreCampagneCompleted += (es, result) =>
                  {
                      if (result.Cancelled || result.Error != null)
                      {
                          string error = result.Error.Message;
                          MessageBox.Show("Erreur à l'exécution du service", "SelectCentreCampagne", MessageBoxButton.OK);
                          return;
                      }

                      if (result.Result == null || result.Result.Count == 0)
                      {
                          MessageBox.Show("Aucune donnée trouvée", "SelectCentreCampagne", MessageBoxButton.OK);
                          return;
                      }

                      List<CsCentre> lCentre = new List<CsCentre>();
                      lCentre.AddRange(result.Result);

                      if ((lCentre != null) && (lCentre.Count > 0))
                      {
                          this.cmbCentre.Items.Clear();
                          this.cmbCentre.DisplayMemberPath = "LIBELLE";
                          this.cmbCentre.SelectedValuePath = "CODE";
                          foreach (CsCentre a in lCentre)
                          {
                              centre = new CsCentre();
                              centre.CODE = a.CODE;
                              centre.LIBELLE = a.LIBELLE;
                              this.cmbCentre.Items.Add(centre);
                          }
                      }
                      //Ligne vide : champ non obligatoire 
                      centre = new CsCentre();
                      centre.CODE = centre.LIBELLE = "    ";// string.Empty;
                      this.cmbCentre.Items.Add(centre);

                      this.cmbCentre.DisplayMemberPath = "LIBELLE";
                      this.cmbCentre.SelectedValue = "CODE";
                      // remplir agent combobox

                  };
              client.SelectCentreCampagneAsync();
             
          }

         void Recherche()
         {
                 try
                 {
                     btnsearch.IsEnabled = false;
                     aClient critere = new aClient();
                     critere.Centre = (string.IsNullOrEmpty(this.txtCentre.Text)) ? string.Empty : this.txtCentre.Text;
                     critere.Client = (string.IsNullOrEmpty(this.txtClient.Text)) ? string.Empty : this.txtClient.Text;
                     critere.Ordre = (string.IsNullOrEmpty(this.txtOrdre.Text)) ? string.Empty : this.txtOrdre.Text;

                     RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                     client.RechercherListeClientsCompleted += (ss, args) =>
                     {
                         if (args.Cancelled || args.Error != null)
                         {
                             string error = args.Error.Message;
                             MessageBox.Show(args.Error.Message, "RechercherListeClients", MessageBoxButton.OK);
                             btnsearch.IsEnabled = true;
                             return;
                         }

                         if (args.Result == null || args.Result.Count == 0)
                         {
                             MessageBox.Show("Aucune donnée trouvée", "RechercherListeClients", MessageBoxButton.OK);
                             btnsearch.IsEnabled = true;
                             return;
                         }

                         List<aClient> clients = new List<aClient>();
                         clients.AddRange(args.Result);

                         this.lvwResultat.ItemsSource = null;
                         this.lvwResultat.ItemsSource = clients;
                         btnsearch.IsEnabled = true;
                     };
                     client.RechercherListeClientsAsync(critere);
                    
                 }
                 catch (Exception ex)
                 {
                     btnsearch.IsEnabled = true;
                     string error = ex.Message;
                     //MessageBox.Show(ex.Message, Program.rm.GetString("msgSuiviRecouvrements"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
         }

         private void cmbCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             if ((this.cmbCentre.SelectedItem != null) &&
                ((this.cmbCentre.SelectedItem as CsCentre).CODE != string.Empty))
                 this.txtCentre.Text = (this.cmbCentre.SelectedItem as CsCentre).CODE;
         }

         private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             Recherche();
         }

         private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             btnOk.IsEnabled = true;
         }

         private void btnOk_Click(object sender, RoutedEventArgs e)
         {
             if (Closed != null)
             {
                 client = this.RecupererLesElementsSelectionnes().FirstOrDefault();
                 Closed(this, new EventArgs());
                 this.DialogResult = true;
             }
         }

         private void btnreset_Click(object sender, RoutedEventArgs e)
         {
             this.lvwResultat.ItemsSource = null;
             this.cmbCentre.SelectedValue = string.Empty;
         }
    }
}

