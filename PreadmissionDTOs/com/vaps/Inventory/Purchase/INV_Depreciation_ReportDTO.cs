using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory.Purchase
{
   public class INV_Depreciation_ReportDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public Array Institution { get; set; }
        public Array YEAR { get; set; }
        public long IMFY_Id { get; set; }
        public string FYear { get; set; }
       public Array getReport { get; set; }
        public string MI_Logo { get; set; }
        public string MIName { get; set; }



    }
}
