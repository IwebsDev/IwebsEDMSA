﻿#pragma checksum "D:\TFS_SOURCE_EDM\iWEBS_EDMSA\Galatee.Silverlight\Caisse\FrmDemandeAnnulation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "40254C419EEF8A40A4B7498904045612"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class FrmDemandeAnnulation : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button btn_Ajouter;
        
        internal System.Windows.Controls.DataGrid dtg_FactureAAnnule;
        
        internal System.Windows.Controls.Button btn_Retirer;
        
        internal System.Windows.Controls.DataGrid dtg_FactureAjouter;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Caisse/FrmDemandeAnnulation.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.btn_Ajouter = ((System.Windows.Controls.Button)(this.FindName("btn_Ajouter")));
            this.dtg_FactureAAnnule = ((System.Windows.Controls.DataGrid)(this.FindName("dtg_FactureAAnnule")));
            this.btn_Retirer = ((System.Windows.Controls.Button)(this.FindName("btn_Retirer")));
            this.dtg_FactureAjouter = ((System.Windows.Controls.DataGrid)(this.FindName("dtg_FactureAjouter")));
        }
    }
}

