﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Devis\UcEtablissementDevis.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1E40443F930A562080DBFE5B9A101150"
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


namespace Galatee.Silverlight.Devis {
    
    
    public partial class UcEtablissementDevis : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ContextMenu MenuContextuel;
        
        internal System.Windows.Controls.MenuItem MenuContextuelAjouter;
        
        internal System.Windows.Controls.MenuItem MenuContextuelSupprimer;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationAccount;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationDevis;
        
        internal System.Windows.Controls.TextBox Txt_NumDevis;
        
        internal System.Windows.Controls.Label lab_AmountOfDeposit;
        
        internal System.Windows.Controls.TextBox Txt_TypeDevis;
        
        internal System.Windows.Controls.TextBox Txt_Distance;
        
        internal System.Windows.Controls.Label lab_Decision;
        
        internal System.Windows.Controls.CheckBox Chk_DossierComplet;
        
        internal System.Windows.Controls.CheckBox Chk_DossierImcomplet;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button BtnTransmettre;
        
        internal System.Windows.Controls.DataGrid dataGridElementDevis;
        
        internal System.Windows.Controls.Button Btn_Ajouter;
        
        internal System.Windows.Controls.Button Btn_Supprimer;
        
        internal System.Windows.Controls.Button button2;
        
        internal System.Windows.Controls.TextBox Txt_PrixUnitaire;
        
        internal System.Windows.Controls.Label Lab_PrixUnitaire;
        
        internal System.Windows.Controls.TextBox Txt_MontantTotal;
        
        internal System.Windows.Controls.Label Lab_TotalMontant;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_Quantite;
        
        internal System.Windows.Controls.Label Lab_Quantite;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBox Txt_DistanceExtension;
        
        internal System.Windows.Controls.Label labelDistExt;
        
        internal System.Windows.Controls.Button btn_MiseEnAttente;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Devis/UcEtablissementDevis.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MenuContextuel = ((System.Windows.Controls.ContextMenu)(this.FindName("MenuContextuel")));
            this.MenuContextuelAjouter = ((System.Windows.Controls.MenuItem)(this.FindName("MenuContextuelAjouter")));
            this.MenuContextuelSupprimer = ((System.Windows.Controls.MenuItem)(this.FindName("MenuContextuelSupprimer")));
            this.Gbo_InformationAccount = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationAccount")));
            this.Gbo_InformationDevis = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationDevis")));
            this.Txt_NumDevis = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NumDevis")));
            this.lab_AmountOfDeposit = ((System.Windows.Controls.Label)(this.FindName("lab_AmountOfDeposit")));
            this.Txt_TypeDevis = ((System.Windows.Controls.TextBox)(this.FindName("Txt_TypeDevis")));
            this.Txt_Distance = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Distance")));
            this.lab_Decision = ((System.Windows.Controls.Label)(this.FindName("lab_Decision")));
            this.Chk_DossierComplet = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_DossierComplet")));
            this.Chk_DossierImcomplet = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_DossierImcomplet")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.BtnTransmettre = ((System.Windows.Controls.Button)(this.FindName("BtnTransmettre")));
            this.dataGridElementDevis = ((System.Windows.Controls.DataGrid)(this.FindName("dataGridElementDevis")));
            this.Btn_Ajouter = ((System.Windows.Controls.Button)(this.FindName("Btn_Ajouter")));
            this.Btn_Supprimer = ((System.Windows.Controls.Button)(this.FindName("Btn_Supprimer")));
            this.button2 = ((System.Windows.Controls.Button)(this.FindName("button2")));
            this.Txt_PrixUnitaire = ((System.Windows.Controls.TextBox)(this.FindName("Txt_PrixUnitaire")));
            this.Lab_PrixUnitaire = ((System.Windows.Controls.Label)(this.FindName("Lab_PrixUnitaire")));
            this.Txt_MontantTotal = ((System.Windows.Controls.TextBox)(this.FindName("Txt_MontantTotal")));
            this.Lab_TotalMontant = ((System.Windows.Controls.Label)(this.FindName("Lab_TotalMontant")));
            this.Txt_Quantite = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_Quantite")));
            this.Lab_Quantite = ((System.Windows.Controls.Label)(this.FindName("Lab_Quantite")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.Txt_DistanceExtension = ((System.Windows.Controls.TextBox)(this.FindName("Txt_DistanceExtension")));
            this.labelDistExt = ((System.Windows.Controls.Label)(this.FindName("labelDistExt")));
            this.btn_MiseEnAttente = ((System.Windows.Controls.Button)(this.FindName("btn_MiseEnAttente")));
        }
    }
}

