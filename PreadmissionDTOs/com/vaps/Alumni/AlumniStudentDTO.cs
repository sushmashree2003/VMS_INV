using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class AlumniStudentDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }

        public Array fillyear { get; set; }

        public Array fillclass { get; set; }

        public long ASMAY_Id { get; set; }

        public long? ALMST_Id { get; set; }

        public string ASMAY_Year { get; set; }

        public long IVRMUL_Id { get; set; }

        public Array batch { get; set; }

        public bool alumnitrue { get; set; }

        public long? AMST_ID { get; set; }

        public Array alumnilist { get; set; }

        public Array calenderlist { get; set; }

        public Array yearwiselist { get; set; }

        public Array classwiselist { get; set; }

        public int count { get; set; }

        public long ASMAY_Id_Left { get; set; }

        public long ASMAY_Id_Join { get; set; }

        public string ALMST_FirstName { get; set; }

        public string ALMST_MiddleName { get; set; }

        public string ALMST_LastName { get; set; }

        public string rolename { get; set; }

        public long ALSREG_Id { get; set; }

        public Array stu_name { get; set; }

        public Array fillstudent { get; set; }

        public string AMST_FirstName { get; set; }

        public string searchfilter { get; set; }

        public long roleId { get; set; }

        public long userid  { get; set; }

        public Array studentDetails { get; set; }

        public Array memebersetails { get; set; }

        public string AMST_CLASS_LEFT { get; set; }

        public string AMST_CLASS_JOIN { get; set; }

        public string AMST_JOIN_YEAR { get; set; }

        public string AMST_JOIN_LEFT { get; set; }

        public long ASMCL_Id_Left { get; set; }

        public long ASMCL_Id_Join { get; set; }
        public long ASMCL_Id { get; set; }

        public string ALMST_AdmNo { get; set; }

        public long? ALMST_MobileNo { get; set; }

        public string ALMST_emailId { get; set; }

        public string ALMST_PerStreet { get; set; }

        public string ALMST_PerArea { get; set; }

        public long? ALMST_PerCountry { get; set; }

        public string ALMST_PerCity { get; set; }

        public DateTime? ALMST_DOB { get; set; }

        public string ALMST_FatherName { get; set; }

        public string membername { get; set; }

        public string classnameadmitted { get; set; }

        public string classnameleft { get; set; }

        public string yeardmitted { get; set; }

        public string yearleft { get; set; }

        public long? ALMST_PerPincode { get; set; }


        public long? ALMST_PerState { get; set; }



        public Array countryDrpDown { get; set; }

        public Array statedropdown { get; set; }


        public long ALMST_DETAILS_ID { get; set; }
        public string ALMST_PUC_QS_DETAILS { get; set; }
        public string ALMST_PUC_INS_NAME { get; set; }
        public string ALMST_PUC_PASSED_OUT { get; set; }
        public string ALMST_PUC_PERCENTAGE { get; set; }
        public string ALMST_PUC_PLACE { get; set; }
        public string ALMST_PUC_STATE { get; set; }
        public string ALMST_UG_QS_DETAILS { get; set; }
        public string ALMST_UG_INS_NAME { get; set; }
        public string ALMST_UG_PASSED_OUT { get; set; }
        public string ALMST_UG_PERCENTAGE { get; set; }
        public string ALMST_UG_PLACE { get; set; }
        public string ALMST_UG_STATE { get; set; }
        public string ALMST_PG_QS_DETAILS { get; set; }

        public string ALMST_PG_INS_NAME { get; set; }
        public string ALMST_PG_PASSED_OUT { get; set; }
        public string ALMST_PG_PERCENTAGE { get; set; }
        public string ALMST_PG_PLACE { get; set; }
        public string ALMST_PG_STATE { get; set; }
        public string ALMST_ACH_DET { get; set; }
        public string ALMST_ACH_REMARKS { get; set; }
        public string ALMST_PRO_COMPANY_NAME { get; set; }

        public string ALMST_PRO_DESIGNATION { get; set; }
        public string ALMST_PRO_OFFICE_NO { get; set; }
        public string ALMST_PRO_ADDRESS { get; set; }
        public string ALMST_PRO_REMARKS { get; set; }

        public Array saveddata { get; set; }

        public string returnval { get; set; }


        public int ALMST_AmountPaid { get; set; }
        public string ALMST_AppDownloadedDeviceId { get; set; }
        public string ALMST_ApplStatus { get; set; }
        public int ALMST_BPLCardFlag { get; set; }
        public string ALMST_BPLCardNo { get; set; }
        public string ALMST_ConAdd3 { get; set; }
        public long ALMST_ConCountryId { get; set; }
        public long ALMST_CreatedBy { get; set; }
        public string ALMST_District { get; set; }
        public string ALMST_DOBinwords { get; set; }
        public int ALMST_ECSFlag { get; set; }
        public string ALMST_EMSINo { get; set; }
        public string ALMST_FatherBankIFSCCode { get; set; }
        public string ALMST_FatherFingerprint { get; set; }
        public long ALMST_FatherPANCardNo { get; set; }
        public string ALMST_FatherSign { get; set; }
        public bool ALMST_FinalpaymentFlag { get; set; }
        public long ALMST_GPSTrackingId { get; set; }
        public int ALMST_GymReqdFlag { get; set; }
        public string ALMST_HomeLaguage { get; set; }
        public int ALMST_HostelReqdFlag { get; set; }
        public string ALMST_MOInstruction { get; set; }
        public string ALMST_MotherBankIFSCCode { get; set; }
        public string ALMST_MotherFingerprint { get; set; }
        public long ALMST_MotherMobleNo { get; set; }
        public string ALMST_MotherPANCardNo { get; set; }
        public string ALMST_MotherSign { get; set; }
        public int ALMST_NoOfBrothers { get; set; }
        public int ALMST_NoofDependencies { get; set; }
        public int ALMST_NoofSiblings { get; set; }
        public int ALMST_NoofSiblingsSchool { get; set; }
        public int ALMST_NoOfSisters { get; set; }
        public DateTime? ALMST_PaymentDate { get; set; }
        public int ALMST_PaymentFlag { get; set; }
        public string ALMST_PaymentType { get; set; }
        public string ALMST_ReceiptNo { get; set; }
        public string ALMST_StuBankIFSCCode { get; set; }
        public string ALMST_StudentFingerprint { get; set; }
        public long ALMST_StudentPANCard { get; set; }
        public string ALMST_StudentPhoto { get; set; }
        public string ALMST_StudentSign { get; set; }
        public long ALMST_StudentSubCaste { get; set; }
        public string ALMST_Taluk { get; set; }
        public string ALMST_TPINNO { get; set; }
        public int ALMST_TransportReqdFlag { get; set; }
        public long ALMST_UpdatedBy { get; set; }
        public string ALMST_Village { get; set; }
        public long IVRMMB_Id { get; set; }
        public long IVRMMC_Id { get; set; }

        public string ALMST_Marital_Status { get; set; }

        public long? ALMST_PhoneNo { get; set; }

        public AlumniApproveStudentDTO approvalarray { get; set; }

        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }

        public Array alumnibirthday { get; set; }
    }

    public class AlumniApproveStudentDTO  
    {
        public long ALSREG_Id { get; set; }
    }
}



