using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Training_Assignment_TopicIMPL:Interfaces.Training_Assignment_TopicInterface
    {
         public VMSContext _vmsconte;



        public Training_Assignment_TopicIMPL(VMSContext vmsContext)
        {
            _vmsconte = vmsContext;
        }
        public HR_Training_Assignment_Topic_DTO getdata_TAT(HR_Training_Assignment_Topic_DTO dto)
        {
            try
            {
                dto.training = _vmsconte.HR_Master_TrainingTopicDMO.Where(a => a.HRMTT_Id == dto.HRMTT_Id && a.HRMTT_ActiveFlg == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }
}
