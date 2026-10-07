using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_AdavanceApply_PaymentDelegate
    {
     
        CommonDelegate<TADA_AdavanceApply_PaymentDTO, TADA_AdavanceApply_PaymentDTO> COMMI = new CommonDelegate<TADA_AdavanceApply_PaymentDTO, TADA_AdavanceApply_PaymentDTO>();
        public TADA_AdavanceApply_PaymentDTO getalldata(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/getalldata/");
        }
        //savedta //getOnload
        public TADA_AdavanceApply_PaymentDTO getOnload(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/getOnload/");
        }
        public TADA_AdavanceApply_PaymentDTO savedta(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/savedta/");
        }
        public TADA_AdavanceApply_PaymentDTO deactive(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/deactive/");
        }
        //Viewdetails
        public TADA_AdavanceApply_PaymentDTO Viewdetails(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/Viewdetails/");
        }
        //edittt
        public TADA_AdavanceApply_PaymentDTO edittt(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/edittt/");
        }
        //savePayment
        public TADA_AdavanceApply_PaymentDTO savePayment(TADA_AdavanceApply_PaymentDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_AdavanceApply_PaymentFacade/savePayment/");
        }
    }
}
