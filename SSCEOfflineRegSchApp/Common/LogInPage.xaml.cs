using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.RegistryHelper;
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
using System.Windows.Shapes;

namespace SSCEOfflineRegSchApp
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Window
    {
        private bool isLoginSucceeded = false;
        public LogInPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public bool IsLoginSucceeded
        {
            get { return isLoginSucceeded; }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LongActionDialog.ShowDialog("Activating Centre, Please wait ...", Task.Run(async () =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result= await fd.FetchFinRecords(SafeGuiWpf.GetText(txtOfficerPin), SafeGuiWpf.GetText(txtOfficerCode));
                    if(result!=null)
                    {
                        using (RegistryHelperClass rh = new RegistryHelperClass())
                        {
                            rh.SchoolName = result.SchoolName.Trim();
                            rh.SchoolNo = result.schnum;
                        }
                        isLoginSucceeded = true;
                        SafeGuiWpf.CloseWindow(this);
                    }
                }
            }));
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
