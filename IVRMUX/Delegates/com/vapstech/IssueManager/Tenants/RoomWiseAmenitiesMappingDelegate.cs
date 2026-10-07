using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class RoomWiseAmenitiesMappingDelegate
    {
        CommonDelegate<RoomWiseAmenitiesMappingDTO, RoomWiseAmenitiesMappingDTO> _comm = new CommonDelegate<RoomWiseAmenitiesMappingDTO, RoomWiseAmenitiesMappingDTO>();




        public RoomWiseAmenitiesMappingDTO getloaddata(RoomWiseAmenitiesMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomWiseAmenitiesMappingFacade/getloaddata");
        }

        public RoomWiseAmenitiesMappingDTO savedetails(RoomWiseAmenitiesMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomWiseAmenitiesMappingFacade/savedata/");
        }
        public RoomWiseAmenitiesMappingDTO deactive(RoomWiseAmenitiesMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomWiseAmenitiesMappingFacade/deactive/");
        }
        public RoomWiseAmenitiesMappingDTO Edit(RoomWiseAmenitiesMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "RoomWiseAmenitiesMappingFacade/Edit/");
        }
    }
}
