using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class StudentGatePass_DTO:CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public long GPHS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string GPHS_GatePassNo { get; set; }
        public string GPHS_IDCardNo { get; set; }
        public DateTime? GPHS_DateTime { get; set; }
        public string GPHS_Remarks { get; set; }
        public bool GPHS_ActiveFlg { get; set; }
        public long? GPHS_CreatedBy { get; set; }
        public long? GPHS_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }

        public long ASMCL_Id { get; set; }
        public long UserId { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }

        public string trans_id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string studentname { get; set; }
        public Array yearlist { get; set; }
        public Array classList { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array currentstuddata { get; set; }
        public Array institution { get; set; }
        public bool returnval { get; set; }
        public bool dulicate { get; set; }
        public long? AMST_MobileNo { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_SecretCode { get; set; }
        public string GPHS_ReceiverName { get; set; }
        public string GPHS_ReceiverPhoneNo { get; set; }
        public string GPHS_ReceiverIdProof { get; set; }
        public string GPHS_SecretCode { get; set; }
        public long? GPHS_OTP { get; set; }
        public bool? GPHS_SentFlg { get; set; }
        public string mobileNo { get; set; }
        public string message { get; set; }
        public Array otpid { get; set; }  
        public Array singlestudentdata { get; set; }
        public string htmldata { get; set; }
    }
}
