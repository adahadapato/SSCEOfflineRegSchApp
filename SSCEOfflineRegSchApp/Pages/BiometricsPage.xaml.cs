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

//Griaule Finger Prints Libs
//using GriauleFingerprintLibrary;
//using GriauleFingerprintLibrary.Exceptions;
using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.Model;
using System.IO;
using System.Drawing;
using System.Windows.Threading;

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for BiometricsPage.xaml
    /// </summary>
    public partial class BiometricsPage : Window
    {
        private string SerialNumber;
        private string Cand_name;


        //Griaule SDK Variables Declaration
       // public FingerprintCore refFingercore;
       // private GriauleFingerprintLibrary.DataTypes.FingerprintRawImage rawImage;
        //GriauleFingerprintLibrary.DataTypes.FingerprintTemplate _template;
        //GrCaptureImageFormat imformat = new GrCaptureImageFormat();


        //Finger Prints Variables
        private System.Windows.Forms.ListView lvwFPReaders = new System.Windows.Forms.ListView();
        //Fingers
       /* private byte[] leftThumbImage = null;
        private byte[] leftIndexImage = null;
        private byte[] leftMiddleImage = null;
        private byte[] leftRingImage = null;
        private byte[] leftPinkyImage = null;


        //Right
        private byte[] rightThumbImage = null;
        private byte[] rightIndexImage = null;
        private byte[] rightMiddleImage = null;
        private byte[] rightRingImage = null;
        private byte[] rightPinkyImage = null;*/

        //Other Local Variables
        private int fingerCount = 1;
        private int TickCount = 0;
        private bool IsReady = false;

        List<TemplatesModel> biometrics = new List<TemplatesModel>();

        public BiometricsPage(string SerialNumber, string Cand_name)
        {
            InitializeComponent();
           // refFingercore = MainWindow.fingerPrint;
            this.SerialNumber = SerialNumber;
            this.Cand_name = Cand_name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.SetText(lblSerialNUber, SerialNumber);
            SafeGuiWpf.SetText(lblCand_name, Cand_name);

           // imformat = GrCaptureImageFormat.GRCAP_IMAGE_FORMAT_BMP;
           // refFingercore.onFinger += new FingerEventHandler(refFingercore_onFinger); //Trigererd when finger is placed on the reader
           // refFingercore.onImage += new ImageEventHandler(refFingercore_onImage);

           // int thresholdId = 80;
           // int rotationMaxId = 180;
           // refFingercore.SetIdentifyParameters(thresholdId, rotationMaxId);
        }

        private void CloseW()
        {
            LongActionDialog.ShowDialog("Exiting ... ", Task.Run(() =>
            {
                //Detach the attached events
               // refFingercore.onImage -= new ImageEventHandler(refFingercore_onImage); //trigered when an image is captured.
               // refFingercore.onFinger -= new FingerEventHandler(refFingercore_onFinger);

                //Delete temporary Finger files
                string[] files = Directory.GetFiles(AppPathClass.FetchPath, "*.bmp");
                foreach (string f in files)
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch { continue; }
                }

                SafeGuiWpf.CloseWindow(this);
            }));

        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            fingerCount = 1;
            TickCount = 0;
           // refFingercore.StartEnroll();

            SafeGuiWpf.SetImage(imgRightHand, "right_hand.png");
            SafeGuiWpf.SetImage(imgLeftHand, "left_thump_red.png");

            IsReady = true;
        }

        private void btnResetCapture_Click(object sender, RoutedEventArgs e)
        {
            string message = "This action will remove the Candidate's fingers\n" +
               "Do you want to continue";
            MessageBoxResult result;
            result = SafeGuiWpf.WMsgBox(this, "Clear Fingers", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            LongActionDialog.ShowDialog("Busy, Please wait ...", Task.Run(async() =>
            {
                using (WriteDataClass wd = new WriteDataClass())
                {
                    var chk = await wd.ClearTemplates(SerialNumber);
                    if (chk)
                    {
                        SafeGuiWpf.ShowSuccess("Candidate Fingers Cleared Successfully");
                        IsReady = false;
                    }
                    else
                    {
                        SafeGuiWpf.ShowError("Unable to Clear Candidate Fingers!");
                    }
                    await Task.Delay(3000);
                }
            }));
           
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async() =>
            {
                using (WriteDataClass wd = new WriteDataClass())
                {
                    var model = new BiometricsSaveModel
                    {
                        serialNumber = this.SerialNumber,
                       
                        leftThumbImage = biometrics[0].finger,
                        leftThumbTemplate=biometrics[0].template,
                        leftThumbQuality=biometrics[0].quality,

                        leftIndexImage = biometrics[1].finger,
                        leftIndexTemplate= biometrics[1].template,
                        leftIndexQuality= biometrics[1].quality,


                        leftMiddleImage = biometrics[2].finger,
                        leftMiddleTemplate= biometrics[2].template,
                        leftMiddleQuality= biometrics[2].quality,

                        leftRingImage = biometrics[3].finger,
                        leftRingTemplate= biometrics[3].template,
                        leftRingQuality= biometrics[3].quality,

                        leftPinkyImage = biometrics[4].finger,
                        leftPinkyTemplate= biometrics[4].template,
                        leftPinkyQuality= biometrics[4].quality,

                        rightThumbImage = biometrics[5].finger,
                        rightThumbTemplate= biometrics[5].template,
                        rightThumbQuality= biometrics[5].quality,

                        rightIndexImage = biometrics[6].finger,
                        rightIndexTemplate= biometrics[6].template,
                        rightIndexQuality= biometrics[6].quality,

                        rightMiddleImage = biometrics[7].finger,
                        rightMiddleTemplate= biometrics[7].template,
                        rightMiddleQuality= biometrics[7].quality,

                        rightRingImage = biometrics[8].finger,
                        rightRingTemplate= biometrics[8].template,
                        rightRingQuality= biometrics[8].quality,

                        rightPinkyImage = biometrics[9].finger,
                        rightPinkyTemplate= biometrics[9].template,
                        rightPinkyQuality= biometrics[9].quality,

                        hasBiometrics =true

                    };
                    var result = await wd.SaveBiometrics(model);
                    if (result)
                    {
                        SafeGuiWpf.ShowSuccess("Candidate Biometrics Saved Successfully");
                        await Task.Delay(3000);
                        CloseW();

                    }
                }
            });
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseW();
            SafeGuiWpf.CloseWindow(this);

        }


        #region Griaule SDK Core Functions
        /// <summary>
        /// On status event function
        /// </summary>
        /// <param name="source"></param>
        /// <param name="se"></param>
      /*  void refFingercore_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs se)
        {
            if (se.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
            {
                refFingercore.StartCapture(source.ToString());
                SetLvwFPReaders(se.Source, 0);
            }
            else
            {
                refFingercore.StopCapture(source);
                SetLvwFPReaders(se.Source, 1);
            }
        }*/


        private void CLearAll()
        {
            /* SafeGuiWpf.SetText(txtVerifymsg, "");
             SafeGuiWpf.SetText(lblVerifyCustomerTIN, "");
             SafeGuiWpf.SetText(lblCustomerName, "");
             SafeGuiWpf.SetText(lblCustomerRegType, "");
             SafeGuiWpf.SetText(lblCustomerState, "");
             SafeGuiWpf.SetText(lblCustomerLGA, "");
             SafeGuiWpf.SetText(lblWorkPlaceCategory, "");
             SafeGuiWpf.ClearImage(PassportBox);*/
        }

        /// <summary>
        /// On Image event function
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ie"></param>
       /* private void refFingercore_onImage(object source, GriauleFingerprintLibrary.Events.ImageEventArgs ie)
        {
           
            if (!IsReady)
            {
                SafeGuiWpf.ShowWarning("Click Start Capture to continue");
                return;
            }
            try
            {
                //SetFingerStatus(ie.Source, 2);
                rawImage = ie.RawImage;
                ExtractTemplate();
                _template = new GriauleFingerprintLibrary.DataTypes.FingerprintTemplate();
                int ret = (int)refFingercore.Enroll(rawImage, ref _template, GrTemplateFormat.GR_FORMAT_DEFAULT, FingerprintConstants.GR_DEFAULT_CONTEXT);
                if (ret >= FingerprintConstants.GR_ENROLL_SUFFICIENT && TickCount == 4)
                {
                    Identify();
                    TickCount = 0;
                    SafeGuiWpf.SetImage(Tick, "tick4.png");
                }

            }
            catch (Exception e)
            {
                SafeGuiWpf.ShowError("Error: " + e.Message);
            }

            System.Threading.Thread.Sleep(100);

        }*/

      /*  void refFingercore_onFinger(object source, GriauleFingerprintLibrary.Events.FingerEventArgs fe)
        {
            try
            {
                switch (fe.EventType)
                {
                    case GriauleFingerprintLibrary.Events.FingerEventType.FINGER_DOWN:
                        {
                            //SetStatusMessage("");
                            SetFingerStatus(fe.Source, 0);
                            //pict_device_status.BackColor = Color.WhiteSmoke;
                            SafeGuiWpf.SetImage(imgFingerState, "touch.png");
                        }
                        break;
                    case GriauleFingerprintLibrary.Events.FingerEventType.FINGER_UP:
                        {
                            SafeGuiWpf.SetImage(imgFingerState, "off.png");
                            SetFingerStatus(fe.Source, 1);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(e.Message, "On Finger");
            }

        }*/

        /// <summary>
        /// Verifies that finger ptiny is unique
        /// </summary>
        /// <param name="queryTemplate"></param>
        /// <param name="referenceTemplate"></param>
        /// <param name="verifyScore"></param>
        /// <returns></returns>
      /*  public bool Verify(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate queryTemplate, GriauleFingerprintLibrary.DataTypes.FingerprintTemplate referenceTemplate, out int verifyScore)
        {
            return refFingercore.Verify(queryTemplate, referenceTemplate, out verifyScore) == 1 ? true : false;
        }*/

        /// <summary>
        /// Used to identify a finger print.
        /// Not neccessarily for storage reasons.
        /// </summary>
        /// <param name="testTemplate"></param>
        /// <param name="score"></param>
        /// <returns></returns>
      /*  private bool Identify(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate testTemplate, out int score)
        {
            return refFingercore.Identify(testTemplate, out score) == 1 ? true : false;
        }*/

        /// <summary>
        /// Displays the finger image on the UI
        /// </summary>
        /// <param name="template"></param>
        /// <param name="identify"></param>
       /* private void DisplayImage(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate template, bool identify, string fileName)
        {
            IntPtr hdc = FingerprintCore.GetDC();
            IntPtr image = new IntPtr();
            try
            {
                if (identify)
                {
                    refFingercore.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_DEFAULT_CONTEXT);
                }
                else
                {
                    refFingercore.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_NO_CONTEXT);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            FingerprintCore.ReleaseDC(hdc);
        }*/

       /* private void SetLvwFPReaders(string readerName, int op)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                switch (op)
                {
                    case 0:
                        {
                            lvwFPReaders.Items.Add(readerName, readerName, 1);

                        }
                        break;
                    case 1:
                        {
                            lvwFPReaders.Items.RemoveByKey(readerName);

                        }
                        break;
                }
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() =>
                 {
                     switch (op)
                     {
                         case 0:
                             {
                                 lvwFPReaders.Items.Add(readerName, readerName, 1);

                             }
                             break;
                         case 1:
                             {
                                 lvwFPReaders.Items.RemoveByKey(readerName);

                             }
                             break;
                     }
                 }));

            }
        }*/



       /* private void SetFingerStatus(string readerName, int status)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Forms.ListViewItem[] listItens = lvwFPReaders.Items.Find(readerName, false);
                System.Threading.Thread.BeginCriticalRegion();
                foreach (System.Windows.Forms.ListViewItem item in listItens)
                {

                    switch (status)
                    {
                        case 0:
                            {
                                item.ImageIndex = 0;
                                //SetCandidateName(status.ToString());
                            }
                            break;
                        case 1:
                            {
                                item.ImageIndex = 1;
                                //SetStatusMessage(status.ToString(), c);
                                //SetCandidateName(status.ToString());
                            }
                            break;
                        case 2:
                            {
                                item.ImageIndex = 2;
                                //SetCandidateName(status.ToString());
                            }
                            break;

                    }
                }
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      System.Windows.Forms.ListViewItem[] listItens = lvwFPReaders.Items.Find(readerName, false);
                      System.Threading.Thread.BeginCriticalRegion();
                      foreach (System.Windows.Forms.ListViewItem item in listItens)
                      {

                          switch (status)
                          {
                              case 0:
                                  {
                                      item.ImageIndex = 0;
                                      //SetCandidateName(status.ToString());
                                  }
                                  break;
                              case 1:
                                  {
                                      item.ImageIndex = 1;
                                      //SetStatusMessage(status.ToString(), c);
                                      //SetCandidateName(status.ToString());
                                  }
                                  break;
                              case 2:
                                  {
                                      item.ImageIndex = 2;
                                      //SetCandidateName(status.ToString());
                                  }
                                  break;

                          }
                      }
                  }));



                //System.Threading.Thread.BeginCriticalRegion();
                System.Threading.Thread.Sleep(300);
                System.Threading.Thread.EndCriticalRegion();
            }
        }*/

       /* private void ExtractTemplate()
        {

            if (rawImage != null)
            {
                try
                {
                    _template = null;
                    refFingercore.Extract(rawImage, ref _template);
                    TickCount++;
                    switch (TickCount)
                    {
                        case 1:
                            SafeGuiWpf.SetImage(Tick, "tick3.png");
                            break;
                        case 2:
                            SafeGuiWpf.SetImage(Tick, "tick2.png");
                            break;
                        case 3:
                            SafeGuiWpf.SetImage(Tick, "tick1.png");
                            break;
                        case 4:
                            SafeGuiWpf.SetImage(Tick, "tick0.png");
                            break;
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Extract Template");
                }
            }
            else
            {
                MessageBox.Show("Error: Finger not Captured !");
            }
        }*/

        int imCount = 0;
        int fileCount = 0;


        //int imCountEx = 0;
        //int fileCountEx = 0;

        //Bitmap bmp;
       /* private void GrabImageEx(int imCount, ref int fileCount)
        {

            string fileName = "";
            fileName = AppPathClass.FetchPath + "TestImage" + fileCount.ToString() + ".bmp";
            if (File.Exists(fileName))
            {
                fileCount++;
                fileName = AppPathClass.FetchPath + "TestImage" + fileCount.ToString() + ".bmp";
            }


            refFingercore.SaveRawImageToFile(rawImage, fileName, imformat);

            //Image dimension
            int height = rawImage.Height;
            int width = rawImage.Width;
            int resolution = rawImage.Resolution;

            bmp = new Bitmap(System.Drawing.Image.FromFile(fileName));
            //SafeGuiWpf.SetImage(pictBox, bmp);

            //finger = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
            //finger_Hieght = height;
            //finger_Width = width;
            //finger_Res = resolution;
        }*/

       /* private void GrabImage(int imCount, ref int fileCount, GriauleFingerprintLibrary.DataTypes.FingerprintTemplate sTemplate)
        {
            try
            {
                string[] Files = Directory.GetFiles(AppPathClass.FetchPath, "*.bmp");
                string fileName = "";
                fileName = AppPathClass.FetchPath + SerialNumber + fileCount.ToString() + ".bmp";
                foreach (string file in Files)
                {
                    if (File.Exists(fileName))
                    {
                        fileCount++;
                        fileName = AppPathClass.FetchPath + SerialNumber + fileCount.ToString() + ".bmp";
                    }
                    else
                    {
                        fileName = AppPathClass.FetchPath + SerialNumber + fileCount.ToString() + ".bmp";
                    }
                }


                refFingercore.SaveRawImageToFile(rawImage, fileName, imformat);

                //Image dimension
                int height = rawImage.Height;
                int width = rawImage.Width;
                int resolution = rawImage.Resolution;

                Bitmap bmp = new Bitmap(System.Drawing.Image.FromFile(fileName));
                SafeGuiWpf.SetImage(pictBox, bmp);
                biometrics.Add(new TemplatesModel
                {
                    finger= SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox)),
                    template= sTemplate.Buffer,
                    quality=sTemplate.Quality
                });
               /* switch (imCount)
                {
                    case 0:
                        leftThumbImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 1:
                        leftIndexImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 2:
                        leftMiddleImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 3:
                        leftRingImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 4:
                        leftPinkyImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 5:
                        rightThumbImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 6:
                        rightIndexImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 7:
                        rightMiddleImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 8:
                        rightRingImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                    case 9:
                        rightPinkyImage = SafeGuiWpf.ByteFromImageSource(SafeGuiWpf.GetImageSource(pictBox));
                        break;
                }*/
            /*}
            catch (Exception e)
            {
                SafeGuiWpf.ShowError(e.Message);
            }
        }*/

        System.Windows.Controls.Image pictBox = new System.Windows.Controls.Image();
        /// <summary>
        /// Retrieves Templates from DB as compares with that captured from the reader.
        /// </summary>
        private void Identify()
        {
           /* GriauleFingerprintLibrary.DataTypes.FingerprintTemplate testTemplate = null;
            try
            {
                if ((_template != null) && (_template.Size > 0))
                {
                    using (FetchDataClass fd = new FetchDataClass())
                    {
                        refFingercore.IdentifyPrepare(_template);
                        var dataReader = await fd.FetchTemplates();
                        if (dataReader.Count > 0)
                        {
                            foreach (var t in dataReader)
                            {
                                byte[] buff = t.template;// (byte[])dataReader["template"];
                                int quality = t.quality;// Convert.ToInt32(dataReader["quality"]);

                                testTemplate = new GriauleFingerprintLibrary.DataTypes.FingerprintTemplate();


                                testTemplate.Size = buff.Length;
                                testTemplate.Buffer = buff;
                                testTemplate.Quality = quality;

                                int score;
                                if (Verify(_template, testTemplate, out score))
                                {
                                    SafeGuiWpf.ShowError("Error: Duplicate fingers found !");

                                    refFingercore.StartEnroll();
                                    return;
                                }

                            }
                        }

                        if (fingerCount <= 10)
                        {
                            //isCaptureInProcess = true;
                            GrabImage(imCount, ref fileCount, _template);
                            //ImageSource img = SafeGuiWpf.GetImageSource(pictBox);
                            TemplatesModel t = new TemplatesModel
                            {
                               ser_no =SerialNumber,// SafeGuiWpf.GetText(lblCustomerTIN).Trim(),
                                //finger = SafeGuiWpf.ByteFromImageSource(img),
                                template = _template.Buffer,
                                quality = _template.Quality

                            };

                            SaveTemplate(t);
                            fingerCount++;
                            ShiftFinger(fingerCount);


                            imCount++;
                            fileCount++;
                            refFingercore.StartEnroll();
                        }

                        if (fingerCount == 11)
                        {
                            SafeGuiWpf.ShowSuccess("Success: Fingers Captured");
                            refFingercore.StartEnroll();
                            SafeGuiWpf.SetEnabled(btnSave, true);
                        }
                    }
                }
            }
            catch (FingerprintException ge)
            {
                MessageBox.Show(ge.Message, "Identify");
                if (ge.ErrorCode == -8)
                {
                    FileStream dumpTemplate = File.Create(@".\Dumptemplate.gt");
                    StreamWriter stWriter = new StreamWriter(dumpTemplate);

                    stWriter.WriteLine(BitConverter.ToString(testTemplate.Buffer, 0));
                    stWriter.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Identify");
            }*/
        }




        /// <summary>
        /// Show progress as finger is enrolled
        /// </summary>
        /// <param name="FingerIndex"></param>
        private void ShiftFinger(int FingerIndex)
        {
            switch (FingerIndex)
            {
                case 1:
                    SafeGuiWpf.SetImage(imgRightHand, "right_thump_red.png");
                    //ShowFingerProgress(imgLeftHand, "left_index_red.jpg");
                    break;
                case 2:
                    //SafeGuiWpf.SetImage(imgRightHand, "left_thump_green.png");
                    SafeGuiWpf.SetImage(imgLeftHand, "left_index_red.png");
                    break;
                case 3:
                    SafeGuiWpf.SetImage(imgLeftHand, "left_fore_red.png");
                    //SafeGuiWpf.SetImage(imgLeftHand, "left_index_green.png");
                    break;
                case 4:
                    //SafeGuiWpf.SetImage(imgLeftHand, "left_fore_green.png");
                    SafeGuiWpf.SetImage(imgLeftHand, "left_ring_red.png");
                    break;
                case 5:
                    SafeGuiWpf.SetImage(imgLeftHand, "left_little_red.png");
                    //SafeGuiWpf.SetImage(imgLeftHand, "left_ring_green.png");
                    break;
                case 6:
                    SafeGuiWpf.SetImage(imgLeftHand, "left_all_green.png");
                    SafeGuiWpf.SetImage(imgRightHand, "right_thump_red.png");
                    break;
                case 7:
                    //SafeGuiWpf.SetImage(imgRightHand, "right_thump_green.png");
                    SafeGuiWpf.SetImage(imgRightHand, "right_index_red.png");
                    break;
                case 8:
                    //SafeGuiWpf.SetImage(imgRightHand, "right_index_green.png");
                    SafeGuiWpf.SetImage(imgRightHand, "right_fore_red.png");
                    break;
                case 9:
                    SafeGuiWpf.SetImage(imgRightHand, "right_ring_red.png");
                    //SafeGuiWpf.SetImage(imgRightHand, "right_fore_green.png");
                    break;
                case 10:
                    //SafeGuiWpf.SetImage(imgRightHand, "right_ring_green.png");
                    SafeGuiWpf.SetImage(imgRightHand, "right_little_red.png");
                    break;
                case 11:
                    SafeGuiWpf.SetImage(imgRightHand, "right_all_green.png");
                    break;
            }
        }




       /* private void SaveTemplate(TemplatesModel t)
        {
            Task.Run(async() =>
            {
                try
                {
                    if (_template == null)
                    {
                        SafeGuiWpf.ShowWarning("No Templates Extracted");
                        return;
                    }

                    using (WriteDataClass wd = new WriteDataClass())
                    {
                       var b= await wd.SaveTemplate(t);
                    }

                }
                catch { }
            });

        }*/
        #endregion
    }
}
