using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MonthEndReportDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        //public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_FatherName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRME_ESIAccNo { get; set; }
        public string HRME_PFAccNo { get; set; }
        public long? IVRMMG_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public long? ivrM_Month_Id { get; set; }
        public long? hreS_Year { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public decimal? basicamount { get; set; }
        public decimal? allowanceamount { get; set; }
        public decimal? hraamount { get; set; }

        public Array employeeDetails { get; set; }
        public Array employeeDetailssingle { get; set; }
        public Array employeeAttendance { get; set; }
        public Array companydetails { get; set; }
        public Array leavedetails { get; set; }


        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        
        public string HRML_LeaveName { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public decimal HRELS_OBLeaves { get; set; }
        public decimal HRELS_CreditedLeaves { get; set; }


        public int workingEmployee { get; set; }
        public int leftEmployee { get; set; }
        public int newEmployee { get; set; }

        public int salaryGenerated { get; set; }
        public int salaryslipGenerated { get; set; }


        //missing details of working employee
        public int missingPhoto { get; set; }
        public int missingEmailId { get; set; }
        public int missingContactNumber { get; set; }


        //missing details of left employee
        public int missingContactNumberleft { get; set; }
        public int missingEmailIdleft { get; set; }
        public int missingPhotoleft { get; set; }


        //missing details of new employee
        public int missingPhotonew { get; set; }
        public int missingEmailIdnew { get; set; }
        public int missingContactNumbernew { get; set; }
        public string monthendDate { get; set; }
        public InstitutionDTO institutionDetails { get; set; }
        public int salaryslipsent { get; set; }
        public int salaryslipsmssent { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array groupTypedropdown { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long[] empid { get; set; }
        public string empIds { get; set; }
        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }
        public long HRMED_Id { get; set; }
        public decimal? HREED_Amount { get; set; }
        public Array earningheadlist { get; set; }
        public Array deductionheadlist { get; set; }
        public Array earningheadvalues { get; set; }
        public Array GetEmpinstitutiondetails { get; set; }
    }
}
