using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CollegeFeeTransactionDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long RoleId { get; set; }
        public Array feeconfiglist { get; set; }
        public Array fillmastergroup { get; set; }
        public Array transnumconfig { get; set; }
        public Array fillstudent { get; set; }
        public string rolename { get; set; }
        public long FYP_Id { get; set; }
        public string filterinitialdata { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMCST_FatherName { get; set; }
        public DateTime? AMCST_DOB { get; set; }
        public long AMCST_MobileNo { get; set; }
        public string validationvalue { get; set; }
        public string searchfilter { get; set; }
        public string returnval { get; set; }
        public string configset { get; set; }
        public Array disableterms { get; set; }
        public string FMT_Name { get; set; }
        public long FMT_Id { get; set; }

        public string FMG_GroupName { get; set; }
        public long FMG_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public long FMH_Id { get; set; }
        public string FTI_Name { get; set; }
        public long FTI_Id { get; set; }
        public Array fillstudentviewdetails { get; set; }
        public Array showstudetails { get; set; }
        public Array showstaticticsdetails { get; set; }
        public string FYP_ReceiptNo { get; set; }
        public decimal FTCP_PaidAmount { get; set; }
        public decimal FTCP_ConcessionAmount { get; set; }
        public decimal FTCP_FineAmount { get; set; }
        public DateTime FYP_ReceiptDate { get; set; }
        public string FYP_PayModeType { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public DateTime FYP_DD_Cheque_Date { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Remarks { get; set; }
        public long FCSS_CurrentYrCharges { get; set; }
        public long FCSS_ToBePaid { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public long FCSS_NetAmount { get; set; }
        public string FYP_ChallanNo { get; set; }
        public bool Challan_Flag { get; set; }
        public Array alldata { get; set; }
        public long FCMAS_Id { get; set; }
        public string FMH_Flag { get; set; }
        public long FCSS_ConcessionAmount { get; set; }
        public long FCSS_FineAmount { get; set; }
        public long FCSS_WaivedAmount { get; set; }
        public long FCSS_TotalCharges { get; set; }
        public int Fine_Amt { get; set; }
        public Array Fine_FCMAS_Ids { get; set; }
        public string multiplegroups { get; set; }
        public long[] terms_groups { get; set; }
        public Array instalspecial { get; set; }
        public long FCSS_RefundAmount { get; set; }
        public long FCSS_OBArrearAmount { get; set; }
        public int FMH_Order { get; set; }
        public int grp_count { get; set; }
        public long validationgroupid { get; set; }
        public long validationgrougidcount { get; set; }
        public string FYP_Chq_Bounce { get; set; }
        public string automanualreceiptno { get; set; }
        public int auto_receipt_flag { get; set; }
        public Temp_Savedata[] savetmpdata { get; set; }
        public Temp_Savedata[] temp_head_list { get; set; }
        public decimal FYP_TotalPaidAmount { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal FYP_TotalFineAmount { get; set; }
        public string displaymessage { get; set; }
        public Array fillyear { get; set; }
        public Array receiparraydelete { get; set; }
        public string FGAR_PrefixName { get; set; }
        public string FGAR_SuffixName { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public Temp_ModeDTO[] Modes { get; set; }
        public Array specialheaddetails { get; set; }
        public Array specialheadlist { get; set; }
        public string htmldata { get; set; }
        public string minstall { get; set; }
        public Array receiptformathead { get; set; }
        public Array termremarks { get; set; }
        public Array currpaymentdetails { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }
        public Array searcharray { get; set; }
        public Array filltotaldetails { get; set; }
        public long dueamount { get; set; }
        public string date { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public Array duedetails { get; set; }
        public string duration { get; set; }
        public string enddate { get; set; }
        public string AMCST_MotherName { get; set; }
        public decimal FTCP_WaivedAmount { get; set; }
        public Array masterinstitution { get; set; }
        public Array duplicatereceipt { get; set; }
        public Array paymentmode_details { get; set; }
        public int? FMG_Order { get; set; }
        public int ASMAY_Order { get; set; }
        
        public string FMC_Online_Payment_Aca_Yr_Flag { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int AMSE_SEMOrder { get; set; }

        public Array filonlinepaymentgrid { get; set; }
        public long ACMS_Id { get; set; }
        public Array fillgroup { get; set; }
        public string AMCST_emailId { get; set; }

        public string groupfmgidss { get; set; }

        public Array fillpaymentgateway { get; set; }

        public long FPGD_Id { get; set; }
        public string FPGD_PGName { get; set; }

        public string FPGD_Image { get; set; }
        public string onlinepaygteway { get; set; }

        public Array paydet { get; set; }

        public long topayamount { get; set; }

        public CollegeFeeTransactionDTO[] selected_list { get; set; }

        public string merchantid { get; set; }
        public long FCSS_RebateAmount { get; set; }
        public long pendingamount { get; set; }

        public string trans_id { get; set; }
        public string grpidss { get; set; }

        public Array recenocol { get; set; }
        public long FGAR_Id { get; set; }

        public string FYP_Receipt_No { get; set; }

        public long FMHOT_Id { get; set; }
        public decimal FMA_Amount { get; set; }
        public long enduserid { get; set; }
    }
    public class Temp_Savedata
    {                                               
        public long FCMAS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public long FMG_Id { get; set; }
        public string FMH_Flag { get; set; }
        public string FMH_FeeName { get; set; }
        public long FMH_Id { get; set; }
        public string FTI_Name { get; set; }
        public long FTI_Id { get; set; }
        public long FCSS_NetAmount { get; set; }
        public long FCSS_ToBePaid { get; set; }
        public long FCSS_ConcessionAmount { get; set; }
        public long FCSS_FineAmount { get; set; }
        public long FCSS_CurrentYrCharges { get; set; }
        public long FCSS_TotalCharges { get; set; }
        public long FCSS_TotalToBePaidaddfine { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public long FCSS_RefundAmount { get; set; }
        public long FCSS_OBArrearAmount { get; set; }
        public long FCSS_WaivedAmount { get; set; }
        public int FMH_Order { get; set; }      
    }
    public class Temp_ModeDTO
    {
        public string FYPPM_TransactionTypeFlag { get; set; }
        public decimal FYPPM_TotalPaidAmount { get; set; }
        public string FYPPM_BankName { get; set; }
        public string FYPPM_DDChequeNo { get; set; }
        public DateTime FYPPM_DDChequeDate { get; set; }

    }
 
}
