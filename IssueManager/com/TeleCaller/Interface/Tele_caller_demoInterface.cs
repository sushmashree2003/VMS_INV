using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.VMS.TeleCaller;


namespace IssueManager.com.TeleCaller.Interfaces
{
  public  interface Tele_caller_demoInterface
    {
        TeleCallerDTO TeleCaller_List(TeleCallerDTO data);

        TeleCallerDTO UpdateTeleCaller_List(TeleCallerDTO data);


        
    }
}
