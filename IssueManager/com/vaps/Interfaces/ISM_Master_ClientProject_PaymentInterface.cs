using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_Master_ClientProject_PaymentInterface
    {
        ISM_Master_ClientProject_PaymentDTO loaddata(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO savedata(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO Editdata(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO clientDecative(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO getProject(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO viewdetails(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO savepaymentdetailsrecord(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO loaddataconfig(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO savedataconfig(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO loaddatareport(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO getreport(ISM_Master_ClientProject_PaymentDTO data);
        ISM_Master_ClientProject_PaymentDTO paymentnotification(ISM_Master_ClientProject_PaymentDTO data);
        //ISM_Master_ClientProject_PaymentDTO getYear(ISM_Master_ClientProject_PaymentDTO data);
    }
}
