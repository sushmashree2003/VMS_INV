using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CheckBook_BudgetHeads : Controller
    {
        public CheckBook_BudgetHeadsInterface _objinter;


        public CheckBook_BudgetHeads(CheckBook_BudgetHeadsInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public CheckBook_BudgetHeadsDTO getalldata([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("TadaAllwence")]
        public CheckBook_BudgetHeadsDTO TadaAllwence([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("savedta")]
        public CheckBook_BudgetHeadsDTO savedta([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.savedta(data);
        }
        //ApprovalData
        [Route("ApprovalData")]
        public CheckBook_BudgetHeadsDTO ApprovalData([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.ApprovalData(data);
        }
        //edit
        [Route("edit")]
        public CheckBook_BudgetHeadsDTO edit([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.edit(data);
        }
        //deactive
        [Route("deactive")]
        public CheckBook_BudgetHeadsDTO deactive([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.deactive(data);
        }

        //Getdata
        [Route("Getdata")]
        public CheckBook_BudgetHeadsDTO Getdata([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.Getdata(data);
        }
        //BudgetReport
        [Route("BudgetReport")]
        public CheckBook_BudgetHeadsDTO BudgetReport([FromBody]CheckBook_BudgetHeadsDTO data)
        {
            return _objinter.BudgetReport(data);
        }
    }
}
