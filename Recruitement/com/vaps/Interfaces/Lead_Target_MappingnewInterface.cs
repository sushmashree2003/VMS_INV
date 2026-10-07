using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Lead_Target_MappingnewInterface
    {
        Lead_Target_MappingnewDTO getBasicData(Lead_Target_MappingnewDTO dto);
        Lead_Target_MappingnewDTO get_desig(Lead_Target_MappingnewDTO dto);
        Lead_Target_MappingnewDTO get_employee(Lead_Target_MappingnewDTO dto);
        Lead_Target_MappingnewDTO getdetails(Lead_Target_MappingnewDTO dto);
        Lead_Target_MappingnewDTO getcatdetails(Lead_Target_MappingnewDTO dto);
        Lead_Target_MappingnewDTO SaveTarget(Lead_Target_MappingnewDTO dto);

        Lead_Target_MappingnewDTO Edit (Lead_Target_MappingnewDTO dto);

    }
}
