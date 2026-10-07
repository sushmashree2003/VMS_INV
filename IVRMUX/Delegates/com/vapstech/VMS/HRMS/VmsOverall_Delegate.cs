using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.HRMS
{
    public class VmsOverall_Delegate
    {

        CommonDelegate<VmsoverallDTO, VmsoverallDTO> COMMM = new CommonDelegate<VmsoverallDTO, VmsoverallDTO>();
        public VmsoverallDTO getalldetails(VmsoverallDTO dto)
        {
            return COMMM.POSTVMS(dto, "VmsOverall_ReportFacade/getalldetails");
        }
        public VmsoverallDTO getReport(VmsoverallDTO data)
        {
            return COMMM.POSTVMS(data, "VmsOverall_ReportFacade/getReport/");
        }
    }
}
