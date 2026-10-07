using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_Driver_IndentDTO
    {
        public DateTime? ISMDIT_Date { get; set; }
        public long TRMV_Id { get; set; }
        public string ISMDIT_BillNo { get; set; }
        public decimal ISMDIT_Qty { get; set; }
        public decimal ISMDIT_Amount { get; set; }
        public string ISMDIT_Remarks { get; set; }
        public decimal ISMDIT_OpeningKM { get; set; }
        public string MI_logo { get; set; }
        public decimal ISMDIT_ClosingKM { get; set; }
        public decimal ISMDIT_BalanceDiesel { get; set; }
        public decimal ISMDIT_PreviousReading { get; set; }
        public decimal ISMDIT_CurrentReading { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMDIT_PreparedByUserId { get; set; }
        public long ISMDIT_ApprovalUserId { get; set; }
        public long ISMDIT_ReceivedByUserId { get; set; }
        public Array DriverIndentList { get; set; }
        public Array vehicle_list { get; set; }
        public Array alldata { get; set; }
        public Array edittab1 { get; set; }
        public Array editlist { get; set; }
        public long MI_Id { get; set; }
        public long MI_Id_new { get; set; }
        public string responsemessage { get; set; }
        public long User_Id { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public string vehicleno { get; set; }
        public string msg { get; set; }
        public bool duplicate { get; set; }
        public long ISMDIT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string DesignationName { get; set; }
        public Array IndentApprovalList { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string ISMDIT_Remark { get; set; }
        public ISM_Driver_IndentDTO[] get_indent_status { get; set; }
        public long roleId { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public Array indentreport { get; set; }
        public Array IndentApproved { get; set; }
        public Array vehicledetails { get; set; }
        public string rdotype { get; set; }
        public string MI_Logo { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public List<ISM_Driver_IndentDTO> devicelist12 { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string ISMDIT_Status { get; set; }
        public decimal ISMDIT_DiffrenceKm { get; set; }
    }
}
