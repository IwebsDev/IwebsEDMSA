﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Devis\UcSaisiCompteurBta.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F830CD60E2F893D023222500B8DCBEF2"
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
    
    
    public partial class UcSaisiCompteur : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationDevis;
        
        internal System.Windows.Controls.TextBox txt_ANNEEFAB;
        
        internal System.Windows.Controls.ComboBox Cbo_Etat_cmpt;
        
        internal System.Windows.Controls.ComboBox Cbo_Marque;
        
        internal System.Windows.Controls.ComboBox Cbo_Diametre;
        
        internal System.Windows.Controls.ComboBox Cbo_typeCmpt;
        
        internal System.Windows.Controls.ComboBox Cbo_Modele;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBox txt_Cadran;
        
        internal System.Windows.Controls.TextBox txt_NumCpteur;
        
        internal System.Windows.Controls.DataGrid dtg_CompteurSaisie;
        
        internal System.Windows.Controls.TextBox txt_CodeProduit;
        
        internal System.Windows.Controls.Button btn_Ajouter;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Devis/UcSaisiCompteurBta.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Gbo_InformationDevis = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationDevis")));
            this.txt_ANNEEFAB = ((System.Windows.Controls.TextBox)(this.FindName("txt_ANNEEFAB")));
            this.Cbo_Etat_cmpt = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Etat_cmpt")));
            this.Cbo_Marque = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Marque")));
            this.Cbo_Diametre = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Diametre")));
            this.Cbo_typeCmpt = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_typeCmpt")));
            this.Cbo_Modele = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Modele")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.txt_Cadran = ((System.Windows.Controls.TextBox)(this.FindName("txt_Cadran")));
            this.txt_NumCpteur = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumCpteur")));
            this.dtg_CompteurSaisie = ((System.Windows.Controls.DataGrid)(this.FindName("dtg_CompteurSaisie")));
            this.txt_CodeProduit = ((System.Windows.Controls.TextBox)(this.FindName("txt_CodeProduit")));
            this.btn_Ajouter = ((System.Windows.Controls.Button)(this.FindName("btn_Ajouter")));
        }
    }
}

