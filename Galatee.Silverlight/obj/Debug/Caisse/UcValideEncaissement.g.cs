﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Caisse\UcValideEncaissement.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5634D7DEEF8305C3027DE442686E9654"
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


namespace Galatee.Silverlight.Caisse {
    
    
    public partial class UcValideEncaissement : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal SilverlightContrib.Controls.GroupBox gbo_PaiementEspece;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.TextBox Txt_MontantFacture;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_FraisTimbre;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_MontantEspece;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.TextBox Txt_LibBank;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.TextBox Txt_NumCheque;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_MontantCheque;
        
        internal Galatee.Silverlight.Library.NumericTextBox txtOtherPaie;
        
        internal System.Windows.Controls.ComboBox cbo_OtherPaiement;
        
        internal System.Windows.Controls.CheckBox Chk_Autre;
        
        internal System.Windows.Controls.CheckBox Chk_Cash;
        
        internal System.Windows.Controls.CheckBox Chk_Cheque;
        
        internal System.Windows.Controls.Label lb_Numcaisse;
        
        internal System.Windows.Controls.Label label8;
        
        internal System.Windows.Controls.Label lb_Matricule;
        
        internal System.Windows.Controls.TextBox Txt_NumRecu;
        
        internal System.Windows.Controls.Label label7;
        
        internal System.Windows.Controls.Label label9;
        
        internal System.Windows.Controls.Label label10;
        
        internal Galatee.Silverlight.Library.NumericTextBox txt_MontantRendu;
        
        internal Galatee.Silverlight.Library.NumericTextBox txt_MontantRecu;
        
        internal System.Windows.Controls.TextBox txt_MontantTimbreEspece;
        
        internal System.Windows.Controls.TextBox txt_MontantPayeEspece;
        
        internal System.Windows.Controls.Label label;
        
        internal Galatee.Silverlight.Library.NumericTextBox Txt_MontantEncaisse;
        
        internal System.Windows.Controls.CheckBox Chk_InclureFrais;
        
        internal System.Windows.Controls.Label MontantPayeChequeTotal;
        
        internal System.Windows.Controls.Label lbl_MontantPayeChequeFrais;
        
        internal System.Windows.Controls.Label lbl_MontantPayeCheque;
        
        internal Galatee.Silverlight.Library.NumericTextBox txt_MontantRecuCheque;
        
        internal System.Windows.Controls.TextBox txt_MontantFrais;
        
        internal System.Windows.Controls.TextBox txt_MontantPayeCheque;
        
        internal System.Windows.Controls.TextBox txt_MontantTotalRegle;
        
        internal System.Windows.Controls.Label lbl_MontantPaye_Copy2;
        
        internal System.Windows.Controls.DataGrid dtgAutre;
        
        internal System.Windows.Controls.Button Btn_AjouterAutre;
        
        internal System.Windows.Controls.Button Btn_SupprimerAutre;
        
        internal System.Windows.Controls.TextBox txt_MontantAutre;
        
        internal System.Windows.Controls.Button btn_Facture;
        
        internal System.Windows.Controls.Label MontantPayeChequeTotal_Copy;
        
        internal System.Windows.Controls.Label lbl_MontantPayeChequeFrais_Copy;
        
        internal System.Windows.Controls.Label lbl_MontantPayeCheque_Copy;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Caisse/UcValideEncaissement.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.gbo_PaiementEspece = ((SilverlightContrib.Controls.GroupBox)(this.FindName("gbo_PaiementEspece")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.Txt_MontantFacture = ((System.Windows.Controls.TextBox)(this.FindName("Txt_MontantFacture")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.Txt_FraisTimbre = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_FraisTimbre")));
            this.Txt_MontantEspece = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_MontantEspece")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.Txt_LibBank = ((System.Windows.Controls.TextBox)(this.FindName("Txt_LibBank")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.Txt_NumCheque = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NumCheque")));
            this.Txt_MontantCheque = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_MontantCheque")));
            this.txtOtherPaie = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("txtOtherPaie")));
            this.cbo_OtherPaiement = ((System.Windows.Controls.ComboBox)(this.FindName("cbo_OtherPaiement")));
            this.Chk_Autre = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_Autre")));
            this.Chk_Cash = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_Cash")));
            this.Chk_Cheque = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_Cheque")));
            this.lb_Numcaisse = ((System.Windows.Controls.Label)(this.FindName("lb_Numcaisse")));
            this.label8 = ((System.Windows.Controls.Label)(this.FindName("label8")));
            this.lb_Matricule = ((System.Windows.Controls.Label)(this.FindName("lb_Matricule")));
            this.Txt_NumRecu = ((System.Windows.Controls.TextBox)(this.FindName("Txt_NumRecu")));
            this.label7 = ((System.Windows.Controls.Label)(this.FindName("label7")));
            this.label9 = ((System.Windows.Controls.Label)(this.FindName("label9")));
            this.label10 = ((System.Windows.Controls.Label)(this.FindName("label10")));
            this.txt_MontantRendu = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("txt_MontantRendu")));
            this.txt_MontantRecu = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("txt_MontantRecu")));
            this.txt_MontantTimbreEspece = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantTimbreEspece")));
            this.txt_MontantPayeEspece = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantPayeEspece")));
            this.label = ((System.Windows.Controls.Label)(this.FindName("label")));
            this.Txt_MontantEncaisse = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("Txt_MontantEncaisse")));
            this.Chk_InclureFrais = ((System.Windows.Controls.CheckBox)(this.FindName("Chk_InclureFrais")));
            this.MontantPayeChequeTotal = ((System.Windows.Controls.Label)(this.FindName("MontantPayeChequeTotal")));
            this.lbl_MontantPayeChequeFrais = ((System.Windows.Controls.Label)(this.FindName("lbl_MontantPayeChequeFrais")));
            this.lbl_MontantPayeCheque = ((System.Windows.Controls.Label)(this.FindName("lbl_MontantPayeCheque")));
            this.txt_MontantRecuCheque = ((Galatee.Silverlight.Library.NumericTextBox)(this.FindName("txt_MontantRecuCheque")));
            this.txt_MontantFrais = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantFrais")));
            this.txt_MontantPayeCheque = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantPayeCheque")));
            this.txt_MontantTotalRegle = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantTotalRegle")));
            this.lbl_MontantPaye_Copy2 = ((System.Windows.Controls.Label)(this.FindName("lbl_MontantPaye_Copy2")));
            this.dtgAutre = ((System.Windows.Controls.DataGrid)(this.FindName("dtgAutre")));
            this.Btn_AjouterAutre = ((System.Windows.Controls.Button)(this.FindName("Btn_AjouterAutre")));
            this.Btn_SupprimerAutre = ((System.Windows.Controls.Button)(this.FindName("Btn_SupprimerAutre")));
            this.txt_MontantAutre = ((System.Windows.Controls.TextBox)(this.FindName("txt_MontantAutre")));
            this.btn_Facture = ((System.Windows.Controls.Button)(this.FindName("btn_Facture")));
            this.MontantPayeChequeTotal_Copy = ((System.Windows.Controls.Label)(this.FindName("MontantPayeChequeTotal_Copy")));
            this.lbl_MontantPayeChequeFrais_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_MontantPayeChequeFrais_Copy")));
            this.lbl_MontantPayeCheque_Copy = ((System.Windows.Controls.Label)(this.FindName("lbl_MontantPayeCheque_Copy")));
        }
    }
}

