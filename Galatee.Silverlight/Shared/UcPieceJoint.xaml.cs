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
using Galatee.Silverlight;
using System.IO;
using Galatee.Silverlight.Tarification.Helper;
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcPieceJoint : ChildWindow
    {

        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        #region Variables

        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public List<CsTypeDOCUMENTSCANNE> LstTypeDocument = new List<CsTypeDOCUMENTSCANNE>();

        private UcImageScanne formScanne = null;


        #endregion

        #region Contructeurs

        public UcPieceJoint()
        {
            InitializeComponent();
            ChargerTypeDocument();
            this.dgListePiece.ItemsSource = this.LstPiece;
        }
        public UcPieceJoint(List<ObjDOCUMENTSCANNE> LstPiece)
        {
            InitializeComponent();
            foreach (var item in LstPiece)
	        {
                this.LstPiece.Add(item);
	        }
            ChargerTypeDocument();
            this.dgListePiece.ItemsSource = this.LstPiece;
        }
        #endregion

        #region Even Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_refClient.Text) && hyperlinkButtonPropScannee.Tag!=null)
                {
                    if (cbo_typedoc.SelectedItem!=null)
                    {
                        var image = hyperlinkButtonPropScannee.Tag as byte[];
                        this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(),
                            NOMDOCUMENT = this.txt_refClient.Text, 
                            //FK_TYPEDOCUMENT =((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, 
                            CONTENU = image, DATECREATION = DateTime.Now, 
                            DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, 
                            USERMODIFICATION = UserConnecte.matricule });
                        this.dgListePiece.ItemsSource = this.LstPiece;
                        hyperlinkButtonPropScannee.Tag = null;
                    }
                       

                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        private void btn_editer_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgListePiece.SelectedItem != null)
            {
                if (btn_editer.Content.ToString().Trim() == "Editer".Trim())
                {
                    txt_refClient.Text = ((ObjDOCUMENTSCANNE)dgListePiece.SelectedItem).NOMDOCUMENT;
                    txt_refClient.Tag = ((ObjDOCUMENTSCANNE)dgListePiece.SelectedItem);
                    //cbo_typedoc.SelectedItem =LstTypeDocument.FirstOrDefault(t=>t.PK_ID== ((ObjDOCUMENTSCANNE)dgListePiece.SelectedItem).FK_TYPEDOCUMENT);
                    hyperlinkButtonPropScannee.Tag = ((ObjDOCUMENTSCANNE)dgListePiece.SelectedItem).CONTENU;

                    btn_editer.Content = "Mise a Jour";
                }
                else
                {
                    ObjDOCUMENTSCANNE Fraix = ((ObjDOCUMENTSCANNE)txt_refClient.Tag);
                    if (Fraix != null)
                    {
                       
                            int index = this.LstPiece.IndexOf(Fraix);

                            Fraix.NOMDOCUMENT = txt_refClient.Text;
                            Fraix.CONTENU = hyperlinkButtonPropScannee.Tag as byte[];
                            //Fraix.FK_TYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID;
                            this.LstPiece[index] = Fraix;
                            this.dgListePiece.ItemsSource = this.LstPiece;
                            btn_editer.Content = "Editer".Trim();
                            txt_refClient.Tag = null;
                        
                    }

                }


            }
        }
        private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void txt_refClient_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txt_montant_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void hyperlinkButtonPropScannee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (hyperlinkButtonPropScannee.Tag!=null)
                {
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow("Devis", "Voulez vous supprimer le manuscrit scanné ? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            this.hyperlinkButtonPropScannee.Tag = null;
                        }
                        else
                        {
                                MemoryStream memoryStream = new MemoryStream(hyperlinkButtonPropScannee.Tag as byte[]);
                                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                                //var ucImageScanne = new UcImageScanne(hyperlinkButtonPropScannee.Tag as byte[], SessionObject.ExecMode.Modification);
                                ucImageScanne.Show();
                        }
                        //ActiverEnregistrerOuTransmettre();
                    };
                    mBoxControl.Show();
                }
                else
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
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            hyperlinkButtonPropScannee.Tag = memoryStream.GetBuffer();
                            formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                            formScanne.Show();
                        }
                    }
                }
                   
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            try
            {
                var form = (UcImageScanne)sender;
                if (form != null)
                {
                    if (form.DialogResult == true /*&& form.ImageScannee != null*/)
                    {
                        this.hyperlinkButtonPropScannee.Content = "Document scannée";
                        //this.hyperlinkButtonPropScannee.Tag = form.ImageScannee;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Service

        private void ChargerTypeDocument()
        {
            try
            {

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                    {
                        if ((args != null && args.Cancelled) || (args.Error != null))
                            return;

                        LstTypeDocument = args.Result;
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

        #endregion

    }
}



