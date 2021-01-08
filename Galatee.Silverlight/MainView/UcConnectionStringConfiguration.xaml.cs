using Galatee.Silverlight.ServiceAdministration;
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
using Galatee.Silverlight.ServiceParametrage;

namespace Galatee.Silverlight.MainView
{
    public partial class UcConnectionStringConfiguration : ChildWindow
    {
        public UcConnectionStringConfiguration()
        {
            InitializeComponent();
        }

        private List<Galatee.Silverlight.ServiceParametrage.CsLibelle> ConstruireListeTypeDeBaseDeDonnees()
        {
            List<CsLibelle> ListeDeRetour = new List<CsLibelle>();
            CsLibelle TypeDeBaseDeDonnees = null;
            try
            {
                //SQL SERVER
                TypeDeBaseDeDonnees = new CsLibelle();
                TypeDeBaseDeDonnees.CODE = "SQL SERVER";
                TypeDeBaseDeDonnees.LIBELLE = "System.Data.SqlClient";
                ListeDeRetour.Add(TypeDeBaseDeDonnees);
                // POSTGRESQL
                TypeDeBaseDeDonnees = new CsLibelle();
                TypeDeBaseDeDonnees.CODE = "POSTGRESQL";
                TypeDeBaseDeDonnees.LIBELLE = "Devart.Data.PostgreSql";
                ListeDeRetour.Add(TypeDeBaseDeDonnees);
                // ORACLE
                TypeDeBaseDeDonnees = new CsLibelle();
                TypeDeBaseDeDonnees.CODE = "ORACLE";
                TypeDeBaseDeDonnees.LIBELLE = "Oracle.DataAccess.Client";
                ListeDeRetour.Add(TypeDeBaseDeDonnees);
                return ListeDeRetour;
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        //private List<CsLibelle> ConstruireListeBaseDeDonnees()
        //{
        //    List<CsLibelle> ListeDeRetour = new List<CsLibelle>();
        //    CsLibelle BaseDeDonnees = null;
        //    try
        //    {
        //        //GALADB
        //        BaseDeDonnees = new CsLibelle();
        //        BaseDeDonnees.CODE = "SQL SERVER";
        //        BaseDeDonnees.LIBELLE = "System.Data.SqlClient";
        //        ListeDeRetour.Add(BaseDeDonnees);
        //        // ABO07
        //        BaseDeDonnees = new CsLibelle();
        //        BaseDeDonnees.CODE = "POSTGRESQL";
        //        BaseDeDonnees.LIBELLE = "Devart.Data.PostgreSql";
        //        ListeDeRetour.Add(BaseDeDonnees);
        //        // RPNT
        //        BaseDeDonnees = new CsLibelle();
        //        BaseDeDonnees.CODE = "ORACLE";
        //        BaseDeDonnees.LIBELLE = "Oracle.DataAccess.Client";
        //        ListeDeRetour.Add(BaseDeDonnees);
        //        // RPNT
        //        BaseDeDonnees = new CsLibelle();
        //        BaseDeDonnees.CODE = "ORACLE";
        //        BaseDeDonnees.LIBELLE = "Oracle.DataAccess.Client";
        //        ListeDeRetour.Add(BaseDeDonnees);
        //        return ListeDeRetour;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirListeTypeDeBaseDeDonnees()
        {
            try
            {
                CboTypeBd.Items.Clear();
                CboTypeBd.ItemsSource = ConstruireListeTypeDeBaseDeDonnees();
                CboTypeBd.SelectedValuePath = "CODE";
                CboTypeBd.DisplayMemberPath = "LIBELLE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RemplirListeTypeDeBaseDeDonnees();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                BtnTestConnexion.IsEnabled = !string.IsNullOrEmpty(TxtNomBd.Text) && !string.IsNullOrEmpty(TxtNomServeur.Text) && CboTypeBd.SelectedItem != null ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtNomServeur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void TxtNomBd_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void CboTypeBd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void BtnTestConnexion_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
    }
}

