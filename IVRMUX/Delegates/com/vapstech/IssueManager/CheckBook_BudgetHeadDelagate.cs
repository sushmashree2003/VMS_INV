using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBook_BudgetHeadDelagate
    {
        CommonDelegate<CheckBook_BudgetHeadsDTO, CheckBook_BudgetHeadsDTO> COMMI = new CommonDelegate<CheckBook_BudgetHeadsDTO, CheckBook_BudgetHeadsDTO>();
         public CheckBook_BudgetHeadsDTO getalldata(CheckBook_BudgetHeadsDTO id)
        {
            return COMMI.POSTIssueManager(id, "CheckBook_BudgetHeads/getalldata/");
        }
        public CheckBook_BudgetHeadsDTO TadaAllwence(CheckBook_BudgetHeadsDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "CheckBook_BudgetHeads/TadaAllwence");
        }
        public CheckBook_BudgetHeadsDTO savedta(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/savedta");
        }
        //ApprovalData
        public CheckBook_BudgetHeadsDTO ApprovalData(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/ApprovalData");
        }
        //edit
        public CheckBook_BudgetHeadsDTO edit(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/edit");
        }
        //deactive
        public CheckBook_BudgetHeadsDTO deactive(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/deactive");
        }
        //Getdata
        public CheckBook_BudgetHeadsDTO Getdata(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/Getdata");
        }
        //BudgetReport
        public CheckBook_BudgetHeadsDTO BudgetReport(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBook_BudgetHeads/BudgetReport");
        }
    }
}
