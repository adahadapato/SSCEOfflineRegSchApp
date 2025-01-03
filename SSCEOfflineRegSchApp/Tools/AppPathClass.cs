

namespace SSCEOfflineRegSchApp.Tools
{
    using RegistryHelper;
    using System.IO;
    public class AppPathClass
    {
        public static string FetchPath
        {
            get
            {
                return string.Format("{0}\\", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            }
          
        }

       public static string ExportPath
        {
            get
            {
                string examYear = "";
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    examYear = rh.ExamYear;
                }
                return string.Format("c:\\export\\ssce\\{0}\\", examYear);
            }
        }

    }
}
