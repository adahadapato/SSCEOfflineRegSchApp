using SSCEOfflineRegSchApp.RegistryHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp
{
    public class SystemPropertiesClass
    {
        RegistryHelperClass rh = new RegistryHelperClass();
        public string systemTime
        {
            get
            {
                DateTime d;
                d = DateTime.Now;
                string date = string.Format("{0:T}", d);
                string strPresentDate = string.Format("{0:D}", date);
                DateTime PresentDate = Convert.ToDateTime(strPresentDate);
                return date;
            }
        }

        public string systemDate
        {
            get
            {
                string VerifyDate = string.Format("{0:D}", DateTime.Now);
                return VerifyDate;
            }
        }

        public string systemOperatorName
        {
            get
            {
                return string.Format("Operator : {0} : {1}", rh.OperatorID, rh.OperatorName);
            }
        }

        public string systemSchoolName
        {
            get
            {
                return string.Format("Organisation : {0}", rh.SchoolName);
            }
        }

      
    }
}
