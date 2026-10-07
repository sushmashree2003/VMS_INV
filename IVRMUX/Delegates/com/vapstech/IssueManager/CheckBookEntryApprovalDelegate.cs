using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBookEntryApprovalDelegate
    {
        CommonDelegate<CheckBookEntryApprovalDTO, CheckBookEntryApprovalDTO> COMMI = new CommonDelegate<CheckBookEntryApprovalDTO, CheckBookEntryApprovalDTO>();
        //CheckApprovalDTO
        CommonDelegate<CheckApprovalDTO, CheckApprovalDTO> COMMIN = new CommonDelegate<CheckApprovalDTO, CheckApprovalDTO>();
        public CheckBookEntryApprovalDTO getalldata(CheckBookEntryApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "CheckBookEntryApprovalFacade/getalldata/");
        }
        public CheckBookEntryApprovalDTO TadaAllwence(CheckBookEntryApprovalDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "CheckBookEntryApprovalFacade/TadaAllwence");
        }
        public CheckBookEntryApprovalDTO savedta(CheckBookEntryApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBookEntryApprovalFacade/savedta");
        }
        //ApprovalData
        public CheckBookEntryApprovalDTO ApprovalData(CheckBookEntryApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBookEntryApprovalFacade/ApprovalData");
        }
        //edit
        public CheckBookEntryApprovalDTO edit(CheckBookEntryApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBookEntryApprovalFacade/edit");
        }
        //deactive
        public CheckBookEntryApprovalDTO deactive(CheckBookEntryApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "CheckBookEntryApprovalFacade/deactive");
        }
        //Getdata
        public CheckApprovalDTO Getdata(CheckApprovalDTO data)
        {
            return COMMIN.POSTIssueManager(data, "CheckBookEntryApprovalFacade/Getdata");
        }
    }
}
