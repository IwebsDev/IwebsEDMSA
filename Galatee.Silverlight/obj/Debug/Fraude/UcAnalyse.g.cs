﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Fraude\UcAnalyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38D183965932A867E11588A5E1F06068"
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


namespace Galatee.Silverlight.Fraude {
    
    
    public partial class UcAnalyse : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_Traitement;
        
        internal System.Windows.Controls.DataGrid dtgrdAnnalyse;
        
        internal System.Windows.Controls.TextBox txtConsommationEstimeeEquipement;
        
        internal System.Windows.Controls.TextBox txtRetrogradation;
        
        internal System.Windows.Controls.TextBox txtTotalEstime;
        
        internal System.Windows.Controls.TextBox txtConsommationDejaFacturee;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_Cosoestime;
        
        internal System.Windows.Controls.TextBox txtConsommationAFacturer;
        
        internal System.Windows.Controls.CheckBox ckbFraudeConfirmée;
        
        internal System.Windows.Controls.ComboBox Cbo_Decision;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Fraude/UcAnalyse.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.Gbo_Traitement = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_Traitement")));
            this.dtgrdAnnalyse = ((System.Windows.Controls.DataGrid)(this.FindName("dtgrdAnnalyse")));
            this.txtConsommationEstimeeEquipement = ((System.Windows.Controls.TextBox)(this.FindName("txtConsommationEstimeeEquipement")));
            this.txtRetrogradation = ((System.Windows.Controls.TextBox)(this.FindName("txtRetrogradation")));
            this.txtTotalEstime = ((System.Windows.Controls.TextBox)(this.FindName("txtTotalEstime")));
            this.txtConsommationDejaFacturee = ((System.Windows.Controls.TextBox)(this.FindName("txtConsommationDejaFacturee")));
            this.Gbo_Cosoestime = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_Cosoestime")));
            this.txtConsommationAFacturer = ((System.Windows.Controls.TextBox)(this.FindName("txtConsommationAFacturer")));
            this.ckbFraudeConfirmée = ((System.Windows.Controls.CheckBox)(this.FindName("ckbFraudeConfirmée")));
            this.Cbo_Decision = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Decision")));
        }
    }
}

