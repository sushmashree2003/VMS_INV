using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMSApprovalRequestPageInterface
    {
        FMSApprovalRequestPageDTO loaddata(FMSApprovalRequestPageDTO data);
        FMSApprovalRequestPageDTO savedetails(FMSApprovalRequestPageDTO data);
        FMSApprovalRequestPageDTO editdetails(FMSApprovalRequestPageDTO data);
        FMSApprovalRequestPageDTO deactive(FMSApprovalRequestPageDTO data);
        FMSApprovalRequestPageDTO viewuploadflies(FMSApprovalRequestPageDTO data);
        FMSApprovalRequestPageDTO Updatedetails(FMSApprovalRequestPageDTO data);
    }
}
