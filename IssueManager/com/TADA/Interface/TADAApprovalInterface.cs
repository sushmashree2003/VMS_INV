using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
    public interface TADAApprovalInterface
    {
        // getalldata
        TADAApprovalDTO getalldata(TADAApprovalDTO data);
        TADAApprovalDTO getfinaldata(TADAApprovalDTO data);
        
         TADAApprovalDTO savedta(TADAApprovalDTO data);
        //savefinaldta
        TADAApprovalDTO savefinaldta(TADAApprovalDTO data);
        TADAApprovalDTO edit(TADAApprovalDTO data);
        //editTada
        TADAApprovalDTO editTada(TADAApprovalDTO data);
        TADAApprovalDTO deactive(TADAApprovalDTO data);
        //Getdata
        TADAApprovalDTO getfile(TADAApprovalDTO data);
        TADAApprovalDTO GetSavedDetails(TADAApprovalDTO data);
        //
        TADAApprovalDTO getalldocument(TADAApprovalDTO data);
        //getfileedit
        TADAApprovalDTO getfileedit(TADAApprovalDTO data);
        //Uploadfile
        TADAApprovalDTO Uploadfile(TADAApprovalDTO data);
        //getfinaldata
       

    }
}
