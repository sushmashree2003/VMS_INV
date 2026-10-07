using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.StatutoryTaxCompliance.Interface
{
  public  interface StatutoryTaxComplianceInterface
    {
        StatutoryTaxComplianceDTO getdetails(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabOne(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO GetReference(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO Getincome(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO viewData(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabtwo(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabthree(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabFour(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabFive(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabSix(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SaveTabSeven(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO ViewFiles(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO Edit(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO Consultantreport(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO Casereports(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO ClosedCasereports(StatutoryTaxComplianceDTO data);
        StatutoryTaxComplianceDTO SendmailStatutory(StatutoryTaxComplianceDTO data);
    }
}
