using CommonLibrary;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.SpaceBooking
{
    public class SBReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<SBReport_DTO, SBReport_DTO> _areazone = new CommonDelegate<SBReport_DTO, SBReport_DTO>();

        public SBReport_DTO getdetails(SBReport_DTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "SBReportFacade/getdetails/");
        }
        public SBReport_DTO registration_report(SBReport_DTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "SBReportFacade/registration_report/");
        }
        public SBReport_DTO enquary_report(SBReport_DTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "SBReportFacade/enquary_report/");
        }
        public SBReport_DTO booking_report(SBReport_DTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "SBReportFacade/booking_report/");
        }
         public SBReport_DTO getroom(SBReport_DTO data)
        {
            return _areazone.POSTDataSpaceBooking(data, "SBReportFacade/getroom/");
        }

    }
}
