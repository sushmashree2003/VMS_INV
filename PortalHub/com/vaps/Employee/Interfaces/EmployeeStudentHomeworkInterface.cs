using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface EmployeeStudentHomeworkInterface
    {
        IVRM_Homework_DTO savedetail(IVRM_Homework_DTO data);
        IVRM_Homework_DTO Getdetails(IVRM_Homework_DTO data);
        IVRM_Homework_DTO deactivate(IVRM_Homework_DTO data);
        Task<IVRM_Homework_DTO> get_classes(IVRM_Homework_DTO data);
        IVRM_Homework_DTO getsectiondata(IVRM_Homework_DTO data); 
         IVRM_Homework_DTO getsubject(IVRM_Homework_DTO data);
        IVRM_Homework_DTO editData(IVRM_Homework_DTO data);


    }
}
