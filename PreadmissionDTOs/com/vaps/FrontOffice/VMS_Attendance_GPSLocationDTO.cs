using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class VMS_Attendance_GPSLocationDTO
    {
        public long VMSATGPSL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public DateTime? VMSATGPSL_DateTime { get; set; }
        public string VMSATGPSL_Address { get; set; }
        public string VMSATGPSL_LATLONGValue { get; set; }
        public string VMSATGPSL_Remarks { get; set; }
        public bool VMSATGPSL_ActiveFlg { get; set; }
        public DateTime? VMSATGPSL_CreatedDate { get; set; }
        public DateTime? VMSATGPSL_UpdatedDate { get; set; }
        public long VMSATGPSL_CreatedBy { get; set; }
        public long VMSATGPSL_UpdatedBy { get; set; }
        public long UserId { get; set; }

        public Array employee_name { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public results[] results { get; set; }
        public string documentation  { get; set; }
        public Array clientlist { get; set; }
        public Array saleslist { get; set; }
        public string returnmsg { get; set; }
        public Array empdetails { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public bool punchflag { get; set; }
    }

    public class results
    {
        public components components { get; set; }
        public string formatted { get; set; }
    }

    public class components
    {
        public string city { get; set; }
        public string city_district { get; set; }
        public string continent { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string state { get; set; }
        public string state_code { get; set; }
        public string state_district { get; set; }
        public string suburb { get; set; }        
    }
}
