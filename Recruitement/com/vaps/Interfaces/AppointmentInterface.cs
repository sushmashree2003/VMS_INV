using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface AppointmentInterface
    {
        HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO editData(int id);
        HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO savesalarydata(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcandidate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getCandidateList(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcandidatename(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO getcompanydetails(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO savescopenature(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO deactivate1(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO SaveUpdateApproval(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO On_Desgination(HR_Candidate_DetailsDTO dto);

        HR_Candidate_DetailsDTO getfloordetail(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO SaveMapDetails(HR_Candidate_DetailsDTO data);

        HR_Candidate_DetailsDTO SaveFODetails(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO SaveHRDetails(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO getReport(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO SaveAppraisal(HR_Candidate_DetailsDTO data);
        HR_Candidate_DetailsDTO saveVerifyApproval(HR_Candidate_DetailsDTO data);

        //HR_Candidate_DetailsDTO SaVeAppointmentsHead(HR_Candidate_DetailsDTO data);



    }
}
