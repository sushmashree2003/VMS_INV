using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface FF_SettlemetAdminApplyInterface
    {
        FF_Settlement_Process_DTO getBasicData(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO SaveUpdate(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO editData(int id);
        FF_Settlement_Process_DTO empdetails(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO deactivate(FF_Settlement_Process_DTO dto);

        FF_Settlement_Process_DTO getDetailsByEmployee(FF_Settlement_Process_DTO dto);

        FF_Settlement_Process_DTO searchfilter(FF_Settlement_Process_DTO dto);
   }
}
