using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.HRMS
{
    public class InternalJobOpeningPostDelegate
    {
        CommonDelegate<InternalJobOpeningPostDTO, InternalJobOpeningPostDTO> COMMM = new CommonDelegate<InternalJobOpeningPostDTO, InternalJobOpeningPostDTO>();

        public InternalJobOpeningPostDTO getalldetails(InternalJobOpeningPostDTO data)
        {
            return COMMM.POSTVMS(data, "InternalJobOpeningPostFacade/getalldetails");
        }
    }
}
