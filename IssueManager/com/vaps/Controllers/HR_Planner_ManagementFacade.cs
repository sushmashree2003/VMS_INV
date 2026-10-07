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
    public class HR_Planner_ManagementFacade : Controller
    {
        public HR_Planner_ManagementInterface _objinter;

        public HR_Planner_ManagementFacade(HR_Planner_ManagementInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public HR_Planner_ManagementDTO getdetails([FromBody] HR_Planner_ManagementDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("getdetailsreminder")]
        public HR_Planner_ManagementDTO getdetailsreminder([FromBody] HR_Planner_ManagementDTO data)
        {
            return _objinter.getdetailsreminder(data);
        }

        [Route("savePlanner")]
        public HR_Planner_ManagementDTO savePlanner([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.savePlanner(data);
        }

        [Route("edit")]
        public HR_Planner_ManagementDTO edit([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.edit(data);
        }
        [Route("activedeactive")]
        public HR_Planner_ManagementDTO activedeactive([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.activedeactive(data);
        }
        [Route("DeleteYY")]
        public HR_Planner_ManagementDTO DeleteYY([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.DeleteYY(data);
        }

        [Route("viewuploadflies")]
        public HR_Planner_ManagementDTO viewuploadflies([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.viewuploadflies(data);
        }
        [Route("getempmailid")]
        public HR_Planner_ManagementDTO getempmailid([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.getempmailid(data);
        }
        [Route("deleteuploadfile")]
        public HR_Planner_ManagementDTO deleteuploadfile([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.deleteuploadfile(data);
        }

        [Route("rptmail")]
        public HR_Planner_ManagementDTO rptmail([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.rptmail(data);
        }


        [Route("generalreminderemailtemp")]
        public HR_Planner_ManagementDTO generalreminderemailtemp([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalreminderemailtemp(data);
        }

        [Route("generalreminderemail")]
        public HR_Planner_ManagementDTO generalreminderemail([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalreminderemail(data);
        }
        [Route("generalreminderemail_temp")]
        public HR_Planner_ManagementDTO generalreminderemail_temp([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalreminderemail_temp(data);
        }
        [Route("generalremindersms")]
        public HR_Planner_ManagementDTO generalremindersms([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalremindersms(data);
        }

        [Route("generalreminderrepeatEmail")]
        public HR_Planner_ManagementDTO generalreminderrepeatEmail([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalreminderrepeatEmail(data);
        }
        [Route("generalreminderrepeatsms")]
        public HR_Planner_ManagementDTO generalreminderrepeatsms([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.generalreminderrepeatsms(data);
        }
        [Route("clientDetails")]
        public HR_Planner_ManagementDTO clientDetails([FromBody]HR_Planner_ManagementDTO data)
        {
            return _objinter.clientDetails(data);
        }
    }
}
