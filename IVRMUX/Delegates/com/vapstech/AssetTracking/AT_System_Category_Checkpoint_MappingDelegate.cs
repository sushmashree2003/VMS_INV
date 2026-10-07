using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_System_Category_Checkpoint_MappingDelegate
    {
        readonly CommonDelegate<AT_System_Category_Checkpoint_MappingDTO, AT_System_Category_Checkpoint_MappingDTO> _comm
            = new CommonDelegate<AT_System_Category_Checkpoint_MappingDTO, AT_System_Category_Checkpoint_MappingDTO>();

        readonly CommonDelegate<Health_Entry_CheckupDTO, Health_Entry_CheckupDTO> _healthcomm = new CommonDelegate<Health_Entry_CheckupDTO, Health_Entry_CheckupDTO>();

        public AT_System_Category_Checkpoint_MappingDTO GetDetails(AT_System_Category_Checkpoint_MappingDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_System_Category_Checkpoint_MappingFacade/getdetails/");
        }
 
        public AT_System_Category_Checkpoint_MappingDTO SaveRecord(AT_System_Category_Checkpoint_MappingDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_System_Category_Checkpoint_MappingFacade/saverecord");
        }

        public Health_Entry_CheckupDTO getProjectDetails(Health_Entry_CheckupDTO request)
        {
            //return _healthcomm.POSTDataAssetsTracking(request, "AT_System_Category_Checkpoint_MappingFacade/getHealthdetails/");
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/getdetails/");
        }

        public Health_Entry_CheckupDTO SaveHealthRecord(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/savedata");
        }

        public Health_Entry_CheckupDTO GetCheckpoints(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/getcheckpoints");
        }

        public Health_Entry_CheckupDTO FirstLevelApproval(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/firstLevelApproval");
        }

        public Health_Entry_CheckupDTO saveApproval(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/saveApproval");
        }

        public Health_Entry_CheckupDTO GetProjectSummaryReport(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/getProjectSummaryReport");
        }

        public Health_Entry_CheckupDTO getSignOffReportDetails(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/getSignOffReportDetails");
        }

        public Health_Entry_CheckupDTO getDetailedCheckupReport(Health_Entry_CheckupDTO request)
        {
            return _healthcomm.POSTDataAssetsTracking(request, "HealthCheckupEntryFacade/getDetailedCheckupReport");
        }
    }
}
