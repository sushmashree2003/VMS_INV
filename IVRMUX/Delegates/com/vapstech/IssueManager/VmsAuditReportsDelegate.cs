using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class VmsAuditReportsDelegate
    {
        CommonDelegate<VmsAuditReportsDTO, VmsAuditReportsDTO> COMMI = new CommonDelegate<VmsAuditReportsDTO, VmsAuditReportsDTO>();

        public VmsAuditReportsDTO getalldata(VmsAuditReportsDTO id)
        {
            return COMMI.POSTIssueManager(id, "VmsAuditReportsFacade/getalldata/");
        }
        public VmsAuditReportsDTO TadaAllwence(VmsAuditReportsDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "VmsAuditReportsFacade/TadaAllwence");
        }
        public VmsAuditReportsDTO savedta(VmsAuditReportsDTO data)
        {
            return COMMI.POSTIssueManager(data, "VmsAuditReportsFacade/savedta");
        }
        //edit
        public VmsAuditReportsDTO edit(VmsAuditReportsDTO data)
        {
            return COMMI.POSTIssueManager(data, "VmsAuditReportsFacade/edit");
        }
        //deactive
        public VmsAuditReportsDTO deactive(VmsAuditReportsDTO data)
        {
            return COMMI.POSTIssueManager(data, "VmsAuditReportsFacade/deactive");
        }
        //Getdata
        public VmsAuditReportsDTO Getdata(VmsAuditReportsDTO data)
        {
            return COMMI.POSTIssueManager(data, "VmsAuditReportsFacade/Getdata");
        }
    }
}
