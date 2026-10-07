using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADAAdvanceApprovalDelegate
    {
        CommonDelegate<TADAAdvanceApprovalDTO, TADAAdvanceApprovalDTO> COMMI = new CommonDelegate<TADAAdvanceApprovalDTO, TADAAdvanceApprovalDTO>();

        public TADAAdvanceApprovalDTO getalldata(TADAAdvanceApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAAdvanceApprovalFacade/getalldata/");
        }
        //getTourApproval
        public TADAAdvanceApprovalDTO getTourApproval(TADAAdvanceApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAAdvanceApprovalFacade/getTourApproval/");
        }
        public TADAAdvanceApprovalDTO savedta(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/savedta");
        }
        //edit
        public TADAAdvanceApprovalDTO edit(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/edit");
        }
        //editTour
        public TADAAdvanceApprovalDTO editTour(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/editTour");
        }
        //getView
        public TADAAdvanceApprovalDTO getView(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/getView");
        }
        //TadaAllwence
        public TADAAdvanceApprovalDTO TadaAllwence(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/TadaAllwence");
        }
        //updateallowence
        public TADAAdvanceApprovalDTO updateallowence(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/updateallowence");
        }
        //CalculateTada
        public TADAAdvanceApprovalDTO CalculateTada(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/CalculateTada");
        }
        //TourApproval
        public TADAAdvanceApprovalDTO TourApproval(TADAAdvanceApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAAdvanceApprovalFacade/TourApproval");
        }
    }
}
