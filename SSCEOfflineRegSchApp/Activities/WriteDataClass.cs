using SSCEOfflineRegSchApp.DB;
using SSCEOfflineRegSchApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Activities
{
    public class WriteDataClass:IDisposable
    {

        public async Task<bool> SaveNewSerialNo()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveNewSerialNo();
            return result;
        }
        public async Task<bool> BackupRecords()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.BackupRecords();
            return result;
        }

        public async Task<bool> UpdateSerialNo(List<SerialNoModel> model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.UpdateSerialNo(model);
            return result;
        }
        public async Task<bool> DeleteRecord(string SerialNumber)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.DeleteRecord(SerialNumber);
            return result;
        }

        public async Task<bool> UpdateStatus(List<Registration> Models, int status)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.UpdateStatus(Models, status);
            return result;
        }
        public async Task<bool> UpdateStatus(List<Registration> Models)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.UpdateStatus(Models);
            return result;
        }
        public async Task<bool> SaveTemplate(TemplatesModel model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveTemplate(model);
            return result;
        }

        public async Task<bool> SaveBiometrics(BiometricsSaveModel model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveBiometrics(model);
            return result;
        }

        public async Task<bool> ClearTemplates(string SerialNumber)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.ClearTemplates(SerialNumber);
            return result;
        }


        public async Task<bool> SaveCadidateRecord(CandidateSaveModel Models)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveCadidateRecord(Models);
            return result;
        }

        public async Task<bool> UpdateCadidateRecord(CandidateSaveModel Models)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.UpdateCadidateRecord(Models);
            return result;
        }
        public async Task<bool> CreateDataBase()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result= await dl.CreateDataBase();
            return result;
        }

       public async Task<bool> SaveStatesToDatabase(List<StateSaveModel> model)
       {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveStatesToDatabase(model);
            return result;
        }

        public async Task<bool> SaveLGAToDatabase(List<LGASaveModel> model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveLGAToDatabase(model);
            return result;
        }

        public async Task<bool> SaveSubjectToDatabase(List<SubjectRefModel> model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveSubjectToDatabase(model);
            return result;
        }

        public async Task<bool> SaveFinToDatabase(List<FinSaveModel> model)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.SaveFinToDatabase(model);
            return result;
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
        // ~WriteDataClass() {
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
