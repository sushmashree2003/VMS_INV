using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBookEntryDelegate
    {
        CommonDelegate<CheckBookEntryDTO, CheckBookEntryDTO> comm = new CommonDelegate<CheckBookEntryDTO, CheckBookEntryDTO>();

        public CheckBookEntryDTO loaddata(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/loaddata");
        }
        public CheckBookEntryDTO savedata(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/savedata");
        }
        //savedataSource
        public CheckBookEntryDTO savedataSource(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/savedataSource");
        }
        public CheckBookEntryDTO Editdata(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/Editdata");
        }
        public CheckBookEntryDTO clientDecative(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/clientDecative");
        }
        public CheckBookEntryDTO getProject(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/getProject");
        }
        public CheckBookEntryDTO getbom(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/getbom");
        }
        public CheckBookEntryDTO companychange(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/companychange");
        }
        public CheckBookEntryDTO viewdetails(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/viewdetails");
        }
        public CheckBookEntryDTO getinvoiceno(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/getinvoiceno");
        }
        public CheckBookEntryDTO savepaymentdetailsrecord(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/sms");
        }
        public CheckBookEntryDTO loaddataconfig(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/loaddataconfig");
        }
        public CheckBookEntryDTO savedataconfig(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/savedataconfig");
        }
        public CheckBookEntryDTO loaddatareport(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/loaddatareport");
        }
        public CheckBookEntryDTO getreport(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/getreport");
        }
        public CheckBookEntryDTO savedataEMI(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/savedataEMI");
        }
        //paymentnotification
        public CheckBookEntryDTO paymentnotification(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/paymentnotification");
        }
        public CheckBookEntryDTO viewuploadflies(CheckBookEntryDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookEntryFacade/viewuploadflies");
        }
    }
}
