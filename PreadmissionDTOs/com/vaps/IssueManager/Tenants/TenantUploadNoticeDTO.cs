using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantUploadNoticeDTO
    {
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public bool getclass { get; set; }
        public string message { get; set; }
        public string ASMC_SectionName { get; set; }
        public string studentname { get; set; }
        //----------------------------------------------
        public long ASMAY_Id { get; set; }
        public long? HRMDC_ID { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRME_Id { get; set; }
        public string employeename { get; set; }
        public string MI_Name { get; set; }
        public string HRMDC_Name { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long TENENTNTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string TENENTNTB_Title { get; set; }
        public string TENENTNTB_Description { get; set; }
        public string TENENTNTB_AllOrIndFlg { get; set; }
        public string INTBFL_FileName { get; set; }
        public string INTBFL_FilePath { get; set; }
        public string INTB_FilePath { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public bool INTB_DispalyDisableFlg { get; set; }
        public bool INTB_ToStaffFlg { get; set; }
        public bool INTB_ToStudentFlg { get; set; }



        public long INTBC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool INTBC_ActiveFlag { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
  
        public DateTime? TENENTNTB_DisplayDate { get; set; }
        public DateTime TENENTNTB_StartDate { get; set; }
        public DateTime TENENTNTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public Array notice_details { get; set; }
        public Array editdetails { get; set; }
        public Array editsection { get; set; }
        public Array editstudent { get; set; }
        public Array editdesignation { get; set; }
        public Array editdepartment { get; set; }
        public Array editstaff { get; set; }
        public Array editclasslist { get; set; }

        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array studentlistedit { get; set; }
        public Array deviceArray { get; set; }
        public long dd { get; set; }
        public Array academicyear { get; set; }
        public Array yearlist { get; set; }
        public Array departmentList { get; set; }
        public Array designation { get; set; }
        public Array get_userEmplist { get; set; }
        public Array attachementlist { get; set; }


      //  public IVRM_NoticeBoardDTO[] classlst { get; set; }
        public sectionlistarray1[] sectionlistarray { get; set; }
        public studentarray1[] studentarray { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public employeearraylistDTO[] employeearraylist { get; set; }
        public FilePath_Array1[] FilePath_Array { get; set; }
        public classlistarray1[] classlistarray { get; set; }

        //added by roopa//
        public classlsttwo[] classlsttwo { get; set; }

        public defarray1[] defarray { get; set; }
        public Array seclist { get; set; }

        public bool fee_def { get; set; }

        public bool select_student { get; set; }
        public Array fee_group { get; set; }
        public Array fee_terms { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_Name { get; set; }
        public long FMT_Id { get; set; }
        public string FMT_Name { get; set; }
        public string notice_Name { get; set; }
        public string flag { get; set; }
        public Array viewlist { get; set; }
        public defgrparaay1[] defgrparray { get; set; }
        public Array sect { get; set; }
        public Array stu { get; set; }

        public long AMST_MobileNo { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public string amst_name { get; set; }
        public Array feestulist { get; set; }

        public Array building { get; set; }
        public Array tenants { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long HRMB_Id { get; set; }

        public string TENENTNTBFL_FileName { get; set; }
        public string TENENTNTBFL_FilePath { get; set; }

        // public List<IVRM_NoticeBoardDTO> deviceArr { get; set; }
        //
        public bool TENANTNTBTNT_ActiveFlag { get; set; }
        public TENANTNOTICE_Id[] TENANTNOTICE_Ids { get; set; }

    }
    public class TENANTNOTICE_Id
    {
        public long TENENTNTB_Id { get; set; }
    }

    public class deviceArray1
    {
        public long AMST_MobileNo { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public long AMST_Id { get; set; }

    }
    public class defarray1
    {
        public long FMT_Id { get; set; }

    }
    public class defgrparaay1
    {
        public long FMG_Id { get; set; }
    }
    public class classlsttwo
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }
    public class classlst
    {
        public long ASMCL_Id { get; set; }
    }
    public class classlistarray1
    {
        public long ASMCL_Id { get; set; }

        public long ASMS_Id { get; set; }
    }
    public class sectionlistarray1
    {
        public long ASMS_Id { get; set; }

        public long ASMCL_Id { get; set; }
    }

    public class studentarray1
    {
        public long TENANTAGR_Id { get; set; }

    }
    public class DepartmentDTO
    {
        public int HRMDC_ID { get; set; }
    }
    public class DesignationDTO
    {
        public long HRMDES_Id { get; set; }
    }
    public class employeearraylistDTO
    {
        public long HRME_Id { get; set; }
    }
    public class FilePath_Array1
    {
       
        public string TENENTNTBFL_FileName { get; set; }
        public string  TENENTNTBFL_FilePath { get; set; }
}

}
