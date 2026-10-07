using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface DCReportInterface
    {
        Task<DCReportDTO> getloaddata(DCReportDTO data);
        Task<DCReportDTO> getreport(DCReportDTO data);

    }
}
