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
    /// Interaction logic for ViewExportedRecordsPage.xaml
    /// </summary>
    public partial class ViewExportedRecordsPage : UserControl
    {

        List<CandidateViewModel> data = new List<CandidateViewModel>();
        ClickableHeaderListView clv = new ClickableHeaderListView();
        int PageSize = 20;
        int PageNum = 0;
        int PageIndex = 0;
        int TotalRec = 0;
        int TotalPage = 0;

        public ViewExportedRecordsPage()
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
                                where d.status == 1 
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
                                where d.status == 1
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
        #endregion
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FetchDataToView();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.RemoveUserControlFromGrid((this.Parent as Grid), this);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var tmpExport = (from d in data
                             where d.IsSelected == true
                             select d).ToList();

            if (tmpExport.Count == 0)
            {
                SafeGuiWpf.ShowError("Please Select Records to Recall");
                return;
            }

            string msg = "Note that this Action will make this Record Available for Editing and Re-Exporting\n" +
                         "Do you want to continue";

            MessageBoxResult answer = SafeGuiWpf.UMsgBox(this, "Recall Exported Records", msg, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;


            LongActionDialog.ShowDialog("Busy, Please wait ...", Task.Run(async() =>
            {
                var model = (from t in tmpExport
                             select t).Select(m => new Registration
                             {
                                 ser_no = m.serialNumber
                             }).ToList();

                using (WriteDataClass wd = new WriteDataClass())
                {
                    var result = await wd.UpdateStatus(model, 0);
                    if(result)
                    {
                        SafeGuiWpf.ShowSuccess("Recall Completed Successfully");
                        FetchDataToView("");
                        await Task.Delay(3000);
                    }
                }
            }));

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
                {
                    if(!model.isComplete)
                    {
                        chkBx.IsChecked = true;
                    }
                }
                   
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
                    if(!item.isComplete)
                    {
                        item.IsSelected = true;
                        lv_Data.SelectedItems.Add(item);
                    }
                    
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
    }
}
