﻿#pragma checksum "D:\TFS_SOURCE_EDM\iWEBS_EDMSA\Galatee.Silverlight\Caisse\FrmPayementDevis.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6ECA64557A338F9E597524E69AD46A8E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Galatee.Silverlight.Library;
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


namespace Galatee.Silverlight.Caisse {
    
    
    public partial class FrmPayementDevis : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox Gb_Credit;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBox Txt_Caissier;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_NumDevis;
        
        internal System.Windows.Controls.TextBox Txt_NomClient;
        
        internal System.Windows.Controls.TextBox Txt_NumClient;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_MontantDevis;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label lblNumerodevis;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.Label lblRefclient;
        
        internal System.Windows.Controls.Label lblMontantDevis;
        
        internal System.Windows.Controls.TextBox Txt_Avance;
        
        internal System.Windows.Controls.Label lblAvance;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_MontantTotal;
        
        internal System.Windows.Controls.Label lblMontantPaye;
        
        internal System.Windows.Controls.Button Btn_RechercheDevis;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Caisse/FrmPayementDevis.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Gb_Credit = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gb_Credit")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.Txt_Caissier = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Caissier")));
            this.Txt_NumDevis = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_NumDevis")));
            this.Txt_NomClient = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NomClient")));
            this.Txt_NumClient = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NumClient")));
            this.Txt_MontantDevis = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_MontantDevis")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.lblNumerodevis = ((System.Windows.Controls.Label)(this.FindName("lblNumerodevis")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.lblRefclient = ((System.Windows.Controls.Label)(this.FindName("lblRefclient")));
            this.lblMontantDevis = ((System.Windows.Controls.Label)(this.FindName("lblMontantDevis")));
            this.Txt_Avance = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Avance")));
            this.lblAvance = ((System.Windows.Controls.Label)(this.FindName("lblAvance")));
            this.Txt_MontantTotal = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_MontantTotal")));
            this.lblMontantPaye = ((System.Windows.Controls.Label)(this.FindName("lblMontantPaye")));
            this.Btn_RechercheDevis = ((System.Windows.Controls.Button)(this.FindName("Btn_RechercheDevis")));
        }
    }
}

