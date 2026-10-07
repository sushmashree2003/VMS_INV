using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interface
{
    public interface VMS_SendNoticeInterface
    {
        VMS_SendNoticeDTO savedetail(VMS_SendNoticeDTO data);
        VMS_SendNoticeDTO Getdetails(VMS_SendNoticeDTO data);
        VMS_SendNoticeDTO deactivate(VMS_SendNoticeDTO data);
        VMS_SendNoticeDTO editdetails(VMS_SendNoticeDTO data);

        VMS_SendNoticeDTO viewData(VMS_SendNoticeDTO data);
        VMS_SendNoticeDTO gettenant(VMS_SendNoticeDTO data);
    }
}
