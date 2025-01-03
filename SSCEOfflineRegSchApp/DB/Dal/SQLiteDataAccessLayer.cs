using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using SSCEOfflineRegSchApp.Tools;
using SSCEOfflineRegSchApp.RegistryHelper;
using SSCEOfflineRegSchApp.Model;
using System.Globalization;

namespace SSCEOfflineRegSchApp.DB.Dal
{
    public class SQLiteDataAccessLayer : IGRDal
    {
        private SQLiteConnection dbConection;
        public readonly string CONNECTION_STRING;
        private readonly string DBFileName;


        private SQLiteDataAccessLayer()
        {
            using (RegistryHelperClass rh = new RegistryHelperClass())
            {
                DBFileName = string.Format("{0}{1}.db", AppPathClass.FetchPath, rh.SchoolNo);
                CONNECTION_STRING = "Data Source = " + DBFileName + "; Version=3;";
            }

            dbConection = new SQLiteConnection(CONNECTION_STRING);
            //operatorID = regClass.Operartorid;
            //operatorUserName = regClass.UserName;
        }

        #region Database and Tables Creation Section
        public async Task<bool> CreateDataBase()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    bool TablesCreated = false;
                    if (File.Exists(DBFileName))
                    {
                        File.Delete(DBFileName);
                    }

                    SQLiteConnection.CreateFile(DBFileName);
                    if (CreateTables("BZPersonalInfo"))
                        TablesCreated = true;
                    else
                        return false;


                    if (CreateTables("BZSubjectRef"))
                        TablesCreated = true;
                    else
                        return false;


                    //CreateTables("BZBiometrics");
                    if (CreateTables("BZFin"))
                        TablesCreated = true;
                    else
                        return false;


                    if (CreateTables("BZState"))
                        TablesCreated = true;
                    else
                        return false;

                    if (CreateTables("BZLGA"))
                        TablesCreated = true;
                    else
                        return false;

                    if (CreateTables("BZTemplates"))
                        TablesCreated = true;
                    else
                        return false;

                    if (CreateTables("BZTemp"))
                        TablesCreated = true;
                    else
                        return false;

                    


