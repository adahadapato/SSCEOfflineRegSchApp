using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;
using System.Windows;


namespace SSCEOfflineRegSchApp.Tools
{
    public class CreateZipFIle
    {
        public static async Task<bool> CompressFiles(string zipPath, string JsonFileName, bool isBatched, bool isComplete=false)
        {
            return await Task.Run(() =>
            {
                try
                {
                    
                    if (File.Exists(zipPath))
                    {
                        string prompt = string.Format("The file {0} already exist\n"+
                            "Do you want to overwrite it", zipPath);
                        if (SafeGuiWpf.MsgBox("Export Records",prompt, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            File.Delete(zipPath);

                        }
                        else
                        {
                            File.Delete(JsonFileName);
                            return false;
                        }
                    }
                    if (!isBatched)
                    {
                        using (ZipFile zip = new ZipFile(zipPath))
                        {
                            //zip.UseUnicodeAsNecessary = true;
                            zip.AddFile(JsonFileName, "");
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                            zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                            zip.Save(zipPath);
                        }
                        SafeGuiWpf.MsgBox("Export Records", "Registration data exported successfully to " + zipPath + "\n" +
                        "You can proceed to upload your exported zip file to NECO website\n" +
                        "[www.mynecoexams.com/ssce]",  MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                        File.Delete(JsonFileName);
                       
                        return true;
                    }
                    else
                    {
                        using (ZipFile zip = new ZipFile(zipPath))
                        {
                            //zip.UseUnicodeAsNecessary = true;
                            zip.AddFile(JsonFileName, "");
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                            zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                            zip.Save(zipPath);
                        }
                        if (isComplete)
                        {
                            SafeGuiWpf.MsgBox("Export Records", "Registration data exported successfully to 'C:\\Export\\ssce' \n" +
                            "You can proceed to upload your exported zip file to NECO website\n" +
                            "[www.mynecoexams.com/ssce]",  MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                        }
                        File.Delete(JsonFileName);
                       
                        return true;
                    }

                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Zip files");

                }
                return false;
            });
            
        }
        
    }
}
