﻿#pragma checksum "D:\SPRINT 1 ZEG\IwebsEDMSA_Github\Galatee.Silverlight\Devis\UcAutreMateriel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8714AFAB28AD9246EC53DA6A9CCAB124"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace Galatee.Silverlight.Devis {
    
    
    public partial class UcAutreMateriel : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBox txtLibelle;
        
        internal System.Windows.Controls.TextBox txtNombre;
        
        internal System.Windows.Controls.TextBox txtLivre;
        
        internal System.Windows.Controls.ComboBox cboTypeMateriel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Devis/UcAutreMateriel.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.txtLibelle = ((System.Windows.Controls.TextBox)(this.FindName("txtLibelle")));
            this.txtNombre = ((System.Windows.Controls.TextBox)(this.FindName("txtNombre")));
            this.txtLivre = ((System.Windows.Controls.TextBox)(this.FindName("txtLivre")));
            this.cboTypeMateriel = ((System.Windows.Controls.ComboBox)(this.FindName("cboTypeMateriel")));
        }
    }
}

