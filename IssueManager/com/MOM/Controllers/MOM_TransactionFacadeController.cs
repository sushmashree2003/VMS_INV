using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.MOM.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.MOM.Controllers
{
    [Route("api/[controller]")]
    public class MOM_TransactionFacadeController : Controller
    {
        public MOM_TransactionInterface _leave;
        public MOM_TransactionFacadeController(MOM_TransactionInterface _leavec)
        {
            _leave = _leavec;
        }

        [Route("getDetails")]
        public MOM_TransactionDTO getDetails([FromBody] MOM_TransactionDTO data)
        {
            return _leave.getDetails(data);
        }

        [Route("savedata")]
        public MOM_TransactionDTO savedata([FromBody] MOM_TransactionDTO data)
        {
            return _leave.savedata(data);
        }
    }
}