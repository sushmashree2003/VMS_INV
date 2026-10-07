using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class IVRM_Homework_DTO : CommonParamDTO
    {
        public long IHW_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long IHW_AssignmentNo { get; set; }
        public long ISMS_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? IHW_Date { get; set; }
        public string IHW_Topic { get; set; }
        public string IHW_Assignment { get; set; }
        public string IHW_Attachment { get; set; }
        public string IHW_FilePath { get; set; }
        public bool IHW_ActiveFlag { get; set; }
        public long Login_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool returnval { get; set; }
        public string returnsavestatus { get; set; }

        public string Role_flag { get; set; }
        public string roleflg { get; set; }
        public Array roletype { get; set; }
        public Array configflag { get; set; }


        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array classwork { get; set; }
        public Array subjectlist { get; set; }
        public bool dulicate { get; set; }

        public string ASMC_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array editlist { get; set; }
        public clsarrayDTO[] clsarray { get; set; }
        public secarrayDTO[] secarray { get; set; }

        public class clsarrayDTO
        {
            public long ASMCL_Id { get; set; }
        }
        public class secarrayDTO
        {
            public long ASMS_Id { get; set; }
        }

    }
}
