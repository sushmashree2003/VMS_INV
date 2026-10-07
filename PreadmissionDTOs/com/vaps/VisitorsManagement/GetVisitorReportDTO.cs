using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class GetVisitorReportDTO
    {
        public long AMVM_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMVM_Name { get; set; }
        public string AMVM_Contact_No { get; set; }
        public string AMVM_Emailid { get; set; }
        public string AMVM_Card_No { get; set; }
        public string AMVM_Identity_Type { get; set; }
        public string AMVM_Address { get; set; }
        public string AMVM_To_Meet { get; set; }
        public string AMVM_Purpose { get; set; }
        public DateTime? Date_Visit { get; set; }
        public string Time_Visit { get; set; }
        public string AMVM_Type { get; set; }
        public string AMVM_Status { get; set; }
        public string AMVM_Out_Time { get; set; }
        public DateTime? AMVM_Entry_Date { get; set; }
        public string AMVM_Remarks { get; set; }
        public bool AMVM_ActiveFlag { get; set; }
        public Array viewlist { get; set; }
        public Array attendancegpslist { get; set; }
        public string searchby { get; set; }
        public string txtbox { get; set; }
        public DateTime? Date_vist { get; set; }
        public string IVRMRT_Role { get; set; }

        public string yearid { get; set; }
        public string month_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string all1 { get; set; }

        public Array institute { get; set; }
        public Array employeelist { get; set; }
        public long UserId { get; set; }
        public long roleId { get; set; }
        public string userName { get; set; }
        public string employeename { get; set; }
        public long HRME_Id { get; set; }
        public string optionflag { get; set; }
        public GetVisitorReportDTO[] selected_Inst { get; set; }
        public employeearray1[] employeearray { get; set; }
        public class employeearray1
        {
            public long HRME_Id { get; set; }
        }

    }
}
