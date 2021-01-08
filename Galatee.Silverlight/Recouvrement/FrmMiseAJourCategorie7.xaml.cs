using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmMiseAJourCategorie7 : ChildWindow
    {
        List<string> lines = new List<string>();
        string FullName = string.Empty;
        public FrmMiseAJourCategorie7()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Chemin_DAcces.Text) && lines.Count > 0)
                MiseAJourCategorie7();
            else
            {
                Message.ShowWarning("Veuillez selectionner le fichier", "Alert");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //ofd.Filter = "WordFile|*.docx";
            //if (ofd.ShowDialog() == true)
            //{
            //    string folderPath = ofd.File.DirectoryName;
            //}


            // Create an instance of the open file dialog box.
            lines = new List<string>();
            var openDialog = new OpenFileDialog();
            //openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openDialog.InitialDirectory = SessionObject.CheminImpression.Replace('[', '\\');
            // Set filter options and filter index.
            openDialog.Filter =
                "files (*.FEP) | *.FEP";
            openDialog.FilterIndex = 1;
            openDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOk = openDialog.ShowDialog();
            // Process input if the user clicked OK.
            if (userClickedOk == true)
            {

                if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                {
                     FullName = SessionObject.CheminImpression.Replace('[', '\\')+"\\"+ openDialog.File;
                    txt_Chemin_DAcces.Text = FullName;
                    lines.Add(FullName);

                    //lines = System.IO.File.ReadLines(openDialog.File.DirectoryName).ToList();
                    //Message.ShowInformation("Fichier OK3", "Resultat");

                }
                else
                {
                    Message.ShowInformation("Le chemin d'acces n'a pas pu être récupéré", "Resultat");

                }
            }
        }

        private void MiseAJourCategorie7()
        {
            try
            {
                prgBar.Visibility = Visibility.Visible;
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.MiseAJourCategorie7Completed += (s, args) =>
                {
                    prgBar.Visibility = Visibility.Collapsed;

                    if ((args != null && args.Cancelled) || (args.Error != null))
                    {
                        Message.ShowInformation("Erreur de la Mise à jour", "Resultat");
                        return;
                    }
                    if (args.Result!=null && args.Result.Count() == 0)
                    {
                        Message.ShowInformation("Mise à jour effectuée avec succès", "Resultat");
                        this.DialogResult = true;
                    }
                    else
                    {
                        if (args.Result != null && args.Result.Count() > 0)
                        Message.ShowWarning("Mise à jour effectué ,mais les utilisateurs suivant non pas été pris en compte:\n"+string.Join(";",args.Result.ToArray()), "Resultat");
                        else
                            if(args.Result==null)
                            Message.ShowInformation("Mise à jour à échoué", "Resultat");

                    }
                };
                service.MiseAJourCategorie7Async(lines,UserConnecte.matricule);
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

