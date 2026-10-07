using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADAApprovalDelegate
    {
        CommonDelegate<TADAApprovalDTO, TADAApprovalDTO> COMMI = new CommonDelegate<TADAApprovalDTO, TADAApprovalDTO>();

        public TADAApprovalDTO getalldata(TADAApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApprovalFacade/getalldata/");
        }
        //getalldocument //getfinaldata
        public TADAApprovalDTO getfinaldata(TADAApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApprovalFacade/getfinaldata/");
        }
        public TADAApprovalDTO getalldocument(TADAApprovalDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApprovalFacade/getalldocument/");
        }
        public TADAApprovalDTO TadaAllwence(TADAApprovalDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADAApprovalFacade/TadaAllwence");
        }
        public TADAApprovalDTO savedta(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/savedta");
        }
        //savefinaldta
        public TADAApprovalDTO savefinaldta(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/savefinaldta");
        }
        public TADAApprovalDTO edit(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/edit");
        }
        public TADAApprovalDTO editTada(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/editTada");
        }
        //editTada
        public TADAApprovalDTO deactive(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/deactive");
        }
        //Getdata
        public TADAApprovalDTO getfile(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/getfile");
        }
        //GetSavedDetails
        public TADAApprovalDTO GetSavedDetails(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/GetSavedDetails");
        }
        //
        //getfileedit
        public TADAApprovalDTO getfileedit(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/getfileedit");
        }
        //Uploadfile
        public TADAApprovalDTO Uploadfile(TADAApprovalDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADAApprovalFacade/Uploadfile");
        }
    }
}
