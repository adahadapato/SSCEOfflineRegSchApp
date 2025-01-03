

//using SSCEOfflineRegSchApp.DB;
using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.Model;
using SSCEOfflineRegSchApp.RegistryHelper;
using SSCEOfflineRegSchApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Neurotec;

namespace SSCEOfflineRegSchApp.Setup
{
    public class EntryPoint
    {
        

        [STAThread]
        public static void Main()
        {
            const string Address = "/local";
            const string Port = "5000";
            bool retry;
            string[] licenses =
            {
                "Biometrics.FaceDetection",
                "Biometrics.FaceExtraction",
                "Devices.Cameras",
                //"Biometrics.FaceSegmentation",
                //"Biometrics.FaceQualityAssessment",
                "Biometrics.FaceSegmentsDetection",
                "Biometrics.FaceMatching"
            };


            do
            {
                try
                {
                    retry = false;
                    foreach (string license in licenses)
                    {
                        if (!Neurotec.Licensing.NLicense.ObtainComponents(Address, Port, license))
                        {
                            throw new NotActivatedException(string.Format("Could not obtain licenses for components: {0}", license));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("Failed to obtain licenses for components.\nError message: {0}", ex.Message);
                    if (ex is System.IO.IOException)
                    {
                        message += "\n(Probably licensing service is not running. Use Activation Wizard to figure it out.)";
                    }
                    if (MessageBox.Show(message, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                    {
                        retry = true;
                    }
                    else
                    {
                        retry = false;
                        return;
                    }
                }
            }
            while (retry);

            string[] args = Environment.GetCommandLineArgs();
            SingleInstanceController controller = new SingleInstanceController();
            controller.Run(args);
        }
    }

    public class SingleInstanceApplication : System.Windows.Application
    {
        private bool ActivationStatus = false;
        private bool LoginSucceed = false;
        private const int MINIMUM_SPLASH_TIME = 1800; // Miliseconds
        private const int SPLASH_FADE_TIME = 500;     // Miliseconds
        
        System.Collections.ObjectModel.ReadOnlyCollection<string> _commandLine;
        public SingleInstanceApplication(System.Collections.ObjectModel.ReadOnlyCollection<string> _commandLine)
        {
            this._commandLine = _commandLine;
        }

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow window = new MainWindow();

            // Step 1 - Load the splash screen
            SplashScreen splash = new SplashScreen("Images/splash.png");
            splash.Show(false, true);

            // Step 2 - Start a stop watch
            Stopwatch timer = new Stopwatch();
            timer.Start();

            base.OnStartup(e);
            string fileName = "";
            if (_commandLine.Count > 1)
                fileName = _commandLine[1].ToString();
            else
                fileName = "";



            // Step 4 - Make sure that the splash screen lasts at least two seconds
            timer.Stop();
            int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplash > 0)
                Thread.Sleep(remainingTimeToShowSplash);

            // Step 5 - show the page
            splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME));




            if (!CheckActivationStatus)
            {
                if (CreateDB())
                {
                    LoginSucceed = PerformLoginAction;
                    if (LoginSucceed)
                    {
                        CheckActivationStatus = true;
                    }
                }
                   
            }
            else
            {
                LoginSucceed = CheckActivationStatus;
            }
            
            
            if(LoginSucceed)
                window.Show();
            else
                Environment.Exit(0);
        }

        public void Activate()
        {
            // Reactivate the main window
            MainWindow.Activate();
        }

        private bool CheckActivationStatus
        {
            get
            {
                //
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    ActivationStatus = rh.ActivationStatus;
                    /*if (!rh.ActivationStatus)
                    {
                        var result = CreateDB();
                        if (result)
                            rh.ActivationStatus = result;
                    }*/
                    return ActivationStatus;

                }
            }

