using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class HW_HeathCheckUpDelegate
    {
        CommonDelegate<HW_HeathCheckUpDTO, HW_HeathCheckUpDTO> _comm = new CommonDelegate<HW_HeathCheckUpDTO, HW_HeathCheckUpDTO>();

        public HW_HeathCheckUpDTO OnLoadHWHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/OnLoadHWHealthCheckUp");
        }
        public HW_HeathCheckUpDTO SaveHWHealthChekUp(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/SaveHWHealthChekUp");
        }
        public HW_HeathCheckUpDTO welclosesavedata(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/welclosesavedata");
        }

        //added by praveenkuamr Proposal Signoff
        public HW_HeathCheckUpDTO ProposalSignOff(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/ProposalSignOff");
        }
        public HW_HeathCheckUpDTO EditHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/EditHealthCheckUp");
        }
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/ViewHealthCheckUpFiles");
        }
        public HW_HeathCheckUpDTO ActiveDeactiveHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/ActiveDeactiveHealthCheckUp");
        }
        public HW_HeathCheckUpDTO DeleteHWHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/DeleteHWHealthCheckUpFiles");
        }

        //Approval Process
        public HW_HeathCheckUpDTO LoadHWHealthCheckUpdataApproval(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/LoadHWHealthCheckUpdataApproval");
        }
        public HW_HeathCheckUpDTO SaveHWHealthChekUpApproval(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/SaveHWHealthChekUpApproval");
        }
        public HW_HeathCheckUpDTO ViewHealthCheckUpComments(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/ViewHealthCheckUpComments");
        }

        //health check up report
        public HW_HeathCheckUpDTO HealthCheckUpInstitution(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/HealthCheckUpInstitution");
        }

        public HW_HeathCheckUpDTO getdetails(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/getdetails");
        }


        public HW_HeathCheckUpDTO esculationmail(HW_HeathCheckUpDTO data)
        {
            return _comm.POSTIssueManager(data, "HW_HeathCheckUpFacade/esculationmail");
        }
    }
}
