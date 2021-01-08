using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Galatee.Structure
{
    public class Monitor
    {


        public static bool StartMonitor()
        {
            return true;
        }

        public static void DisplayError(string msg)
        {
            MessageBox.Show(msg,
                "Erreur:",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void DisplayError(string msg, string titre)
        {
            MessageBox.Show(msg,
                "Erreur:" + titre,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void DisplayWarning(string msg)
        {
            MessageBox.Show(msg,"",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void DisplayWarning(string msg, string titre)
        {
            MessageBox.Show(msg, titre,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void DisplayInfos(string msg, string titre)
        {
            MessageBox.Show(msg,
                     titre,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
        }

        public static DialogResult DisplayConfirmation(string msg, string titre)
        {
            return MessageBox.Show(msg,
                     titre,
                     MessageBoxButtons.OKCancel,
                     MessageBoxIcon.Question);
        }

        public static bool DisplayInformationAvecValidation(string msg, string titre)
        {
            if (MessageBox.Show(msg,
                     titre,
                     MessageBoxButtons.OKCancel,
                     MessageBoxIcon.Information) == DialogResult.OK)
                return true;
            return false;
        }

    }
}
