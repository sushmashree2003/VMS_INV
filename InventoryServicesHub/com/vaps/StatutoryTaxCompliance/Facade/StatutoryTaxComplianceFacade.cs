using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.StatutoryTaxCompliance.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.StatutoryTaxCompliance.Facade
{
    [Route("api/[controller]")]
    public class StatutoryTaxComplianceFacade : Controller
    {
        public StatutoryTaxComplianceInterface _objinter;
        public StatutoryTaxComplianceFacade(StatutoryTaxComplianceInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getdetails")]
        public StatutoryTaxComplianceDTO getdetails([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("SaveTabOne")]
        public StatutoryTaxComplianceDTO getalldata([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabOne(data);
        }
        [Route("GetReference")]
        public StatutoryTaxComplianceDTO GetReference([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.GetReference(data);
        }
        [Route("Getincome")]
        public StatutoryTaxComplianceDTO Getincome([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.Getincome(data);
        }
        [Route("viewData")]
        public StatutoryTaxComplianceDTO viewData([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.viewData(data);
        }
        [Route("SaveTabtwo")]
        public StatutoryTaxComplianceDTO SaveTabtwo([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabtwo(data);
        }
        [Route("SaveTabthree")]
        public StatutoryTaxComplianceDTO SaveTabthree([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabthree(data);
        }
        [Route("SaveTabFour")]
        public StatutoryTaxComplianceDTO SaveTabFour([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabFour(data);
        }
        [Route("SaveTabFive")]
        public StatutoryTaxComplianceDTO SaveTabFive([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabFive(data);
        }
        [Route("SaveTabSix")]
        public StatutoryTaxComplianceDTO SaveTabSix([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabSix(data);
        }
        [Route("SaveTabSeven")]
        public StatutoryTaxComplianceDTO SaveTabSeven([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SaveTabSeven(data);
        }
        [Route("ViewFiles")]
        public StatutoryTaxComplianceDTO ViewFiles([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.ViewFiles(data);
        }
        [Route("Edit")]
        public StatutoryTaxComplianceDTO Edit([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.Edit(data);
        }
        [Route("Consultantreport")]
        public StatutoryTaxComplianceDTO Consultantreport([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.Consultantreport(data);
        }
        [Route("Casereports")]
        public StatutoryTaxComplianceDTO Casereports([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.Casereports(data);
        }
        [Route("ClosedCasereports")]
        public StatutoryTaxComplianceDTO ClosedCasereports([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.ClosedCasereports(data);
        }
        [Route("SendmailStatutory")]
        public StatutoryTaxComplianceDTO SendmailStatutory([FromBody]StatutoryTaxComplianceDTO data)
        {
            return _objinter.SendmailStatutory(data);
        }
    }
}
