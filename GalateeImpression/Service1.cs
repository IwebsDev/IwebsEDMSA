using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GalateeImpression
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
              //private Timer _timerAbsence,
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string CheminFichier = @"C:\Impression";
                DogParam dp = new DogParam(CheminFichier);
            }
            catch (Exception ex)
            {
                ProcessStarter processStarterLog = new ProcessStarter();

                throw;
                processStarterLog.writeLog(ex.Message);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
