using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
   public interface VMSoverall_ReportInterface
    {

        VmsoverallDTO getalldetails(VmsoverallDTO data);

        VmsoverallDTO getReport(VmsoverallDTO data);
    }
}
