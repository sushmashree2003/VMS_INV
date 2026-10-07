using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface HRAluminiEmployeeBGVerificationInterface
    {
        HRAluminiEmployeeBGVerificationDTO getBasicData(HRAluminiEmployeeBGVerificationDTO dto);
        HRAluminiEmployeeBGVerificationDTO sendResignationMail(HRAluminiEmployeeBGVerificationDTO dto);
        HRAluminiEmployeeBGVerificationDTO savedata(HRAluminiEmployeeBGVerificationDTO dto);
        HRAluminiEmployeeBGVerificationDTO deactivate(HRAluminiEmployeeBGVerificationDTO dto);
        HRAluminiEmployeeBGVerificationDTO Editdata(HRAluminiEmployeeBGVerificationDTO data);
        ISM_Certificate_RequestDTO nocdetails(ISM_Certificate_RequestDTO data);
        ISM_Resignation_DTO FANDFSettlement (ISM_Resignation_DTO data);
    }



}
