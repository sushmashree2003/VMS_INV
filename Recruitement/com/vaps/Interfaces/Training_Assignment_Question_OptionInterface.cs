using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Training_Assignment_Question_OptionInterface
    {
        HR_Training_Assignment_Question_Option_DTO getdata_TAQO(HR_Training_Assignment_Question_Option_DTO dto);

        HR_Training_Assignment_Question_Option_DTO SaveEdit_TAQO(HR_Training_Assignment_Question_Option_DTO dto);
        HR_Training_Assignment_Question_Option_DTO deactivate_TAQO(HR_Training_Assignment_Question_Option_DTO id);
        HR_Training_Assignment_Question_Option_DTO details_TAQO(HR_Training_Assignment_Question_Option_DTO id);
        HR_Training_Assignment_Question_Option_DTO option_view_TAQO(HR_Training_Assignment_Question_Option_DTO id);
    }
}
