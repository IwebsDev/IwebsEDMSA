﻿#pragma checksum "D:\SPRINT 1 ZEG\IwebsEDMSA_Github\Galatee.Silverlight\Devis\UcProgrammationEP.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D122C7A8DDA349060DD9BBCE225B5C7C"
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


namespace Galatee.Silverlight.Devis {
    
    
    public partial class UcProgrammationEP : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationDevis;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.ComboBox cboEquipe;
        
        internal System.Windows.Controls.DataGrid dgDemande;
        
        internal System.Windows.Controls.DatePicker dtProgram;
        
        internal System.Windows.Controls.DataGrid dgDemandeAffecte;
        
        internal System.Windows.Controls.Button Charger;
        
        internal System.Windows.Controls.Button Decharger;
        
        internal System.Windows.Controls.Button chargerTout;
        
        internal System.Windows.Controls.Button DechargerTout;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Devis/UcProgrammationEP.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Gbo_InformationDevis = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationDevis")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.cboEquipe = ((System.Windows.Controls.ComboBox)(this.FindName("cboEquipe")));
            this.dgDemande = ((System.Windows.Controls.DataGrid)(this.FindName("dgDemande")));
            this.dtProgram = ((System.Windows.Controls.DatePicker)(this.FindName("dtProgram")));
            this.dgDemandeAffecte = ((System.Windows.Controls.DataGrid)(this.FindName("dgDemandeAffecte")));
            this.Charger = ((System.Windows.Controls.Button)(this.FindName("Charger")));
            this.Decharger = ((System.Windows.Controls.Button)(this.FindName("Decharger")));
            this.chargerTout = ((System.Windows.Controls.Button)(this.FindName("chargerTout")));
            this.DechargerTout = ((System.Windows.Controls.Button)(this.FindName("DechargerTout")));
        }
    }
}

