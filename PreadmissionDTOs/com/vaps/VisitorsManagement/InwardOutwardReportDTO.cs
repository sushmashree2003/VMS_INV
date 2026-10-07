using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class InwardOutwardReportDTO
    {
        public long MI_Id { get; set; }
        public Array viewlist { get; set; }
        public Array viewlist1 { get; set; }
        public string radiotype { get; set; }
        public string month_id { get; set; }
        public string yearid { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string all1 { get; set; }
        public Array institute { get; set; }

        public long UserId { get; set; }
        public long roleId { get; set; }
        public string IVRMRT_Role { get; set; }
        public string userName { get; set; }

        public InwardOutwardReportDTO[] selected_Inst { get; set; }
    }
}
