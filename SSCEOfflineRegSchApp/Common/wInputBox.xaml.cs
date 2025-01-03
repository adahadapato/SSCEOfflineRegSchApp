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

namespace SSCEOfflineRegSchApp
{
    /// <summary>
    /// Interaction logic for wInputBox.xaml
    /// </summary>
    public partial class wInputBox : Window
    {
        private string Caption;
        private string message;
        private string result;
        public wInputBox(string Caption)
        {
            InitializeComponent();
            this.Caption = Caption;
            //this.message = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SafeGuiWpf.SetText(MessageTitle, this.Caption);
        }

        public string Result
        {
            get
            {
                return result;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = string.Empty;
            SafeGuiWpf.CloseWindow(this);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            result = SafeGuiWpf.GetText(txtAnswer);
            SafeGuiWpf.CloseWindow(this);
        }
    }
}
