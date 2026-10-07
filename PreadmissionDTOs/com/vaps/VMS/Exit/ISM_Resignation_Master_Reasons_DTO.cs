using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
    public class ISM_Resignation_Master_Reasons_DTO
    {
        public long ISMRESGMRE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMRESGMRE_ResignationReasons { get; set; }
        public bool ISMRESGMRE_ActiveFlag { get; set; }
        public string ISMRESGMRE_Flg { get; set; }
        public string ISMRESGMRE_Template { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? HRME_DOL { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEM_EmailId { get; set; }
        public long ISMRESGMRE_CreatedBy { get; set; }
        public long ISMRESGMRE_UpdatedBy { get; set; }
        public long userId { get; set; }
        public long? ISMRESGMRE_NoticePeriodDays { get; set; }
        public string returnval { get; set; }
        public string MI_Name { get; set; }

        //==================================================
        public Array get_reason_list { get; set; }
        public Array get_details_reason_list { get; set; }


        //Exit Overall Report ---- Aneeket
        public Array getalldetails { get; set; }
        public Array employeedetails { get; set; }
        public Array company_list_dd { get; set; }


        public Array getReport { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public string HRME_EmployeeCode { get; set; }
        public string ISMRESG_ResignationDate { get; set; }

        public string ISMRESGAB_RelievingDate { get; set; }

        public string ApprovedBy { get; set; }

        public string ISMRESGAB_AppRejFlg { get; set; }
        public string HRPAON_SanctionLevelNo { get; set; }

        public Array fillyear { get; set; }
        public Array fillmonth { get; set; }

        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string monthname { get; set; }
        public long HRMLY_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string month { get; set; }
        public string type { get; set; }
        public long year { get; set; }
        public string HRName { get; set; }
        public string HRemail { get; set; }
        public string DeptHead { get; set; }
        public string DeptheadEmail { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public DateTime HRME_ENDDate { get; set; }

        public string NextlevelApprovalMail { get; set; }
        public string CurrentApprovalMail { get; set; }
        public long currentlevel { get; set; }
        public long NextLevel { get; set; }

        public Array getMonth { get; set; }
        public EmployeeName[] EmpName { get; set; }
        public int savecount { get; set; }
        public int duplicatecount { get; set; }


        public class EmployeeName
        {
            public long HRME_Id { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
        }



        public TempDTOO[] ActivitynameArray { get; set; }

        public class TempDTOO
        {
            public long MI_Id { get; set; }
            public long ISMRESGMRE_Id { get; set; }
        }

    }
}


