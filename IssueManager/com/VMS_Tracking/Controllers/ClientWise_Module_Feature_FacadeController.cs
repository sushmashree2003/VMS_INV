using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.VMS_Tracking.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.VMS_Tracking.Controllers
{

    [Route("api/[controller]")]
    public class ClientWise_Module_Feature_FacadeController : Controller
    {
        public ClientWise_Module_Feature_Inrerface _inter;
        public ClientWise_Module_Feature_FacadeController(ClientWise_Module_Feature_Inrerface inter)
        {
            _inter = inter;
        }
        [Route("getloaddata")]
        public ClientWise_Module_Feature_DTO getloaddata([FromBody] ClientWise_Module_Feature_DTO dto)
        {
            return _inter.getloaddata(dto);
        }
      
        [Route("getmodule")]
        public Task<ClientWise_Module_Feature_DTO> getmodule([FromBody] ClientWise_Module_Feature_DTO dto)
        {
            return _inter.getmoduleAsync(dto);
        }
        [Route("getreport")]
        public ClientWise_Module_Feature_DTO getreport([FromBody] ClientWise_Module_Feature_DTO dto)
        {
            return _inter.getreport(dto);
        }

    }
}