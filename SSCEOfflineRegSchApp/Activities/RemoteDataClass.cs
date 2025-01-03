using SSCEOfflineRegSchApp.Model;
using SSCEOfflineRegSchApp.RegistryHelper;
using SSCEOfflineRegSchApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Activities
{
    public class RemoteDataClass :IDisposable
    {
       public async Task<bool> ExportData(List<Registration> registration, List<Biometrics> biometrics)
       {
            return await Task<bool>.Run(async() =>
            {
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    var model = new DataExportModel
                    {
                        ExamType=rh.ExamType,
                        ExamYear=rh.ExamYear,
                        CentreNo=rh.SchoolNo,
                        CentreName=rh.SchoolName.Trim(),
                        registration= registration,
                        biometrics= biometrics
                    };

                    var Json = await FileHandlerClass.EncodeModelToJsonAsync<DataExportModel>(model);
                    string ExportPath = AppPathClass.ExportPath;
                    var exportD= await FileHandlerClass.CreateExportPath(ExportPath);
                    if (!exportD)
                        return false;

                    string fileName = string.Format("{0}.json", rh.SchoolNo);
                    string fileNameX = string.Format("{0}E.json", rh.SchoolNo);
                    string FileIn = string.Format("{0}{1}", ExportPath, fileName);
                    string FileOut = string.Format("{0}{1}", ExportPath, fileNameX);
                    string zipFile = string.Format("{0}{1}.zip", ExportPath, rh.SchoolNo);
                    var writeJ = await FileHandlerClass.WriteJson(FileIn, Json);
                    if(writeJ)
                    {
                        FileHandlerClass.EncryptFile(FileIn, FileOut);
                        var b= await CreateZipFIle.CompressFiles(zipFile, FileIn, false);
                        if(b)
                        {
                            using (WriteDataClass wd = new WriteDataClass())
                            {
                                var saveb = await wd.UpdateStatus(registration);
                                if (saveb)
                                    return true;
                            }
                        }
                    }
                    return false;
                }
            });
       }


        public async Task<bool> ExportData(List<Registration> registration, List<Biometrics> biometrics, int page, bool isComplete)
        {
            return await Task<bool>.Run(async () =>
            {
                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    var model = new DataExportModel
                    {
                        ExamType = rh.ExamType,
                        ExamYear = rh.ExamYear,
                        CentreNo = rh.SchoolNo,
                        CentreName = rh.SchoolName.Trim(),
                        registration = registration,
                        biometrics = biometrics
                    };

                    var Json = await FileHandlerClass.EncodeModelToJsonAsync<DataExportModel>(model);
                    string ExportPath = AppPathClass.ExportPath;
                    var exportD = await FileHandlerClass.CreateExportPath(ExportPath);
                    if (!exportD)
                        return false;

                    int Page = --page;

                    string fileName = Page > 0 ? string.Format("{0}({1}).json", rh.SchoolNo, Page):string.Format("{0}.json", rh.SchoolNo) ;
                    string fileNameX = Page > 0 ? string.Format("{0}({1})E.json", rh.SchoolNo, Page):string.Format("{0}E.json", rh.SchoolNo) ;
                    string FileIn = string.Format("{0}{1}", ExportPath, fileName);
                    string FileOut = string.Format("{0}{1}", ExportPath, fileNameX);
                    string zipFile = Page > 0 ? string.Format("{0}{1}({2}).zip", ExportPath, rh.SchoolNo, Page):string.Format("{0}{1}.zip", ExportPath, rh.SchoolNo) ;
                    var writeJ = await FileHandlerClass.WriteJson(FileIn, Json);
                    if (writeJ)
                    {
                        FileHandlerClass.EncryptFile(FileIn, FileOut);
                        var b = await CreateZipFIle.CompressFiles(zipFile, FileIn, true, isComplete);
                        if (b)
                        {
                            using (WriteDataClass wd = new WriteDataClass())
                            {
                                var saveb = await wd.UpdateStatus(registration);
                                if (saveb)
                                    return true;
                            }
                        }
                    }
                    return false;
                }
            });
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
        // ~RemoteDataClass() {
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
