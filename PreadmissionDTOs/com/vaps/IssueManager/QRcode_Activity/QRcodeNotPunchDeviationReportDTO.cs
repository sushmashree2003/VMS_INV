using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity
{
    public class QRcodeNotPunchDeviationReportDTO
    {

        public long HRMARACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long HRMAR_Id { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }

        public long HRSTARACTRCD_Id { get; set; }

        public DateTime? HRSTARACTRCD_Date { get; set; }
        public string HRSTARACTRCD_Time { get; set; }
        public bool HRSTARACTRCD_ActiveFlag { get; set; }
        public DateTime? HRSTARACTRCD_CreatedDate { get; set; }
        public DateTime? HRSTARACTRCD_UpdatedDate { get; set; }
        public long HRSTARACTRCD_CreatedBy { get; set; }
        public long HRSTARACTRCD_UpdatedBy { get; set; }
        public long IVRMRT_Id { get; set; }

        public Array fillroletype { get; set; }
        public Array fill_buildings { get; set; }

        public Array fill_Floors { get; set; }
        public Array fill_Areas { get; set; }



        public string returnval { get; set; }
        public string HRMA_QRCODEPath { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMF_FloorName { get; set; }
        public string HRMAR_AreaName { get; set; }





        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }

        public Array emp_deatils { get; set; }

        public Array get_deviationreport { get; set; }
        public string Template { get; set; }

        public arrayuserEmployeeDTOPPP[] arrayuserEmp { get; set; }
        public multipletypes[] multipletype { get; set; }
        public multipletypess[] multipletypea { get; set; }

        public multitypesArea[] multitypesAreaee { get; set; }
        public class multitypesArea
        {
            public long HRMAR_ID { get; set; }
        }
        public class arrayuserEmployeeDTOPPP
        {
            public long HRME_Id { get; set; }
            public long UserId { get; set; }

        }
        public class multipletypes
        {
            public long HRMB_Id { get; set; }
        }

        public class multipletypess
        {
            public long HRMF_Id { get; set; }
        }
    }
}
