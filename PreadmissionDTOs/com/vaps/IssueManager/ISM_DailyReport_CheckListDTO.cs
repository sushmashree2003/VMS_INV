using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_CheckListDTO
    {
        public long MI_Id { get; set; }
        public long MI_IdNew { get; set; }
        public long User_Id { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string selectionflag { get; set; }
        public Array get_userEmplist { get; set; }
        public Array mi_list { get; set; }
        public Array getdailyreport { get; set; }
        public arrayuserEmp1[] arrayuserEmp { get; set; }

        public class arrayuserEmp1
        {
            public long HRME_Id { get; set; }
        }
    }
}
