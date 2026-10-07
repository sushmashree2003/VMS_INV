using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantUploadNoticeDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantUploadNoticeDTO, TenantUploadNoticeDTO> COMMM = new CommonDelegate<TenantUploadNoticeDTO, TenantUploadNoticeDTO>();
        public TenantUploadNoticeDTO savedetail(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/savedetail/");
        }
        public TenantUploadNoticeDTO Getdetails(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/Getdetails/");
        }
        public TenantUploadNoticeDTO deactivate(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/deactivate/");
        }

        public TenantUploadNoticeDTO editdetails(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/editdetails/");
        }

        public TenantUploadNoticeDTO get_noticelist(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/get_noticelist/");
        }
        public TenantUploadNoticeDTO getsection(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/getsection/");
        }
        public TenantUploadNoticeDTO getstudent(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/getstudent/");
        }
        public TenantUploadNoticeDTO Deptselectiondetails(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/Deptselectiondetails/");
        }
        public TenantUploadNoticeDTO Desgselectiondetails(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/Desgselectiondetails/");
        }
        public TenantUploadNoticeDTO viewData(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/viewData/");
        }
        public TenantUploadNoticeDTO viewrecords(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/viewrecords/");
        }
        public TenantUploadNoticeDTO sendmail(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/sendmail/");
        }
        public TenantUploadNoticeDTO gettenant(TenantUploadNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "TenantUploadNoticeFacade/gettenant/");
        }
    }
}
