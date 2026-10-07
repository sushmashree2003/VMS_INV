using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_LocationReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array milist { get; set; }
        public string optionflag { get; set; }
        public Array get_itemreportdetails { get; set; }
        public itemssArrayDTO[] itemsArray { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public Array get_itemreport { get; set; }
        public string optionflagtwo { get; set; }
    }

    public class itemssArrayDTO
    {
        public long INVMI_Id { get; set; }
        public long INVMG_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVMST_Id { get; set; }
    }
}
