using System;
using System.ComponentModel;

namespace MonitorAPI.Model
{
    public class ClassroomView
    {
        public string ClassroomTypeName { get; set; }
        [Browsable(false)]
        public int ClassroomTypeId { set; get; }
        [Browsable(false)]
        public int ClassroomID { get; set; }
        public bool OnScheduleNow { get; set; }
        public string PPCPublicIP { get; set; }
        public string IPCPublicIP { get; set; }
        public int PPCPort { get; set; }
        public DateTime? PPCReportTime { get; set; }
        public DateTime? IPCReportTime { get; set; }
        public long? AVCaputureFrames { get; set; }
        public int AgentRecordData { get; set; }
        public string ClassroomName { get; set; }
        public string EngineStatus { get; set; }
        public string AgentStatus { get; set; }
        public bool NoIPC { get; set; }
        [DisplayName("PPC")]
        public string PPCConnectionStatus { get; set; }
        [DisplayName("Course")]
        public string CourseName { get; set; }
        public string AVStatus { get; set; }
        public string WBStatus { get; set; }
        public int WBNumber { get; set; }
        public int ClassRoomWBNumber { get; set; }
        public int KaptivoNumber { get; set; }
        public string Status { get; set; }
        public int FreeDisk { get; set; }
        public string ScreenCaptureStatus { get; set; }
        public string Kaptivo1Status { get; set; }
        public string Kaptivo2Status { get; set; }
        public int Kaptivo1DataSize { get; set; }
        public int Kaptivo2DataSize { get; set; }
    }
}
