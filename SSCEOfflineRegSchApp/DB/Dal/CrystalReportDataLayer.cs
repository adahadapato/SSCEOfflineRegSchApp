using CrystalDecisions.CrystalReports.Engine;
using SSCEOfflineRegSchApp.Activities;
using SSCEOfflineRegSchApp.DB.Reports;
using SSCEOfflineRegSchApp.Model;
using SSCEOfflineRegSchApp.RegistryHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.DB.Dal
{
    public class CrystalReportDataLayer : IDisposable
    {

        public async Task<ReportDocument> GenerateDataForEntryScheduleReport()
        {

            try
            {
                DataSet ds = new dsStudentsRecord();
                var rpt = new rptEntrySchedule() as ReportDocument;
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var data = await fd.FetchRecordsToPreview("");
                    foreach (var d in data)
                    {
                        DataRow drow = ds.Tables[0].NewRow();
                        drow["SerialNumber"] = d.serialNumber;
                        drow["cand_name"] = d.Name;
                        //drow["Firstname"] = d.firstName;
                        //drow["Middlename"] = d.middleName;
                        drow["sex"] = d.Gender;
                        drow["lgaName"] = d.lgaName;
                        //drow["s_of_o"] = d.stateOfOriginName;
                        drow["d_of_b"] = d.d_of_b;
                        //drow["Passport"] = d.bPassport;

                        drow["Subj1"] = d.Subj1;
                        //drow["Subj1_CA1"] = d.Subj1_CA1;
                        //drow["Subj1_CA2"] = d.Subj1_CA2;

                        drow["Subj2"] = d.Subj2;
                        //drow["Subj2_CA1"] = d.Subj2_CA1;
                        //drow["Subj2_CA2"] = d.Subj2_CA2;

                        drow["Subj3"] = d.Subj3;
                        //drow["Subj3_CA1"] = d.Subj3_CA1;
                        //drow["Subj3_CA2"] = d.Subj3_CA2;

                        drow["Subj4"] = d.Subj4;
                        //drow["Subj4_CA1"] = d.Subj4_CA1;
                        //drow["Subj4_CA2"] = d.Subj4_CA2;

                        drow["Subj5"] = d.Subj5;
                        //drow["Subj5_CA1"] = d.Subj5_CA1;
                        //drow["Subj5_CA2"] = d.Subj5_CA2;

                        drow["Subj6"] = d.Subj6;
                        //drow["Subj6_CA1"] = d.Subj6_CA1;
                        //drow["Subj6_CA2"] = d.Subj6_CA2;

                        drow["Subj7"] = d.Subj7;
                        //drow["Subj7_CA1"] = d.Subj7_CA1;
                        //drow["Subj7_CA2"] = d.Subj7_CA2;

                        drow["Subj8"] = d.Subj8;
                        //drow["Subj8_CA1"] = d.Subj8_CA1;
                        //drow["Subj8_CA2"] = d.Subj8_CA2;

                        drow["Subj9"] = d.Subj9;
                        //drow["Subj9_CA1"] = d.Subj9_CA1;
                        //drow["Subj9_CA2"] = d.Subj9_CA2;
                        drow["NumberOfSubjects"] = d.numberOfSubjects;

                        ds.Tables[0].Rows.Add(drow);
                    }
                }

                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    string SchoolName = rh.SchoolName.Trim();
                    string SchoolNumber = rh.SchoolNo;
                    string Examyear = rh.ExamYear;
                    // string ExamType = (model.CardType.Contains("RESULTS")) ? string.Format("{0} RECEIPT", model.CardType) : string.Format("{0} {1} ({2}) {3} RECEIPT", model.Exam, model.ExamYear, model.ExamType, model.CardType);
                    //string numberOfcards = model.NumberOfCards.ToString();

                    rpt.DataDefinition.FormulaFields["schnum"].Text = '"' + SchoolNumber + '"';
                    rpt.DataDefinition.FormulaFields["sch_name"].Text = '"' + SchoolName + '"';
                    rpt.DataDefinition.FormulaFields["exam_year"].Text = '"' + Examyear.ToString() + '"';
                    // rpt.DataDefinition.FormulaFields["OfficerCode"].Text = '"' + OfficerCode.ToString() + '"';
                    //rpt.DataDefinition.FormulaFields["numberOfCards"].Text = '"' + numberOfcards.ToString() + '"';
                }



                ds.Tables[0].AcceptChanges();
                rpt.SetDataSource(ds);
                return rpt;
            }
            catch (Exception)
            {
                //Program.Passed = false;
            }
            return null;

        }

        public async Task<ReportDocument> GenerateDataForDocumentRegistrationReport()
        {
           
            try
            {
                DataSet ds = new dsStudentsRecord();
                var rpt = new rptRegistrationReport() as ReportDocument;
                using (FetchDataClass fd = new FetchDataClass())
                {
                    var data = await fd.FetchRecordsToPreview("");
                    foreach (var d in data)
                    {
                        DataRow drow = ds.Tables[0].NewRow();
                        drow["SerialNumber"] = d.serialNumber;
                        drow["Surname"] = d.surName;
                        drow["Firstname"] = d.firstName;
                        drow["Middlename"] = d.middleName;
                        drow["sex"] = d.Sex;
                        drow["lgaName"] = d.lgaName;
                        drow["s_of_o"] = d.stateOfOriginName;
                        drow["d_of_b"] = d.d_of_b;
                        drow["Passport"] = d.bPassport;

                        drow["Subj1"] = d.Subj1;
                        drow["Subj1_CA1"] = d.Subj1_CA1;
                        drow["Subj1_CA2"] = d.Subj1_CA2;

                        drow["Subj2"] = d.Subj2;
                        drow["Subj2_CA1"] = d.Subj2_CA1;
                        drow["Subj2_CA2"] = d.Subj2_CA2;

                        drow["Subj3"] = d.Subj3;
                        drow["Subj3_CA1"] = d.Subj3_CA1;
                        drow["Subj3_CA2"] = d.Subj3_CA2;

                        drow["Subj4"] = d.Subj4;
                        drow["Subj4_CA1"] = d.Subj4_CA1;
                        drow["Subj4_CA2"] = d.Subj4_CA2;

                        drow["Subj5"] = d.Subj5;
                        drow["Subj5_CA1"] = d.Subj5_CA1;
                        drow["Subj5_CA2"] = d.Subj5_CA2;

                        drow["Subj6"] = d.Subj6;
                        drow["Subj6_CA1"] = d.Subj6_CA1;
                        drow["Subj6_CA2"] = d.Subj6_CA2;

                        drow["Subj7"] = d.Subj7;
                        drow["Subj7_CA1"] = d.Subj7_CA1;
                        drow["Subj7_CA2"] = d.Subj7_CA2;

                        drow["Subj8"] = d.Subj8;
                        drow["Subj8_CA1"] = d.Subj8_CA1;
                        drow["Subj8_CA2"] = d.Subj8_CA2;

                        drow["Subj9"] = d.Subj9;
                        drow["Subj9_CA1"] = d.Subj9_CA1;
                        drow["Subj9_CA2"] = d.Subj9_CA2;

                        ds.Tables[0].Rows.Add(drow);
                    }
                }

                using (RegistryHelperClass rh = new RegistryHelperClass())
                {
                    string SchoolName = rh.SchoolName.Trim();
                    string SchoolNumber = rh.SchoolNo;
                    string Examyear = rh.ExamYear;
                    // string ExamType = (model.CardType.Contains("RESULTS")) ? string.Format("{0} RECEIPT", model.CardType) : string.Format("{0} {1} ({2}) {3} RECEIPT", model.Exam, model.ExamYear, model.ExamType, model.CardType);
                    //string numberOfcards = model.NumberOfCards.ToString();

                    rpt.DataDefinition.FormulaFields["schnum"].Text = '"' + SchoolNumber + '"';
                    rpt.DataDefinition.FormulaFields["sch_name"].Text = '"' + SchoolName + '"';
                    rpt.DataDefinition.FormulaFields["exam_year"].Text = '"' + Examyear.ToString() + '"';
                    // rpt.DataDefinition.FormulaFields["OfficerCode"].Text = '"' + OfficerCode.ToString() + '"';
                    //rpt.DataDefinition.FormulaFields["numberOfCards"].Text = '"' + numberOfcards.ToString() + '"';
                }



                ds.Tables[0].AcceptChanges();
                rpt.SetDataSource(ds);
                return rpt;
            }
            catch (Exception)
            {
                //Program.Passed = false;
            }
            return null;

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
        // ~CrystalReportDataLayer() {
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
