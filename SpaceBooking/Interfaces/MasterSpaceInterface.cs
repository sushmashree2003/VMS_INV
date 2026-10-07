using PreadmissionDTOs.com.vaps.SpaceBooking;

using System.Threading.Tasks;

namespace SpaceBooking.Interfaces
{
    public interface MasterSpaceInterface
    {
        OS_EnquiryDTO getdata(int id);
        OS_EnquiryDTO savedata(OS_EnquiryDTO data);
        OS_EnquiryDTO saveEnquiry(OS_EnquiryDTO data);
        OS_EnquiryDTO geteditdata(OS_EnquiryDTO data);
        OS_EnquiryDTO activedeactive(OS_EnquiryDTO data);
        OS_EnquiryDTO loadcountry(OS_EnquiryDTO data);
        OS_EnquiryDTO get_state(OS_EnquiryDTO data);
        OS_Space_BookingDTO SaveBooking(OS_Space_BookingDTO Spacebooking);
        OS_Space_BookingDTO Availability(OS_Space_BookingDTO Spacebooking);

        OS_Space_BookingDTO OnlinePaymentDetails(OS_Space_BookingDTO data);
        OS_Space_BookingDTO GenerateOrderId(OS_Space_BookingDTO data);
        OS_Space_BookingDTO UserBookingDetails(OS_Space_BookingDTO data);
        OS_Space_BookingDTO SaveBooking_Payment(OS_Space_BookingDTO data);
        OS_Space_BookingDTO send_email(OS_Space_BookingDTO data);

        
    }
}
