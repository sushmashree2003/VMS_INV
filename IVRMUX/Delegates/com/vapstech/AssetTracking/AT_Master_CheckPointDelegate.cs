using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_Master_CheckPointDelegate
    {
        readonly CommonDelegate<AT_Master_CheckPointDTO, AT_Master_CheckPointDTO> _comm = new CommonDelegate<AT_Master_CheckPointDTO, AT_Master_CheckPointDTO>();

        public AT_Master_CheckPointDTO GetDetails(AT_Master_CheckPointDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CheckPointFacade/Getdetails");
        }

        public AT_Master_CheckPointDTO SaveDetail(AT_Master_CheckPointDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CheckPointFacade/saverecord");
        }

        public AT_Master_CheckPointDTO DeactiveY(AT_Master_CheckPointDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CheckPointFacade/deactiveY");
        }
    }
}
