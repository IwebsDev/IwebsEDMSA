﻿#pragma checksum "D:\SPRINT 1 ZEG\IwebsEDMSA_Github\Galatee.Silverlight\Accueil\FrmRechercherDemande.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BC1B1E4C110BC76CC6BE7167A3A51FF5"
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


namespace Galatee.Silverlight.Accueil {
    
    
    public partial class FrmRechercherDemande : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DataGrid dgt_Typedemande;
        
        internal System.Windows.Controls.CheckBox chk_EnCaisse;
        
        internal System.Windows.Controls.TextBox txt_NumDemande;
        
        internal System.Windows.Controls.Label lbl_Demande;
        
        internal System.Windows.Controls.Button btn_rechercher;
        
        internal System.Windows.Controls.CheckBox chk_Terminer;
        
        internal System.Windows.Controls.CheckBox chk_EnAttente;
        
        internal System.Windows.Controls.Label lbl_Centre_Copy;
        
        internal System.Windows.Controls.TextBox Txt_CodeCentre;
        
        internal System.Windows.Controls.Button btn_Centre;
        
        internal System.Windows.Controls.Label lbl_Centre_Copy1;
        
        internal System.Windows.Controls.TextBox Txt_CodeSite;
        
        internal System.Windows.Controls.TextBox Txt_LibelleSite;
        
        internal System.Windows.Controls.Button btn_Site;
        
        internal System.Windows.Controls.TextBox Txt_LibelleCentre;
        
        internal System.Windows.Controls.Label lbl_date;
        
        internal System.Windows.Controls.DatePicker Dtp_Date;
        
        internal System.Windows.Controls.Label lbl_date_Copy;
        
        internal System.Windows.Controls.DatePicker Dtp_DateFin;
        
        internal System.Windows.Controls.Label lbl_date_Copy1;
        
        internal System.Windows.Controls.DatePicker Dtp_DateDebut;
        
        internal System.Windows.Controls.TextBox txt_NumDemandeDebut;
        
        internal System.Windows.Controls.Label lbl_date_Copy2;
        
        internal System.Windows.Controls.Label lbl_date_Copy3;
        
        internal System.Windows.Controls.TextBox txt_NumDemandeFin;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationAdresse;
        
        internal System.Windows.Controls.Label label13;
        
        internal System.Windows.Controls.TextBox txt_Commune;
        
        internal System.Windows.Controls.ComboBox Cbo_Commune;
        
        internal System.Windows.Controls.Label label14;
        
        internal System.Windows.Controls.TextBox txt_Quartier;
        
        internal System.Windows.Controls.ComboBox Cbo_Quartier;
        
        internal System.Windows.Controls.Label label15;
        
        internal System.Windows.Controls.TextBox txt_NumRue;
        
        internal System.Windows.Controls.Label label15_Copy;
        
        internal System.Windows.Controls.TextBox txt_NumSecteur;
        
        internal System.Windows.Controls.ComboBox Cbo_Secteur;
        
        internal System.Windows.Controls.TextBox Txt_Etage;
        
        internal System.Windows.Controls.Label lbl_Etage;
        
        internal System.Windows.Controls.TextBox Txt_Porte;
        
        internal System.Windows.Controls.Label label17_Copy;
        
        internal System.Windows.Controls.TextBox txtPropriete;
        
        internal System.Windows.Controls.Label label20;
        
        internal System.Windows.Controls.TextBox txtCompteur;
        
        internal System.Windows.Controls.Label label20_Copy;
        
        internal System.Windows.Controls.TextBox txtNomClient;
        
