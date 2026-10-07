using DataAccessMsSqlServerProvider.com.vapstech.SpaceBooking;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Services
{
    public class Enquiry_StatusImpl:Interfaces.Enquiry_StatusInterface
    {
        public SpaceBookingContext _SpaceBookingContext;
        public Enquiry_StatusImpl(SpaceBookingContext SpaceBookingContext)
        {
            _SpaceBookingContext = SpaceBookingContext;
        }
        public Enquiry_StatusDTO load_data(Enquiry_StatusDTO data)
        {
            try
            {
                data.enquirydetailList = _SpaceBookingContext.OS_EnquiryDMO.Where(t => t.MI_Id == data.MI_Id && t.OSENQ_Type == "B").ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Enquiry_StatusDTO ClickBookVisit(Enquiry_StatusDTO data)
        {
            if (data.Type == "B")
            {
                data.enquirydetailList = _SpaceBookingContext.OS_EnquiryDMO.Where(t => t.MI_Id == data.MI_Id && t.OSENQ_Type == "B").ToArray();
            }
            else if(data.Type == "V")
            {
                data.enquirydetailList = _SpaceBookingContext.OS_EnquiryDMO.Where(t => t.MI_Id == data.MI_Id && t.OSENQ_Type == "V").ToArray();
            }
            return data;           
        }
        public Enquiry_StatusDTO SendStatus(Enquiry_StatusDTO data)
        {
            try
            {
                data.datalist = _SpaceBookingContext.OS_EnquiryDMO.Where(t => t.MI_Id == data.MI_Id && t.OSENQ_Id==data.OSENQ_Id).ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Enquiry_StatusDTO getedit(Enquiry_StatusDTO data)
        {
            try
            {
                data.edit_list = _SpaceBookingContext.OS_EnquiryDMO.Where(a => a.OSENQ_Id == data.OSENQ_Id && a.MI_Id == data.MI_Id).ToList().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
