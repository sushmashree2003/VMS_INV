using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class COETriggerFacade : Controller
    {
        public COETriggerInterface _ads;

        public COETriggerFacade(COETriggerInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public COETriggerDTO getinitialdata([FromBody]COETriggerDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        [Route("sendsmsemail")]
        public COETriggerDTO sendsmsemail([FromBody]COETriggerDTO dto)
        {
            return _ads.sendsmsemail(dto);
        }
        [Route("getcoetemplate")]
        public COETriggerDTO get_state([FromBody]COETriggerDTO dto)
        {
            return _ads.getcoetemplate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public COETriggerDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        
        [Route("getrpt")]
        public COETriggerDTO getrpt([FromBody]COETriggerDTO dto)
        {
            return _ads.getrpt(dto);
        }
        [Route("getrpt_lead")]
        public COETriggerDTO getrpt_lead([FromBody]COETriggerDTO dto)
        {
            return _ads.getrpt_lead(dto);
        }

        [Route("loadtemplate")]
        public COETriggerDTO loadtemplate([FromBody]COETriggerDTO dto)
        {
            return _ads.loadtemplate(dto);
        }
        [Route("viewtemplatedetails")]
        public COETriggerDTO viewtemplatedetails([FromBody]COETriggerDTO dto)
        {
            return _ads.viewtemplatedetails(dto);
        }

    }
}
