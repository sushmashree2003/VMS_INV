using CommonLibrary;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.SpaceBooking
{
    public class Enquiry_StatusDelegate
    {
        CommonDelegate<Enquiry_StatusDTO, Enquiry_StatusDTO> COMMM = new CommonDelegate<Enquiry_StatusDTO, Enquiry_StatusDTO>();
        public Enquiry_StatusDTO load_data(Enquiry_StatusDTO data)
        {
            return COMMM.POSTDataSpaceBooking(data, "Enquiry_StatusFacade/load_data/");
        }
        public Enquiry_StatusDTO ClickBookVisit(Enquiry_StatusDTO data)
        {
            return COMMM.POSTDataSpaceBooking(data, "Enquiry_StatusFacade/ClickBookVisit/");
        }
        public Enquiry_StatusDTO SendStatus(Enquiry_StatusDTO data)
        {
            return COMMM.POSTDataSpaceBooking(data, "Enquiry_StatusFacade/SendStatus/");
        }
        public Enquiry_StatusDTO getedit(Enquiry_StatusDTO data)
        {
            return COMMM.POSTDataSpaceBooking(data, "Enquiry_StatusFacade/getedit/");
        }
    }
}
