

using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_MasterLocationDelegate
    {
        CommonDelegate<AT_MasterLocationDTO, AT_MasterLocationDTO> COMAT = new CommonDelegate<AT_MasterLocationDTO, AT_MasterLocationDTO>();

        CommonDelegate<EmpfloorMappingDTO, EmpfloorMappingDTO> COMATTT = new CommonDelegate<EmpfloorMappingDTO, EmpfloorMappingDTO>();
        public AT_MasterLocationDTO getloaddata(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/getloaddata/");
        }
        public AT_MasterLocationDTO savedetails(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/savedetails/");
        }
        public AT_MasterLocationDTO deactive(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/deactive/");
        }

        public EmpfloorMappingDTO savedetailEmpFloorMapping(EmpfloorMappingDTO data)
        {
            return COMATTT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/savedetailEmpFloorMapping/");
        }

    }
}
