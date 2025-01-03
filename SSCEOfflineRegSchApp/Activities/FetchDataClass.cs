using Neurotec.Devices;
using SSCEOfflineRegSchApp.DB;
using SSCEOfflineRegSchApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Activities
{
    public class FetchDataClass:IDisposable
    {

        public async Task<FinSaveModel> FetchFinRecords(string Application, string SchoolNo)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchFinRecords(Application, SchoolNo);
            return result;
        }
        public async Task<CandidateSaveModel> FetchRecordsToEdit(string search)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordsToEdit(search);
            return result;
        }

        public async Task<List<SerialNoModel>> FetchSerailNo()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchSerailNo();
            return result;
        }
        public async Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search, int PageSize, int PageNum)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordsToExport(Search, PageSize, PageNum) as Tuple<List<Registration>, List<Biometrics>>;
            return result;
        }

        public async Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordsToExport(Search) as Tuple<List<Registration>, List<Biometrics>>;
            return result;
        }

        public async Task<List<TemplatesModel>> FetchTemplates()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchTemplates();
            return result;
        }
        public async Task<int> FetchRecordCount(bool Export)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordCount(Export);
            return result;
        }


        public async Task<int> FetchRecordCount()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordCount();
            return result;
        }
        public async Task<List<CandidateViewModel>> FetchRecordsToPreview(string search)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordsToPreview(search);
            return result;
        }

        public async Task<List<CandidateViewModel>> FetchRecordsToPreview(string Search, int PageSize, int PageNum)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchRecordsToPreview(Search, PageSize, PageNum);
            return result;
        }

        public async Task<List<SubjectRefModel>> FetchSubjectsRef()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchSubjectsRef();
            return result;
        }


        public async Task<List<StateSaveModel>> FetchStateRecords()
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchStateRecords();
            return result;
        }

        public async Task<List<LGASaveModel>> FetchLGARecords(string stateCode)
        {
            IGRDal dl = SQLiteDalFactory.GetDal(GrConnector.AccessSQLiteDal);
            var result = await dl.FetchLGARecords(stateCode);
            return result;
        }


        public async Task<List<DisabilityModel>> FetcDisability()
        {
            return await Task<List<DisabilityModel>>.Run(() =>
            {
                var lst = new List<DisabilityModel>();
                string[] ArrGener = { "Blind", "Deaf","Dumb", "Others", "None" };
                foreach (var g in ArrGener)
                {
                    lst.Add(new DisabilityModel
                    {
                        disabled = g
                    });
                }
                return lst;
            });
        }

        public async Task<List<GenderModel>> FetcGender()
        {
            return await Task<List<GenderModel>>.Run(() =>
            {
                var lst = new List<GenderModel>();
                string[] ArrGener = { "Male", "Female" };
                foreach (var g in ArrGener)
                {
                    lst.Add(new GenderModel
                    {
                        Gender = g
                    });
                }
                return lst;
            });
        }

        public async Task<List<PageSizeModel>> FetchPageSize()
        {
            return await Task.Run(() =>
            {
                List<PageSizeModel> lst = new List<PageSizeModel>();
                for (int i = 5; i < 101; i++)
                {
                    lst.Add(new PageSizeModel
                    {
                        size = i
                    });
                }
                return lst;
            });
        }

        public async Task<List<CameraModel>> FetchCameras(NDeviceManager _deviceManager)
        {
            return await Task.Run(() =>
            {
                List<CameraModel> lst = new List<CameraModel>();
                foreach (NDevice device in _deviceManager.Devices)
                {
                   lst.Add(new CameraModel
                   {
                     device=device
                   });
                }
                return lst;
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
