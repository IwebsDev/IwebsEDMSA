﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Parametrage\Parametrage\UcCaisse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "35137197250291A33CAB7A9CD6CCF9CC"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
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


namespace Galatee.Silverlight.Parametrage {
    
    
    public partial class UcCaisse : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox groupBox5;
        
        internal System.Windows.Controls.Label lab_Code;
        
        internal System.Windows.Controls.TextBox Txt_Code;
        
        internal System.Windows.Controls.Label lab_Libelle;
        
        internal System.Windows.Controls.TextBox Txt_Libelle;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button btnV;
        
        internal System.Windows.Controls.Label lbl_Centre;
        
        internal System.Windows.Controls.TextBox Txt_CodeCentre;
        
        internal System.Windows.Controls.TextBox Txt_LibelleCentre;
        
        internal System.Windows.Controls.Button btn_Centre;
        
        internal System.Windows.Controls.Label lbl_Centre_Copy;
        
        internal System.Windows.Controls.TextBox Txt_CodeSite;
        
        internal System.Windows.Controls.TextBox Txt_LibelleSite;
        
        internal System.Windows.Controls.Button btn_Site;
        
        internal System.Windows.Controls.Label lab_fondCaisse;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_FondDeCaisse;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Parametrage/Parametrage/UcCaisse.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.groupBox5 = ((SilverlightContrib.Controls.GroupBox)(this.FindName("groupBox5")));
            this.lab_Code = ((System.Windows.Controls.Label)(this.FindName("lab_Code")));
            this.Txt_Code = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Code")));
            this.lab_Libelle = ((System.Windows.Controls.Label)(this.FindName("lab_Libelle")));
            this.Txt_Libelle = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Libelle")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.btnV = ((System.Windows.Controls.Button)(this.FindName("btnV")));
            this.lbl_Centre = ((System.Windows.Controls.Label)(this.FindName("lbl_Centre")));
            this.Txt_CodeCentre = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeCentre")));
            this.Txt_LibelleCentre = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleCentre")));
            this.btn_Centre = ((System.Windows.Controls.Button)(this.FindName("btn_Centre")));
            this.lbl_Centre_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_Centre_Copy")));
            this.Txt_CodeSite = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeSite")));
            this.Txt_LibelleSite = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleSite")));
            this.btn_Site = ((System.Windows.Controls.Button)(this.FindName("btn_Site")));
            this.lab_fondCaisse = ((System.Windows.Controls.Label)(this.FindName("lab_fondCaisse")));
            this.Txt_FondDeCaisse = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_FondDeCaisse")));
        }
    }
}
