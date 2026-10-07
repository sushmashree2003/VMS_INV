using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Training_Assignment_OptionInterface
    {
        HR_Training_Assignment_Option_DTO getdata_MFO(HR_Training_Assignment_Option_DTO dto);
        HR_Training_Assignment_Option_DTO SaveEdit_MFO(HR_Training_Assignment_Option_DTO dto);
        HR_Training_Assignment_Option_DTO deactivate_MFO(HR_Training_Assignment_Option_DTO id);
        HR_Training_Assignment_Option_DTO details_MFO(HR_Training_Assignment_Option_DTO id);

    }
}
