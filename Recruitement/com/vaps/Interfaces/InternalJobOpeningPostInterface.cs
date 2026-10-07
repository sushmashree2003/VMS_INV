using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface InternalJobOpeningPostInterface
    {
        InternalJobOpeningPostDTO getalldetails(InternalJobOpeningPostDTO data);
    }
}
