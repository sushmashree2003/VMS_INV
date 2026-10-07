using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.VMS_BUDGET.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.VMS_BUDGET.Controllers
{
    [Route("api/[controller]")]
    public class Vms_BudgetFacade : Controller
    {
        public Vms_BudgetInterface _objinter;


        public Vms_BudgetFacade(Vms_BudgetInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata1")]
        public CheckBook_BudgetHeadsDTO getalldata1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.getalldata1(data);
        }
        //deactive
        [Route("deactive1")]
        public CheckBook_BudgetHeadsDTO deactive1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.deactive1(data);
        }
        [Route("Savedata")]
        public CheckBook_BudgetHeadsDTO Savedata([FromBody] CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.Savedata(data);
        }
        [Route("Getdata1")]
        public CheckBook_BudgetHeadsDTO Getdata1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.Getdata1(data);
        }
        [Route("editBudget")]
        public CheckBook_BudgetHeadsDTO editBudget([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.editBudget(data);
        }
        [Route("Getdept")]
        public CheckBook_BudgetHeadsDTO Getdept([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.Getdept(data);
        }
        [Route("ApprovalData1")]
        public CheckBook_BudgetHeadsDTO ApprovalData1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.ApprovalData1(data);
        }


        //edit1
        [Route("edit1")]
        public CheckBook_BudgetHeadsDTO edit1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.edit1(data);
        }

        ////BudgetReport

        [Route("BudgetHeadsReport1")]
        public CheckBook_BudgetHeadsDTO BudgetHeadsReport1([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.BudgetHeadsReport1(data);
        }
        [Route("GetdeptClient")]
        public CheckBook_BudgetHeadsDTO GetdeptClient([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.GetdeptClient(data);
        }
        [Route("GetReport")]
        public CheckBook_BudgetHeadsDTO GetReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.GetReport(data);
        }


        [Route("BudgetApprovalUpdate")]
        public CheckBook_BudgetHeadsDTO BudgetApprovalUpdate([FromBody] CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.BudgetApprovalUpdate(data);
        }



        [Route("GetApprovalReport")]
        public CheckBook_BudgetHeadsDTO GetApprovalReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.GetApprovalReport(data);
        }
    }
}
