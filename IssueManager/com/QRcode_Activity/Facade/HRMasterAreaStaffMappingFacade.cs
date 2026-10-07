using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.QRcode_Activity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.QRcode_Activity.Facade
{
    [Route("api/[controller]")]
    public class HRMasterAreaStaffMappingFacade : Controller
    {
        // GET: api/values
        HRMasterAreaStaffMappingInterface _Inv;

        public HRMasterAreaStaffMappingFacade(HRMasterAreaStaffMappingInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public HRMasterAreaStaffMappingDTO getloaddata([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getfloors")]
        public HRMasterAreaStaffMappingDTO getfloors([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getfloors(data);
        }

        [Route("getAreaName")]
        public HRMasterAreaStaffMappingDTO getAreaName([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getAreaName(data);
        }

        [Route("getActivityName")]
        public HRMasterAreaStaffMappingDTO getActivityName([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getActivityName(data);
        }
        [Route("getEmployeeName")]
        public HRMasterAreaStaffMappingDTO getEmployeeName([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getEmployeeName(data);
        }

        [Route("getUsername")]
        public HRMasterAreaStaffMappingDTO getUsername([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.getUsername(data);
        }

        [Route("AutoStaffMapping")]
        public HRMasterAreaStaffMappingDTO AutoStaffMapping([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.AutoStaffMapping(data);
        }

        [Route("savedetailsStaffMapping")]
        public HRMasterAreaStaffMappingDTO savedetailsStaffMapping([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.savedetailsStaffMapping(data);
        }

        [Route("deactive")]
        public HRMasterAreaStaffMappingDTO deactive([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.deactive(data);
        }

        [Route("edit")]
        public HRMasterAreaStaffMappingDTO edit([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            return _Inv.edit(data);
        }


    }
}
