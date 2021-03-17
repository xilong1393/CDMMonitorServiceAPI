using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitorAPI.Models.OperationModel
{
    public class SubmitStartTestCourse
    {
        public ClassSchedule ClassSchedule { get; set; }
        public string SessionID { get; set; }
    }
}