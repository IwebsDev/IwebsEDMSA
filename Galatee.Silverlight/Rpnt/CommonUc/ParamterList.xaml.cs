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

namespace Galatee.Silverlight.Rpnt.CommonUc
{
    public partial class ParamterList : UserControl
    {
        public ParamterList()
        {
            InitializeComponent();
            dg_param_mth.BeginEdit();
        }
        
        //ContextMenu ctxmn = null;
        //MenuItem mnitem;
        //private void dg_param_mth_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //private void dg_param_mth_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    //if (((DataGrid)sender).SelectedItem != null)
        //    //{
        //    ctxmn = new ContextMenu();

        //    mnitem = new MenuItem();
        //    mnitem.Header = "Consultation";
        //    ctxmn.Items.Add(mnitem);
        //    mnitem.Click += new RoutedEventHandler(mnitem_Click);

        //    mnitem = new MenuItem();
        //    mnitem.Header = "Modification";
        //    ctxmn.Items.Add(mnitem);
        //    mnitem.Click += new RoutedEventHandler(mnitem_Click);

        //    mnitem = new MenuItem();
        //    mnitem.Header = "Supprimer";
        //    ctxmn.Items.Add(mnitem);
        //    mnitem.Click += new RoutedEventHandler(mnitem_Click);

        //    ctxmn.IsOpen = true;
        //    ctxmn.HorizontalOffset = e.GetPosition(null).X;
        //    ctxmn.VerticalOffset = e.GetPosition(null).Y;
        //    //}
        //}

        //void mnitem_Click(object sender, RoutedEventArgs e)
        //{
        //    MenuItem mnitem = (MenuItem)sender;

        //    switch (mnitem.Header.ToString())
        //    {
        //        case "Consultation":
        //            new EditParamettreMethode(true, dg_param_mth.SelectedItem).Show();
        //            break;
                
        //        case "Supprimer":
        //            //Message de confirmation
        //            Message.Question("Ete-vous sure de vouloir supprimer se élément?", "Confirmation de Suppression");
        //            if (Message.DialogResult)
        //            {
        //                //DeleteMethodeDetectionBta(((CsREFMETHODEDEDETECTIONCLIENTSBTA)dg_param_mth.SelectedItem).METHODE_ID);
        //            }
        //            else
        //            {
        //                return;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
