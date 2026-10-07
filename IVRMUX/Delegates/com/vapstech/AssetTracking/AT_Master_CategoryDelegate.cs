using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_Master_CategoryDelegate
    {
        readonly CommonDelegate<AT_Master_CategoryDTO, AT_Master_CategoryDTO> _comm = new CommonDelegate<AT_Master_CategoryDTO, AT_Master_CategoryDTO>();

        public AT_Master_CategoryDTO GetDetails(AT_Master_CategoryDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CategoryFacade/getdetails");
        }

        public AT_Master_CategoryDTO SaveDetail(AT_Master_CategoryDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CategoryFacade/saverecord");
        }

        public AT_Master_CategoryDTO DeactiveY(AT_Master_CategoryDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_Master_CategoryFacade/deactiveY");
        }
    }
}
