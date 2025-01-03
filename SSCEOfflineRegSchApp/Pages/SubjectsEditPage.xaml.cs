using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.Model;
using SSCEOfflineRegSchApp.RegistryHelper;
using SSCEOfflineRegSchApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SubjectsPage.xaml
    /// </summary>
    public partial class SubjectsEditPage : UserControl
    {
        private CandidateSaveModel saveModel;
        private List<SubjectModel> Subjects= new List<SubjectModel>();
        private int NumberOfSubjects = 0;
        private string CA1 = "";
        private string CA2 = "";
        public SubjectsEditPage(CandidateSaveModel saveModel)
        {
            InitializeComponent();
            this.saveModel = saveModel;
           
        }


        public SubjectsEditPage()
            :this(null)
        {
            
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(saveModel != null)
            {
                if(saveModel.personalInfo!=null)
                {
                    var p = saveModel.personalInfo as PersonalInfoSaveModel;
                    var candInfo = string.Format("S/No.: {0} | Name: {1}, {2} {3} | Sex: {4}", p.serialNumber, p.surName,
                   p.firstName, p.middleName, p.Sex);
                    SafeGuiWpf.SetText(txtCandidateInfo, candInfo);
                }

                if(saveModel.subjects!=null)
                {
                    var subj  = new List<SubjectModel>();
                    var s = saveModel.subjects as SubjectSaveModel;
                    //this.NumberOfSubjects = s.numberOfSubjects;
                    //DoNumberOfSubjects();
                    subj.Add(new SubjectModel
                    {
                        Subject=s.Subj1,
                        code=s.Code1,
                        CA1=s.Subj1_CA1,
                        CA2=s.Subj1_CA2
                    });

                    subj.Add(new SubjectModel
                    {
                        Subject = s.Subj2,
                        code = s.Code2,
                        CA1 = s.Subj2_CA1,
                        CA2 = s.Subj2_CA2
                    });
                    subj.Add(new SubjectModel
                    {
                        Subject = s.Subj3,
                        code = s.Code3,
                        CA1 = s.Subj3_CA1,
                        CA2 = s.Subj3_CA2
                    });

                    subj.Add(new SubjectModel
                    {
                        Subject = s.Subj4,
                        code = s.Code4,
                        CA1 = s.Subj4_CA1,
                        CA2 = s.Subj4_CA2
                    });

                    subj.Add(new SubjectModel
                    {
                        Subject = s.Subj5,
                        code = s.Code5,
                        CA1 = s.Subj5_CA1,
                        CA2 = s.Subj5_CA2
                    });

                    subj.Add(new SubjectModel
                    {
                        Subject = s.Subj6,
                        code = s.Code6,
                        CA1 = s.Subj6_CA1,
                        CA2 = s.Subj6_CA2
                    });
                    if (!string.IsNullOrWhiteSpace(s.Subj7))
                    {
                        subj.Add(new SubjectModel
                        {
                            Subject = s.Subj7,
                            code = s.Code7,
                            CA1 = s.Subj7_CA1,
                            CA2 = s.Subj7_CA2
                        });
                    }
                        
                    if(!string.IsNullOrWhiteSpace(s.Subj8))
                    {
                        subj.Add(new SubjectModel
                        {
                            Subject = s.Subj8,
                            code = s.Code8,
                            CA1 = s.Subj8_CA1,
                            CA2 = s.Subj8_CA2
                        });
                    }
                   

                    if (!string.IsNullOrWhiteSpace(s.Subj9))
                    {
                        subj.Add(new SubjectModel
                        {
                            Subject = s.Subj9,
                            code = s.Code9,
                            CA1 = s.Subj9_CA1,
                            CA2 = s.Subj9_CA2
                        });
                    }
                    DoCheckBoxes(subj);
                }
               
            }
            
            
        }


        private void DoCheckBoxes(List<SubjectModel> model)
        {
            foreach(var s in model)
            {
               
                string chkBox = string.Format("chkreg_{0}", s.Subject.ToLower());
                CheckBox TB = (CheckBox)SafeGuiWpf.FindChild<CheckBox>(this, chkBox);
                
                if(TB!=null)
                {
                    SafeGuiWpf.SetChecked(TB, true);

                    string txt1 = string.Format("txtCA1_{0}", chkBox.Substring(7, 3));
                    TextBox textBox_CA1 = (TextBox)SafeGuiWpf.FindChild<TextBox>(this, txt1);
                    if (textBox_CA1 != null)
                    {
                        SafeGuiWpf.SetText(textBox_CA1, s.CA1);
                    }

                    string txt2 = string.Format("txtCA2_{0}", chkBox.Substring(7, 3));
                    TextBox textBox_CA2 = (TextBox)SafeGuiWpf.FindChild<TextBox>(this, txt2);
                    if (textBox_CA2 != null)
                    {
                        SafeGuiWpf.SetText(textBox_CA2, s.CA2);
                    }
                }
                
            }
           
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            var length = SafeGuiWpf.GetText(TB).Length;
            if (length == 2)
            {
                string name = SafeGuiWpf.GetName(TB).Substring(7, 3);
                string CA1 = SafeGuiWpf.GetText(TB);
                foreach(var s in Subjects)
                {
                    if(s.Subject==name)
                    {
                        s.CA1 = CA1;
                        break;
                    }
                }
                string txt2 = string.Format("txtCA2_{0}", name);
                var cntrl = SafeGuiWpf.FindChild<TextBox>(this, txt2);
                if(cntrl!=null)
                {
                    SafeGuiWpf.SetEnabled(cntrl, true);
                    SafeGuiWpf.MoveCursorToNextTextBox(cntrl);
                }
            }
        }


        private void TextBox_TextChanged2(object sender, TextChangedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            var length = SafeGuiWpf.GetText(TB).Length;
            if (length == 2)
            {
                string name = SafeGuiWpf.GetName(TB).Substring(7, 3);
                string CA2 = SafeGuiWpf.GetText(TB);
                foreach (var s in Subjects)
                {
                    if (s.Subject == name)
                    {
                        s.CA2 = CA2;
                        break;
                    }
                }
                if(name.Contains("eng"))
                {
                    SafeGuiWpf.MoveCursorToNextTextBox(txtCA1_mth);
                }

                if (name.Contains("mth"))
                {
                    SafeGuiWpf.MoveCursorToNextTextBox(txtCA1_civ);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NumberOfSubjects++;
            CheckBox TB = sender as CheckBox;
            if (NumberOfSubjects <= 9)
            {
                string name = SafeGuiWpf.GetName(TB);
                string txt1 = string.Format("txtCA1_{0}", name.Substring(7, 3));
                string txt2 = string.Format("txtCA2_{0}", name.Substring(7, 3));
                var cntrl = SafeGuiWpf.FindChild<TextBox>(this, txt1);
                var cntrl2 = SafeGuiWpf.FindChild<TextBox>(this, txt2);

                Subjects.Add(new SubjectModel
                {
                    Subject = name.Substring(7, 3)
                });
               
                if (cntrl != null)
                {
                    SafeGuiWpf.SetEnabled(cntrl, true);
                    //SafeGuiWpf.SetFocus(cntrl);
                    SafeGuiWpf.SetText(cntrl, CA1);
                    this.CA1 = "";
                }

                if (cntrl2 != null)
                {
                    SafeGuiWpf.SetEnabled(cntrl2, true);
                    //SafeGuiWpf.SetFocus(cntrl);
                    SafeGuiWpf.SetText(cntrl2, CA2);
                    this.CA2 = "";
                }
            }


            if(NumberOfSubjects>9)
            {
                SafeGuiWpf.SetChecked(TB, false);
            }
            DoNumberOfSubjects();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox TB = sender as CheckBox;
            string name = SafeGuiWpf.GetName(TB);
            for(int i=0;i< Subjects.Count;i++)
            {
                var s = Subjects[i].Subject;
                if(s== name.Substring(7, 3))
                {
                    Subjects.RemoveAt(i);
                }
            }

          
            string txt1 = string.Format("txtCA1_{0}", name.Substring(7, 3));
            string txt2 = string.Format("txtCA2_{0}", name.Substring(7, 3));
            var cntrl = (TextBox)SafeGuiWpf.FindChild<TextBox>(this, txt1);

            if (cntrl != null)
            {
                
                SafeGuiWpf.SetEnabled(cntrl, false);
                CA1 = SafeGuiWpf.GetText(cntrl);
                SafeGuiWpf.SetText(cntrl, "");
                NumberOfSubjects--;
            }
                

            var cntrl2 = (TextBox)SafeGuiWpf.FindChild<TextBox>(this, txt2);
            if (cntrl2 != null)
            {
               
                SafeGuiWpf.SetEnabled(cntrl2, false);
                CA2 = SafeGuiWpf.GetText(cntrl2);
                SafeGuiWpf.SetText(cntrl2, "");
            }
            DoNumberOfSubjects();
        }

       /* private void txtCA2_eng_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SafeGuiWpf.GetText(txtCA2_eng).Length == 2)
                SafeGuiWpf.MoveCursorToNextTextBox(txtCA1_mth);
        }

        private void txtCA2_mth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SafeGuiWpf.GetText(txtCA2_mth).Length == 2)
                SafeGuiWpf.MoveCursorToNextTextBox(txtCA1_civ);
        }*/

        private void DoNumberOfSubjects()
        {
           Task.Run(() =>
            {
                var val = string.Format("No of Subjects: {0}", NumberOfSubjects);
                SafeGuiWpf.SetText(txtNumberOfSubjects, val);
            });
            
        }
        private void btnSaveRecord_Click(object sender, RoutedEventArgs e)
        {
            var invalidCA = (from s in Subjects
                             where s.CA1 == null || s.CA2 == null || string.IsNullOrWhiteSpace(s.CA1) || string.IsNullOrWhiteSpace(s.CA2)
                             select s).ToList().Count();


            if(Subjects.Count<7)
            {
                SafeGuiWpf.ShowError("Minimum Number of Subjects is Seven (7)");
                return;
            }


            if (invalidCA > 0)
            {
                SafeGuiWpf.ShowError("Please Enter CA for all Selected Subjects");
                return;
            }

           LongActionDialog.ShowDialog("Updating Changes, Please wait ...", Task.Run(async() =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var subjref = await fd.FetchSubjectsRef();
                   

                    foreach(var sbj in Subjects)
                    {
                        foreach(var sub in subjref)
                        {
                            if(sbj.Subject.ToUpper()==sub.subject.ToUpper())
                            {
                                sbj.code = sub.code;
                            }
                        }
                    }
                }

                Subjects = Subjects.OrderBy(x => x.code).ToList();

                SubjectSaveModel msubj = new SubjectSaveModel
                {
                    SerialNumber= this.saveModel.personalInfo.serialNumber,
                    Subj1 = Subjects[0].Subject,
                    Code1=Subjects[0].code,
                    Subj1_CA1 = Subjects[0].CA1,
                    Subj1_CA2 = Subjects[0].CA2,

                    Subj2 = Subjects[1].Subject,
                    Code2 = Subjects[1].code,
                    Subj2_CA1 = Subjects[1].CA1,
                    Subj2_CA2 = Subjects[1].CA2,

                    Subj3 = Subjects[2].Subject,
                    Code3 = Subjects[2].code,
                    Subj3_CA1 = Subjects[2].CA1,
                    Subj3_CA2 = Subjects[2].CA2,

                    Subj4 = Subjects.Count>4? Subjects[3].Subject : "",
                    Code4 = Subjects.Count > 4 ? Subjects[3].code:"",
                    Subj4_CA1 = Subjects.Count > 4 ? Subjects[3].CA1 : "",
                    Subj4_CA2 = Subjects.Count > 4 ? Subjects[3].CA2 : "",

                    Subj5 = Subjects.Count > 5 ? Subjects[4].Subject : "",
                    Code5 = Subjects.Count > 5 ? Subjects[4].code : "",
                    Subj5_CA1 = Subjects.Count > 5 ? Subjects[4].CA1 : "",
                    Subj5_CA2 = Subjects.Count > 5 ? Subjects[4].CA2 : "",

                    Subj6 = Subjects.Count > 6 ? Subjects[5].Subject : "",
                    Code6 = Subjects.Count > 6 ? Subjects[5].code : "",
                    Subj6_CA1 = Subjects.Count > 6 ? Subjects[5].CA1 : "",
                    Subj6_CA2 = Subjects.Count > 6 ? Subjects[5].CA2 : "",

                    Subj7 = Subjects.Count >= 7 ? Subjects[6].Subject : "",
                    Code7 = Subjects.Count >= 7 ? Subjects[6].code : "",
                    Subj7_CA1 = Subjects.Count >= 7 ? Subjects[6].CA1 : "",
                    Subj7_CA2 = Subjects.Count >= 7 ? Subjects[6].CA2 : "",

                    Subj8 = Subjects.Count >= 8 ? Subjects[7].Subject : "",
                    Code8 = Subjects.Count >= 8 ? Subjects[7].code : "",
                    Subj8_CA1 = Subjects.Count >= 8 ? Subjects[7].CA1 : "",
                    Subj8_CA2 = Subjects.Count >= 8 ? Subjects[7].CA2 : "",

                    Subj9 = Subjects.Count > 8 ? Subjects[8].Subject : "",
                    Code9 = Subjects.Count > 8 ? Subjects[8].code : "",
                    Subj9_CA1 = Subjects.Count > 8 ? Subjects[8].CA1 : "",
                    Subj9_CA2 = Subjects.Count > 8 ? Subjects[8].CA2 : "",
                    numberOfSubjects=Subjects.Count
                };
                
                var candRec = new CandidateSaveModel
                {
                    personalInfo = this.saveModel.personalInfo,
                    subjects= msubj
                };
                using (WriteDataClass wd = new WriteDataClass())
                {
                    var result = await wd.UpdateCadidateRecord(candRec);
                    if(result)
                    {
                        SafeGuiWpf.ShowSuccess("Record Updated Successfully");
                        Refresh();
                        await Task.Delay(3000);
                        SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
                    }
                }
            }));
        }

        private void Refresh()
        {
            SafeGuiWpf.RefreshData<RegisteredCandidatesPage>(this, "RefreshDataEx");
        }

        private void DoNumberOfCandidates()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                SafeGuiWpf.RefreshData(Application.Current.MainWindow, "DoNumberOfCandidates");
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      SafeGuiWpf.RefreshData(Application.Current.MainWindow, "DoNumberOfCandidates");
                  }));
            }           
        }


        private void DoSnumber(string serialNumber)
        {
            using (RegistryHelperClass rh = new RegistryHelperClass())
            {
                int oldNo = Convert.ToInt32(serialNumber);
                rh.SerialNumber = oldNo;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnBiometrics_Click(object sender, RoutedEventArgs e)
        {
            var candInfo = string.Format("{0}, {1} {2}", saveModel.personalInfo.surName, saveModel.personalInfo.firstName,saveModel.personalInfo.middleName);
            SafeGuiWpf.ShowDialog(new BiometricsPage(saveModel.personalInfo.serialNumber, candInfo));

        }

       
        private void chkreg_eng_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkreg_eng_Unchecked(object sender, RoutedEventArgs e)
        {

        }

       

       
    }
}
