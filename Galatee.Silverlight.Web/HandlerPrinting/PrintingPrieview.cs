using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Galatee.Silverlight.Web
{
    public static class PrintingPrieview
    {
        public static Dictionary<string, ReportDataSource> reportDataSource = new Dictionary<string, ReportDataSource>();
        public static Dictionary<string, List<ReportParameter>> reportParameter = new Dictionary<string, List<ReportParameter>>();
        public static Dictionary<string, string> reportPath = new Dictionary<string, string>();
        public static Dictionary<string, string> datasourceName = new Dictionary<string, string>();
    }
}