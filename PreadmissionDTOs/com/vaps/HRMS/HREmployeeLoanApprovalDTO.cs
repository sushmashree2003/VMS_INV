using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HREmployeeLoanApprovalDTO
    {
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public long institutionID { get; set; }
        public string MI_Name { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public string HRES_DailyRates { get; set; }
        public decimal? HRES_EPF { get; set; }
        public decimal? HRES_FPF { get; set; }
        public decimal? HRES_Ac21 { get; set; }
        public decimal? HRES_Ac22 { get; set; }
        public decimal? HRES_Ac5 { get; set; }
        public DateTime? HRES_FromDate { get; set; }
        public DateTime? HRES_ToDate { get; set; }
        public bool? HRES_ArrearRegFlag { get; set; }
        public string HRES_BankCashFlag { get; set; }
        public string HRESA_Remarks { get; set; }
        public long? HRMGT_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string HRES_BankCode { get; set; }
        public string HRES_AccountNo { get; set; }
        public decimal? HRES_ESIEmplr { get; set; }
        public string radiotype { get; set; }
        public long roleId { get; set; }
        public string retrunMsg { get; set; }
        public Array monthdropdown { get; set; }
        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array companydetails { get; set; }
        public Array earninganddeduction { get; set; }

        public Array employeeTypedropdown { get; set; }
        public Array employeedetailList { get; set; }
        public Array employeeRatingDetails { get; set; }
        public Array salaryapprovedgridview { get; set; }
        public Array salaryholddgridview { get; set; }

        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array getviewfilesdetails { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long LogInUserId { get; set; }
        public long nonearned { get; set; }
        public decimal HREL_SanctionedAmount { get; set; }
        public Array earnedandnonearned { get; set; }
        public MasterEmployeeDTO[] masterEmployeeList { get; set; }
        public salaryUpdationDTO[] selectedEmpdetails { get; set; }
        public Array employeeDetails { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }


        public InstitutionDTO institutionDetails { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }

        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }

        public string NetAmountInwords { get; set; }

        public Array employeeSalaryslipDetails { get; set; }

        public Array groupTypedropdown { get; set; }

        public long?[] groupTypeIdList { get; set; }

        public CumulativeSalaryReportDTO CumulativeSalaryReportDTO { get; set; }
        public HR_Master_EarningsDeductionsDTO[] headList { get; set; }

        public decimal LopAmount { get; set; }
        public decimal Lopdays { get; set; }


        public Array employeeLeaveDetails { get; set; }
        public HR_Employee_SalaryDTO empsaldetail { get; set; }
        public HR_ConfigurationDTO PayrollStandard { get; set; }

        public string EmailSMS { get; set; }

        public string hrmE_EmployeeCode { get; set; }

        public string TemplateString { get; set; }

        public HR_Employee_SalaryDTO[] Template { get; set; }


        public Array employe { get; set; }

        public long[] empid { get; set; }

        public Array main_list { get; set; }

        public decimal? empGrossSal { get; set; }

        public decimal? HRESD_Amount { get; set; }

        public Array employeeSalaryslipapproveDetails { get; set; }
        public string comm { get; set; }
        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }

        public Array filldata { get; set; }
        public string[] MonthList { get; set; }
        public int UserId { get; set; }
        public long HREL_ID { get; set; }

        public Array getemployee { get; set; }
        public Array GetReport { get; set; }
        public bool finalflag { get; set; }
        public Companylist[] selected_Inst { get; set; }
        public string HRMDOC_DocumentName { get; set; }
        public string HRMDOC_Description { get; set; }
        public string HRELD_Filepath { get; set; }
        public string HRELD_Filetype { get; set; }

        public decimal HREL_TotalPending { get; set; }
        public decimal HREL_TotalPrincipalPaid { get; set; }
    }
    public class Companylist
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
    }
}