            set
            {
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    rh.ActivationStatus = value;
                   
                }
            }
           
        }


        private bool PerformLoginAction
        {
            get
            {
                LogInPage loginPage = new LogInPage();
                loginPage.ShowDialog();
                LoginSucceed = loginPage.IsLoginSucceeded;
                SafeGuiWpf.CloseWindow(loginPage);
                return LoginSucceed;
            }

        }

        private bool CreateDB()
        {
           return LongActionDialog.ShowDialog("Initializing Application, Please wait ...", Task.Run(async() =>
             {
                 using (WriteDataClass wd = new WriteDataClass())
                 {
                     var result = await wd.CreateDataBase();
                     if(result)
                     {

                         string stateFilein = string.Format("{0}state.ssce", AppPathClass.FetchPath);
                         var stateOutFile= string.Format("{0}stateX.ssce", AppPathClass.FetchPath);
                         FileHandlerClass.DecryptFile(stateFilein, stateOutFile);
                         var Json = FileHandlerClass.LoadJson(stateOutFile);
                         var stateModel = await FileHandlerClass.DecodeJsonToModelAsync<List<StateSaveModel>>(Json);
                         var stResults= await wd.SaveStatesToDatabase(stateModel);
                         FileHandlerClass.DeleteFile(stateOutFile);

                         string LGAFilein = string.Format("{0}lga.ssce", AppPathClass.FetchPath);
                         var LGAOutFile = string.Format("{0}lgaX.ssce", AppPathClass.FetchPath);
                         FileHandlerClass.DecryptFile(LGAFilein, LGAOutFile);
                         var Json2 = FileHandlerClass.LoadJson(LGAOutFile);
                         var LGAModel = await FileHandlerClass.DecodeJsonToModelAsync<List<LGASaveModel>>(Json2);
                         var lgResults = await wd.SaveLGAToDatabase(LGAModel);
                         FileHandlerClass.DeleteFile(LGAOutFile);

                         string SubjectFilein = string.Format("{0}Subject.ssce", AppPathClass.FetchPath);
                         var SubjectOutFile = string.Format("{0}SubjectX.ssce", AppPathClass.FetchPath);
                         FileHandlerClass.DecryptFile(SubjectFilein, SubjectOutFile);
                         var Json3 = FileHandlerClass.LoadJson(SubjectOutFile);
                         var SubjectRefModel = await FileHandlerClass.DecodeJsonToModelAsync<List<SubjectRefModel>>(Json3);
                         var sbResults = await wd.SaveSubjectToDatabase(SubjectRefModel);
                         FileHandlerClass.DeleteFile(SubjectOutFile);

                         string FinFilein = string.Format("{0}Fin.ssce", AppPathClass.FetchPath);
                         var FinOutFile = string.Format("{0}FinX.ssce", AppPathClass.FetchPath);
                         FileHandlerClass.DecryptFile(FinFilein, FinOutFile);
                         var FinJson = FileHandlerClass.LoadJson(FinOutFile);
                         var FinRefModel = await FileHandlerClass.DecodeJsonToModelAsync<List<FinSaveModel>>(FinJson);
                         var fnResults = await wd.SaveFinToDatabase(FinRefModel);
                         FileHandlerClass.DeleteFile(FinOutFile);



                     }
                     return result;
                 }
             }));
        }

        /// <summary>
        /// Creates the DB and default working
        /// files as subject ref, et all
        /// this is run only at first activation
        /// </summary>
        private void CreateSubjectTables()
        {
            try
            {
              /*  string SubjFile = AppPathClass.FetcAppPath() + @"\Subject.neco";
                string temp = AppPathClass.FetcAppPath() + @"\temp.neco";
                if (File.Exists(temp))
                    File.Delete(temp);

                CryptograhyClass.Decrypt(SubjFile, temp, CryptograhyClass.EncryptionPWD);
                JsonLayer.CreateSubjectsStorageJson(temp);*/
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Create Subject files", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void CreateStaffTables()
        {
            try
            {
              /*  IGRDal dl = SQLDalFactory.GetDal(GrConnector.AccessSQLDal);
                string StaffFile = AppPathClass.FetcAppPath() + @"\Staff.neco";
                string temp = AppPathClass.FetcAppPath() + @"\temp.neco";
                if (File.Exists(temp))
                    File.Delete(temp);

                CryptograhyClass.Decrypt(StaffFile, temp, CryptograhyClass.EncryptionPWD);
                JsonLayer.CreateStaffStorageJson(temp);*/
             //   ShowStatusText("Done ...");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Create Staff files", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Loads a resource dictionary from a URI and merges it into the application resources.
        /// </summary>
        /// <param name="resourceLocater">URI of resource dictionary</param>
        public static void MergeResourceDictionary(Uri resourceLocater)
        {
            if (Application.Current != null)
            {
                var dictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
        }

    }
    
}