        internal System.Windows.Controls.Label label20_Copy1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Accueil/FrmRechercherDemande.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dgt_Typedemande = ((System.Windows.Controls.DataGrid)(this.FindName("dgt_Typedemande")));
            this.chk_EnCaisse = ((System.Windows.Controls.CheckBox)(this.FindName("chk_EnCaisse")));
            this.txt_NumDemande = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumDemande")));
            this.lbl_Demande = ((System.Windows.Controls.Label)(this.FindName("lbl_Demande")));
            this.btn_rechercher = ((System.Windows.Controls.Button)(this.FindName("btn_rechercher")));
            this.chk_Terminer = ((System.Windows.Controls.CheckBox)(this.FindName("chk_Terminer")));
            this.chk_EnAttente = ((System.Windows.Controls.CheckBox)(this.FindName("chk_EnAttente")));
            this.lbl_Centre_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_Centre_Copy")));
            this.Txt_CodeCentre = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeCentre")));
            this.btn_Centre = ((System.Windows.Controls.Button)(this.FindName("btn_Centre")));
            this.lbl_Centre_Copy1 = ((System.Windows.Controls.Label)(this.FindName("lbl_Centre_Copy1")));
            this.Txt_CodeSite = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeSite")));
            this.Txt_LibelleSite = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleSite")));
            this.btn_Site = ((System.Windows.Controls.Button)(this.FindName("btn_Site")));
            this.Txt_LibelleCentre = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleCentre")));
            this.lbl_date = ((System.Windows.Controls.Label)(this.FindName("lbl_date")));
            this.Dtp_Date = ((System.Windows.Controls.DatePicker)(this.FindName("Dtp_Date")));
            this.lbl_date_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_date_Copy")));
            this.Dtp_DateFin = ((System.Windows.Controls.DatePicker)(this.FindName("Dtp_DateFin")));
            this.lbl_date_Copy1 = ((System.Windows.Controls.Label)(this.FindName("lbl_date_Copy1")));
            this.Dtp_DateDebut = ((System.Windows.Controls.DatePicker)(this.FindName("Dtp_DateDebut")));
            this.txt_NumDemandeDebut = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumDemandeDebut")));
            this.lbl_date_Copy2 = ((System.Windows.Controls.Label)(this.FindName("lbl_date_Copy2")));
            this.lbl_date_Copy3 = ((System.Windows.Controls.Label)(this.FindName("lbl_date_Copy3")));
            this.txt_NumDemandeFin = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumDemandeFin")));
            this.Gbo_InformationAdresse = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationAdresse")));
            this.label13 = ((System.Windows.Controls.Label)(this.FindName("label13")));
            this.txt_Commune = ((System.Windows.Controls.TextBox)(this.FindName("txt_Commune")));
            this.Cbo_Commune = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Commune")));
            this.label14 = ((System.Windows.Controls.Label)(this.FindName("label14")));
            this.txt_Quartier = ((System.Windows.Controls.TextBox)(this.FindName("txt_Quartier")));
            this.Cbo_Quartier = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Quartier")));
            this.label15 = ((System.Windows.Controls.Label)(this.FindName("label15")));
            this.txt_NumRue = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumRue")));
            this.label15_Copy = ((System.Windows.Controls.Label)(this.FindName("label15_Copy")));
            this.txt_NumSecteur = ((System.Windows.Controls.TextBox)(this.FindName("txt_NumSecteur")));
            this.Cbo_Secteur = ((System.Windows.Controls.ComboBox)(this.FindName("Cbo_Secteur")));
            this.Txt_Etage = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Etage")));
            this.lbl_Etage = ((System.Windows.Controls.Label)(this.FindName("lbl_Etage")));
            this.Txt_Porte = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Porte")));
            this.label17_Copy = ((System.Windows.Controls.Label)(this.FindName("label17_Copy")));
            this.txtPropriete = ((System.Windows.Controls.TextBox)(this.FindName("txtPropriete")));
            this.label20 = ((System.Windows.Controls.Label)(this.FindName("label20")));
            this.txtCompteur = ((System.Windows.Controls.TextBox)(this.FindName("txtCompteur")));
            this.label20_Copy = ((System.Windows.Controls.Label)(this.FindName("label20_Copy")));
            this.txtNomClient = ((System.Windows.Controls.TextBox)(this.FindName("txtNomClient")));
            this.label20_Copy1 = ((System.Windows.Controls.Label)(this.FindName("label20_Copy1")));
        }
    }
}

