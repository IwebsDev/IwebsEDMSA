﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Parametrage\Parametrage\UcMonnaie.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4215EF47746BFAD7595DF71A4000C154"
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


namespace Galatee.Silverlight.Parametrage {
    
    
    public partial class UcMonnaie : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Label lab_Code;
        
        internal System.Windows.Controls.TextBox Txt_Code;
        
        internal System.Windows.Controls.Label lab_Libelle;
        
        internal System.Windows.Controls.TextBox Txt_Libelle;
        
        internal System.Windows.Controls.Button btnOk;
        
        internal System.Windows.Controls.Button Btn_Reinitialiser;
        
        internal SilverlightContrib.Controls.GroupBox GboCodeDepart;
        
        internal System.Windows.Controls.ComboBox CboCentre;
        
        internal System.Windows.Controls.ComboBox CboSupport;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Parametrage/Parametrage/UcMonnaie.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lab_Code = ((System.Windows.Controls.Label)(this.FindName("lab_Code")));
            this.Txt_Code = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Code")));
            this.lab_Libelle = ((System.Windows.Controls.Label)(this.FindName("lab_Libelle")));
            this.Txt_Libelle = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Libelle")));
            this.btnOk = ((System.Windows.Controls.Button)(this.FindName("btnOk")));
            this.Btn_Reinitialiser = ((System.Windows.Controls.Button)(this.FindName("Btn_Reinitialiser")));
            this.GboCodeDepart = ((SilverlightContrib.Controls.GroupBox)(this.FindName("GboCodeDepart")));
            this.CboCentre = ((System.Windows.Controls.ComboBox)(this.FindName("CboCentre")));
            this.CboSupport = ((System.Windows.Controls.ComboBox)(this.FindName("CboSupport")));
        }
    }
}

