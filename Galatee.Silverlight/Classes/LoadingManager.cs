using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Generic;
using Galatee.Silverlight.MainView;
using System.Linq;


namespace Galatee.Silverlight
{
    public static class LoadingManager 
    {
        static int numeroOrdreLoaders = 0;
        public static int BeginLoading(string message)
        {
            try
            {
                BusyIndicator busyIndicator = new BusyIndicator();
                busyIndicator.BusyContent = message;
                busyIndicator.IsBusy = true;
                
                
                FrmMain mainUI = ((App)Application.Current).FrmMain;
                mainUI.LoadingPanel.Items.Add(busyIndicator);

                int numberOfActiveloading = ((App)Application.Current).FrmMain.LoadingPanel.Items.Count;
                numeroOrdreLoaders++;
                busyIndicator.Name = numeroOrdreLoaders.ToString();
                return numeroOrdreLoaders;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Erreur inconnu. Code erreur : 1 \n Message : "+ex.Message);
                //return -1;
                throw new Exception("Erreur inconnu. Code erreur : 1 \n Message : " + ex.Message);
            }            
        }


        public static void EndLoading(int loaderHandler)
        {
            FrmMain mainUI = ((App)Application.Current).FrmMain;
            if ((mainUI.LoadingPanel.Items.Count > 0) && (loaderHandler >= 0))
            {
                try
                {
                    List<BusyIndicator> liste =new  List<BusyIndicator>();
                    foreach(var select in mainUI.LoadingPanel.Items)
                    {
                     liste.Add(select as BusyIndicator);
                    }
                    
                    BusyIndicator indicator = (from d in liste
                                                    where (d.Name == loaderHandler.ToString())
                                                    select d).ElementAt(0);

                    mainUI.LoadingPanel.Items.Remove(indicator);
                }
                catch (Exception ex)
                {
                   throw new Exception("Erreur inconnu. Code erreur : 2 \n Message : " + ex.Message);
                }
            }
        }

    }
}
