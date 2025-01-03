using SSCEOfflineRegSchApp.DB.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.DB
{
    public abstract class SQLiteDalFactory
    {
        public static IGRDal GetDal(GrConnector connector)
        {
            switch (connector)
            {
                case GrConnector.AccessSQLiteDal:
                    return (IGRDal)Activator.CreateInstance(typeof(SQLiteDataAccessLayer), true);

                default:
                    return null;
            }
        }


    }

    public enum GrConnector
    {
        AccessSQLiteDal
    }
}
