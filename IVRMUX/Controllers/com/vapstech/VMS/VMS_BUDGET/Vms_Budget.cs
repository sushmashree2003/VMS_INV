using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using IVRMUX.Delegates.com.vapstech.VMS.VMS_BUDGET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.VMS_BUDGET
{
    [Route("api/[controller]")]
    public class Vms_Budget : Controller
    {
        Vms_BudgetDelagate _delegate = new Vms_BudgetDelagate();
        [HttpGet]
        [Route("getalldata1/{id:int}")]
        public CheckBook_BudgetHeadsDTO getalldata1(int id)
        {
            CheckBook_BudgetHeadsDTO dto = new CheckBook_BudgetHeadsDTO();
         
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.getalldata1(dto);


        }
        [Route("deactive1")]

        public CheckBook_BudgetHeadsDTO deactive1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.deactive1(data);
        }

        [Route("Savedata")]
        public CheckBook_BudgetHeadsDTO Savedata([FromBody] CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.Savedata(data);
        }


        [Route("Getdata1")]

        public CheckBook_BudgetHeadsDTO Getdata1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.Getdata1(data);
        }

        [Route("editBudget")]
        public CheckBook_BudgetHeadsDTO editBudget([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _delegate.editBudget(data);
        }
        [Route("Getdept")]

        public CheckBook_BudgetHeadsDTO Getdept([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.Getdept(data);
        }
        [Route("ApprovalData1")]
        public CheckBook_BudgetHeadsDTO ApprovalData1([FromBody]CheckBook_BudgetHeadsDTO data)
        {

            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.ApprovalData1(data);
        }
        [Route("edit1")]
        public CheckBook_BudgetHeadsDTO edit1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _delegate.edit1(data);
        }

        [HttpGet]
        [Route("BudgetHeadsReport1/{id:int}")]

        public CheckBook_BudgetHeadsDTO BudgetHeadsReport1(int id)
        {
            CheckBook_BudgetHeadsDTO data = new CheckBook_BudgetHeadsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.BudgetHeadsReport1(data);
        }
        [Route("GetdeptClient")]

        public CheckBook_BudgetHeadsDTO GetdeptClient([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetdeptClient(data);
        }
        [Route("GetReport")]

        public CheckBook_BudgetHeadsDTO GetReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetReport(data);
        }


        [Route("BudgetApprovalUpdate")]
        public CheckBook_BudgetHeadsDTO BudgetApprovalUpdate([FromBody] CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.BudgetApprovalUpdate(data);
        }
        [Route("GetApprovalReport")]
        public CheckBook_BudgetHeadsDTO GetApprovalReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            data.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetApprovalReport(data);
        }
    }
}
