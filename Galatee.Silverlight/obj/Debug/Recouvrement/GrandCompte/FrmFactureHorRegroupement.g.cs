﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Recouvrement\GrandCompte\FrmFactureHorRegroupement.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C0F414D1AA10D0C8415FA1CBF4CD7B2C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using SilverlightContrib.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Galatee.Silverlight.Recouvrement {
    
    
    public partial class FrmFactureHorRegroupement : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal SilverlightContrib.Controls.GroupBox groupBox1_Copy1;
        
        internal System.Windows.Controls.DataGrid dg_facture;
        
        internal System.Windows.Controls.TextBox txt_centre;
        
        internal System.Windows.Controls.TextBox txt_ordre;
        
        internal System.Windows.Controls.TextBox txt_client;
        
        internal System.Windows.Controls.TextBox txt_periode;
        
        internal System.Windows.Controls.ProgressBar progressBar1;
        
        internal System.Windows.Controls.TextBox txt_site;
        
        internal System.Windows.Controls.ComboBox cbo_Site;
        
        internal System.Windows.Controls.ComboBox cbo_centre;
        
        internal System.Windows.Controls.TextBox txt_Num_Fac;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Recouvrement/GrandCompte/FrmFactureHorRegroupement" +
                        ".xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.groupBox1_Copy1 = ((SilverlightContrib.Controls.GroupBox)(this.FindName("groupBox1_Copy1")));
            this.dg_facture = ((System.Windows.Controls.DataGrid)(this.FindName("dg_facture")));
            this.txt_centre = ((System.Windows.Controls.TextBox)(this.FindName("txt_centre")));
            this.txt_ordre = ((System.Windows.Controls.TextBox)(this.FindName("txt_ordre")));
            this.txt_client = ((System.Windows.Controls.TextBox)(this.FindName("txt_client")));
            this.txt_periode = ((System.Windows.Controls.TextBox)(this.FindName("txt_periode")));
            this.progressBar1 = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar1")));
            this.txt_site = ((System.Windows.Controls.TextBox)(this.FindName("txt_site")));
            this.cbo_Site = ((System.Windows.Controls.ComboBox)(this.FindName("cbo_Site")));
            this.cbo_centre = ((System.Windows.Controls.ComboBox)(this.FindName("cbo_centre")));
            this.txt_Num_Fac = ((System.Windows.Controls.TextBox)(this.FindName("txt_Num_Fac")));
        }
    }
}

