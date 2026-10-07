using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class DriverOTDTO
    {
        public long VMSDT_ID { get; set; }
        public long HRME_Id { get; set; }
        public DateTime VMSDT_Punchdate { get; set; }
        public string VMSDT_PunchIn { get; set; }
        public string VMSDT_PunchOut { get; set; }
        public string VMSDT_LateIn { get; set; }
        public string VMSDT_EarlyOut { get; set; }
        public string VMSDT_EarlyComingOt { get; set; }
        public string VMSDT_LategoingOt { get; set; }
        public string VMSDT_TotalOt { get; set; }
        public string VMSDT_FinalOt { get; set; }
        public string VMSDT_Remarks { get; set; }
        public long VMSDT_MonthId { get; set; }
        public string VMSDT_FinalLatein { get; set; }
        public long VMSDT_CreatedBy { get; set; }
        public long VMSDT_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long MI_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array employeelist { get; set; }
        public Array monthlist { get; set; }
        public long monthid { get; set; }
        public Array reportdata { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string returnvalue { get; set; }
        public long UserId { get; set; }
        public string FOMST_IHalfLoginTime { get; set; }
        public string FOMST_IIHalfLogoutTime { get; set; }
        public string FOMSTDayofWeek { get; set; }
        public bool withpreviousdayflag { get; set; }
        public Array editdata { get; set; }
        public long VMSDT_ShiftId { get; set; }
        public long FOMS_Id { get; set; }
        public Array otapprovallist { get; set; }
        public string VSMDTA_Status { get; set; }
        public string VSMDTA_Remarks { get; set; }
        public DriverOTDTO[] get_OT_status { get; set; }
        public Array finalotreport { get; set; }
        public Array yearlist { get; set; }
        public string year { get; set; }
    }
}
