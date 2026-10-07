using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class GeneralConfigDTO
    {
        public int IVRMGC_Id { get; set; }
        public long MI_Id { get; set; }
        public bool IVRMGC_MobileValOTPFlag { get; set; }
        public bool IVRMGC_emailValOTPFlag { get; set; }
        public string IVRMGC_StudentPhotoPath { get; set; }
        public string IVRMGC_StaffPhotoPath { get; set; }
        public bool IVRMGC_ComTrasaNoFlag { get; set; }
        public string IVRMGC_SMSDomain { get; set; }
        public string IVRMGC_SMSURL { get; set; }
        public string IVRMGC_SMSUserName { get; set; }
        public string IVRMGC_SMSPassword { get; set; }
        public string IVRMGC_SMSSenderId { get; set; }
        public string IVRMGC_SMSWorkingKey { get; set; }
        public string IVRMGC_SMSFooter { get; set; }
        public bool IVRMGC_SMSActiveFlag { get; set; }
        public string IVRMGC_emailUserName { get; set; }
        public string IVRMGC_emailPassword { get; set; }
        public string IVRMGC_HostName { get; set; }
        public int IVRMGC_PortNo { get; set; }
        public string IVRMGC_MailGenralDesc { get; set; }
        public string IVRMGC_Webiste { get; set; }
        public string IVRMGC_emailid { get; set; }
        public string IVRMGC_emailFooter { get; set; }
        public string IVRMGC_CCMail { get; set; }
        public string IVRMGC_BCCMail { get; set; }
        public string IVRMGC_ToMail { get; set; }
        public bool IVRMGC_EmailActiveFlag { get; set; }
        public int IVRMGC_Pagination { get; set; }
        public int IVRMGC_ReminderDays { get; set; }
        public int IVRMGC_ClassCapacity { get; set; }
        public int IVRMGC_SectionCapacity { get; set; }
        public int IVRMGC_SCLockingPeriod { get; set; }
        public int IVRMGC_FPLockingPeriod { get; set; }
        public bool IVRMGC_SCActive { get; set; }
        public bool IVRMGC_FPActive { get; set; }
        public bool IVRMGC_OnlineFPActive { get; set; }
        public bool IVRMGC_FaceReaderActive { get; set; }
        public bool IVRMGC_DefaultStudentSelection { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int IVRMGC_PagePagination { get; set; }
        public int IVRMGC_ReportPagination { get; set; }
        public bool IVRMGC_AdmnoColumnDisplay { get; set; }
        public bool IVRMGC_RegnoColumnDisplay { get; set; }
        public bool IVRMGC_RollnoColumnDisplay { get; set; }
        public string IVRMGC_Disclaimer { get; set; }
        public string returnval { get; set; }
        public int ASMAY_Id { get; set; }
        public Array AcademicList { get; set; }
        public Array mstConfigList { get; set; }
        public string Instuitename { get; set; }
        public string Academicyear { get; set; }
        public string IVRMGC_PrincipalSign { get; set; }
        public string IVRMGC_ManagerSign { get; set; }
        public string IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag { get; set; }
        public int IVRMGC_TransportRequired { get; set; }
        public string IVRMGC_OnlinePaymentCompany { get; set; }
        public long IVRMGC_OTPMobileNo { get; set; }
        public string IVRMGC_OTPMailId { get; set; }
        public string IVRMGC_Classwise_Payment { get; set; }
        public string IVRMGC_APIOrSMTPFlg { get; set; }
        public string IVRMGC_SendGrid_Key { get; set; }

        public bool? IVRMGC_FMSManualReferenceFlg { get; set; }
        public string IVRMGC_VMReminderSchedule { get; set; }
        public string IVRMGC_VMReminderFlag { get; set; }
        public string IVRMGC_VMRepeatFlag { get; set; }
    }
}
