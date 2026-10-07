using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface AddJobVMSInterface
    {
        HR_MRFRequisitionDTO getBasicData(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO SaveUpdate(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO staterecord(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO deleterecord(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO editData(int id);
        HR_MRFRequisitionDTO deactivate(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO JobDisplay(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO saverecord(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO activedeactive(HR_MRFRequisitionDTO dto);

    }
}
