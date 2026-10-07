using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class VMS_SendNoticeDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<VMS_SendNoticeDTO, VMS_SendNoticeDTO> COMMM = new CommonDelegate<VMS_SendNoticeDTO, VMS_SendNoticeDTO>();
        public VMS_SendNoticeDTO savedetail(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/savedetail/");
        }
        public VMS_SendNoticeDTO Getdetails(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/Getdetails/");
        }
        public VMS_SendNoticeDTO deactivate(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/deactivate/");
        }

        public VMS_SendNoticeDTO editdetails(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/editdetails/");
        }

        public VMS_SendNoticeDTO get_noticelist(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/get_noticelist/");
        }
        public VMS_SendNoticeDTO getsection(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/getsection/");
        }
        public VMS_SendNoticeDTO getstudent(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/getstudent/");
        }
        public VMS_SendNoticeDTO Deptselectiondetails(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/Deptselectiondetails/");
        }
        public VMS_SendNoticeDTO Desgselectiondetails(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/Desgselectiondetails/");
        }
        public VMS_SendNoticeDTO viewData(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/viewData/");
        }
        public VMS_SendNoticeDTO viewrecords(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/viewrecords/");
        }
        //public VMS_SendNoticeDTO sendmail(VMS_SendNoticeDTO data)
        //{
        //    return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/sendmail/");
        //}
        public VMS_SendNoticeDTO gettenant(VMS_SendNoticeDTO data)
        {
            return COMMM.POSTIssueManager(data, "VMS_SendNoticeFacade/gettenant/");
        }
    }
}
