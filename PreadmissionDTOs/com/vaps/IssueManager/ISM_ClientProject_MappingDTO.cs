using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_ClientProject_MappingDTO
    {
        public long ISMMCLTPR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMMMD_Id { get; set; }     
        public long ISMMPR_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }
        public bool ISMMCLTPR_ActiveFlag { get; set; }
        public bool Module_ActiveFlag { get; set; }
        public long ISMMCLTPR_CreatedBy { get; set; }
        public long ISMMCLTPR_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public bool Active_Flag { get; set; }
        public Array client_list { get; set; }
        public Array Attendancelist { get; set; }
        public Array Invoicedata { get; set; }
        public Array Telecallerlist { get; set; }
        public Array project_list { get; set; }
        public Array module_list { get; set; }
        public Array patchlist { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string getproject { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public long IVRMVT_ID { get; set; }
        
        public Array Sms_Quantity { get; set; }
        public Array Offt_letter { get; set; }
        public Array Onnt_letter { get; set; }
        public Array Icc_letter { get; set; }
        public Array Replacement_data { get; set; }
        public Array Custamization_report { get; set; }
        public Array Components_docs { get; set; }
       // public long UserID { get; set; }
       public long ISMCLTPRDOC_Id { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public Array ModuleName { get; set; }
        public string Flag { get; set; }
        public long Ayear { get; set; }
        public Array closing_letter { get; set; }
        public Array versionreleasedata { get; set; }
        public Array leadprojectpayment { get; set; }
        public Array Momproductpendinglist { get; set; }
        public Array Momproductpendinglist1 { get; set; }
        public Array EnduserDetails { get; set; }
        public Array Service_System { get; set; }
        //added by praveenkumar
        public Array ProposalTempleate { get; set; }
        public Array Month_yearreprot { get; set; }
        public Array Custamization { get; set; }
        public Array It_HealthCheckup { get; set; }
        public Array Man_power { get; set; }
        public Array Feedback_System { get; set; }
        public Array Payment { get; set; }
        public Array Mou_details { get; set; }
        public Array Patch_release { get; set; }
        //Start additional data for post sales 
        public Array getviewfilesdetails { get; set; }
        public string ISMMCLT_Contract_YearFrom { get; set; }
        public string ISMMCLT_Contract_YearTo { get; set; }
        public string Contract_Year_To { get; set; }
        public string Contract_Year_From { get; set; }
        public string ISMMCLTPR_OperatorCost { get; set; }
        public string ISMMCLTPR_IEcharge { get; set; }
        public string ISMMCLTPR_Percentage { get; set; }
        public string ISMMCLT_Host { get; set; }
        //end additional data for post sales 
        public string MOU_subject { get; set; }
        public string Template { get; set; }
        public Array Approvals { get; set; }
        public Array editlist { get; set; }
        public ISM_ClientProject_MappingDTO[] moduleidss { get; set; }
        public Array get_clentlist { get; set; }
        public Array get_projectdetail { get; set; }
        public Array get_department { get; set; }
        public bool returnval { get; set; }
        public long projectid { get; set; }
        public bool OrderConfirm_Flg { get; set; }

        public string ISMOAA_ApprovalFlag { get; set; }
        public string ISMOAA_Manpower { get; set; }

        //public bool ISMOAA_ApprovalFlag { get; set; }
        public long ISMSLET_Id { get; set; }
        public string ISMMCLTPR_ProposalRefNo { get; set; }
        public DateTime? ISMMCLTPR_ProposalSentDate { get; set; }
        public DateTime? ISMMCLTPR_DealClosureDate { get; set; }
        public string ISMMCLTPR_MOURefNo { get; set; }
        public DateTime? ISMMCLTPR_MOUDate { get; set; }
        public long? ISMMCLTPR_MOURepresentedBy { get; set; }
        public DateTime? ISMMCLTPR_MOUStartDate { get; set; }
        public DateTime? ISMMCLTPR_MOUEndDate { get; set; }
        public string ISMMCLTPR_NodalOfficerName { get; set; }
        public long? ISMMCLTPR_NodalOfficerContactNo { get; set; }
        public string ISMMCLTPR_NodalOfficerEmailId { get; set; }
        public string ISMMCLTPR_ProjectDuration { get; set; }
        public long? ISMMCLTPR_TotalStudent { get; set; }
        public decimal? ISMMCLTPR_CostPerStudent { get; set; }
        public decimal? ISMMCLTPR_EnhancementPerYr { get; set; }
        public string ISMMCLTPR_Image { get; set; }
        public string ISMMCLTPR_WorkOrder { get; set; }
        public long?[] BOMListt { get; set; }
        //MOU template
        public long? ISMSBOM_Id { get; set; }
        public Array Sales_CostPerMonth { get; set; }
        public Array BOMTemplateList { get; set; }
        public string pending { get; set; }
        public Array templatelist { get; set; }
        public pocolumns[] columnids { get; set; }
        public lead[] arrayLead { get; set; }
        public decimal? ISMMCLTPR_CostPerYearStudent { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public long ISMSLE_Id { get; set; }


        public Array Welcomebasicdata { get; set; }
        public Array WelcomeImplementation { get; set; }
        public Array WelcomeNonImplementation { get; set; }
        public Array WelcomePatchrelease { get; set; }
        public Array WelcomeHealthCheckup { get; set; }
        public Array WelcomeManPower { get; set; }
        public Array WelcomeAmount { get; set; }
        public Array WelcomeSms { get; set; }
        public Array WelcomeOfflinetraining { get; set; }

        public Array WelcomeOnlinetraining { get; set; }

        public Array WelcomeCustom { get; set; }
        public Array WelcomeIENAME { get; set; }




    }
    public class pocolumns
    {
        public long ID { get; set; }
        public string coulmn { get; set; }
        public string title { get; set; }

    }

    public class lead
    {
       public long ISMMCLT_Id { get; set; }
        public string OrderConfirm_Flg { get; set; }
        public string ISMOAA_ApprovalFlag { get; set; }
        public long ISMCL_ID { get; set; }
        public string ISMOC__Remark { get; set; }
        public string ISMOAA_Manpower { get; set; }
    }

}
