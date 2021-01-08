using Galatee.Silverlight.ServiceAccueil  ;
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

namespace Galatee.Silverlight.Devis
{
    public partial class UcSaisiQuantite : ChildWindow
    {
        public UcSaisiQuantite()
        {
            InitializeComponent();
            lbl_Montant.Visibility = System.Windows.Visibility.Collapsed;
            this.txt_Montant.Visibility = System.Windows.Visibility.Collapsed;
        }
        bool Istaxe = false;
        bool IsMontant = false;
        CsCtax LaTaxe = new CsCtax();
        public   ObjELEMENTDEVIS  SelectedObject = new ObjELEMENTDEVIS();
        public UcSaisiQuantite(ObjELEMENTDEVIS _SelectedObject)
        {
            InitializeComponent();
            SelectedObject = _SelectedObject;
            lbl_Montant.Visibility = System.Windows.Visibility.Collapsed;
            this.txt_Montant.Visibility = System.Windows.Visibility.Collapsed;
        }
        public UcSaisiQuantite(ObjELEMENTDEVIS _SelectedObject, bool IsTaxe)
        {
            InitializeComponent();
            lbl_Montant.Visibility = System.Windows.Visibility.Collapsed;
            this.txt_Montant.Visibility = System.Windows.Visibility.Collapsed;
            SelectedObject = _SelectedObject;
            Istaxe = IsTaxe;
        }
        public UcSaisiQuantite(ObjELEMENTDEVIS _SelectedObject, bool IsTaxe, CsCtax laTaxe)
        {
            InitializeComponent();
            lbl_Montant.Visibility = System.Windows.Visibility.Collapsed;
            this.txt_Montant.Visibility = System.Windows.Visibility.Collapsed;
            SelectedObject = _SelectedObject;
            Istaxe = IsTaxe;
            LaTaxe = laTaxe;
        }
        public UcSaisiQuantite(ObjELEMENTDEVIS _SelectedObject, bool IsTaxe,bool AvecMontant, CsCtax laTaxe)
        {
            InitializeComponent();
            SelectedObject = _SelectedObject;
            Istaxe = IsTaxe;
            LaTaxe = laTaxe;
            IsMontant = AvecMontant ;
            if (AvecMontant)
            {
                lbl_Montant.Visibility = System.Windows.Visibility.Visible;
                this.txt_Montant.Visibility = System.Windows.Visibility.Visible;
                this.txt_Montant.Text = _SelectedObject.COUTUNITAIRE.ToString(SessionObject.FormatMontant);
            }
        }
       public bool isOkClick = false;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_Quantite.Text))
            {
                if (!Istaxe)
                {
                    SelectedObject.QUANTITE = Convert.ToInt16(this.txt_Quantite.Text);
                    SelectedObject.COUT = Convert.ToDecimal(SelectedObject.PRIX * SelectedObject.QUANTITE);
                    SelectedObject.TAXE = SelectedObject.COUT * SelectedObject.TAUXTAXE;
                }
                else
                {
                    if (Istaxe)
                    {
                        SelectedObject.QUANTITE = int.Parse(txt_Quantite.Text);
                        SelectedObject.MONTANTHT = SelectedObject.COUTUNITAIRE * SelectedObject.QUANTITE;
                        SelectedObject.MONTANTTAXE = SelectedObject.MONTANTHT * LaTaxe.TAUX;
                        SelectedObject.MONTANTTTC = SelectedObject.MONTANTHT + SelectedObject.MONTANTTAXE;
                    }
                    if (IsMontant)
                    {
                        SelectedObject.QUANTITE = int.Parse(txt_Quantite.Text);
                        SelectedObject.MONTANTHT = decimal.Parse(txt_Montant.Text) * SelectedObject.QUANTITE;
                        SelectedObject.COUTUNITAIRE  = decimal.Parse(txt_Montant.Text) ;
                        SelectedObject.MONTANTTAXE = SelectedObject.MONTANTHT * LaTaxe.TAUX;
                        SelectedObject.MONTANTTTC = SelectedObject.MONTANTHT + SelectedObject.MONTANTTAXE;
                    }
                }
                isOkClick = true;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                OKButton_Click(null, null);
            }
        }
    }
}

