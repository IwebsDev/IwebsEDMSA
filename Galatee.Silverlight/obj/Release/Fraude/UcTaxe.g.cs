﻿#pragma checksum "D:\TFS_SOURCE_EDM\iWEBS_EDMSA\Galatee.Silverlight\Fraude\UcTaxe.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5264BF7E365FED00CA82F8ADAE4BAD3B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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


namespace Galatee.Silverlight.Fraude {
    
    
    public partial class UcTaxe : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_SourceControle;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBox txt_Produit;
        
        internal System.Windows.Controls.ComboBox Cbo_Taxe;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Fraude/UcTaxe.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Gbo_SourceControle = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_SourceControle")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.txt_Produit = ((System.Windows.Controls.TextBox)(this.FindName("txt_Produit")));
            this.Cbo_Taxe = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Taxe")));
        }
    }
}

