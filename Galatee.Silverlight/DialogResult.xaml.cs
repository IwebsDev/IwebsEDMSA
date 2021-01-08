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
using Galatee.Silverlight.Resources;

namespace Galatee.Silverlight
{
    public partial class DialogResult : ChildWindow
    {
        public event EventHandler Closed;
        bool _isClickOK;
        bool reject = false;
        public bool IsClickOK
        {
            get { return _isClickOK; }
            set { _isClickOK = value; }
        }
        bool _notCloseInCancel;
        bool _IsInformation = true;
        bool _IsInterrogation = false;
        public bool Yes 
          {
              get { return reject; }
              set { reject = value; } 
          }

        public bool notCloseInCancel
        {
            get { return _notCloseInCancel; }
            set { _notCloseInCancel = value; }
        }

        public DialogResult()
        {
            InitializeComponent();
        }

        public DialogResult(string message, bool notCloseInCancel)
        {
            InitializeComponent();

            _notCloseInCancel = notCloseInCancel;
            Message.Text = message;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="title">Title of that dialog</param>
        /// <param name="notCloseInCancel">Not cloe this Dialog when Cancel Button click</param>
        /// <param name="IsInformation">Is Information Dialog </param>
        /// <param name="IsInterogation">Is Interrogative Dialog.That means a value is returned </param>
        public DialogResult(string message,string title, bool notCloseInCancel,bool IsInformation,bool IsInterogation)
        {
            InitializeComponent();
            Title = title;
            _IsInformation = IsInformation;
            _notCloseInCancel = notCloseInCancel;
            Message.Text = message;
            if (IsInformation)
            {
                this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
                this.OKButton.Content = Galatee.Silverlight.Resources.Langue.ok;
            }
            else
            {
                if (IsInterogation)
                {
                    this.OKButton.Content = Galatee.Silverlight.Resources.Langue.Oui;
                    this.CancelButton.Content = Galatee.Silverlight.Resources.Langue.Non;
                }
            }
        }

        public DialogResult(string message, string title, bool IsInterogation)
        {
            InitializeComponent();
            Title = title;
            Message.Text = message;
            _IsInterrogation = IsInterogation;
            this.OKButton.Content = Langue.Oui;
            this.CancelButton.Content = Langue.Non;
        }

        void CreateMsg(string message)
        {
            try
            {
                double width = Message.Width;
            }
            catch (Exception ex )
            {
                
                throw ex;
            }
        }

        public DialogResult(string message, string title)
        {
            InitializeComponent();
            Title = title;
            Message.Text = message;
            _IsInterrogation = false;

            this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
            this.OKButton.Content = Langue.ok;
            OKButton.HorizontalAlignment = HorizontalAlignment.Center;

        }

        public DialogResult(string message, bool notCloseInCancel, bool IsInformation, bool IsInterogation)
        {
            InitializeComponent();
            _IsInformation = IsInformation;
            _notCloseInCancel = notCloseInCancel;
            Message.Text = message;
            if (IsInformation)
            {
                this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
                this.OKButton.Content = "Ok";
            }
            else
            {
                if (IsInterogation)
                {
                    this.OKButton.Content = "Yes";
                    this.CancelButton.Content = "No";
                }
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _isClickOK = true;
                Yes = true;
                if (_IsInterrogation)
                {
                    this.DialogResult = true;
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
            _isClickOK = false;
            //Closed(this, new EventArgs());
            //this.DialogResult = false;
            if (_IsInterrogation)
                Closed(this, new EventArgs());
            this.DialogResult = true;
        }
    }
}

