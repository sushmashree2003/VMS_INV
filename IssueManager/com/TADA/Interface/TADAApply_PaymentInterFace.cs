using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public interface TADAApply_PaymentInterFace
    {
        TADAApply_PaymentDTO getalldata(TADAApply_PaymentDTO data);
        //savedta
        TADAApply_PaymentDTO savedta(TADAApply_PaymentDTO data);

        //deactive
        TADAApply_PaymentDTO deactive(TADAApply_PaymentDTO data);
        //edittt
        TADAApply_PaymentDTO edittt(TADAApply_PaymentDTO data);
        TADAApply_PaymentDTO AutoSchedule(TADAApply_PaymentDTO data);
        //  AutoSchedule
        TADAApply_PaymentDTO Viewdetails(TADAApply_PaymentDTO data);
    }
}
