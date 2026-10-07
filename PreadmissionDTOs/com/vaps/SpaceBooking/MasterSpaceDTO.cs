using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
    public class MasterSpaceDTO : CommonParamDTO
    {
        public long MS_ID { get; set; }
        public long MI_Id { get; set; }
        public string MS_Name { get; set; }
        public long ST_ID { get; set; }
        public string MS_Description { get; set; }
        public bool MS_ActiveFlg { get; set; }
        public long MF_ID { get; set; }
        public string MS_Area { get; set; }
        public long MS_Capacity { get; set; }
        public decimal MS_RentDayPrice { get; set; }
        public decimal MS_RentPriceHr { get; set; }
        public long MS_DayHours { get; set; }
        public bool MS_GSTflg { get; set; }
        public string MS_Code { get; set; }
        public Array spacetypelist { get; set; }
        public Array floorlist { get; set; }

    }
}
