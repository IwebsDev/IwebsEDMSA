using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace Galatee.Sms.WsManagement
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            eventLogInstaller.Source = "Galatee.Sms.WsManagement";
            eventLogInstaller.Log = "Application";
            AfterInstall += ProjectInstaller_AfterInstall;
        }

        private void ProjectInstaller_AfterInstall(Object sender, InstallEventArgs e)
        {
            try
            {
                var sc = new ServiceController("Galatee.Sms.WsManagement");
                //if (sc.Status == ServiceControllerStatus.Stopped)
                //    sc.Start();
                eventLogInstaller.WriteEntry(sc.Status.ToString());
            }
            catch (Exception ex)
            {
                eventLogInstaller.WriteEntry(ex.Message);
            }
        }
    }
}