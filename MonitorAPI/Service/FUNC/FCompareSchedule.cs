using MonitorAPI.Models.OperationModel;
using MonitorAPI.Service.Operations;
using System;
using System.Data;

namespace MonitorAPI.Service.FUNC
{
    public class FCompareSchedule
    {
        public static DataTable CompareClassSchedule(int nClassroomID, string sessionID)
        {

            DataTable server = ServiceFactory.OperationService.GetClassRecordingTablebyClassroomID(nClassroomID);
            DataTable local = SingleCommand.GetDataTableStyleLocalSchedule(nClassroomID, sessionID);

            DataTable xtb = new DataTable("Results");
            xtb.Columns.Add("ClassID", typeof(String));
            xtb.Columns.Add("ClassName", typeof(String));
            xtb.Columns.Add("ClassStartTime", typeof(DateTime));
            xtb.Columns.Add("ClassLength", typeof(Int32));
            xtb.Columns.Add("ClassroomID", typeof(Int32));
            xtb.Columns.Add("Server", typeof(String));
            xtb.Columns.Add("Local", typeof(String));
            DataColumn[] keys = new DataColumn[2];
            keys[0] = xtb.Columns["ClassID"];
            keys[1] = xtb.Columns["ClassStartTime"];
            xtb.PrimaryKey = keys;

            DatePeriod datePeriod = ServiceFactory.OperationService.GetDatePeriod(0);
            DateTime dtNow = datePeriod.StartDate;

            foreach (DataRow row in server.Rows)
            {
                if (dtNow < Convert.ToDateTime(row["ClassStartTime"]))
                {
                    DataRow newrow = xtb.NewRow();
                    newrow["ClassID"] = row["ClassID"];
                    newrow["ClassName"] = row["ClassName"];
                    newrow["ClassStartTime"] = row["ClassStartTime"];
                    newrow["ClassLength"] = row["ClassLength"];
                    newrow["ClassroomID"] = nClassroomID;
                    newrow["Server"] = "yes";
                    newrow["Local"] = "no";
                    xtb.Rows.Add(newrow);
                }
            }
            foreach (DataRow row in local.Rows)
            {
                object[] s = { Convert.ToString(row["ClassID"]), Convert.ToDateTime(row["ClassStartTime"]) };
                DataRow xrow = xtb.Rows.Find(s);
                if (xrow == null)
                {
                    if (dtNow < Convert.ToDateTime(row["ClassStartTime"]))
                    {
                        DataRow newrow = xtb.NewRow();
                        newrow["ClassID"] = row["ClassID"];
                        newrow["ClassName"] = row["ClassName"];
                        newrow["ClassStartTime"] = row["ClassStartTime"];
                        newrow["ClassLength"] = row["ClassLength"];
                        newrow["ClassroomID"] = nClassroomID;
                        newrow["Server"] = "no";
                        newrow["Local"] = "yes";
                        xtb.Rows.Add(newrow);
                    }
                }
                else
                {
                    xrow["Local"] = "yes";
                }
            }
            return xtb;
        }

        public static DataTable CompareClassScheduleByGroupAndDate(string sessionID, int nGroupID, DateTime sdate)
        {
            DataTable dtClassroom = ServiceFactory.ClassroomService.GetClassroomByGroupID(sessionID, nGroupID, sdate);
            DataTable xtb = new DataTable("Results");
            xtb.Columns.Add("ClassID", typeof(String));
            xtb.Columns.Add("ClassName", typeof(String));
            xtb.Columns.Add("ClassStartTime", typeof(DateTime));
            xtb.Columns.Add("Server", typeof(String));
            xtb.Columns.Add("Local", typeof(String));
            DataColumn[] keys = new DataColumn[2];
            keys[0] = xtb.Columns["ClassID"];
            keys[1] = xtb.Columns["ClassStartTime"];
            xtb.PrimaryKey = keys;
            foreach (DataRow row in dtClassroom.Rows)
            {
                DataTable temptb = CompareClassScheduleByDate(Convert.ToInt32(row["ClassroomID"]), sdate, sessionID);
                xtb.Merge(temptb);
            }
            return xtb;
        }

        public static DataTable CompareClassScheduleByDate(int nClassroomID, DateTime checkscheduleDate, string sessionID)
        {
            DataTable xtb = new DataTable("Results");
            xtb.Columns.Add("ClassID", typeof(String));
            xtb.Columns.Add("ClassName", typeof(String));
            xtb.Columns.Add("ClassStartTime", typeof(DateTime));
            xtb.Columns.Add("ClassLength", typeof(Int32));
            xtb.Columns.Add("ClassroomID", typeof(Int32));
            xtb.Columns.Add("Server", typeof(String));
            xtb.Columns.Add("Local", typeof(String));
            DataColumn[] keys = new DataColumn[2];
            keys[0] = xtb.Columns["ClassID"];
            keys[1] = xtb.Columns["ClassStartTime"];
            xtb.PrimaryKey = keys;

            if (checkscheduleDate == null || checkscheduleDate < DateTime.Today)
            {
                return xtb;
            }

            DataTable server = ServiceFactory.OperationService.GetClassRecordingTablebyClassroomIDAndDate(nClassroomID, checkscheduleDate);
            DataTable local = SingleCommand.GetDataTableStyleLocalSchedule(nClassroomID, sessionID);

            DateTime currenttime = DateTime.Now;

            foreach (DataRow row in server.Rows)
            {
                DateTime cscheduletime = Convert.ToDateTime(row["ClassStartTime"]);
                //double check
                if ((checkscheduleDate.Year == cscheduletime.Year) && (checkscheduleDate.Month == cscheduletime.Month) && (checkscheduleDate.Day == cscheduletime.Day) &&
                    cscheduletime >= currenttime)
                {
                    DataRow newrow = xtb.NewRow();
                    newrow["ClassID"] = row["ClassID"];
                    newrow["ClassName"] = row["ClassName"];
                    newrow["ClassStartTime"] = row["ClassStartTime"];
                    newrow["ClassLength"] = row["ClassLength"];
                    newrow["ClassroomID"] = nClassroomID;
                    newrow["Server"] = "yes";
                    newrow["Local"] = "no";

                    xtb.Rows.Add(newrow);
                }
            }
            foreach (DataRow row in local.Rows)
            {
                object[] s = { Convert.ToString(row["ClassID"]), Convert.ToDateTime(row["ClassStartTime"]) };
                DataRow xrow = xtb.Rows.Find(s);
                if (xrow == null)
                {
                    DateTime cscheduletime = Convert.ToDateTime(row["ClassStartTime"]);
                    if ((checkscheduleDate.Year == cscheduletime.Year) && (checkscheduleDate.Month == cscheduletime.Month) && (checkscheduleDate.Day == cscheduletime.Day) &&
                        cscheduletime >= currenttime)
                    {
                        DataRow newrow = xtb.NewRow();
                        newrow["ClassID"] = row["ClassID"];
                        newrow["ClassName"] = row["ClassName"];
                        newrow["ClassStartTime"] = row["ClassStartTime"];
                        newrow["ClassLength"] = row["ClassLength"];
                        newrow["ClassroomID"] = nClassroomID;
                        newrow["Server"] = "no";
                        newrow["Local"] = "yes";
                        xtb.Rows.Add(newrow);
                    }
                }
                else
                {
                    xrow["Local"] = "yes";
                }
            }
            return xtb;
        }
    }
}