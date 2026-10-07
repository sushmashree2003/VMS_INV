using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ClientInvoiceDelegate
    {
        CommonDelegate<ClientInvoiceDTO, ClientInvoiceDTO> comm = new CommonDelegate<ClientInvoiceDTO, ClientInvoiceDTO>();

        public ClientInvoiceDTO loaddata(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/loaddata");
        }
        public ClientInvoiceDTO savedata(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/savedata");
        }
        public ClientInvoiceDTO Editdata(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/Editdata");
        }
        public ClientInvoiceDTO clientDecative(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/clientDecative");
        }
        public ClientInvoiceDTO getProject(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/getProject");
        }
        public ClientInvoiceDTO getbom(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/getbom");
        }
        public ClientInvoiceDTO companychange(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/companychange");
        }
        public ClientInvoiceDTO viewdetails(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/viewdetails");
        }
        public ClientInvoiceDTO getinvoiceno(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/getinvoiceno");
        }
        public ClientInvoiceDTO savepaymentdetailsrecord(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/savepaymentdetailsrecord");
        }
        public ClientInvoiceDTO loaddataconfig(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/loaddataconfig");
        }
        public ClientInvoiceDTO savedataconfig(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/savedataconfig");
        }
        public ClientInvoiceDTO loaddatareport(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/loaddatareport");
        }
        public ClientInvoiceDTO getreport(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/getreport");
        }
        public ClientInvoiceDTO paymentnotification(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/paymentnotification");
        }
        public ClientInvoiceDTO savedatadoc(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/savedatadoc");
        }
        public ClientInvoiceDTO Duplicatedoc(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/Duplicatedoc");
        }
        public ClientInvoiceDTO ViewFiles(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/ViewFiles");
        }
        public ClientInvoiceDTO ClientData(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/ClientData");
        }
        public ClientInvoiceDTO GetIEName(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/GetIEName");
        }
        public ClientInvoiceDTO savefeedbackdetails(ClientInvoiceDTO dto)
        {
            return comm.POSTIssueManager(dto, "ClientInvoiceFacade/savefeedbackdetails/");
        }
        public ClientInvoiceDTO Editclientfeedback(ClientInvoiceDTO dto)
        {
            return comm.POSTIssueManager(dto, "ClientInvoiceFacade/Editclientfeedback/");
        }
        public ClientInvoiceDTO uploaddeactivate(ClientInvoiceDTO data)
        {
            return comm.POSTIssueManager(data, "ClientInvoiceFacade/uploaddeactivate");
        }
    }
}
