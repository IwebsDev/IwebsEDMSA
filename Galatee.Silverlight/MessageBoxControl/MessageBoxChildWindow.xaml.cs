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
using System.Windows.Media.Imaging;

namespace MessageBoxControl
{
    public partial class MessageBoxChildWindow : ChildWindow
    {
        public delegate void MessageBoxClosedDelegate(MessageBoxResult result);
        public event  EventHandler OnMessageBoxClosed;
        public MessageBoxResult Result { get; set; }
        private Exception execption = null;
        private string ErrorSource = string.Empty;
        public MessageBoxChildWindow()
        {
            InitializeComponent();
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);
        }

        public MessageBoxChildWindow(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent();
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);

            this.Title = title;
            this.txtMsg.Text = message;
            DisplayButtons(buttons);
            DisplayIcon(icon);
        }

        public MessageBoxChildWindow(string title, Exception message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            try
            {
                InitializeComponent();
                this.HasCloseButton = false;
                this.Closed += new EventHandler(MessageBoxChildWindow_Closed);
                this.ErrorSource = Galatee.Silverlight.ErrorManager.ExceptionSource(message);
                this.Title = title;
                this.execption = message;
                this.txtMsg.Text = message.Message;
                DisplayButtons(buttons);
                DisplayIcon(icon);
            } 
            catch (Exception ex)
            {
                MessageBox.Show("MessageBoxChildWindow");
            }
        }

        public MessageBoxChildWindow(string title, string message)
        {
          
            InitializeComponent();
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);

            this.Title = title;
            this.txtMsg.Text = message;
            DisplayButtons(buttons);
            DisplayIcon(icon);
        }

        public MessageBoxChildWindow(string title, Exception  exc)
        {

            InitializeComponent();
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);
            this.ErrorSource = Galatee.Silverlight.ErrorManager.ExceptionSource(exc);
            this.Title = title;
            this.execption = exc;
            this.txtMsg.Text = exc.Message;
            DisplayButtons(buttons);
            DisplayIcon(icon);
        }

        private void DisplayButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.Ok:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.OkCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.YesNo:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.YesNoCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    break;
            }
        }
        private void DisplayButtonsNoResult(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.Ok:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.OkCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.YesNo:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    break;

                case MessageBoxButtons.YesNoCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    break;
            }
        }

        private void DisplayIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    imgIcon.Source = new BitmapImage(new Uri("error.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Information:
                    imgIcon.Source = new BitmapImage(new Uri("info.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Question:
                    imgIcon.Source = new BitmapImage(new Uri("question.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Warning:
                    imgIcon.Source = new BitmapImage(new Uri("warning.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.None:
                    imgIcon.Source = new BitmapImage(new Uri("info.png", UriKind.Relative));
                    break;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (btnYes.Content.ToString().ToLower().Equals("yes") == true)
            {
                //yes button
                this.Result = MessageBoxResult.Yes;
            }
            else
            {
                //ok button
                this.Result = MessageBoxResult.OK;
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;
            this.Close();
        }

        private void MessageBoxChildWindow_Closed(object sender, EventArgs e)
        {
            if (OnMessageBoxClosed != null)
                OnMessageBoxClosed(sender,e);
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void hyperlinkButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Exception is triggered from " +
                    ErrorSource + " method");
            //ToolTipService.SetToolTip((HyperlinkButton)sender,"Exception is triggered from "+
            //        ErrorSource + " method");
            //hyperlinkButton1.Content = ErrorSource;
        }
    }

    public enum MessageBoxButtons
    {
        Ok, YesNo, YesNoCancel, OkCancel
    }

    public enum MessageBoxIcon
    {
        Question, Information, Error, None, Warning
    }
}

