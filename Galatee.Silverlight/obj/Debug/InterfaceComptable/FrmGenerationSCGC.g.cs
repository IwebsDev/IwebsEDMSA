﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\InterfaceComptable\FrmGenerationSCGC.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A2885A1B2F9267DC097E35D0971E5F5F"
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


namespace Galatee.Silverlight.InterfaceComptable {
    
    
    public partial class FrmGenerationSCGC : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox groupBox2_Copy;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.RadioButton RdbDate;
        
        internal System.Windows.Controls.RadioButton RdbIntervalle;
        
        internal System.Windows.Controls.DatePicker dtpDateDebut;
        
        internal System.Windows.Controls.DatePicker dtpDateFin;
        
        internal System.Windows.Controls.DatePicker dtpDateCaisse;
        
        internal System.Windows.Controls.TabItem TBOperationClient;
        
        internal System.Windows.Controls.DataGrid DTOperationClientele;
        
        internal System.Windows.Controls.TextBox txt_total;
        
        internal System.Windows.Controls.TabItem TBEcritureComptable;
        
        internal System.Windows.Controls.DataGrid DTEcritureComptableFacture;
        
        internal System.Windows.Controls.TextBox txt_Debit;
        
        internal System.Windows.Controls.TextBox txt_Credit;
        
        internal System.Windows.Controls.Button ValiderButton;
        
        internal System.Windows.Controls.Button GenererButton;
        
        internal System.Windows.Controls.Button EditerButton;
        
        internal System.Windows.Controls.RadioButton rdbEncaisse;
        
        internal System.Windows.Controls.ProgressBar prgBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/InterfaceComptable/FrmGenerationSCGC.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.groupBox2_Copy = ((SilverlightContrib.Controls.GroupBox)(this.FindName("groupBox2_Copy")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.RdbDate = ((System.Windows.Controls.RadioButton)(this.FindName("RdbDate")));
            this.RdbIntervalle = ((System.Windows.Controls.RadioButton)(this.FindName("RdbIntervalle")));
            this.dtpDateDebut = ((System.Windows.Controls.DatePicker)(this.FindName("dtpDateDebut")));
            this.dtpDateFin = ((System.Windows.Controls.DatePicker)(this.FindName("dtpDateFin")));
            this.dtpDateCaisse = ((System.Windows.Controls.DatePicker)(this.FindName("dtpDateCaisse")));
            this.TBOperationClient = ((System.Windows.Controls.TabItem)(this.FindName("TBOperationClient")));
            this.DTOperationClientele = ((System.Windows.Controls.DataGrid)(this.FindName("DTOperationClientele")));
            this.txt_total = ((System.Windows.Controls.TextBox)(this.FindName("txt_total")));
            this.TBEcritureComptable = ((System.Windows.Controls.TabItem)(this.FindName("TBEcritureComptable")));
            this.DTEcritureComptableFacture = ((System.Windows.Controls.DataGrid)(this.FindName("DTEcritureComptableFacture")));
            this.txt_Debit = ((System.Windows.Controls.TextBox)(this.FindName("txt_Debit")));
            this.txt_Credit = ((System.Windows.Controls.TextBox)(this.FindName("txt_Credit")));
            this.ValiderButton = ((System.Windows.Controls.Button)(this.FindName("ValiderButton")));
            this.GenererButton = ((System.Windows.Controls.Button)(this.FindName("GenererButton")));
            this.EditerButton = ((System.Windows.Controls.Button)(this.FindName("EditerButton")));
            this.rdbEncaisse = ((System.Windows.Controls.RadioButton)(this.FindName("rdbEncaisse")));
            this.prgBar = ((System.Windows.Controls.ProgressBar)(this.FindName("prgBar")));
        }
    }
}

