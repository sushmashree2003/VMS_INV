using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ModuleWiseFeedbackReport_DTO : CommonParamDTO
    {

        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public bool returnval { get; set; }
        public long IVRM_MI_Id { get; set; }
        public string staffname { get; set; }
        public string Institute_Name { get; set; }
        public Array getfeedbackdata { get; set; }
        public Array institutiondata { get; set; }
        public Array masterclientlist { get; set; }
        public clientlistarrr[] clientlistarr { get; set; }
        public string Institute_code { get; set; }
        public string r_month { get; set; }
        public string r_year { get; set; }
        public Array vmsmoduleratingreport { get; set; }
        //---------------------added by adarsh for canteen AND HS-----------
        public Array canteenfeedbackreport { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string flag {get; set;}
        public string type {get; set; }
        //----------------------------end -----------------------------------
        public savemodulefeedbackDTO[] savemodulefeedback { get; set; }

        public class clientlistarrr
        {
            public long IVRM_MI_Id { get; set; }
            public string Institute_code { get; set; }
        }

        public class savemodulefeedbackDTO
        {
            public long FMTY_Id { get; set; }
            public long FMQE_Id { get; set; }
            public string name { get; set; }
            public string FSTTR_FeedBack { get; set; }
            public int FMOP_OptionsValue { get; set; }
            public string FMOP_FeedbackOptions { get; set; }
            public string FMTY_FeedbackTypeName { get; set; }
            public string FMQE_FeedbackQuestions { get; set; }
            public string FMQE_FeedbackQRemarks { get; set; }
        }
        public Array hr_masteryear { get; set; }
        public string optionflag { get; set; }
    }
}
