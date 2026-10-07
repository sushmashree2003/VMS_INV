using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class IVRMandVMSInterlinkingDashboardDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRMandVMSInterlinkingDashboardDTO, IVRMandVMSInterlinkingDashboardDTO> COMMM = new CommonDelegate<IVRMandVMSInterlinkingDashboardDTO, IVRMandVMSInterlinkingDashboardDTO>();

        public IVRMandVMSInterlinkingDashboardDTO onloadgetdetails(IVRMandVMSInterlinkingDashboardDTO dto)
        {
            return COMMM.POSTDataa(dto, "IVRMandVMSInterlinkingDashboardFacade/onloadgetdetails");
        }

        public IVRMandVMSInterlinkingDashboardDTO getclientdetails(IVRMandVMSInterlinkingDashboardDTO maspage)
        {
            return COMMM.POSTData(maspage, "IVRMandVMSInterlinkingDashboardFacade/getclientdetails/");
        }

        public IVRMandVMSInterlinkingDashboardDTO getalldetailsQForm(IVRMandVMSInterlinkingDashboardDTO maspage)
        {
            return COMMM.POSTData(maspage, "IVRMandVMSInterlinkingDashboardFacade/getalldetailsQForm/");
        }

        public IVRMandVMSInterlinkingDashboardDTO getEmployeedetailsQForm(IVRMandVMSInterlinkingDashboardDTO maspage)
        {
            return COMMM.POSTData(maspage, "IVRMandVMSInterlinkingDashboardFacade/getEmployeedetailsQForm/");
        }

        public IVRMandVMSInterlinkingDashboardDTO SearchUdetails(IVRMandVMSInterlinkingDashboardDTO maspage)
        {
            return COMMM.POSTData(maspage, "IVRMandVMSInterlinkingDashboardFacade/SearchUdetails/");
        }

        public IVRMandVMSInterlinkingDashboardDTO SearchFReport(IVRMandVMSInterlinkingDashboardDTO maspage)
        {
            return COMMM.POSTData(maspage, "IVRMandVMSInterlinkingDashboardFacade/SearchFReport/");
        }

    }
}
