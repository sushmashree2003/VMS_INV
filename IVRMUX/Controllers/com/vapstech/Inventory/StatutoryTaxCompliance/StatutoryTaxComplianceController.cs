using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.StatutoryTaxCompliance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Inventory.StatutoryTaxCompliance
{
    [Route("api/[controller]")]
    public class StatutoryTaxComplianceController : Controller
    {
        StatutoryTaxComplianceDelegate _delegate = new StatutoryTaxComplianceDelegate();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public StatutoryTaxComplianceDTO getdetails(int id)
        {
            StatutoryTaxComplianceDTO dto = new StatutoryTaxComplianceDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getdetails(dto);
        }
        [Route("GetReference")]
        public StatutoryTaxComplianceDTO GetReference([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.GetReference(data);
        }
        [Route("Getincome")]
        public StatutoryTaxComplianceDTO Getincome([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getincome(data);
        }
        [Route("viewData")]
        public StatutoryTaxComplianceDTO viewData([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.viewData(data);
        }
        [Route("SaveTabOne")]
        public StatutoryTaxComplianceDTO SaveTabOne([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabOne(data);
        }
        [Route("SaveTabtwo")]
        public StatutoryTaxComplianceDTO SaveTabtwo([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabtwo(data);
        }
        [Route("SaveTabThree")]
        public StatutoryTaxComplianceDTO SaveTabThree([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabThree(data);
        }
        [Route("SaveTabFour")]
        public StatutoryTaxComplianceDTO SaveTabFour([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabFour(data);
        }
        [Route("SaveTabFive")]
        public StatutoryTaxComplianceDTO SaveTabFive([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabFive(data);
        }
        [Route("SaveTabSix")]
        public StatutoryTaxComplianceDTO SaveTabSix([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabSix(data);
        }
        [Route("SaveTabSeven")]
        public StatutoryTaxComplianceDTO SaveTabSeven([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTabSeven(data);
        }
        [Route("ViewFiles")]
        public StatutoryTaxComplianceDTO ViewFiles([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.ViewFiles(data);
        }
        [Route("Edit")]
        public StatutoryTaxComplianceDTO Edit([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Edit(data);
        }
        [Route("Consultantreport")]
        public StatutoryTaxComplianceDTO Consultantreport([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Consultantreport(data);
        }
        [Route("Casereports")]
        public StatutoryTaxComplianceDTO Casereports([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Casereports(data);
        }
        [Route("ClosedCasereports")]
        public StatutoryTaxComplianceDTO ClosedCasereports([FromBody] StatutoryTaxComplianceDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.ClosedCasereports(data);
        }
    }
}
