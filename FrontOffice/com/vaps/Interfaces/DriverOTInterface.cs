using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
    public  interface DriverOTInterface
    {
        DriverOTDTO getdata(DriverOTDTO data);
        DriverOTDTO employeereport(DriverOTDTO data);
        DriverOTDTO savedata(DriverOTDTO data);
        DriverOTDTO getdetails(int id);
        DriverOTDTO deletedata(int id);
        DriverOTDTO driverInTimeEntry(DriverOTDTO data);
        DriverOTDTO driverOutTimeEntry(DriverOTDTO data);
        Task<DriverOTDTO> getApprovalStatusAsync(DriverOTDTO data);
        Task<DriverOTDTO> OTFinalReportAsync(DriverOTDTO data);
        DriverOTDTO get_status(DriverOTDTO data);
        DriverOTDTO reject_status(DriverOTDTO data);
    }
}
