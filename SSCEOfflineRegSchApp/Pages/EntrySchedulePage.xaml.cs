using CrystalDecisions.CrystalReports.Engine;
using SSCEOfflineRegSchApp.DB.Dal;
using SSCEOfflineRegSchApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for EntrySchedulePage.xaml
    /// </summary>
    public partial class EntrySchedulePage : UserControl
    {
        public EntrySchedulePage()
        {
            InitializeComponent();
        }
        ReportDocument report = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReport();
            crv.ViewerCore.ReportSource = report;
        }

        private void LoadReport()
        {
            LongActionDialog.ShowDialog("Loading, Please Wait ...", Task.Run(async () =>
            {
                using (CrystalReportDataLayer rpt = new CrystalReportDataLayer())
                {
                    report = await rpt.GenerateDataForEntryScheduleReport();

                }
            }));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
