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
using Galatee.Silverlight.ServiceAccueil;
using System.IO;

namespace Galatee.Silverlight.Shared
{
    public partial class UcFichierJoint : UserControl
    {
        ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private UcImageScanne formScanne = null;


        public UcFichierJoint(List<ObjDOCUMENTSCANNE>  _lstPieceJoint,bool IsConsultation)
        {
            InitializeComponent();
            ChargerTypeDocument();
            RenseignerDocument(_lstPieceJoint);
            if (IsConsultation)
            {
                this.cbo_typedoc.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_ajoutpiece.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_supprimerpiece .Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                        LstTypeDocument.Add(item);

                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RenseignerDocument(List<ObjDOCUMENTSCANNE> lesPieceJoint)
        {
            try
            {
                if (lesPieceJoint != null && lesPieceJoint.Count != 0)
                {
                    foreach (var item in lesPieceJoint)
                        LstPiece.Add(item);
                    dgListePiece.ItemsSource = this.LstPiece.Where(o => o.ISTOREMOVE != true); ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        /*18/02/2019 */
        string CheminInitialDeCopyDeFichier = string.Empty;
        string NomDeCopyDeFichier = string.Empty;
        /**/
      
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CheminInitialDeCopyDeFichier = string.Empty;
            NomDeCopyDeFichier = string.Empty;
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.

                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        /*18/02/2019 */
                        CheminInitialDeCopyDeFichier = SessionObject.CheminDocumentScanne;
                        NomDeCopyDeFichier = openDialog.Files.First().Name;

                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();

                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE,
                                             FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, 
                                             CONTENU = image,
                                             CODETYPEDOC =((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).CODE ,
                                                  DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now,
                                                      USERCREATION = UserConnecte.matricule,
                                                      USERMODIFICATION = UserConnecte.matricule,
                                                      ISNEW = true,
                                                      CHEMININIT = CheminInitialDeCopyDeFichier + "\\" + NomDeCopyDeFichier ,
                                                      //CHEMINCOPY = SessionObject.Enumere.CheminImpressionServeur + "\\" + laDemande.LaDemande.NUMDEM + "_" + ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).CODE + "." + NomDeCopyDeFichier.Split('.').[1],
                                                      NOMDUFICHIER = NomDeCopyDeFichier
            });

            this.dgListePiece.ItemsSource = null;
            this.dgListePiece.ItemsSource = this.LstPiece.Where(o=>o.ISTOREMOVE != true );
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            if (dgListePiece.SelectedItem != null)
            {
                ObjDOCUMENTSCANNE selectObj = (ObjDOCUMENTSCANNE)this.dgListePiece.SelectedItem;
                if (selectObj.CONTENU != null)
                {
                    MemoryStream memoryStream = new MemoryStream(selectObj.CONTENU);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.DocumentScanneContenuCompleted += (s, args) =>
                     {
                         if ((args != null && args.Cancelled) || (args.Error != null))
                             return;

                         MemoryStream memoryStream = new MemoryStream(args.Result.CONTENU);
                         var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                         ucImageScanne.Show();
                     };
                    service.DocumentScanneContenuAsync(selectObj);
                    service.CloseAsync();

                }

                
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    Fraix.ISTOREMOVE = true;
                    this.dgListePiece.ItemsSource = null;
                    this.dgListePiece.ItemsSource = this.LstPiece.Where(u=>u.ISTOREMOVE != true ).ToList();

                    //ZEG 28/09/2017
                    this.SupprimerPieceJointe(Fraix);
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }


        //ZEG 28/09/2017
        private void SupprimerPieceJointe(ObjDOCUMENTSCANNE piece)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.SupprimerPieceJointeCompleted += (s, args) =>
            {
                if ((args != null && args.Cancelled) || (args.Error != null))
                    return;

                if (args.Result == true)
                    Message.Show("Suppression réussie...", "Pièce jointe");
            };
            service.SupprimerPieceJointeAsync(piece);
            service.CloseAsync();


        }

        FileStream fs;
        private void OuvrirPieceJointe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgListePiece.SelectedItem != null)
                {
                    ObjDOCUMENTSCANNE selectObj = (ObjDOCUMENTSCANNE)this.dgListePiece.SelectedItem;
                    if (selectObj.CONTENU != null)
                    {
                        MemoryStream memoryStream = new MemoryStream(selectObj.CONTENU);
                        var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                        ucImageScanne.Show();
                    }
                    else
                    {
                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        service.DocumentScanneContenuCompleted += (s, args) =>
                        {
                            if ((args != null && args.Cancelled) || (args.Error != null))
                                return;

                            MemoryStream memoryStream = new MemoryStream(args.Result.CONTENU);
                            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                            ucImageScanne.Show();
                        };
                        service.DocumentScanneContenuAsync(selectObj);
                        service.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Erreur");
            }
        }


    }
}
