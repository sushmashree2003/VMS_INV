using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMSApprovalInterface
    {
        FMSApprovalDTO loaddata(FMSApprovalDTO data);
        FMSApprovalDTO getdatas(FMSApprovalDTO data);
        FMSApprovalDTO savedetails(FMSApprovalDTO data);
    }
}
