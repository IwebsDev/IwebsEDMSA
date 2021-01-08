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
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Devis
{
    public partial class UcAutreMateriel : ChildWindow
    {
        int iddemande;
        public event EventHandler Closed;
        CsSortieAutreMateriel ObjectSelect = new CsSortieAutreMateriel();
        public CsSortieAutreMateriel MyObject { get; set; }
        public bool isOkClick;
        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }


        public UcAutreMateriel(int fk_iddemande)
        {
            iddemande = fk_iddemande;
            InitializeComponent();
            ChargeTypeMateriel();
        }

        private void ChargeTypeMateriel()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneMaterielBranchementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    cboTypeMateriel.ItemsSource = args.Result;

                    cboTypeMateriel.DisplayMemberPath = "LIBELLE";
                    cboTypeMateriel.SelectedValuePath = "PK_ID";


                };
                service.RetourneMaterielBranchementAsync();
                service.CloseAsync();
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
                //this.DialogResult = true;

                if (cboTypeMateriel.SelectedValue != null && !string.IsNullOrEmpty(txtLibelle.Text) && !string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtLivre.Text))
                {
                    isOkClick = true;
                    getElementCloseAfterSelection();
                    this.DialogResult = true;
                }
                else
                    Message.ShowInformation("", "Renseignez tous les champs");
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void _ctrl_Closed(object sender, EventArgs e)
        {
            UcSortieMateriel ctr = sender as UcSortieMateriel;
            CsSortieAutreMateriel _LeCompteurSelectDatagrid = new CsSortieAutreMateriel();
            CsSortieAutreMateriel _LeCompteurSelect = new CsSortieAutreMateriel();

            CsSortieAutreMateriel sortie = new CsSortieAutreMateriel()
            {
                FK_IDDEMANDE = iddemande,
                FK_IDTYPEMATERIEL = int.Parse(cboTypeMateriel.SelectedValue.ToString()),
                LIBELLE = txtLibelle.Text,
                NOMBRE = txtNombre.Text,
                LIVRE = txtLivre.Text

            };

        }

        public void getElementCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                    CsSortieAutreMateriel sortie = new CsSortieAutreMateriel()
                    {
                        FK_IDDEMANDE = iddemande,
                        FK_IDTYPEMATERIEL = int.Parse(cboTypeMateriel.SelectedValue.ToString()),
                        LIBELLE = txtLibelle.Text,
                        NOMBRE = txtNombre.Text,
                        LIVRE = txtLivre.Text

                    };
                    MyObject = sortie;
                    Closed(this, new EventArgs());
                }
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

        private void cboTypeMateriel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTypeMateriel.SelectedValue != null)
            {
                if (int.Parse(cboTypeMateriel.SelectedValue.ToString()) != 9)
                {
                    txtLibelle.Text = ((CsMaterielBranchement)cboTypeMateriel.SelectedItem).LIBELLE;
                    txtLibelle.IsReadOnly = true;
                }

            }
        }
    }
}

