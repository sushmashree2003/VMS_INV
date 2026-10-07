using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class SalesClientInformationDto
    {
        public long MI_Id { get; set; }

        public long User_Id { get; set; }

        public long MDS_id { get; set; }
        public long? HRME_ID { get; set; }

        public Array products { get; set; }
    }
}
