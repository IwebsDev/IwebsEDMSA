﻿#pragma checksum "D:\AUTRES\IWEBS\iWEBS_EDMSA\Galatee.Silverlight\Administration\FrmStrategiesSecurite.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FE26B2D5A5CE2551BABD682CC4FF7D23"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
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


namespace Galatee.Silverlight.Administration {
    
    
    public partial class FrmStrategiesSecurite : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DataGrid lvwResultat;
        
        internal System.Windows.Controls.ProgressBar progressBar1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Galatee.Silverlight;component/Administration/FrmStrategiesSecurite.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lvwResultat = ((System.Windows.Controls.DataGrid)(this.FindName("lvwResultat")));
            this.progressBar1 = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar1")));
        }
    }
}
