using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Tarification.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmCampagne : ChildWindow
    {
        #region Variables
        public ServiceRecouvrement.CsCampagneGc csCampagneGc;
        public bool Arefaire=false;
        #endregion

        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();
        

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        #region Constructeur

        public FrmCampagne()
        {
            InitializeComponent();
        }

        public FrmCampagne(CsCampagneGc csCampagneGc)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.csCampagneGc = csCampagneGc;
            LayoutRoot.DataContext = this.csCampagneGc;
        }
    

        #endregion

        #region Event Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Bag = this.csCampagneGc;
            OnEvent(MyEventArg);
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Bag = null;
            this.Arefaire = true;
            OnEvent(MyEventArg);
            this.DialogResult = false;
        }

        private void btn_refaire_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Bag = this.csCampagneGc;
            this.Arefaire = true;
            OnEvent(MyEventArg);
            this.DialogResult = true;
        }

        #endregion



    }
}

