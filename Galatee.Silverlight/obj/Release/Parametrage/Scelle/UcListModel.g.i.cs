﻿#pragma checksum "D:\TFS_SOURCE_EDM\iWEBS_EDMSA\Galatee.Silverlight\Parametrage\Scelle\UcListModel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2E67B81947DA55B7C34E0E73F3169EAA"
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
    
    
    public partial class UcListModel : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button btnDelete;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.DataGrid dtgrdParametre;
        
        internal SilverlightContrib.Controls.GroupBox Gbo_InformationDevis;
        
        internal System.Windows.Controls.TextBox txt_LibelleModele;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.Button btn_Modifier;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Parametrage/Scelle/UcListModel.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.btnDelete = ((System.Windows.Controls.Button)(this.FindName("btnDelete")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.dtgrdParametre = ((System.Windows.Controls.DataGrid)(this.FindName("dtgrdParametre")));
            this.Gbo_InformationDevis = ((SilverlightContrib.Controls.GroupBox)(this.FindName("Gbo_InformationDevis")));
            this.txt_LibelleModele = ((System.Windows.Controls.TextBox)(this.FindName("txt_LibelleModele")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.btn_Modifier = ((System.Windows.Controls.Button)(this.FindName("btn_Modifier")));
        }
    }
}

