using SSCEOfflineRegSchApp.Model;
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
    /// Interaction logic for CandidateProfilePage.xaml
    /// </summary>
    public partial class CandidateProfilePage : UserControl
    {
        CandidateViewModel candidate;
        public CandidateProfilePage()
            :this(null)
        {
            
        }


        public CandidateProfilePage(CandidateViewModel candidate)
        {
            InitializeComponent();
            this.candidate = candidate;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(candidate != null)
            {
                SafeGuiWpf.SetImage(CamImageBox, candidate.bPassport);
                SafeGuiWpf.SetText(lblSerialNumber, candidate.serialNumber);
                SafeGuiWpf.SetText(lblcand_name, candidate.Name);
                SafeGuiWpf.SetText(lblSex, candidate.Sex);
                SafeGuiWpf.SetText(lbld_of_b, candidate.d_of_b);
                SafeGuiWpf.SetText(lblDisability, candidate.disabled);
                SafeGuiWpf.SetText(lblStateofOrigin, candidate.stateOfOriginName);
                SafeGuiWpf.SetText(lblLocalGoverment, candidate.lgaName);
                //candidate.hasBiometrics = true;
                if ((bool)candidate.hasBiometrics)
                {
                    SafeGuiWpf.SetText(txtCompleted, "Biometrics Captured");
                    SafeGuiWpf.SetBackground(txtCompleted, "#3aDF00");
                }
                else
                {
                    SafeGuiWpf.SetText(txtCompleted, "Biometrics Pending");
                    SafeGuiWpf.SetBackground(txtCompleted, "#E83511");
                }

                if ((int)candidate.status==1)
                {
                    SafeGuiWpf.SetText(txtStatus, "Record Exported");
                    SafeGuiWpf.SetBackground(txtStatus, "#3aDF00");
                }
                else
                {
                    SafeGuiWpf.SetText(txtStatus, "Pending Export");
                    SafeGuiWpf.SetBackground(txtStatus, "#E83511");
                }
                if((int)candidate.status == 1 && (bool)candidate.isComplete)
                {
                    SafeGuiWpf.SetText(txtStatus, "Completed");
                    SafeGuiWpf.SetBackground(txtStatus, "#138830");
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.AddUserControlToGrid<PersonalInfoEditPage>((this.Parent as Grid), candidate.serialNumber, false);
        }

        private void btnBiometrics_Click(object sender, RoutedEventArgs e)
        {
            if (candidate.hasBiometrics)
            {
                SafeGuiWpf.ShowWarning("Candidate Biometrics Already Captured");
                return;
            }
            SafeGuiWpf.ShowDialog(new BiometricsPage(candidate.serialNumber, candidate.Name));
        }
    }
}
