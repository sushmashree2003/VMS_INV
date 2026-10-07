using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Interfaces
{
    public interface Space_Amenities_Interface
    {
        SpaceBookingDTO get_master_amenities(SpaceBookingDTO dto);
        SpaceBookingDTO SaveEdit_MasterAmenities(SpaceBookingDTO dto);
        SpaceBookingDTO Edit_MasterAmenities(SpaceBookingDTO dto);
        SpaceBookingDTO deactivate_MasterAmenities(SpaceBookingDTO dto);
    }
}
