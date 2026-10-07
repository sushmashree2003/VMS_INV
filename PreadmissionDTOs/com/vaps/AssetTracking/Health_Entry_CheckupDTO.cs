using System;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class Health_Entry_CheckupDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public string Technician_Name { get; set; }
        public string Company_Name { get; set; }
        public string Contact_Number { get; set; }
        public string ISMMCLT_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string Designation { get; set; }
        public DateTime Next_Due_Date { get; set; }
        public string Emailid { get; set; }
        public string Remarks { get; set; }
        public string Priority { get; set; }
        public bool returnval { get; set; }

        // Header Properties
        public string projectName { get; set; }
        public string clientName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public ClientDropDownDTO[] ClientList { get; set; }
        public ProjectDropdownDTO[] projectlist { get; set; }
        public Health_Entry_StatusDTO[] StatusList { get; set; }
        public SystemCategoryCheckpointDto[] systemCategoryCheckpointDtos { get; set; }
        public Health_Checkup_Grid_DTO[] gridDataList { get; set; }
        public ApprovalDetailsDto[] approvalDetailsList { get; set; }
        public string Approval_Status { get; set; }
        public long HESCCM_Id { get; set; }
        public long[] selectedHesccmIds { get; set; }
        public HealthCheckupSummaryDTO[] summaryReportList { get; set; }

        // ADDED PROPERTY FOR DETAILED CHECKLIST REPORT
        public SystemCategoryCheckpointDto[] detailedReportList { get; set; }

        public long HCUE_Id { get; set; }
        public InspectionDetailsSignOffDTO inspectionDetailsSignOff { get; set; }
        public InspectionSummarySignOffDTO inspectionSummarySignOff { get; set; }
        public TechnicalSignOffDTO technicalSignOff { get; set; }
        public ClientOwnerVerificationDTO clientOwnerVerification { get; set; }
    }
    public class ClientDropDownDTO
    {
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
    }

    public class SystemCategoryCheckpointDto
    {
        public string SystemCategoryHeader { get; set; }
        public string System { get; set; }
        public string Category { get; set; }
        public string CheckPoint { get; set; }

        [Key]
        public long SCCC_Id { get; set; }
        public long MS_Id { get; set; }
        public long MC_Id { get; set; }

        // MAPPING TABLE SPECIFIC PROPERTIES
        public long? Health_Entry_Status_Id { get; set; }
        public string Priority { get; set; }
        public string Remarks { get; set; }
        public string Approval_Status { get; set; }
        public string StatusName { get; set; }
        public string CheckedBy { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
    }

    public class Health_Entry_StatusDTO
    {
        [Key]
        public long Health_Entry_Status_Id { get; set; }
        public string StatusName { get; set; }
    }

    public class Health_Checkup_Grid_DTO
    {
        public long HCUE_Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string TechnicianName { get; set; }
        public string StatusName { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ClientName { get; set; }
    }

    public class ApprovalDetailsDto : SystemCategoryCheckpointDto
    { 
        public long HESCCM_Id { get; set; }
        new public string StatusName { get; set; } = string.Empty;
        new public string CheckedDate { get; set; } = string.Empty;
        new public string CheckedBy { get; set; } = string.Empty;

    }
    public class HealthCheckupSummaryDTO
    {
        public string SystemName { get; set; }
        public string CategoryName { get; set; }
        public int TotalItems { get; set; }
        public int OkCount { get; set; }
        public int AttentionCount { get; set; }
        public int FailCount { get; set; }
        public int InProgressCount { get; set; }
        public int NotCheckedCount { get; set; }
    }
    public class InspectionDetailsSignOffDTO
    {
        public string ReportRefNo { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string InspectionTime { get; set; }
        public DateTime? NextDueDate { get; set; }
    }

    public class InspectionSummarySignOffDTO
    {
        public long ISMMPR_Id { get; set; }
        public int TotalSystemsInspected { get; set; }
        public int TotalCheckpoints { get; set; }
        public int OkCount { get; set; }
        public int AttentionCount { get; set; }
        public int FailCount { get; set; }
        public int InProgressCount { get; set; }
        public int NotCheckedCount { get; set; }
        public decimal? OverallHealthScorePercent { get; set; }
    }

    public class TechnicalSignOffDTO
    {
        public long ISMMPR_Id { get; set; }
        public string TechnicianName { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }
        public string Emailid { get; set; }
        public DateTime? Date { get; set; }
    }

    public class ClientOwnerVerificationDTO
    {
        public long ISMMPR_Id { get; set; }
        public string ClientName { get; set; }
        public string Designation { get; set; }
        public DateTime? Date { get; set; }
        public string CurrentStatus { get; set; }
    }
}