using CommonLibrary;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.SpaceBooking
{
    public class Space_AmenitiesDelegate
    {
        CommonDelegate<SpaceBookingDTO, SpaceBookingDTO> COMMA = new CommonDelegate<SpaceBookingDTO, SpaceBookingDTO>();

        public SpaceBookingDTO get_master_amenities(SpaceBookingDTO dto)
        {
           
            return COMMA.POSTDataSpaceBooking(dto, "Space_AmenitiesFacade/get_master_amenities/");
        }
        public SpaceBookingDTO SaveEdit_MasterAmenities(SpaceBookingDTO dto)
        {
           
            return COMMA.POSTDataSpaceBooking(dto, "Space_AmenitiesFacade/SaveEdit_MasterAmenities/");
        }
        public SpaceBookingDTO Edit_MasterAmenities(SpaceBookingDTO dto)
        {
           
            return COMMA.POSTDataSpaceBooking(dto, "Space_AmenitiesFacade/Edit_MasterAmenities/");
        }
        public SpaceBookingDTO deactivate_MasterAmenities(SpaceBookingDTO dto)
        {
           
            return COMMA.POSTDataSpaceBooking(dto, "Space_AmenitiesFacade/deactivate_MasterAmenities/");
        }
    }
}
