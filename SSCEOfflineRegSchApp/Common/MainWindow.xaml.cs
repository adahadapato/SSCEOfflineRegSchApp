using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

//Griaule Finger print Lib
//using GriauleFingerprintLibrary;
using System.IO;

namespace SSCEOfflineRegSchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Finger Print Variables
        //public static FingerprintCore fingerPrint;
        //public string ReaderName;
        //public int Opt;

        DispatcherTimer Timer = new DispatcherTimer();
        IRegistryToken regToken = new RegistryToken();
        public MainWindow()
        {
            InitializeComponent();

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            //fingerPrint = new FingerprintCore();
            //fingerPrint.onStatus += new StatusEventHandler(fingerPrint_onStatus);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var t = Task.Run(() =>
            {
                DoDate();
                DoExamType();
                DoSchoolInfo();
                DoNumberOfCandidates();
            });

            try
            {
               // fingerPrint.Initialize();
               // fingerPrint.CaptureInitialize();
               // fingerPrint.SetBiometricDisplayColors(FingerprintConstants.GR_IMAGE_NO_COLOR, FingerprintConstants.GR_IMAGE_NO_COLOR,
                //                                    FingerprintConstants.GR_IMAGE_NO_COLOR, FingerprintConstants.GR_IMAGE_NO_COLOR,
                //                                    FingerprintConstants.GR_IMAGE_NO_COLOR, FingerprintConstants.GR_IMAGE_NO_COLOR);

               
            }
            catch (Exception) { }
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            string date = string.Format("{0:T}", d);
            txtTime.Text = "Time: " + date;// d.Hour + " : " + d.Minute + " : " + d.Second;
        }

        private void DoDate()
        {
            var date = DateTime.Now;
            string VerifyDate = string.Format("{0:D}", date);
            SafeGuiWpf.SetText(txtDate, VerifyDate);
        }

        private void DoExamType()
        {
            Task.Run(() =>
            {
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    var data = string.Format("NECO {0} {1}", rh.ExamType, rh.ExamYear);
                    SafeGuiWpf.SetText(txtexam, data);
                }
            });
               
        }

        private void DoSchoolInfo()
        {
            Task.Run(() =>
            {
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    var data = string.Format("{0}: {1}", rh.SchoolNo, rh.SchoolName);
                    SafeGuiWpf.SetText(txtSchoolInfo, data);
                }
            });   
        }

        public void DoNumberOfCandidates()
        {
           
            Task.Run(async() =>
            {
                try
                {
                    using (FetchDataClass fd = new FetchDataClass())
                    {
                        var result = await fd.FetchRecordCount();
                        var message = string.Format("Registered : {0}", result);
                        SafeGuiWpf.SetText(txtNumberofCandidates, message);
                    }
                }
                catch (Exception)
                {
                    var message = string.Format("Registered : {0}", 0);
                    SafeGuiWpf.SetText(txtNumberofCandidates, message);
                }
               
            });
        }

        public static void ShutDown()
        {
            MainWindow w = new MainWindow();
            w.CleanUp();
            Environment.Exit(0);
        }

        public void CleanUp()
        {
            LongActionDialog.ShowDialog("Exiting Application, Please wait ...", Task.Run(async() =>
            {
                try
                {
                    string[] Files = Directory.GetFiles(AppPathClass.FetchPath, "*.bmp");
                    foreach (string file in Files)
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }

                    contentGrid.Children.Clear();
                   // fingerPrint.onStatus -= new StatusEventHandler(fingerPrint_onStatus);
                   // fingerPrint.CaptureFinalize();
                }
                catch (Exception) { Environment.Exit(0); }
                await Task.Delay(3000);
               
            }));

        }

      /*  #region Griaule Functions
        void fingerPrint_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs se)
        {
            try
            {
                if (se.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
                {
                    if (se.Source.ToString().Contains("File"))
                    {
                        //ShowDeviceStatus(this.imgStatus, "red_dot.png");
                        // SafeGuiWpf.SetImage(imgDevice, "off.png");
                        //  ShowStatusText("Connect device");
                        SafeGuiWpf.SetText(txtSensorStatus, "Connect device");
                        SafeGuiWpf.SetForeground(txtSensorStatus, "#E83511");
                    }
                    else
                    {
                        // ShowDeviceStatus(this.imgStatus, "green_dot.png");// pict_device_status.Image = Properties.Resources.on;
                        //ShowDeviceStatusToolTip();
                        //  ShowStatusText("Connected");
                        // SafeGuiWpf.SetImage(imgDevice, "on.png");
                        SafeGuiWpf.SetText(txtSensorStatus, "Ready");
                        SafeGuiWpf.SetForeground(txtSensorStatus, "#01DFA5");
                    }
                    fingerPrint.StartCapture(source.ToString());
                    //SetLvwFPReaders(se.Source, 0);
                }
                else
                {
                    fingerPrint.StopCapture(source);
                    //SafeGuiWpf.SetImage(imgDevice, "off.png");
                    //SetLvwFPReaders(se.Source, 1);
                    //ShowDeviceStatus(this.imgStatus, "red_dot.png");
                    // ShowStatusText("Connect device");
                    SafeGuiWpf.SetText(txtSensorStatus, "Connect device");
                    SafeGuiWpf.SetForeground(txtSensorStatus, "#E83511");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        #endregion*/

        private void btnRegisterCandidate_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<PersonalInfoPage>(contentGrid,true);
        }

        private void btnPreViewRegisteredCandidates_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<RegisteredCandidatesPage>(contentGrid, true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // MainWindow w = new MainWindow();
           // CleanUp();
            //Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            CleanUp();
        }

        private void btnRestoreDatafromBackup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBackupData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegisterationReport_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<RegistrationReportPage>(contentGrid, true);
        }

        private void btnViewEntrySchedule_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<EntrySchedulePage>(contentGrid, true);
        }

        private void btnExportData_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<ExportRecordsPage>(contentGrid, true);
        }

        private void btnBrowseExportFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPreviewExportedData_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<ViewExportedRecordsPage>(contentGrid, true);
        }
    }
}
