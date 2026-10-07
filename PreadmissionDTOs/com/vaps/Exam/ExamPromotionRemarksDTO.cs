using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamPromotionRemarksDTO
    {
        public long EPRD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long roleId { get; set; }
        public long userId { get; set; }
        public long Emp_Code { get; set; }
        public long examtype { get; set; }
        public long AMAY_RollNo { get; set; }
        public string EPRD_PromotionName { get; set; }
        public string EPRD_Remarks { get; set; }
        public string EPRD_ClassPromoted { get; set; }
        public string EPRD_Promotionflag { get; set; }
        public string message { get; set; }
        public string username { get; set; }     
        public string flag { get; set; }     
        public string studentname { get; set; }
        public string AMST_Admno { get; set; }
        public string rolename { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public bool returnval { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getexam { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getsavedetails { get; set; }
        public examtype_temp[] examtype_temp { get; set; }

    }
    public class examtype_temp
    {
        public long AMST_Id { get; set; }
        public long EPRD_Id { get; set; }
        public string EPRD_PromotionName { get; set; }
        public string EPRD_Remarks { get; set; }
        public string EPRD_ClassPromoted { get; set; }
        public string studentname { get; set; }
    }
}
