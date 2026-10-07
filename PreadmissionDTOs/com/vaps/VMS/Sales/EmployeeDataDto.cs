using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
   public class EmployeeDataDto
    {
        public Array EmpDetails { get; set; }
        public Array SalesNegaotionData { get; set; } 

        public long MI_Id { get; set; }

        public long User_Id { get; set; }

        public long MDS_id { get; set; }
        public long? HRME_ID { get; set; }

        public string Scenario { get; set; }

        public long? ActiveFlag { get; set; }

        public string returndata { get; set; }

        public Array MasterData { get; set; }
    }
}
