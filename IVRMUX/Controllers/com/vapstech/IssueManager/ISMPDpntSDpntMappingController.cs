using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

//For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISMPDpntSDpntMappingController : Controller
    {
        ISMPDpntSDpntMappingDelegate driver = new ISMPDpntSDpntMappingDelegate();

        [Route("loaddata/{id:int}")]
        public ISM_PDpnt_SDpnt_MappingDTO loaddata(int id)
        {
            ISM_PDpnt_SDpnt_MappingDTO data = new ISM_PDpnt_SDpnt_MappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.loaddata(data);
        }

        [Route("savedata")]
        public ISM_PDpnt_SDpnt_MappingDTO save([FromBody]ISM_PDpnt_SDpnt_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.save(data);
        }

        [Route("editdata/{id:int}")]
        public ISM_PDpnt_SDpnt_MappingDTO editdata(int id)
        {
            ISM_PDpnt_SDpnt_MappingDTO data = new ISM_PDpnt_SDpnt_MappingDTO();
            data.ISMPDSDMAP_Id = id;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return driver.editdata(data);
        }

        [Route("deletedata/{id:int}")]
        public ISM_PDpnt_SDpnt_MappingDTO deletedata(int id)
        {
            ISM_PDpnt_SDpnt_MappingDTO data = new ISM_PDpnt_SDpnt_MappingDTO();
            data.ISMPDSDMAP_Id = id;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return driver.deletedata(data);
        }

        [Route("loaddepartment/{id:int}")]
        public ISM_PDpnt_SDpnt_MappingDTO loaddepartment(int id)
        {
            ISM_PDpnt_SDpnt_MappingDTO data = new ISM_PDpnt_SDpnt_MappingDTO();
            data.ISMPDSDMAP_SMI_Id = id;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return driver.loaddepartment(data);
        }
    }
}
