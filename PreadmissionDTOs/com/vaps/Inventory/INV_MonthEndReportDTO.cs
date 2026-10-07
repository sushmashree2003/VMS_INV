using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_MonthEndReportDTO
    {

        public long MI_Id { get; set; }
        public long MI_Id_new { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string roleflag { get; set; }
        public int month { get; set; }
        public string year { get; set; }
        public string staffcheck { get; set; }
        public Array acayear { get; set; }
        public Array Month_array { get; set; }
        public Array get_monthendreport { get; set; }
        public Array get_mi_list { get; set; }
        public string MI_Logo { get; set; }
        public string logopath { get; set; }

    }
}
