﻿#pragma checksum "D:\TFS_SOURCE_EDM\iWEBS_EDMSA\Galatee.Silverlight\Accueil\FrmModificationAdresse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "74CE64AB3DA991169A1E763621F72B0A"
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
    
    
    public partial class FrmModificationAdresse : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.ProgressBar prgBar;
        
        internal System.Windows.Controls.TextBox Txt_NomClient;
        
        internal System.Windows.Controls.Label lbl_NomProprietaire;
        
        internal System.Windows.Controls.TextBox Txt_CodeCommune;
        
        internal System.Windows.Controls.Label lbl_Commune;
        
        internal System.Windows.Controls.TextBox Txt_CodeQuartier;
        
        internal System.Windows.Controls.Label lbl_Quartier;
        
        internal System.Windows.Controls.Button btn_Quartier;
        
        internal System.Windows.Controls.TextBox Txt_CodeSecteur;
        
        internal System.Windows.Controls.Label lbl_Secteur;
        
        internal System.Windows.Controls.Button btn_Secteur;
        
        internal System.Windows.Controls.TextBox Txt_CodeNomRue;
        
        internal System.Windows.Controls.Label lbl_Rue;
        
        internal System.Windows.Controls.TextBox Txt_LibelleCommune;
        
        internal System.Windows.Controls.TextBox Txt_LibelleQuartier;
        
        internal System.Windows.Controls.TextBox Txt_LibelleSecteur;
        
        internal System.Windows.Controls.TextBox Txt_Etage;
        
        internal System.Windows.Controls.Label lbl_Etage;
        
        internal System.Windows.Controls.TextBox Txt_OrdreTour;
        
        internal System.Windows.Controls.Label lbl_Sequence;
        
        internal System.Windows.Controls.Button btn_Commune;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_PieceJointe;
        
        internal System.Windows.Controls.DataGrid dgListePiece;
        
        internal System.Windows.Controls.Button btn_ajoutpiece;
        
        internal System.Windows.Controls.Button btn_supprimerpiece;
        
        internal System.Windows.Controls.ComboBox cbo_typedoc;
        
        internal System.Windows.Controls.TextBox Txt_Tournee;
        
        internal System.Windows.Controls.Label lbl_Tournee;
        
        internal System.Windows.Controls.Button btn_zone;
        
        internal SilverlightContrib.Controls.GroupBox gbo_typefact_Copy1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.TextBox txtSite;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.TextBox txtCentre;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.TextBox txt_tdem;
        
        internal System.Windows.Controls.Button btn_RechercheClient;
        
        internal System.Windows.Controls.Label lbl_NumerodeDemande;
        
        internal System.Windows.Controls.TextBox Txt_ReferenceClient;
        
        internal System.Windows.Controls.TextBox Txt_Motif;
        
        internal System.Windows.Controls.Label lbl_Etage_Copy;
        
        internal System.Windows.Controls.TextBox Txt_Porte;
        
        internal System.Windows.Controls.TextBox Txt_MotifRejet;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Accueil/FrmModificationAdresse.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.prgBar = ((System.Windows.Controls.ProgressBar)(this.FindName("prgBar")));
            this.Txt_NomClient = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NomClient")));
            this.lbl_NomProprietaire = ((System.Windows.Controls.Label)(this.FindName("lbl_NomProprietaire")));
            this.Txt_CodeCommune = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeCommune")));
            this.lbl_Commune = ((System.Windows.Controls.Label)(this.FindName("lbl_Commune")));
            this.Txt_CodeQuartier = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeQuartier")));
            this.lbl_Quartier = ((System.Windows.Controls.Label)(this.FindName("lbl_Quartier")));
            this.btn_Quartier = ((System.Windows.Controls.Button)(this.FindName("btn_Quartier")));
            this.Txt_CodeSecteur = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeSecteur")));
            this.lbl_Secteur = ((System.Windows.Controls.Label)(this.FindName("lbl_Secteur")));
            this.btn_Secteur = ((System.Windows.Controls.Button)(this.FindName("btn_Secteur")));
            this.Txt_CodeNomRue = ((System.Windows.Controls.TextBox)(this.FindName("Txt_CodeNomRue")));
            this.lbl_Rue = ((System.Windows.Controls.Label)(this.FindName("lbl_Rue")));
            this.Txt_LibelleCommune = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleCommune")));
            this.Txt_LibelleQuartier = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleQuartier")));
            this.Txt_LibelleSecteur = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibelleSecteur")));
            this.Txt_Etage = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Etage")));
            this.lbl_Etage = ((System.Windows.Controls.Label)(this.FindName("lbl_Etage")));
            this.Txt_OrdreTour = ((System.Windows.Controls.TextBox)(this.FindName("Txt_OrdreTour")));
            this.lbl_Sequence = ((System.Windows.Controls.Label)(this.FindName("lbl_Sequence")));
            this.btn_Commune = ((System.Windows.Controls.Button)(this.FindName("btn_Commune")));
            this.Gbo_PieceJointe = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_PieceJointe")));
            this.dgListePiece = ((System.Windows.Controls.DataGrid)(this.FindName("dgListePiece")));
            this.btn_ajoutpiece = ((System.Windows.Controls.Button)(this.FindName("btn_ajoutpiece")));
            this.btn_supprimerpiece = ((System.Windows.Controls.Button)(this.FindName("btn_supprimerpiece")));
            this.cbo_typedoc = ((System.Windows.Controls.ComboBox)(this.FindName("cbo_typedoc")));
            this.Txt_Tournee = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Tournee")));
            this.lbl_Tournee = ((System.Windows.Controls.Label)(this.FindName("lbl_Tournee")));
            this.btn_zone = ((System.Windows.Controls.Button)(this.FindName("btn_zone")));
            this.gbo_typefact_Copy1 = ((SilverlightContrib.Controls.GroupBox)(this.FindName("gbo_typefact_Copy1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.txtSite = ((System.Windows.Controls.TextBox)(this.FindName("txtSite")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.txtCentre = ((System.Windows.Controls.TextBox)(this.FindName("txtCentre")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.txt_tdem = ((System.Windows.Controls.TextBox)(this.FindName("txt_tdem")));
            this.btn_RechercheClient = ((System.Windows.Controls.Button)(this.FindName("btn_RechercheClient")));
            this.lbl_NumerodeDemande = ((System.Windows.Controls.Label)(this.FindName("lbl_NumerodeDemande")));
            this.Txt_ReferenceClient = ((System.Windows.Controls.TextBox)(this.FindName("Txt_ReferenceClient")));
            this.Txt_Motif = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Motif")));
            this.lbl_Etage_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_Etage_Copy")));
            this.Txt_Porte = ((System.Windows.Controls.TextBox)(this.FindName("Txt_Porte")));
            this.Txt_MotifRejet = ((System.Windows.Controls.TextBox)(this.FindName("Txt_MotifRejet")));
        }
    }
}

