using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface CheckBookEntryApprovalInterface
    {
        // getalldata
        CheckBookEntryApprovalDTO getalldata(CheckBookEntryApprovalDTO data);

        CheckBookEntryApprovalDTO TadaAllwence(CheckBookEntryApprovalDTO data);
        CheckBookEntryApprovalDTO savedta(CheckBookEntryApprovalDTO data);
        //edit
        CheckBookEntryApprovalDTO edit(CheckBookEntryApprovalDTO data);
        //deactive
        CheckBookEntryApprovalDTO deactive(CheckBookEntryApprovalDTO data);
        //Getdata
        CheckApprovalDTO Getdata(CheckApprovalDTO data);
        //ApprovalData
        CheckBookEntryApprovalDTO ApprovalData(CheckBookEntryApprovalDTO data);
    }
}
