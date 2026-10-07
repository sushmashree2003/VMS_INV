using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity
{
    public class HRMasterAreaStaffMappingDTO
    {

        public long HRMAR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMAR_AreaName { get; set; }
        public string HRMAR_AreaCode { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public string HRMAR_TypeFlag { get; set; }
        public string HRMAR_QRCode { get; set; }
        public bool HRMAR_ActiveFlag { get; set; }
        public long HRMAR_CreatedBy { get; set; }
        public long HRMAR_UpdatedBy { get; set; }

        public string HRMB_BuildingName { get; set; }
        public string HRMF_FloorName { get; set; }


        public Array get_building { get; set; }
        public Array get_floor { get; set; }
        public Array get_getAreaName { get; set; }

        //Activity
        public long HRMARACT_Id { get; set; }
        public string HRMARACT_ActivityName { get; set; }
        public string HRMARACT_ActivityDesc { get; set; }
        public bool HRMARACT_ActiveFlag { get; set; }

        public long HRMARACT_CreatedDate { get; set; }
        public long HRMARACT_UpdatedDate { get; set; }

        public Array get_AreaActivity { get; set; }

        public Array get_Area { get; set; }
        public string returnduplicatestatus { get; set; }


        //Activitynames
        public Array get_AreaActivityName { get; set; }
        public Array get_getActivity { get; set; }
        public long HRMARACTT_Id { get; set; }
        public string HRMARACTT_Time { get; set; }
        public long HRMARACTT_ActivityDuration { get; set; }
        public bool HRMARACTT_ActiveFlag { get; set; }
        public DateTime? HRMARACTT_CreatedDate { get; set; }
        public DateTime? HRMARACTT_UpdatedDate { get; set; }
        public long HRMARACTT_CreatedBy { get; set; }
        public long HRMARACTT_UpdatedBy { get; set; }
        public Array get_AreaActivityTime { get; set; }
        //  public Array stafflist { get; set; }
        public string employeename { get; set; }
        public long id { get; set; }

        public long HEME_Id { get; set; }


        public long HRMARACTSTMAP_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime? HRMARACTSTMAP_Date { get; set; }
        public bool HRMARACTSTMAP_ActiveFlag { get; set; }
        public DateTime? HRMARACTSTMAP_CreatedDate { get; set; }
        public DateTime? HRMARACTSTMAP_UpdatedDate { get; set; }
        public long HRMARACTSTMAP_CreatedBy { get; set; }
        public long HRMARACTSTMAP_UpdatedBy { get; set; }
        public Array get_getActivityTime { get; set; }

        public Array staffmappinglist { get; set; }

        public bool returnval { get; set; }
        public Array fillroletype { get; set; }
        public Array fillinstitution { get; set; }


        public int MI_ActiveFlag { get; set; }

        public Array get_getusernames { get;set;}

        public long IVRMRT_Id { get; set; }
        public string UserName { get; set; }
       
        public Array editarrey { get; set; }
        public Array emp_deatils { get; set; }
        public Array AutoStaffMapping_deatils { get; set; }

        public TempDTOO[] ActivitynameArray { get; set; }

        public class TempDTOO
        {
            public long HRMARACT_Id { get; set; }
        }

    }
}
