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
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using Galatee.Silverlight.ServiceCaisse;
namespace Galatee.Silverlight.Caisse
{
    public class TimeFacade
    {
        private AutoResetEvent m_autoResetEvent = new AutoResetEvent(false);

        public void UpdateTimeInfo(TimeInformationViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel", "Argument cannot be null.");
            }

            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.ChargerListeCodeRegroupementCompleted += new EventHandler<ChargerListeCodeRegroupementCompletedEventArgs>(FillData);

            var Status = new AsyncCallStatus<ChargerListeCodeRegroupementCompletedEventArgs>();
            service.ChargerListeCodeRegroupementAsync(Status);
            m_autoResetEvent.WaitOne();

            //if (Status.CompletedEventArgs. != null)
            //{
            //    throw Status.CompletedEventArgs.Error;
            //}
            List<CsRegCli> groupingCode = new List<CsRegCli>();
            groupingCodes = Status.CompletedEventArgs.Result;
            //viewModel.CurrentCode.AddRange(Status.CompletedEventArgs.Result);
        }

        public void FillData(object sender, ChargerListeCodeRegroupementCompletedEventArgs e)
        {
            var status = e.UserState as AsyncCallStatus<ChargerListeCodeRegroupementCompletedEventArgs>;
            status.CompletedEventArgs = e;
            m_autoResetEvent.Set();
        }

        public List<CsRegCli> groupingCodes { get; set; }
    }
}