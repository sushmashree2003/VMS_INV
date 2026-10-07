using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
    public class TADA_PaymentReportDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public Array get_emp { get; set; }       
        public string HRME_EmployeeFirstName { get; set; }
        public string RDO_PUNCH { get; set; }
        public Array GetReport { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string company_name { get; set; }
        public string MI_Logo { get; set; }     
        public list[] HRME_Id_List { get; set; }
        public long HRME_Id { get; set; }
    }
    public class list
    {
        public long HRME_IdS { get; set; }
    }
}
