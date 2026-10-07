using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class HW_HeathCheckUpDTO
    {
        public object temp_Master;

        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HWHCHUP_Id { get; set; }
        public long HWHCHUPF_Id { get; set; }
        public long count { get; set; }
        public long LevelNo { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string OnLoadOrOnChange { get; set; }
        public string Remarks { get; set; }
        public string escreason { get; set; }
        public string escpriority { get; set; }
        public string ISMPSSUP_TemplateName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string returnmsg { get; set; }
        public DateTime? VisitedDate { get; set; }
        public long ISMPSSUP_TemplateID { get; set; }
        public Array getuserinstitution { get; set; }
        public Array getclientlist { get; set; }
        public UploadedFiles_Temp[] UploadedFiles { get; set; }
        public EndUserName_Temp[] EndUserName { get; set; }
        public bool returnval { get; set; }
        public Array Welcomeclosearray { get; set; }

        public bool ActiveFlag { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string status { get; set; }
        public string UserName { get; set; }
        public long ISMPSSUPF_Id { get; set; }
        public Array geteditdetails { get; set; }
        public string ISMPSSUP_EndUSer { get; set; }
        public Array geteditfilesdetails { get; set; }
        public Array geteditclientdetails { get; set; }
        public Array getviewfilesdetails { get; set; }
        public Array getviewdocumetsignoff { get; set; }
        public Array getloaddata { get; set; }
        public Array getemployeedetails { get; set; }
        public Array geteditemployeedetails { get; set; }
        public Array hwcheckupesc { get; set; }
        public Array getapprovaldetails { get; set; }
        public Array viewcommens { get; set; }

        public Array detailsarray { get; set; }
        public long ISMCLTPRMP_AcademicYear { get; set; }
        public ApprovalDetails_Temp[] ApprovalDetails_Temp { get; set; }

        //added  by praveenkumar
        public long ASMAY_ID { get; set; }

        public string ISMPCL_TemplateName { get; set; }
        public string msg { get; set; }

        public long ISMPSSUP_Id { get; set; }
        public Array tem_Master { get; set; }
        public Array get_HWreport { get; set; }
        public Array get_HWreportOne { get; set; }
        public string HWHCHUP_Stauts { get; set; }
        public Array Mompendinglist { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string MI_Logo { get; set; }
       // public Array EndUserName { get; set;}
        //
      //  public long HRME_ID { get; set; }
        //call center data
        public string monthname { get; set; }
        public long year { get; set; }
        public Array Masteryear { get; set; }
        public Array Monthnames { get; set; }
        public CallcenterclientDto[] CallFeedback_List { get; set; }
        public Array feedbacklist { get; set; }

    }
  
    public class UploadedFiles_Temp
    {
        public string filepath { get; set; }
        public string filename { get; set; }
        public string filetype { get; set; }
    }

    public class EndUserName_Temp
    {
        public string HRME_EmployeeFirstName { get; set; }
        public long HRME_ID { get; set; }
    }
    public class ApprovalDetails_Temp
    {
        public long HWHCHUP_Id { get; set; }
        public long Approved_Level { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FinalFlag { get; set; }
        public string apprvlremarks { get; set; }
        public string approvalstatus { get; set; }
    }

    public class CallcenterclientDto
    {
        public long HRME_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string VEF_Remarks { get; set; }
        public bool VEF_NegativeFlag { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public DateTime HWHCFB_CreatedDate { get; set; }
        public long HWHCHUP_Id { get; set; }



    }
}