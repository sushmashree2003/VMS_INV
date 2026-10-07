using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISMImportFacade : Controller
    {

        public ISMImportInterface _objinter;

        public ISMImportFacade(ISMImportInterface parameter)
        {
            _objinter = parameter;
        }

       

        [Route("Savedata")]
        public ISMImportDTO Savedata([FromBody]ISMImportDTO data)
        {
            return _objinter.Savedata(data);
        }
        [Route("saveIP")]
        public ISMImportDTO saveIP([FromBody]ISMImportDTO data)
        {
            return _objinter.saveIP(data);
        }
        [Route("saveadvance")]
        public ISMImportDTO saveadvance([FromBody]ISMImportDTO data)
        {
            return _objinter.saveadvance(data);
        }
        [Route("save_adv_planner_task")]
        public ISMImportDTO save_adv_planner_task([FromBody]ISMImportDTO data)
        {
            return _objinter.save_adv_planner_task(data);
        }


        [Route("OnloadApproval")]
        public ISMImportDTO OnloadApproval([FromBody] ISMImportDTO data)
        {
            return _objinter.OnloadApproval(data);
        }

        [Route("getyearplannerreport")]
        public ISMImportDTO getyearplannerreport([FromBody] ISMImportDTO data)
        {
            return _objinter.getyearplannerreport(data);
        }

        [Route("yearplannerapp")]
        public ISMImportDTO yearplannerapp([FromBody] ISMImportDTO data)
        {
            return _objinter.yearplannerapp(data);
        }

        [Route("department")]
        public ISMImportDTO department([FromBody] ISMImportDTO data)
        {
            return _objinter.department(data);
        }
        [Route("Depemplist")]
        public ISMImportDTO Depemplist([FromBody] ISMImportDTO data)
        {
            return _objinter.Depemplist(data);
        }

        [Route("Depemptasklist")]
        public ISMImportDTO Depemptasklist([FromBody] ISMImportDTO data)
        {
            return _objinter.Depemptasklist(data);
        }


        [Route("swaptask_list")]
        public ISMImportDTO swaptask_list([FromBody]ISMImportDTO data)
        {
            return _objinter.swaptask_list(data);
        }


        [Route("swaptask_list_save")]
        public ISMImportDTO swaptask_list_save([FromBody]ISMImportDTO data)
        {
            return _objinter.swaptask_list_save(data);
        }
    }
}
