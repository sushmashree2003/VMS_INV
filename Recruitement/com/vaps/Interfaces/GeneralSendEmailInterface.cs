using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface GeneralSendEmailInterface
    {
        SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO dto);
        SalesSMSEMAILDTO onload(SalesSMSEMAILDTO dto);
        
        Task<SalesSMSEMAILDTO> sendsmsemailsales(SalesSMSEMAILDTO data);
    }
}
