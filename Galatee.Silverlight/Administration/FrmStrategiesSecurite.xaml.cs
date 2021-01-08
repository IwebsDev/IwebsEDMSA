using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAdministration;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;

namespace Galatee.Silverlight.Administration
{   
    public partial class FrmStrategiesSecurite : ChildWindow
    {
        public FrmStrategiesSecurite()
        {
            InitializeComponent();
            try
            {
                GetData();
                this.DataContext = SelectedUser;

                Translate();

                var AdministrationNamespace = "Galatee.Silverlight.Administration.";
                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererStrategie",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Administration.Langue.Securite },
                new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererStrategie",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Administration.Langue.Securite },
                new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererStrategie",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Administration.Langue.Securite },
                new ContextMenuItem(){ Code=AdministrationNamespace+"UcSupprimerUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuActivate,ModeExcecution=SessionObject.ExecMode.Active,Title =Galatee.Silverlight.Resources.Langue.Active + " " + Galatee.Silverlight.Resources.Administration.Langue.Securite },
                new ContextMenuItem(){ Code=AdministrationNamespace+"UcSupprimerStrategieSecurite",Label=Galatee.Silverlight.Resources.Langue.ContextMenuDelete,ModeExcecution=SessionObject.ExecMode.Suppression,Title =Galatee.Silverlight.Resources.Langue.Delete+ " " + Galatee.Silverlight.Resources.Administration.Langue.Securite }
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }

            
        }

        private void Translate()
        {
            try
            {
                //lvwResultat.Columns[0].Header = Galatee.Silverlight.Resources.Langue.lblCentre;
                //lvwResultat.Columns[1].Header =  Galatee.Silverlight.Resources.Langue.dg_matricule;
                //lvwResultat.Columns[2].Header = Galatee.Silverlight.Resources.Langue.dg_loginName;
                //lvwResultat.Columns[3].Header =Galatee.Silverlight.Resources.Langue.dg_nomprenom;
                //lvwResultat.Columns[4].Header =Galatee.Silverlight.Resources.Langue.dg_statuscompte;
                //lvwResultat.Columns[5].Header = Galatee.Silverlight.Resources.Langue.dg_jobtitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<CsStrategieSecurite> donnesDatagrid = new List<CsStrategieSecurite>();

        public CsStrategieSecurite SelectedUser
        {
            get;
            set;
        }

        private string text = string.Empty;
        void GetData()
        {
            try
            {
                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.GetAllStrategieSecuriteCompleted += (ss, res) =>
                    {
                        if (res.Cancelled || res.Error != null)
                        {
                            string error = res.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            desableProgressBar();
                            return;
                        }

                        if (res.Result == null || res.Result.Count == 0)
                        {
                            Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata,Galatee.Silverlight.Resources.Langue.informationTitle);
                            desableProgressBar();
                            return;
                        }

                        donnesDatagrid.AddRange(res.Result);
                        lvwResultat.ItemsSource = res.Result;
                        lvwResultat.SelectedItem = res.Result[0];
                        lvwResultat.Tag = res.Result;
                    };
                client.GetAllStrategieSecuriteAsync();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        private void lvwResultat_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
           
        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvwResultat.SelectedItem == null)
                return;

            SelectedUser = lvwResultat.SelectedItem as CsStrategieSecurite;
            var _ObjectSelected = new Galatee.Silverlight.ServiceAdministration.CsStrategieSecurite();
            SessionObject.objectSelected = lvwResultat.SelectedItem as CsStrategieSecurite;//Utility.ParseObject(_ObjectSelected, SelectedUser);
            SessionObject.gridUtilisateur = lvwResultat;

        }

        private void myDataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //MessageBox.Show(ex.Message, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                     
        }
}
}


