using PreadmissionDTOs.com.vaps.HRMS;


namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface FF_Settlement_ProcessInterface
    {
        ////////////////////////////////////Request/////////////////////////////
        FF_Settlement_Process_DTO getalldetails(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO SaveUpdate(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO Edit(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO deactivate(FF_Settlement_Process_DTO dto);
        
     ///////////////////////////////////////////////Approval//////////////////////
        FF_Settlement_Process_DTO getData(FF_Settlement_Process_DTO dto);       
        FF_Settlement_Process_DTO empdetail(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO approvedrecord(FF_Settlement_Process_DTO dto);

        ///////////////////////////REPORT///////////////////////////////////////////
        FF_Settlement_Process_DTO Onload(FF_Settlement_Process_DTO dto);
        FF_Settlement_Process_DTO getapprovalreport(FF_Settlement_Process_DTO dto);
    }
}
