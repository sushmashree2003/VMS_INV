using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_EmployeeWise_MonthEnd_Details")]
    public class ISM_EmployeeWise_MonthEnd_DetailsDMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ISMEWMED_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMEWMED_Year { get; set; }
        public string ISMEWMED_MonthName { get; set; }
        public decimal ISMEWMED_NoOfResDev { get; set; }
        public decimal ISMEWMED_PtsAsPerPlannerCount { get; set; }
        public long ISMEWMED_NoOfCriticalTasksOpen { get; set; }
        public long ISMEWMED_NoOfMajorTasksOpen { get; set; }
        public long ISMEWMED_NoOfMinorTasksOpen { get; set; }
        public long ISMEWMED_NoOfTasksCompleted { get; set; }
        public long ISMEWMED_NoOfIssuesOpen { get; set; }
        public long ISMEWMED_NoOfIssuesCompleted { get; set; }
        public long ISMEWMED_NoOfEnhancementsOpen { get; set; }
        public long ISMEWMED_NoOfEnhancementsCompleted { get; set; }
        public decimal ISMEWMED_MonthlyDeviation { get; set; }
        public decimal ISMEWMED_NoOfDaysDrGenerated { get; set; }
        public decimal ISMEWMED_NoOfDaysDrNotGenerated { get; set; }
        public long ISMEWMED_NoOfWeeksPlCreated { get; set; }
        public long ISMEWMED_NoOfWeeksPlNotCreated { get; set; }
        public long ISMEWMED_NoOfActualhrsPlanned { get; set; }
        public decimal ISMEWMED_NoOfHrsWorked { get; set; }
        public decimal ISMEWMED_NoOfHrsApproved { get; set; }
        public decimal ISMEWMED_NoOfHrsRejected { get; set; }
        public decimal ISMEWMED_NoOfWorkingDays { get; set; }
        public decimal ISMEWMED_NoOfDaysPresent { get; set; }
        public decimal ISMEWMED_NoOfDaysAbsent { get; set; }
        public decimal ISMEWMED_TotalNoOfLeaves { get; set; }
        public decimal ISMEWMED_NoOfLeavesApplied { get; set; }
        public decimal ISMEWMED_NoOfLeavesApproved { get; set; }
        public decimal ISMEWMED_NoOfLeavesRejected { get; set; }
        public long ISMEWMED_NoOfSods { get; set; }
        public decimal ISMEWMED_Rating { get; set; }
        public long ISMEWMED_EscalationsFromClient { get; set; }
        public long ISMEWMED_NoOfMemo { get; set; }
        public long ISMEWMED_NoOfNotices { get; set; }
        public long ISMEWMED_AppeciationsfromClients { get; set; }
        public DateTime Createddate { get; set; }
        public long CreatedBy { get; set; }

        public string ISMEWMED_Remarks { get; set; }
        public string ISMEWMED_NoOfResDevRemarks { get; set; }
        public string ISMEWMED_PtsAsPerPlannerCountRemarks { get; set; }
        public string ISMEWMED_NoOfCriticalTasksOpenRemarks { get; set; }
        public string ISMEWMED_NoOfMajorTasksOpenRemarks { get; set; }
        public string ISMEWMED_NoOfMinorTasksOpenRemarks { get; set; }
        public string ISMEWMED_NoOfTasksCompletedRemarks { get; set; }
        public string ISMEWMED_NoOfIssuesOpenRemarks { get; set; }
        public string ISMEWMED_NoOfIssuesCompletedRemarks { get; set; }
        public string ISMEWMED_NoOfEnhancementsOpenRemarks { get; set; }
        public string ISMEWMED_NoOfEnhancementsCompletedRemarks { get; set; }
        public string ISMEWMED_MonthlyDeviationRemarks { get; set; }
        public string ISMEWMED_NoOfDaysDrGeneratedRemarks { get; set; }
        public string ISMEWMED_NoOfDaysDrNotGeneratedRemarks { get; set; }
        public string ISMEWMED_NoOfWeeksPlCreatedRemarks { get; set; }
        public string ISMEWMED_NoOfWeeksPlNotCreatedRemarks { get; set; }
        public string ISMEWMED_NoOfActualhrsPlannedRemarks { get; set; }
        public string ISMEWMED_NoOfHrsWorkedRemarks { get; set; }
        public string ISMEWMED_NoOfHrsApprovedRemarks { get; set; }
        public string ISMEWMED_NoOfHrsRejectedRemarks { get; set; }
        public string ISMEWMED_NoOfWorkingDaysRemarks { get; set; }
        public string ISMEWMED_NoOfDaysPresentRemarks { get; set; }
        public string ISMEWMED_NoOfDaysAbsentRemarks { get; set; }
        public string ISMEWMED_TotalNoOfLeavesRemarks { get; set; }
        public string ISMEWMED_NoOfLeavesAppliedRemarks { get; set; }
        public string ISMEWMED_NoOfLeavesApprovedRemarks { get; set; }
        public string ISMEWMED_NoOfLeavesRejectedRemarks { get; set; }
        public string ISMEWMED_NoOfSodsRemarks { get; set; }
        public string ISMEWMED_RatingRemarks { get; set; }
        public string ISMEWMED_EscalationsFromClientRemarks { get; set; }
        public string ISMEWMED_NoOfMemoRemarks { get; set; }
        public string ISMEWMED_NoOfNoticesRemarks { get; set; }
        public string ISMEWMED_AppeciationsfromClientsRemarks { get; set; }
    }
}