                    return TablesCreated;
                }
                catch (Exception e)
                {
                     SafeGuiWpf.ShowError("Create DB\n" + e.Message + " " + e.StackTrace);
                }
                await Task.Delay(3000);
                return false;
            });

        }


        private bool CreateTables(string TableName)
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            try
            {
                string strCommand = string.Empty;
                switch (TableName)
                {
                    case "BZPersonalInfo":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZPersonalInfo] (
                          [SerialNumber] NVARCHAR(4) NOT NULL PRIMARY KEY, 
                          [SerialNoTemp] NVARCHAR(4) NULL,
                          [schoolNo] NVARCHAR(7) NOT NULL,
                          [surName] NVARCHAR(45) NOT NULL,
                          [firstName] NVARCHAR(45) NOT NULL,
                          [middleName] NVARCHAR(45) NULL,
                          [Sex] NVARCHAR(8) NOT NULL,
                          [dateOfBirth] INTEGER NOT NULL,
                          [disabled] NVARCHAR(15) NOT NULL,
                          [stateOfOriginCode] NVARCHAR(3) NOT NULL,
                          [stateOfOriginName] NVARCHAR(20) NOT NULL,
                          [lgaCode] NVARCHAR(4) NOT NULL,
                          [lgaName] NVARCHAR(60) NOT NULL,
                          [Passport] BLOB NOT NULL,
                          
                          [subj1] NVARCHAR(3) NOT NULL,
                          [code1] NVARCHAR(3) NOT NULL,
                          [subj2] NVARCHAR(3) NOT NULL,
                          [code2] NVARCHAR(3) NOT NULL,
                          [subj3] NVARCHAR(3) NULL,
                          [code3] NVARCHAR(3) NULL,
                          [subj4] NVARCHAR(3) NULL,
                          [code4] NVARCHAR(3) NULL,
                          [subj5] NVARCHAR(3) NULL,
                          [code5] NVARCHAR(3) NULL,
                          [subj6] NVARCHAR(3) NULL,
                          [code6] NVARCHAR(3) NULL,
                          [subj7] NVARCHAR(3) NULL,
                          [code7] NVARCHAR(3) NULL,
                          [subj8] NVARCHAR(3) NULL,
                          [code8] NVARCHAR(3) NULL,
                          [subj9] NVARCHAR(3) NULL,
                          [code9] NVARCHAR(3) NULL,
                          
                          [subj1_CA1] NVARCHAR(2) NOT NULL,
                          [subj2_CA1] NVARCHAR(2) NOT NULL,
                          [subj3_CA1] NVARCHAR(2) NULL,
                          [subj4_CA1] NVARCHAR(2) NULL,
                          [subj5_CA1] NVARCHAR(2) NULL,
                          [subj6_CA1] NVARCHAR(2) NULL,
                          [subj7_CA1] NVARCHAR(2) NULL,
                          [subj8_CA1] NVARCHAR(2) NULL,
                          [subj9_CA1] NVARCHAR(2) NULL,

                          [subj1_CA2] NVARCHAR(2) NOT NULL,
                          [subj2_CA2] NVARCHAR(2) NOT NULL,
                          [subj3_CA2] NVARCHAR(2) NULL,
                          [subj4_CA2] NVARCHAR(2) NULL,
                          [subj5_CA2] NVARCHAR(2) NULL,
                          [subj6_CA2] NVARCHAR(2) NULL,
                          [subj7_CA2] NVARCHAR(2) NULL,
                          [subj8_CA2] NVARCHAR(2) NULL,
                          [subj9_CA2] NVARCHAR(2) NULL,

                          [numberOfSubjects] INTEGER NOT NULL,

                          [leftThumbImage] BLOB NULL,
                          [leftThumbTemplate] BLOB NULL,
                          [leftThumbQuality] INTEGER NULL,

                          [leftIndexImage] BLOB NULL,
                          [leftIndexTemplate] BLOB NULL,
                          [leftIndexQuality] INTEGER NULL,

                          [leftMiddleImage] BLOB NULL,  
                          [leftMiddleTemplate] BLOB NULL,
                          [leftMiddleQuality] INTEGER NULL,

                          [leftRingImage] BLOB NULL,
                          [leftRingTemplate] BLOB NULL,
                          [leftRingQuality] INTEGER NULL,

                          [leftPinkyImage] BLOB NULL,
                          [leftPinkyTemplate] BLOB NULL,
                          [leftPinkyQuality] INTEGER NULL,

                          [rightThumbImage] BLOB NULL,
                          [rightThumbTemplate] BLOB NULL,
                          [rightThumbQuality] INTEGER NULL,

                          [rightIndexImage] BLOB NULL,
                          [rightIndexTemplate] BLOB NULL,
                          [rightIndexQuality] INTEGER NULL,

                          [rightMiddleImage] BLOB NULL,
                          [rightMiddleTemplate] BLOB NULL,
                          [rightMiddleQuality] INTEGER NULL,

                          [rightRingImage] BLOB NULL,
                          [rightRingTemplate] BLOB NULL,
                          [rightRingQuality] INTEGER NULL,

                          [rightPinkyImage] BLOB NULL,
                          [rightPinkyTemplate] BLOB NULL,
                          [rightPinkyQuality] INTEGER NULL,
                          [status] INTEGER NOT NULL,
                          [hasBiometrics] BOOLEAN NOT NULL,
                          [isComplete] BOOLEAN NOT NULL
                          )";
                        break;
                    case "BZTemp":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZTemp] (
                          [SerialNumber] NVARCHAR(4) NOT NULL PRIMARY KEY, 
                          [SerialNoTemp] NVARCHAR(4) NULL,
                          [schoolNo] NVARCHAR(7) NOT NULL,
                          [surName] NVARCHAR(45) NOT NULL,
                          [firstName] NVARCHAR(45) NOT NULL,
                          [middleName] NVARCHAR(45) NULL,
                          [Sex] NVARCHAR(8) NOT NULL,
                          [dateOfBirth] INTEGER NOT NULL,
                          [disabled] NVARCHAR(15) NOT NULL,
                          [stateOfOriginCode] NVARCHAR(3) NOT NULL,
                          [stateOfOriginName] NVARCHAR(20) NOT NULL,
                          [lgaCode] NVARCHAR(4) NOT NULL,
                          [lgaName] NVARCHAR(60) NOT NULL,
                          [Passport] BLOB NOT NULL,
                          
                          [subj1] NVARCHAR(3) NOT NULL,
                          [code1] NVARCHAR(3) NOT NULL,
                          [subj2] NVARCHAR(3) NOT NULL,
                          [code2] NVARCHAR(3) NOT NULL,
                          [subj3] NVARCHAR(3) NULL,
                          [code3] NVARCHAR(3) NULL,
                          [subj4] NVARCHAR(3) NULL,
                          [code4] NVARCHAR(3) NULL,
                          [subj5] NVARCHAR(3) NULL,
                          [code5] NVARCHAR(3) NULL,
                          [subj6] NVARCHAR(3) NULL,
                          [code6] NVARCHAR(3) NULL,
                          [subj7] NVARCHAR(3) NULL,
                          [code7] NVARCHAR(3) NULL,
                          [subj8] NVARCHAR(3) NULL,
                          [code8] NVARCHAR(3) NULL,
                          [subj9] NVARCHAR(3) NULL,
                          [code9] NVARCHAR(3) NULL,
                          
                          [subj1_CA1] NVARCHAR(2) NOT NULL,
                          [subj2_CA1] NVARCHAR(2) NOT NULL,
                          [subj3_CA1] NVARCHAR(2) NULL,
                          [subj4_CA1] NVARCHAR(2) NULL,
                          [subj5_CA1] NVARCHAR(2) NULL,
                          [subj6_CA1] NVARCHAR(2) NULL,
                          [subj7_CA1] NVARCHAR(2) NULL,
                          [subj8_CA1] NVARCHAR(2) NULL,
                          [subj9_CA1] NVARCHAR(2) NULL,

                          [subj1_CA2] NVARCHAR(2) NOT NULL,
                          [subj2_CA2] NVARCHAR(2) NOT NULL,
                          [subj3_CA2] NVARCHAR(2) NULL,
                          [subj4_CA2] NVARCHAR(2) NULL,
                          [subj5_CA2] NVARCHAR(2) NULL,
                          [subj6_CA2] NVARCHAR(2) NULL,
                          [subj7_CA2] NVARCHAR(2) NULL,
                          [subj8_CA2] NVARCHAR(2) NULL,
                          [subj9_CA2] NVARCHAR(2) NULL,

                          [numberOfSubjects] INTEGER NOT NULL,

                          [leftThumbImage] BLOB NULL,
                          [leftThumbTemplate] BLOB NULL,
                          [leftThumbQuality] INTEGER NULL,

                          [leftIndexImage] BLOB NULL,
                          [leftIndexTemplate] BLOB NULL,
                          [leftIndexQuality] INTEGER NULL,

                          [leftMiddleImage] BLOB NULL,  
                          [leftMiddleTemplate] BLOB NULL,
                          [leftMiddleQuality] INTEGER NULL,

                          [leftRingImage] BLOB NULL,
                          [leftRingTemplate] BLOB NULL,
                          [leftRingQuality] INTEGER NULL,

                          [leftPinkyImage] BLOB NULL,
                          [leftPinkyTemplate] BLOB NULL,
                          [leftPinkyQuality] INTEGER NULL,

                          [rightThumbImage] BLOB NULL,
                          [rightThumbTemplate] BLOB NULL,
                          [rightThumbQuality] INTEGER NULL,

                          [rightIndexImage] BLOB NULL,
                          [rightIndexTemplate] BLOB NULL,
                          [rightIndexQuality] INTEGER NULL,

                          [rightMiddleImage] BLOB NULL,
                          [rightMiddleTemplate] BLOB NULL,
                          [rightMiddleQuality] INTEGER NULL,

                          [rightRingImage] BLOB NULL,
                          [rightRingTemplate] BLOB NULL,
                          [rightRingQuality] INTEGER NULL,

                          [rightPinkyImage] BLOB NULL,
                          [rightPinkyTemplate] BLOB NULL,
                          [rightPinkyQuality] INTEGER NULL,
                          [status] INTEGER NOT NULL,
                          [hasBiometrics] BOOLEAN NOT NULL,
                          [isComplete] BOOLEAN NOT NULL
                          )";
                        break;
                    case "BZSubjectRef":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZSubjectRef] (
                          [code] NVARCHAR(3) NOT NULL PRIMARY KEY,
                          [subject] NVARCHAR(3) NOT NULL,
                          [descript] NVARCHAR(40) NOT NULL
                          )";
                        break;
                    case "BZFin":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZFin] (
                          [schnum] NVARCHAR(7) NOT NULL PRIMARY KEY,
                          [SchoolName] NVARCHAR(104) NOT NULL,
                          [ApplicationId] NVARCHAR(15) NOT NULL
                          )";
                        break;
                    case "BZState":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZState] (
                          [Code] NVARCHAR(3) NOT NULL PRIMARY KEY,
                          [State] NVARCHAR(45) NOT NULL
                          )";
                        break;
                    case "BZLGA":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZLGA] (
                          [lgaCode] NVARCHAR(3) NOT NULL PRIMARY KEY,
                          [stateCode] NVARCHAR(3) NOT NULL,
                          [LgaName] NVARCHAR(50) NOT NULL
                          )";
                        break;
                    case "BZBiometrics":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZBiometrics] (
                          [SerialNumber] NVARCHAR(4) NOT NULL PRIMARY KEY, 
                                                    
                          [leftThumbImage] BLOB NOT NULL,
                          [leftIndexImage] BLOB NOT NULL,
                          [leftMiddleImage] BLOB NOT NULL,  
                          [leftRingImage] BLOB NOT NULL,
                          [leftPinkyImage] BLOB NOT NULL,

                          [rightThumbImage] BLOB NOT NULL,
                          [rightIndexImage] BLOB NOT NULL,
                          [rightMiddleImage] BLOB NOT NULL,
                          [rightRingImage] BLOB NOT NULL,
                          [rightPinkyImage] BLOB NOT NULL,
                          [status] NVARCHAR(1) NOT NULL
                         
                          )";
                        break;
                    case "BZTemplates":
                        strCommand = @"CREATE TABLE IF NOT EXISTS [BZTemplates] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [SerialNumber] NVARCHAR(4) NOT NULL , 
                          [template] BLOB NOT NULL,
                          [quality] INTEGER NOT NULL
                          
                          )";
                        break;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
               SafeGuiWpf.ShowError(e.Message);
            }
            return false;
        }

        public void TrucateTable(string TableName)
        {
            string sql = "DELETE FROM " + TableName;
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Fin Section
        public async Task<bool> SaveFinToDatabase(List<FinSaveModel> model)
        {
            TrucateTable("BZFin");
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            return await Task.Run(() =>
            {
                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            // 100,000 inserts
                            foreach (var i in model)//= 0; i < 1000000; i++)
                            {
                                cmd.CommandText =
                                    "INSERT INTO BZFin (schnum, SchoolName, ApplicationId) VALUES (@schnum, @SchoolName, @ApplicationId)";
                                cmd.Parameters.Add(new SQLiteParameter("@schnum", i.schnum));
                                cmd.Parameters.Add(new SQLiteParameter("@SchoolName", i.SchoolName.Trim()));
                                cmd.Parameters.Add(new SQLiteParameter("@ApplicationId", i.ApplicationId));
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                    }
                    // message = "All States saved successfully";
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError("Error: " + e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<FinSaveModel> FetchFinRecords(string Application, string SchoolNo)
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
  
            return await Task<List<FinSaveModel>>.Run(() =>
            {
                try
                {
                    string sql = "SELECT *  FROM BZFin WHERE schnum=@SchoolNo AND ApplicationId=@Application";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Application", Application));
                        cmd.Parameters.Add(new SQLiteParameter("@SchoolNo", SchoolNo));
                        SQLiteDataReader dr;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                return new FinSaveModel
                                {
                                   schnum = dr["schnum"].ToString(),
                                    SchoolName = dr["SchoolName"].ToString(),
                                    ApplicationId = dr["ApplicationId"].ToString()
                                };
                            }
                        }
                    }

                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });

        }
        #endregion

        #region Subject Section
        public async Task<bool> SaveSubjectToDatabase(List<SubjectRefModel> model)
        {
            TrucateTable("BZSubjectRef");
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            return await Task.Run(() =>
            {
                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            // 100,000 inserts
                            foreach (var i in model)//= 0; i < 1000000; i++)
                            {
                                cmd.CommandText =
                                    "INSERT INTO BZSubjectRef (code, subject, descript) VALUES (@code, @subject, @descript)";
                                cmd.Parameters.Add(new SQLiteParameter("@code", i.code));
                                cmd.Parameters.Add(new SQLiteParameter("@subject", i.subject));
                                cmd.Parameters.Add(new SQLiteParameter("@descript", i.descript.Trim()));
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                    }
                    // message = "All States saved successfully";
                    return true;
                }
                catch (Exception e)
                {
                     SafeGuiWpf.ShowError("Error: " + e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<List<SubjectRefModel>> FetchSubjectsRef()
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<SubjectRefModel>.Run(() =>
            {
                var list = new List<SubjectRefModel>();
                try
                {
                    string sql = "SELECT *  FROM BZSubjectRef ";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        SQLiteDataReader dr;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                list.Add(new SubjectRefModel
                                {
                                    code = dr["code"].ToString(),
                                    subject = dr["subject"].ToString(),
                                    descript = dr["descript"].ToString()
                                });
                            }
                        }
                    }
                    return list;
                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });
        }
        #endregion

        #region States & LGA Section

        public async Task<bool> SaveStatesToDatabase(List<StateSaveModel> model)
        {
            TrucateTable("BZState");
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<bool>.Run(() =>
            {
                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            foreach (var i in model)//= 0; i < 1000000; i++)
                            {
                                cmd.CommandText =
                                    "INSERT INTO BZState(code, state) VALUES(@code, @state)";
                                cmd.Parameters.Add(new SQLiteParameter("@code", i.stateCode));
                                cmd.Parameters.Add(new SQLiteParameter("@state", i.stateName.Trim()));
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError("Error: " + e.Message + "" + e.StackTrace);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });

        }


        public async Task<bool> SaveLGAToDatabase(List<LGASaveModel> model)
        {
            TrucateTable("BZLGA");
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            return await Task.Run(() =>
            {
                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            // 100,000 inserts
                            foreach (var i in model)//= 0; i < 1000000; i++)
                            {
                                cmd.CommandText =
                                    "INSERT INTO BZLGA (lgaCode, stateCode, LGAName) VALUES (@lgaCode, @stateCode, @lgaName)";
                                cmd.Parameters.Add(new SQLiteParameter("@lgaCode", i.lgaCode));
                                cmd.Parameters.Add(new SQLiteParameter("@stateCode", i.stateCode));
                                cmd.Parameters.Add(new SQLiteParameter("@lgaName", i.LGAName.Trim()));
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                    }
                    // message = "All States saved successfully";
                    return true;
                }
                catch (Exception e)
                {
                     SafeGuiWpf.ShowError("Error: " + e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }

        public async Task<List<LGASaveModel>> FetchLGARecords()
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<LGASaveModel>.Run(() =>
            {
                var list = new List<LGASaveModel>();
                try
                {
                    string sql = "SELECT *  FROM BZLGA ";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        SQLiteDataReader dr;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                list.Add(new LGASaveModel
                                {
                                    lgaCode = dr["lgaCode"].ToString(),
                                    LGAName = dr["LGAName"].ToString(),
                                    stateCode = dr["stateCode"].ToString()
                                });
                            }
                        }
                    }
                    return list;
                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });
        }


        public async Task<List<LGASaveModel>> FetchLGARecords(string stateCode)
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            var list = new List<LGASaveModel>();
            return await Task<List<LGASaveModel>>.Run(() =>
            {
                try
                {
                    string sql = "SELECT *  FROM BZLGA WHERE stateCode=@stateCode";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@stateCode", stateCode));
                        SQLiteDataReader dr;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                list.Add(new LGASaveModel
                                {
                                    lgaCode = dr["lgaCode"].ToString(),
                                    LGAName = dr["LGAName"].ToString(),
                                    stateCode = dr["stateCode"].ToString()
                                });
                            }
                        }
                    }
                    return list;

                }
                catch (Exception e)
                {

                   SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });

        }


        public async Task<List<StateSaveModel>> FetchStateRecords()
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            return await Task.Run(() =>
            {
                var states = new List<StateSaveModel>();
                try
                {
                    string sql = "SELECT *  FROM BZState";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        SQLiteDataReader dr;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                states.Add(new StateSaveModel
                                {
                                    stateCode = dr["code"].ToString(),
                                    stateName = dr["state"].ToString()
                                });
                            }
                            return states;
                        }
                    }
                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });

        }

        #endregion

        #region Cadidate Record Section
        public async Task<bool> SaveCadidateRecord(CandidateSaveModel Models)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            var model = Models.personalInfo as PersonalInfoSaveModel;
                            var Model = Models.subjects as SubjectSaveModel;
                            var status = 0;
                            var hasBiometrics = false;
                            cmd.CommandText =
                                      "INSERT INTO BZPersonalInfo(schoolNo, SerialNumber," +
                                       "disabled, surName, firstName, middleName," +
                                       "Sex, dateOfBirth, stateOfOriginCode, stateOfOriginName, lgaCode, lgaName, Passport," +
                                       "subj1, subj2, subj3, subj4, subj5, subj6, subj7, subj8, subj9," +
                                       "code1, code2, code3, code4, code5, code6, code7, code8, code9, "+
                                       "subj1_CA1, subj2_CA1, subj3_CA1, subj4_CA1, subj5_CA1, subj6_CA1, subj7_CA1, subj8_CA1, subj9_CA1," +
                                       "subj1_CA2, subj2_CA2, subj3_CA2, subj4_CA2, subj5_CA2, subj6_CA2, subj7_CA2, subj8_CA2, subj9_CA2," +
                                       "status, hasBiometrics, numberOfSubjects, isComplete) VALUES (" +
                                       "@schoolNo, @serialNumber," +
                                       "@disabled, @surName, @firstName, @middleName," +
                                       "@Sex, @dateOfBirth, @stateOfOriginCode, @stateOfOriginName, @lgaCode, @lgaName, @Passport," +
                                       "@subj1, @subj2, @subj3, @subj4, @subj5, @subj6, @subj7, @subj8, @subj9," +
                                       "@code1, @code2, @code3, @code4, @code5, @code6, @code7, @code8, @code9, "+
                                       "@subj1_CA1, @subj2_CA1, @subj3_CA1, @subj4_CA1, @subj5_CA1, @subj6_CA1, @subj7_CA1, @subj8_CA1, @subj9_CA1," +
                                       "@subj1_CA2, @subj2_CA2, @subj3_CA2, @subj4_CA2, @subj5_CA2, @subj6_CA2, @subj7_CA2, @subj8_CA2, @subj9_CA2," +
                                       "@status, @hasBiometrics, @numberOfSubjects, @isComplete)";

                            cmd.Parameters.Add(new SQLiteParameter("@schoolNo", model.SchoolNo));
                            cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", model.serialNumber));

                            cmd.Parameters.Add(new SQLiteParameter("@disabled", model.disabled));
                            cmd.Parameters.Add(new SQLiteParameter("@surName", model.surName));
                            cmd.Parameters.Add(new SQLiteParameter("@firstName", model.firstName));
                            cmd.Parameters.Add(new SQLiteParameter("@middleName", model.middleName));

                            cmd.Parameters.Add(new SQLiteParameter("@Sex", model.Sex));
                            cmd.Parameters.Add(new SQLiteParameter("@dateOfBirth", model.dateOfBirth));
                            cmd.Parameters.Add(new SQLiteParameter("@stateOfOriginCode", model.stateOfOriginCode));
                            cmd.Parameters.Add(new SQLiteParameter("@stateOfOriginName", model.stateOfOriginName));
                            cmd.Parameters.Add(new SQLiteParameter("@lgaCode", model.lgaCode));
                            cmd.Parameters.Add(new SQLiteParameter("@lgaName", model.lgaName));
                            cmd.Parameters.Add(new SQLiteParameter("@Passport", model.bPassport));
                            cmd.Parameters.Add(new SQLiteParameter("@isComplete", model.isComplete));

                            cmd.Parameters.Add(new SQLiteParameter("@subj1", Model.Subj1));
                            cmd.Parameters.Add(new SQLiteParameter("@code1", Model.Code1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj1_CA1", Model.Subj1_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj1_CA2", Model.Subj1_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj2", Model.Subj2));
                            cmd.Parameters.Add(new SQLiteParameter("@code2", Model.Code2));
                            cmd.Parameters.Add(new SQLiteParameter("@subj2_CA1", Model.Subj2_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj2_CA2", Model.Subj2_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj3", Model.Subj3));
                            cmd.Parameters.Add(new SQLiteParameter("@code3", Model.Code3));
                            cmd.Parameters.Add(new SQLiteParameter("@subj3_CA1", Model.Subj3_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj3_CA2", Model.Subj3_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj4", Model.Subj4));
                            cmd.Parameters.Add(new SQLiteParameter("@code4", Model.Code4));
                            cmd.Parameters.Add(new SQLiteParameter("@subj4_CA1", Model.Subj4_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj4_CA2", Model.Subj4_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj5", Model.Subj5));
                            cmd.Parameters.Add(new SQLiteParameter("@code5", Model.Code5));
                            cmd.Parameters.Add(new SQLiteParameter("@subj5_CA1", Model.Subj5_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj5_CA2", Model.Subj5_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj6", Model.Subj6));
                            cmd.Parameters.Add(new SQLiteParameter("@code6", Model.Code6));
                            cmd.Parameters.Add(new SQLiteParameter("@subj6_CA1", Model.Subj6_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj6_CA2", Model.Subj6_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj7", Model.Subj7));
                            cmd.Parameters.Add(new SQLiteParameter("@code7", Model.Code7));
                            cmd.Parameters.Add(new SQLiteParameter("@subj7_CA1", Model.Subj7_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj7_CA2", Model.Subj7_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj8", Model.Subj8));
                            cmd.Parameters.Add(new SQLiteParameter("@code8", Model.Code8));
                            cmd.Parameters.Add(new SQLiteParameter("@subj8_CA1", Model.Subj8_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj8_CA2", Model.Subj8_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj9", Model.Subj9));
                            cmd.Parameters.Add(new SQLiteParameter("@code9", Model.Code9));
                            cmd.Parameters.Add(new SQLiteParameter("@subj9_CA1", Model.Subj9_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj9_CA2", Model.Subj9_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@numberOfSubjects", Model.numberOfSubjects));

                            cmd.Parameters.Add(new SQLiteParameter("@status", status));//
                            cmd.Parameters.Add(new SQLiteParameter("@hasBiometrics", hasBiometrics));
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<bool> UpdateStatus(List<Registration> Models, int status)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            
                            foreach (var model in Models)
                            {
                                cmd.CommandText =
                                      "UPDATE BZPersonalInfo SET status=@status WHERE SerialNumber= @SerialNumber";

                                cmd.Parameters.Add(new SQLiteParameter("@status", status));
                                cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", model.ser_no));
                                cmd.ExecuteNonQuery();
                            }
                            
                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<bool> UpdateStatus(List<Registration> Models)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            var status = 1;
                            foreach (var model in Models)
                            {
                                cmd.CommandText =
                                      "UPDATE BZPersonalInfo SET status=@status WHERE SerialNumber= @SerialNumber";

                                cmd.Parameters.Add(new SQLiteParameter("@status", status));
                                cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", model.ser_no));
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }
        public async Task<bool> UpdateCadidateRecord(CandidateSaveModel Models)
        {
            return await Task.Run(() =>
            {
       
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            var model = Models.personalInfo as PersonalInfoSaveModel;
                            var Model = Models.subjects as SubjectSaveModel;
                            
                            cmd.CommandText =
                                      "UPDATE BZPersonalInfo SET schoolNo=@schoolNo," +
                                      "disabled=@disabled, surName=@surName, firstName=@firstName, middleName=@middleName," +
                                      "Sex=@Sex, dateOfBirth= @dateOfBirth, stateOfOriginCode= @stateOfOriginCode, stateOfOriginName=@stateOfOriginName, "+
                                      "lgaCode=@lgaCode, lgaName=@lgaName, Passport=@Passport, status=@status, " +
                                      "subj1=@subj1, subj2= @subj2, subj3= @subj3, subj4=@subj4," +
                                      " subj5=@subj5, subj6=@subj6, subj7=@subj7, subj8=@subj8, subj9=@subj9," +
                                      "code1=@code1, code2=@code2, code3=@code3, code4=@code4, "+
                                      "code5=@code5, code6=@code6, code7=@code7, code8=@code8, code9=@code9, "+
                                      "subj1_CA1=@subj1_CA1, subj2_CA1=@subj2_CA1, subj3_CA1=@subj3_CA1, subj4_CA1=@subj4_CA1, subj5_CA1= @subj5_CA1," +
                                      " subj6_CA1= @subj6_CA1, subj7_CA1=@subj7_CA1, subj8_CA1=@subj8_CA1, subj9_CA1=@subj9_CA1," +
                                      "subj1_CA2=@subj1_CA2, subj2_CA2=@subj2_CA2, subj3_CA2=@subj3_CA2, subj4_CA2=@subj4_CA2, subj5_CA2=@subj5_CA2," +
                                      " subj6_CA2=@subj6_CA2, subj7_CA2=@subj7_CA2, subj8_CA2=@subj8_CA2, subj9_CA2=@subj9_CA2, " +
                                      " numberOfSubjects=@numberOfSubjects, isComplete=@isComplete WHERE SerialNumber= @SerialNumber";

                            cmd.Parameters.Add(new SQLiteParameter("@schoolNo", model.SchoolNo));
                            cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", model.serialNumber));

                            cmd.Parameters.Add(new SQLiteParameter("@disabled", model.disabled));
                            cmd.Parameters.Add(new SQLiteParameter("@surName", model.surName));
                            cmd.Parameters.Add(new SQLiteParameter("@firstName", model.firstName));
                            cmd.Parameters.Add(new SQLiteParameter("@middleName", model.middleName));

                            cmd.Parameters.Add(new SQLiteParameter("@Sex", model.Sex));
                            cmd.Parameters.Add(new SQLiteParameter("@dateOfBirth", model.dateOfBirth));
                            cmd.Parameters.Add(new SQLiteParameter("@stateOfOriginCode", model.stateOfOriginCode));
                            cmd.Parameters.Add(new SQLiteParameter("@stateOfOriginName", model.stateOfOriginName));
                            cmd.Parameters.Add(new SQLiteParameter("@lgaCode", model.lgaCode));
                            cmd.Parameters.Add(new SQLiteParameter("@lgaName", model.lgaName));
                            cmd.Parameters.Add(new SQLiteParameter("@Passport", model.bPassport));
                            cmd.Parameters.Add(new SQLiteParameter("@isComplete", model.isComplete));
                            cmd.Parameters.Add(new SQLiteParameter("@status", model.status));

                            cmd.Parameters.Add(new SQLiteParameter("@subj1", Model.Subj1));
                            cmd.Parameters.Add(new SQLiteParameter("@code1", Model.Code1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj1_CA1", Model.Subj1_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj1_CA2", Model.Subj1_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj2", Model.Subj2));
                            cmd.Parameters.Add(new SQLiteParameter("@code2", Model.Code2));
                            cmd.Parameters.Add(new SQLiteParameter("@subj2_CA1", Model.Subj2_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj2_CA2", Model.Subj2_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj3", Model.Subj3));
                            cmd.Parameters.Add(new SQLiteParameter("@code3", Model.Code3));
                            cmd.Parameters.Add(new SQLiteParameter("@subj3_CA1", Model.Subj3_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj3_CA2", Model.Subj3_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj4", Model.Subj4));
                            cmd.Parameters.Add(new SQLiteParameter("@code4", Model.Code4));
                            cmd.Parameters.Add(new SQLiteParameter("@subj4_CA1", Model.Subj4_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj4_CA2", Model.Subj4_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj5", Model.Subj5));
                            cmd.Parameters.Add(new SQLiteParameter("@code5", Model.Code5));
                            cmd.Parameters.Add(new SQLiteParameter("@subj5_CA1", Model.Subj5_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj5_CA2", Model.Subj5_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj6", Model.Subj6));
                            cmd.Parameters.Add(new SQLiteParameter("@code6", Model.Code6));
                            cmd.Parameters.Add(new SQLiteParameter("@subj6_CA1", Model.Subj6_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj6_CA2", Model.Subj6_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj7", Model.Subj7));
                            cmd.Parameters.Add(new SQLiteParameter("@code7", Model.Code7));
                            cmd.Parameters.Add(new SQLiteParameter("@subj7_CA1", Model.Subj7_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj7_CA2", Model.Subj7_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj8", Model.Subj8));
                            cmd.Parameters.Add(new SQLiteParameter("@code8", Model.Code8));
                            cmd.Parameters.Add(new SQLiteParameter("@subj8_CA1", Model.Subj8_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj8_CA2", Model.Subj8_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@subj9", Model.Subj9));
                            cmd.Parameters.Add(new SQLiteParameter("@code9", Model.Code9));
                            cmd.Parameters.Add(new SQLiteParameter("@subj9_CA1", Model.Subj9_CA1));
                            cmd.Parameters.Add(new SQLiteParameter("@subj9_CA2", Model.Subj9_CA2));

                            cmd.Parameters.Add(new SQLiteParameter("@numberOfSubjects", Model.numberOfSubjects));

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<CandidateSaveModel> FetchRecordsToEdit(string search)
        {
            return await Task<CandidateSaveModel>.Run(() =>
            {
                try
                {
                    //var model = new CandidateSaveModel();
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    var sql = 
                       "SELECT * FROM BZPersonalInfo WHERE SerialNumber=@search";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@search", search));
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                return new CandidateSaveModel
                                {
                                   personalInfo =new PersonalInfoSaveModel
                                   {
                                       SchoolNo=dr["SchoolNo"].ToString(),
                                    serialNumber = dr["SerialNumber"].ToString(),
                                    surName = dr["surName"].ToString(),
                                    firstName = dr["firstName"].ToString(),
                                    middleName = dr["middleName"].ToString(),
                                    dateOfBirth = Convert.ToInt64(dr["dateOfBirth"]),
                                    Sex = dr["Sex"].ToString(),
                                    bPassport = ((byte[])dr["Passport"]),
                                    stateOfOriginCode = dr["stateOfOriginCode"].ToString(),
                                    stateOfOriginName = dr["stateOfOriginName"].ToString(),
                                    lgaCode = dr["lgaCode"].ToString(),
                                    lgaName = dr["lgaName"].ToString(),
                                    disabled = dr["disabled"].ToString(),
                                    status = Convert.ToInt32(dr["status"]),
                                    },

                                   subjects= new SubjectSaveModel
                                   {
                                       Subj1 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj1"].ToString().ToLower()),
                                       Code1= dr["code1"].ToString(),
                                       Subj1_CA1 = dr["Subj1_CA1"].ToString(),
                                       Subj1_CA2 = dr["Subj1_CA2"].ToString(),

                                       Subj2 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj2"].ToString().ToLower()),
                                       Code2 = dr["code2"].ToString(),
                                       Subj2_CA1 = dr["Subj2_CA1"].ToString(),
                                       Subj2_CA2 = dr["Subj2_CA2"].ToString(),

                                       Subj3 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj3"].ToString().ToLower()),
                                       Code3 = dr["code3"].ToString(),
                                       Subj3_CA1 = dr["Subj3_CA1"].ToString(),
                                       Subj3_CA2 = dr["Subj3_CA2"].ToString(),

                                       Subj4 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj4"].ToString().ToLower()),
                                       Code4 = dr["code4"].ToString(),
                                       Subj4_CA1 = dr["Subj4_CA1"].ToString(),
                                       Subj4_CA2 = dr["Subj4_CA2"].ToString(),

                                       Subj5 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj5"].ToString().ToLower()),
                                       Code5 = dr["code5"].ToString(),
                                       Subj5_CA1 = dr["Subj5_CA1"].ToString(),
                                       Subj5_CA2 = dr["Subj5_CA2"].ToString(),

                                       Subj6 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj6"].ToString().ToLower()),
                                       Code6 = dr["code6"].ToString(),
                                       Subj6_CA1 = dr["Subj6_CA1"].ToString(),
                                       Subj6_CA2 = dr["Subj6_CA2"].ToString(),

                                       Subj7 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj7"].ToString().ToLower()),
                                       Code7 = dr["code7"].ToString(),
                                       Subj7_CA1 = dr["Subj7_CA1"].ToString(),
                                       Subj7_CA2 = dr["Subj7_CA2"].ToString(),

                                       Subj8 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj8"].ToString().ToLower()),
                                       Code8 = dr["code8"].ToString(),
                                       Subj8_CA1 = dr["Subj8_CA1"].ToString(),
                                       Subj8_CA2 = dr["Subj8_CA2"].ToString(),

                                       Subj9 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj9"].ToString().ToLower()),
                                       Code9 = dr["code9"].ToString(),
                                       Subj9_CA1 = dr["Subj9_CA1"].ToString(),
                                       Subj9_CA2 = dr["Subj9_CA2"].ToString(),
                                       numberOfSubjects = Convert.ToInt32(dr["numberOfSubjects"])
                                   }
                                };
                               /* var PersonalInfo=new PersonalInfoSaveModel
                                {
                                    serialNumber = dr["SerialNumber"].ToString(),
                                    surName = dr["surName"].ToString(),
                                    firstName = dr["firstName"].ToString(),
                                    middleName = dr["middleName"].ToString(),
                                    dateOfBirth = Convert.ToInt64(dr["dateOfBirth"]),
                                    Sex = dr["Sex"].ToString(),
                                    bPassport = ((byte[])dr["Passport"]),
                                    stateOfOriginCode = dr["stateOfOriginCode"].ToString(),
                                    stateOfOriginName = dr["stateOfOriginName"].ToString(),
                                    lgaCode = dr["lgaCode"].ToString(),
                                    lgaName = dr["lgaName"].ToString(),
                                    disabled = dr["disabled"].ToString(),
                                    status = Convert.ToInt32(dr["status"]),

                                    //hasBiometrics = (Convert.ToBoolean(dr["hasBiometrics"])),
                                    
                                    //BiometricsImageSource = (Convert.ToBoolean(dr["hasBiometrics"])) ? "/SSCEOfflineRegSchApp;component/Images/finger_red.png" : "/SSCEOfflineRegSchApp;component/Images/finger_green.png",
                                    //BiometricsToolTip = (Convert.ToBoolean(dr["hasBiometrics"])) ? "Biometrics Captured" : "Click to Capture Biometrics",
                                    //listViewItemToolTip = "Doubleclick to View Candidate Profile",

                                    Subj1 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj1"].ToString().ToLower()),
                                    Subj1_CA1 = dr["Subj1_CA1"].ToString(),
                                    Subj1_CA2 = dr["Subj1_CA2"].ToString(),

                                    Subj2 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj2"].ToString().ToLower()),
                                    Subj2_CA1 = dr["Subj2_CA1"].ToString(),
                                    Subj2_CA2 = dr["Subj2_CA2"].ToString(),

                                    Subj3 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj3"].ToString().ToLower()),
                                    Subj3_CA1 = dr["Subj3_CA1"].ToString(),
                                    Subj3_CA2 = dr["Subj3_CA2"].ToString(),

                                    Subj4 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj4"].ToString().ToLower()),
                                    Subj4_CA1 = dr["Subj4_CA1"].ToString(),
                                    Subj4_CA2 = dr["Subj4_CA2"].ToString(),

                                    Subj5 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj5"].ToString().ToLower()),
                                    Subj5_CA1 = dr["Subj5_CA1"].ToString(),
                                    Subj5_CA2 = dr["Subj5_CA2"].ToString(),

                                    Subj6 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj6"].ToString().ToLower()),
                                    Subj6_CA1 = dr["Subj6_CA1"].ToString(),
                                    Subj6_CA2 = dr["Subj6_CA2"].ToString(),

                                    Subj7 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj7"].ToString().ToLower()),
                                    Subj7_CA1 = dr["Subj7_CA1"].ToString(),
                                    Subj7_CA2 = dr["Subj7_CA2"].ToString(),

                                    Subj8 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj8"].ToString().ToLower()),
                                    Subj8_CA1 = dr["Subj8_CA1"].ToString(),
                                    Subj8_CA2 = dr["Subj8_CA2"].ToString(),

                                    Subj9 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj9"].ToString().ToLower()),
                                    Subj9_CA1 = dr["Subj9_CA1"].ToString(),
                                    Subj9_CA2 = dr["Subj9_CA2"].ToString(),
                                    numberOfSubjects = Convert.ToInt32(dr["numberOfSubjects"])

                                };*/
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });
        }

        public async Task<List<CandidateViewModel>> FetchRecordsToPreview(string search)
        {
            return await Task<List<CandidateViewModel>>.Run(() =>
            {
                try
                {
                    var model = new List<CandidateViewModel>();
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    var sql = string.IsNullOrWhiteSpace(search) ? "SELECT * FROM BZPersonalInfo" :
                       "SELECT * FROM BZPersonalInfo WHERE SerialNumber=@search";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@search", search));
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if(dr.HasRows)
                        {
                            while(dr.Read())
                            {

                                model.Add(new CandidateViewModel
                                {
                                    serialNumber = dr["SerialNumber"].ToString(),
                                    surName = dr["surName"].ToString(),
                                    firstName = dr["firstName"].ToString(),
                                    middleName = dr["middleName"].ToString(),
                                    dateOfBirth = Convert.ToInt64(dr["dateOfBirth"]),
                                    Sex = dr["Sex"].ToString(),
                                    bPassport = ((byte[])dr["Passport"]),
                                    stateOfOriginCode = dr["stateOfOriginCode"].ToString(),
                                    stateOfOriginName = dr["stateOfOriginName"].ToString(),
                                    lgaCode = dr["lgaCode"].ToString(),
                                    lgaName = dr["lgaName"].ToString(),
                                    disabled = dr["disabled"].ToString(),

                                    hasBiometrics = (Convert.ToBoolean(dr["hasBiometrics"])),
                                    status = Convert.ToInt32(dr["status"]),
                                    BiometricsImageSource = (Convert.ToBoolean(dr["hasBiometrics"])) ? "/SSCEOfflineRegSchApp;component/Images/finger_red.png" : "/SSCEOfflineRegSchApp;component/Images/finger_green.png",
                                    BiometricsToolTip = (Convert.ToBoolean(dr["hasBiometrics"])) ? "Biometrics Captured" : "Click to Capture Biometrics",
                                    listViewItemToolTip = "Doubleclick to View Candidate Profile",

                                    Subj1 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj1"].ToString().ToLower()),
                                    Subj1_CA1 = dr["Subj1_CA1"].ToString(),
                                    Subj1_CA2 = dr["Subj1_CA2"].ToString(),

                                    Subj2 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj2"].ToString().ToLower()),
                                    Subj2_CA1 = dr["Subj2_CA1"].ToString(),
                                    Subj2_CA2 = dr["Subj2_CA2"].ToString(),

                                    Subj3 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj3"].ToString().ToLower()),
                                    Subj3_CA1 = dr["Subj3_CA1"].ToString(),
                                    Subj3_CA2 = dr["Subj3_CA2"].ToString(),

                                    Subj4 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj4"].ToString().ToLower()),
                                    Subj4_CA1 = dr["Subj4_CA1"].ToString(),
                                    Subj4_CA2 = dr["Subj4_CA2"].ToString(),

                                    Subj5 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj5"].ToString().ToLower()),
                                    Subj5_CA1 = dr["Subj5_CA1"].ToString(),
                                    Subj5_CA2 = dr["Subj5_CA2"].ToString(),

                                    Subj6 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj6"].ToString().ToLower()),
                                    Subj6_CA1 = dr["Subj6_CA1"].ToString(),
                                    Subj6_CA2 = dr["Subj6_CA2"].ToString(),

                                    Subj7 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj7"].ToString().ToLower()),
                                    Subj7_CA1 = dr["Subj7_CA1"].ToString(),
                                    Subj7_CA2 = dr["Subj7_CA2"].ToString(),

                                    Subj8 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj8"].ToString().ToLower()),
                                    Subj8_CA1 = dr["Subj8_CA1"].ToString(),
                                    Subj8_CA2 = dr["Subj8_CA2"].ToString(),

                                    Subj9 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj9"].ToString().ToLower()),
                                    Subj9_CA1 = dr["Subj9_CA1"].ToString(),
                                    Subj9_CA2 = dr["Subj9_CA2"].ToString(),
                                    numberOfSubjects=Convert.ToInt32(dr["numberOfSubjects"])

                                });
                            }
                        }
                    }
                    return model;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });
        }


        public async Task<List<CandidateViewModel>> FetchRecordsToPreview(string Search, int PageSize, int PageNum)
        {
            return await Task<List<CandidateViewModel>>.Run(() =>
            {
                try
                {
                    var model = new List<CandidateViewModel>();
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    var sql = string.IsNullOrWhiteSpace(Search) ? "SELECT * FROM BZPersonalInfo ORDER BY SerialNumber LIMIT @PageSize OFFSET @PageNum" :
                       "SELECT * FROM BZPersonalInfo WHERE SerialNumber LIKE @Search||'%' OR surName LIKE @Search||'%' OR " +
                    "firstName LIKE @Search||'%' OR  middleName LIKE @Search||'%' LIMIT @PageSize OFFSET @PageNum";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Search", Search));
                        cmd.Parameters.Add(new SQLiteParameter("@PageSize", PageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@PageNum", PageNum));
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                model.Add(new CandidateViewModel
                                {
                                    serialNumber = dr["SerialNumber"].ToString(),
                                    surName = dr["surName"].ToString(),
                                    firstName = dr["firstName"].ToString(),
                                    middleName = dr["middleName"].ToString(),
                                    dateOfBirth = Convert.ToInt64(dr["dateOfBirth"]),
                                    Sex = dr["Sex"].ToString(),
                                    bPassport = ((byte[])dr["Passport"]),
                                    stateOfOriginCode=dr["stateOfOriginCode"].ToString(),
                                    stateOfOriginName=dr["stateOfOriginName"].ToString(),
                                    lgaCode=dr["lgaCode"].ToString(),
                                    lgaName=dr["lgaName"].ToString(),
                                    disabled=dr["disabled"].ToString(),

                                    hasBiometrics= (Convert.ToBoolean(dr["hasBiometrics"])),
                                    status=Convert.ToInt32(dr["status"]),
                                    BiometricsImageSource = (Convert.ToBoolean(dr["hasBiometrics"])) ? "/SSCEOfflineRegSchApp;component/Images/finger_red.png" : "/SSCEOfflineRegSchApp;component/Images/finger_green.png",
                                    BiometricsToolTip = (Convert.ToBoolean(dr["hasBiometrics"])) ? "Biometrics Captured" : "Click to Capture Biometrics",
                                    listViewItemToolTip = "Doubleclick to View Candidate Profile",

                                    Subj1 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj1"].ToString().ToLower()),
                                    Subj1_CA1 = dr["Subj1_CA1"].ToString(),
                                    Subj1_CA2 = dr["Subj1_CA2"].ToString(),

                                    Subj2 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj2"].ToString().ToLower()),
                                    Subj2_CA1 = dr["Subj2_CA1"].ToString(),
                                    Subj2_CA2 = dr["Subj2_CA2"].ToString(),

                                    Subj3 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj3"].ToString().ToLower()),
                                    Subj3_CA1 = dr["Subj3_CA1"].ToString(),
                                    Subj3_CA2 = dr["Subj3_CA2"].ToString(),

                                    Subj4 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj4"].ToString().ToLower()),
                                    Subj4_CA1 = dr["Subj4_CA1"].ToString(),
                                    Subj4_CA2 = dr["Subj4_CA2"].ToString(),

                                    Subj5 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj5"].ToString().ToLower()),
                                    Subj5_CA1 = dr["Subj5_CA1"].ToString(),
                                    Subj5_CA2 = dr["Subj5_CA2"].ToString(),

                                    Subj6 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj6"].ToString().ToLower()),
                                    Subj6_CA1 = dr["Subj6_CA1"].ToString(),
                                    Subj6_CA2 = dr["Subj6_CA2"].ToString(),

                                    Subj7 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj7"].ToString().ToLower()),
                                    Subj7_CA1 = dr["Subj7_CA1"].ToString(),
                                    Subj7_CA2 = dr["Subj7_CA2"].ToString(),

                                    Subj8 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj8"].ToString().ToLower()),
                                    Subj8_CA1 = dr["Subj8_CA1"].ToString(),
                                    Subj8_CA2 = dr["Subj8_CA2"].ToString(),

                                    Subj9 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["subj9"].ToString().ToLower()),
                                    Subj9_CA1 = dr["Subj9_CA1"].ToString(),
                                    Subj9_CA2 = dr["Subj9_CA2"].ToString(),

                                    numberOfSubjects = Convert.ToInt32(dr["numberOfSubjects"])

                                });
                            }
                        }
                    }
                    return model;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });
        }


        public async Task<int> FetchRecordCount(bool Export)
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<int>.Run(() =>
            {
                try
                {
                    var hasBiometrics = true;
                    string strCommand =(Export)? "SELECT COUNT(SerialNumber) total FROM BZPersonalInfo WHERE status='0' AND hasBiometrics=@hasBiometrics": "SELECT COUNT(SerialNumber) total FROM BZPersonalInfo WHERE status='1'";
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@hasBiometrics", hasBiometrics));
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count;
                    }
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError("Count Records\n" + e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return 0;
            });

        }

        public async Task<int> FetchRecordCount()
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<int>.Run(() =>
            {
                try
                {
                    string strCommand = "SELECT COUNT(SerialNumber) total FROM BZPersonalInfo";
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count;
                    }
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError("Count Records\n" + e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return 0;
            });

        }
        #endregion

        #region Finger Prints Section

        public async Task<bool> SaveBiometrics(BiometricsSaveModel model)
        {
            return await Task<bool>.Run(() =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            cmd.CommandText =
                                      "UPDATE BZPersonalInfo SET "+
                                      "leftThumbImage=@leftThumbImage, leftThumbTemplate=@leftThumbTemplate, leftThumbQuality=@leftThumbQuality, "+
                                      "leftIndexImage=@leftIndexImage, leftIndexTemplate=@leftIndexTemplate, leftIndexQuality=@leftIndexQuality, "+
                                      "leftMiddleImage=@leftMiddleImage, leftMiddleTemplate=@leftMiddleTemplate, leftMiddleQuality=@leftMiddleQuality, "+
                                      "leftRingImage=@leftRingImage, leftRingTemplate=@leftRingTemplate, leftRingQuality=@leftRingQuality, "+
                                      "leftPinkyImage=@leftPinkyImage, leftPinkyTemplate=@leftPinkyTemplate, leftPinkyQuality=@leftPinkyQuality, "+
                                      "rightThumbImage=@rightThumbImage, rightThumbTemplate=@rightThumbTemplate, rightThumbQuality=@rightThumbQuality, "+
                                      "rightIndexImage=@rightIndexImage, rightIndexTemplate=@rightIndexTemplate, rightIndexQuality=@rightIndexQuality, "+
                                      "rightMiddleImage=@rightMiddleImage, rightMiddleTemplate=@rightMiddleTemplate, rightMiddleQuality=@rightMiddleQuality, "+
                                      "rightRingImage=@rightRingImage, rightRingTemplate=@rightRingTemplate, rightRingQuality=@rightRingQuality, "+
                                      "rightPinkyImage=@rightPinkyImage, rightPinkyTemplate=@rightPinkyTemplate, rightPinkyQuality=@rightPinkyQuality, "+
                                      "hasBiometrics=@hasBiometrics WHERE SerialNumber=@SerialNumber";

                            cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", model.serialNumber));

                            cmd.Parameters.Add(new SQLiteParameter("@leftThumbImage", model.leftThumbImage));
                            cmd.Parameters.Add(new SQLiteParameter("@leftThumbTemplate", model.leftThumbTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@leftThumbQuality", model.leftThumbQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@leftIndexImage", model.leftIndexImage));
                            cmd.Parameters.Add(new SQLiteParameter("@leftIndexTemplate", model.leftIndexTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@leftIndexQuality", model.leftIndexQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@leftMiddleImage", model.leftMiddleImage));
                            cmd.Parameters.Add(new SQLiteParameter("@leftMiddleTemplate", model.leftMiddleTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@leftMiddleQuality", model.leftMiddleQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@leftRingImage", model.leftRingImage));
                            cmd.Parameters.Add(new SQLiteParameter("@leftRingTemplate", model.leftRingTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@leftRingQuality", model.leftRingQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@leftPinkyImage", model.leftPinkyImage));
                            cmd.Parameters.Add(new SQLiteParameter("@leftPinkyTemplate", model.leftPinkyTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@leftPinkyQuality", model.leftPinkyQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@rightThumbImage", model.rightThumbImage));
                            cmd.Parameters.Add(new SQLiteParameter("@rightThumbTemplate", model.rightThumbTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@rightThumbQuality", model.rightThumbQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@rightIndexImage", model.rightIndexImage));
                            cmd.Parameters.Add(new SQLiteParameter("@rightIndexTemplate", model.rightIndexTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@rightIndexQuality", model.rightIndexQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@rightMiddleImage", model.rightMiddleImage));
                            cmd.Parameters.Add(new SQLiteParameter("@rightMiddleTemplate", model.rightMiddleTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@rightMiddleQuality", model.rightMiddleQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@rightRingImage", model.rightRingImage));
                            cmd.Parameters.Add(new SQLiteParameter("@rightRingTemplate", model.rightRingTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@rightRingQuality", model.rightRingQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@rightPinkyImage", model.rightPinkyImage));
                            cmd.Parameters.Add(new SQLiteParameter("@rightPinkyTemplate", model.rightPinkyTemplate));
                            cmd.Parameters.Add(new SQLiteParameter("@rightPinkyQuality", model.rightPinkyQuality));

                            cmd.Parameters.Add(new SQLiteParameter("@hasBiometrics", model.hasBiometrics));
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }


        public async Task<bool> SaveTemplate(TemplatesModel model)
        {
            return await Task<bool>.Run(async () =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    using (dbConection)
                    {
                        string strCommand = "INSERT INTO BZTemplates(SerialNumber, template, quality) VALUES (@SerialNumber, @template, @quality)";
                        SQLiteCommand oleCommand = new SQLiteCommand(strCommand, dbConection);
                        oleCommand.Parameters.Add(new SQLiteParameter("@SerialNumber", model.ser_no));
                        //oleCommand.Parameters.Add(new SQLiteParameter("@finger", model.finger));
                        oleCommand.Parameters.Add(new SQLiteParameter("@template", model.template));
                        oleCommand.Parameters.Add(new SQLiteParameter("@quality", model.quality));
                        oleCommand.ExecuteNonQuery();
                        return true;
                    }

                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError("Save Template\n" + e.Message);
                }
                await Task.Delay(3000);
                return false;
            });
        }


        public async Task<List<TemplatesModel>> FetchTemplates()
        {
            return await Task<List<TemplatesModel>>.Run(() =>
            {
                var templates = new List<TemplatesModel>();
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();

                    string strCommand = "SELECT  SerialNumber,template,quality FROM BZTemplates";
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                templates.Add(new TemplatesModel
                                {
                                    template = (byte[])dr["template"],
                                    quality = Convert.ToInt32(dr["quality"]),
                                    ser_no = dr["SerialNumber"].ToString()
                                });
                            }
                        }
                    }
                    return templates;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return null;
            });

        }

        public async Task<bool> ClearTemplates(string SerialNumber)
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();
            return await Task.Run(async () =>
            {
                try
                {
                    string strCommand = (string.IsNullOrWhiteSpace(SerialNumber)) ? "DELETE  FROM BZTemplates" :
                    "DELETE  FROM BZTemplates WHERE SerialNumber=@SerialNumber";
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@SerialNumber", SerialNumber));
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    SafeGuiWpf.ShowError(ex.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);
                return false;
            });
        }

        public async Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search)
        {

            //GZipClassLayer.CompressByteToGzipBase64

            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<Tuple<List<Registration>, List<Biometrics>>>.Run(async () =>
            {
                var Reg = new List<Registration>();
                var Tmp = new List<Biometrics>();
                var hasBiometrics = true;

                try
                {
                    string strCommand = string.IsNullOrWhiteSpace(Search) ? "SELECT a.serialNumber ser_no, a.disabled disabled, a.surName surname, a.firstName firstname, a.middleName middlename, " +
                                   "a.stateOfOriginCode s_of_o,a.lgaCode lgaid, a.dateOfBirth d_of_b, a.sex sex, a.passport passport, " +
                                   "a.code1 subj1, a.code2 subj2, a.code3 subj3, a.code4 subj4, a.code5 subj5, a.code6 subj6, a.code7 subj7, a.code8 subj8, a.code9 subj9, " +
                                   "a.subj1_CA1 subj1_CA1, a.subj2_CA1 subj2_CA1, a.subj3_CA1 subj3_CA1, a.subj4_CA1 subj4_CA1, a.subj5_CA1 subj5_CA1, a.subj6_CA1 subj6_CA1, a.subj7_CA1 subj7_CA1, a.subj8_CA1 subj8_CA1, a.subj9_CA1 subj9_CA1, " +
                                   "a.subj1_CA2 subj1_CA2, a.subj2_CA2 subj2_CA2, a.subj3_CA2 subj3_CA2, a.subj4_CA2 subj4_CA2, a.subj5_CA2 subj5_CA2, a.subj6_CA2 subj6_CA2, a.subj7_CA2 subj7_CA2, a.subj8_CA2 subj8_CA2, a.subj9_CA2 subj9_CA2," +
                                    "a.leftThumbImage leftThumbImage, a.leftThumbTemplate leftThumbTemplate, leftThumbQuality leftThumbQuality," +
                                   "a.leftIndexImage leftIndexImage, a.leftIndexTemplate leftIndexTemplate, a.leftIndexQuality leftIndexQuality, " +
                                   "a.leftMiddleImage leftMiddleImage, a.leftMiddleTemplate leftMiddleTemplate, a.leftMiddleQuality leftMiddleQuality, " +
                                   "a.leftRingImage leftRingImage, a.leftRingTemplate leftRingTemplate, a.leftRingQuality leftRingQuality, " +
                                   "a.leftPinkyImage leftPinkyImage, a.leftPinkyTemplate leftPinkyTemplate, a.leftPinkyQuality leftPinkyQuality, " +
                                   "a.rightThumbImage rightThumbImage, a.rightThumbTemplate rightThumbTemplate, a.rightThumbQuality rightThumbQuality, " +
                                   "a.rightIndexImage rightIndexImage, a.rightIndexTemplate rightIndexTemplate, a.rightIndexQuality rightIndexQuality, " +
                                   "a.rightMiddleImage rightMiddleImage, a.rightMiddleTemplate rightMiddleTemplate, a.rightMiddleQuality rightMiddleQuality, " +
                                   "a.rightRingImage rightRingImage, a.rightRingTemplate rightRingTemplate, a.rightRingQuality rightRingQuality, " +
                                   "a.rightPinkyImage rightPinkyImage, a.rightPinkyTemplate rightPinkyTemplate, a.rightPinkyQuality rightPinkyQuality, " +
                                   " a.status status FROM BZPersonalInfo a WHERE status='0' AND hasBiometrics= @hasBiometrics ORDER BY serialNumber" :
                                   "SELECT a.serialNumber ser_no, a.disabled disabled, a.surName surname, a.firstName firstname, a.middleName middlename, " +
                                   "a.stateOfOriginCode s_of_o,a.lgaCode lgaid, a.dateOfBirth d_of_b, a.sex sex, a.passport passport, " +
                                   "a.code1 subj1, a.code2 subj2, a.code3 subj3, a.code4 subj4, a.code5 subj5, a.code6 subj6, a.code7 subj7, a.code8 subj8, a.code9 subj9, " +
                                   "a.subj1_CA1 subj1_CA1, a.subj2_CA1 subj2_CA1, a.subj3_CA1 subj3_CA1, a.subj4_CA1 subj4_CA1, a.subj5_CA1 subj5_CA1, a.subj6_CA1 subj6_CA1, a.subj7_CA1 subj7_CA1, a.subj8_CA1 subj8_CA1, a.subj9_CA1 subj9_CA1, " +
                                   "a.subj1_CA2 subj1_CA2, a.subj2_CA2 subj2_CA2, a.subj3_CA2 subj3_CA2, a.subj4_CA2 subj4_CA2, a.subj5_CA2 subj5_CA2, a.subj6_CA2 subj6_CA2, a.subj7_CA2 subj7_CA2, a.subj8_CA2 subj8_CA2, a.subj9_CA2 subj9_CA2," +
                                    "a.leftThumbImage leftThumbImage, a.leftThumbTemplate leftThumbTemplate, leftThumbQuality leftThumbQuality," +
                                   "a.leftIndexImage leftIndexImage, a.leftIndexTemplate leftIndexTemplate, a.leftIndexQuality leftIndexQuality, " +
                                   "a.leftMiddleImage leftMiddleImage, a.leftMiddleTemplate leftMiddleTemplate, a.leftMiddleQuality leftMiddleQuality, " +
                                   "a.leftRingImage leftRingImage, a.leftRingTemplate leftRingTemplate, a.leftRingQuality leftRingQuality, " +
                                   "a.leftPinkyImage leftPinkyImage, a.leftPinkyTemplate leftPinkyTemplate, a.leftPinkyQuality leftPinkyQuality, " +
                                   "a.rightThumbImage rightThumbImage, a.rightThumbTemplate rightThumbTemplate, a.rightThumbQuality rightThumbQuality, " +
                                   "a.rightIndexImage rightIndexImage, a.rightIndexTemplate rightIndexTemplate, a.rightIndexQuality rightIndexQuality, " +
                                   "a.rightMiddleImage rightMiddleImage, a.rightMiddleTemplate rightMiddleTemplate, a.rightMiddleQuality rightMiddleQuality, " +
                                   "a.rightRingImage rightRingImage, a.rightRingTemplate rightRingTemplate, a.rightRingQuality rightRingQuality, " +
                                   "a.rightPinkyImage rightPinkyImage, a.rightPinkyTemplate rightPinkyTemplate, a.rightPinkyQuality rightPinkyQuality, " +
                                   " a.status status FROM BZPersonalInfo a WHERE status='0' AND hasBiometrics= @hasBiometrics AND serialNumber=@Search ORDER BY serialNumber";


                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Search", Search));
                        cmd.Parameters.Add(new SQLiteParameter("@hasBiometrics", hasBiometrics));
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Reg.Add(new Registration
                                {
                                    ser_no = dr["ser_no"].ToString(),
                                    disabled = dr["disabled"].ToString(),
                                    surname = dr["surname"].ToString().Trim(),
                                    firstname = dr["firstname"].ToString().Trim(),
                                    middlename = dr["middlename"].ToString().Trim(),
                                    s_of_o = dr["s_of_o"].ToString(),
                                    lgaid = Convert.ToInt32(dr["lgaid"]),
                                    d_of_b = DateTimeToLong.ConvertToDateTime((long)dr["d_of_b"]).ToShortDateString(),
                                    sex = dr["sex"].ToString(),
                                    passport = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["passport"]),
                                    subj1 = dr["subj1"].ToString(),
                                    subj2 = dr["subj2"].ToString(),
                                    subj3 = dr["subj3"].ToString(),
                                    subj4 = dr["subj4"].ToString(),
                                    subj5 = dr["subj5"].ToString(),
                                    subj6 = dr["subj6"].ToString(),
                                    subj7 = dr["subj7"].ToString(),
                                    subj8 = dr["subj8"].ToString(),
                                    subj9 = dr["subj9"].ToString(),

                                    subj1_CA1 = dr["subj1_CA1"].ToString(),
                                    subj2_CA1 = dr["subj2_CA1"].ToString(),
                                    subj3_CA1 = dr["subj3_CA1"].ToString(),
                                    subj4_CA1 = dr["subj4_CA1"].ToString(),
                                    subj5_CA1 = dr["subj5_CA1"].ToString(),
                                    subj6_CA1 = dr["subj6_CA1"].ToString(),
                                    subj7_CA1 = dr["subj7_CA1"].ToString(),
                                    subj8_CA1 = dr["subj8_CA1"].ToString(),
                                    subj9_CA1 = dr["subj9_CA1"].ToString(),

                                    subj1_CA2 = dr["subj1_CA2"].ToString(),
                                    subj2_CA2 = dr["subj2_CA2"].ToString(),
                                    subj3_CA2 = dr["subj3_CA2"].ToString(),
                                    subj4_CA2 = dr["subj4_CA2"].ToString(),
                                    subj5_CA2 = dr["subj5_CA2"].ToString(),
                                    subj6_CA2 = dr["subj6_CA2"].ToString(),
                                    subj7_CA2 = dr["subj7_CA2"].ToString(),
                                    subj8_CA2 = dr["subj8_CA2"].ToString(),
                                    subj9_CA2 = dr["subj9_CA2"].ToString()
                                });

                                Tmp.Add(new Biometrics
                                {
                                    ser_no = dr["ser_no"].ToString(),

                                    leftThumbImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftThumbImage"]),
                                    leftThumbTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftThumbTemplate"]),
                                    leftThumbQuality = Convert.ToInt32(dr["leftThumbQuality"]),

                                    leftIndexImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftIndexImage"]),
                                    leftIndexTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftIndexTemplate"]),
                                    leftIndexQuality = Convert.ToInt32(dr["leftIndexQuality"]),

                                    leftMiddleImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftMiddleImage"]),
                                    leftMiddleTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftMiddleTemplate"]),
                                    leftMiddleQuality = Convert.ToInt32(dr["leftMiddleQuality"]),

                                    leftRingImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftRingImage"]),
                                    leftRingTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftRingTemplate"]),
                                    leftRingQuality = Convert.ToInt32(dr["leftRingQuality"]),

                                    leftPinkyImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftPinkyImage"]),
                                    leftPinkyTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftPinkyTemplate"]),
                                    leftPinkyQuality = Convert.ToInt32(dr["leftPinkyQuality"]),

                                    rightThumbImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightThumbImage"]),
                                    rightThumbTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightThumbTemplate"]),
                                    rightThumbQuality = Convert.ToInt32(dr["rightThumbQuality"]),

                                    rightIndexImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightIndexImage"]),
                                    rightIndexTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightIndexTemplate"]),
                                    rightIndexQuality = Convert.ToInt32(dr["rightIndexQuality"]),

                                    rightMiddleImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightMiddleImage"]),
                                    rightMiddleTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightMiddleTemplate"]),
                                    rightMiddleQuality = Convert.ToInt32(dr["rightMiddleQuality"]),

                                    rightRingImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightRingImage"]),
                                    rightRingTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightRingTemplate"]),
                                    rightRingQuality = Convert.ToInt32(dr["rightRingQuality"]),

                                    rightPinkyImage = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightPinkyImage"]),
                                    rightPinkyTemplate = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightPinkyTemplate"]),
                                    rightPinkyQuality = Convert.ToInt32(dr["rightPinkyQuality"])
                                });
                            }
                        }
                    }
                    return new Tuple<List<Registration>, List<Biometrics>>(Reg, Tmp);

                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);
                return null;
            });

        }

        public async Task<List<SerialNoModel>> FetchSerailNo()
        {
            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<List<SerialNoModel>>.Run(async () =>
            {
                var Reg = new List<SerialNoModel>();
                
                try
                {
                    string strCommand =  "SELECT serialNumber FROM BZPersonalInfo";
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                       
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Reg.Add(new SerialNoModel
                                {
                                   SerialNo = dr["serialNumber"].ToString(),
                                   
                                });
                            }
                        }
                    }
                    return Reg;

                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);
                return null;
            });

        }

        public async Task<Tuple<List<Registration>, List<Biometrics>>> FetchRecordsToExport(string Search, int PageSize, int PageNum)
        {

            //GZipClassLayer.CompressByteToGzipBase64

            if (dbConection.State == ConnectionState.Closed)
                dbConection.Open();

            return await Task<Tuple<List<Registration>, List<Biometrics>>>.Run(async() => 
            {
                var Reg = new List<Registration>();
                var Tmp = new List<Biometrics>();
                var hasBiometrics = true;
                try
                {
                    string strCommand =string.IsNullOrWhiteSpace(Search) ? "SELECT a.serialNumber ser_no, a.disabled disabled, a.surName surname, a.firstName firstname, a.middleName middlename, " +
                                   "a.stateOfOriginCode s_of_o,a.lgaCode lgaid, a.dateOfBirth d_of_b, a.sex sex, a.passport passport, " +
                                   "a.code1 subj1, a.code2 subj2, a.code3 subj3, a.code4 subj4, a.code5 subj5, a.code6 subj6, a.code7 subj7, a.code8 subj8, a.code9 subj9, " +
                                   "a.subj1_CA1 subj1_CA1, a.subj2_CA1 subj2_CA1, a.subj3_CA1 subj3_CA1, a.subj4_CA1 subj4_CA1, a.subj5_CA1 subj5_CA1, a.subj6_CA1 subj6_CA1, a.subj7_CA1 subj7_CA1, a.subj8_CA1 subj8_CA1, a.subj9_CA1 subj9_CA1, " +
                                   "a.subj1_CA2 subj1_CA2, a.subj2_CA2 subj2_CA2, a.subj3_CA2 subj3_CA2, a.subj4_CA2 subj4_CA2, a.subj5_CA2 subj5_CA2, a.subj6_CA2 subj6_CA2, a.subj7_CA2 subj7_CA2, a.subj8_CA2 subj8_CA2, a.subj9_CA2 subj9_CA2," +
                                    "a.leftThumbImage leftThumbImage, a.leftThumbTemplate leftThumbTemplate, leftThumbQuality leftThumbQuality," +
                                   "a.leftIndexImage leftIndexImage, a.leftIndexTemplate leftIndexTemplate, a.leftIndexQuality leftIndexQuality, " +
                                   "a.leftMiddleImage leftMiddleImage, a.leftMiddleTemplate leftMiddleTemplate, a.leftMiddleQuality leftMiddleQuality, " +
                                   "a.leftRingImage leftRingImage, a.leftRingTemplate leftRingTemplate, a.leftRingQuality leftRingQuality, " +
                                   "a.leftPinkyImage leftPinkyImage, a.leftPinkyTemplate leftPinkyTemplate, a.leftPinkyQuality leftPinkyQuality, " +
                                   "a.rightThumbImage rightThumbImage, a.rightThumbTemplate rightThumbTemplate, a.rightThumbQuality rightThumbQuality, " +
                                   "a.rightIndexImage rightIndexImage, a.rightIndexTemplate rightIndexTemplate, a.rightIndexQuality rightIndexQuality, " +
                                   "a.rightMiddleImage rightMiddleImage, a.rightMiddleTemplate rightMiddleTemplate, a.rightMiddleQuality rightMiddleQuality, " +
                                   "a.rightRingImage rightRingImage, a.rightRingTemplate rightRingTemplate, a.rightRingQuality rightRingQuality, " +
                                   "a.rightPinkyImage rightPinkyImage, a.rightPinkyTemplate rightPinkyTemplate, a.rightPinkyQuality rightPinkyQuality, " +
                                   " a.status status FROM BZPersonalInfo a WHERE status='0' AND hasBiometrics= @hasBiometrics ORDER BY serialNumber LIMIT @PageSize OFFSET @PageNum" :
                                   "SELECT a.serialNumber ser_no, a.disabled disabled, a.surName surname, a.firstName firstname, a.middleName middlename, " +
                                   "a.stateOfOriginCode s_of_o,a.lgaCode lgaid, a.dateOfBirth d_of_b, a.sex sex, a.passport passport, " +
                                   "a.code1 subj1, a.code2 subj2, a.code3 subj3, a.code4 subj4, a.code5 subj5, a.code6 subj6, a.code7 subj7, a.code8 subj8, a.code9 subj9, " +
                                   "a.subj1_CA1 subj1_CA1, a.subj2_CA1 subj2_CA1, a.subj3_CA1 subj3_CA1, a.subj4_CA1 subj4_CA1, a.subj5_CA1 subj5_CA1, a.subj6_CA1 subj6_CA1, a.subj7_CA1 subj7_CA1, a.subj8_CA1 subj8_CA1, a.subj9_CA1 subj9_CA1, " +
                                   "a.subj1_CA2 subj1_CA2, a.subj2_CA2 subj2_CA2, a.subj3_CA2 subj3_CA2, a.subj4_CA2 subj4_CA2, a.subj5_CA2 subj5_CA2, a.subj6_CA2 subj6_CA2, a.subj7_CA2 subj7_CA2, a.subj8_CA2 subj8_CA2, a.subj9_CA2 subj9_CA2," +
                                   "a.leftThumbImage leftThumbImage, a.leftThumbTemplate leftThumbTemplate, leftThumbQuality leftThumbQuality," +
                                   "a.leftIndexImage leftIndexImage, a.leftIndexTemplate leftIndexTemplate, a.leftIndexQuality leftIndexQuality, " +
                                   "a.leftMiddleImage leftMiddleImage, a.leftMiddleTemplate leftMiddleTemplate, a.leftMiddleQuality leftMiddleQuality, " +
                                   "a.leftRingImage leftRingImage, a.leftRingTemplate leftRingTemplate, a.leftRingQuality leftRingQuality, " +
                                   "a.leftPinkyImage leftPinkyImage, a.leftPinkyTemplate leftPinkyTemplate, a.leftPinkyQuality leftPinkyQuality, " +
                                   "a.rightThumbImage rightThumbImage, a.rightThumbTemplate rightThumbTemplate, a.rightThumbQuality rightThumbQuality, " +
                                   "a.rightIndexImage rightIndexImage, a.rightIndexTemplate rightIndexTemplate, a.rightIndexQuality rightIndexQuality, " +
                                   "a.rightMiddleImage rightMiddleImage, a.rightMiddleTemplate rightMiddleTemplate, a.rightMiddleQuality rightMiddleQuality, " +
                                   "a.rightRingImage rightRingImage, a.rightRingTemplate rightRingTemplate, a.rightRingQuality rightRingQuality, " +
                                   "a.rightPinkyImage rightPinkyImage, a.rightPinkyTemplate rightPinkyTemplate, a.rightPinkyQuality rightPinkyQuality, " +
                                   " a.status status FROM BZPersonalInfo a WHERE status='0' AND hasBiometrics= @hasBiometrics AND " +
                                   "ser_no LIKE @Search||'%' OR surname LIKE @Search||'%' OR firstname LIKE@Search||'%'" +
                                   " ORDER BY ser_no LIMIT @PageSize OFFSET @PageNum";



                   
                    using (SQLiteCommand cmd = new SQLiteCommand(strCommand, dbConection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Search", Search));
                        cmd.Parameters.Add(new SQLiteParameter("@PageSize", PageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@PageNum", PageNum));
                        cmd.Parameters.Add(new SQLiteParameter("@hasBiometrics", hasBiometrics));
                        SQLiteDataReader dr= cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Reg.Add(new Registration
                                {
                                    ser_no=dr["ser_no"].ToString(),
                                    disabled=dr["disabled"].ToString(),
                                    surname=dr["surname"].ToString().Trim(),
                                    firstname=dr["firstname"].ToString().Trim(),
                                    middlename=dr["middlename"].ToString().Trim(),
                                    s_of_o=dr["s_of_o"].ToString(),
                                    lgaid=Convert.ToInt32(dr["lgaid"]),
                                    d_of_b= DateTimeToLong.ConvertToDateTime((long)dr["d_of_b"]).ToShortDateString(),
                                    sex = dr["sex"].ToString(),
                                    passport = GZipClassLayer.CompressByteToGzipBase64((byte[])dr["passport"]),
                                    subj1=dr["subj1"].ToString(),
                                    subj2 = dr["subj2"].ToString(),
                                    subj3 = dr["subj3"].ToString(),
                                    subj4 = dr["subj4"].ToString(),
                                    subj5 = dr["subj5"].ToString(),
                                    subj6 = dr["subj6"].ToString(),
                                    subj7 = dr["subj7"].ToString(),
                                    subj8 = dr["subj8"].ToString(),
                                    subj9 = dr["subj9"].ToString(),

                                    subj1_CA1=dr["subj1_CA1"].ToString(),
                                    subj2_CA1 = dr["subj2_CA1"].ToString(),
                                    subj3_CA1 = dr["subj3_CA1"].ToString(),
                                    subj4_CA1 = dr["subj4_CA1"].ToString(),
                                    subj5_CA1 = dr["subj5_CA1"].ToString(),
                                    subj6_CA1 = dr["subj6_CA1"].ToString(),
                                    subj7_CA1 = dr["subj7_CA1"].ToString(),
                                    subj8_CA1 = dr["subj8_CA1"].ToString(),
                                    subj9_CA1 = dr["subj9_CA1"].ToString(),

                                    subj1_CA2=dr["subj1_CA2"].ToString(),
                                    subj2_CA2 = dr["subj2_CA2"].ToString(),
                                    subj3_CA2 = dr["subj3_CA2"].ToString(),
                                    subj4_CA2 = dr["subj4_CA2"].ToString(),
                                    subj5_CA2 = dr["subj5_CA2"].ToString(),
                                    subj6_CA2 = dr["subj6_CA2"].ToString(),
                                    subj7_CA2 = dr["subj7_CA2"].ToString(),
                                    subj8_CA2 = dr["subj8_CA2"].ToString(),
                                    subj9_CA2 = dr["subj9_CA2"].ToString()
                                });

                                Tmp.Add(new Biometrics
                                {
                                    ser_no = dr["ser_no"].ToString(),

                                    leftThumbImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftThumbImage"]),
                                    leftThumbTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftThumbTemplate"]),
                                    leftThumbQuality=Convert.ToInt32(dr["leftThumbQuality"]),

                                    leftIndexImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftIndexImage"]),
                                    leftIndexTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftIndexTemplate"]),
                                    leftIndexQuality=Convert.ToInt32(dr["leftIndexQuality"]),

                                    leftMiddleImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftMiddleImage"]),
                                    leftMiddleTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftMiddleTemplate"]),
                                    leftMiddleQuality=Convert.ToInt32(dr["leftMiddleQuality"]),

                                    leftRingImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftRingImage"]),
                                    leftRingTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftRingTemplate"]),
                                    leftRingQuality=Convert.ToInt32(dr["leftRingQuality"]),

                                    leftPinkyImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftPinkyImage"]),
                                    leftPinkyTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["leftPinkyTemplate"]),
                                    leftPinkyQuality=Convert.ToInt32(dr["leftPinkyQuality"]),

                                    rightThumbImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightThumbImage"]),
                                    rightThumbTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightThumbTemplate"]),
                                    rightThumbQuality=Convert.ToInt32(dr["rightThumbQuality"]),

                                    rightIndexImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightIndexImage"]),
                                    rightIndexTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightIndexTemplate"]),
                                    rightIndexQuality=Convert.ToInt32(dr["rightIndexQuality"]),

                                    rightMiddleImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightMiddleImage"]),
                                    rightMiddleTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightMiddleTemplate"]),
                                    rightMiddleQuality=Convert.ToInt32(dr["rightMiddleQuality"]),

                                    rightRingImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightRingImage"]),
                                    rightRingTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightRingTemplate"]),
                                    rightRingQuality=Convert.ToInt32(dr["rightRingQuality"]),

                                    rightPinkyImage= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightPinkyImage"]),
                                    rightPinkyTemplate= GZipClassLayer.CompressByteToGzipBase64((byte[])dr["rightPinkyTemplate"]),
                                    rightPinkyQuality=Convert.ToInt32(dr["rightPinkyQuality"])
                                });
                            }
                        }
                    }
                    return new Tuple<List<Registration>, List<Biometrics>>(Reg, Tmp);

                }
                catch (Exception e)
                {

                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);
                return null;
            });
        }


        public async Task<bool> DeleteRecord(string SerialNumber)
        {
            return await Task.Run(async() =>
            {
                if (dbConection.State == ConnectionState.Closed)
                    dbConection.Open();
                try
                {
                    string strCommand = "DELETE FROM BZPersonalInfo WHERE SerialNumber=@SerialNumber AND status='0' ";
                    SQLiteCommand oleCommand = new SQLiteCommand(strCommand, dbConection);
                    oleCommand.Parameters.Add(new SQLiteParameter("@SerialNumber", SerialNumber));
                    oleCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);

                return false;
            });
        }

        public async Task<bool> UpdateSerialNo(List<SerialNoModel> model)
        {
            return await Task.Run(async() =>
            {
                if (dbConection.State == ConnectionState.Closed)
                    dbConection.Open();

                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            int i = 0;
                            foreach(var m in model)
                            {
                                int NewNo = ++i;
                                string serialNo = NewNo.ToString("D4");
                                cmd.CommandText =
                                   "UPDATE BZPersonalInfo SET SerialNoTemp=@serialNo WHERE SerialNumber=@OldserialNo";

                                cmd.Parameters.Add(new SQLiteParameter("@serialNo", serialNo));
                                cmd.Parameters.Add(new SQLiteParameter("@OldserialNo", m.SerialNo));
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                    }
                    /*INSERT INTO new_db.TABLE(col1, col2) SELECT col1, col2 FROM old_db.TABLE;*/
                    //INSERT INTO new_db.TABLE SELECT * FROM old_db.TABLE;

                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);

                return false;
            });
        }

        public async Task<bool> BackupRecords()
        {
            return await Task.Run(async () =>
            {
                TrucateTable("BZTemp");
                if (dbConection.State == ConnectionState.Closed)
                    dbConection.Open();
                try
                {
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            cmd.CommandText =
                                    "INSERT INTO BZTemp SELECT * FROM BZPersonalInfo ";

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                            /*INSERT INTO new_db.TABLE(col1, col2) SELECT col1, col2 FROM old_db.TABLE;*/
                            //INSERT INTO new_db.TABLE SELECT * FROM old_db.TABLE;
                            
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                await Task.Delay(3000);

                return false;
            });
            
        }


        public async Task<bool> SaveNewSerialNo()
        {
            TrucateTable("BZPersonalInfo");
            return await Task.Run(() =>
            {
                try
                {
                    if (dbConection.State == ConnectionState.Closed)
                        dbConection.Open();
                    using (var cmd = new SQLiteCommand(dbConection))
                    {
                        using (var transaction = dbConection.BeginTransaction())
                        {
                            cmd.CommandText =
                                      "INSERT INTO BZPersonalInfo(schoolNo, SerialNumber, SerialNoTemp, " +
                                       "disabled, surName, firstName, middleName," +
                                       "Sex, dateOfBirth, stateOfOriginCode, stateOfOriginName, lgaCode, lgaName, Passport," +
                                       "subj1, subj2, subj3, subj4, subj5, subj6, subj7, subj8, subj9," +
                                       "code1, code2, code3, code4, code5, code6, code7, code8, code9, " +
                                       "subj1_CA1, subj2_CA1, subj3_CA1, subj4_CA1, subj5_CA1, subj6_CA1, subj7_CA1, subj8_CA1, subj9_CA1," +
                                       "subj1_CA2, subj2_CA2, subj3_CA2, subj4_CA2, subj5_CA2, subj6_CA2, subj7_CA2, subj8_CA2, subj9_CA2," +
                                       "status, hasBiometrics, numberOfSubjects, isComplete, " +
                                        "leftThumbImage, leftThumbTemplate, leftThumbQuality, " +
                                      "leftIndexImage, leftIndexTemplate, leftIndexQuality, " +
                                      "leftMiddleImage, leftMiddleTemplate, leftMiddleQuality, " +
                                      "leftRingImage, leftRingTemplate, leftRingQuality, " +
                                      "leftPinkyImage, leftPinkyTemplate, leftPinkyQuality, " +
                                      "rightThumbImage, rightThumbTemplate, rightThumbQuality, " +
                                      "rightIndexImage, rightIndexTemplate, rightIndexQuality, " +
                                      "rightMiddleImage, rightMiddleTemplate, rightMiddleQuality, " +
                                      "rightRingImage, rightRingTemplate, rightRingQuality, " +
                                      "rightPinkyImage, rightPinkyTemplate, rightPinkyQuality) " +

                                      "SELECT schoolNo, SerialNoTemp, SerialNumber," +
                                       "disabled, surName, firstName, middleName," +
                                       "Sex, dateOfBirth, stateOfOriginCode, stateOfOriginName, lgaCode, lgaName, Passport," +
                                       "subj1, subj2, subj3, subj4, subj5, subj6, subj7, subj8, subj9," +
                                       "code1, code2, code3, code4, code5, code6, code7, code8, code9, " +
                                       "subj1_CA1, subj2_CA1, subj3_CA1, subj4_CA1, subj5_CA1, subj6_CA1, subj7_CA1, subj8_CA1, subj9_CA1," +
                                       "subj1_CA2, subj2_CA2, subj3_CA2, subj4_CA2, subj5_CA2, subj6_CA2, subj7_CA2, subj8_CA2, subj9_CA2," +
                                       "status, hasBiometrics, numberOfSubjects, isComplete, " +
                                        "leftThumbImage, leftThumbTemplate, leftThumbQuality, " +
                                      "leftIndexImage, leftIndexTemplate, leftIndexQuality, " +
                                      "leftMiddleImage, leftMiddleTemplate, leftMiddleQuality, " +
                                      "leftRingImage, leftRingTemplate, leftRingQuality, " +
                                      "leftPinkyImage, leftPinkyTemplate, leftPinkyQuality, " +
                                      "rightThumbImage, rightThumbTemplate, rightThumbQuality, " +
                                      "rightIndexImage, rightIndexTemplate, rightIndexQuality, " +
                                      "rightMiddleImage, rightMiddleTemplate, rightMiddleQuality, " +
                                      "rightRingImage, rightRingTemplate, rightRingQuality, " +
                                      "rightPinkyImage, rightPinkyTemplate, rightPinkyQuality FROM BZTemp";

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    SafeGuiWpf.ShowError(e.Message);
                    Task.Delay(3000);
                }
                finally
                {
                    dbConection.Close();
                    SQLiteConnection.ClearAllPools();
                }
                return false;
            });
        }
        #endregion
    }
}
