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
    public class HRMasterAreaFacade : Controller
    {
        // GET: api/values
        HRMasterAreaInterface _Inv;

        public HRMasterAreaFacade(HRMasterAreaInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public HRMasterAreaDTO getloaddata([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.getloaddata(data);
        }

        
        [Route("getfloors")]
        public HRMasterAreaDTO getfloors([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.getfloors(data);
        }

        [Route("getAreaName")]
        public HRMasterAreaDTO getAreaName([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.getAreaName(data);
        }


        [Route("getActivityName")]
        public HRMasterAreaDTO getActivityName([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.getActivityName(data);
        }

        [Route("savedetailsAREA")]
        public HRMasterAreaDTO savedetailsAREA([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.savedetailsAREA(data);
        }

        [Route("savedetailsAREAActivity")]
        public HRMasterAreaDTO savedetailsAREAActivity([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.savedetailsAREAActivity(data);
        }

        [Route("savedetailsAREAActivityTime")]
        public HRMasterAreaDTO savedetailsAREAActivityTime([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.savedetailsAREAActivityTime(data);
        }

        [Route("deactiveArea")]
        public HRMasterAreaDTO deactiveArea([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.deactiveArea(data);
        }

        [Route("deactiveAreaActivity")]
        public HRMasterAreaDTO deactiveAreaActivity([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.deactiveAreaActivity(data);
        }

        [Route("deactiveAreaActivityTime")]
        public HRMasterAreaDTO deactiveAreaActivityTime([FromBody] HRMasterAreaDTO data)
        {
            return _Inv.deactiveAreaActivityTime(data);
        }
        [Route("FloorQRCODE")]
        public HRMasterAreaDTO FloorQRCODE([FromBody]HRMasterAreaDTO dto)
        {
            return _Inv.FloorQRCODE(dto);
        }
        [Route("edit")]
        public HRMasterAreaDTO edit([FromBody]HRMasterAreaDTO dto)
        {
            return _Inv.edit(dto);
        }
    }
}
