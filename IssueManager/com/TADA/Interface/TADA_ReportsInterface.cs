using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public interface TADA_ReportsInterface
    {
        TADA_ApplyReportsDTO getalldata(TADA_ApplyReportsDTO data);

        TADA_ApplyReportsDTO Deptselectiondetails(TADA_ApplyReportsDTO data);
        TADA_ApplyReportsDTO Desgselectiondetails(TADA_ApplyReportsDTO data);
        TADA_ApplyReportsDTO GetReport(TADA_ApplyReportsDTO data);

        //Viewdetails
        TADA_ApplyReportsDTO Viewdetails(TADA_ApplyReportsDTO data);

    }
}
