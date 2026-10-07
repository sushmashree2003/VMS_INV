using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class Lead_Target_MappingDTO
    {

        public long User_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLETARM_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMSMOUA_Id { get; set; }
        public Array ModuleName { get; set; }
        public Array ModuleName1 { get; set; }
        public long ISMSMOU_Id { get; set; }

        public long ISMSLE_Id { get; set; }
        public long ISPSIF_Id { get; set; }

        public long ISMSLBOM_Id { get; set; }

        public bool ISMSLETARM_LeadApplFlg { get; set; }

        public bool ISMSLETARM_DemoApplFlg { get; set; }

        public string ISMSLBOM_BOMFlg { get; set; }

        public Array leadloaddata { get; set; }
        public long ISMSLETARM_NoOfLeads { get; set; }

        public long ISMSLETARM_NoOfDemos { get; set; }

        //added by praveenkumar 
        public Array Signedmoudetails { get; set; }
       public long  installment { get; set; }
        public long Duration { get; set; }
        public long ASMAY_ID { get; set; }
        public Array Paymentterms { get; set; }
        public string ISMSLETARM_LeadPeriodicity { get; set; }

        public string ISMSLBOM_BOMName { get; set; }
        public string ISMSLBOM_BOMDescription { get; set; }

        public long ISMSMOU_Studentstrength { get; set; }
        public decimal ISMSMOU_Amount { get; set; }
        public string ISMSLETARM_DemoPeriodicity { get; set; }

        public Array Orderacceptanceapproval { get; set; }

        public string ISMSMOU_Packagename { get; set; }
        public SCINCE_Park[] SCINCE_Parks { get; set; }
        public Robotic_Lab[] Robotic_Labs { get; set; }
        
        public Array leadnegotiation_Approvaldetails { get; set; }
        public long?[] product { get; set; }
        public long?[] BOMListt { get; set; }
        public Array MouApproval { get; set; }
        public Array leadlist { get; set; }
        public Array SendQuotationleadlist { get; set; }
        public Array BOMList { get; set; }

        public Array BOMTemplateList { get; set; }

        public Array leadnegotiation_details { get; set; }
        public Array mouattachmentsdeatils { get; set; }
        public Array leadloaddata1 { get; set; }

        public bomListdetailss[] bomListdetails { get; set; }

        public string Templates { get; set; }
        public string msg { get; set; }
        public string return_value { get; set; }
        public bool? ISMSLE_QuotationSent { get; set; }

        public Array mousavedeatils { get; set; }

        public leadlistarrays[] leadlistarray { get; set; }

        public ISMSMOU_Idarray[] ISMSMOU_Ids { get; set; }

        public decimal? ISMSLET_Cost { get; set; }
        public decimal? ISMSLET_CostApproved { get; set; }

        public  ISM_Sales_Lead_SciencePark_Boot[]  SciencePark_Boot { get; set; }
        public bool ISMSLETARM_ProposalApplFlg { get; set; }

        public long ISMSLETARM_NoOfProposals { get; set; }

        public string ISMSLETARM_ProposalPeriodicity { get; set; }

        public string Template { get; set; }
        public Array bomfinallist { get; set; }

        public Array leadnegotiation { get; set; }

        public Array Sales_CostPerMonth { get; set; }

        public bool ISMSLETARM_ClosureApplFlg { get; set; }

        public long ISMSLETARM_NoOfClosures { get; set; }

        public string ISMSLETARM_ClosurePeriodicity { get; set; }

        public bool ISMSLETARM_NegotiationFlg { get; set; }

        public decimal ISMSLETARM_NegotiationPercentage { get; set; }

        public long ISMSLETARM_ApprovedBy { get; set; }
        public string retrunMsg { get; set; }
        public string ISMSMOUA_ProductName { get; set; }

        public Array approvedBy { get; set; }
        public Array perstudent_QutationReq { get; set; }

        public Array getInstitution_lst { get; set; }

        public Array getLeadtarget_Mapping { get; set; }

        public Array editLeadTarget_Mapping { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Array leadtargetMapping_lts { get; set; }

        public long ISMSLETARM_CreatedBy { get; set; }

        public long ISMSLETARM_UpdatedBy { get; set; }

        public bool ISMS_NegotiationFlag { get; set; }
        public bool ISMS_MOMFlag { get; set; }

        public bool ISMS_NegoMDApprovedFlag { get; set; }

        public decimal? Negotiation_Amount { get; set; }

        public DateTime? ISMSLETARM_CreatedDate { get; set; }

        public DateTime? ISMSLETARM_UpdatedDate { get; set; }

        public bool ISMSLETARM_ActiveFlag { get; set; }
        public string ManagerMailid { get; set; }

      
        public Array Mompendinglist { get; set; }
        public Array leadprojectpayment { get; set; }

        public Array Momproductpendinglist { get; set; }
        public Array Momproductpendinglist1 { get; set; }

        public attachmentArray1[] attachmentArray { get; set; }

        public ISM_Sales_Lead_MOU_Attachments1[] ISM_Sales_Lead_MOU_Attachments { get; set; }
        public string retrunwhatsup { get; set; }
        public string ISMSLED_MobileNo { get; set; }


        public Array yearlist { get; set; }
        public Array getinstallment { get; set; }

        //new added
        public Array selectedleadlist { get; set; }
        public Array leadTempDetails { get; set; }
        public Array leadApproval { get; set; }
        public Array editleadtemp { get; set; }
        public Array productlist { get; set; }
        public Array confirmList { get; set; }
        public Lead_Target_MappingDTO[] approvTempList  { get; set; }
        public ISM_Client_Project_Payment_Lead[] Project_Payment_Lead { get; set; }
        public long ISMSLE_StudentStrength { get; set; }
        
        public long ISMSLET_Id { get; set; }
        public string ISMSLET_HtmlTemplate { get; set; }
        public string ISMSLET_HtmlPdf { get; set; }
        public string ISMSLET_HtmlName { get; set; }
        public string ISMSLET_ApprovedRemark { get; set; }
        public string ISMSLET_Amount_I { get; set; }
        public string ISMSLET_Amount_Final { get; set; }       
        public DateTime? ISMSLET_CreatedDate { get; set; }
        public DateTime? ISMSLET_UpdatedDate { get; set; }
        public long ISMSLET_CreatedBy { get; set; }
        public long ISMSLET_UpdatedBy { get; set; }
        public string flag { get; set; }
        public string preAmount_I { get; set; }
        public string PreAmount_Final { get; set; }
        public string ISMSLET_Amount_IMD { get; set; }
        public string ISMSLET_Amount_FinalMD { get; set; }

        public goldandsilver[] goldandsilverData { get; set; }
        public goldandsilverYrwise[] goldandsilverYrwiseData { get; set; }



        public SalesMultipleTransaction[] ISMSalesMultipleTransaction { get; set; }




        public class goldandsilver
        {
            public string IMSV_TotalCostPerQty { get; set; }
            public long? ISMSBOM_Id { get; set; }
            public long? ISMSLE_Id { get; set; }
            public string ISMSPSI_ItemFlag { get; set; }
            public string ISMSPSI_ItemsDescription { get; set; }
            public string ISPSIF_AmountScincePark { get; set; }
            public string ISPSIF_AmtScincePark { get; set; }
            public string ISPSIF_Cost { get; set; }
            public string ISPSIF_CostNew { get; set; }
            public long? ISPSIF_Id { get; set; }
            public string ISPSIF_ItemsName { get; set; }
            public string ISPSIF_Quantity { get; set; }
            public string ISPSIF_Quantitys { get; set; }
            public string ScienceParkFlag { get; set; }
        }
        public class goldandsilverYrwise
        {
            public string Cost { get; set; }
            public string FifthYear { get; set; }
            public string FifthYearNew { get; set; }
            public string FourYear { get; set; }
            public string FourYearNew { get; set; }
            public string ThirdYear { get; set; }
            public string ThirdYearNew { get; set; }
            public string SecondYear { get; set; }
            public string SecondYearNew { get; set; }
            public string FirstYear { get; set; }
            public string FirstYearNew { get; set; }
            public long? ISMSBOM_Id { get; set; }
            public long? ISMSLE_Id { get; set; }
            public string ItemName { get; set; }
            public string PakageType { get; set; }
            public string ScienceParkFlag { get; set; }

        }
        public class bomListdetailss
        {
            public long ISPSIF_Id { get; set; }
            public long ISMSMPR_Id { get; set; }
            public long ISMSBOM_Id { get; set; }
            public long ISMSPS_ID { get; set; }
            public string ISPSIF_ItemsName { get; set; }
            public decimal ISPSIF_Quantity { get; set; }
            public decimal ISPSIF_Cost { get; set; }
            public long  ISMSLE_Id { get; set; }
            public string ISMSMPR_ProductName { get; set; }
        }

        public class ISM_Sales_Lead_MOU_Attachments1
        {
           public long ISMSMOU_Id { get; set; }
            public string ISMSMOUA_AttachmentName { get; set; }
           public string ISMSMOUA_AttachmentPath { get; set; }
   
        }
        public class leadlistarrays
        {
            public long ISMSLE_Id { get; set; }
            public bool ISMS_NegotiationFlag { get; set; }
            public bool ISMS_MOMFlag { get; set; }
            public string Negotiation_Client { get; set; }
            public decimal? Negotiation_Amount { get; set; }
            public long SanctionLevelNo { get; set; }
            public string ISMSALENA_Remarks { get; set; }
            public bool HRPAON_FinalFlg { get; set; }
            public ISM_Client_Project_Payment_Lead[] Project_Payment_Lead { get; set; }
            public long ISMSLET_Id { get; set; }
            public string ISMSLET_HtmlTemplate { get; set; }
            public string ISMSLET_HtmlName { get; set; }
        }
        public class ISM_Client_Project_Payment_Lead
        {
            public long ISMCLTPRPL_Id { get; set; }
            public long ISMSLE_Id { get; set; }
            public long ISMCLTPRP_Year { get; set; }
            public string ISMCLTPRP_InstallmentName { get; set; }
            public DateTime? ISMCLTPRP_PaymentDate { get; set; }
            public decimal? ISMCLTPRP_InstallmentAmt { get; set; }
            public bool ISMCLTPRP_ActiveFlag { get; set; }
            public long ISMCLTPRP_CreatedBy { get; set; }
            public long ISMCLTPRP_UpdatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public long FTI_Id { get; set; }
            public string ISMCLTPRP_PaymentStatus { get; set; }
            public decimal? ISMCLTPRP_BalanceAmt { get; set; }
            public decimal? ISMCLTPRP_ExcessAmt { get; set; }
            public string itrS_Id { get; set; }
            public long FMI_Id { get; set; }
            public decimal ISMCLTPRPL_Per { get; set; }
        }
        public class ISMSMOU_Idarray
        {
            public long ISMSMOU_Id { get; set; }


        }
        public class ISM_Sales_Lead_SciencePark_Boot
        {
            public long ISMLESPB_Id { get; set; }
            public long ISMSLET_Id { get; set; }
            public string ISMLESPB_First_Year { get; set; }
            public string ISMLESPB_Second_Year { get; set; }
            public string ISMLESPB_Third_Year { get; set; }
            public string ISMLESPB_Fourth_Year { get; set; }
            public string ISMLESPB_Fifth_Year { get; set; }
            public bool ISMLESPB_ActiveFlag { get; set; }
            public string ISMLESPB_Flag { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public long CreatedBy { get; set; }
            public long UpdatedBy { get; set; }
            public string ISMLESPB_FlagName { get; set; }

        }
        public class SCINCE_Park
        {
            public string ISPSIF_ItemsName { get; set; }
            public decimal? ISPSIF_Quantity { get; set; }
            public decimal? ISPSIF_Cost { get; set; }
            public long ISPSIF_Id { get; set; }

        }
        public class Robotic_Lab
        {
            public string ISPSIF_ItemsName { get; set; }
            public decimal? ISPSIF_Quantity { get; set; }
            public decimal? ISPSIF_Cost { get; set; }
            public long ISPSIF_Id { get; set; }

        }
        public class attachmentArray1
        {
            public string IVRM_Att_Name { get; set; }
            public string IVRM_Att_Path { get; set; }
        }

        public class SalesMultipleTransaction
        {
            public long? ISMSBOM_Id { get; set; }
            public long? ISMSLE_Id { get; set; }
            public string ISPSIF_ItemsName { get; set; }
            public string ISMSPSI_ItemsName { get; set; }
            public long? ISMSPSI_ID { get; set; }
            public string ISMSPSI_ItemFlag { get; set; }
            public string ISMSPSI_ItemsDescription { get; set; }
            public long? ISPSIF_Id { get; set; }
            public decimal? ISPSIF_CostAppMd { get; set; }
            public decimal? ISPSIF_CostOriginal { get; set; }
            public long? ISMSLET_Id { get; set; }
            public decimal? ISPSIF_Quantity { get; set; }
            public decimal? ISPSIF_Quantitys { get; set; }
            public decimal? TotalCost { get; set; }
            public string ISMSBOM_Name { get; set; }
        }

    }
}
