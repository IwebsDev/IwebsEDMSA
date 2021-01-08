using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Shared
{
    public static  class CommonMethode
    {
        public static void TransfertDataGrid<T>(DataGrid grid_depart, DataGrid grid_Arrive)
        {
            List<T> clientselectionne = new List<T>();
            List<T> clientselectionne_depart = new List<T>();

            if (grid_depart.SelectedItems.Count > 0)
            {
                if (grid_Arrive.ItemsSource != null)
                {
                    foreach (var item in grid_Arrive.ItemsSource)
                    {
                        clientselectionne.Add((T)item);
                    }
                }

                foreach (var item in grid_depart.ItemsSource)
                {
                    clientselectionne_depart.Add((T)item);
                }

                foreach (var item in grid_depart.SelectedItems)
                {
                    if (!clientselectionne.Contains((T)item))
                    {
                        clientselectionne.Add((T)item);
                        clientselectionne_depart.Remove((T)item);
                    }

                }
                grid_depart.ItemsSource = null;
                grid_depart.ItemsSource = clientselectionne_depart;

                grid_Arrive.ItemsSource = null;
                grid_Arrive.ItemsSource = clientselectionne;
            }
        }




    }
}
