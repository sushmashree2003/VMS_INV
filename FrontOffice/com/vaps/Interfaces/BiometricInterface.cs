using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
  public  interface BiometricInterface
    {
        FO_Emp_PunchDTO punchdata(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO Latedata(FO_Emp_PunchDTO id);
        FO_Emp_PunchDTO LateInAbs_Email(FO_Emp_PunchDTO id);
        FO_Emp_PunchDTO EarlyOut_Email(FO_Emp_PunchDTO id);
        FO_Emp_PunchDTO Earlydata(FO_Emp_PunchDTO id);
        FO_Biometric_VAPS_IEMapping_DTO vapsdata(FO_Biometric_VAPS_IEMapping_DTO data);
        FO_Emp_PunchDTO punchdata_vaps(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO RFCardpunchdata(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO AutoAbsent(FO_Emp_PunchDTO data);
        FO_Emp_PunchDTO BioMeteric_TemperatureData(FO_Emp_PunchDTO data);
        Task<FO_Emp_PunchDTO> kaleyra(FO_Emp_PunchDTO data);

    }
}
