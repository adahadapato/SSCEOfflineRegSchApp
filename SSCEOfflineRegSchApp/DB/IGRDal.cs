using SSCEOfflineRegSchApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.DB
{
    public interface IGRDal
    {
        Task<bool> CreateDataBase();
        Task<bool> SaveLGAToDatabase(List<LGASaveModel> model);
        Task<bool> SaveStatesToDatabase(List<StateSaveModel> model);
        Task<List<StateSaveModel>> FetchStateRecords();
        Task<List<LGASaveModel>> FetchLGARecords(string stateCode);
        Task<List<LGASaveModel>> FetchLGARecords();
        Task<bool> SaveSubjectToDatabase(List<SubjectRefModel> model);
        Task<List<SubjectRefModel>> FetchSubjectsRef();

        Task<bool> SaveCadidateRecord(CandidateSaveModel Models);
        Task<bool> UpdateCadidateRecord(CandidateSaveModel Models);
        Task<List<CandidateViewModel>> FetchRecordsToPreview(string search);
        Task<List<CandidateViewModel>> FetchRecordsToPreview(string Search, int PageSize, int PageNum);
        Task<int> FetchRecordCount(bool Export);
        Task<int> FetchRecordCount();
        Task<List<TemplatesModel>> FetchTemplates();
        Task<bool> ClearTemplates(string SerialNumber);
        Task<bool> SaveBiometrics(BiometricsSaveModel model);

        Task<bool> SaveTemplate(TemplatesModel model);
        Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search, int PageSize, int PageNum);
        Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search);
        Task<bool> UpdateStatus(List<Registration> Models);
        Task<bool> UpdateStatus(List<Registration> Models, int status);
        Task<bool> DeleteRecord(string SerialNumber);
        Task<bool> BackupRecords();
        Task<bool> UpdateSerialNo(List<SerialNoModel> model);
        Task<List<SerialNoModel>> FetchSerailNo();
        Task<bool> SaveNewSerialNo();
        Task<CandidateSaveModel> FetchRecordsToEdit(string search);
        Task<bool> SaveFinToDatabase(List<FinSaveModel> model);
        Task<FinSaveModel> FetchFinRecords(string Application, string SchoolNo);
    }
}
