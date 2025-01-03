using SSCEOfflineRegSchApp.Activities;
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
using System.Windows.Threading;

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for ExportRecordsPage.xaml
    /// </summary>
    public partial class ExportRecordsPage : UserControl
    {
        List<CandidateViewModel> data = new List<CandidateViewModel>();
        ClickableHeaderListView clv = new ClickableHeaderListView();
        int PageSize = 20;
        int PageNum = 0;
        int PageIndex = 0;
        int TotalRec = 0;
        int TotalPage = 0;
        public ExportRecordsPage()
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
        #region Fetch Data Section
        private void FetchDataToView(string search)
        {
            Task.Run(async () =>
            {
               
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var result = await fd.FetchRecordsToPreview(search, PageSize, PageNum);
                    if (result != null)
                    {
                        data = (from d in result
                                    where d.status == 0 && d.hasBiometrics==true
                                    select d).ToList();
                        
                        SafeGuiWpf.SetItems<CandidateViewModel>(lv_Data, data);

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
                        data = (from d in result
                                    where d.status == 0 && d.hasBiometrics==true
                                    select d).ToList();
                       
                        SafeGuiWpf.SetItems<CandidateViewModel>(lv_Data, data);
                        Display();
                    }
                }
            }));
        }

        private async void FetchRecordCount()
        {
            using (FetchDataClass fd = new FetchDataClass())
            {
                var results = await fd.FetchRecordCount(true);
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
        #endregion



        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FetchDataToView();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }
        
        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Run(() =>
            {
                string SearchText = SafeGuiWpf.GetText(txtsearch);
                FetchDataToView(SearchText);
            });
        }

        #region ListView Control Section
        private static bool individualChkBxUnCheckedFlag { get; set; }
        private void lv_Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lv_Data_Click(object sender, RoutedEventArgs e)
        {
            clv.Click(this, sender, e);
        }


        private void lv_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SafeGuiWpf.SetText(txtmsg, "");
            if (e.AddedItems.Count > 0)
            {
                //------------
                CandidateViewModel model = (CandidateViewModel)e.AddedItems[0];
                ListViewItem lvi = (ListViewItem)lv_Data.ItemContainerGenerator.ContainerFromItem(model);
                CheckBox chkBx = SafeGuiWpf.FindVisualChild<CheckBox>(lvi);
                if (chkBx != null)
                    chkBx.IsChecked = true;
                //------------               
            }
            else // Remove Select All chkBox
            {
                CandidateViewModel model = (CandidateViewModel)e.RemovedItems[0];
                ListViewItem lvi = (ListViewItem)lv_Data.ItemContainerGenerator.ContainerFromItem(model);
                CheckBox chkBx = SafeGuiWpf.FindVisualChild<CheckBox>(lvi);
                if (chkBx != null)
                    chkBx.IsChecked = false;
            }
            //============        
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                individualChkBxUnCheckedFlag = false;
                foreach (CandidateViewModel item in lv_Data.ItemsSource)
                {
                    item.IsSelected = true;
                    lv_Data.SelectedItems.Add(item);
                }
            }
            catch (Exception) { }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!individualChkBxUnCheckedFlag)
            {
                foreach (CandidateViewModel item in lv_Data.ItemsSource)
                {
                    item.IsSelected = false;
                    //int itemIndex = items.IndexOf(item);
                    lv_Data.SelectedItems.Remove(item);
                    //lstgrd.SelectedItems.Add(lstgrd.Items.GetItemAt(2));               
                }
            }
        }

        private void chkSelect_Checked(object sender, RoutedEventArgs e)
        {
            ListViewItem item = ItemsControl.ContainerFromElement(lv_Data, e.OriginalSource as DependencyObject) as ListViewItem;
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private void chkSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            ListViewItem item = ItemsControl.ContainerFromElement(lv_Data, e.OriginalSource as DependencyObject) as ListViewItem;
            if (item != null)
                item.IsSelected = false;

            individualChkBxUnCheckedFlag = true;
            CheckBox headerChk = (CheckBox)((GridView)lv_Data.View).Columns[0].Header;
            headerChk.IsChecked = false;
        }
        #endregion



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

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var tmpExport = (from d in data
                             where d.IsSelected == true
                             select d).ToList();

            if(tmpExport.Count==0)
            {
                var dlg = new wInputBox("Batch Export");
                dlg.ShowDialog();
                var batch = dlg.Result;
                if (string.IsNullOrWhiteSpace(batch))
                {
                    SafeGuiWpf.ShowError("Invalid Batch Size");
                    return;
                }
                int iInt;
                if (!int.TryParse(batch, out iInt))
                {
                    SafeGuiWpf.ShowError("Invalid Batch Size");
                    return;
                }
                int iSize = Convert.ToInt32(batch);
                if (iSize == 0)
                {
                    SafeGuiWpf.ShowError("Invalid Batch Size");
                    return;
                }
                ExportBatch(iSize);
                return;
            }
            else
            {
                ExportNormal(tmpExport);
            }
            
        }

        private void ExportBatch(int batchSize)
        {
            LongActionDialog.ShowDialog("Exporting Records, Please wait ...", Task.Run(async() =>
            {
                var pageSize = batchSize;// 100; // set your page size, which is number of records per page
                var page = 1; // set current page number, must be >= 1
                var Mod = TotalRec % pageSize;

                var iAdditives = (Mod > 0) ? 1 : 0;
                var Iteration = (TotalRec / pageSize) + iAdditives;
                var IterationCount = 0;
                for (int i = 0; i < Iteration; i++)
                {
                    IterationCount++;
                    bool isComplete = IterationCount == Iteration;
                    var skip = pageSize * (page - 1);
                    var canPage = skip < TotalRec;
                    if (canPage) // do what you wish if you can page no further
                    {
                        using (FetchDataClass fd = new FetchDataClass())
                        {
                            var RegistrationData = await fd.FetchRecordsToExport("", pageSize, skip) as Tuple<List<Registration>, List<Biometrics>>;
                            if (RegistrationData != null)
                            {
                                if (RegistrationData.Item1 != null && RegistrationData.Item2 != null)
                                {
                                    var Registration = RegistrationData.Item1 as List<Registration>;
                                    var Biometrics = RegistrationData.Item2 as List<Biometrics>;
                                    
                                    using (RemoteDataClass rd = new RemoteDataClass())
                                    {
                                        var result = await rd.ExportData(Registration, Biometrics, IterationCount, isComplete);
                                        if (result)
                                        {
                                            FetchDataToView("");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }));
            
        }
        private void ExportNormal(List<CandidateViewModel> tmpExport)
        {
           LongActionDialog.ShowDialog("Exporting Records, Please wait ...", Task.Run(async () =>
            {
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var RegistrationData = await fd.FetchRecordsToExport("", PageSize, PageNum) as Tuple<List<Registration>, List<Biometrics>>;
                    if (RegistrationData != null)
                    {
                        if (RegistrationData.Item1 != null && RegistrationData.Item2 != null)
                        {
                            var RegistrationTemp = RegistrationData.Item1 as List<Registration>;
                            var BiometricsTemp = RegistrationData.Item2 as List<Biometrics>;


                            var Registration = (from d in RegistrationTemp
                                                from m in tmpExport
                                                where d.ser_no == m.serialNumber
                                                select d).ToList();

                            var Biometrics = (from d in BiometricsTemp
                                              from m in tmpExport
                                              where d.ser_no == m.serialNumber
                                              select d).ToList();


                            using (RemoteDataClass rd = new RemoteDataClass())
                            {
                                var result = await rd.ExportData(Registration, Biometrics);
                                if (result)
                                {
                                    FetchDataToView("");
                                }
                            }
                        }

                    }
                }


            }));
        }
    }
}
