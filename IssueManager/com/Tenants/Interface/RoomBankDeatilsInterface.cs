using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface RoomBankDeatilsInterface
    {

        RoomBankDeatilsDTO getloaddata(RoomBankDeatilsDTO data);
        RoomBankDeatilsDTO savedetails(RoomBankDeatilsDTO data);
        RoomBankDeatilsDTO deactive(RoomBankDeatilsDTO data);

    }
}

