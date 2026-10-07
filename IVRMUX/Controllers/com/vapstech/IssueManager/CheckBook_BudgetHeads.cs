using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class CheckBook_BudgetHeads : Controller
    {
        CheckBook_BudgetHeadDelagate _delegate = new CheckBook_BudgetHeadDelagate();
       
        
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public CheckBook_BudgetHeadsDTO getalldata(int id)
        {
            CheckBook_BudgetHeadsDTO dto = new CheckBook_BudgetHeadsDTO();
           // dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        [HttpPost]
        [Route("TadaAllwence")]
        public CheckBook_BudgetHeadsDTO TadaAllwence([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        [Route("savedta")]
        public CheckBook_BudgetHeadsDTO savedta([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        //ApprovalData
        [Route("ApprovalData")]
        public CheckBook_BudgetHeadsDTO ApprovalData([FromBody]CheckBook_BudgetHeadsDTO data)
        {

            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.ApprovalData(data);
        }
        [Route("edit")]
        public CheckBook_BudgetHeadsDTO edit([FromBody]CheckBook_BudgetHeadsDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delegate.edit(data);
        }
        //edit
        [Route("deactive")]

        public CheckBook_BudgetHeadsDTO deactive([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //Getdata
        [Route("Getdata")]

        public CheckBook_BudgetHeadsDTO Getdata([FromBody]CheckBook_BudgetHeadsDTO data)
        {
           // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getdata(data);
        }
        //BudgetReport
        [Route("BudgetReport")]

        public CheckBook_BudgetHeadsDTO BudgetReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.BudgetReport(data);
        }
    }
}
