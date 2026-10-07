using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Master_ClientProject_PaymentDelegate
    {
        CommonDelegate<ISM_Master_ClientProject_PaymentDTO, ISM_Master_ClientProject_PaymentDTO> comm = new CommonDelegate<ISM_Master_ClientProject_PaymentDTO, ISM_Master_ClientProject_PaymentDTO>();

        public ISM_Master_ClientProject_PaymentDTO loaddata(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/loaddata");
        }
        public ISM_Master_ClientProject_PaymentDTO savedata(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/savedata");
        }
        public ISM_Master_ClientProject_PaymentDTO Editdata(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/Editdata");
        }
        public ISM_Master_ClientProject_PaymentDTO clientDecative(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/clientDecative");
        }
        public ISM_Master_ClientProject_PaymentDTO getProject(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/getProject");
        }
        public ISM_Master_ClientProject_PaymentDTO viewdetails(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/viewdetails");
        }
        public ISM_Master_ClientProject_PaymentDTO savepaymentdetailsrecord(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/savepaymentdetailsrecord");
        }
        public ISM_Master_ClientProject_PaymentDTO loaddataconfig(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/loaddataconfig");
        }
        public ISM_Master_ClientProject_PaymentDTO savedataconfig(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/savedataconfig");
        }
        public ISM_Master_ClientProject_PaymentDTO loaddatareport(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/loaddatareport");
        }
        public ISM_Master_ClientProject_PaymentDTO getreport(ISM_Master_ClientProject_PaymentDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/getreport");
        }
        //public ISM_Master_ClientProject_PaymentDTO getYear(ISM_Master_ClientProject_PaymentDTO data)
        //{
        //    return comm.POSTIssueManager(data, "ISM_Master_ClientProject_PaymentFacade/getYear");
        //}
    }
}
