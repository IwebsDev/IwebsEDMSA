using Galatee.Silverlight.ServiceCaisse;
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
using Galatee.Silverlight.Resources.Caisse;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmMotifAction : ChildWindow
    {
        public FrmMotifAction()
        {
            InitializeComponent();
            this.txt_raisonannulation.Focus();
        }
        public bool IsValide = false;
        public List<CsLclient> lstReglementAnnuler = new List<CsLclient>();
        public FrmMotifAction(List<CsLclient> _lstReglementAnnuler)
        {
            InitializeComponent();
            lstReglementAnnuler = _lstReglementAnnuler;
            this.Title = Langue.Raison_annulation;
            this.txt_raisonannulation.Focus();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_raisonannulation.Text))
                {
                    //lstReglementAnnuler.ForEach(p=>p.MOTIFANNULATION = this.txt_raisonannulation.Text && i  );
                    foreach (CsLclient item in lstReglementAnnuler)
                        item.MOTIFANNULATION = this.txt_raisonannulation.Text;
                    IsValide = true;
                    this.DialogResult = true;
                }
                else
                    Message.ShowInformation("Veuillez saisir la raison de l'annulation", Langue.LibelleModule);
            }
            catch (Exception ex)
            {
                  Message.ShowInformation(ex.Message ,"OKButton_Click=>" + Langue.LibelleModule);

            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseMove_1(object sender, MouseEventArgs e)
        {
        }
    }
}

