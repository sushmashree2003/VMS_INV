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
    public class VMS_ChequePrinting_SettingFacade : Controller
    {
        public VMS_ChequePrinting_SettingInterface inter;
        public VMS_ChequePrinting_SettingFacade(VMS_ChequePrinting_SettingInterface s)
        {
            inter = s;
        }
        [Route("getloaddata")]
        public VMS_ChequePrinting_SettingDTO getloaddata([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            return inter.getloaddata(data);
        }
        [Route("savedetails")]
        public VMS_ChequePrinting_SettingDTO savedetails([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            return inter.savedetails(data);
        }
        [Route("deactive")]
        public VMS_ChequePrinting_SettingDTO deactive([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            return inter.deactive(data);
        }
        [Route("companychange")]
        public VMS_ChequePrinting_SettingDTO companychange([FromBody] VMS_ChequePrinting_SettingDTO data)
        {
            return inter.companychange(data);
        }
    }
}
