using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
    public interface TADA_ApplyInterface
    {
        // getalldata
        TADA_ApplyDTO getalldata(TADA_ApplyDTO data);

        TADA_ApplyDTO TadaAllwence(TADA_ApplyDTO data);
        TADA_ApplyDTO savedta(TADA_ApplyDTO data);
        //edit
        TADA_ApplyDTO edit(TADA_ApplyDTO data);
        //deactive
        TADA_ApplyDTO deactive(TADA_ApplyDTO data);
        //Getdata
        TADA_ApplyDTO Getdata(TADA_ApplyDTO data);
        TADA_ApplyDTO Viewdetails(TADA_ApplyDTO data);
        //getstate
        TADA_ApplyDTO getstate(TADA_ApplyDTO data);
        //Updatefile
        TADA_ApplyDTO Updatefile(TADA_ApplyDTO data);
    }
}
