using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class ActiveDeactiveEmployeeReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }

        public Array companydetails { get; set; }

        public string radiotype { get; set; }
        public Array GetReport { get; set; }

        public MultiCompany[] selected_Inst { get; set; }
    }

    public class MultiCompanys
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
    }
}
