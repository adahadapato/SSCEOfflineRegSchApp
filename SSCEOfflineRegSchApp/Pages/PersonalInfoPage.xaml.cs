using Microsoft.Win32;
using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.Model;
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

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for PersonalInfoPage.xaml
    /// </summary>
    public partial class PersonalInfoPage : UserControl
    {
        //private string SerialNumber;
        //private string Surname;
        //private string FirstName;
        //private string MiddleName;
        private string Sex;
        private long DateOfBirth;
        private string disabillity="None";
        private string stateOfOriginCode;
        private string stateOfOriginName;
        private string localgovernmentCode;
        private string localgovernmentName;
        private string SchoolNo = "";
        private int TestDate;
        //private string Passport;

        private bool hasPersonalInfo = false;

        //private PersonalInfoSaveModel CandidatePersonalInfo;

        public PersonalInfoPage()
        {
            InitializeComponent();

           /* EventManager.RegisterClassHandler(typeof(DatePicker),
            DatePicker.LoadedEvent,
            new RoutedEventHandler(CustomDatePicker.DateOfBirthPicker_Loaded));*/
        }

        private void PersonalInfoPage_Loaded(object sendere, RoutedEventArgs w)
        {
            // dpDateOfBirth.v

            DoSNumber();
            DoGender();
            DoDisability();
            FetchStateRecords();

            //SafeGuiWpf.SetText(dpDateOfBirth,)
        }

        #region Passport
        private void ShowPassport(System.Windows.Controls.Image img, string Source)
        {
            //var imgUri = new Uri("pack://application:,,,/IGRJos;component/Images/" + Source, UriKind.RelativeOrAbsolute);
            var imgUri = new Uri(Source, UriKind.RelativeOrAbsolute);
            if (Application.Current.Dispatcher.CheckAccess())
            {
                img.Source = new BitmapImage(imgUri);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() =>
                  {
                      img.Source = new BitmapImage(imgUri);
                  }));
            }

        }
        #endregion

        #region State of Origin
        private void cmbStatOfOrigin_DropDownClosed(object sender, EventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item != null)
            {
                StateSaveModel g = (StateSaveModel)cmbStatOfOrigin.SelectedItem;
                stateOfOriginCode = g.stateCode;
                stateOfOriginName = g.stateName;
                hasPersonalInfo = true;
                FetchLGARecords(g.stateCode);
                return;
            }
            hasPersonalInfo = false;
        }

        private void FetchStateRecords()
        {
            Task.Run(async() =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchStateRecords();
                    if (result != null)
                        UpdateStateComboBox(result);
                }
            });
        }

        void UpdateStateComboBox(List<StateSaveModel> lst)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cmbStatOfOrigin.ItemsSource = lst;
                cmbStatOfOrigin.DisplayMemberPath = "stateName";
                cmbStatOfOrigin.SelectedValuePath = "stateCode";
                //cmbDevice.SelectedValue = "0";
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cmbStatOfOrigin.ItemsSource = lst;
                      cmbStatOfOrigin.DisplayMemberPath = "stateName";
                      cmbStatOfOrigin.SelectedValuePath = "stateCode";
                  }));
            }
        }
        #endregion

        #region LGA of Origin
        private void cmbLGA_DropDownClosed(object sender, EventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item != null)
            {
                LGASaveModel g = (LGASaveModel)cmbLGA.SelectedItem;
                localgovernmentCode = g.lgaCode;
                localgovernmentName = g.LGAName;
                hasPersonalInfo = true;
                return;
            }
            hasPersonalInfo = false;
        }

        private void FetchLGARecords(string stateCode)
        {
            Task.Run(async () =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchLGARecords(stateCode);
                    if (result != null)
                        UpdateLGAComboBox(result);
                }
            });
        }

        void UpdateLGAComboBox(List<LGASaveModel> lst)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cmbLGA.ItemsSource = lst;
                cmbLGA.DisplayMemberPath = "LGAName";
                cmbLGA.SelectedValuePath = "lgaCode";
                //cmbDevice.SelectedValue = "0";
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cmbLGA.ItemsSource = lst;
                      cmbLGA.DisplayMemberPath = "LGAName";
                      cmbLGA.SelectedValuePath = "lgaCode";
                  }));
            }
        }
        #endregion
        #region Gender
        private void DoGender()
        {
            Task.Run(async() =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetcGender();
                    UpdateGenderComboBox(result);
                }
            });
           
        }
        private void cmbGender_DropDownClosed(object sender, EventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item != null)
            {
                GenderModel g = (GenderModel)cmbGender.SelectedItem;
                this.Sex = g.Gender.Trim();
                hasPersonalInfo = true;
                return;
            }
            hasPersonalInfo = false;
        }

        void UpdateGenderComboBox(List<GenderModel> lst)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cmbGender.ItemsSource = lst;
                cmbGender.DisplayMemberPath = "Gender";
                cmbGender.SelectedValuePath = "Gender";
                //cmbDevice.SelectedValue = "0";
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cmbGender.ItemsSource = lst;
                      cmbGender.DisplayMemberPath = "Gender";
                      cmbGender.SelectedValuePath = "Gender";
                  }));
            }
        }
        #endregion

        #region Disability
        private void DoDisability()
        {
            Task<List<GenderModel>>.Run(async() =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetcDisability();
                    UpdateDisabilityComboBox(result);
                }
            });
           
        }
        private void cmbDisability_DropDownClosed(object sender, EventArgs e)
        {
             var item = (sender as ComboBox).SelectedItem;
             if (item != null)
             {
                DisabilityModel g = (DisabilityModel)cmbDisability.SelectedItem;
                 this.disabillity = g.disabled.Trim();
                 //hasPersonalInfo = true;
                 return;
             }
             //hasPersonalInfo = false;
        }
        void UpdateDisabilityComboBox(List<DisabilityModel> lst)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cmbDisability.ItemsSource = lst;
                cmbDisability.DisplayMemberPath = "disabled";
                cmbDisability.SelectedValuePath = "disabled";
                //cmbDevice.SelectedValue = "0";
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cmbDisability.ItemsSource = lst;
                      cmbDisability.DisplayMemberPath = "disabled";
                      cmbDisability.SelectedValuePath = "disabled";
                  }));
            }
        }
        #endregion

        #region Date Picker
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            if (date != null)
            {
               // DateOfBirth = date.Value.Day.ToString() + "/" + date.Value.Month.ToString() + "/" + date.Value.Year.ToString();
                int iYear = date.Value.Year;
                int iMonth = date.Value.Month;
                int iDay = date.Value.Day;
                int iHour = date.Value.Hour;
                int iMinute = date.Value.Minute;
                int iSecond = date.Value.Second;
                long d_of_b = DateTimeToLong.ConvertToLong(iYear, iMonth, iDay, iHour, iMinute, iSecond);
                DateOfBirth = d_of_b;
                TestDate = date.Value.Year;
            }
        }

        private void dPicker_Loaded(object sender, RoutedEventArgs e)
        {
            CustomDatePicker.DateOfBirthPicker_Loaded(sender, e);
        }
        #endregion

        #region Serial Number
        private void DoSNumber()
        {
            Task.Run(() =>
            {
                string NewSNo = "";
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    var LastNo = rh.SerialNumber;
                    int NewNo = ++LastNo;
                    NewSNo = NewNo.ToString("D4");
                    SafeGuiWpf.SetText(txtserNo, NewSNo);
                    SchoolNo = rh.SchoolNo;
                }
            });
           
        }
        #endregion
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBiometrics_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelectSubject_Click(object sender, RoutedEventArgs e)
        {
            hasPersonalInfo = true;
            if (string.IsNullOrWhiteSpace(SchoolNo))
            {
                SafeGuiWpf.ShowError("Please Contact NECO Support, the Application was not installed Corecttly");
                hasPersonalInfo = false;
                return;
            }

            var PresentDate = DateTime.Now.Year;
            var tryDate = PresentDate - TestDate;
            if (tryDate > 40 || tryDate < 13)
            {
                SafeGuiWpf.ShowError("Candidate must be between 13 and 40 years old");
                hasPersonalInfo = false;
                return;
            }

            if(string.IsNullOrWhiteSpace(SafeGuiWpf.GetText(txtsurName)) || string.IsNullOrWhiteSpace(SafeGuiWpf.GetText(txtfirstName)))
            {
                SafeGuiWpf.ShowError("Please Enter Candidate Surname and First Name");
                hasPersonalInfo = false;
                return;
            }

            if(string.IsNullOrWhiteSpace(Sex))
            {
                SafeGuiWpf.ShowError("Please Select Candidate Sex");
                hasPersonalInfo = false;
                return;
            }
            
            if(string.IsNullOrWhiteSpace(stateOfOriginCode) || string.IsNullOrWhiteSpace(localgovernmentCode))
            {
                SafeGuiWpf.ShowError("Please Select State of Origin and Local Government");
                hasPersonalInfo = false;
                return;
            }

            if(DateOfBirth==0)
            {
                SafeGuiWpf.ShowError("Please Select Cadidate Date of Birth");
                hasPersonalInfo = false;
                return;
            }

            var bms = SafeGuiWpf.GetImageSource(CamImageBox);
            if(bms==null)
            {
                SafeGuiWpf.ShowError("Please Capture Candidate Passport");
                hasPersonalInfo = false;
                return;
            }
            var passport = SafeGuiWpf.ByteFromImageSource(bms) as byte[];

            var personalInfoModel = new PersonalInfoSaveModel
            {
                SchoolNo=this.SchoolNo,
                serialNumber=SafeGuiWpf.GetText(txtserNo),
                surName=SafeGuiWpf.GetText(txtsurName),
                firstName=SafeGuiWpf.GetText(txtfirstName),
                middleName=SafeGuiWpf.GetText(txtmiddleName),
                Sex=this.Sex,
                dateOfBirth=this.DateOfBirth,
                disabled=this.disabillity,
                stateOfOriginCode=this.stateOfOriginCode,
                stateOfOriginName=this.stateOfOriginName,
                lgaCode=this.localgovernmentCode,
                lgaName=this.localgovernmentName,
                bPassport= passport,
                isComplete = false
            };

            if(hasPersonalInfo)
            {
                SafeGuiWpf.SetEnabled(btnSelectSubject, false);
                SafeGuiWpf.SetEnabled(btnNext, true);
                SafeGuiWpf.AddUserControlToGrid<SubjectsPage, PersonalInfoSaveModel>((this.Parent as Grid), personalInfoModel,false);
            }else
            {
                SafeGuiWpf.ShowWarning("Please complete all required fields");
            }
        }

        private void Reset()
        {
            SafeGuiWpf.SetText(txtsurName, "");
            SafeGuiWpf.SetText(txtfirstName, "");
            SafeGuiWpf.SetText(txtmiddleName, "");
            FetchStateRecords();
            DoGender();
            FetchLGARecords("");
            DoSNumber();
            SafeGuiWpf.ClearDate(dpDateOfBirth);
            SafeGuiWpf.ClearImage(CamImageBox);
            SafeGuiWpf.SetEnabled(btnSelectSubject, true);
            SafeGuiWpf.SetEnabled(btnNext, false);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnBrowsePicture_Click(object sender, RoutedEventArgs e)
        {
          /*  string Title = "";
            using (RegistryHelperClass rh = new RegistryHelperClass())
            {
                Title = string.Format("NECO {0} {1}", rh.ExamType, rh.ExamYear);
            }
            OpenFileDialog openFileDialog = new OpenFileDialog
            {

                Filter = "Picture files (*.jpg)|*.jpg|All files (*.*)|*.*",
                Title = Title,
                DefaultExt = ".jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                //var fileName = openFileDialog.FileName;
                /*InputImg = Image.FromFile(openFileDialog.FileName);
                if ((InputImg.Height <= 140 && InputImg.Height >= 70) && (InputImg.Width <= 140 && InputImg.Width >= 70))
                {*/
               // ShowPassport(CamImageBox, fileName);
            //}

             SafeGuiWpf.ShowDialog(new wPassportPage());
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }
}
