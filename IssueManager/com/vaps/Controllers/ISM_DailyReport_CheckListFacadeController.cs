using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Controllers
{

    [Route("api/[controller]")]
    public class ISM_DailyReport_CheckListFacadeController : Controller
    {
        public ISM_DailyReport_CheckListInterface _inter;
        public ISM_DailyReport_CheckListFacadeController(ISM_DailyReport_CheckListInterface inter)
        {
            _inter = inter;
        }
        [Route("loaddata")]
        public ISM_DailyReport_CheckListDTO loaddata([FromBody] ISM_DailyReport_CheckListDTO dto)
        {
            return _inter.loaddata(dto);
        }
        [Route("getdata")]
        public ISM_DailyReport_CheckListDTO getdata([FromBody] ISM_DailyReport_CheckListDTO dto)
        {
            return _inter.getdata(dto);
        }
    }
}