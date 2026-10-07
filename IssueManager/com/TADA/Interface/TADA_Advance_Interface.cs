using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
  public  interface TADA_Advance_Interface
    {

        TADA_Advance_ApplyDTO getalldata(TADA_Advance_ApplyDTO data);

        TADA_Advance_ApplyDTO TadaAllwence(TADA_Advance_ApplyDTO data);
        TADA_Advance_ApplyDTO savedta(TADA_Advance_ApplyDTO data);
        TADA_Advance_ApplyDTO edit(TADA_Advance_ApplyDTO data);

        //deactive
        TADA_Advance_ApplyDTO deactive(TADA_Advance_ApplyDTO data);
        //getstate
        TADA_Advance_ApplyDTO getstate(TADA_Advance_ApplyDTO data);
    }
}
