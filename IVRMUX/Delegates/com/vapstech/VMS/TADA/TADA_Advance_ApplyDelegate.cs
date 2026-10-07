using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_Advance_ApplyDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TADA_Advance_ApplyDTO, TADA_Advance_ApplyDTO> COMMI = new CommonDelegate<TADA_Advance_ApplyDTO, TADA_Advance_ApplyDTO>();
        public TADA_Advance_ApplyDTO getalldata(TADA_Advance_ApplyDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_Advance_ApplyFacade/getalldata/");
        }
        //public TADA_Advance_ApplyDTO saverecord(TADA_Advance_ApplyDTO dTO)
        //{
        //    return COMMI.POSTIssueManager(dTO, "TADA_Advance_ApplyFacade/saverecord");
        //}
        public TADA_Advance_ApplyDTO TadaAllwence(TADA_Advance_ApplyDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_ApplyFacade/TadaAllwence");
        }
        public TADA_Advance_ApplyDTO savedta(TADA_Advance_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_ApplyFacade/savedta");
        }
        public TADA_Advance_ApplyDTO edit(TADA_Advance_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_ApplyFacade/edit");
        }
        //deactive
        public TADA_Advance_ApplyDTO deactive(TADA_Advance_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_ApplyFacade/deactive");
        }
        //getstate
        public TADA_Advance_ApplyDTO getstate(TADA_Advance_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_ApplyFacade/getstate");
        }
    }
}
