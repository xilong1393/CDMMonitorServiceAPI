using MonitorAPI.Dao.framework;
using MonitorAPI.Models.OperationModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MonitorAPI.Dao
{
    public class TestCourseDao : BaseDao
    {
        public TestCourseDao(PersistenceContext persistenceContext) : base(persistenceContext)
        {
        }
        private const string SQL_GETALLQUARTER = "SELECT QUARTERID, YEAR, QUARTER, BEGINDATE, PS_QUARTERID, YEARTERM, TERMCODE, NAME, ENDDATE,CONVERT(CHAR(4),[YEAR])+' '+ [NAME] AS QUARTERDESC FROM QUARTER ORDER BY QUARTERID";
        private const string SQL_GETALLCLASSTYPE = "SELECT CLASSTYPEID, CLASSTYPENAME FROM CLASSTYPE ORDER BY CLASSTYPENAME DESC";
        private const string SQL_GETALLFILESERVER = "SELECT FILESERVERID, FILESERVERNAME FROM FILESERVER where Deleted=0";
        private const string SQL_GETQUARTERBYDATE = "SELECT QUARTERID, YEAR, QUARTER, BEGINDATE, PS_QUARTERID, YEARTERM, TERMCODE, NAME, ENDDATE,CONVERT(CHAR(4),[YEAR])+' '+ [NAME] AS QUARTERDESC FROM QUARTER WHERE BEGINDATE <= @QUERYDATE AND ENDDATE >= @QUERYDATE";

        public List<Quater> GetAllQuarter()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = SQL_GETALLQUARTER;
                List<Quater> result = SqlHelper.ExecuteReaderCmdList<Quater>(command);
                return result;
            }
        }

        public Quater GetCurrentQuarter(DateTime QueryDate)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@QueryDate", QueryDate);
                command.CommandText = SQL_GETQUARTERBYDATE;
                Quater result = SqlHelper.ExecuteReaderCmdObject<Quater>(command);
                return result;
            }
        }


        public List<ClassType> GetAllClassType()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = SQL_GETALLCLASSTYPE;
                List<ClassType> result = SqlHelper.ExecuteReaderCmdList<ClassType>(command);
                return result;
            }
        }

        public List<FileServer> GetAllFileServer()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = SQL_GETALLFILESERVER;
                List<FileServer> result = SqlHelper.ExecuteReaderCmdList<FileServer>(command);
                return result;
            }
        }
    }
}