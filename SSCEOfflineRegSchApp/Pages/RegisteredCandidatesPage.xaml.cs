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
    /// Interaction logic for RegisteredCandidatesPage.xaml
    /// </summary>
    public partial class RegisteredCandidatesPage : UserControl
    {
        
        ClickableHeaderListView clv = new ClickableHeaderListView();
        int PageSize = 20;
        int PageNum = 0;
        int PageIndex = 0;
        int TotalRec = 0;
        int TotalPage = 0;
        public RegisteredCandidatesPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
             Task.Run(() =>
            {
                FetchPageSize();
                FetchRecordCount();
                FetchDataToView();
            });
            
        }

        private void FetchDataToView(string search)
        {

            Task.Run(async() =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchRecordsToPreview(search, PageSize, PageNum);
                    if (result != null)
                    {
                        SafeGuiWpf.SetItems<CandidateViewModel>(lv_Data, result);
                        Display();
                    }
                }
            });
           
        }

        private void FetchDataToView()
        {
            LongActionDialog.ShowDialog("Loading ... ", Task.Run(async () =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchRecordsToPreview("", PageSize, PageNum);
                    if (result != null)
                    {
                        SafeGuiWpf.SetItems<CandidateViewModel>(lv_Data, result);
                        Display();
                    }
                }
            }));
        }

        public void RefreshDataEx()
        {
            FetchDataToView("");
        }

        private async void FetchRecordCount()
        {

            using (FetchDataClass fd = new FetchDataClass())
            {
                var results = await fd.FetchRecordCount(false);
                TotalRec = (int)results;
                PageIndex++;
                Display();
            }
        }

        private void Display()
        {
            int rem = TotalRec % PageSize;
            TotalPage = (rem > 0) ? (TotalRec / PageSize) + 1 : (TotalRec / PageSize);
            string text = string.Format("Showing {0} Recs of {1}/{2} Page(s)", PageSize, PageIndex, TotalPage);
            SafeGuiWpf.SetText(lblTotal, text);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnListViewAddBiometrics_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Button button = sender as Button;
                var item = button.DataContext as CandidateViewModel;
                if (item != null)
                {
                    if (item.hasBiometrics)
                    {
                        SafeGuiWpf.ShowWarning("Candidate Biometrics Already Captured");
                        return;
                    }
                    SafeGuiWpf.ShowDialog(new BiometricsPage(item.serialNumber, item.Name));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
           
        }

        private void lv_Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    var item = SafeGuiWpf.GetSelectedItem(sender as ListView) as CandidateViewModel;
                    if (item != null)
                    {
                        //string serialNumber = item.serialNumber;
                        SafeGuiWpf.AddUserControlToGrid<CandidateProfilePage, CandidateViewModel>((this.Parent as Grid), item, false);
                    }
                }
                catch (Exception) { }
            });
        }

        private void lv_Data_Click(object sender, RoutedEventArgs e)
        {
            clv.Click(this, sender, e);
        }

        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Run(() =>
            {
                string SearchText = SafeGuiWpf.GetText(txtsearch);
                FetchDataToView(SearchText);
            });
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex == TotalPage)
            {
                SafeGuiWpf.ShowWarning("End of Page Encountered!");
                return;
            }
            PageNum += PageSize;
            PageIndex++;
            FetchDataToView("");
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (PageNum == 0)
            {
                SafeGuiWpf.ShowWarning("Begining of Page Encountered!");
                return;
            }
            PageNum -= PageSize;
            PageIndex--;
            FetchDataToView("");
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex == TotalPage)
            {
                SafeGuiWpf.ShowWarning("End of Page Encountered!");
                return;
            }
            PageNum = PageSize * (TotalPage - 1);
            PageIndex = TotalPage;
            FetchDataToView("");
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (PageNum == 0)
            {
                SafeGuiWpf.ShowWarning("Begining of Page Encountered!");
                return;
            }
            PageNum = 0;
            PageIndex = 1;
            FetchDataToView("");
        }

        #region Page Size Section
        private void FetchPageSize()
        {
            Task.Run(async () =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchPageSize();
                    if (result != null)
                    {
                        UpdatePageSizeComboBox(result);
                    }
                }
            });
        }

        void UpdatePageSizeComboBox(List<PageSizeModel> model)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                cmbPageSize.ItemsSource = model;
                cmbPageSize.DisplayMemberPath = "size";
                cmbPageSize.SelectedValuePath = "size";
                cmbPageSize.SelectedValue = PageSize;
                cmbPageSize.Items.Refresh();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      cmbPageSize.ItemsSource = model;
                      cmbPageSize.DisplayMemberPath = "size";
                      cmbPageSize.SelectedValuePath = "size";
                      cmbPageSize.SelectedValue = PageSize;
                      cmbPageSize.Items.Refresh();
                  }));
            }
        }

        private void cmbPageSize_DropDownClosed(object sender, EventArgs e)
        {
            var items = SafeGuiWpf.GetItems<PageSizeModel>(sender as ComboBox) as PageSizeModel;
            if (items != null)
            {
                this.PageSize = items.size;
                FetchDataToView("");
            }
        }
        #endregion

        private void btnRefreshe_Click(object sender, RoutedEventArgs e)
        {
            FetchDataToView();
        }

        private void btnListViewDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = "Are you sure you want to delete this Record";
                MessageBoxResult result;
                result = SafeGuiWpf.UMsgBox(this, "Delete Record", msg, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;

                Task.Run(async () =>
                {

                    Button button = sender as Button;
                    var butdevice = SafeGuiWpf.GetContext<CandidateViewModel>(button) as CandidateViewModel;
                    if (butdevice.status == 1)
                    {
                        SafeGuiWpf.ShowWarning("Exported Records cannot be deleted");
                        return;
                    }
                    using (WriteDataClass wd = new WriteDataClass())
                    {
                        await wd.DeleteRecord(butdevice.serialNumber);
                        var serials = await FetchSrialNo();
                        await wd.UpdateSerialNo(serials);
                        var Backuprest=await wd.BackupRecords();
                        if(Backuprest)
                        {
                            await wd.SaveNewSerialNo();
                            DoSNumber(serials.Count);
                            RefreshData();
                        }
                        FetchDataToView("");
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DoSNumber(int LastNo)
        {

            using (RegistryHelperClass rh = new RegistryHelperClass())
            {
                rh.SerialNumber = LastNo;
            }
        }

        private void RefreshData()
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

        private async Task<List<SerialNoModel>> FetchSrialNo()
        {
            using (FetchDataClass fd = new FetchDataClass())
            {
                return await fd.FetchSerailNo();
            }
        }

        private void btnListViewEdit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var data = button.DataContext as CandidateViewModel;
            if (data != null)
            {
                if(data.status==0)
                {
                    SafeGuiWpf.AddUserControlToGrid<PersonalInfoEditPage>((this.Parent as Grid), data.serialNumber, false);
                }
            }
        }

       
    }
}
