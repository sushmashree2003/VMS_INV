using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
  public class ISM_Master_TaskCategory_Employee_DTO
    {

        public long ISMMTCATE_Id { get; set; }
        public decimal TaskPercentage { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public bool checkingflg { get; set; }
        public long HRME_Id { get; set; }
        public Decimal ISMMTCATE_TaskPercentage { get; set; }
        public Decimal ISMMTCAT_TaskPercentage { get; set; }
        public bool ISMMTCATE_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public long ISMMTCATE_CreatedBy { get; set; }
        public long ISMMTCATE_UpdatedBy { get; set; }
        public DateTime ISMMTCATE_CreatedDate { get; set; }
        public DateTime ISMMTCATE_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public bool ISMMTCAT_CompulsoryFlg { get; set; }
        public long MI_Id { get; set; }
        public Array alldata1 { get; set; }
        public Array categorylist { get; set; }
        public Array employeelist { get; set; }
        public decimal percent { get; set; }
        public long notsave { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string msg { get; set; }
        public string MI_Name { get; set; }
        public string message { get; set; }
        public decimal total { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public Array alldata2 { get; set; }
        public Array departmentlist { get; set; }
        public long HRMD_Id { get; set; } 
        public string returnduplicatestatus { get; set; } 
        public Array catlistselected { get; set; } 
        public Array selectflg { get; set; } 
        public bool? ISMMTCATE_CompulsoryFlg { get; set; } 
        public ISM_Master_TaskCategory_Employee_DTO[] selectedcategorylist { get; set; }
        public ISM_Master_TaskCategory_Employee_DTO[] selectedemployeelist { get; set; }
        public ISM_Master_TaskCategory_Employee_DTO[] emplcategorylist { get; set; }

        public int count { get; set; }
        public int count1 { get; set; }
    }
}
