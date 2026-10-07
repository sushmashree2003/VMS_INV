using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_InterDepartmentReport  : CommonParamDTO
    {
        public string Role_flag { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long? HRMD_Id { get; set; }
        public long? yearid { get; set; }
        public long? month { get; set; }
        public string rdbbutton { get; set; }
        public string reporttype { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public int successcount { get; set; }
        public bool emparrayexist  { get; set; }
        public bool allarrayexist { get; set; }       
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public Array consolidatedissues { get; set; }
        public Array consolidatedissuesview { get; set; }
        public Array InterDepartmentissues { get; set; }
        public Array InterDepartmentissuesview { get; set; }
        public Array Departmentempissues { get; set; }
        public Array Departmentempissuesview { get; set; }
        public Array Departmentempissuesall { get; set; }
        public Array temp_epartmentempissuesall { get; set; }
        public Array Departmentempissuesviewall { get; set; }
        public Array overallempissues { get; set; }
        public Array overallempissuesempissuesview  { get; set; }
        public Array overallempissuesall { get; set; }
        public Array temp_overallempissuesall { get; set; }
        public Array overallempmemonotice { get; set; }
        public Array empmanagementdetailreport { get; set; }
        public Array overallempissuesempissuesviewall { get; set; }       
        public Array get_department { get; set; }     
        public string Emailcontent { get; set; }
        public string result { get; set; }
    }
}