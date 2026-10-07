using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class Dept_HeadWiseLeaveReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getReport { get; set; }
        public DateTime GETFROMDATE { get; set; }
        public DateTime GETTODATE { get; set; }
    }

}
