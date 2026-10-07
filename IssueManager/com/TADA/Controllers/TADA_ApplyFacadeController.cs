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
    [Route("api/TADA_ApplyFacade")]
    public class TADA_ApplyFacadeController : Controller
    {
        public TADA_ApplyInterface _objinter;        
        public TADA_ApplyFacadeController(TADA_ApplyInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_ApplyDTO getalldata([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("TadaAllwence")]
        public TADA_ApplyDTO TadaAllwence([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("savedta")]
        public TADA_ApplyDTO savedta([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.savedta(data);
        }
        //edit
        [Route("edit")]
        public TADA_ApplyDTO edit([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.edit(data);
        }
        //Viewdetails
        [Route("Viewdetails")]
        public TADA_ApplyDTO Viewdetails([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.Viewdetails(data);
        }
        //deactive
        [Route("deactive")]
        public TADA_ApplyDTO deactive([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.deactive(data);
        }

        //Getdata
        [Route("Getdata")]
        public TADA_ApplyDTO Getdata([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.Getdata(data);
        }
        //getstate
        [Route("getstate")]
        public TADA_ApplyDTO getstate([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.getstate(data);
        }
        //Updatefile
        [Route("Updatefile")]
        public TADA_ApplyDTO Updatefile([FromBody]TADA_ApplyDTO data)
        {
            return _objinter.Updatefile(data);
        }
    }
}