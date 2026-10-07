using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADA_ApplyReportsDTO
    {
        public Array GetReport { get; set; }
        public long IVRMMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array get_userEmplist { get; set; }
        public Array Veiwdetails { get; set; }
        public Array Veiwdetailstt { get; set; }
        public bool roleflag { get; set; }
        public string Role_flag { get; set; }
        public string empdepartment { get; set; }
        public string empname { get; set; }
        public long ASMAY_Id { get; set; }
        public Array City_Type { get; set; }
        public Array Get_MasterReport { get; set; }
        public long HRMDES_Id { get; set; }
        public string RdoPunch { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public arrayuserEmployeeDTO[] arrayuserEmp { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public long VTADAAA_Id { get; set; }
        //extra
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public decimal VTADAA_TotalSactionedAmount { get; set; }
        public decimal VTADAA_TotalPaidAmount { get; set; }
        public decimal VTADAAA_TotalAppliedAmount { get; set; }
        public decimal VTADAAA_TotalSactionedAmount { get; set; }
        public decimal VTADAAA_TotalPaidAmount { get; set; }
        public long VTADAA_Id { get; set; }
        public emplist[] emplist { get; set; }

        public Array consolited { get; set; }
        public DateTime? VTADAA_FromDate { get; set; }
        public DateTime? VTADAA_ToDate { get; set; }
        public string VTADAA_ToAddress { get; set; }
        public string VTADAA_Remarks { get; set; }
        public string IVRMMCT_Name { get; set; }
        public string VTADAA_StatusFlg { get; set; }
        public string EmplYoeeName { get; set; }
        public string MI_Name { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string optionflag { get; set; }
    }
    public class arrayuserEmployeeDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }

    }
    public class DepartmentDTO
    {
        public int HRMDC_ID { get; set; }
    }
    public class DesignationDTO
    {
        public long HRMDES_Id { get; set; }
    }
    public class emplist
    {
        public long HRME_Id { get; set; }
    }

    //public class deptlist
    //{
    //    public long HRMDC_ID { get; set; }
    //}
    //public class desglist
    //{
    //    public long HRMDES_Id { get; set; }
    //}

}
