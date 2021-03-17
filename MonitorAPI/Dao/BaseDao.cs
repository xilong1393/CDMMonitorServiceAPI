using MonitorAPI.Dao.framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MonitorAPI.Dao
{
    public class BaseDao
    {
        public PersistenceContext persistenceContext;

        protected SqlConnection Connection
        {
            get
            {
                return persistenceContext.Connection;
            }
        }
        protected BaseDao(PersistenceContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
        }

        protected void SetCommandContent(SqlCommand SQL, CommandType cmdType, string cmdText)
        {
            SQL.CommandType = cmdType;
            SQL.CommandText = cmdText;
        }

        protected void AddParamToSQLCmd(SqlCommand SQL, string paramID, SqlDbType sqlType, int paramSize,
            ParameterDirection paramDirection, object paramvalue)
        {

            if (SQL == null)
                throw (new Exception("Invalid SqlCommand."));
            if (paramID == string.Empty)
                throw (new Exception("Invalid ParamID."));

            SqlParameter newSqlParam = new SqlParameter();
            newSqlParam.ParameterName = paramID;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Direction = paramDirection;

            if (paramSize > 0)
                newSqlParam.Size = paramSize;

            newSqlParam.Value = (paramvalue != null ? paramvalue : System.DBNull.Value);

            SQL.Parameters.Add(newSqlParam);
        }
    }
}
