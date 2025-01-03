using System;
using System.Windows;
using System.Windows.Controls;


//Word DOC
using System.IO;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Threading;
using SSCEOfflineRegSchApp.Tools;

namespace SSCEOfflineRegSchApp.Pages
{
    /// <summary>
    /// Interaction logic for InstructionsPage.xaml
    /// </summary>
    public partial class InstructionsPage : UserControl
    {
        public InstructionsPage()
        {
            InitializeComponent();
        }


        private void InstructionsPage_Load(object sender, RoutedEventArgs e)
        {
            var t = System.Threading.Tasks.Task.Run(() =>
            {
                  ViewDock();
            });
           
        }

        /// <summary> 
        ///  Convert the word document to xps document 
        /// </summary> 
        /// <param name="wordFilename">Word document Path</param> 
        /// <param name="xpsFilename">Xps document Path</param> 
        /// <returns></returns> 
        private XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            // Create a WordApplication and host word document 
            Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);
                // To Invisible the word document 
                wordApp.Application.Visible = false;

                // Minimize the opened word document 
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;
                Document doc = wordApp.ActiveDocument;
                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);
                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read);
                
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }

        /// <summary> 
        ///  View Word Document in WPF DocumentView Control 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        private void ViewDock()
        {
            string wordDocument = AppPathClass.FetchPath+ "USER_MANUAL.docx";
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("The file is invalid. Please select an existing file again.");
            }
            else
            {
                string convertedXpsDoc = string.Concat(System.IO.Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
                XpsDocument xpsDocument = ConvertWordToXps(wordDocument, convertedXpsDoc);
                if (xpsDocument == null)
                {
                    return;
                }

                // documentviewWord.Document = xpsDocument.GetFixedDocumentSequence();
                InsertDocument(xpsDocument);
            }
        }

        private void InsertDocument(XpsDocument xpsDocument)
        {
            if (System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                //fixedPage.Width = 11.69 * 96;
                //fixedPage.Height = 8.27 * 96;
                
                documentviewWord.Document = xpsDocument.GetFixedDocumentSequence();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      documentviewWord.Document = xpsDocument.GetFixedDocumentSequence();
                  }));
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Tools.SafeGuiWpf.SetVisible(this, Visibility.Collapsed);
        }
    }
}
