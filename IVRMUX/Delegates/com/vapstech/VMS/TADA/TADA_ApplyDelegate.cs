using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_ApplyDelegate
    {
        CommonDelegate<TADA_ApplyDTO, TADA_ApplyDTO> COMMI = new CommonDelegate<TADA_ApplyDTO, TADA_ApplyDTO>();

        public TADA_ApplyDTO getalldata(TADA_ApplyDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_ApplyFacade/getalldata/");
        }
        public TADA_ApplyDTO TadaAllwence(TADA_ApplyDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_ApplyFacade/TadaAllwence");
        }
        public TADA_ApplyDTO savedta(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/savedta");
        }
        //edit
        public TADA_ApplyDTO edit(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/edit");
        }
        //deactive
        public TADA_ApplyDTO deactive(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/deactive");
        }
        //Getdata
        public TADA_ApplyDTO Getdata(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/Getdata");
        }
        //Viewdetails
        public TADA_ApplyDTO Viewdetails(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/Viewdetails");
        }
        //getstate
        public TADA_ApplyDTO getstate(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/getstate");
        }
        //Updatefile
        public TADA_ApplyDTO Updatefile(TADA_ApplyDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_ApplyFacade/Updatefile");
        }
    }
}
