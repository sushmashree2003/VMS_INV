using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class MasterSystemDelegate
    {
       readonly CommonDelegate<MasterSystemDTO, MasterSystemDTO> _comm = new CommonDelegate<MasterSystemDTO, MasterSystemDTO>();


        public MasterSystemDTO GetDetails(MasterSystemDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_MasterSiteFacade/Getdetails");         
        }

        public MasterSystemDTO SaveDetail(MasterSystemDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_MasterSiteFacade/saverecord");
        }

        public MasterSystemDTO DeleteData(MasterSystemDTO request)
        {
            return _comm.POSTDataAssetsTracking(request, "AT_MasterSiteFacade/deactiveY");
        }
    }
}
