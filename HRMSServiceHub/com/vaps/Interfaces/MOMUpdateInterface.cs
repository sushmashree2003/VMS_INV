using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface MOMUpdateInterface
    {


        //===/////////////////////trans///////////////////////
        MOMUpdateDTO getonloaddata(MOMUpdateDTO data);
        MOMUpdateDTO savetransaction(MOMUpdateDTO data);
        MOMUpdateDTO viewflies(MOMUpdateDTO data);
        MOMUpdateDTO deactivate(MOMUpdateDTO data);
        MOMUpdateDTO Edittrans(MOMUpdateDTO data);
        MOMUpdateDTO oldmeetingdetail(MOMUpdateDTO data);

        ///update//////////////////////////////
        MOMUpdateDTO getDetails(MOMUpdateDTO data);
        MOMUpdateDTO UpdateDetails(MOMUpdateDTO data);
        MOMUpdateDTO Edit(MOMUpdateDTO data);
        MOMUpdateDTO viewuploadflies(MOMUpdateDTO data);
        MOMUpdateDTO saveData(MOMUpdateDTO data);
        Task<MOMUpdateDTO> checkvalidation(MOMUpdateDTO data);
        //Task<MOMUpdateDTO> save_excel_data(MOMUpdateDTO data);


        ///////////////////////////////////////////////////////////////
        MOMUpdateDTO getreport(MOMUpdateDTO data);
        MOMUpdateDTO showreport(MOMUpdateDTO data);
        MOMUpdateDTO viewdetails(MOMUpdateDTO data);
        //APPROVAL/////////////////////////////////////////////////////////
        MOMUpdateDTO getdata(MOMUpdateDTO data);
        MOMUpdateDTO approvaldata(MOMUpdateDTO data);
        MOMUpdateDTO getstatus(MOMUpdateDTO data);
        MOMUpdateDTO oldmeeting(MOMUpdateDTO data);
        MOMUpdateDTO sendapprovalMail(MOMUpdateDTO data);
        MOMUpdateDTO SaveVendors(MOMUpdateDTO data);
        MOMUpdateDTO onloadVendors(MOMUpdateDTO data);
    }
}
