using MonitorAPI.Dao.framework;
using MonitorAPI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MonitorAPI.Dao
{
    public class ClassroomGroupDao:BaseDao
    {
        private const string QUERY_ALL_CLASSROOMGROUP_SQL = "SELECT * FROM CLASSROOMGROUP WHERE STATUS=1";
        private const string SQL_GETCLASSROOMGROUPBYCLASSROOMID = "SELECT CLASSROOMGROUP.CLASSROOMGROUPID, CLASSROOMGROUPNAME, FILESERVERID, CLASSROOMGROUP.STATUS, CLASSROOMGROUP.IsNoIPCGroup FROM CLASSROOMGROUP,CLASSROOM WHERE CLASSROOMGROUP.CLASSROOMGROUPID=CLASSROOM.CLASSROOMGROUPID AND CLASSROOM.CLASSROOMID=@CLASSROOMID";
        private const string QUERY_ALL_CLASSROOMGROUP_ByRoleID_SQL = "SELECT * FROM CLASSROOMGROUP where Status=1 and ClassroomGroupID in (select ClassroomGroupID from RoleClassroomGroup where RoleID=@RoleID) order by ClassroomgroupName";
        public ClassroomGroupDao(PersistenceContext pc) : base(pc) { }

        public List<ClassroomGroup> GetClassroomGroups()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_ALL_CLASSROOMGROUP_SQL;
                List<ClassroomGroup> list = SqlHelper.ExecuteReaderCmdList<ClassroomGroup>(command);
                return list;
            }
        }

        public ClassroomGroup GetClassroomGroupbyClassroomID(int ClassroomID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = SQL_GETCLASSROOMGROUPBYCLASSROOMID;
                command.Parameters.AddWithValue("@CLASSROOMID", ClassroomID);
                ClassroomGroup classroomGroup = SqlHelper.ExecuteReaderCmdObject<ClassroomGroup>(command);
                return classroomGroup;
            }
        }

        internal List<ClassroomGroup> GetClassroomGroupListByRoleID(int roleID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_ALL_CLASSROOMGROUP_ByRoleID_SQL;
                command.Parameters.AddWithValue("@RoleID", roleID);
                List<ClassroomGroup> list = SqlHelper.ExecuteReaderCmdList<ClassroomGroup>(command);
                return list;
            }
        }
    }
}
