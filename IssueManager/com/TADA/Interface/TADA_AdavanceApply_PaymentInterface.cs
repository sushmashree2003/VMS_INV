using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public  interface TADA_AdavanceApply_PaymentInterface
    {
        TADA_AdavanceApply_PaymentDTO getalldata(TADA_AdavanceApply_PaymentDTO data);
        //savedta//getOnload
        TADA_AdavanceApply_PaymentDTO getOnload(TADA_AdavanceApply_PaymentDTO data);
        TADA_AdavanceApply_PaymentDTO savedta(TADA_AdavanceApply_PaymentDTO data);

        //deactive
        TADA_AdavanceApply_PaymentDTO deactive(TADA_AdavanceApply_PaymentDTO data);
        //Viewdetails
        TADA_AdavanceApply_PaymentDTO Viewdetails(TADA_AdavanceApply_PaymentDTO data);
        //edittt
        TADA_AdavanceApply_PaymentDTO edittt(TADA_AdavanceApply_PaymentDTO data);
        //savePayment
        TADA_AdavanceApply_PaymentDTO savePayment(TADA_AdavanceApply_PaymentDTO data);
    }
}
