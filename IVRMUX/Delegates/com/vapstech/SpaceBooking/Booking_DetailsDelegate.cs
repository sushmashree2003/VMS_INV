using CommonLibrary;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.SpaceBooking
{
    public class Booking_DetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<Booking_DetailsDTO, Booking_DetailsDTO> _areazone = new CommonDelegate<Booking_DetailsDTO, Booking_DetailsDTO>();

        public Booking_DetailsDTO getdetails(Booking_DetailsDTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "Booking_DetailsFacade/getdetails/");
        }
        public Booking_DetailsDTO booking_report(Booking_DetailsDTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "Booking_DetailsFacade/booking_report/");
        }
    }
}
