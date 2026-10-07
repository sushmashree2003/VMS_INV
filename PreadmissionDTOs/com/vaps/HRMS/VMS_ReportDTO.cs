using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class VMS_ReportDTO
    {
        public long MI_Id { get; set; }



        public long ISMMPR_Id { get; set; }

        public string ISMMPR_ProjectName { get; set; }

        public Array companydetails { get; set; }

        public Array getemployee { get; set; }

        public Array projectName { get; set; }

        public Array GetReport { get; set; }
       
        public Array fillemployee { get; set; }

        public string HRME_EmployeeFirstName { get; set; }

        public string HRME_EmployeeMiddleName { get; set; }

        public string HRME_EmployeeLastName { get; set; }

        public string HRME_EmployeeCode { get; set; }

        public long HRME_Id { get; set; }

        public string multipledep { get; set; }

        public string allind { get; set; }

        public selectedcompany[] selected_Inst { get; set; }
    }

    public class selectedcompany
    {
        public long MI_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long HRME_Id { get; set; }
    }
}
