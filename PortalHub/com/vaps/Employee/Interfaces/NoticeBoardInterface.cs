using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface NoticeBoardInterface
    {
        IVRM_NoticeBoardDTO savedetail(IVRM_NoticeBoardDTO data);
        IVRM_NoticeBoardDTO Getdetails(IVRM_NoticeBoardDTO data);
        IVRM_NoticeBoardDTO deactivate(IVRM_NoticeBoardDTO data);
        IVRM_NoticeBoardDTO editdetails(IVRM_NoticeBoardDTO data);
        
    }
}
