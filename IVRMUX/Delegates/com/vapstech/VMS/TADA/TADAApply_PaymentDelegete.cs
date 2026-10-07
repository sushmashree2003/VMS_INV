using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADAApply_PaymentDelegete
    {
        CommonDelegate<TADAApply_PaymentDTO, TADAApply_PaymentDTO> COMMI = new CommonDelegate<TADAApply_PaymentDTO, TADAApply_PaymentDTO>();
        public TADAApply_PaymentDTO getalldata(TADAApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApply_PaymentFACADE/getalldata/");
        }
        //savedta
        public TADAApply_PaymentDTO savedta(TADAApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApply_PaymentFACADE/savedta/");
        }
        public TADAApply_PaymentDTO deactive(TADAApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApply_PaymentFACADE/deactive/");
        }
        //edittt
        public TADAApply_PaymentDTO edittt(TADAApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApply_PaymentFACADE/edittt/");
        }
        //Viewdetails
        public TADAApply_PaymentDTO Viewdetails(TADAApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADAApply_PaymentFACADE/Viewdetails/");
        }
    }
}
