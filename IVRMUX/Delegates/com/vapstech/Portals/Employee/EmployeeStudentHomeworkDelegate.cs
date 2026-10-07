using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeStudentHomeworkDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO> COMMM = new CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO>();
        public IVRM_Homework_DTO savedetail(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/savedetail/");
        }

        public IVRM_Homework_DTO Getdetails(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/Getdetails/");
        }

        public IVRM_Homework_DTO deactivate(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/deactivate/");
        }

        public IVRM_Homework_DTO get_classes(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/get_classes/");
        }

        public IVRM_Homework_DTO getsectiondata(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/getsectiondata/");
        }

        public IVRM_Homework_DTO getsubject(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/getsubject/");
        }

        public IVRM_Homework_DTO editData(IVRM_Homework_DTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentHomeworkFacade/editData/");
        }
    }
}
