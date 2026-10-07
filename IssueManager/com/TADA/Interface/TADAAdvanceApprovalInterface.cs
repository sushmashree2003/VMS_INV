using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
    public interface TADAAdvanceApprovalInterface
    {
        // getalldata
        TADAAdvanceApprovalDTO getalldata(TADAAdvanceApprovalDTO data);

        
        TADAAdvanceApprovalDTO savedta(TADAAdvanceApprovalDTO data);
        //edit
        TADAAdvanceApprovalDTO edit(TADAAdvanceApprovalDTO data);
        //editTour
        TADAAdvanceApprovalDTO editTour(TADAAdvanceApprovalDTO data);
        //deactive

        //Getdata //getTourApproval
        TADAAdvanceApprovalDTO getTourApproval(TADAAdvanceApprovalDTO data);
        //getView
        TADAAdvanceApprovalDTO getView(TADAAdvanceApprovalDTO data);
        //TadaAllwence
        TADAAdvanceApprovalDTO TadaAllwence(TADAAdvanceApprovalDTO data);
        //updateallowence
        TADAAdvanceApprovalDTO updateallowence(TADAAdvanceApprovalDTO data);
        //CalculateTada
        TADAAdvanceApprovalDTO CalculateTada(TADAAdvanceApprovalDTO data);
        //TourApproval
        TADAAdvanceApprovalDTO TourApproval(TADAAdvanceApprovalDTO data);
    }
}
