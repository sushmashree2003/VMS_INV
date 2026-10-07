using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface HW_HeathCheckUpInterface
    {
        HW_HeathCheckUpDTO OnLoadHWHealthCheckUp(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO SaveHWHealthChekUp(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO welclosesavedata(HW_HeathCheckUpDTO data);
        //added by praveenkuamr 
        HW_HeathCheckUpDTO ProposalSignOff(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO EditHealthCheckUp(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO ActiveDeactiveHealthCheckUp(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO DeleteHWHealthCheckUpFiles(HW_HeathCheckUpDTO data);

        //Approval Process
        HW_HeathCheckUpDTO LoadHWHealthCheckUpdataApproval(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO SaveHWHealthChekUpApproval(HW_HeathCheckUpDTO data);
        HW_HeathCheckUpDTO ViewHealthCheckUpComments(HW_HeathCheckUpDTO data);

        //health checkup report
        HW_HeathCheckUpDTO HealthCheckUpInstitution(HW_HeathCheckUpDTO data);
        Task<HW_HeathCheckUpDTO> getdetails(HW_HeathCheckUpDTO data);

        HW_HeathCheckUpDTO esculationmail(HW_HeathCheckUpDTO data);
    }
}
