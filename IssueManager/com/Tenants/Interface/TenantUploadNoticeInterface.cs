using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantUploadNoticeInterface
    {
        TenantUploadNoticeDTO savedetail(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO Getdetails(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO deactivate(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO editdetails(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO getsection(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO getstudent(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO Deptselectiondetails(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO Desgselectiondetails(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO viewData(TenantUploadNoticeDTO data);

        TenantUploadNoticeDTO viewrecords(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO sendmail(TenantUploadNoticeDTO data);
        TenantUploadNoticeDTO gettenant(TenantUploadNoticeDTO data);
    }
}
