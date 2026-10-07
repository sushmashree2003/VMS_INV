using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class VMS_ReportDelegate
    {
        CommonDelegate<VMS_ReportDTO, VMS_ReportDTO> COMFRNT = new CommonDelegate<VMS_ReportDTO, VMS_ReportDTO>();

        public VMS_ReportDTO onloadgetdetails(VMS_ReportDTO dto)
        {
            return COMFRNT.POSTDataHRMS(dto, "VMS_ReportFacade/onloadgetdetails/");
        }

        public VMS_ReportDTO report(VMS_ReportDTO dto)
        {
            return COMFRNT.POSTDataHRMS(dto, "VMS_ReportFacade/report/");
        }

        public VMS_ReportDTO get_Employee(VMS_ReportDTO dto)
        {
            return COMFRNT.POSTDataHRMS(dto, "VMS_ReportFacade/get_Employee/");
        }

    }
}
