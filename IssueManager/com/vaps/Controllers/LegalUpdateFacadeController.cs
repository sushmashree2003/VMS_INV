using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IssueManager.com.vaps.Controllers
{
    //[Produces("application/json")]
    //[Route("api/LegalUpdate")]

    [Route("api/[controller]")]
    public class LegalUpdateFacadeController : Controller
    {

        public LegalUpdateInterface _Update;
        // GET: api/<controller>
        public LegalUpdateFacadeController(LegalUpdateInterface para)
        {
            _Update = para;
        }

        //===////////////////////////////////////trans/////////////////////////////////////////////////////////

        [Route("loaddata")]
        public LegalUpdateDTO loaddata([FromBody]LegalUpdateDTO data)
        {
            return _Update.loaddata(data);
        }


        [Route("saveData1")]
        public LegalUpdateDTO saveData1([FromBody]LegalUpdateDTO data)
        {
            return _Update.saveData1(data);
        }

        [Route("validation")]
        public Task<LegalUpdateDTO> validation([FromBody]LegalUpdateDTO data)
        {
            return _Update.validation(data);
        }

    }
}