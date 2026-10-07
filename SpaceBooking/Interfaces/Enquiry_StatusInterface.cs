using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Interfaces
{
  public  interface Enquiry_StatusInterface
    {
        Enquiry_StatusDTO load_data(Enquiry_StatusDTO data);
        Enquiry_StatusDTO ClickBookVisit(Enquiry_StatusDTO data);
        Enquiry_StatusDTO SendStatus(Enquiry_StatusDTO data);
        Enquiry_StatusDTO getedit(Enquiry_StatusDTO data);
    }
}
