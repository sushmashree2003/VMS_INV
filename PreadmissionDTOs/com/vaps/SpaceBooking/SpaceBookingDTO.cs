using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
    public class SpaceBookingDTO : CommonParamDTO
    {
        public string OSUREG_UserName { get; set; }
        public string OSUREG_EmailId { get; set; }
        public long OSUREG_MobileNo { get; set; }
        public string OSUREG_Password { get; set; }
        public string response { get; set; }
        public long HRMAM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMAM_AmenitiesName { get; set; }
        public string HRMAM_AmenitiesDes { get; set; }
        public bool? HRMAM_PriceApplFlg { get; set; }
        public decimal? HRMAM_Price { get; set; }
        public bool? HRMAM_ActiveFlag { get; set; }
        public DateTime? HRMAM_CreatedDate { get; set; }
        public DateTime? HRMAM_UpdatedDate { get; set; }
        public long HRMAM_CreatedBy { get; set; }
        public long? HRMAM_UpdatedBy { get; set; }
        public string returnvalue { get; set; }
        public long userId { get; set; }
        public Array master_amenities_list { get; set; }
        public Array master_amenities_details { get; set; }
    }
}
