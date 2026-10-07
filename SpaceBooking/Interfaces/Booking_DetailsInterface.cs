using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Interfaces
{
   public interface Booking_DetailsInterface
    {
        Booking_DetailsDTO getdetails(Booking_DetailsDTO data);
        Task<Booking_DetailsDTO> booking_report(Booking_DetailsDTO data);
    }
}
