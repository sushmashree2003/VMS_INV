using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class StudentDashboardDTO
    {
        public long MI_Id { get; set; }

        public long FSS_Id { get; set; }

        public string mobileprivileges { get; set; }
        public long ASMAY_Id { get; set; }
        public Array pushnotiflist { get; set; }
        public long AMST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSS_OBArrearAmount { get; set; }
        public long FSS_OBExcessAmount { get; set; }
        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_TotalToBePaid { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }
        public long FSS_ExcessPaidAmount { get; set; }
        public long FSS_ExcessAdjustedAmount { get; set; }
        public long FSS_RunningExcessAmount { get; set; }
        public long FSS_ConcessionAmount { get; set; }
        public long FSS_AdjustedAmount { get; set; }
        public long FSS_WaivedAmount { get; set; }
        public long FSS_RebateAmount { get; set; }
        public decimal FSS_FineAmount { get; set; }
        public long FSS_RefundAmount { get; set; }
        public long FSS_RefundAmountAdjusted { get; set; }
        public decimal FSS_NetAmount { get; set; }
        public bool FSS_ChequeBounceFlag { get; set; }
        public bool FSS_ArrearFlag { get; set; }
        public bool FSS_RefundOverFlag { get; set; }
        public bool FSS_ActiveFlag { get; set; }
        public long User_Id { get; set; }

        public long roleid { get; set; }
        public string ICW_FilePath { get; set; }
        public string messag { get; set; }

        public Array Staffmobileappprivileges { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime ASMAY_From_Date { get; set; }
        public DateTime ASMAY_To_Date { get; set; }
        public DateTime ASMAY_PreAdm_F_Date { get; set; }
        public DateTime ASMAY_PreAdm_T_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMAY_ActiveFlag { get; set; }
        public int ASMAY_Pre_ActiveFlag { get; set; }
        public DateTime ASMAY_Cut_Of_Date { get; set; }
        public bool Is_Active { get; set; }
        public string IHW_FilePath { get; set; }

        public string mobiledeviceid { get; set; }

        public int COEME_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public string COEME_SMSMessage { get; set; }
        public string COEME_MailSubject { get; set; }
        // public string COEME_MailBody { get; set; }
        public string COEME_MailHeader { get; set; }
        public string COEME_MailFooter { get; set; }
        public string COEME_Mail_Message { get; set; }
        public string COEME_MailHTMLTemplate { get; set; }
        public bool COEME_ActiveFlag { get; set; }

        public string AMST_FirstName { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array academicyearFeedata { get; set; }

        public Array academicyearAttendancedata { get; set; }
        public Array yearclsList { get; set; }

        public long TotalBalance { get; set; }
        public long Receivable { get; set; }
        public long Balance { get; set; }
        public long Paid { get; set; }
        public string type { get; set; }
        public Array studentfeedetails { get; set; }
        public Array getfeedetails { get; set; }
        public Array feeAnalysisList { get; set; }
        public Array yearlist { get; set; }
        public Array recnolist { get; set; }
        public long FYP_Id { get; set; }
        public string FYP_Receipt_No { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public Array getfeereceipt { get; set; }
        public Array attList { get; set; }
        public Array attyearlist { get; set; }
        public Array coeyearlist { get; set; }
        public Array coereportlist { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public Array currentyear { get; set; }
        public Array feecurrentyear { get; set; }
        public Array studetailslist { get; set; }
        public Array sportsdetails { get; set; }
        public Array librarydetails { get; set; }
        public int month { get; set; }

        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public Array getyear { get; set; }
        public Array getStudentTT { get; set; }


        //RD

        public long HRME_ID { get; set; }
        public Array sectionlist { get; set; }
        public Array specialheaddetails { get; set; }
        public Array specialheadlist { get; set; }



        public long studentaccyear { get; set; }


        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public long? AMST_PerState { get; set; }
        public long? AMST_PerCountry { get; set; }
        public int? AMST_PerPincode { get; set; }

        public Array receiptformathead { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public decimal FTP_Concession_Amt { get; set; }
        public decimal FTP_Paid_Amt { get; set; }
        public decimal FTP_Fine_Amt { get; set; }
        public Array currpaymentdetails { get; set; }
        public DateTime FYP_Date { get; set; }

        public DateTime FYP_DD_Cheque_Date { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Remarks { get; set; }
        public Array fillstudentviewdetails { get; set; }
        public Array termremarks { get; set; }
        public Array filltotaldetails { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Fine_Amt { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public long FMT_Id { get; set; }
        public long dueamount { get; set; }
        public string date { get; set; }
        public List<FeetransactionSMS> Due_Date_array { get; set; }
        public Array fillacclst { get; set; }
        public Array showstudetails { get; set; }
        public Array alldata { get; set; }
        //stu TT
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        public Array Break_list { get; set; }
        public StudentDashboardDTO[] classarray { get; set; }
        public StudentDashboardDTO[] sectionarray { get; set; }
        public StudentDashboardDTO[] Time_Table { get; set; }
        public long TTMC_Id { get; set; }

        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string staffName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public decimal TTMB_AfterPeriod { get; set; }
        public Array Break_list_all { get; set; }
        public StudentDashboardDTO[] TT { get; set; }
        public StudentDashboardDTO[] TT_Break_list { get; set; }
        public Array TT_Break_list_all { get; set; }

        //---------
        public long IDU_Id { get; set; }
        public string IDU_Type { get; set; }
        public string IDU_Remarks { get; set; }
        public string IDU_Attachment { get; set; }
        public bool IDU_ActiveFlag { get; set; }

        public Array docs_Details { get; set; }
        public Array noticelist { get; set; }
        public Array assignmentlist { get; set; }
        public long INTB_Id { get; set; }
        public long ICW_Id { get; set; }
        public Array calenderlist { get; set; }
        public Array imgs_list { get; set; }


        public Array notice_details { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public DateTime? IHW_Date { get; set; }
        public string IHW_Topic { get; set; }
        public string IHW_Assignment { get; set; }
        public string IHW_Attachment { get; set; }
        public long IVRMUL_Id { get; set; }
        public long IHW_Id { get; set; }
        public long IHW_AssignmentNo { get; set; }
        public Array homeworklist { get; set; }
        public Array hwdisplylist { get; set; }
        public Array assignmentdislst { get; set; }
        public Array promotionstatus { get; set; }

        public string ICW_Topic { get; set; }
        public string ICW_SubTopic { get; set; }
        public string ICW_Content { get; set; }
        public bool ICW_ActiveFlag { get; set; }
        public DateTime ICW_FromDate { get; set; }
        public DateTime ICW_ToDate { get; set; }
        public string ICW_Assignment { get; set; }
        public string ICW_Evaluation { get; set; }
        public string ICW_Attachment { get; set; }
        public Array pushnotification { get; set; }


        public long IPN_Id { get; set; }
        public string IPN_No { get; set; }
        public string IPN_StuStaffFlg { get; set; }
        public DateTime IPN_Date { get; set; }
        public string IPN_PushNotification { get; set; }
        public bool IPN_ActiveFlag { get; set; }
        public long IPNS_Id { get; set; }



        public long IVRMMAD_Id { get; set; }       
        public string IVRMMAD_MobileModel { get; set; }
        public string IVRMMAD_DownlaodDateTime { get; set; }
        public bool returnval { get; set; }

        public Array swimmingatt { get; set; }
        public Array targetgatt { get; set; }



    }
}