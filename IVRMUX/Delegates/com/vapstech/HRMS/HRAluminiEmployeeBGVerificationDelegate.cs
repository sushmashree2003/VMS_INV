using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class HRAluminiEmployeeBGVerificationDelegate
    {
        //private readonly object resource;
        //private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
            CommonDelegate<HRAluminiEmployeeBGVerificationDTO, HRAluminiEmployeeBGVerificationDTO> COMMM = new CommonDelegate<HRAluminiEmployeeBGVerificationDTO, HRAluminiEmployeeBGVerificationDTO>();

            public HRAluminiEmployeeBGVerificationDTO onloadgetdetails(HRAluminiEmployeeBGVerificationDTO dto)
            {
                return COMMM.POSTDataHRMS(dto, "HRAluminiEmolpyeeBGVFacade/onloadgetdetails");
            }

        public HRAluminiEmployeeBGVerificationDTO sendResignationMail(HRAluminiEmployeeBGVerificationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRAluminiEmolpyeeBGVFacade/sendResignationMail/");
        }

        public HRAluminiEmployeeBGVerificationDTO savedata(HRAluminiEmployeeBGVerificationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRAluminiEmolpyeeBGVFacade/");
        }
        public HRAluminiEmployeeBGVerificationDTO deactivate(HRAluminiEmployeeBGVerificationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRAluminiEmolpyeeBGVFacade/deactivate");
        }

        public HRAluminiEmployeeBGVerificationDTO Editdata(HRAluminiEmployeeBGVerificationDTO data)
        {
            return COMMM.POSTDataHRMS(data, "HRAluminiEmolpyeeBGVFacade/Editdata");
        }

    }
}
