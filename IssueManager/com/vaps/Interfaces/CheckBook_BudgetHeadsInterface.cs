using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface CheckBook_BudgetHeadsInterface
    {
        CheckBook_BudgetHeadsDTO getalldata(CheckBook_BudgetHeadsDTO data);

        CheckBook_BudgetHeadsDTO TadaAllwence(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO savedta(CheckBook_BudgetHeadsDTO data);
        //edit
        CheckBook_BudgetHeadsDTO edit(CheckBook_BudgetHeadsDTO data);
        //deactive
        CheckBook_BudgetHeadsDTO deactive(CheckBook_BudgetHeadsDTO data);
        //Getdata
        CheckBook_BudgetHeadsDTO Getdata(CheckBook_BudgetHeadsDTO data);
        //ApprovalData
        CheckBook_BudgetHeadsDTO ApprovalData(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO BudgetReport(CheckBook_BudgetHeadsDTO data);

    
    }
}
