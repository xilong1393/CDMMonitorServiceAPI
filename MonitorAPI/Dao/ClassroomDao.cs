using MonitorAPI.Dao.framework;
using MonitorAPI.Model;
using MonitorAPI.Models;
using MonitorAPI.Models.OperationModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MonitorAPI.Dao
{
    public class ClassroomDao:BaseDao
    {
        public ClassroomDao(PersistenceContext pc) : base(pc) { }

        //private const string QUERY_CLASSROOMBYGROUPID_SQL = "SELECT * FROM CLASSROOM WHERE CLASSROOMGROUPID=@GROUPID";
        private const string QUERY_CLASSROOMBYID_SQL = "SELECT * FROM CLASSROOM WHERE ClassroomID=@ClassroomID";

        //private const string QUERY_CLASSROOMBYGROUPID_SQL =
        //    "SELECT a.ClassroomID, b.ppcreporttime, b.avcaputureframes, c.ipcreporttime, " +
        //    "c.agentrecorddata, a.PPCPublicIP, a.PPCPort, a.ClassroomName, b.EngineStatus, " +
        //    "c.AgentStatus, b.CourseName, a.WBNUMBER, b.WBStatus, b.WBNumber as ClassRoomWBNumber, " +
        //    "a.KaptivoNumber, a.Status, a.NoIPC, b.PPCConnectionStatus, " +
        //    "b.AVStatus, b.FreeDisk, d.ClassroomTypeName FROM CLASSROOM a " +
        //    "left join ClassroomEngineStatus b on a.ClassroomID=b.ClassroomID " +
        //    "left join CLassroomAgentStatus c on b.ClassroomID=c.ClassroomId " +
        //    "left join ClassroomType d on a.ClassroomTypeID=d.ClassroomTypeId " +
        //    "WHERE CLASSROOMGROUPID=@GROUPID ORDER BY CLASSROOMNAME";

        // get the all the classrooms that is currently recording
        private const string QUERY_ISRECORDINGCLASSROOM = "(SELECT distinct ClassroomID from ClassRecording cr " +
            "inner join ClassSchedule cs on cr.ClassScheduleID = cs.ClassScheduleID " +
            "where ClassroomID=a.ClassroomID and " +
            "getDate() between cr.ClassStartTime and DATEADD(second, cs.ClassLength, cr.ClassStartTime)) ";

        private const string QUERY_CLASSROOMBYGROUPID_SQL =
            "SELECT a.ClassroomID, cast(case when EXISTS " +
            QUERY_ISRECORDINGCLASSROOM +
            "THEN 1 ELSE 0 END AS BIT) " +
            "as OnScheduleNow, b.ppcreporttime, b.avcaputureframes, c.ipcreporttime, " +
            "c.agentrecorddata, a.PPCPublicIP, a.IPCPublicIP, a.PPCPort, a.ClassroomName, b.EngineStatus, " +
            "c.AgentStatus, b.CourseName, a.WBNUMBER, b.WBStatus, b.WBNumber as ClassRoomWBNumber, " +
            "a.KaptivoNumber, a.Status, a.NoIPC, b.PPCConnectionStatus, " +
            "b.AVStatus, b.FreeDisk,d.ClassroomTypeId, d.ClassroomTypeName, b.ScreenCaptureStatus, b.Kaptivo1Status, " +
            "b.Kaptivo2Status, b.Kaptivo1DataSize, b.Kaptivo2DataSize FROM CLASSROOM a " +
            "left join ClassroomEngineStatus b on a.ClassroomID=b.ClassroomID " +
            "left join CLassroomAgentStatus c on b.ClassroomID=c.ClassroomId " +
            "left join ClassroomType d on a.ClassroomTypeID=d.ClassroomTypeId " +
            "WHERE CLASSROOMGROUPID=@GROUPID ORDER BY CLASSROOMNAME";

        //private const string SQL_GETCLASSROOMSchedule_CLASSROOMGROUPID = "SELECT CLASSROOMID, CLASSROOMNAME, PSCLASSROOMNAME, CLASSROOM.CLASSROOMGROUPID, PPCPUBLICIP, IPCPUBLICIP, "+
        //    "SVRPORTALPAGEID, PPCPRIVATEIP, IPCPRIVATEIP, PPCPORT, IPCPORT, WBNUMBER,CLASSROOM.STATUS FROM CLASSROOM,ClassroomGroup "+
        //    "WHERE ClassroomGroup.CLASSROOMGROUPID=CLASSROOM.CLASSROOMGROUPID "+
        //    "and CLASSROOM.CLASSROOMGROUPID = @GROUPID ORDER BY CLASSROOM.CLASSROOMGROUPID,CLASSROOMNAME";

        private const string SQL_GETCLASSROOMSchedule_CLASSROOMGROUPID = "select c.ClassroomName, cs.ClassName, cs.InstructorName, wd.WeekDayName, cs.ClassStartTime, cs.ClassLength from ClassSchedule cs, Classroom c, WeekDay wd " +
            "where cs.ClassroomID=c.ClassroomID and c.CLASSROOMGROUPID=@GROUPID " +
            "and cs.ClassStartTime>DATEADD(DAY, -190-2-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE())) " +
            "and cs.WeekDayID = wd.WeekDayID";
            
        private const string QUERY_CLASSROOMDetailBYID_SQL = "SELECT a.ClassroomID, a.ClassroomName, b.EngineStatus, c.AgentStatus, a.WBNUMBER, a.Status, b.PPCConnectionStatus, " +
          "b.AVStatus, b.FreeDisk FROM CLASSROOM a " +
          "left join ClassroomEngineStatus b on a.ClassroomID=b.ClassroomID " +
          "left join CLassroomAgentStatus c on b.ClassroomID=c.ClassroomID " +
          "WHERE a.CLASSROOMID=@CLASSROOMID";

        private const string QUERY_CLASSROOMINFODetailBYID_SQL = "SELECT a.ClassroomID, a.ClassroomName, a.PPCPublicIP, a.IPCPublicIP, b.EngineStatus, c.AgentStatus, a.WBNUMBER, a.Status, b.PPCConnectionStatus, " +
          "b.AVStatus, b.FreeDisk FROM CLASSROOM a " +
          "left join ClassroomEngineStatus b on a.ClassroomID=b.ClassroomID " +
          "left join CLassroomAgentStatus c on b.ClassroomID=c.ClassroomID " +
          "WHERE a.CLASSROOMID=@CLASSROOMID";

        private const string SQL_GETCLASSROOM_CLASSROOMGROUPID = "SELECT CLASSROOMID, CLASSROOMNAME, PSCLASSROOMNAME, CLASSROOM.CLASSROOMGROUPID, PPCPUBLICIP, IPCPUBLICIP, SVRPORTALPAGEID, PPCPRIVATEIP, IPCPRIVATEIP, PPCPORT, IPCPORT, WBNUMBER,CLASSROOM.STATUS FROM CLASSROOM,ClassroomGroup WHERE ClassroomGroup.CLASSROOMGROUPID=CLASSROOM.CLASSROOMGROUPID  and CLASSROOM.CLASSROOMGROUPID = @CLASSROOMGROUPID ORDER BY CLASSROOM.CLASSROOMGROUPID,CLASSROOMNAME";

        internal ClassroomInfo GetClassroomInfoByID(int classID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_CLASSROOMBYID_SQL;
                command.Parameters.AddWithValue("@ClassroomID", classID);
                ClassroomInfo classroomInfo = SqlHelper.ExecuteReaderCmdObject<ClassroomInfo>(command);
                return classroomInfo;
            }
        }

        public List<ClassroomView> GetClassroomByGroupID(int GroupID) {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_CLASSROOMBYGROUPID_SQL;
                command.Parameters.AddWithValue("@GROUPID", GroupID);
                List<ClassroomView> list = SqlHelper.ExecuteReaderCmdList<ClassroomView>(command);
                return list;
            }
        }

        public List<ClassroomScheduleView> GetClassroomScheduleByGroupID(int GroupID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = SQL_GETCLASSROOMSchedule_CLASSROOMGROUPID;
                command.Parameters.AddWithValue("@GROUPID", GroupID);
                List<ClassroomScheduleView> list = SqlHelper.ExecuteReaderCmdList<ClassroomScheduleView>(command);
                return list;
            }
        }

        public Classroom GetClassroomByID(int classroomID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_CLASSROOMBYID_SQL;
                command.Parameters.AddWithValue("@ClassroomID", classroomID);
                Classroom classroom = SqlHelper.ExecuteReaderCmdObject<Classroom>(command);
                return classroom;
            }
        }

        public ClassroomInfoView GetClassroomDetailByID(int ClassroomID)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_CLASSROOMINFODetailBYID_SQL;
                command.Parameters.AddWithValue("@CLASSROOMID", ClassroomID);
                ClassroomInfoView classroomView = SqlHelper.ExecuteReaderCmdObject<ClassroomInfoView>(command);
                return classroomView;
            }
        }
        public IEnumerable<Classroom> GetClassroomTableByGroupID(int nGroupID)
        {
            using (SqlCommand command = new SqlCommand()) {
                command.Connection = Connection;
                AddParamToSQLCmd(command, "@CLASSROOMGROUPID", SqlDbType.Int, 0, ParameterDirection.Input, nGroupID);
                SetCommandContent(command, CommandType.Text, SQL_GETCLASSROOM_CLASSROOMGROUPID);
                return SqlHelper.ExecuteReaderCmd<Classroom>(command);
            }
        }
    }
}
