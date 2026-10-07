using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBooking.Interfaces
{
   public interface SBReportInterface
    {

        SBReport_DTO getdetails(SBReport_DTO data);
        SBReport_DTO registration_report(SBReport_DTO data);
        Task<SBReport_DTO> enquary_report(SBReport_DTO data);
        Task<SBReport_DTO> booking_report(SBReport_DTO data);
        Task<SBReport_DTO> getroom(SBReport_DTO data);
    }
}
