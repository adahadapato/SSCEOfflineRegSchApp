using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace SSCEOfflineRegSchApp.Tools
{
    public class FileHandlerClass
    {
        public static void DecryptFile(string FileIN, string FileOut)
        {
            CryptograhyClass.Decrypt(FileIN, FileOut, CryptograhyClass.EncryptionPWD);
        }


        public static void EncryptFile(string FileIN, string FileOut)
        {
            CryptograhyClass.Encrypt(FileIN, FileOut, CryptograhyClass.EncryptionPWD);
        }

        public static async Task<bool> CreateExportPath(string Path)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    SafeGuiWpf.ShowError(ex.Message);
                }
                return false;
            });
        }

        public static async Task<bool> WriteJson(string fileName, string Json)
        {
            return await Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(fileName, Json);
                    return true;
                }
                catch (Exception ex)
                {
                    SafeGuiWpf.ShowError(ex.Message);
                }
                return false;
            });
          
           
        }

        public static string LoadJson(string FileName)
        {
            using (StreamReader r = new StreamReader(FileName))
            {
                string Json = r.ReadToEnd();
                return Json;
            }
        }

        public static async Task<TDecode> DecodeJsonToModelAsync<TDecode>(string apiResponse)
        {
            return await Task.Run(() =>
            {
                
                return Json.Decode<TDecode>(apiResponse);
            });
        }

        public static async Task<string> EncodeModelToJsonAsync<TModel>(object model)
        {
            return await Task.Run(() =>
            {
                model=(TModel)model;
                return Json.Encode(model);
            });
        }

        public static void DeleteFile(string FileOut)
        {
            File.Delete(FileOut);
        }
    }
}
