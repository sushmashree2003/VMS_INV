using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PromotionCalculationDTO
    {
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public Array yearlist { get; set; }
        public Array ctlist { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }
        public string EMP_MarksPerFlg { get; set; }
        public long AMST_Id { get; set; }
    }
    public class TEmp_GroupWise_Marks
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public decimal? ESTMPPSG_GroupMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarks { get; set; }
    }
}


