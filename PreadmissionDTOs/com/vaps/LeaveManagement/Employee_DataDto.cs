using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class Employee_DataDto
    {
        public Array empData { get; set; }
        public string Date { get; set; }
        public long HRME_ID { get; set; }
        public long MI_Id { get; set; }

        public long User_Id { get; set; }
        public string returnData { get; set; }
    }
}
