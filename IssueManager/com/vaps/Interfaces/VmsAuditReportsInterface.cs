using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface VmsAuditReportsInterface
    {
        VmsAuditReportsDTO getalldata(VmsAuditReportsDTO data);

        VmsAuditReportsDTO TadaAllwence(VmsAuditReportsDTO data);
        VmsAuditReportsDTO savedta(VmsAuditReportsDTO data);
        //edit
        VmsAuditReportsDTO edit(VmsAuditReportsDTO data);
        //deactive
        VmsAuditReportsDTO deactive(VmsAuditReportsDTO data);
        //Getdata
        VmsAuditReportsDTO Getdata(VmsAuditReportsDTO data);
    }
}
