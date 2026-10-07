using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.IVRM
{
    public class ClgNoticeBoardDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMCO_Id { get; set; }      
        public long AMCST_Id { get; set; }      
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }                  
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public int AMCO_Order { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public long INTBCB_Id { get; set; }    
        public bool INTBCB_ActiveFlag { get; set; }

        public long INTB_Id { get; set; }  
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public bool INTB_DispalyDisableFlg { get; set; }


        public Array course_list { get; set; }
        public Array branch_list { get; set; }
        public Array sem_list { get; set; }
        public Array noticelist { get; set; }
        public Array noticedetails { get; set; }
        public Array editdetails { get; set; }
        public Array coursebranchsem { get; set; }

        
        public CourseArrayDTO[] courseArray { get; set; }
        public BranchArrayDTO[] branchArray { get; set; }
        public SemesterArrayDTO[] semesterArray { get; set; }
    }

    public class CourseArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
    }
    public class BranchArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class SemesterArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }
}
