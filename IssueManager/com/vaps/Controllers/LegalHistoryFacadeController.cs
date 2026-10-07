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
    [Route("api/[controller]")]
    public class LegalHistoryFacadeController : Controller
    {
        public LegalHistoryInterface _Hist;
        // GET: api/<controller>
        public LegalHistoryFacadeController(LegalHistoryInterface para)
        {
            _Hist = para;
        }       


        [Route("saveDataHistory")]
        public LegalHistoryDTO saveDataHistory([FromBody]LegalHistoryDTO data)
        {
            return _Hist.saveDataHistory(data);
        }

        [Route("checkvalidation1")]
        public Task<LegalHistoryDTO> checkvalidation1([FromBody]LegalHistoryDTO data)
        {
            return _Hist.checkvalidation1(data);
        }

    }
}