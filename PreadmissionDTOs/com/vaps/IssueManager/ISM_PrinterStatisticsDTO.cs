using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_PrinterStatisticsDTO 
    {
        public long VMSPOD_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string Role_flag { get; set; }

        public long IVRMRT_Id { get; set; }
        public Array get_department { get; set; }
        public Array get_printdetails { get; set; }
        public Array get_printdetailsedit { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string roletype { get; set; }
        public long HRME_Id { get; set; }

        public long dept_Id { get; set; }
        public string returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public string type { get; set; }
       

        public long VMSPOD_NoOfPages { get; set; }
        public string VMSPOD_Remarks { get; set; }

        public DateTime VMSPOD_PODate { get; set; }

        public string VMSPOD_Type { get; set; }
        public bool VMSPOD_ActiveFlag { get; set; }

        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public long flag { get; set; }
        public string VMSPOD_Type_One { get; set; }
        public string VMSPOD_Type_Two { get; set; }


        public Array get_printers { get; set; }

        public long VMSMPR_Id { get; set; }

        public string VMSMPR_Name { get; set; }

        public DateTime? VMSPOD_FromDate { get; set; }
          public DateTime? VMSPOD_ToDate { get; set; }

        public string duplicate { get; set; }

        public hrmdidlist[] hrmdids { get; set; }

        public string Templatedata { get; set; }
        public string retrunMsg { get; set; }
    }
    public class hrmdidlist
    {
        public long HRMD_Id { get; set; }
    }
}
