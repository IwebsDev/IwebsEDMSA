using Galatee.Silverlight.ServiceAccueil;
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

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmConsultationCompteur : ChildWindow
    {
        public FrmConsultationCompteur()
        {
            InitializeComponent();
            ChargerMarque();
            ChargerDiametreCompteur();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur != null && SessionObject.LstDiametreCompteur.Count != 0)
                {
                    this.Cbo_Diametre.ItemsSource = null;
                    this.Cbo_Diametre.ItemsSource = SessionObject.LstDiametreCompteur;
                    this.Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerDiametreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDiametreCompteur = args.Result;
                    this.Cbo_Diametre.ItemsSource = null;
                    this.Cbo_Diametre.ItemsSource = SessionObject.LstDiametreCompteur;
                    this.Cbo_Diametre.DisplayMemberPath = "LIBELLE";

                };
                service.ChargerDiametreCompteurAsync();
                service.CloseAsync();
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
                    this.Cbo_Marque.ItemsSource = null;
                    this.Cbo_Marque.ItemsSource = SessionObject.LstMarque;
                    this.Cbo_Marque.DisplayMemberPath = "LIBELLE";
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMarque = args.Result;
                    this.Cbo_Marque.ItemsSource = null;
                    this.Cbo_Marque.ItemsSource = SessionObject.LstMarque;
                    this.Cbo_Marque.DisplayMemberPath = "LIBELLE";

                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RechercherCompteur(CsDiacomp leCompteur )
        {
        
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FrmPayeurFacture ctrl = new FrmPayeurFacture();
            ctrl.Show();

        }
    }
}

