using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class ISM_Master_TaskCategory_DTO
    {
        public long ISMMTCAT_Id { get; set; }
        public long ISMMTCATCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIId { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public string ISMMTCATCL_CheckListName { get; set; }
        public bool? ISMMTCATCL_UploadEnterFlg { get; set; }
        public bool? ISMMTCATCL_CheckListFlg { get; set; }
        public bool ISMMTCATCL_ActiveFlag { get; set; }
        public decimal ISMMTCAT_TaskPercentage { get; set; }
        public string ISMMTCAT_TCRemarks { get; set; }
        public bool ISMMTCAT_ActiveFlag { get; set; }
        public long ISMMTCAT_CreatedBy { get; set; }
        public long ISMMTCAT_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public DateTime ISMMTCAT_CreatedDate { get; set; }
        public DateTime ISMMTCAT_UpdatedDate { get; set; }
        public string ISMMTCAT_EachTaskMaxDuration { get; set; }
        public string ISMMTCAT_DurationFlg { get; set; }
        public string message { get; set; }
        public Array departmentlist { get; set; }
        public Array departmentlistedit { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public Array checklist { get; set; }
        public Array checklist_report { get; set; }
        public Array categorylist { get; set; }
        public Array checklistedit { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public string msg { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public bool ISMMTCAT_CompulsoryFlg { get; set; }
        public string MI_Name { get; set; }
        public int HRMD_Order { get; set; }
        public checklistarray1[] checklistarray { get; set; }
        public class checklistarray1
        {
            public string ISMMTCATCL_CheckListName { get; set; }
            public bool ISMMTCATCL_UploadEnterFlg { get; set; }
            public bool ISMMTCATCL_CheckListFlg { get; set; }
        }
    }
}
