using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight
{
    public static class Message 
    {
        public static bool DialogResult;
        public static event EventHandler Closed;
        public static bool DialogResultOk;

        /// <summary>
        /// Permet l'affichage des messages sans retour 
        /// </summary>
        /// <param name="message">message à afficher </param>
        /// <param name="title">Titre du l'affichage</param>
        public static void Show(string message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowInformation(string message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowInformation(Exception message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowError(string message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowError(Exception message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowWarning(string message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void ShowWarning(Exception message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        public static void Show(Exception message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title);
            //smgForm.Title = title;
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                    DialogResultOk = true;
            };
            w.Show();
        }

        /// <summary>
        /// Permet l'affichage des messages avec retour
        /// </summary>
        /// <param name="message">message à afficher </param>
        /// <param name="title">Titre du l'affichage</param>
        /// <param name="pYesAndNoButton"></param>
        /// <param name="pRetour"></param>
        public static void Question(string message, string title)
        {
            //DialogResult smgForm = new DialogResult(message, title, pYesAndNoButton);
            //smgForm.Title = title;
            //smgForm.Closed+=new EventHandler(smgForm_Closed);
            //smgForm.Show();

            var w = new MessageBoxControl.MessageBoxChildWindow(title, message, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.OK)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            };
            w.Show();
        }

        private static void smgForm_Closed(object sender, EventArgs e)
        {
            if(sender != null)
            {
                var dialogResult = ((DialogResult) sender).DialogResult;
                if (dialogResult != null)
                    DialogResult = (bool) dialogResult;
            }
        }

    }
}
