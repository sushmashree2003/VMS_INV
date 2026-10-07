using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBookPrintDelegate
    {
        CommonDelegate<CheckBookPrintDTO, CheckBookPrintDTO> comm = new CommonDelegate<CheckBookPrintDTO, CheckBookPrintDTO>();

        public CheckBookPrintDTO loaddata(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/loaddata");
        }
        public CheckBookPrintDTO savedata(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/savedata");
        }
        public CheckBookPrintDTO Editdata(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/Editdata");
        }
        public CheckBookPrintDTO clientDecative(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/clientDecative");
        }
        public CheckBookPrintDTO getProject(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/getProject");
        }
        public CheckBookPrintDTO getbom(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/getbom");
        }
        public CheckBookPrintDTO companychange(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/companychange");
        }
        public CheckBookPrintDTO viewdetails(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/viewdetails");
        }
        public CheckBookPrintDTO getinvoiceno(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/getinvoiceno");
        }
        public CheckBookPrintDTO savepaymentdetailsrecord(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/savepaymentdetailsrecord");
        }
        public CheckBookPrintDTO loaddataconfig(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/loaddataconfig");
        }
        public CheckBookPrintDTO savedataconfig(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/savedataconfig");
        }
        public CheckBookPrintDTO loaddatareport(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/loaddatareport");
        }
        public CheckBookPrintDTO getreport(CheckBookPrintDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookPrintFacade/getreport");
        }
        //public CheckBookPrintDTO getYear(CheckBookPrintDTO data)
        //{
        //    return comm.POSTIssueManager(data, "CheckBookPrintFacade/getYear");
        //}
    }
}
