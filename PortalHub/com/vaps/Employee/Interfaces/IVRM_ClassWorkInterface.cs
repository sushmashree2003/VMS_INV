using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface IVRM_ClassWorkInterface
    {
        IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data);
        Task<IVRM_ClassWorkDTO> get_classes(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data);
    }
}
