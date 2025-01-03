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

//Neurotec Libs
using System.IO;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.ComponentModel;
using Neurotec.Devices;
using Neurotec.Plugins;
using Neurotec.Licensing;
using Neurotec.Images;
using SSCEOfflineRegSchApp.Model;
using SSCEOfflineRegSchApp.Activities;
using System.Windows.Threading;
using Neurotec.Biometrics.Gui;

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for wPassportPage.xaml
    /// </summary>
    public partial class wPassportPage : Window
    {
        public wPassportPage()
        {
            InitializeComponent();
            _biometricClient = new NBiometricClient { BiometricTypes = NBiometricType.Face, UseDeviceManager = true, FacesCheckIcaoCompliance = true };
            //if (!DesignMode)
            //{
             
        }


        #region Private fields

        private NBiometricClient _biometricClient;
        private NSubject _subject;
        private NDeviceManager _deviceManager;
        private bool CaptureAutomatically = false;
        //private NFaceView CamImageBox;

        #endregion

        private  async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            await _biometricClient.InitializeAsync();

            if (_biometricClient != null)
            {
                EnableControls(false);
            }
            /* (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).FaceRectangleColor = System.Drawing.Color.Green;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).TokenImageRectangleColor = System.Drawing.Color.White;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).ShowTokenImageRectangle = true;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).ShowIcaoArrows = true;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).IcaoArrowsColor = System.Drawing.Color.Red;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).LivenessItemsColor = System.Drawing.Color.Yellow;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).FeaturePointSize = 4;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).FaceRectangleWidth = 2;
             (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).ZoomToFit=true;*/



            try
                {
                    SafeGuiWpf.SetText(lblStatus, string.Empty);
                    SafeGuiWpf.SetText(lblQuality, string.Empty);
                    _deviceManager = _biometricClient.DeviceManager;
                    //saveImageDialog.Filter = NImages.GetSaveFileFilterString();
                    UpdateCameraList();
                }
                catch (Exception ex)
                {
                  SafeGuiWpf.ShowError(ex.Message+": "+ex.StackTrace);
                }
            //}
        }

       
        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Bitmap img = (System.Drawing.Bitmap)_subject.Faces[0].Image.ToBitmap();
            SafeGuiWpf.SetImage(PassportBox, img);
            SafeGuiWpf.SetText(lblStatus, "Extracting ...");
            _biometricClient.ForceStart();
            //_subject.Faces[0].Image.Save("c:\\works\\facejj.png");
            //_biometricClient.Cancel();
        }



        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            //_biometricClient.ForceStart();
            // File.WriteAllBytes(saveTemplateDialog.FileName, _subject.GetTemplateBuffer().ToArray());
            //_subject.Faces[0].Image.Save(saveImageDialog.FileName);
            //_subject.Faces[0].Image.Save(saveImageDialog.FileName);
            _biometricClient.Cancel();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.CloseWindow(this);
        }

        private async void btnStartCam_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(async() =>
            //{
                if (_biometricClient.FaceCaptureDevice == null)
                {
                   SafeGuiWpf.ShowWarning("Please select camera from the list");
                    return;
                }

                // Set face capture from stream
                var face = new NFace { CaptureOptions = NBiometricCaptureOptions.Stream };
                if (!CaptureAutomatically) face.CaptureOptions |= NBiometricCaptureOptions.Manual;
                _subject = new NSubject();
                _subject.Faces.Add(face);

            (wfh.Child as Neurotec.Biometrics.Gui.NFaceView).Face=face;
            //CamImageBox.Face = face;

                // Begin capturing faces
                EnableControls(true);
                SafeGuiWpf.SetText(lblStatus, string.Empty);
                SafeGuiWpf.SetText(lblQuality, string.Empty);
                try
                {
                    var status = await _biometricClient.CaptureAsync(_subject);
                    await OnCapturingCompletedAsync(status);
                }
                catch (Exception ex)
                {
                   SafeGuiWpf.ShowError(ex.Message+" "+ex.InnerException);
                //MessageBox.Show(ex.Message + ":  " + ex.StackTrace+" "+ex.InnerException);
                   SafeGuiWpf.SetText(lblStatus, string.Empty);
                   SafeGuiWpf.SetText(lblQuality, string.Empty);
                    EnableControls(false);
                }
           // });
           
        }



        #region Private methods

        private void UpdateCameraList()
        {
            try
            {
                Task.Run(async() =>
                {
                    using (FetchDataClass fd = new FetchDataClass())
                    {
                        var device = await fd.FetchCameras(_deviceManager);
                        if(device!=null)
                        {
                            UpdateDeviceComboBox(device);
                        }
                    }
                });
              /*  var devices = new List<CameraModel>();
                foreach (NDevice device in _deviceManager.Devices)
                {
                    cbCameras.Items.Add(device);
                }

                if (_biometricClient.FaceCaptureDevice == null && cbCameras.Items.Count > 0)
                {
                    cbCameras.SelectedIndex = 0;
                    return;
                }

                if (_biometricClient.FaceCaptureDevice != null)
                {
                    cbCameras.SelectedIndex = cbCameras.Items.IndexOf(_biometricClient.FaceCaptureDevice);
                }*/
            }catch{ }
        }

        private void UpdateDeviceComboBox(List<CameraModel> lst)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cbCameras.ItemsSource = lst;
                cbCameras.DisplayMemberPath = "device";
                cbCameras.SelectedValuePath = "device";
                //cmbDevice.SelectedValue = "0";
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cbCameras.ItemsSource = lst;
                      cbCameras.DisplayMemberPath = "device";
                      cbCameras.SelectedValuePath = "device";
                  }));
            }
        }

        private void cbCameras_DropDownClosed(object sender, EventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item != null)
            {
                CameraModel camg = (CameraModel)cbCameras.SelectedItem;
                /* if (_biometricClient.FaceCaptureDevice == null )
                 {
                     cbCameras.SelectedIndex = 0;
                     return;
                 }
                 if (_biometricClient.FaceCaptureDevice != null)
                 {
                     cbCameras.SelectedIndex = cbCameras.Items.IndexOf(_biometricClient.FaceCaptureDevice);
                 }*/
                _biometricClient.FaceCaptureDevice = camg.device as NCamera;
            }
        }

        private void EnableControls(bool capturing)
        {
            var hasTemplate = !capturing && _subject != null && _subject.Status == NBiometricStatus.Ok;
            if (hasTemplate)
                SafeGuiWpf.ShowInformation("Template Extracted Successfully");

            SafeGuiWpf.SetEnabled(btnCapture,(capturing && !CaptureAutomatically));
            //btnSaveImage.Enabled = hasTemplate;
            //btnSaveTemplate.Enabled = hasTemplate;
            SafeGuiWpf.SetEnabled(btnStartCam, !capturing);
            //btnRefreshList.Enabled = !capturing;
            //btnStop.Enabled = capturing;
            
            SafeGuiWpf.SetEnabled(cbCameras, !capturing);
            //btnStartExtraction.Enabled = capturing && !chbCaptureAutomatically.Checked;
            
            SafeGuiWpf.SetEnabled(chkCaptureAutomatically, !capturing);
            SafeGuiWpf.SetEnabled(chkCheckLiveness, !capturing);
        }

        private async Task OnCapturingCompletedAsync(NBiometricStatus status)
        {
            try
            {
                // If Stop button was pushed
                if (status == NBiometricStatus.Canceled) return;

                SafeGuiWpf.SetText(lblStatus, status.ToString());
                SafeGuiWpf.ShowInformation(status.ToString());
                if (status != NBiometricStatus.Ok)
                {
                    // Since capture failed start capturing again
                    _subject.Faces[0].Image = null;
                    status = await _biometricClient.CaptureAsync(_subject);
                    await OnCapturingCompletedAsync(status);
                }
                else
                {
                    //SafeGuiWpf.SetText(lblQuality, string.Format("Quality: {0}", _subject.GetTemplate().Faces.Records[0].Quality));
                    SafeGuiWpf.ShowInformation(string.Format("Quality: {0}", _subject.GetTemplate().Faces.Records[0].Quality));
                    EnableControls(false);
                }
            }
            catch (Exception ex)
            {
               SafeGuiWpf.ShowError(ex.Message);
               SafeGuiWpf.SetText(lblStatus, string.Empty);
               SafeGuiWpf.SetText(lblQuality, string.Empty);
                EnableControls(false);
            }
        }


        #endregion

        private void chkCaptureAutomatically_Checked(object sender, RoutedEventArgs e)
        {
            CaptureAutomatically = true;
        }

        private void chkCaptureAutomatically_Unchecked(object sender, RoutedEventArgs e)
        {
            CaptureAutomatically = false;
        }

        private void chkCheckLiveness_Checked(object sender, RoutedEventArgs e)
        {
            _biometricClient.FacesLivenessMode =  NLivenessMode.PassiveAndActive ;
        }

        private void chkCheckLiveness_Unchecked(object sender, RoutedEventArgs e)
        {
            _biometricClient.FacesLivenessMode = NLivenessMode.None;
        }
    }
}
