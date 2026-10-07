using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Client_Project_DTO
    {

        public long ISMPEE_Id { get; set; }

        public long ISMPEE_Enhancement { get; set; }
        public long ISMPEE_AcademicYear { get; set; }
        public bool ISMPEE_ActiveFlag { get; set; }
        public long ISMPEE_Studentstrength { get; set; }
        public Array save_elearning { get; set; }


        public long ISMSMPR_Id { get; set; }
        public long ISMACC_Id { get; set; }
        public long ISMMP_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public Array welcomeNonImplementation { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMHCA_Id { get; set; }
        public bool ISMHCA_ActiveFlag { get; set; }

        public DateTime ISMMP_Date { get; set; }
        public long ISMMP_AcademicYear { get; set; }

        //added
        public long?[] cplists { get; set; }
        public Array ChannelParter_lead_Reportonloadregion { get; set; }
        public Array ChannelParter_lead_Reportonloadstatus { get; set; }
        public Array ChannelParter_lead_ReportonloadCP{ get; set; }

        public Array Ism_Sales_ChannelParter_lead_Report { get; set; }

        public Array Ism_Sales_ChannelParterReport { get; set; }

        public Array ReffralLeadReport { get; set; }


        public long month { get; set; }
        public string fromdateNew { get; set; }
        public string todateNew { get; set; }


        public Array ReffralLeadReportonload { get; set; }

        public long ISMRLM_Id { get; set; }
        public Array referralDatasLoad { get; set; }
        public referraldata[] referralDatasNew { get; set; }

        public Array CallStatusData { get; set; }
        public Array ReferralClientData { get; set; }
        public Array SalesPortfolioReport { get; set; }
        public long?[] RegionlistLists { get; set; }
        public long?[] ClientAcquisitionlists { get; set; }
        public long?[] Telecallerlists { get; set; }
        public string[] Closurestatuss { get; set; }
        public string[] Revenues { get; set; }
        public long?[] BDElists { get; set; }

        public Array Regionlist { get; set; }
        public Array ClientAcquisitionlist { get; set; }
        public Array Telecallerlist { get; set; }
        public Array Closurestatus { get; set; }
        public Array Revenue { get; set; }

        public Array BDElist { get; set; }
        //added
        public long ISMHCA_AcademicYear { get; set; }

        public string ISMHCA_Quarter { get; set; }
        public DateTime ISMHCA_HealthCheckupDate { get; set; }
        public long ISMCLTPRBOM_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSMSO_Id { get; set; }
        public string searchleadvalue { get; set; }
        public string flag { get; set; }
        public long FlagType { get; set; }
        public string Status { get; set; }
        public long ISMCLTPRMDOC_Id { get; set; }
        public long ISMCLTPRDOC_Id { get; set; }
        public long ISMCLTPRMP_Id { get; set; }
        public long User_Id { get; set; }
        public long ISMCLTC_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public decimal ISMCLTPRMP_Qty { get; set; }
        public string ISMCLTPRBOM_Remarks { get; set; }
        public string ISMCLTPRMDOC_Name { get; set; }
        public long ISMCLTPRMP_AcademicYear { get; set; }
        public string ISMCLTPRMDOC_Description { get; set; }
        public string ISMCLTPRMP_ResourceName { get; set; }
        public string ISMCLTPRMP_Remarks { get; set; }
        public DateTime? DateofDeployment { get; set; }
        public DateTime? EndDate { get; set; }
        public string returndata { get; set; }
        public string ISMCLTC_Name { get; set; }
        public string ISMCLTC_Description { get; set; }
        public string ISMCLTPRDOC_FilePath { get; set; }
        public string ISMCLTPRDOC_FileName { get; set; }
        public bool ISMCLTPRBOM_ActiveFlag { get; set; }
        public bool ISMCLTPRMDOC_ActiveFlag { get; set; }
        public bool ISMCLTC_ActiveFlag { get; set; }
        public bool ISMCLTPRDOC_ActiveFlag { get; set; }
        public bool ISMCLTPRMP_ActiveFlag { get; set; }
        public long ISMCLTPRBOM_CreatedBy { get; set; }
        public long ISMCLTPRBOM_UpdatedBy { get; set; }
        public long ISMSLECOM_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLECOM_Comments { get; set; }
        public string message { get; set; }
        public string leadname { get; set; }

        public string ISMSMPR_ProductName { get; set; }
        public string ISMSLECOM_Date { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }

        public bool ISMSLECOM_ActiveFlag { get; set; }
        public string employeename { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime ISMSLECOM_CreatedDate { get; set; }
        public DateTime ISMCLTPRDOC_Date { get; set; }

        public string ISMSLE_FilePath { get; set; }
        public string logo { get; set; }
        public string StatusName { get; set; }
        public Array followuplead { get; set; }
        public Array components_list { get; set; }
        public Array components_details { get; set; }
        public Array bom_details { get; set; }
        public Array mp_details { get; set; }
        public Array doc_details { get; set; }
        public Array leadlist { get; set; }
        public Array companies { get; set; }
        public Array Institutions { get; set; }
        public Array leadcommentsreport { get; set; }
        public Array leadcommentsreportDeatils { get; set; }
        public Array Totalcommentreport { get; set; }
        public Array clientproject_dd { get; set; }
        public Array Master_Employee_List { get; set; }

        public Array bom_list { get; set; }
        public Array mp_list { get; set; }
        public Array doc_list { get; set; }
        public Array docmaster_list { get; set; }
        public Array docmaster_details { get; set; }
        public Array components_dd { get; set; }
        public Array document_dd { get; set; }
        public Array leadlist_dd { get; set; }
        public Array commentlist { get; set; }
        public Array edit_comment { get; set; }
        public leadlist1[] leaddatalist { get; set; }
        //added by roopa
        public filListTest[] filelist { get; set; }
        public Array editArryThree { get; set; }
        public bool ISMSLECOM_SendProposalFlg { get; set; }
        public bool? ISMSLECOM_SendQuotationFlg { get; set; }
        public DateTime? ISMSLECOM_NextReminderDate { get; set; }
        public DateTime ISMSLECOM_RemainderDate { get; set; }
        public long ISMSLECOM_RemainderHrs { get; set; }
        public string ISMSLECOM_RemainderTime { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array AcademicYearlist { get; set; }
        public Array clientprojectdata { get; set; }


        //Ganesh
        public string AYear { get; set; }
        public string BOMFlag { get; set; }
        public string DCNo { get; set; }
        public DateTime? BOMDate { get; set; }
        public string LearningID { get; set; }

        public Array sourcelist { get; set; }
        public Array statelist { get; set; }
        public Array employeedropdown { get; set; }
        public string ISMSMSO_SourceName { get; set; }
        public string MI_Name { get; set; }
        public long?[] hrmdes_IdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmE_IdList { get; set; }
        public long?[] MI_Id_IdList { get; set; }

        public long?[] source_IdList { get; set; }
        public long?[] state_IdList { get; set; }
        public Array yearlist { get; set; }
        public long IMFY_Id { get; set; }

        public Array leadIncharge { get; set; }
        public Array leadInchargetList { get; set; }

        public Array leaddatabasereport { get; set; }
        public Array leadquotationreport { get; set; }
        public Array leaddatabasereportdetails { get; set; }

        public Array leadQuotationrequestreportdetails { get; set; }
        public long HRME_Id { get; set; }

        public Array StatusUpdateOnLoad { get; set; }
        public Array StatusDropDownData { get; set; }

        public long ISMSLEDMPR_Id { get; set; }
        public long ISMSMST_Id { get; set; }
        public long ISMSLEDM_Id { get; set; }
        public bool ISMSLEDMPR_SendQuotationFlg { get; set; }

        public Array ClientDataOnload { get; set; }
        public Array ProductListOnload { get; set; }
        public Array StatusListOnload { get; set; }
        public Array ClientReport { get; set; }

        public Array Sales_Lead_RemarkReport { get; set; }
        public string ISMSLE_IdNew { get; set; }
        public string ISMSMPR_IdNew { get; set; }
        public string ISMSMST_IdNew { get; set; }

        public selectedDataArrayNew[] selectedDataArray { get; set; }
        public class selectedDataArrayNew
        {
            public long ISMSLEDMPR_Id { get; set; }
            public long ISMSMST_Id { get; set; }
            public long ISMSLEDM_Id { get; set; }
            public bool ISMSLEDMPR_SendQuotationFlg { get; set; }
        }
        //
        public class leadlist1
        {
            public long ISMSLE_Id { get; set; }
        }


    }
    //added by roopa
    public class filListTest
    {
        //  public long ISMSLECOM_Id { get; set; }
        public string ISMSLECF_FileName { get; set; }
        public string ISMSLECF_FileDesc { get; set; }
        public string ISMSLECF_FilePath { get; set; }
    }

    public class referraldata
    {
        public long? ISMMCLT_Id { get; set; }
        public string callstatus { get; set; }
        public string reffredpersonname { get; set; }
        public long? reffredpersonContactNumber { get; set; }
        public string reffredpersonEmail { get; set; }
        public string relationwithclient { get; set; }
        public string RefferalPersonName { get; set; }
        public long? Refferalconatctnumber { get; set; }
        public string Refferalemail { get; set; }
        public string Remarks { get; set; }


        public long ISMRLM_Id { get; set; }
        //public string Source { get; set; }
        //public string Category { get; set; }
        //public string Contact_Person { get; set; }
        //public string Designation { get; set; }
        //public string Health { get; set; }
        //public string State { get; set; }
        //public string Remarks { get; set; }
    }

}
