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
    public partial class FrmImportFichierColonne : ChildWindow
    {
        static aImportFichierColonne unecolonne;
        static int CodeImport;
        int IdCodeColonne=0;
        static List<aImportFichierColonne> listColonne= new List<aImportFichierColonne>();
        public FrmImportFichierColonne(int codeImport)
        {
            InitializeComponent();
            ChargerCombo();
            CodeImport = codeImport;
            GetColonneImport(codeImport);
        }
        public FrmImportFichierColonne(int codeImport, int idColonne)
        {
            InitializeComponent();
            ChargerCombo();
            CodeImport = codeImport;
            IdCodeColonne = idColonne;
            GetColonneImport(codeImport);
        }
        private void ChargerCombo()
        {

            List<aImportFichier> LstType = new List<aImportFichier>();
            aImportFichier bin= new aImportFichier();
            bin.LIBELLE = "SqlDbType.Binary";
            bin.DESCRIPTION="Binary";
            LstType.Add(bin);
            aImportFichier bit= new aImportFichier();
            bit.LIBELLE = "SqlDbType.Bit";
            bit.DESCRIPTION="Bit";
            LstType.Add(bit);
            aImportFichier Char= new aImportFichier();
            Char.LIBELLE = "SqlDbType.Char";
            Char.DESCRIPTION="Char";
            LstType.Add(Char);
            aImportFichier dat= new aImportFichier();
            dat.LIBELLE = "SqlDbType.DateTime";
            dat.DESCRIPTION="DateTime";
            LstType.Add(dat);
            aImportFichier Decimal= new aImportFichier();
            Decimal.LIBELLE = "SqlDbType.Decimal";
            Decimal.DESCRIPTION="Decimal";
            LstType.Add(Decimal);
            aImportFichier Float= new aImportFichier();
            Float.LIBELLE = "SqlDbType.Float";
            Float.DESCRIPTION="Float";
            LstType.Add(Float);
            aImportFichier Int = new aImportFichier();
            Int.LIBELLE = "SqlDbType.Int";
            Int.DESCRIPTION = "Int";
            LstType.Add(Int);
            aImportFichier NChar = new aImportFichier();
            NChar.LIBELLE = "SqlDbType.NChar";
            NChar.DESCRIPTION = "NChar";
            LstType.Add(NChar);
            aImportFichier NVarChar = new aImportFichier();
            NVarChar.LIBELLE = "SqlDbType.NVarChar";
            NVarChar.DESCRIPTION = "NVarChar";
            LstType.Add(NVarChar);
            aImportFichier VarChar = new aImportFichier();
            VarChar.LIBELLE = "SqlDbType.VarChar";
            VarChar.DESCRIPTION = "VarChar";
            LstType.Add(VarChar);

            cbType.ItemsSource = LstType;
            cbType.DisplayMemberPath = "DESCRIPTION";
            cbType.SelectedValuePath = "LIBELLE";


        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
        }
        private void MiseAjourColonneImport(aImportFichierColonne importColonne)
        {
            bool UpdateImport = false;
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.MisaAjourImportColonneCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                UpdateImport = res.Result;
                if (UpdateImport == true)
                {
                    GetColonneImport(CodeImport);
                   
                }
                else
                {
                    Message.ShowError("Echoué", "Paramétrage");
               
                }
               

            };
            service1.MisaAjourImportColonneAsync(importColonne);
            service1.CloseAsync();

        }
        private void DeleteColonneImport(int idcolone)
        {
            bool DeleteImport = false;
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.DeleteColonneCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                DeleteImport = res.Result;
                if (DeleteImport == true)
                {
                    Message.ShowInformation("Supprimé!", "Paramétrage");
                    GetColonneImport(CodeImport);
                }


            };
            service1.DeleteColonneAsync(idcolone);
            service1.CloseAsync();

        }
         private void GetColonneImport(int codeImport)
        {
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.GetAllImportFichierColonneCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                listColonne = res.Result;
                if (listColonne!= null && listColonne.Count>0)
                {
                    dgColonne.ItemsSource = listColonne;
                }
                
            };
            service1.GetAllImportFichierColonneAsync(codeImport);
            service1.CloseAsync();

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void dgColonne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((aImportFichierColonne)dgColonne.SelectedItem) != null)
            {
                AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                service1.GetImportColonneCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    unecolonne = res.Result;
                    if (unecolonne != null)
                    {
                        IdCodeColonne = unecolonne.ID_COLONNE;
                        txtCol.Text = (unecolonne.NOM != null) ? unecolonne.NOM : string.Empty;
                        txtDesc.Text = (unecolonne.DESCRIPTION != null) ? unecolonne.DESCRIPTION : string.Empty;
                        txtLong.Text = (unecolonne.LONGUEUR != null) ? unecolonne.LONGUEUR.ToString() : string.Empty;
                        cbType.SelectedValue = (unecolonne.TYPE != null) ? unecolonne.TYPE : string.Empty;
                        CodeImport = int.Parse(unecolonne.ID_PARAMETRAGE.ToString());
                    }

                };
                service1.GetImportColonneAsync(((aImportFichierColonne)dgColonne.SelectedItem).ID_COLONNE);
                service1.CloseAsync();
            }
        }


        private void btnAjout_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbType.SelectedItem != null && !string.IsNullOrEmpty(txtCol.Text))
            {
                aImportFichierColonne importation = new aImportFichierColonne()
                {
                    ID_COLONNE = IdCodeColonne,
                    NOM = this.txtCol.Text,
                    TYPE = this.cbType.SelectedValue.ToString(),
                    LONGUEUR = (!string.IsNullOrEmpty(txtLong.Text)) ? int.Parse(txtLong.Text) : 0,
                    DESCRIPTION = txtDesc.Text,
                    ID_PARAMETRAGE = CodeImport

                };

                MiseAjourColonneImport(importation);
            }
            else
            {
                Message.ShowError("Veuillez remplir les champs svp!", "Paramétrage");

            }
        }

        private void btnSupprime_Click(object sender, RoutedEventArgs e)
        {
            if (((aImportFichierColonne)dgColonne.SelectedItem).ID_COLONNE != null)
            {
                DeleteColonneImport(int.Parse(((aImportFichierColonne)dgColonne.SelectedItem).ID_COLONNE.ToString()));

            }
        }
    }
}

