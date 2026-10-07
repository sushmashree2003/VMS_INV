using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.VMS_BUDGET
{
    public class Vms_BudgetDelagate
    {
        CommonDelegate<CheckBook_BudgetHeadsDTO, CheckBook_BudgetHeadsDTO> COMMI = new CommonDelegate<CheckBook_BudgetHeadsDTO, CheckBook_BudgetHeadsDTO>();
        public CheckBook_BudgetHeadsDTO getalldata1(CheckBook_BudgetHeadsDTO id)
        {
            return COMMI.POSTIssueManager(id, "Vms_BudgetFacade/getalldata1/");
        }

        public CheckBook_BudgetHeadsDTO deactive1(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/deactive1");
        }
        public CheckBook_BudgetHeadsDTO Savedata(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/Savedata");
        }
        public CheckBook_BudgetHeadsDTO Getdata1(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/Getdata1");
        }
        public CheckBook_BudgetHeadsDTO editBudget(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/editBudget");
        }

        public CheckBook_BudgetHeadsDTO Getdept(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/Getdept");
        }
        public CheckBook_BudgetHeadsDTO ApprovalData1(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/ApprovalData1");
        }
        public CheckBook_BudgetHeadsDTO edit1(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/edit1");
        }




        public CheckBook_BudgetHeadsDTO BudgetHeadsReport1(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/BudgetHeadsReport1");
        }

        public CheckBook_BudgetHeadsDTO GetReport(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/GetReport");
        }

        public CheckBook_BudgetHeadsDTO GetdeptClient(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/GetdeptClient");
        }


        public CheckBook_BudgetHeadsDTO BudgetApprovalUpdate(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/BudgetApprovalUpdate");
        }





        public CheckBook_BudgetHeadsDTO GetApprovalReport(CheckBook_BudgetHeadsDTO data)
        {
            return COMMI.POSTIssueManager(data, "Vms_BudgetFacade/GetApprovalReport");
        }
    }
}
