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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Galatee.Silverlight.Resources.Devis;
using System.Runtime.Serialization;
//using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServicePrintings;
namespace Galatee.Silverlight.Shared
{
    public partial class UcImageScanne : ChildWindow
    {
        //public FileStream ImageScannee { get; set; }
        private SessionObject.ExecMode _modeExecution ;
        string extension;
        public UcImageScanne()
        {
            InitializeComponent();
        }

        public UcImageScanne(string pFileNamepath)
        {
            try
            {
                InitializeComponent();

                Message.Show("ICI25", "");


                if (File.Exists(pFileNamepath))
                    AfficherImage(File.ReadAllBytes(pFileNamepath));
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        public UcImageScanne(Byte[] pFile)
        {
            try
            {
                InitializeComponent();
                bytes=pFile;
                AfficherImage(pFile);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        public UcImageScanne(System.IO.MemoryStream pFile, SessionObject.ExecMode pExecMode)
        {
            try
            {
                InitializeComponent();
                _modeExecution = pExecMode;
                SetImageByteArray(pFile);
                AfficherImage(pFile);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        public UcImageScanne(System.IO.MemoryStream pFile,string extension, SessionObject.ExecMode pExecMode)
        {
            try
            {
                InitializeComponent();
                _modeExecution = pExecMode;
                SetImageByteArray(pFile,extension);
                AfficherImage(pFile);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void SetImageByteArray(System.IO.MemoryStream pFile, string extension)
        {
            bytes = new byte[pFile.Length];
            bytes = pFile.ToArray();
            //pFile.Read(bytes, 0, (int)pFile.Length);
            this.extension = extension;
            this.MS = pFile;
        }
        private void SetImageByteArray(System.IO.MemoryStream pFile)
        {
            bytes = new byte[pFile.Length];
            pFile.Read(bytes, 0, (int)pFile.Length);
            //this.extension = extension;
        }
        private void AfficherImage(System.IO.MemoryStream pFileStream)
        {
            try
            {
                if (pFileStream != null)
                {
                    var bmp = new System.Windows.Media.Imaging.BitmapImage();
                    bmp.SetSource(pFileStream);
                    image.Source = bmp;
                    if(_modeExecution == SessionObject.ExecMode.Consultation)
                    {
                        OKButton.Visibility = Visibility.Collapsed;
                        CancelButton.Content = "Fermer";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AfficherImage(byte[] pImage)
        {
            try
            {
                if (pImage != null)
                {
                    var stream = new System.IO.MemoryStream(pImage);
                    var bmp = new System.Windows.Media.Imaging.BitmapImage();
                    bmp.SetSource(stream);
                    image.Source = bmp;
                    if (_modeExecution == SessionObject.ExecMode.Consultation)
                    {
                        OKButton.Visibility = Visibility.Collapsed;
                        CancelButton.Content = "Fermer";
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public byte[] bytes { get; set; }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImprimerImage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ImprimerImage()
        {
            ServiceAccueil.CsImageFile imagefile = new ServiceAccueil.CsImageFile();
            imagefile.Imagestream = bytes;
            List<ServiceAccueil.CsImageFile> lstimagefile = new List<ServiceAccueil.CsImageFile>();
            lstimagefile.Add(imagefile);
            Utility.ActionExportFormatWithSplitingPrinting<ServicePrintings.CsImageFile, ServiceAccueil.CsImageFile>(lstimagefile, null, SessionObject.CheminImpression, "FichierJoint", "Devis", true, "pdf");

        }
        public System.IO.MemoryStream MS { get; set; }
    }
}

