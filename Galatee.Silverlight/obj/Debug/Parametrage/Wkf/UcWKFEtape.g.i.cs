﻿#pragma checksum "D:\SPRINT 1 ZEG\IwebsEDMSA_Github\Galatee.Silverlight\Parametrage\Wkf\UcWKFEtape.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9414483FE11253F785721648032342AB"
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


namespace Galatee.Silverlight.Parametrage {
    
    
    public partial class UcWKFEtape : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox OperationGroupBox;
        
        internal System.Windows.Controls.TextBox txtCode;
        
        internal System.Windows.Controls.TextBox txtNom;
        
        internal System.Windows.Controls.TextBox txtDescription;
        
        internal System.Windows.Controls.ComboBox cmbFormulaire;
        
        internal System.Windows.Controls.CheckBox chkboxConsultationSeulement;
        
        internal System.Windows.Controls.CheckBox chkTraitementParLot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Parametrage/Wkf/UcWKFEtape.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.OperationGroupBox = ((SilverlightContrib.Controls.GroupBox)(this.FindName("OperationGroupBox")));
            this.txtCode = ((System.Windows.Controls.TextBox)(this.FindName("txtCode")));
            this.txtNom = ((System.Windows.Controls.TextBox)(this.FindName("txtNom")));
            this.txtDescription = ((System.Windows.Controls.TextBox)(this.FindName("txtDescription")));
            this.cmbFormulaire = ((System.Windows.Controls.ComboBox)(this.FindName("cmbFormulaire")));
            this.chkboxConsultationSeulement = ((System.Windows.Controls.CheckBox)(this.FindName("chkboxConsultationSeulement")));
            this.chkTraitementParLot = ((System.Windows.Controls.CheckBox)(this.FindName("chkTraitementParLot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}

