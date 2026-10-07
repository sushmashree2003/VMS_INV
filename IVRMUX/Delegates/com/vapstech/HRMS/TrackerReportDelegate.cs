using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class TrackerReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TrackerReportDTO, TrackerReportDTO> COMMM = new CommonDelegate<TrackerReportDTO, TrackerReportDTO>();

        public TrackerReportDTO findstatistics(TrackerReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "TrackerReportFacade/findstatistics");
        }
    }
}
