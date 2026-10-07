using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class IVRM_ClassWorkDTO
    {
        public long ICW_Id { get; set; }
        public long MI_Id { get; set; }
        public string ICW_Topic { get; set; }
        public string ICW_SubTopic { get; set; }
        public string ICW_Content { get; set; }
        public long ICW_TeachingAid { get; set; }
        public bool ICW_ActiveFlag { get; set; }
        public DateTime ICW_FromDate { get; set; }
        public DateTime ICW_ToDate { get; set; }
        public string ICW_Evaluation { get; set; }
        public string ICW_Attachment { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long Login_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ICW_Assignment { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
        public string finalDate { get; set; }

        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public Array subjectlist { get; set; }
        public string ISMS_SubjectName { get; set; }
        public IVRM_ClassWorkDTO[] images_list { get; set; }
        public string returnsavestatus { get; set; }
        public bool returnval { get; set; }
        public string imagePath { get; set; }

        public long AMST_Id { get; set; }
        public Array worklist { get; set; }
        public Array classSectionlist { get; set; }
        public Array yearlist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array classwork { get; set; }
        public string ICW_FilePath { get; set; }
        public bool dulicate { get; set; }
        public Array editlist { get; set; }
        public int ASMCL_Order { get; set; }
        public long ISMS_order { get; set; }



    }
}

