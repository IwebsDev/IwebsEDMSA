﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Devis\UcBilanEtablissementDevis.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CB0D9B16E2D3AD4ABD18FFD422627C18"
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


namespace Galatee.Silverlight.Devis {
    
    
    public partial class UcBilanEtablissementDevis : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox Txt_NumDevis;
        
        internal System.Windows.Controls.Label lab_AmountOfDeposit;
        
        internal System.Windows.Controls.TextBox Txt_TypeDevis;
        
        internal System.Windows.Controls.Label lab_Decision;
        
        internal System.Windows.Controls.CheckBox Chk_DossierComplet;
        
        internal System.Windows.Controls.CheckBox Chk_DossierImcomplet;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationAccount;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationDevis;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.DataGrid dataGridElementDevis;
        
        internal System.Windows.Controls.Button Btn_Transmettre;
        
        internal System.Windows.Controls.Label lbl_TypeDevis;
        
        internal System.Windows.Controls.Button EditerButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Devis/UcBilanEtablissementDevis.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Txt_NumDevis = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NumDevis")));
            this.lab_AmountOfDeposit = ((System.Windows.Controls.Label)(this.FindName("lab_AmountOfDeposit")));
            this.Txt_TypeDevis = ((System.Windows.Controls.TextBox)(this.FindName("Txt_TypeDevis")));
            this.lab_Decision = ((System.Windows.Controls.Label)(this.FindName("lab_Decision")));
            this.Chk_DossierComplet = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_DossierComplet")));
            this.Chk_DossierImcomplet = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_DossierImcomplet")));
            this.Gbo_InformationAccount = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationAccount")));
            this.Gbo_InformationDevis = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationDevis")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.dataGridElementDevis = ((System.Windows.Controls.DataGrid)(this.FindName("dataGridElementDevis")));
            this.Btn_Transmettre = ((System.Windows.Controls.Button)(this.FindName("Btn_Transmettre")));
            this.lbl_TypeDevis = ((System.Windows.Controls.Label)(this.FindName("lbl_TypeDevis")));
            this.EditerButton = ((System.Windows.Controls.Button)(this.FindName("EditerButton")));
        }
    }
}

