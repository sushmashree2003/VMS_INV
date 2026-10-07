using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class EmailDeviationDTO : CommonParamDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ename { get; set; }
        public string emailids { get; set; }
        public string responseby { get; set; }
        public string ISMTCRRES_Response { get; set; }
        public DateTime ISMTCRRES_ResponseDate { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMTCR_Title { get; set; }
        public string ISMMCLT_Desc { get; set; }
        public long ISMMCLT_ContactNo { get; set; }
        public string ISMMCLT_EmailId { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public string ISMMCLT_Address { get; set; }
        public string ISMMCLT_NOName { get; set; }
        public string ISMMCLT_NOEmailId { get; set; }
        public long ISMMCLT_NOContactNo { get; set; }
        public long? ISMMCLT_CordinatorId { get; set; }
        public long? ISMMCLT_TeamLeadId { get; set; }
        public bool ISMMCLT_ActiveFlag { get; set; }
        public long ISMMCLT_CreatedBy { get; set; }
        public long ISMMCLT_UpdatedBy { get; set; }
        public long ISMCIM_IEList { get; set; }

        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long HRMD_Id { get; set; }
       
        public string cordinatorname { get; set; }
        public string ieEmpname { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string leadername { get; set; }
        public string teamleadFirstName { get; set; }
        public string teamleadMiddleName { get; set; }
        public string teamleadLastName { get; set; }
        public string effort { get; set; }
        public string desname { get; set; }
        public string daydiff { get; set; }
        public string MI_Name { get; set; }




        public string deptname { get; set; }
        public string HRMP_Name { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public string assignedtoe { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public string assignedby { get; set; }

        public string cordinatorFirstName { get; set; }
        public string cordinatorMiddleName { get; set; }
        public string cordinatorLastName { get; set; }
        public string ieEmployName { get; set; }
        public string usersname { get; set; }
        public string employeename { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string username { get; set; }
        public long ISMUSEMM_Id { get; set; }
        public bool ISMUSEMM_ActiveFlag { get; set; }
        public long ISMUSEMM_CreatedBy { get; set; }
        public long ISMUSEMM_UpdatedBy { get; set; }

        public Array cordinartorlist { get; set; }
        public Array get_Assignedtasklist { get; set; }
        public Array teamleadlist { get; set; }
        public Array get_clentlist { get; set; }
        public Array editClient { get; set; }
        public Array clientlist { get; set; }
        public Array ieEmpList { get; set; }
        public Array get_userslist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_mappedEmplist { get; set; }
        public Array get_departmentlist { get; set; }
        public Array allIeMappingdata { get; set; }
        public Array get_userEmpcount { get; set; }
        public Array get_userEmpDetails { get; set; }

        public bool duplicate { get; set; }  
        public long ISMMCLTIE_Id { get; set; }
        public long User_Id { get; set; }
        public bool ISMMCLTIE_ActiveFlag { get; set; }
        public long ISMMCLTIE_CreatedBy { get; set; }
        public long ISMMCLTIE_UpdatedBy { get; set; }


        public long ISMMCLTUS_Id { get; set; }  
        public DateTime ISMTCR_CreationDate { get; set; }  
        public DateTime ISMTCRASTO_StartDate { get; set; }  
        public DateTime ISMTCRASTO_EndDate { get; set; }  
        public bool ISMMCLTUS_ActiveFlag { get; set; }

        public Array editCltMappinglist { get; set; }
        public Array modaliEMapingList { get; set; }
        public Clients_DTO[] ieEmpListdata { get; set; }
        public Array modalUserMapingList { get; set; }
        public string useempName { get; set; }
        public employeeListDTO[] employeeList { get; set; }

        public long HeadId { get; set; }
        public long IndEmpId { get; set; }
        public string HeadName { get; set; }
        public string IndEmpName { get; set; }
        public string DRDate { get; set; }
        public string DRStatus { get; set; }
    }

    public class employeeListDTOww
    {      
        public long HRME_Id { get; set; }
        public long User_Id { get; set; }
        public long ISMUSEMM_Id { get; set; }
        public bool ISMUSEMM_ActiveFlag { get; set; }
        public long ISMUSEMM_CreatedBy { get; set; }
        public long ISMUSEMM_UpdatedBy { get; set; }

    }
}
