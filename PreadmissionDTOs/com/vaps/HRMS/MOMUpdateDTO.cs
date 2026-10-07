using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MOMUpdateDTO
    {
  
        public long MOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string return_status { get; set; }
        public string ISMSLEDM_Remarks { get; set; }
        public string ManagerMailid { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string MI_Name { get; set; }
        public Int32? HRMD_Order { get; set; }
        public int MI_ActiveFlag { get; set; }
        public long HRMD_Id { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long MOM_ParentId { get; set; }
        public string MOM_Agenda { get; set; }
        public string MOM_Name { get; set; }
        public string MOMMNT_Names { get; set; }
        public DateTime MOM_Date { get; set; }
        public string MOM_StartTime { get; set; }
        public string MOM_EndTime { get; set; }
        public string MOM_Venue { get; set; }
        public string Emp_type { get; set; }
        public string MOM_Minutes { get; set; }
        public bool? MOM_ActiveFlag { get; set; }
        public string MOM_EXTINTFlag { get; set; }
        public bool? MOMPRT_ActiveFlag { get; set; }
        public string MOM_TentativeEndTime { get; set; }
        public string MOM_TentativeStartTime { get; set; }
        public string Template { get; set; }
        public string flg { get; set; }
        public DateTime MOM_CreatedDate { get; set; }
        public DateTime MOM_UpdatedDate { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public long MOM_CreatedBy { get; set; }
        public long roleId { get; set; }
        public long Year_id { get; set; }
        public long Month_id { get; set; }
        public long MOM_UpdatedBy { get; set; }
        public string MOM_Status { get; set; }
        public int userId { get; set; }
        public string retrunMsg { get; set; }
        public long HRME_Id { get; set; }
        public Array empactionplan { get; set; }
        public Array absentdata { get; set; }
        public Array fillyear { get; set; }
        public Array edittransdata { get; set; }
        public Array editempdata { get; set; }
        public Array aprovedlist { get; set; }
        public Array departmentdropdown { get; set; }
        public Array fillmonth { get; set; }
        public Array empdropdown { get; set; }
        public Array allvisitor { get; set; }
        public Array meetingname { get; set; }
        public Array venue { get; set; }
        public Array approvaldata { get; set; }
        public Array empdata { get; set; }
        public Array actionplan { get; set; }
        public Array editupdateDetails { get; set; }
        public Array updateDetails { get; set; }
        public Array getfiledetails { get; set; }
        public Array getParticipentsdetails { get; set; }
        public Array visitorlist { get; set; }
        public Array emplist { get; set; }
        public Array getapproveddata { get; set; }
        public Array griddata { get; set; }
        public Array failedlist { get; set; }
        public Array getapproval { get; set; }
        public Array momdetailreport { get; set; }
        public Array appliedlist { get; set; }
        public Array approvedata { get; set; }
        public Array oldmeetdetail { get; set; }
        public long? MOMPRT_MobileNo { get; set; }
        public string MOMPRT_Email { get; set; }
        public string MOMPRT_Designation { get; set; }
        public string MOMFL_FilePath { get; set; }
        public string MOMFL_FileName { get; set; }
        public long MOMFL_Id { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string MOMPRT_ParticipantsName { get; set; }
        public string stuStatus { get; set; }
        public string Type { get; set; }
        public string Task { get; set; }
        public string Participants { get; set; }
        public string Empcode { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string ActionPlans { get; set; }
        public string TargetDate { get; set; }
        public string Attendence { get; set; }

        public string Email { get; set; }
        public string MOMPRT_EXTINTFlag { get; set; }
        public string amstdate { get; set; }
        public string returnVal { get; set; }
        public long MOMMNT_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string MOMMNT_ActionPlans { get; set; }
        public string MOM_Flag { get; set; }
        public DateTime? MOMMNT_TargetDate { get; set; }
        public string monthname { get; set; }
        public long HRMLY_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public long? HRMLY_LeaveYearOrder { get; set; }
        //public string MOMMNT_Names { get; set; }
        public long MOMPRT_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string IVRMRT_Role { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public List<MOMUpdateDTO> newlstget { get; set; }
        public MOMUpdateDTO[] albumNameArray { get; set; }
        public MOMUpdateDTO[] newlstget1 { get; set; }
        public MOMUpdateDTO[] selected_Inst { get; set; }
        public filedto[] FilePath_Array { get; set; }
        public participantsdto[] Participent_Array { get; set; }
        public newparticipantsdto[] ParticipentsArray { get; set; }
        public long INVMS_Id { get; set; }

        public string INVMS_SupplierName { get; set; }
        public string INVMS_SupplierCode { get; set; }
        public string INVMS_SupplierConatctPerson { get; set; }
        public long INVMS_SupplierConatctNo { get; set; }
        public string INVMS_EmailId { get; set; }
        public Array onloadVendors { get; set; }
        public Array vendornames { get; set; }
        public class filedto
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
        }

        public class participantsdto
        {
            public long MOMPRT_Id { get; set; }
            public long MOMMNT_Id { get; set; }
            public string MOMPRT_ParticipantsName { get; set; }
            public string MOMMNT_Names { get; set; }
            public string MOMMNT_ActionPlans { get; set; }
            public DateTime? MOMMNT_TargetDate { get; set; }
            public long MOMPRT_MobileNo { get; set; }
            public string MOMPRT_Email { get; set; }
            public string MOMPRT_Designation { get; set; }
            public string MOMPRT_EXTINTFlag { get; set; }
            public long MOM_Id { get; set; }
            public long HRME_Id { get; set; }
        }
        public class newparticipantsdto
        {
            public long MOMPRT_Id { get; set; }
            public long MOMMNT_Id { get; set; }
            public string MOMPRT_ParticipantsName { get; set; }
            public string MOMMNT_Names { get; set; }
            public string MOMMNT_ActionPlans { get; set; }
            public DateTime? MOMMNT_TargetDate { get; set; }
            public long? MOMPRT_MobileNo { get; set; }
            public string MOMPRT_Email { get; set; }
            public string MOMPRT_Designation { get; set; }
            public string MOMPRT_EXTINTFlag { get; set; }
            public long MOM_Id { get; set; }
            public long HRME_Id { get; set; }

            public long INVMS_Id { get; set; }

            public string INVMS_SupplierName { get; set; }
        }
    }
}
