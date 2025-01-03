using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.RegistryHelper
{
    public class RegistryHelperClass:IDisposable
    {
        IRegistryToken regToken = new RegistryToken();

        public int SerialNumber
        {
            get
            {
                return Convert.ToInt32(regToken.Getvalue("SerialNumber"));
            }
            set
            {
                regToken.Setvalue("SerialNumber", value.ToString());
            }
        }

        public string ExamType
        {
            get
            {
                return regToken.Getvalue("ExamType");
            }
            set
            {
                regToken.Setvalue("ExamType", value);
            }
        }

        public string ExamYear
        {
            get
            {
                return regToken.Getvalue("ExamYear");
            }

            set
            {
                regToken.Setvalue("ExamYear", value);
            }
        }

        public string OperatorID
        {
            get
            {
                return regToken.Getvalue("OperatorID");
            }

            set
            {
                regToken.Setvalue("OperatorID", value);
            }
        }

        public string OperatorName
        {
            get
            {
                return regToken.Getvalue("OperatorName");
            }

            set
            {
                regToken.Setvalue("OperatorName", value);
            }
        }

        public string SchoolNo
        {
            get
            {
               return regToken.Getvalue("SchoolNo");
            }

            set
            {
                regToken.Setvalue("SchoolNo", value);
            }
        }

        public string SchoolName
        {
            get
            {
               return regToken.Getvalue("SchoolName");
            }

            set
            {
                regToken.Setvalue("SchoolName", value);
            }
        }

        public bool ActivationStatus
        {
            get
            {
                return Convert.ToBoolean(regToken.Getvalue("ActivationStatus"));
            }

            set
            {
                regToken.Setvalue("ActivationStatus", value.ToString());
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FetchDataClass() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
