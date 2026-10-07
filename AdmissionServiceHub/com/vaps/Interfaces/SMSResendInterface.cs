using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SMSResendInterface
    {
        SMSResendDTO Getdetails(SMSResendDTO mas);
        SMSResendDTO Gettransno(SMSResendDTO mas);
        SMSResendDTO showdata(SMSResendDTO mas);
        Task<SMSResendDTO> resendMsg(SMSResendDTO mas);


        
    }
}
