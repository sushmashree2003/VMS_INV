using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Controllers
{
    [Produces("application/json")]
    [Route("api/TADA_Advance_ApplyFacade")]
    public class TADA_Advance_ApplyFacadeController : Controller
    {

        public TADA_Advance_Interface _objinter;

        public TADA_Advance_ApplyFacadeController(TADA_Advance_Interface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_Advance_ApplyDTO getalldata([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("TadaAllwence")]
        public TADA_Advance_ApplyDTO TadaAllwence([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("savedta")]
        public TADA_Advance_ApplyDTO savedta([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.savedta(data);
        }
        //edit
        [Route("edit")]
        public TADA_Advance_ApplyDTO edit([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.edit(data);
        }
        //deactive
        [Route("deactive")]
        public TADA_Advance_ApplyDTO deactive([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.deactive(data);
        }
        //getstate
        [Route("getstate")]
        public TADA_Advance_ApplyDTO getstate([FromBody]TADA_Advance_ApplyDTO data)
        {
            return _objinter.getstate(data);
        }
    }
}