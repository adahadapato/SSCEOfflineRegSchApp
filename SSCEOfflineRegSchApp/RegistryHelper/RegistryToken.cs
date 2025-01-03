
namespace SSCEOfflineRegSchApp.RegistryHelper
{
    using Microsoft.Win32;
    using System;

    public interface IRegistryToken
    {
        string Getvalue(string regKey);
        void Setvalue(string regKey,string rValue);
    }

    public class RegistryToken : IRegistryToken
    {
        private readonly string GlobalKey = @"software\National Examinations Council\Offline Registration\ssce";
        public string Getvalue(string regKey)
        {
            try
            {
                RegistryKey mICParams = Registry.CurrentUser;
                mICParams = mICParams.OpenSubKey(GlobalKey, false);
                return mICParams.GetValue(regKey).ToString();

            }
            catch (Exception)
            {
               // Message = "Error " + ex.Message;
            }
            return string.Empty;
        }

        public void Setvalue(string regKey, string rValue)
        {
            try
            {
                RegistryKey mICParams = Registry.CurrentUser;
                mICParams = mICParams.OpenSubKey(GlobalKey, true);
                mICParams.SetValue(regKey, rValue);
                mICParams.Close();
                //return true;
            }
            catch (Exception)
            {
               // Message="Error: "+ex.Message;
            }
            //return false;
        }
    }
}
