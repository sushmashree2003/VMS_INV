using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.VMS_BUDGET.Interface
{
    public interface Vms_BudgetInterface
    {
        CheckBook_BudgetHeadsDTO getalldata1(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO deactive1(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO Savedata(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO Getdata1(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO editBudget(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO Getdept(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO ApprovalData1(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO edit1(CheckBook_BudgetHeadsDTO data);

        CheckBook_BudgetHeadsDTO BudgetHeadsReport1(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO GetReport(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO GetdeptClient(CheckBook_BudgetHeadsDTO data);
        CheckBook_BudgetHeadsDTO BudgetApprovalUpdate(CheckBook_BudgetHeadsDTO data);

        CheckBook_BudgetHeadsDTO GetApprovalReport(CheckBook_BudgetHeadsDTO data);

    }
}
