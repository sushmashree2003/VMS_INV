using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface IVRMandVMSInterlinkingDashboardInterface
    {
        IVRMandVMSInterlinkingDashboardDTO getBasicData(IVRMandVMSInterlinkingDashboardDTO dto);
        Task<IVRMandVMSInterlinkingDashboardDTO> getclientdetails(IVRMandVMSInterlinkingDashboardDTO dto);
        IVRMandVMSInterlinkingDashboardDTO getalldetailsQForm(IVRMandVMSInterlinkingDashboardDTO dto);
        IVRMandVMSInterlinkingDashboardDTO getEmployeedetailsQForm(IVRMandVMSInterlinkingDashboardDTO dto);
        IVRMandVMSInterlinkingDashboardDTO SearchUdetails(IVRMandVMSInterlinkingDashboardDTO dto);
        IVRMandVMSInterlinkingDashboardDTO SearchFReport(IVRMandVMSInterlinkingDashboardDTO dto);
    }
}
