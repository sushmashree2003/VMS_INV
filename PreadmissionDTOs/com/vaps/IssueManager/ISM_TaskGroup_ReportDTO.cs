using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_TaskGroup_ReportDTO
    {
        public long ISMMTGRP_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ISMTPLAPTA_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMTGRP_TaskGroupName { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public string ISMTCR_Title { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public Array grouplist { get; set; }
        public Array userdetails { get; set; }
        public Array department_list { get; set; }

        public ISM_TaskGroup_ReportDTO[] selectedgrouplist { get; set; }
        public ISM_TaskGroup_ReportDTO[] selecteddepartment_list { get; set; }

        public long ISMMPR_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public Array reportdetails { get; set; }
        public Array projectlist { get; set; }
        public ISM_TaskGroup_ReportDTO[] selectedprojectlist { get; set; }

        /*CATEGORY GROUP REPORT */
        public bool? roleflag { get; set; }
        public long IVRMRT_Id { get; set; }
        public Array DepartmentList { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMDC_ID { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public string Role_flag { get; set; }
        public Array getcategory { get; set; }
        public Array getemployee { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public categorytempdto[] categorytempdto { get; set; }
        public categoryemptempdto[] categoryemptempdto { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public Array get_report { get; set; }
    }
    public class categorytempdto
    {
        public long ISMMTCAT_Id { get; set; }
    }
    public class categoryemptempdto
    {
        public long HRME_Id { get; set; }
    }
}
