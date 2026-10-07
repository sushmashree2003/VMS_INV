using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ToppersListReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_FirstName { get; set; }
        public string ASMC_SectionName { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int? ESTMP_ClassRank { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array sublist { get; set; }
        public Array datareport { get; set; }
        public Array datareport1 { get; set; }
        public string report_type { get; set; }
        public string sub_check_type { get; set; }
        public string exm_check_type { get; set; }
        public int topper { get; set; }

        public class KioskExamTopperDTO
        {
            public string studentName { get; set; }
            public string className { get; set; }
            public string sectionName { get; set; }
            public string examName { get; set; }
            public string photo { get; set; }
            public KioskExamTopperDTO[] kioskExamToppers { get; set; }

        }
    }
}
